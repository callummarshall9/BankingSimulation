using System.Collections;

namespace BankingSimulation.UI.ViewModels.ImportData;

public class ImportDataViewModel
{
    public IEnumerable<ProviderViewModel> Providers { get; init; } = [];
    public string? SelectedProvider { get; set; }
    public string? CSVData { get; set; }
    public bool Loading { get; set; }
    public Exception? Exception { get; init; }
    public bool Success { get; init; }
}