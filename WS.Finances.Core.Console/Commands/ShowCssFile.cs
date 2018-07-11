using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

namespace WS.Finances.Core.Console.Commands
{
    public class ShowCssFile : ICommand
    {
        private readonly AccountService _accountService;
        private readonly CsvService _csvService;
        private readonly IOutputWriter _outputWriter;

        public ShowCssFile(AccountService accountService, CsvService csvService, IOutputWriter outputWriter)
        {
            _accountService = accountService;
            _csvService = csvService;
            _outputWriter = outputWriter;
        }

        public string Name => "show-css-file";

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;
            string accountName = null;
            string filePath = null;

            var optionSet = new OptionSet {
                {"y|year=", y => year = int.Parse(y)},
                {"m|month=", m => month = int.Parse(m)},
                {"a|account=", a => accountName = a},
                {"f|filePath=", f => filePath = f}
            };
            var extraParameters = optionSet.Parse(options);

            if (year == null || month == null || string.IsNullOrEmpty(accountName) || string.IsNullOrEmpty(filePath) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(_outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year.Value, month.Value, accountName, filePath);
        }

        private void Execute(int year, int month, string accountName, string filePath)
        {
            var account = _accountService.Get(accountName);
            if (account == null)
            {
                _outputWriter.WriteErrorLine($"Unknown account name: {accountName}");
                return;
            }

            _csvService.GetCsvData(year, month, account.Name, filePath)
                .Tabulate(_outputWriter, true, 0);
        }
    }
}
