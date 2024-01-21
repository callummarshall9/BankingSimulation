using System;

namespace BankingSimulation.Data.Models
{
    public class MonthlyAccountSummary
    {
        public DateOnly Date { get; set; }
        public double Outgoings { get; set; }
        public double Incomings { get; set; }
    }
}
