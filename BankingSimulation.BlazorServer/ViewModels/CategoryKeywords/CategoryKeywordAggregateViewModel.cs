namespace BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;

public class CategoryKeywordAggregateViewModel
{
    public Guid CategoryId { get; set; }
    public ICollection<CategoryKeywordViewModel> Keywords { get; set; } = [];
}