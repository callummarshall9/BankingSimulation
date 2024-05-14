using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.Barclays.Services.Processing
{
    public interface IBarclaysTransactionProcessingService
    {
        IEnumerable<Transaction> ParseTransactions(string rawData);
    }
}