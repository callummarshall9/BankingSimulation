using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingSimulation.Data;

public class StorageBroker : IStorageBroker
{
    private readonly ODataContext context;

    public StorageBroker(ODataContext context)
    {
        this.context = context;
    }

    public async Task<T> AddAsync<T>(T item) where T : class
    {
        var entity = context.Add(item);

        await context.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task<T> UpdateAsync<T>(T item) where T : class
    {
        var entity = context.Update(item);

        await context.SaveChangesAsync();

        return entity.Entity;
    }

    public async Task DeleteAsync<T>(T item) where T : class
    {
        context.Set<T>().Remove(item);

        await context.SaveChangesAsync();
    }

    public IQueryable<T> GetAll<T>(bool ignoreFilters = false) where T : class
    {
        var baseQueryable = context.Set<T>()
            .AsNoTracking();

        if (ignoreFilters)
            baseQueryable = baseQueryable.IgnoreQueryFilters();

        return baseQueryable;
    }

    public Task UpdateTransactionsForCategory(Guid categoryId)
        => context.UpdateTransactionsForCategory(categoryId);
}
