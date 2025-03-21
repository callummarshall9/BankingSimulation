using BankingSimulation.BlazorServer.ViewModels.Roles;

namespace BankingSimulation.BlazorServer.ViewServices.Roles;

public interface IRolesViewService
{
    RolesViewModel Index();
    Task<RolesViewModel> DeleteAsync(RoleViewModel role, RolesViewModel model);
}