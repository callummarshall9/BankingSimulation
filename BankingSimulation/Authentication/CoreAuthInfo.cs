using System.Security.Principal;

namespace BankingSimulation.Authentication
{
    public class CoreAuthInfo : IIdentity
    {
        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }

        public string Name { get; set; }
    }
}
