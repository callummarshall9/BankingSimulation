using System.Security.Principal;

namespace BankingSimulation.Blazor
{
    public class DefaultAuthInfo : IIdentity
    {
        public string? AuthenticationType => "Default";

        public bool IsAuthenticated => true;

        public string? Name => "Guest";
    }
}
