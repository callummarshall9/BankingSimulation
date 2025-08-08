namespace BankingSimulation.Blazor.Models.Calendars
{
    public class CalendarViewModel
    {
        public bool Deleting { get; set; } = false;
        public string EventUrl { get; set; } = null!;
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
