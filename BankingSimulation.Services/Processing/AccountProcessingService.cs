using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using BankingSimulation.Data.Models;
using System.Security;

namespace BankingSimulation.Services.Processing
{
    public class AccountProcessingService(IFoundationService foundationService,
        IAuthorisationBroker authorisationBroker) : IAccountProcessingService
    {
        public async Task<Account> AddAsync(Account item)
        {
            //Null out collections
            item.Transactions = null;
            item.AccountSystemReferences = null;
            item.Roles = null;

            var accountCreated = await foundationService.AddAsync(item);

            //Create role & attach user to it to see account
            var newRole = await foundationService.AddAsync(new Role
            {
                Name = $"{item.Name} Role",
                CreatedOn = DateTimeOffset.UtcNow
            });

            string userId = authorisationBroker.GetUserId();

            await foundationService.AddAsync(new UserRole
            {
                RoleId = newRole.Id,
                UserId = userId
            });

            await foundationService.AddAsync(new AccountRole
            {
                RoleId = newRole.Id,
                AccountId = accountCreated.Id
            });

            return accountCreated;
        }

        public Task DeleteAsync(Account item)
        {
            string userId = authorisationBroker.GetUserId();

            bool exists = foundationService.GetAll<Account>()
                .Any(a => a.Id == item.Id);

            if (!exists)
                throw new SecurityException("Access Denied!");

            //Null out collections
            item.Transactions = null;
            item.AccountSystemReferences = null;
            item.Roles = null;

            return foundationService.DeleteAsync(item);
        }

        public IQueryable<Account> GetAll()
            => foundationService.GetAll<Account>();

        public Task<Account> UpdateAsync(Account item)
        {
            //Null out collections
            item.Transactions = null;
            item.AccountSystemReferences = null;
            item.Roles = null;

            string userId = authorisationBroker.GetUserId();

            bool inRole = foundationService.GetAll<Account>()
                .Any(a => a.Id == item.Id);

            if (!inRole)
                throw new SecurityException("Access Denied!");

            return foundationService.UpdateAsync(item);
        }
    }
}
