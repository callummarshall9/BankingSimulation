namespace BankingSimulation.BlazorServer.ViewModels.Categories;

public class AddNewCategoryViewModel
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    
    public bool Success { get; set; }
    public Exception? Exception { get; set; }
    public bool Loading { get; set; }
}