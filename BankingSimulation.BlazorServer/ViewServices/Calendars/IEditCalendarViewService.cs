using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public interface IEditCalendarViewService
{
    EditCalendarViewModel Index(Guid calendarId);
    Task<EditCalendarViewModel> UpdateCalendarAsync(EditCalendarViewModel model);
}