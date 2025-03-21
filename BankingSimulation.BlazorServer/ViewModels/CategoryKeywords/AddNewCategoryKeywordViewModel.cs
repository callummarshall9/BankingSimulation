namespace BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;

public class AddNewCategoryKeywordViewModel
{
    public Guid CategoryId { get; set; }
    public string? Keyword { get; set; }
    
    public bool Loading { get; set; }
    public Exception? Exception { get; set; }
    public bool Success { get; set; }
}