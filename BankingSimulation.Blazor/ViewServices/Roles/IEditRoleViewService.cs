using BankingSimulation.Blazor.Models.Roles;

namespace BankingSimulation.Blazor.ViewServices.Roles
{
    public interface IEditRoleViewService
    {
        EditRoleViewModel GetRole(Guid roleId);
        Task<EditRoleViewModel> UpdateRoleAsync(EditRoleViewModel editRoleViewModel);
    }
}