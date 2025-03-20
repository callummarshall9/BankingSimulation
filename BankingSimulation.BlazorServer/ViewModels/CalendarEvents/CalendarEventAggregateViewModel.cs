using BankingSimulation.Data.Models;

namespace BankingSimulation.UI.ViewModels.CalendarEvents;

public class CalendarEventAggregateViewModel
{
    public Guid CalendarId { get; set; }
    public ICollection<CalendarEventViewModel> Events { get; set; } = [];
}