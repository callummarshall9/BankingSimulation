using BankingSimulation.BarclaysCard.Services.Processing;
using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSimulation.Services.Orchestration;

namespace BankingSimulation.BarclaysCard.Services.Orchestration;

internal partial class BarclaysCardOrchestrationService(
    IBarclaysCardAccountProcessingService accountProcessingService,
    IBarclaysCardTransactionProcessingService transactionProcessingService,
    IFoundationService foundationService,
    IAuthorisationBroker authorisationBroker) : IBarclaysCardOrchestrationService, 
        IAccountImportOrchestrationService,
        ITransactionImportOrchestrationService
{
    public async Task CreateBarclaysCardSystem()
    {
        if (!foundationService.GetAll<BankingSystem>().Any(bs => bs.Id == "BarclaysCard"))
            await foundationService.AddAsync(new BankingSystem { Id = "BarclaysCard" });
    }

    public async Task ImportAccountsFromRawDataAsync(string rawData)
    {
        await CreateBarclaysCardSystem();

        var parsedAccounts = accountProcessingService.ParseAccounts(rawData);

        var parsedAccountNumbers = parsedAccounts.Select(pa => pa.Number).ToArray();

        var existingAccounts = foundationService
            .GetAll<Account>()
            .Where(a => a.AccountSystemReferences.Any(asr => asr.BankingSystemId == "BarclaysCard") && parsedAccountNumbers.Contains(a.Number))
            .ToArray();

        var existingAccountNumbers = existingAccounts.Select(ea => ea.Number).ToList();

        var newAccounts = parsedAccounts.Where(pa => !existingAccountNumbers.Contains(pa.Number)).ToList();

        foreach (var entry in newAccounts)
        {
            var topLevelAccount = await foundationService.AddAsync(new Account { Name = entry.Name, Number = entry.Number });
            await foundationService.AddAsync(new AccountBankingSystemReference { BankingSystemId = "BarclaysCard", AccountId = topLevelAccount.Id });

            var accountRole = await foundationService.AddAsync(new Role
            {
                CreatedOn = DateTimeOffset.UtcNow,
                Name = $"{entry.Name} Role",
            });

            await foundationService.AddAsync(new AccountRole { RoleId = accountRole.Id, AccountId = topLevelAccount.Id });

            string userId = authorisationBroker.GetUserId();

            await foundationService.AddAsync(new UserRole { RoleId = accountRole.Id, UserId = userId });
        }

        foreach (var entry in existingAccounts)
        {
            var matchedParsedAccount = parsedAccounts.First(pa => pa.Number == entry.Number);

            if (matchedParsedAccount.Name != entry.Name)
            {
                entry.Name = matchedParsedAccount.Name;
                await foundationService.UpdateAsync(entry);
            }
        }
    }

    public async Task ImportTransactionsFromRawDataAsync(string rawData)
    {
        var parsedTransactions = transactionProcessingService.ParseTransactions(rawData);

        var receivedAccountNumbers = parsedTransactions.Select(pt => pt.Account.Number).Distinct().ToArray();

        var existingAccounts = foundationService.GetAll<Account>()
            .Where(a => receivedAccountNumbers.Contains(a.Number)
                && a.AccountSystemReferences.Any(asr => asr.BankingSystemId == "BarclaysCard")
            )
            .Select(a => new { a.Number, a.Id })
            .ToList()
            .Select(a => (a.Number, a.Id))
            .ToList();

        var existingAccountNumbers = existingAccounts
            .Select(a => a.Number)
            .Distinct();

        var missingAccountNumbers = receivedAccountNumbers
            .Where(an => !existingAccountNumbers.Contains(an))
            .ToArray();

        if (missingAccountNumbers.Any())
            throw new AccountNumberException(missingAccountNumbers);

        var transactionTypes = parsedTransactions
            .Select(pt => pt.TransactionType.TypeId)
            .Distinct()
            .ToArray();

        var existingTransactionTypes = foundationService
            .GetAll<TransactionType>()
            .Where(tt => tt.SystemId == "BarclaysCard" && transactionTypes.Contains(tt.TypeId))
            .Select(tt => tt.TypeId)
            .ToArray();

        var missingTransactionTypes = transactionTypes
            .Where(tt => !existingTransactionTypes.Contains(tt))
            .ToArray();

        foreach (var missingTransactionType in missingTransactionTypes)
        {
            await foundationService.AddAsync(new TransactionType
            {
                TypeId = missingTransactionType,
                SystemId = "BarclaysCard"
            });
        }

        var dbCategoryKeywords = foundationService.GetAll<CategoryKeyword>()
            .ToArray();

        var dbTransactionTypes = foundationService
            .GetAll<TransactionType>()
            .Where(tt => tt.SystemId == "BarclaysCard" && transactionTypes.Contains(tt.TypeId))
            .Select(tt => new { tt.TypeId, tt.Id })
            .ToArray()
            .Select(tt => (tt.TypeId, tt.Id))
            .ToList();

        var entriesForDate = parsedTransactions.GroupBy(pt => pt.Date)
            .OrderBy(g => g.Key);

        foreach (var group in entriesForDate)
        {
            var groupTransactions = group.ToArray()
                .OrderBy(gt => gt.Description)
                    .ThenBy(gt => gt.Value);

            var existingTransactionsForDate = foundationService
                .GetAll<Transaction>()
                .Where(t => t.Date == group.Key && receivedAccountNumbers.Contains(t.Account.Number) && t.SourceSystemId == "BarclaysCard")
                .Include(t => t.Account)
                .OrderBy(t => t.Description)
                    .ThenBy(t => t.Value)
                .ToArray();

            var receivedStack = new Stack<Transaction>(groupTransactions);
            var databaseStack = new Stack<Transaction>(existingTransactionsForDate);

            await HandleDifferences(
                receivedStack,
                databaseStack,
                (Transaction transaction) => ImportTransaction
                (
                    transaction,
                    existingAccounts,
                    dbCategoryKeywords,
                    dbTransactionTypes
                )
            );
        }
    }

    private async Task HandleDifferences(Stack<Transaction> receivedStack,
        Stack<Transaction> databaseStack,
        Func<Transaction, Task> action)
    {
        while (receivedStack.Count > 0)
        {
            if (databaseStack.Count == 0)
            {
                await action(receivedStack.Pop());
                continue;
            }

            var databaseItem = databaseStack.Peek();
            var receivedItem = receivedStack.Pop();

            if (databaseItem.Description != receivedItem.Description
                && databaseItem.Value != receivedItem.Value
                && databaseItem.Account.Number != receivedItem.Account.Number
                && databaseItem.SourceSystemId != receivedItem.SourceSystemId)
            {
                await action(receivedItem);
            }
            else
                databaseStack.Pop();
        };
    }

    private async Task ImportTransaction(Transaction receivedTransaction,
        List<(string Number, Guid Id)> existingAccounts,
        IEnumerable<CategoryKeyword> dbCategoryKeywords,
        IEnumerable<(string TypeId, Guid Id)> dbTransactionTypes)
    {
        var accountId = existingAccounts
            .Where(a => a.Number == receivedTransaction.Account.Number)
            .Select(a => a.Id)
            .First();

        var transaction = new Transaction
        {
            AccountId = accountId,
            TransactionTypeId = dbTransactionTypes
                .First(tt => tt.TypeId == receivedTransaction.TransactionType.TypeId).Id,
            CategoryId = dbCategoryKeywords
                .FirstOrDefault(ck => receivedTransaction.Description.Contains(ck.Keyword))?.CategoryId,
            SourceSystemId = "BarclaysCard",
            Date = receivedTransaction.Date,
            Description = receivedTransaction.Description,
            Balance = receivedTransaction.Balance,
            Value = receivedTransaction.Value
        };

        await foundationService.AddAsync(transaction);
    }
}