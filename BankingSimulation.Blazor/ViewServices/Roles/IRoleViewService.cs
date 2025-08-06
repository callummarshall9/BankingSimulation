using BankingSimulation.Blazor.Models.Roles;

namespace BankingSimulation.Blazor.ViewServices.Roles
{
    public interface IRoleViewService
    {
        IEnumerable<RoleViewModel> GetRoles();
        Task DeleteRoleAsync(Guid roleId);
    }
}