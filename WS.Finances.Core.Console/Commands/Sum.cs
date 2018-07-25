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
    public class Sum : ICommand
    {
        private readonly SummaryService _summaryService;
        private readonly TransactionService _transactionService;
        private readonly IOutputWriter _outputWriter;

        public string Name => "sum";

        public Sum(SummaryService summaryService, TransactionService transactionService, IOutputWriter outputWriter)
        {
            _summaryService = summaryService;
            _transactionService = transactionService;
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;

            var optionSet = new OptionSet {
                {"y|year=", "The year to show the summary for (REQUIRED)", y => year = y.ToInteger()},
                {"m|month=", "The month to show the summary for (REQUIRED)", m => month = m.ToMonthNumber()}
            };
            var extraParameters = optionSet.Parse(options);

            if (year == null || month == null || extraParameters.Count > 0)
            {
                new ErrorTextWriter(_outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year.Value, month.Value);
        }

        public void Execute(int year, int month)
        {
            if (_transactionService.Get(year, month, unMappedOnly: true).Any())
            {
                _outputWriter.WriteErrorLine("Month contains unmapped transactions");
            }
            _summaryService.Get(year, month)
                .Select(m => new
                {
                    m.Key.Section,
                    m.Key.Position,
                    m.Key.Category,
                    Total = m.Value
                })
                .OrderByDescending(g => g.Section)
                .ThenBy(g => g.Position)
                .ThenBy(g => g.Category)
                .Tabulate(_outputWriter, true, 5, new TotalColumnNames("Section", "Total"));
        }
    }
}
