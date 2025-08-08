using BankingSimulation.Blazor.Models.Roles;
using BankingSimulation.Services.Processing;

namespace BankingSimulation.Blazor.ViewServices.Roles
{
    public class EditRoleViewService(IRoleProcessingService roleProcessingService,
        IUserRoleProcessingService userRoleProcessingService) : IEditRoleViewService
    {
        public EditRoleViewModel GetRole(Guid roleId)
        {
            var result = roleProcessingService.GetAll()
                .Where(r => r.Id == roleId)
                .Select(r => new EditRoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    UserIds = r.UserRoles.OrderBy(ur => ur.UserId).Select(ur => ur.UserId).ToList()
                })
                .FirstOrDefault();

            if (result is null)
            {
                throw new InvalidOperationException($"Role with ID {roleId} not found.");
            }

            return result;
        }

        public async Task<EditRoleViewModel> UpdateRoleAsync(EditRoleViewModel editRoleViewModel)
        {
            var updatedRole = await roleProcessingService.UpdateAsync(new Data.Models.Role
            {
                Id = editRoleViewModel.Id,
                Name = editRoleViewModel.Name
            });

            await MergeUserRolesAsync(editRoleViewModel, updatedRole);

            return GetRole(editRoleViewModel.Id);
        }

        private async Task MergeUserRolesAsync(EditRoleViewModel editRoleViewModel, Data.Models.Role updatedRole)
        {
            var existingUserRoles = userRoleProcessingService
                .GetAll()
                .Where(c => c.RoleId == editRoleViewModel.Id)
                .ToList();

            foreach (var existingUserRole in existingUserRoles)
            {
                var receivedEvent = editRoleViewModel.UserIds.FirstOrDefault(ce =>
                    ce == existingUserRole.UserId
                );

                if (receivedEvent == null)
                    await userRoleProcessingService.DeleteAsync(existingUserRole);
            }

            foreach (var incomingUserRole in editRoleViewModel.UserIds)
            {
                var existingEvent = existingUserRoles.FirstOrDefault(ce =>
                    ce.UserId == incomingUserRole
                );

                if (existingEvent == null)
                    await userRoleProcessingService.AddAsync(new Data.Models.UserRole
                    {
                        RoleId = updatedRole.Id,
                        UserId = incomingUserRole
                    });
            }
        }
    }
}
