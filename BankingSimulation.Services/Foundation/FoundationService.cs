using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using System.Security;

namespace BankingSimulation.Services;

public class FoundationService(IStorageBroker storageBroker, 
    IAuthorisationBroker authorisationBroker) : IFoundationService
{

    public Task<T> AddAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return storageBroker.AddAsync(item);
    }

    public IQueryable<T> GetAll<T>() where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return storageBroker.GetAll<T>();
    }

    public Task<T> UpdateAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return storageBroker.UpdateAsync(item);
    }

    public Task DeleteAsync<T>(T item) where T : class
    {
        if (authorisationBroker.GetUserId() == "Guest")
            throw new SecurityException("Access Denied!");

        return storageBroker.DeleteAsync(item);
    }
}
