using BankingSimulation.BarclaysCard.Models;
using System.Collections.Generic;

namespace BankingSimulation.BarclaysCard.Brokers
{
    internal interface ICSVBroker
    {
        IEnumerable<BarclaysCardTransactionEntry> GetBarclaysCardTransactionEntries(string rawData);
    }
}