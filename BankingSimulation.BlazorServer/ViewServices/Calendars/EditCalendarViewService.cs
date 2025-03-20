using System.Data;
using BankingSimulation.Data.Models;
using BankingSimulation.Services;
using BankingSimulation.UI.ViewModels.Calendars;

namespace BankingSimulation.UI.ViewServices;

public class EditCalendarViewService(IFoundationService foundationService) : IEditCalendarViewService
{
    public EditCalendarViewModel Index(Guid calendarId)
    {
        return new EditCalendarViewModel()
        {
            Id = calendarId,
            Name = foundationService.GetAll<Calendar>()
                .Where(c => c.Id == calendarId)
                .Select(c => c.Name)
                .FirstOrDefault()
        };
    }

    public async Task<EditCalendarViewModel> UpdateCalendarAsync(EditCalendarViewModel model)
    {
        try
        {
            var existing = foundationService
                .GetAll<Calendar>()
                .FirstOrDefault(c => c.Id == model.Id);

            if (existing == null)
                throw new DataException("Calendar not found");
        
            existing.Name = model.Name;
        
            await foundationService.UpdateAsync(existing);

            return new EditCalendarViewModel()
            {
                Name = model.Name,
                Id = model.Id,
                Loading = false,
                Exception = null
            };
        }
        catch (Exception ex)
        {
            return new EditCalendarViewModel()
            {
                Name = model.Name,
                Id = model.Id,
                Loading = false,
                Exception = ex
            };
        }
    }
}