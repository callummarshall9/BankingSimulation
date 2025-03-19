using BankingSimulation.Services.Orchestration;
using BankingSimulation.UI.ViewModels.ImportData;

namespace BankingSimulation.UI.ViewServices;

public class ImportDataViewService(IServiceProvider provider) : IImportDataViewService
{
    public ImportDataViewModel Index()
    {
        return new ImportDataViewModel
        {
            Providers = [
                new ProviderViewModel { Text = "RBS", Value = "rbs" },
                new ProviderViewModel { Text = "Barclays", Value = "barclays" },
                new ProviderViewModel { Text = "Barclays Card", Value = "barclaysCard" },
                new ProviderViewModel { Text = "MBNA", Value = "mbna" },
                new ProviderViewModel { Text = "Nationwide", Value = "nationwide" }
            ],
            Exception = null,
            SelectedProvider = null,
            Loading = false,
            CSVData = null,
            Success = false
        };
    }

    public async Task<ImportDataViewModel> UploadAsync(ImportDataViewModel model)
    {
        try
        {
            ArgumentException.ThrowIfNullOrEmpty(model.CSVData);

            string[] validProviders = ["rbs", "barclays", "barclaysCard", "mbna", "nationwide"];
            
            if (!validProviders.Contains(model.SelectedProvider))
                throw new ArgumentException("Invalid provider");

            var accountImporter = provider.GetKeyedServices<IAccountImportOrchestrationService>(model.SelectedProvider);

            foreach (var importer in accountImporter)
            {
                await importer.ImportAccountsFromRawDataAsync(model.CSVData);
            }

            var transactionImporter =
                provider.GetKeyedServices<ITransactionImportOrchestrationService>(model.SelectedProvider);

            foreach (var importer in transactionImporter)
            {
                await importer.ImportTransactionsFromRawDataAsync(model.CSVData);
            }

            return new ImportDataViewModel
            {
                Providers = model.Providers,
                Loading = false,
                CSVData = model.CSVData,
                SelectedProvider = model.SelectedProvider,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new ImportDataViewModel
            {
                Providers = model.Providers,
                Loading = false,
                CSVData = model.CSVData,
                Exception = ex,
                SelectedProvider = model.SelectedProvider,
                Success = false
            };
        }
        
    }
}