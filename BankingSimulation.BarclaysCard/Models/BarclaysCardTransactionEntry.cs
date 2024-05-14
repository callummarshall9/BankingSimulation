namespace BankingSimulation.BarclaysCard.Models
{
    internal class BarclaysCardTransactionEntry
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public string Card { get; set; }
        public string Cardholder { get; set; }
        public string Type { get; set; }
        public string Amount { get; set; }
        public string Amount2 { get; set; }
    }
}
