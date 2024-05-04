using System;
using System.Collections.Generic;

namespace BankingSimulation.Data.Models
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<Category> ChildCategories { get; set; }
        public virtual ICollection<CategoryKeyword> Keywords { get; set; }
        public Guid RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
}
