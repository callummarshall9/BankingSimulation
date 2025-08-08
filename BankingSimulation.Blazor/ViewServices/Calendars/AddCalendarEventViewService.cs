using BankingSimulation.Blazor.Models.Calendars;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public class AddCalendarEventViewService(ICalendarEventProcessingService calendarEventProcessingService) : IAddCalendarEventViewService
    {
        public Task AddCalendarEventAsync(AddCalendarEventViewModel addCalendarEventViewModel)
            => calendarEventProcessingService.AddAsync(new Data.Models.CalendarEvent
            {
                CalendarId = addCalendarEventViewModel.CalendarId,
                Name = addCalendarEventViewModel.Name,
                Start = addCalendarEventViewModel.Start,
                End = addCalendarEventViewModel.End
            });
    }
}
