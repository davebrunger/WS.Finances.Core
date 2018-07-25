using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using WS.Finances.Core.Console.Commands;
using WS.Finances.Core.Lib.Data;
using WS.Finances.Core.Lib.Services;
using WS.Utilities.Console;
using WS.Utilities.Injection;
using static System.Console;

namespace WS.Finances.Core.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            WriteLine("---------");
            WriteLine($"Number of arguments: {args.Length}");
            WriteLine();
            foreach(var arg in args)
            {
                WriteLine(arg);
            }
            WriteLine("---------");
            try
            {
                var appsettings = LoadConfiguration();
                var container = ConfigureInjectionContainer(appsettings);
                var comamnds = container.Resolve<IReadOnlyCollection<ICommand>>();
                var action = GetAction(comamnds, args);
                if (action == null)
                {
                    return;
                }
                action();
            }
            catch (Exception e)
            {
                PrintError(e.ToString());
            }
            finally
            {
                if (Debugger.IsAttached)
                {
                    WriteLine();
                    WriteLine("Done");
                    ReadLine();
                }
            }
        }

        private static void PrintError(string message)
        {
            var foregroundColour = ForegroundColor;
            ForegroundColor = ConsoleColor.Red;
            WriteLine(message);
            ForegroundColor = foregroundColour;
        }

        private static void PrintUsage(IReadOnlyCollection<ICommand> commands)
        {
            PrintError("Usage:");
            PrintError("Console command [command-options]");
            PrintError("Available Commands:");
            foreach (var command in commands.OrderBy(c => c.Name))
            {
                PrintError(command.Name);
            }
        }

        private static AppSettings LoadConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var appsettings = new AppSettings();
            builder.Build().GetSection("AppSettings").Bind(appsettings);
            return appsettings;
        }

        private static BasicInjectionContainer ConfigureInjectionContainer(AppSettings appsettings)
        {
            var container = new BasicInjectionContainer();

            container.RegisterInstance(appsettings);
            container.RegisterType<IOutputWriter, ConsoleOutputWriter>();
            container.RegisterType<IBaseDirectoryProvider, BaseDirectoryProvider>();
            container.RegisterType<RepositoryFactory>();
            container.RegisterType<AccountService>();
            container.RegisterType<CsvService>();
            container.RegisterType<MapService>();
            container.RegisterType<SummaryService>();
            container.RegisterType<TransactionService>();

            var commands = new List<ICommand>();
            var commandTypes = typeof(Program).GetTypeInfo().Assembly.DefinedTypes
                .Where(t => (typeof(ICommand)).GetTypeInfo().IsAssignableFrom(t))
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => t.AsType())
                .ToList();
            foreach (var commandType in commandTypes)
            {
                container.RegisterType(commandType, commandType);
                var command = container.Resolve(commandType) as ICommand;
                commands.Add(command);
            }

            container.RegisterInstance<IReadOnlyCollection<ICommand>>(commands.AsReadOnly());

            return container;
        }

        private static Action GetAction(IReadOnlyCollection<ICommand> commands, string[] args)
        {
            var commandLookup = commands.ToDictionary(c => c.Name, c => c, StringComparer.OrdinalIgnoreCase);
            if (args.Length < 1 || !commandLookup.ContainsKey(args[0]))
            {
                PrintUsage(commands);
                return null;
            }
            return commandLookup[args[0]].GetAction(args.Skip(1));
        }
    }
}
