using BankingSimulation.Blazor.Models.Calendars;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public class EditCalendarViewService(ICalendarProcessingService calendarProcessingService,
        ICalendarEventProcessingService calendarEventProcessingService) : IEditCalendarViewService
    {
        public EditCalendarViewModel GetEditCalendarViewModel(Guid calendarId)
        {
            var result = calendarProcessingService.GetAll()
                .Where(c => c.Id == calendarId)
                .Select(c => new EditCalendarViewModel
                {
                    CalendarId = c.Id,
                    Name = c.Name,
                    RoleId = c.RoleId,
                    Events = c.CalendarEvents.Select(ce => new EditCalendarEventViewModel
                    {
                        Name = ce.Name,
                        Id = ce.Id,
                        Start = ce.Start,
                        End = ce.End
                    }).ToList()
                })
                .FirstOrDefault();

            if (result is null)
            {
                throw new InvalidOperationException($"Calendar with ID {calendarId} not found.");
            }

            return result;
        }

        public async Task<EditCalendarViewModel> UpdateCalendarAsync(EditCalendarViewModel editCalendarViewModel)
        {
            var updatedCalendar = await calendarProcessingService.UpdateAsync(new Data.Models.Calendar
            {
                Name = editCalendarViewModel.Name,
                Id = editCalendarViewModel.CalendarId,
                RoleId = editCalendarViewModel.RoleId
            });

            await MergeCalendarEventsAsync(editCalendarViewModel, updatedCalendar);

            return GetEditCalendarViewModel(editCalendarViewModel.CalendarId);
        }

        private async Task MergeCalendarEventsAsync(EditCalendarViewModel editCalendarViewModel, Data.Models.Calendar updatedCalendar)
        {
            var existingCalendarEvents = calendarEventProcessingService
                .GetAll()
                .Where(c => c.CalendarId == editCalendarViewModel.CalendarId)
                .ToList();

            foreach (var existingCalendarEvent in existingCalendarEvents)
            {
                var receivedEvent = editCalendarViewModel.Events.FirstOrDefault(ce =>
                    ce.Name == existingCalendarEvent.Name
                    && ce.Start == existingCalendarEvent.Start
                    && ce.End == existingCalendarEvent.End
                );

                if (receivedEvent == null)
                    await calendarEventProcessingService.DeleteAsync(existingCalendarEvent);
            }

            foreach (var incomingCalendarEvent in editCalendarViewModel.Events)
            {
                var existingEvent = existingCalendarEvents.FirstOrDefault(ce =>
                    ce.Name == incomingCalendarEvent.Name
                    && ce.Start == incomingCalendarEvent.Start
                    && ce.End == incomingCalendarEvent.End
                );

                if (existingEvent == null)
                    await calendarEventProcessingService.AddAsync(new Data.Models.CalendarEvent
                    {
                        CalendarId = updatedCalendar.Id,
                        Start = incomingCalendarEvent.Start,
                        End = incomingCalendarEvent.End,
                        Name = incomingCalendarEvent.Name
                    });
            }
        }
    }
}
