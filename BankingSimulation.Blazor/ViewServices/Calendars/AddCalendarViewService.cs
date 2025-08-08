using BankingSimulation.Services.Orchestration;

namespace BankingSimulation.Blazor.ViewServices.Calendars
{
    public class AddCalendarViewService(ICalendarRoleOrchestrationService calendarRoleOrchestrationService) : IAddCalendarViewService
    {
        public async Task AddCalendarAsync(string? name)
            => await calendarRoleOrchestrationService.AddCalendarAsync(name);
    }
}
