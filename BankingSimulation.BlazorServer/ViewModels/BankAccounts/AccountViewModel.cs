namespace BankingSimulation.UI.ViewModels;

public class AccountViewModel
{
    public string? Name { get; set; }
    public string? FriendlyName { get; set; }
    public string? Number { get; set; }
    public ICollection<string> SystemReferences { get; set; } = [];
}