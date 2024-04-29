using System;

namespace BankingSimulation.Data.Models
{
    public class CategoryKeyword
    {
        public Guid Id { get; set; }
        public string Keyword { get; set; }
        public Guid CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
