using BankingSimulation.UI.ViewModels.ImportData;

namespace BankingSimulation.UI.ViewServices;

public interface IImportDataViewService
{
    ImportDataViewModel Index();
    Task<ImportDataViewModel> UploadAsync(ImportDataViewModel model);
}