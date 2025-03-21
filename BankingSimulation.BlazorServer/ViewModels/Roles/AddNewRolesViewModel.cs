namespace BankingSimulation.BlazorServer.ViewModels.Roles;

public class AddNewRolesViewModel
{
    public string? Name { get; set; }
    
    public bool Success { get; set; }
    public Exception? Exception { get; set; }
    public bool Loading { get; set; }
}