using BankingSimulation.Blazor.Models.Roles;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Roles
{
    public class RoleViewService(IRoleProcessingService roleProcessingService) : IRoleViewService
    {
        public IEnumerable<RoleViewModel> GetRoles()
        {
            return roleProcessingService.GetAll()
                .Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    EditUrl = "/roles/" + r.Id,
                    UserIds = r.UserRoles.OrderBy(ur => ur.UserId).Select(ur => ur.UserId)
                }).ToList();
        }

        public async Task DeleteRoleAsync(Guid roleId)
        {
            var role = roleProcessingService.GetAll()
                .FirstOrDefault(c => c.Id == roleId);

            if (role != null)
                await roleProcessingService.DeleteAsync(role);
        }
    }
}
