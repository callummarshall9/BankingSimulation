using System;
using System.Collections.Generic;

namespace BankingSimulation.Barclays.Services.Orchestration
{
    internal partial class BarclaysOrchestrationService
    {
        public class AccountNumberException : Exception
        {
            public AccountNumberException(IEnumerable<string> accountNumbers)
                : base(message: $"Account numbers {string.Join(',', accountNumbers)} missing")
            {
                this.Data.Add("accountIds", accountNumbers);
            }
        }
    }
}
