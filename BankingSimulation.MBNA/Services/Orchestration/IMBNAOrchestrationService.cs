using System.Threading.Tasks;

namespace BankingSimulation.MBNA.Services.Orchestration
{
    public interface IMBNAOrchestrationService
    {
        Task CreateMBNASystem();
        Task ImportAccountsAsync();
        Task ImportTransactionsFromRawDataAsync(string rawData);
    }
}