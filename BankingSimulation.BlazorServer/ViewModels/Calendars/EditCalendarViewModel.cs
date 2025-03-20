namespace BankingSimulation.UI.ViewModels.Calendars;

public class EditCalendarViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    
    public bool Loading { get; set; }
    public Exception? Exception { get; set; }
}