using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.Nationwide.Services.Processing
{
    internal interface INationwideTransactionProcessingService
    {
        IEnumerable<Transaction> ParseTransactions(string rawData);
    }
}