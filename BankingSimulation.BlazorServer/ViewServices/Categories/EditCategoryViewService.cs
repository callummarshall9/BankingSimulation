using System.Data;
using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public class EditCategoryViewService(IFoundationService foundationService) : IEditCategoryViewService
{
    public EditCategoryViewModel Index(Guid categoryId)
    {
        var categoryInformation = foundationService.GetAll<Category>()
            .Where(c => c.Id == categoryId)
            .Select(c => new { c.Name, c.Description })
            .FirstOrDefault();
        
        if (categoryInformation == null)
            throw new DataException("Category not found");
        
        return new EditCategoryViewModel()
        {
            Id = categoryId,
            Name = categoryInformation.Name,
            Description =  categoryInformation.Description
        };
    }
    
    public async Task<EditCategoryViewModel> UpdateAsync(EditCategoryViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<Category>()
                .FirstOrDefault(c => c.Id == model.Id);

            if (existing == null)
                throw new DataException("Calendar not found");
        
            existing.Name = model.Name;
            existing.Description = model.Description;
        
            await foundationService.UpdateAsync(existing);

            return new EditCategoryViewModel()
            {
                Name = model.Name,
                Description = model.Description,
                Id = model.Id,
                Loading = false,
                Exception = null
            };
        }
        catch (Exception ex)
        {
            return new EditCategoryViewModel()
            {
                Name = model.Name,
                Description = model.Description,
                Id = model.Id,
                Loading = false,
                Exception = ex
            };
        }
    }
}