using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.MBNA.Services.Processing
{
    internal interface IMBNATransactionsProcessingService
    {
        IEnumerable<Transaction> ParseTransactions(string rawData);
    }
}