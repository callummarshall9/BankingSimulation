using BankingSimulation.UI.ViewModels.Calendars;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class Calendars : ComponentBase
{
    public CalendarsViewModel Model { get; set; } = new();

    [Inject]
    public ICalendarViewService CalendarViewService { get; set; }

    protected override void OnInitialized()
    {
        Model = CalendarViewService.Index();
        StateHasChanged();
    }
}