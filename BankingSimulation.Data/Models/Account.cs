using System;
using System.Collections.Generic;

namespace BankingSimulation.Data;

public class Account
{
    public Guid Id { get; set; }
    public string Number { get; set; }
    public string Name { get; set; }
    public string FriendlyName { get; set; }
    public ICollection<AccountBankingSystemReference> AccountSystemReferences { get; set; }
    public ICollection<Transaction> Transactions { get;set; }
}
