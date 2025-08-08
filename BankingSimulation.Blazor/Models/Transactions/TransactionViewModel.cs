namespace BankingSimulation.Blazor.Models.Transactions
{
    public class TotalTransactionsViewModel
    {
        public double Summation { get; set; }
        public IEnumerable<TransactionViewModel> Transactions { get; set; } = [];
    }

    public class TransactionViewModel
    {
        public DateOnly Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public string AccountNumber { get; set; } = string.Empty;
        public double Value { get; set; }
    }
}
