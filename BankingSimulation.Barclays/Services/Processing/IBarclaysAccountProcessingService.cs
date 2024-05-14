using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.Barclays.Services.Processing
{
    public interface IBarclaysAccountProcessingService
    {
        IEnumerable<Account> ParseAccounts(string rawData);
    }
}