using System;
using System.Collections.Generic;
using System.Linq;
using NDesk.Options;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

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

            var optionSet = new OptionSet {
                {"y|year=", y => year = int.Parse(y)},
                {"m|month=", m => month = int.Parse(m)},
                {"a|account=", a => accountName = a},
                {"c|category=", c => categoryName = c},
                {"d|descriptionPattern=", d => descriptionPattern = d},
                {"u|unmappedOnly=", u => unMappedOnly = bool.Parse(u)},
                {"t|transactionId=", t => transactionId = int.Parse(t)},
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
