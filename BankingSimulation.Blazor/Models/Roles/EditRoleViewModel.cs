namespace BankingSimulation.Blazor.Models.Roles
{
    public class EditRoleViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public IList<string> UserIds { get; set; } = [];
    }
}
