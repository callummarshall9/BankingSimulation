using BankingSimulation.Data.Models;
using BankingSimulation.Services.Processing;
using System.ComponentModel.DataAnnotations;

namespace BankingSimulation.Services.Orchestration
{
    public class CategoryRoleOrchestrationService(ICategoryProcessingService categoryProcessingService,
        IRoleProcessingService roleProcessingService) : ICategoryRoleOrchestrationService
    {
        public async Task<Category> AddCategoryAsync(string? name, string? description)
        {
            if (string.IsNullOrEmpty(name))
                throw new ValidationException("Name cannot be empty");

            if (string.IsNullOrEmpty(description))
                throw new ValidationException("Description cannot be empty");

            var roleId = await roleProcessingService.CreateCategoryRoleAsync();

            return await categoryProcessingService.AddAsync(new Category
            {
                Name = name,
                Description = description,
                RoleId = roleId
            });
        }
    }
}
