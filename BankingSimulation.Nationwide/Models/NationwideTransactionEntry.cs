using CsvHelper.Configuration.Attributes;

namespace BankingSimulation.Nationwide.Models
{
    internal class NationwideTransactionEntry
    {
        [Name("Date")]
        public string Date { get; set; }

        [Name("Transaction type")]
        public string TransactionType { get; set; }

        [Name("Description")]
        public string Description { get; set; }

        [Name("Paid out")]
        public string Amount1 { get; set; }

        [Name("Paid in")]
        public string Amount2 { get; set; }
    }
}
