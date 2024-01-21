using CsvHelper.Configuration.Attributes;

namespace BankingSimulation.RBS;

internal class RBSTransactionEntry
{
    public string Date { get; set; }
    public string Type { get; set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public double Balance { get;set; }

    [Name("Account Name")]
    public string AccountName { get; set; }

    [Name("Account Number")]
    public string AccountNumber { get; set; }
}