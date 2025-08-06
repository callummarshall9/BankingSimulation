using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;
using System.ComponentModel.DataAnnotations;

namespace BankingSimulation.Services.Orchestration
{
    public class CalendarRoleOrchestrationService(ICalendarProcessingService calendarProcessingService,
        IRoleProcessingService roleProcessingService) : ICalendarRoleOrchestrationService
    {
        public async Task<Calendar> AddCalendarAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Name cannot be left empty");

            var roleId = await roleProcessingService.CreateCalendarRoleAsync();

            var calendar = new Calendar
            {
                Name = name,
                RoleId = roleId
            };

            return await calendarProcessingService.AddAsync(new Calendar
            {
                Name = name,
                RoleId = roleId
            });
        }
    }
}
