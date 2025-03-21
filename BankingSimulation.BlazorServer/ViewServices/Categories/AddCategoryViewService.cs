using System.ComponentModel.DataAnnotations;
using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public class AddCategoryViewService(IFoundationService foundationService, IAuthorisationBroker authorisationBroker) : IAddCategoryViewService
{
    public async Task<AddNewCategoryViewModel> AddAsync(AddNewCategoryViewModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Name))
                throw new ValidationException("Name is required");

            if (string.IsNullOrEmpty(model.Description))
                throw new ValidationException("Description is required");

            string userId = authorisationBroker.GetUserId();
            string roleName = userId + " Calendars";

            var existingRole = foundationService.GetAll<Role>()
                .Where(r => r.Name == roleName)
                .Select(r => r.Id)
                .FirstOrDefault();

            if (existingRole == Guid.Empty)
            {
                existingRole = (await foundationService.AddAsync(new Role
                {
                    Name = roleName,
                    CreatedOn = DateTimeOffset.Now,
                    UserRoles =
                    [
                        new UserRole
                        {
                            UserId = userId,
                            RoleId = Guid.Empty
                        }
                    ]
                })).Id;
            }

            await foundationService.AddAsync(new Category
            {
                Name = model.Name,
                Description = model.Description,
                RoleId = existingRole,
            });

            return new AddNewCategoryViewModel
            {
                Success = true,
                Description = model.Description,
                Name = model.Name,
                Exception = null,
                Loading = false
            };
        }
        catch (Exception ex)
        {
            return new AddNewCategoryViewModel
            {
                Success = false,
                Description = model.Description,
                Name = model.Name,
                Exception = ex,
                Loading = false
            };
        }
    }
}