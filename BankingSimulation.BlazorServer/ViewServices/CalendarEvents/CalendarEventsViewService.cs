using System.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.UI.ViewModels.CalendarEvents;

namespace BankingSimulation.BlazorServer.ViewServices.CalendarEvents;

public class CalendarEventsViewService(IFoundationService foundationService) : ICalendarEventsViewService
{
    public CalendarEventAggregateViewModel Index(Guid calendarId)
        => new()
        {
            CalendarId = calendarId,
            Events = foundationService.GetAll<CalendarEvent>()
                .Where(ce => ce.CalendarId == calendarId)
                .Select(ce => new CalendarEventViewModel()
                {
                    Id = ce.Id,
                    Name = ce.Name,
                    Start = ce.Start,
                    End = ce.End,
                    Exception = null,
                    Deleting = false
                }).ToList()
        };
    
    public async Task<CalendarEventAggregateViewModel> DeleteCalendarEventAsync(CalendarEventViewModel calendarEvent, CalendarEventAggregateViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<CalendarEvent>()
                .FirstOrDefault(c => c.Id == calendarEvent.Id);

            if (existing == null)
                throw new DataException("Calendar event not found");

            await foundationService.DeleteAsync(existing);

            return Index(model.CalendarId);

        }
        catch (Exception ex)
        {
            calendarEvent.Deleting = false;
            calendarEvent.Exception = ex;

            return model;
        }
    }
}