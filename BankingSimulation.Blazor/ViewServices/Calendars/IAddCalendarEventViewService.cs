using BankingSimulation.Blazor.Models.Calendars;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public interface IAddCalendarEventViewService
    {
        Task AddCalendarEventAsync(AddCalendarEventViewModel addCalendarEventViewModel);
    }
}