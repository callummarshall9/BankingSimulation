using System;

namespace BankingSimulation.Data.Models
{
    public class PeriodAccountSummary
    {
        public string CalendarEventName { get; set; }
        public DateOnly CalendarEventStart { get; set; }
        public DateOnly CalendarEventEnd { get; set; }
        public Guid AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public string ComputedAccountInfo { get; set; }
        public double Outgoings { get; set; }
        public double Incomings { get; set; }
        public double Net { get => Math.Round(Outgoings + Incomings, 2); }
    }
}
