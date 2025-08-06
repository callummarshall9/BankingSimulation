namespace BankingSimulation.Blazor.Models.Accounts
{
    public class AccountViewModel
    {
        public string DisplayName { get; set; } = null!;
        public string Number { get; set; } = null!;
        public IEnumerable<string> AccountSystemReferences { get; set; } = [];
    }
}