using BankingSimulation.BlazorServer.ViewModels.Roles;
using BankingSimulation.BlazorServer.Views.Components;
using BankingSimulation.BlazorServer.ViewServices.Roles;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class Roles : ComponentBase
{
    private AddNewRoleDialog addNewRoleDialog = null!;
    
    public RolesViewModel Model { get; set; } = new();
    
    [Inject]
    public IRolesViewService RolesViewService { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;
    
    private Task AddAsync()
        => addNewRoleDialog.ShowAsync();
    
    private async Task DeleteAsync(RoleViewModel role)
    {
        role.Deleting = true;
        StateHasChanged();

        Model = await RolesViewService.DeleteAsync(role, Model);
        StateHasChanged();
    }
}