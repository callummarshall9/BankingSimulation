using System;

namespace BankingSimulation.Data;

public class AccountBalanceDTO
{
    public Guid Id { get;set; }
    public double CurrentAccountBalance { get;set; }
    public DateTimeOffset? BalanceAsOf { get;set; }
}
