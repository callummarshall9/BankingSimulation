namespace BankingSimulation.Blazor.Models.Categories
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? EditUrl { get; set; }
        public IEnumerable<string> Keywords { get; set; } = [];
    }
}
