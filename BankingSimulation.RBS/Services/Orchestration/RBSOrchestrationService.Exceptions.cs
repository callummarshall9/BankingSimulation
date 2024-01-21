using System;
using System.Collections.Generic;

namespace BankingSimulation.RBS;

internal partial class RBSOrchestrationService
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
