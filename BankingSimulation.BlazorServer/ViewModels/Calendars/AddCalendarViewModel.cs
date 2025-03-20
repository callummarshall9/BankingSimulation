namespace BankingSimulation.UI.ViewModels.Calendars;

public class AddCalendarViewModel
{
    public string Name { get; set; }
    
    public bool Success { get; set; }
    public Exception? Exception { get; set; }
    public bool Loading { get; set; }
}