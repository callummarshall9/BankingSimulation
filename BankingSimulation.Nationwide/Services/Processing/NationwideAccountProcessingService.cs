using BankingSimulation.Data;
using BankingSimulation.Nationwide.Brokers;
using System.Collections.Generic;

namespace BankingSimulation.Nationwide.Services.Processing
{
    internal class NationwideAccountProcessingService(ICSVBroker csvBroker) : INationwideAccountProcessingService
    {
        public IEnumerable<Account> ParseAccounts(string rawData)
        {
            string accountName = csvBroker.GetAccountName(rawData);

            return [new Account
                {
                    Name = $"Nationwide {accountName}",
                    Number = accountName,
                    AccountSystemReferences = [
                        new AccountBankingSystemReference() { BankingSystem = new BankingSystem { Id = "Nationwide", Description = "Nationwide" } }
                    ]
                }];
        }
    }
}
