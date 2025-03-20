using BankingSimulation.UI.ViewModels.CalendarEvents;

namespace BankingSimulation.BlazorServer.ViewServices.CalendarEvents;

public interface IAddCalendarEventViewService
{
    Task<AddNewCalendarEventViewModel> AddCalendarEventAsync(AddNewCalendarEventViewModel calendarEvent);
}