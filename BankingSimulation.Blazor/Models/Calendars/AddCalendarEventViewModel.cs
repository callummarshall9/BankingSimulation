namespace BankingSimulation.Blazor.Models.Calendars
{
    public class AddCalendarEventViewModel
    {
        public Guid CalendarId { get; set; }
        public string? Name { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
    }
}
