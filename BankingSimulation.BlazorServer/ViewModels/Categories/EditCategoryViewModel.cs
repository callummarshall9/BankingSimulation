namespace BankingSimulation.BlazorServer.ViewModels.Categories;

public class EditCategoryViewModel
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public bool Loading { get; set; }
    public Exception? Exception { get; set; }
}