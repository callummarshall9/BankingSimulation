using BankingSimulation.Data;

namespace BankingSimulation.Services;

public class FoundationService : IFoundationService
{
    private readonly IStorageBroker storageBroker;

    public FoundationService(IStorageBroker storageBroker)
{
        this.storageBroker = storageBroker;
    }

    public Task<T> AddAsync<T>(T item) where T : class
        => storageBroker.AddAsync(item);

    public IQueryable<T> GetAll<T>() where T : class
        => storageBroker.GetAll<T>();

    public Task<T> UpdateAsync<T>(T item) where T : class
        => storageBroker.UpdateAsync(item);

    public Task DeleteAsync<T>(T item) where T : class
        => storageBroker.DeleteAsync(item);
}
