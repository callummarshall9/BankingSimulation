using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class RoleProcessingService(IFoundationService foundationService, IAuthorisationBroker authorisationBroker) : IRoleProcessingService
    {
        public async Task<Role> AddAsync(Role item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty!");

            var addedRole = await foundationService.AddAsync(new Role
            {
                Name = item.Name,
                CreatedOn = DateTime.Now
            });

            string userId = authorisationBroker.GetUserId();

            await foundationService.AddAsync(new UserRole { RoleId = addedRole.Id, UserId = userId });

            return addedRole;
        }

        public async Task DeleteAsync(Role item)
        {
            string userId = authorisationBroker.GetUserId();

            //Need to perform this here as we are going to do previous logic before deleting the role...
            if (userId == "Guest")
                throw new SecurityException("Access Denied!");

            bool userInRoleUsers = foundationService.GetAll<UserRole>()
                .Any(ur => ur.UserId == userId && ur.RoleId == item.Id);

            if (!userInRoleUsers)
                throw new SecurityException("Access Denied!");

            var userRoles = foundationService.GetAll<UserRole>()
                .Where(ur => ur.RoleId == item.Id)
                .ToArray();

            foreach(var userRole in userRoles)
                await foundationService.DeleteAsync(userRole);

            await foundationService.DeleteAsync(new Role { Id = item.Id });
        }

        public IQueryable<Role> GetAll()
            => foundationService.GetAll<Role>();

        public Task<Role> UpdateAsync(Role item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty!");

            string userId = authorisationBroker.GetUserId();

            bool userInRoleUsers = foundationService.GetAll<UserRole>()
                .Any(ur => ur.UserId == userId && ur.RoleId == item.Id);

            if (!userInRoleUsers)
                throw new SecurityException("Access Denied!");

            var existingCreatedOn = foundationService.GetAll<Role>()
                .Where(r => r.Id == item.Id)
                .Select(r => r.CreatedOn)
                .First();

            return foundationService.UpdateAsync(new Role
            {
                Id = item.Id,
                Name = item.Name,
                CreatedOn = existingCreatedOn
            });
        }
    }
}
