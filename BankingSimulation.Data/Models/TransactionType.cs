using System;
using System.Collections.Generic;

namespace BankingSimulation.Data;

public class TransactionType
{
    public Guid Id { get; set; }
    public string SystemId { get; set; }
    public string TypeId { get; set; }
    public string FriendlyName { get; set; }
    public ICollection<Transaction> Transactions { get;set; }
}
