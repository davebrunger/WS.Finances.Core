using System;
using System.Collections.Generic;
using WS.Utilities.Console;

namespace WS.Finances.Core.Console.Commands
{
    public class About : ICommand
    {
        private readonly IOutputWriter _outputWriter;

        public string Name => "about";

        public About(IOutputWriter outputWriter)
        {
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options) => Execute;

        private void Execute()
        {
            _outputWriter.WriteLine("WS.Finances.Console by David Brunger");
        }
    }
}
