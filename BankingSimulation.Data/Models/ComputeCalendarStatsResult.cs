using System;

namespace BankingSimulation.Data.Models
{
    public class ComputeCalendarStatsResult
    {
        public string CalendarEventName { get; set; }
        public DateOnly CalendarEventStart { get; set; }
        public Guid CalendarEventId { get; set; }
        public double Value { get; set; }
        public string FriendlyName { get; set; }
    }
}
