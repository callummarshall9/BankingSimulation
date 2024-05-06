using BankingSimulation.Data.Models;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class UserRoleProcessingService(IFoundationService foundationService) : IUserRoleProcessingService
    {
        public async Task<UserRole> AddAsync(UserRole item)
        {
            bool roleExists = foundationService.GetAll<Role>()
                .Any(r => r.Id == item.RoleId);

            if (!roleExists)
                throw new SecurityException("Access Denied!");

            return await foundationService.AddAsync(item);
        }

        public async Task DeleteAsync(UserRole item)
        {
            bool roleExists = foundationService.GetAll<Role>()
                .Any(r => r.Id == item.RoleId);

            if (!roleExists)
                throw new SecurityException("Access Denied!");

            await foundationService.DeleteAsync(item);
        }

        public IQueryable<UserRole> GetAll()
            => foundationService.GetAll<UserRole>();
    }
}
