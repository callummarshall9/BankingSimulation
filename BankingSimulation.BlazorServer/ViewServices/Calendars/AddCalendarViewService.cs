using System.ComponentModel.DataAnnotations;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public class AddCalendarViewService(IFoundationService foundationService,
    IAuthorisationBroker broker) : IAddCalendarViewService
{
    public async Task<AddCalendarViewModel> AddCalendarAsync(AddCalendarViewModel model)
    {
        try
        {
            if (string.IsNullOrEmpty(model.Name))
                throw new ValidationException("Name is required");

            string userId = broker.GetUserId();
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

            await foundationService.AddAsync(new Calendar
            {
                Name = model.Name,
                RoleId = existingRole
            });
            
            return new AddCalendarViewModel
            {
                Name = model.Name,
                Exception = null,
                Success = true
            };
        }
        catch (Exception ex)
        {
            return new AddCalendarViewModel
            {
                Name = model.Name,
                Exception = ex,
                Success = false
            };
        }
    }
}