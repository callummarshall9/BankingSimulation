using System.Threading.Tasks;

namespace BankingSimulation.Nationwide.Services.Orchestration
{
    public interface INationwideOrchestrationService
    {
        Task CreateNationwideSystem();
        Task ImportAccountsFromRawDataAsync(string rawData);
        Task ImportTransactionsFromRawDataAsync(string rawData);
    }
}