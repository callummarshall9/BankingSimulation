using BankingSimulation.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CalendarProcessingService(IFoundationService foundationService) : ICalendarProcessingService
    {
        public async Task<Calendar> AddAsync(Calendar item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty");

            bool roleExists = foundationService.GetAll<Role>()
                .Any(r => r.Id == item.RoleId);

            if (!roleExists)
                throw new SecurityException("Access Denied!");

            return await foundationService.AddAsync(item);
        }

        public async Task DeleteAsync(Calendar item)
        {
            var canSeeCalendar = foundationService.GetAll<Calendar>()
                .Any(c => c.Id == item.Id);

            if (!canSeeCalendar)
                throw new SecurityException("Access Denied!");

            await foundationService.DeleteAsync(new Calendar { Id = item.Id });
        }

        public IQueryable<Calendar> GetAll()
            => foundationService.GetAll<Calendar>();

        public async Task<Calendar> UpdateAsync(Calendar item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty");

            var dbRoleId = foundationService.GetAll<Calendar>()
                .Where(c => c.Id == item.Id)
                .Select(c => c.RoleId)
                .FirstOrDefault();

            if (dbRoleId == default)
                throw new SecurityException("Access Denied!");

            if (item.RoleId != dbRoleId)
                throw new InvalidOperationException("Cannot change role id");

            return await foundationService.UpdateAsync(new Calendar { Id = item.Id, Name = item.Name, RoleId = item.RoleId });
        }
    }
}
