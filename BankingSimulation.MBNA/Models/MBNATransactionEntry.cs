using CsvHelper.Configuration.Attributes;

namespace BankingSimulation.MBNA.Models
{
    internal class MBNATransactionEntry
    {
        [Name("Date")]
        public string Date { get; set; }
        
        [Name("Reference")]
        public string Reference { get; set; }
        
        [Name("Description")]
        public string Description { get; set; }

        [Name("Amount")]
        public double Amount { get; set; }
    }
}