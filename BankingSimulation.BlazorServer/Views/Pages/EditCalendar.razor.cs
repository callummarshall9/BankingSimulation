using BankingSimulation.BlazorServer.Views.Components;
using BankingSimulation.UI.ViewModels.Calendars;
using BankingSimulation.UI.ViewServices;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;

namespace BankingSimulation.BlazorServer.Views.Pages;

public partial class EditCalendar : ComponentBase
{
    [Parameter]
    public Guid CalendarId { get; set; }

    [Inject] public IEditCalendarViewService EditCalendarViewService { get; set; } = null!;
    
    public EditCalendarViewModel Model { get; set; } = new();

    protected override void OnInitialized()
    {
        Model = EditCalendarViewService.Index(CalendarId);
    }

    public async Task UpdateAsync()
    {
        Model.Loading = true;
        StateHasChanged();
        
        Model = await EditCalendarViewService.UpdateCalendarAsync(Model);
    }
}