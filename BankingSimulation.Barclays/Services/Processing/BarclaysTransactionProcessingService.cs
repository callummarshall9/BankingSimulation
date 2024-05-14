using BankingSimulation.Barclays.Brokers;
using BankingSimulation.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.Barclays.Services.Processing
{
    public class BarclaysTransactionProcessingService(ICSVBroker csvBroker) : IBarclaysTransactionProcessingService
    {
        public IEnumerable<Transaction> ParseTransactions(string rawData)
        {
            var rows = csvBroker.GetBarclaysTransactionEntries(rawData);

            return rows
                .Select(r => new Transaction
                {
                    Account = new Account { Name = $"Barclays Account {r.Account}", Number = r.Account },
                    Balance = 0.0,
                    Value = r.Amount,
                    Date = DateOnly.Parse(r.Date),
                    SourceSystemId = "Barclays",
                    Description = r.Memo,
                    TransactionType = new TransactionType { TypeId = r.Subcategory }
                })
                .ToArray();
        }
    }
}
