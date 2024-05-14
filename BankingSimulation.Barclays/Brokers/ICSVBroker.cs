using BankingSimulation.Barclays.Models;
using System.Collections.Generic;

namespace BankingSimulation.Barclays.Brokers
{
    public interface ICSVBroker
    {
        IEnumerable<BarclaysTransactionEntry> GetBarclaysTransactionEntries(string rawData);
    }
}