using CsvHelper.Configuration.Attributes;

namespace BankingSimulation.Barclays.Models
{
    public class BarclaysTransactionEntry
    {
        [Name("Date")]
        public string Date { get; set; }

        [Name("Account")]
        public string Account { get; set; }

        [Name("Amount")]
        public double Amount { get; set; }

        [Name("Memo")]
        public string Memo { get; set; }

        [Name("Subcategory")]
        public string Subcategory { get; set; }
    }
}
