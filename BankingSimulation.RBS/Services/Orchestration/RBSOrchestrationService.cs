using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.RBS;

internal partial class RBSOrchestrationService : IRBSOrchestrationService
{
    private readonly IRBSAccountProcessingService accountProcessingService;
    private readonly IRBSTransactionProcessingService transactionProcessingService;
    private readonly IFoundationService foundationService;

    public RBSOrchestrationService(IRBSAccountProcessingService accountProcessingService, 
        IRBSTransactionProcessingService transactionProcessingService,
        IFoundationService foundationService)
    {
        this.accountProcessingService = accountProcessingService;
        this.transactionProcessingService = transactionProcessingService;
        this.foundationService = foundationService;
    }

    public async Task CreateRBSSystem()
    {
        if (!foundationService.GetAll<BankingSystem>().Any(bs => bs.Id == "RBS"))
            await foundationService.AddAsync(new BankingSystem { Id = "RBS" });
    }

    public async Task ImportAccountsFromRawDataAsync(string rawData)
    {
        await CreateRBSSystem();

        var parsedAccounts = accountProcessingService.ParseAccounts(rawData);

        var parsedAccountNumbers = parsedAccounts.Select(pa => pa.Number).ToArray();

        var existingAccounts = foundationService
            .GetAll<Account>()
            .Where(a => a.AccountSystemReferences.Any(asr => asr.BankingSystemId == "RBS") && parsedAccountNumbers.Contains(a.Number))
            .ToArray();

        var existingAccountNumbers = existingAccounts.Select(ea => ea.Number).ToList();

        var newAccounts = parsedAccounts.Where(pa => !existingAccountNumbers.Contains(pa.Number)).ToList();

        foreach(var entry in newAccounts)
        {
            var topLevelAccount = await foundationService.AddAsync(new Account { Name = entry.Name, Number = entry.Number });

            await foundationService.AddAsync(new AccountBankingSystemReference { BankingSystemId = "RBS", AccountId = topLevelAccount.Id });
        }

        foreach(var entry in existingAccounts)
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

        var accountNumbers = parsedTransactions.Select(pt => pt.Account.Number).Distinct().ToArray();

        var existingAccounts = foundationService.GetAll<Account>()
            .Where(a => accountNumbers.Contains(a.Number) && a.AccountSystemReferences.Any(asr => asr.BankingSystemId == "RBS"))
            .Select(a => new { a.Number, a.Id })
            .ToList();

        var existingAccountNumbers = existingAccounts
            .Select(a => a.Number)
            .Distinct();

        var missingAccountNumbers = accountNumbers
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
            .Where(tt => tt.SystemId == "RBS" && transactionTypes.Contains(tt.TypeId))
            .Select(tt => tt.TypeId)
            .ToArray();

        var missingTransactionTypes = transactionTypes
            .Where(tt => !existingTransactionTypes.Contains(tt))
            .ToArray();

        foreach(var missingTransactionType in missingTransactionTypes)
        {
            await foundationService.AddAsync(new TransactionType 
            {
                TypeId = missingTransactionType,
                SystemId = "RBS"
            });
        }

        var dbCategoryKeywords = foundationService.GetAll<CategoryKeyword>();

        var dbTransactionTypes = foundationService
            .GetAll<TransactionType>()
            .Where(tt => tt.SystemId == "RBS" && transactionTypes.Contains(tt.TypeId))
            .Select(tt => new { tt.TypeId, tt.Id })
            .ToArray();

        foreach(var transaction in parsedTransactions) 
        {
            transaction.CategoryId = dbCategoryKeywords
                .FirstOrDefault(ck => transaction.Description.Contains(ck.Keyword))?.CategoryId;

            var accountId = existingAccounts
                .Where(a => a.Number == transaction.Account.Number)
                .Select(a => a.Id)
                .First();

            await foundationService.AddAsync(new Transaction 
            {
                AccountId = accountId,
                TransactionTypeId = dbTransactionTypes.First(tt => tt.TypeId == transaction.TransactionType.TypeId).Id,
                Date = transaction.Date,
                Description = transaction.Description,
                Balance = transaction.Balance,
                Value = transaction.Value
            });
        }
    }
}