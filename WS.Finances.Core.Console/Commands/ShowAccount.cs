using System;
using System.Collections.Generic;
using System.Linq;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Console.Tabulation;

namespace WS.Finances.Core.Console.Commands
{
    public class ShowAccounts : ICommand
    {
        private readonly AccountService _accountService;
        private readonly IOutputWriter _outputWriter;

        public string Name => "show-accounts";

        public ShowAccounts(AccountService accountService, IOutputWriter outputWriter)
        {
            _accountService = accountService;
            _outputWriter = outputWriter;
        }

        public Action GetAction(IEnumerable<string> options) => Execute;

        private void Execute()
        {
            _accountService.Get()
                .OrderBy(a => a.Name).Select(a => new {a.Name})
                .Tabulate(_outputWriter, false, 0);
        }
    }
}
