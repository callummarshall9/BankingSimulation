using BankingSimulation.Data;

namespace BankingSimulation.Services;

public class AccountProcessingService
{
    private readonly IFoundationService foundationService;

    public AccountProcessingService(IFoundationService foundationService)
    {
        this.foundationService = foundationService;
    }

    public IQueryable<AccountBalanceDTO> GetAccountWithBalances()
        => foundationService.GetAll<Account>()
            .Select(a => new AccountBalanceDTO 
            { 
                Id = a.Id, 
                CurrentAccountBalance = a.Transactions.Any() 
                        ? a.Transactions
                        .OrderByDescending(at => at.Date)
                        .First()
                        .Balance
                        : 0.0,
                BalanceAsOf = a.Transactions.Any() 
                        ? a.Transactions
                        .OrderByDescending(at => at.Date)
                        .First()
                        .Date
                        : null
            });
}
