using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;
using WS.Finances.Core.Lib.ExtensionMethods;

namespace WS.Finances.Core.Console.Commands
{
    public class Search : ICommand
    {
        private readonly TransactionService transactionService;

        private readonly IOutputWriter outputWriter;

        public Search(TransactionService transactionService, IOutputWriter outputWriter)
        {
            this.transactionService = transactionService;
            this.outputWriter = outputWriter;
        }

        public string Name => "search";

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;
            string accountName = null;
            string categoryName = null;
            string descriptionPattern = null;
            var unMappedOnly = false;
            int? transactionId = null;
            bool ignoreCase = false;

            var optionSet = new OptionSet {
                {"y|year=", "The year in which to search for transactions (OPTIONAL)", y => year = y.ToInteger()},
                {"m|month=", "The month in which to search for transactions (OPTIONAL)", m => month = m.ToMonthNumber()},
                {"a|account=", "The account in which to search for transactions (OPTIONAL)", a => accountName = a},
                {"c|category=", "The category in which to search for transactions (OPTIONAL)", c => categoryName = c},
                {"d|descriptionPattern=", "A regular expression pattern used to match transaction descriptions (OPTIONAL)", d => descriptionPattern = d},
                {"u|unmappedOnly", "Only search for unmapped subscriptions (OPTIONAL)", u => unMappedOnly = u != null},
                {"t|transactionId=", "A transaction ID to search for (OPTIONAL)", t => transactionId = t.ToInteger()},
                {"i|ignoreCase=", "Set to true to ignore the case of the description pattern (OPTIONAL)", i => ignoreCase = i.ToBoolean() ?? false},
            };
            var extraParameters = optionSet.Parse(options);

            if (string.IsNullOrEmpty(descriptionPattern) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year, month, accountName, categoryName, descriptionPattern, unMappedOnly, transactionId);
        }

        private void Execute(int? year, int? month, string accountName, string categoryName, string descriptionPattern, 
            bool unMappedOnly, int? transactionId)
        {
            transactionService.Get(year, month, accountName, categoryName, descriptionPattern, unMappedOnly, transactionId)
                .OrderByDescending(t => t.Timestamp)
                .Tabulate(outputWriter, true, 5);
        }
    }
}
