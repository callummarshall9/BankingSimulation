namespace BankingSimulation.BlazorServer.ViewModels.Roles;

public class RoleViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public IEnumerable<string> UserIds { get; set; }
    
    public bool Deleting { get; set; }
    public Exception? Exception { get; set; }
}