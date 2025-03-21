using BankingSimulation.UI.ViewModels.Calendars;
using BankingSimulation.UI.ViewServices;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AddNewCalendarDialog : ComponentBase
{
    [Inject]
    public IAddCalendarViewService AddCalendarViewService { get; set; } = null!;
    
    public AddCalendarViewModel Model { get; set; } = new ();
    
    [Parameter]
    public EventCallback OnAdded { get; set; }
    
    private Modal modal = null!;
    
    public Task ShowAsync()
        => this.modal.ShowAsync();

    public Task HideAsync()
        => this.modal.HideAsync();

    public async Task SubmitAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await AddCalendarViewService.AddCalendarAsync(Model);
        StateHasChanged();

        if (Model.Success && OnAdded.HasDelegate)
            await OnAdded.InvokeAsync();
    }
}