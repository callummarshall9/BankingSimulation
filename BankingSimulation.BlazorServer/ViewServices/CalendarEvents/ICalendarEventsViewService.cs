using BankingSimulation.UI.ViewModels.CalendarEvents;

namespace BankingSimulation.BlazorServer.ViewServices.CalendarEvents;

public interface ICalendarEventsViewService
{
    CalendarEventAggregateViewModel Index(Guid calendarId);
    Task<CalendarEventAggregateViewModel> DeleteCalendarEventAsync(CalendarEventViewModel calendarEvent, CalendarEventAggregateViewModel model);
}