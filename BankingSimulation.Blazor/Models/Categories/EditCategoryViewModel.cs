namespace BankingSimulation.Blazor.Models.Categories
{
    public class EditCategoryViewModel
    {
        public Guid Id { get; set; }
        public Guid RoleId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IList<string> Keywords { get; set; } = [];
    }
}
