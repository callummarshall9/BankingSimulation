using BankingSimulation.Data.Models;
using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public interface IAddCalendarViewService
{
    Task<AddCalendarViewModel> AddCalendarAsync(AddCalendarViewModel model);
}