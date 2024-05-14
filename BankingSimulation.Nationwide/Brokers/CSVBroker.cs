using BankingSimulation.Nationwide.Models;
using CsvHelper;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace BankingSimulation.Nationwide.Brokers
{
    internal class CSVBroker : ICSVBroker
    {
        public IEnumerable<NationwideTransactionEntry> GetNationwideTransactionEntries(string rawData)
        {
            var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                ShouldSkipRecord = (ShouldSkipRecordArgs a) => a.Row[0].Contains("Account Name:") || a.Row[0].Contains("Account Balance:") || a.Row[0].Contains("Available Balance:")
            };

            using var streamReader = new StreamReader(new MemoryStream(Encoding.UTF8.GetBytes(rawData.Replace("£", ""))));
            using var csv = new CsvReader(streamReader, configuration);
            return csv.GetRecords<NationwideTransactionEntry>().ToArray();
        }
        public string GetAccountName(string rawData)
            => rawData.Split("\n").First().Split(",").Last().Replace("\"", "");
    }
}
