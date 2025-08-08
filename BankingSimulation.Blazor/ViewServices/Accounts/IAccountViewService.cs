using BankingSimulation.Blazor.Models.Accounts;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public interface IAccountViewService
    {
        IEnumerable<AccountViewModel> GetAccounts();
    }
}