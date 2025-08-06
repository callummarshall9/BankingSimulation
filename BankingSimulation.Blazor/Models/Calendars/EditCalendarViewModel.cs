using BankingSimulation.Data.Models;

namespace BankingSimulation.Blazor.Models.Calendars
{
    public class EditCalendarViewModel
    {
        public Guid CalendarId { get; set; }
        public string? Name { get; set; }
        public IList<EditCalendarEventViewModel> Events { get; set; } = [];
        public Guid RoleId { get; set; }
    }
}
