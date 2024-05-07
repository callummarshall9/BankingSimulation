using BankingSimulation.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CalendarEventProcessingService(IFoundationService foundationService) : ICalendarEventProcessingService
    {
        public async Task<CalendarEvent> AddAsync(CalendarEvent item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Access Denied!");

            if (item.End < item.Start)
                throw new ValidationException("Start cannot be before end");

            bool canSeeCalendar = foundationService.GetAll<Calendar>()
                .Any(c => c.Id == item.CalendarId);

            if (!canSeeCalendar)
                throw new SecurityException("Access Denied!");

            return await foundationService.AddAsync(new CalendarEvent { Id = item.Id, Start = item.Start, End = item.End, CalendarId = item.CalendarId, Name = item.Name });
        }

        public async Task DeleteAsync(CalendarEvent item)
        {
            bool canSeeCalendarEvent = foundationService.GetAll<CalendarEvent>()
                .Any(ce => ce.Id == item.Id);

            if (!canSeeCalendarEvent)
                throw new SecurityException("Access Denied!");

            await foundationService.DeleteAsync(new CalendarEvent { Id = item.Id });
        }

        public IQueryable<CalendarEvent> GetAll()
            => foundationService.GetAll<CalendarEvent>();

        public async Task<CalendarEvent> UpdateAsync(CalendarEvent item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Access Denied!");

            if (item.End < item.Start)
                throw new ValidationException("Start cannot be before end");

            bool canSeeCalendar = foundationService.GetAll<Calendar>()
                .Any(c => c.Id == item.CalendarId);

            if (!canSeeCalendar)
                throw new SecurityException("Access Denied!");

            return await foundationService.UpdateAsync(new CalendarEvent { Id = item.Id, Start = item.Start, End = item.End, CalendarId = item.CalendarId, Name = item.Name });
        }
    }
}
