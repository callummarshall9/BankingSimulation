using System;

namespace BankingSimulation.Data.Models
{
    public class UserRole
    {
        public string UserId { get; set; }
        public Guid RoleId { get; set; }
        public Role Role { get; set; }
    }
}
