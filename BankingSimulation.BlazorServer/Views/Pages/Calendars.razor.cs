using BankingSimulation.BlazorServer.Views.Components;
using BankingSimulation.UI.ViewModels.Calendars;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class Calendars : ComponentBase
{
    private AddNewCalendarDialog addNewCalendarDialog = null!;
    
    public CalendarsViewModel Model { get; set; } = new();

    [Inject] 
    public ICalendarViewService CalendarViewService { get; set; } = null!;
    
    [Inject]
    public NavigationManager NavigationManager { get; set; } = null!;

    protected override void OnInitialized()
    {
        Model = CalendarViewService.Index();
        StateHasChanged();
        
    }

    private Task AddAsync()
        => addNewCalendarDialog.ShowAsync();

    private async Task OnSuccessfulAddAsync()
    {
        await addNewCalendarDialog.HideAsync();
        Model = CalendarViewService.Index();
        StateHasChanged();
    }

    private async Task DeleteAsync(CalendarViewModel calendar)
    {
        calendar.Deleting = true;
        StateHasChanged();

        Model = await CalendarViewService.DeleteCalendarAsync(calendar, Model);
        StateHasChanged();
    }
}