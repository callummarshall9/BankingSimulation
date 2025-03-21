using BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;
using BankingSimulation.BlazorServer.ViewServices.CategoryKeywords;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AddNewCategoryKeywordDialog : ComponentBase
{
    private Modal modal = null!;
    
    [Parameter]
    public Guid CategoryId { get; set; }
    
    [Parameter]
    public EventCallback OnAdded { get; set; }

    public AddNewCategoryKeywordViewModel Model { get; set; } = new();
    
    public Task ShowAsync()
        => modal.ShowAsync();

    public Task HideAsync()
        => modal.HideAsync();
    
    [Inject]
    public IAddCategoryKeywordsViewService AddCategoryKeywordsViewService { get; set; } = null!;
    
    protected override void OnInitialized()
    {
        Model.CategoryId = CategoryId;
    }

    public async Task SubmitAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await AddCategoryKeywordsViewService.AddAsync(Model);
        StateHasChanged();

        if (Model.Success && OnAdded.HasDelegate)
            await OnAdded.InvokeAsync();
    }
}