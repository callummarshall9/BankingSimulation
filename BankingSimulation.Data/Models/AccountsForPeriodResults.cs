using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankingSimulation.Data.Models
{
    public class AccountsForPeriodResultsDTO
    {
        public DateOnly From { get; set; }
        public DateOnly To { get; set; }
        public List<AccountsForPeriodResults> Results { get; set; }
    }

    public class AccountsForPeriodResults
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? RoleId { get; set; }
        public double Sum { get; set; }
        public IEnumerable<CategoryKeyword> Keywords { get; set; }
    }
}
