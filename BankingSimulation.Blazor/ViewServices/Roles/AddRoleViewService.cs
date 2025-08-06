using BankingSimulation.Services.Processing;
using System.ComponentModel.DataAnnotations;

namespace BankingSimulation.Blazor.ViewServices.Roles
{
    public class AddRoleViewService(IRoleProcessingService roleProcessingService) : IAddRoleViewService
    {
        public async Task AddRoleAsync(string? roleName)
        {
            if (string.IsNullOrEmpty(roleName))
                throw new ValidationException("Role name cannot be empty");

            await roleProcessingService.AddAsync(new Data.Models.Role
            {
                Name = roleName
            });
        }
    }
}
