using System.Threading.Tasks;

namespace BankingSimulation.Barclays
{
    public interface IBarclaysOrchestrationService
    {
        Task CreateBarclaysSystem();
        Task ImportAccountsFromRawDataAsync(string rawData);
        Task ImportTransactionsFromRawDataAsync(string rawData);
    }
}