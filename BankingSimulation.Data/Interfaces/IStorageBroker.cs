using System.Linq;
using System.Threading.Tasks;

namespace BankingSimulation.Data;

public interface IStorageBroker
{
    Task<T> AddAsync<T>(T item) where T : class;
    Task<T> UpdateAsync<T>(T item) where T : class;
    Task DeleteAsync<T>(T item) where T : class;
    IQueryable<T> GetAll<T>(bool ignoreFilters = false) where T : class;
}
