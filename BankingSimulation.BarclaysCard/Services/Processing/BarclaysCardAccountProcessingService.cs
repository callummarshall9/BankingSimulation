using BankingSimulation.BarclaysCard.Brokers;
using BankingSimulation.Data;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.BarclaysCard.Services.Processing
{
    internal class BarclaysCardAccountProcessingService(ICSVBroker csvBroker) : IBarclaysCardAccountProcessingService
    {
        public IEnumerable<Account> ParseAccounts(string rawData)
        {
            var rows = csvBroker.GetBarclaysTransactionEntries(rawData);

            return rows
                .Where(r => !string.IsNullOrEmpty(r.Cardholder))
                .GroupBy(r => r.Cardholder)
                .Select(r => r.First())
                .Select(r => new Account
                {
                    Name = $"Barclays Card Account {r.Cardholder}",
                    Number = r.Cardholder + "-BarclaysCard",
                    AccountSystemReferences = [
                        new AccountBankingSystemReference() { BankingSystem = new BankingSystem { Id = "BarclaysCard", Description = "BarclaysCard" } }
                    ]
                })
                .ToArray();
        }
    }
}
