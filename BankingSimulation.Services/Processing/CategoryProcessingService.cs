using BankingSimulation.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CategoryProcessingService(IFoundationService foundationService) : ICategoryProcessingService
    {
        public async Task<Category> AddAsync(Category item)
        {
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty");

            if (string.IsNullOrEmpty(item.Description))
                throw new ValidationException("Description cannot be empty");

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
            if (string.IsNullOrEmpty(item.Name))
                throw new ValidationException("Name cannot be empty");

            if (string.IsNullOrEmpty(item.Description))
                throw new ValidationException("Description cannot be empty");

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
