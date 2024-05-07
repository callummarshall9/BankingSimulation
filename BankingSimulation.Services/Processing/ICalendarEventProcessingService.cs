using BankingSimulation.Data.Models;

namespace BankingSimulation.Services.Processing
{
    public interface ICalendarEventProcessingService
    {
        Task<CalendarEvent> AddAsync(CalendarEvent item);
        Task DeleteAsync(CalendarEvent item);
        IQueryable<CalendarEvent> GetAll();
        Task<CalendarEvent> UpdateAsync(CalendarEvent item);
    }
}