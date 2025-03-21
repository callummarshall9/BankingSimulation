using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.BlazorServer.ViewServices.Categories;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class EditCategory : ComponentBase
{
    [Parameter]
    public Guid CategoryId { get; set; }
    
    public EditCategoryViewModel Model { get; set; } = new();

    [Inject] 
    public IEditCategoryViewService EditCategoryViewService { get; set; } = null!;

    protected override void OnInitialized()
    {
        Model = EditCategoryViewService.Index(CategoryId);
    }

    public async Task UpdateAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await EditCategoryViewService.UpdateAsync(Model);
        StateHasChanged();
    }
}