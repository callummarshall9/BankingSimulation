using BankingSimulation.Barclays.Brokers;
using BankingSimulation.Data;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.Barclays.Services.Processing
{
    public class BarclaysAccountProcessingService(ICSVBroker csvBroker) : IBarclaysAccountProcessingService
    {
        public IEnumerable<Account> ParseAccounts(string rawData)
        {
            var rows = csvBroker.GetBarclaysTransactionEntries(rawData);

            return rows.GroupBy(r => r.Account)
                .Select(r => r.First())
                .Select(r => new Account
                {
                    Name = $"Barclays Account {r.Account}",
                    Number = r.Account,
                    AccountSystemReferences = [
                        new AccountBankingSystemReference() { BankingSystem = new BankingSystem { Id = "Barclays", Description = "Barclays" } }
                    ]
                })
                .ToArray();
        }
    }
}
