using System;

namespace BankingSimulation.Data;

public class AccountBankingSystemReference
{
    public string BankingSystemId { get; set; }
    public BankingSystem BankingSystem { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
}