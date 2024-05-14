using BankingSimulation.BarclaysCard.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BankingSimulation.BarclaysCard.Brokers
{
    internal class CSVBroker : ICSVBroker
    {
        public IEnumerable<BarclaysCardTransactionEntry> GetBarclaysCardTransactionEntries(string rawData)
        {
            using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(rawData)));

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false
            };

            using var csv = new CsvReader(streamReader, config);
            return csv.GetRecords<BarclaysCardTransactionEntry>().ToArray();
        }
    }
}