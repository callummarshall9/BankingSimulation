using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.Nationwide.Services.Processing
{
    internal interface INationwideAccountProcessingService
    {
        IEnumerable<Account> ParseAccounts(string rawData);
    }
}