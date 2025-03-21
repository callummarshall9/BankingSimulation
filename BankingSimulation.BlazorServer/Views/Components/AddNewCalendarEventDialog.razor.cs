using BankingSimulation.BlazorServer.ViewServices.CalendarEvents;
using BankingSimulation.UI.ViewModels.CalendarEvents;
using BlazorBootstrap;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class AddNewCalendarEventDialog : ComponentBase
{
    private Modal modal = null!;

    public AddNewCalendarEventViewModel Model { get; set; } = new();
    
    [Parameter]
    public Guid CalendarId { get; set; }
    
    [Parameter]
    public EventCallback OnAdded { get; set; }
    
    [Inject]
    public IAddCalendarEventViewService AddCalendarEventViewService { get; set; } = null!;
    
    public Task ShowAsync()
        => modal.ShowAsync();

    public Task HideAsync()
        => modal.HideAsync();

    protected override void OnInitialized()
    {
        Model.CalendarId = CalendarId;
    }

    public async Task SubmitAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await AddCalendarEventViewService.AddCalendarEventAsync(Model);
        StateHasChanged();

        if (Model.Success && OnAdded.HasDelegate)
            await OnAdded.InvokeAsync();
    }
}