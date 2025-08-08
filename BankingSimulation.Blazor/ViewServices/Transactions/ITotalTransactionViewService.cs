using BankingSimulation.Blazor.Models.Transactions;

namespace BankingSimulation.Blazor.ViewServices.Transactions
{
    public interface ITotalTransactionViewService
    {
        TotalTransactionsViewModel GetTransactions();
    }
}