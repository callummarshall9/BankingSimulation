using System;

namespace BankingSimulation.Data.Models
{
    public class CalendarEvent
    {
        public Guid Id { get; set; }
        public Guid CalendarId { get; set; }
        public string Name { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
    }
}
