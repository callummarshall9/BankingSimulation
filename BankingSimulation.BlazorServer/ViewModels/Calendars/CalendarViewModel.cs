namespace BankingSimulation.UI.ViewModels.Calendars;

public class CalendarViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public bool Deleting { get; set; }
    public Exception? Exception { get; set; }
}