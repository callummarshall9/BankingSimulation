using System;
using System.Threading.Tasks;

namespace BankingSimulation.RBS;

public interface IRBSOrchestrationService
{
    Task ImportAccountsFromRawDataAsync(string rawData);
    Task ImportTransactionsFromRawDataAsync(string rawData);
}
