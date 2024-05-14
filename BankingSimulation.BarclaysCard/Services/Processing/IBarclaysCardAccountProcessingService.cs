using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.BarclaysCard.Services.Processing
{
    internal interface IBarclaysCardAccountProcessingService
    {
        IEnumerable<Account> ParseAccounts(string rawData);
    }
}