using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.UI.ViewModels.CalendarEvents;

namespace BankingSimulation.BlazorServer.ViewServices.CalendarEvents;

public class AddCalendarEventViewService(IFoundationService foundationService) : IAddCalendarEventViewService
{
    public async Task<AddNewCalendarEventViewModel> AddCalendarEventAsync(AddNewCalendarEventViewModel calendarEvent)
    {
        try
        {
            if(string.IsNullOrEmpty(calendarEvent.Name))
                throw new ArgumentException("Name cannot be null or empty");
        
            if (calendarEvent.Start == default)
                throw new ArgumentException("Start cannot be null");
        
            if (calendarEvent.End == default)
                throw new ArgumentException("End cannot be null");

            await foundationService.AddAsync(new CalendarEvent()
            {
                Start = calendarEvent.Start,
                End = calendarEvent.End,
                Name = calendarEvent.Name,
                CalendarId = calendarEvent.CalendarId
            });

            return new AddNewCalendarEventViewModel()
            {
                Start = calendarEvent.Start,
                End = calendarEvent.End,
                Name = calendarEvent.Name,
                CalendarId = calendarEvent.CalendarId,
                Loading = false,
                Exception = null,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AddNewCalendarEventViewModel()
            {
                Start = calendarEvent.Start,
                End = calendarEvent.End,
                Name = calendarEvent.Name,
                CalendarId = calendarEvent.CalendarId,
                Exception = ex,
                Success = false,
                Loading = false
            };
        }
    }
}