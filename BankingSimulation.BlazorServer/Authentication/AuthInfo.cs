using System.Security.Principal;

namespace BankingSimulation.UI;

public class AuthInfo : IIdentity
{
    public string? AuthenticationType { get; set; }

    public bool IsAuthenticated { get; set; }

    public string? Name { get; set; }
}