using BankingSimulation.Data;
using System.Collections.Generic;

namespace BankingSimulation.MBNA.Services.Processing
{
    internal interface IMBNAAccountProcessingService
    {
        IEnumerable<Account> ParseAccounts();
    }
}