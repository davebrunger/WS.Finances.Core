using System;
using System.Collections.Generic;
using System.Linq;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

namespace WS.Finances.Core.Console.Commands
{
    public class ShowMap : ICommand
    {
        private readonly MapService _mapService;
        private readonly IOutputWriter _outputWriter;

        public string Name => "show-map";

        public ShowMap(MapService mapService, IOutputWriter outputWriter)
        {
            _mapService = mapService;
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options) => Execute;

        private void Execute()
        {
            _mapService.Get()
                .OrderByDescending(m => m.Section)
                .ThenBy(m => m.Position)
                .ThenBy(m => m.Category)
                .Select(m =>
                {
                    string patterns;
                    switch (m.Patterns.Count())
                    {
                        case 0:
                            patterns = string.Empty;
                            break;
                        case 1:
                            patterns = m.Patterns.First();
                            break;
                        default:
                            patterns = string.Join(", ", m.Patterns.Select(p => $@"""{p}"""));
                            break;

                    }
                    if (patterns.Length > 100)
                    {
                        patterns = $"{patterns.Substring(0, 97)}...";
                    }
                    return new
                    {
                        m.Section,
                        m.Position,
                        m.Category,
                        Patterns = patterns
                    };
                })
                .Tabulate(_outputWriter, true, 0);
        }
    }
}
