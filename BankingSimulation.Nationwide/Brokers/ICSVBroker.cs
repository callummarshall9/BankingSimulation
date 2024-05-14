using BankingSimulation.Nationwide.Models;
using System.Collections.Generic;

namespace BankingSimulation.Nationwide.Brokers
{
    internal interface ICSVBroker
    {
        IEnumerable<NationwideTransactionEntry> GetNationwideTransactionEntries(string rawData);
        string GetAccountName(string rawData);
    }
}