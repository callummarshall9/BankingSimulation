using BankingSimulation.Blazor.Models.Calendars;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public class CalendarViewService(ICalendarProcessingService calendarProcessingService) : ICalendarViewService
    {
        public IEnumerable<CalendarViewModel> GetCalendars()
        {
            return calendarProcessingService.GetAll()
                .Select(c => new CalendarViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Deleting = false,
                    EventUrl = "/calendars/" + c.Id
                }).ToList();
        }

        public async Task DeleteCalendar(Guid calendarId)
        {
            var calendar = calendarProcessingService.GetAll()
                .FirstOrDefault(c => c.Id == calendarId);

            if (calendar != null)
                await calendarProcessingService.DeleteAsync(calendar);
        }
    }
}
