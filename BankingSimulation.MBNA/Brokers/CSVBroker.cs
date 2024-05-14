using BankingSimulation.MBNA.Models;
using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BankingSimulation.MBNA.Brokers
{
    internal class CSVBroker : ICSVBroker
    {
        public IEnumerable<MBNATransactionEntry> GetMBNATransactionEntries(string rawData)
        {
            using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(rawData)));
            using var csv = new CsvReader(streamReader, CultureInfo.InvariantCulture);
            return csv.GetRecords<MBNATransactionEntry>().ToArray();
        }
    }
}
