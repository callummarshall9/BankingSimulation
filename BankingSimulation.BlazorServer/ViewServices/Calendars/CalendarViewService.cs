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
}