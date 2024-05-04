using BankingSimulation.Data.Models;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class CategoryProcessingService(IFoundationService foundationService) : ICategoryProcessingService
    {
        public Task<Category> AddAsync(Category item)
        {
            bool roleExists = foundationService.GetAll<Role>()
                .Any(r => r.Id == item.RoleId);

            if (!roleExists)
                throw new SecurityException("Access Denied!");

            return foundationService.AddAsync(item);
        }

        public Task DeleteAsync(Category item)
        {
            bool categoryExists = foundationService.GetAll<Category>()
                .Any(r => r.Id == item.Id);

            if (!categoryExists)
                throw new SecurityException("Access Denied!");

            return foundationService.DeleteAsync(item);
        }

        public IQueryable<Category> GetAll()
            => foundationService.GetAll<Category>();

        public Task<Category> UpdateAsync(Category item)
        {
            bool categoryExists = foundationService.GetAll<Category>()
                .Any(r => r.Id == item.Id);

            if (!categoryExists)
                throw new SecurityException("Access Denied!");

            return foundationService.UpdateAsync(item);
        }
    }
}
