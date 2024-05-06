using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using System.Security;

namespace BankingSimulation.Services;

public class FoundationService(IStorageBroker storageBroker, 
    IAuthorisationBroker authorisationBroker) : IFoundationService
{

    public async Task<T> AddAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return await storageBroker.AddAsync(item);
    }

    public IQueryable<T> GetAll<T>() where T : class
        => storageBroker.GetAll<T>();

    public async Task<T> UpdateAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return await storageBroker.UpdateAsync(item);
    }

    public async Task DeleteAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        await storageBroker.DeleteAsync(item);
    }
}
