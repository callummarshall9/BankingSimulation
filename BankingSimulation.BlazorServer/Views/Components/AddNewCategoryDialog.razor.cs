using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.BlazorServer.ViewServices.Categories;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AddNewCategoryDialog : ComponentBase
{
    public AddNewCategoryViewModel Model { get; set; } = new();

    private Modal modal = null!;
    
    [Parameter]
    public EventCallback OnAdded { get; set; }
    
    public Task ShowAsync()
        => modal.ShowAsync();

    public Task HideAsync()
        => modal.HideAsync();
    
    [Inject]
    public IAddCategoryViewService AddCategoryViewService { get; set; } = null!;
    
    public async Task SubmitAsync()
    {
        Model.Loading = true;
        StateHasChanged();

        Model = await this.AddCategoryViewService.AddAsync(Model);
        StateHasChanged();
        
        if (Model.Success && OnAdded.HasDelegate)
            await OnAdded.InvokeAsync();
    }
}