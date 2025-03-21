using BankingSimulation.BlazorServer.ViewModels.Roles;
using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.BlazorServer.ViewServices.Roles;

public class AddRolesViewService(IRoleProcessingService roleProcessingService) : IAddRolesViewService
{
    public async Task<AddNewRolesViewModel> AddAsync(AddNewRolesViewModel model)
    {
        try
        {
            await roleProcessingService.AddAsync(new Role
            {
                Name = model.Name,
                CreatedOn = DateTimeOffset.Now,
            });
            
            return new AddNewRolesViewModel
            {
                Name = model.Name,
                Exception = null,
                Loading = false,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AddNewRolesViewModel
            {
                Name = model.Name,
                Exception = ex,
                Loading = false,
                Success = false
            };
        }

    }
}