using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ICalendarProcessingService
    {
        Task<Calendar> AddAsync(Calendar item);
        Task DeleteAsync(Calendar item);
        IQueryable<Calendar> GetAll();
        Task<Calendar> UpdateAsync(Calendar item);
    }
}