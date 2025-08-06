using BankingSimulation.Blazor.Models.Accounts;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public class CalendarViewService(ICalendarProcessingService calendarProcessingService) : ICalendarViewService
    {
        public IEnumerable<CalendarViewModel> GetCalendarViewModels()
        {
            return calendarProcessingService.GetAll()
                .Select(c => new CalendarViewModel
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToList();
        }
    }
}
