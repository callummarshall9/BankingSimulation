using System.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public class CalendarViewService(IFoundationService foundationService) : ICalendarViewService
{
    public CalendarsViewModel Index()
    {
        return new CalendarsViewModel()
        {
            Calendars = foundationService.GetAll<Calendar>()
                .Select(c => new CalendarViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Deleting = false
                }).ToList()
        };
    }

    public async Task<CalendarsViewModel> DeleteCalendarAsync(CalendarViewModel calendar, CalendarsViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<Calendar>()
                .FirstOrDefault(c => c.Id == calendar.Id);

            if (existing == null)
                throw new DataException("Calendar not found");

            await foundationService.DeleteAsync(existing);

            return Index();

        }
        catch (Exception ex)
        {
            calendar.Deleting = false;
            calendar.Exception = ex;

            return model;
        }
    }
}