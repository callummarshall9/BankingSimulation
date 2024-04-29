using System;
using System.Collections.Generic;

namespace BankingSimulation.Data.Models
{
    public class Role
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<AccountRole> Accounts { get; set; }
    }
}
