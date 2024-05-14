using BankingSimulation.BarclaysCard.Brokers;
using BankingSimulation.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.BarclaysCard.Services.Processing
{
    internal class BarclaysCardTransactionProcessingService(ICSVBroker csvBroker) : IBarclaysCardTransactionProcessingService
    {
        public IEnumerable<Transaction> ParseTransactions(string rawData)
        {
            var rows = csvBroker.GetBarclaysTransactionEntries(rawData);

            return rows
                .Select(r => new Transaction
                {
                    Account = new Account { Name = $"Barclays Account {r.Cardholder}", Number = r.Cardholder + "-BarclaysCard" },
                    Balance = 0.0,
                    Value = double.Parse(string.IsNullOrEmpty(r.Amount) ? r.Amount2 : r.Amount),
                    Date = DateOnly.Parse(r.Date),
                    SourceSystemId = "BarclaysCard",
                    Description = r.Description,
                    TransactionType = new TransactionType { TypeId = string.IsNullOrEmpty(r.Type) ? "Default" : r.Type }
                })
                .ToArray();
        }
    }
}
