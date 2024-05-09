using System;

namespace BankingSimulation.Data.Models
{
    public class ComputeCalendarCategoryStatsResult
    {
        public string CategoryName { get; set; }
        public Guid? CategoryId { get; set; }
        public string CalendarEventName { get; set; }
        public DateOnly CalendarEventStart { get; set; }
        public Guid CalendarEventId { get; set; }
        public double Value { get; set; }
        public string FriendlyName { get; set; }
    }
}
