using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using WS.Finances.Core.Lib.Models;
using WS.Utilities.Csv;

namespace WS.Finances.Core.Lib.Services
{
    public class CsvService
    {
        private readonly AccountService _accountService;
        private readonly MapService _mapService;

        public CsvService(AccountService accountService, MapService mapService)
        {
            _accountService = accountService;
            _mapService = mapService;
        }

        public IEnumerable<Transaction> GetCsvData(int year, int month, string accountName, string filename,
            bool applyMap = false)
        {
            return GetCsvData(year, month, accountName, new FileEnumerable(filename), filename, applyMap);
        }

        public IEnumerable<Transaction> GetCsvData(int year, int month, string accountName, Stream data, string sourceFilename,
            bool applyMap = false)
        {
            return GetCsvData(year, month, accountName, new StreamEnumerable(data), sourceFilename, applyMap);
        }

        public IEnumerable<Transaction> GetCsvData(int year, int month, string accountName, IEnumerable<char> data, string sourceFilename,
            bool applyMap = false)
        {
            var account = _accountService.Get(accountName);
            if (account == null)
            {
                throw new ArgumentException($"{accountName} does not identify an account", nameof(accountName));
            }
            var tokens = new CsvTokens(data);
            var records = new CsvRecords(tokens);
            Func<string, string> mapDescriptionToCategory;
            if (applyMap)
            {
                var map = _mapService.Get().ToDictionary(m => m.Category, m => m.Patterns.Select(p => new Regex(p)).ToList());
                mapDescriptionToCategory = s => MapDescription(map, s);
            }
            else
            {
                mapDescriptionToCategory = s => null;
            }
            var transactionParser = account.GetTransactionParser(year, month, Path.GetFileName(sourceFilename),
                mapDescriptionToCategory);
            return new CsvData<Transaction>(records, transactionParser);
        }

        private static string MapDescription(Dictionary<string, List<Regex>> map, string description)
        {
            return map
                .Where(m => m.Value.Any(r => r.IsMatch(description)))
                .Select(m => m.Key)
                .FirstOrDefault();
        }
    }
}
