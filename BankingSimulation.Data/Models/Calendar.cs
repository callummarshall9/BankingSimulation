using System;
using System.Collections;
using System.Collections.Generic;

namespace BankingSimulation.Data.Models
{
    public class Calendar
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
        public ICollection<CalendarEvent> CalendarEvents { get; set; }
    }
}
