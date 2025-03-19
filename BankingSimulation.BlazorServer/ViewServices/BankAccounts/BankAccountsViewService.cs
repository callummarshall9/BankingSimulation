using BankingSimulation.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.Services.Processing;
using BankingSimulation.UI.ViewModels;

namespace BankingSimulation.UI.ViewServices;

public class BankAccountsViewService(IFoundationService foundationService) : IBankAccountsViewService
{
    public BankAccountsIndexViewModel Index()
    {
        var accounts = foundationService.GetAll<Account>()
            .Select(a => new AccountViewModel
            {
                Name = a.Name,
                FriendlyName = a.FriendlyName,
                Number = a.Number,
                SystemReferences = a.AccountSystemReferences.Select(accs => accs.BankingSystemId).ToList()
            })
            .ToList();

        var calendars = foundationService.GetAll<Calendar>()
            .OrderBy(a => a.Name)
            .Select(c => new CalendarViewModel
            {
                Id = c.Id,
                Name = c.Name
            })
            .ToList();

        return new BankAccountsIndexViewModel
        {
            Accounts = accounts,
            Calendars = calendars
        };
    }
}