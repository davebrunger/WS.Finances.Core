using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using WS.Finances.Core.Lib.Models;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;
using WS.Finances.Core.Lib.ExtensionMethods;

namespace WS.Finances.Core.Console.Commands
{
    public class All : ICommand
    {
        private readonly AccountService _accountService;
        private readonly TransactionService _transactionService;
        private readonly IOutputWriter _outputWriter;

        public All(AccountService accountService, TransactionService transactionService, IOutputWriter outputWriter)
        {
            _accountService = accountService;
            _transactionService = transactionService;
            _outputWriter = outputWriter;
        }

        public string Name => "all";

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;
            string accountName = null;

            var optionSet = new OptionSet {
                {"y|year=", "The year to show transactions for (REQUIRED)", y => year = y.ToInteger()},
                {"m|month=", "The month to show transactions for (REQUIRED)", m => month = m.ToMonthNumber()},
                {"a|account=", "The name of the account to show transactions for (REQUIRED)", a => accountName = a}
            };
            
            var extraParameters = optionSet.Parse(options);

            if (year == null || month == null || string.IsNullOrEmpty(accountName) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(_outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year.Value, month.Value, accountName);
        }

        private void Execute(int year, int month, string accountName)
        {
            var account = _accountService.Get(accountName);
            if (account == null)
            {
                _outputWriter.WriteErrorLine($"Unknown account name: {accountName}");
                return;
            }

            _transactionService.Get(year, month, account.Name)
                .OrderByDescending(t => t.Timestamp)
                .Tabulate(_outputWriter, true, 5);
        }
    }
}
