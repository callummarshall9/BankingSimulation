namespace BankingSimulation.BlazorServer.ViewModels.Categories;

public class CategoryViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ICollection<string> Keywords { get; set; } = [];
    
    public bool Deleting { get; set; }
    public bool Success { get; set; }
    public Exception? Exception { get; set; }
}