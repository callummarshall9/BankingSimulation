namespace BankingSimulation.UI.ViewModels.CalendarEvents;

public class CalendarEventViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    
    public bool Deleting { get; set; }
    public Exception? Exception { get; set; }
}