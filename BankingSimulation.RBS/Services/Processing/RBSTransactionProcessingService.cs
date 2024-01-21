using System;
using System.Collections.Generic;
using System.Linq;
using BankingSimulation.Data;

namespace BankingSimulation.RBS;

internal class RBSTransactionProcessingService : IRBSTransactionProcessingService
{
    private readonly ICSVBroker csvBroker;

    public RBSTransactionProcessingService(ICSVBroker csvBroker)
    {
        this.csvBroker = csvBroker;
    }

    public IEnumerable<Transaction> ParseTransactions(string rawData)
    {
        var rows = csvBroker.GetRBSTransactionEntries(rawData);

        return rows.Select(r => new Transaction
        {
            Account = new Account { Name = r.AccountName, Number = r.AccountNumber },
            Balance = r.Balance,
            Value = r.Value,
            Date = DateTimeOffset.Parse(r.Date),
            Description = r.Description,
            TransactionType = new TransactionType { TypeId = r.Type }
        }).ToArray();
    }
}
