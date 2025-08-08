using BankingSimulation.Blazor.Models.Transactions;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Transactions
{
    public class TotalTransactionViewService(ITransactionProcessingService transactionProcessingService) : ITotalTransactionViewService
    {
        public TotalTransactionsViewModel GetTransactions()
        {
            var dbTransactions = transactionProcessingService.GetAll()
                .Select(t => new
                {
                    t.Date,
                    t.Account.Number,
                    t.Description,
                    CategoryDescription = t.Category.Description,
                    t.Value
                })
                .ToList();

            return new TotalTransactionsViewModel
            {
                Transactions = dbTransactions
                    .Select(t => new TransactionViewModel
                    {
                        Date = t.Date,
                        AccountNumber = t.Number,
                        Value = t.Value,
                        Description = GetDescription(t.Description, t.CategoryDescription)
                    }).ToList(),
                Summation = dbTransactions
                    .Sum(t => t.Value)
            };
        }

        private string GetDescription(string description, string categoryDescription)
            => description + (!string.IsNullOrEmpty(categoryDescription) ? " (" + categoryDescription + ")" : string.Empty);
    }
}
