using BankingSimulation.BlazorServer.ViewModels.Roles;
using BankingSimulation.BlazorServer.ViewServices.Roles;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AddNewRoleDialog : ComponentBase
{
    public AddNewRolesViewModel Model { get; set; } = new();

    private Modal modal = null!;
    
    [Parameter]
    public EventCallback OnAdded { get; set; }
    
    public Task ShowAsync()
        => modal.ShowAsync();

    public Task HideAsync()
        => modal.HideAsync();
    
    [Inject]
    public IAddRolesViewService AddRoleViewService { get; set; } = null!;
    
    public async Task SubmitAsync()
    {
        Model.Loading = true;
        StateHasChanged();

        Model = await AddRoleViewService.AddAsync(Model);
        StateHasChanged();
        
        if (Model.Success && OnAdded.HasDelegate)
            await OnAdded.InvokeAsync();
    }
}