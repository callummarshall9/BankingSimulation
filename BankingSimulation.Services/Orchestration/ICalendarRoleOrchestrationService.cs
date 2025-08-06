using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Orchestration
{
    public interface ICalendarRoleOrchestrationService
    {
        Task<Calendar> AddCalendarAsync(string? name);
    }
}