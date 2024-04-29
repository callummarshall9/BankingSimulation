using System.Security.Claims;

namespace BankingSimulation.Data.Brokers
{
    public class AuthorisationBroker(ClaimsPrincipal principal) : IAuthorisationBroker
    {
        public string GetUserId()
            => principal?.Identity?.Name;
    }
}
