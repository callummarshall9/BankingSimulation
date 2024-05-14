using System.Threading.Tasks;

namespace BankingSimulation.BarclaysCard.Services.Orchestration
{
    public interface IBarclaysCardOrchestrationService
    {
        Task CreateBarclaysCardSystem();
        Task ImportAccountsFromRawDataAsync(string rawData);
        Task ImportTransactionsFromRawDataAsync(string rawData);
    }
}