using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.BlazorServer.Views.Components;
using BankingSimulation.BlazorServer.ViewServices.Categories;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class Categories : ComponentBase
{
    private AddNewCategoryDialog addNewCategoryDialog = null!;
    
    [Inject]
    public ICategoriesViewService CategoriesViewService { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    public CategoriesViewModel Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model = CategoriesViewService.Index();
    }
    
    private Task AddAsync()
        => addNewCategoryDialog.ShowAsync();

    private async Task OnSuccessfulAddAsync()
    {
        await addNewCategoryDialog.HideAsync();
        Model = CategoriesViewService.Index();
        StateHasChanged();
    }
    
    private async Task DeleteAsync(CategoryViewModel calendar)
    {
        calendar.Deleting = true;
        StateHasChanged();

        Model = await CategoriesViewService.DeleteAsync(calendar, Model);
        StateHasChanged();
    }
}