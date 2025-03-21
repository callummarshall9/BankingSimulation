using System.Data;
using BankingSimulation.BlazorServer.ViewModels.Categories;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;

namespace BankingSimulation.BlazorServer.ViewServices.Categories;

public class CategoriesViewService(IFoundationService foundationService) : ICategoriesViewService
{
    public CategoriesViewModel Index()
    {
        return new CategoriesViewModel()
        {
            Categories = foundationService.GetAll<Category>()
                .OrderBy(c => c.Name)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    Keywords = c.Keywords.Select(ck => ck.Keyword).ToList()
                }).ToList()
        };
    }

    public async Task<CategoriesViewModel> DeleteAsync(CategoryViewModel category, CategoriesViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<Category>()
                .FirstOrDefault(c => c.Id == category.Id);

            if (existing == null)
                throw new DataException("Calendar not found");

            await foundationService.DeleteAsync(existing);

            return Index();

        }
        catch (Exception ex)
        {
            category.Deleting = false;
            category.Exception = ex;

            return model;
        }
    }
}