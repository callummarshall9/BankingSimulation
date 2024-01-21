using System;
using System.Collections.Generic;
using System.Linq;
using BankingSimulation.Data;

namespace BankingSimulation.RBS;

internal class RBSAccountProcessingService : IRBSAccountProcessingService
{
    private readonly ICSVBroker csvBroker;

    public RBSAccountProcessingService(ICSVBroker csvBroker)
    {
        this.csvBroker = csvBroker;
    }

    public IEnumerable<Account> ParseAccounts(string rawData) 
    {
        var rows = csvBroker.GetRBSTransactionEntries(rawData);

        return rows.GroupBy(r => r.AccountNumber)
            .Select(r => r.First())
            .Select(r => new Account
            {
                Name = r.AccountName,
                Number = r.AccountNumber,
                AccountSystemReferences = [ 
                    new AccountBankingSystemReference() { BankingSystem = new BankingSystem { Id = "RBS", Description = "Royal Bank of Scotland." }} 
                ]
            })
            .ToArray();
    }
}
