using System.Collections.Generic;

namespace BankingSimulation.RBS;

internal interface ICSVBroker
{
    IEnumerable<RBSTransactionEntry> GetRBSTransactionEntries(string rawData);
}
