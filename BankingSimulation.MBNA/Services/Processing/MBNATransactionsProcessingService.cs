using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.MBNA.Brokers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BankingSimulation.MBNA.Services.Processing
{
    internal class MBNATransactionsProcessingService(ICSVBroker csvBroker, IAuthorisationBroker authorisationBroker) : IMBNATransactionsProcessingService
    {
        public IEnumerable<Transaction> ParseTransactions(string rawData)
        {
            var rows = csvBroker.GetMBNATransactionEntries(rawData);

            return rows
                .Select(r => new Transaction
                {
                    Account = new Account { Name = $"MBNA-" + authorisationBroker.GetUserId(), Number = $"MBNA-" + authorisationBroker.GetUserId() },
                    Balance = 0.0,
                    Value = r.Amount,
                    Date = DateOnly.Parse(r.Date),
                    SourceSystemId = "MBNA",
                    Description = r.Description,
                    TransactionType = new TransactionType { TypeId = "Default" }
                })
                .ToArray();
        }
    }
}
