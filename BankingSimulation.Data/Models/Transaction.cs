﻿using System;

namespace BankingSimulation.Data;

public class Transaction
{
    public Guid Id { get; set; }
    public DateTimeOffset Date { get; set; }
    public TransactionType TransactionType { get;set; }
    public Guid TransactionTypeId { get;set; }
    public string Description { get; set; }
    public double Value { get; set; }
    public double Balance { get; set; }
    public Guid AccountId { get;set; }
    public Account Account { get; set; }
}
