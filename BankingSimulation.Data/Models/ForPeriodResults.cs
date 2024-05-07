using System;
using System.Collections.Generic;

namespace BankingSimulation.Data.Models
{
    public class ForPeriodResultsDTO
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public List<ForPeriodResults> Results { get; set; }
    }

    public class ForPeriodResults
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid RoleId { get; set; }
        public double Sum { get; set; }
        public IEnumerable<CategoryKeyword> Keywords { get; set; }
    }
}
