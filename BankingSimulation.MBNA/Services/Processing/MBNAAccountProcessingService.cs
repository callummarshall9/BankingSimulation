using BankingSimulation.Data;
using BankingSimulation.Data.Brokers;
using System.Collections.Generic;

namespace BankingSimulation.MBNA.Services.Processing
{
    internal class MBNAAccountProcessingService(IAuthorisationBroker authorisationBroker) : IMBNAAccountProcessingService
    {
        private readonly IAuthorisationBroker authorisationBroker = authorisationBroker;

        public IEnumerable<Account> ParseAccounts()
            => [
                    new Account
                    {
                        Name = $"MBNA-" + authorisationBroker.GetUserId(),
                        Number = $"MBNA-" + authorisationBroker.GetUserId(),
                        AccountSystemReferences = [
                                new AccountBankingSystemReference() { BankingSystem = new BankingSystem { Id = "BarclaysCard", Description = "BarclaysCard" } }
                            ]
                    }
                ];
    }
}
