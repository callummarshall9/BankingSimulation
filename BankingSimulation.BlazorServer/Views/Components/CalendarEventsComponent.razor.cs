using BankingSimulation.BlazorServer.ViewServices.CalendarEvents;
using BankingSimulation.UI.ViewModels.CalendarEvents;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Components;

public partial class CalendarEventsComponent : ComponentBase
{
    private AddNewCalendarEventDialog addNewCalendarEventDialog = null!;

    public CalendarEventAggregateViewModel Model { get; set; } = new();
    
    [Inject]
    public ICalendarEventsViewService CalendarEventsViewService { get; set; }

    [Parameter]
    public Guid CalendarId { get; set; }
    
    protected override void OnInitialized()
    {
        Model = CalendarEventsViewService.Index(CalendarId);
    }

    private async Task DeleteAsync(CalendarEventViewModel calendarEvent)
    {
        calendarEvent.Deleting = true;
        
        StateHasChanged();

        Model = await CalendarEventsViewService.DeleteCalendarEventAsync(calendarEvent, Model);
        
        StateHasChanged();
    }
    
    private Task AddAsync()
        => addNewCalendarEventDialog.ShowAsync();

    private async Task OnSuccessfulAddAsync()
    {
        await addNewCalendarEventDialog.HideAsync();
        Model = CalendarEventsViewService.Index(CalendarId);
        StateHasChanged();
    }
}