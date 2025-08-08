namespace BankingSimulation.Blazor.Models.Roles
{
    public class RoleViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? EditUrl { get; set; }
        public IEnumerable<string> UserIds { get; set; } = [];
    }
}
