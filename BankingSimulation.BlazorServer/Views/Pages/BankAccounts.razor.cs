using BankingSimulation.UI.ViewModels;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class BankAccounts : ComponentBase
{
    [Inject]
    public IBankAccountsViewService BankAccountsViewService { get; set; }
    
    public Guid? SelectedCalendarId { get; set; }
    public BankAccountsIndexViewModel Model { get; set; } = new();
    
    protected override void OnInitialized()
    {
        Model = BankAccountsViewService.Index();
        StateHasChanged();
    }

    void HandleSelect(ChangeEventArgs e)
    {
        if (Guid.TryParse(e.Value.ToString(), out var calendarId))
        {
            SelectedCalendarId = calendarId;
        }
        else
            SelectedCalendarId = null;
        
        StateHasChanged();
    }
}