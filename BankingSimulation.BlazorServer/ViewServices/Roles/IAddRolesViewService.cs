using BankingSimulation.BlazorServer.ViewModels.Roles;

namespace BankingSimulation.BlazorServer.ViewServices.Roles;

public interface IAddRolesViewService
{
    Task<AddNewRolesViewModel> AddAsync(AddNewRolesViewModel model);
}