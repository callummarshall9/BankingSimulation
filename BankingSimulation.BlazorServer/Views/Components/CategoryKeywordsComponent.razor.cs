using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;
using BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class CategoryKeywordsComponent
{
    private AddNewCategoryKeywordDialog addNewCategoryKeywordDialog = null!;
    
    public CategoryKeywordAggregateViewModel Model { get; set; } = new();
    
    [Inject]
    public ICategoryKeywordsViewService CategoryKeywordsViewService { get; set; } = null!;

    [Parameter]
    public Guid CategoryId { get; set; }
    
    protected override void OnInitialized()
    {
        Model = CategoryKeywordsViewService.Index(CategoryId);
    }

    private async Task DeleteAsync(CategoryKeywordViewModel categoryKeyword)
    {
        categoryKeyword.Deleting = true;
        
        StateHasChanged();

        Model = await CategoryKeywordsViewService.DeleteAsync(categoryKeyword, Model);
        
        StateHasChanged();
    }
    
    private Task AddAsync()
        => addNewCategoryKeywordDialog.ShowAsync();
    
    private async Task OnSuccessfulAddAsync()
    {
        await addNewCategoryKeywordDialog.HideAsync();
        Model = CategoryKeywordsViewService.Index(CategoryId);
        StateHasChanged();
    }
}