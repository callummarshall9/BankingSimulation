namespace BankingSimulation.UI.ViewModels.CalendarEvents;

public class AddNewCalendarEventViewModel
{
    public string? Name { get; set; }
    
    public DateOnly Start { get; set; }
    public DateOnly End { get; set; }
    
    public Guid CalendarId { get; set; }
    public bool Loading { get; set; }
    public Exception? Exception { get; set; }
    public bool Success { get; set; }
}