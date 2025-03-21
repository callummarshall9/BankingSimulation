namespace BankingSimulation.BlazorServer.ViewModels.CategoryKeywords;

public class CategoryKeywordViewModel
{
    public Guid Id { get; set; }
    public string? Keyword { get; set; }
    
    public bool Deleting { get; set; }
    public Exception? Exception { get; set; }
}