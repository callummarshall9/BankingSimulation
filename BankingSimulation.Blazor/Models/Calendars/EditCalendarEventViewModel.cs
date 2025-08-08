namespace BankingSimulation.Blazor.Models.Calendars
{
    public class EditCalendarEventViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public DateOnly Start { get; set; }
        public DateOnly End { get; set; }
    }
}
