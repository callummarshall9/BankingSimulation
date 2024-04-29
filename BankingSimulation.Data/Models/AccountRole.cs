using System;

namespace BankingSimulation.Data.Models
{
    public class AccountRole
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }

        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
