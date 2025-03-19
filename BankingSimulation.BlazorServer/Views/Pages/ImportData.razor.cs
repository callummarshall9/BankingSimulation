using BankingSimulation.UI.ViewModels.ImportData;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class ImportData : ComponentBase
{
    [Inject]
    public IImportDataViewService ImportDataViewService { get; set; }

    public ImportDataViewModel Model { get; set; }

    protected override void OnInitialized()
    {
        Model = ImportDataViewService.Index();
    }

    public async Task ImportDataAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await ImportDataViewService.UploadAsync(Model);
        StateHasChanged();
    }

    public void HandleSelect(ChangeEventArgs e)
    {
        Model.SelectedProvider = e.Value.ToString();
    }
}