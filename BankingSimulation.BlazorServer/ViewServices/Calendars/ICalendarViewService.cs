using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public interface ICalendarViewService
{
    CalendarsViewModel Index();
    Task<CalendarsViewModel> DeleteCalendarAsync(CalendarViewModel calendar, CalendarsViewModel model);
}