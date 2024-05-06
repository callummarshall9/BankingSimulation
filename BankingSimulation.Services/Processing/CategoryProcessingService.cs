using BankingSimulation.Data.Models;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CategoryProcessingService(IFoundationService foundationService) : ICategoryProcessingService
    {
        public async Task<Category> AddAsync(Category item)
        {
            bool roleExists = foundationService.GetAll<Role>()
                .Any(r => r.Id == item.RoleId);

            if (!roleExists)
                throw new SecurityException("Access Denied!");

            return await foundationService.AddAsync(
                new Category { 
                    Name = item.Name,
                    Description = item.Description,
                    RoleId = item.RoleId 
                });
        }

        public async Task DeleteAsync(Category item)
        {
            bool categoryExists = foundationService.GetAll<Category>()
                .Any(r => r.Id == item.Id);

            if (!categoryExists)
                throw new SecurityException("Access Denied!");

            await foundationService.DeleteAsync(new Category { Id = item.Id });
        }

        public IQueryable<Category> GetAll()
            => foundationService.GetAll<Category>();

        public async Task<Category> UpdateAsync(Category item)
        {
            bool categoryExists = foundationService.GetAll<Category>()
                .Any(r => r.Id == item.Id);

            if (!categoryExists)
                throw new SecurityException("Access Denied!");

            return await foundationService.UpdateAsync(new Category 
            { 
                Name = item.Name,
                Description = item.Description,
                RoleId = item.RoleId,
            });
        }
    }
}
