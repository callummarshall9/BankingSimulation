using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.BarclaysCard.Services.Processing
{
    internal interface IBarclaysCardTransactionProcessingService
    {
        IEnumerable<Transaction> ParseTransactions(string rawData);
    }
}