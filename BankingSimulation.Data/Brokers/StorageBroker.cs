using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BankingSimulation.Data;

public class StorageBroker : IStorageBroker
{
    private readonly BankSimulationContext context;

    public StorageBroker(BankSimulationContext context)
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

    public IQueryable<T> GetAll<T>() where T : class
        => context.Set<T>()
            .AsNoTracking();
}
