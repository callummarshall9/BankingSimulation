namespace BankingSimulation.Services;

public interface IFoundationService
{
    Task<T> AddAsync<T>(T item) where T : class;
    Task<T> UpdateAsync<T>(T item) where T : class;
    Task DeleteAsync<T>(T item) where T : class;
    IQueryable<T> GetAll<T>() where T : class;
}
