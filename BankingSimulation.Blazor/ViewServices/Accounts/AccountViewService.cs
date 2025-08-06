using BankingSimulation.Blazor.Models.Accounts;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Accounts
{
    public class AccountViewService(IAccountProcessingService accountProcessingService) : IAccountViewService
    {
        public IEnumerable<AccountViewModel> GetAccounts()
        {
            var query = accountProcessingService.GetAll()
                .Select(a => new
                {
                    a.Name,
                    a.FriendlyName,
                    a.Number,
                    AccountSystemReferences = a.AccountSystemReferences.Select(asr => asr.BankingSystemId).ToList()
                })
                .ToList();

            return query.Select(a => new AccountViewModel
            {
                DisplayName = GetAccountDisplayName(a.Name, a.FriendlyName),
                Number = a.Number,
                AccountSystemReferences = a.AccountSystemReferences
            }).ToList();
        }

        static string GetAccountDisplayName(string name, string friendlyName)
        {
            if (!string.IsNullOrEmpty(friendlyName))
            {
                return friendlyName + " (" + name + ")";
            }

            return name;
        }
    }
}