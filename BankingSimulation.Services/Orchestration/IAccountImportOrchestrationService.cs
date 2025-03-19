namespace BankingSimulation.Services.Orchestration;

public interface IAccountImportOrchestrationService
{
    Task ImportAccountsFromRawDataAsync(string rawData);
}