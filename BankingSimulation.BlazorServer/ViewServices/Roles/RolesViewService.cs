using System.Data;
using BankingSimulation.BlazorServer.ViewModels.Roles;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.BlazorServer.ViewServices.Roles;

public class RolesViewService(IFoundationService foundationService) : IRolesViewService
{
    public RolesViewModel Index()
    {
        return new RolesViewModel()
        {
            Roles = foundationService.GetAll<Role>()
                .Select(r => new RoleViewModel()
                {
                    Name = r.Name,
                    Exception = null,
                    Deleting = false,
                    Id = r.Id,
                    UserIds = r.UserRoles
                        .OrderBy(ur => ur.UserId)
                        .Select(ur => ur.UserId)
                        .ToList()
                }).ToList()
        };
    }

    public async Task<RolesViewModel> DeleteAsync(RoleViewModel role, RolesViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<RoleViewModel>()
                .FirstOrDefault(rvm => rvm.Id == role.Id);

            if (existing == null)
                throw new DataException("Category keyword not found");

            await foundationService.DeleteAsync(existing);

            return Index();

        }
        catch (Exception ex)
        {
            role.Deleting = false;
            role.Exception = ex;

            return model;
        }
    }
}