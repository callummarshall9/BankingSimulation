using CsvHelper.Configuration.Attributes;

namespace BankingSimulation.RBS;

internal class RBSTransactionEntry
{
    [Name("Date")]
    public string Date { get; set; }

    [Name("Type")]
    public string Type { get; set; }

    [Name("Description")]
    public string Description { get; set; }
    [Name("Value")]
    public double Value { get; set; }

    [Name("Account Name")]
    public string AccountName { get; set; }

    [Name("Account Number")]
    public string AccountNumber { get; set; }
}