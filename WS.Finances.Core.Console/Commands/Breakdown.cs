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
    public class Breakdown : ICommand
    {
        private readonly MapService _mapService;
        private readonly TransactionService _transactionService;
        private readonly IOutputWriter _outputWriter;

        public string Name => "breakdown";

        public Breakdown(MapService mapService, TransactionService transactionService, IOutputWriter outputWriter)
        {
            _mapService = mapService;
            _transactionService = transactionService;
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options)
        {
            int? year = null;
            int? month = null;
            string categoryName = null;

            var optionSet = new OptionSet {
                {"y|year=", "The year to show transactions for (REQUIRED)", y => year = y.ToInteger()},
                {"m|month=", "The the month to show transactions for (REQUIRED)", m => month = m.ToMonthNumber()},
                {"c|category=", "The name of the catergory to show transactions for (REQUIRED)", c => categoryName = c},
            };
            var extraParameters = optionSet.Parse(options);

            if (year == null || month == null || string.IsNullOrEmpty(categoryName) || extraParameters.Count > 0)
            {
                new ErrorTextWriter(_outputWriter).WriteUsage(Name, optionSet);
                return null;
            }

            return () => Execute(year.Value, month.Value, categoryName);
        }

        private void Execute(int year, int month, string categoryName)
        {
            var category = _mapService.Get(categoryName);
            if (category == null)
            {
                _outputWriter.WriteErrorLine($"Unknown category: {categoryName}");
                return;
            }
            if (_transactionService.Get(year, month, unMappedOnly: true).Any())
            {
                _outputWriter.WriteErrorLine("Month contains unmapped transactions");
            }
            _transactionService.Get(year, month, category: category.Category)
                .OrderBy(t => t.Timestamp)
                .Tabulate(_outputWriter, true, 5);
        }
    }
}
