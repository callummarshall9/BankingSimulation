using BankingSimulation.MBNA.Models;
using System.Collections.Generic;

namespace BankingSimulation.MBNA.Brokers
{
    internal interface ICSVBroker
    {
        IEnumerable<MBNATransactionEntry> GetMBNATransactionEntries(string rawData);
    }
}