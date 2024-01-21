using System.Collections.Generic;
using BankingSimulation.Data;

namespace BankingSimulation.RBS;

internal interface IRBSAccountProcessingService
{
    IEnumerable<Account> ParseAccounts(string rawData);
}
