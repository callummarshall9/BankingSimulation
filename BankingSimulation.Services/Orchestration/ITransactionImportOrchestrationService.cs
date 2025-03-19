namespace BankingSimulation.Services.Orchestration;

public interface ITransactionImportOrchestrationService
{
    Task ImportTransactionsFromRawDataAsync(string rawData);
}