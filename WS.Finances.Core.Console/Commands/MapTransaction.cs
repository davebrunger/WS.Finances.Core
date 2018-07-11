using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NDesk.Options;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

namespace WS.Finances.Core.Console.Commands
{
    public class MapTransaction : ICommand
    {
        private readonly AccountService _accountService;
        private readonly MapService _mapService;
        private readonly TransactionService _transactionService;
        private readonly IOutputWriter _outputWriter;

        public string Name => "map-transaction";

        public MapTransaction(AccountService accountService, MapService mapService, TransactionService transactionService, IOutputWriter outputWriter)
        {
            _accountService = accountService;
            _mapService = mapService;
            _transactionService = transactionService;
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;
            string accountName = null;
            string descriptionPattern = null;
            string categoryName = null;

            var optionSet = new OptionSet {
                {"y|year=", y => year = int.Parse(y)},
                {"m|month=", m => month = int.Parse(m)},
                {"a|account=", a => accountName = a},
                {"d|descriptionPattern=", d => descriptionPattern = d},
                {"c|category=", c => categoryName = c}
            };
            var extraParameters = optionSet.Parse(options);

            if (year == null || month == null || string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(descriptionPattern) || string.IsNullOrEmpty(categoryName) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(_outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year.Value, month.Value, accountName, descriptionPattern, categoryName);
        }

        private void Execute(int year, int month, string accountName, string descriptionPattern, string categoryName)
        {
            var account = _accountService.Get(accountName);
            if (account == null)
            {
                _outputWriter.WriteErrorLine($"Unknown account name: {accountName}");
                return;
            }
            var category = _mapService.Get(categoryName);
            if (category == null)
            {
                _outputWriter.WriteErrorLine($"Unknown category: {categoryName}");
                return;
            }
            var fileName = $@"Data\{year}-{month}-{account.Name}.json";
            if (!File.Exists(fileName))
            {
                _outputWriter.WriteErrorLine($"Cannot find data file: {fileName}");
            }

            var transactionsToMap = _transactionService.Get(year, month, account.Name, descriptionPattern: descriptionPattern).ToList();
            foreach (var transaction in transactionsToMap)
            {
                transaction.Category = category.Category;
                _transactionService.Put(transaction);
            }
            _outputWriter.WriteLine($"Mapped {transactionsToMap.Count} Transaction{(transactionsToMap.Count == 1 ? "" : "s")}");
            _outputWriter.WriteLine("");
            _outputWriter.WriteLine("Unmapped Transactions:");
            _outputWriter.WriteLine("");
            _transactionService.Get(year, month, account.Name, unMappedOnly: true)
                .Tabulate(_outputWriter, true, 0);
        }
    }
}
