using BankingSimulation.Data;
using BankingSimulation.Nationwide.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.Nationwide.Services.Processing
{
    internal class NationwideTransactionProcessingService(ICSVBroker csvBroker) : INationwideTransactionProcessingService
    {
        public IEnumerable<Transaction> ParseTransactions(string rawData)
        {
            var rows = csvBroker.GetNationwideTransactionEntries(rawData);
            string accountName = csvBroker.GetAccountName(rawData);

            return rows
                .Select(r => new Transaction
                {
                    Account = new Account { Name = accountName, Number = accountName },
                    Balance = 0.0,
                    Value = double.Parse(string.IsNullOrEmpty(r.Amount1) ? r.Amount2 : r.Amount1),
                    Date = DateOnly.Parse(r.Date),
                    SourceSystemId = "Nationwide",
                    Description = r.Description,
                    TransactionType = new TransactionType { TypeId = r.TransactionType }
                })
                .ToArray();
        }
    }
}
