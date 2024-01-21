using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using CsvHelper;

namespace BankingSimulation.RBS;

internal class CSVBroker : ICSVBroker
{
    public IEnumerable<RBSTransactionEntry> GetRBSTransactionEntries(string rawData)
    {
        using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(rawData)));
        using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
        return csv.GetRecords<RBSTransactionEntry>().ToArray(); 
    }
}