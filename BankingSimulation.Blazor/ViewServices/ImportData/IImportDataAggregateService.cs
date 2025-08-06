
namespace BankingSimulation.Blazor.ViewServices.ImportData
{
    public interface IImportDataAggregateService
    {
        Task ImportAsync(string? option, string? rawData);
    }
}