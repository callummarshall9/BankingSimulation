using System.Collections.Generic;

namespace BankingSimulation.Data;

public class BankingSystem
{
    public string Id { get;set; }
    public string Description { get; set; }
    public ICollection<AccountBankingSystemReference> AccountSystemReferences { get; set; }
}
