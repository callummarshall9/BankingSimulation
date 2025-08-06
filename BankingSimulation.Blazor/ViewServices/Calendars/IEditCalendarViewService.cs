using BankingSimulation.Blazor.Models.Calendars;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public interface IEditCalendarViewService
    {
        EditCalendarViewModel GetEditCalendarViewModel(Guid calendarId);
        Task<EditCalendarViewModel> UpdateCalendarAsync(EditCalendarViewModel editCalendarViewModel);
    }
}