namespace BankingSimulation.UI.ViewModels;

public class BankAccountsIndexViewModel
{
    public IEnumerable<AccountViewModel> Accounts { get; init; } = [];
    public IEnumerable<CalendarViewModel> Calendars { get; init; } = [];
}