using System.Collections.Generic;
using BankingSimulation.Data;

namespace BankingSimulation.RBS;

internal interface IRBSTransactionProcessingService
{
    IEnumerable<Transaction> ParseTransactions(string rawData);
}
