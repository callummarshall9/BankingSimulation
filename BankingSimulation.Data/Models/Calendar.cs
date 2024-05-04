using System;

namespace BankingSimulation.Data.Models
{
    public class Calendar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
