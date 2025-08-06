using BankingSimulation.Blazor.Models.Calendars;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public interface ICalendarViewService
    {
        IEnumerable<CalendarViewModel> GetCalendars();
        Task DeleteCalendar(Guid calendarId);
    }
}