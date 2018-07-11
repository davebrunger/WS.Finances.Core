using System.IO;
using System.Text;
using NDesk.Options;
using WS.Utilities.Console;

namespace WS.Finances.Core.Console
{
    public class ErrorTextWriter : TextWriter
    {
        private string line = "";

        private readonly IOutputWriter outputWriter;

        public override Encoding Encoding => Encoding.Default;

        public ErrorTextWriter(IOutputWriter outputWriter)
        {
            this.outputWriter = outputWriter;
        }

        public override void Write(char value)
        {
            line = line + value;
        }

        public override void WriteLine()
        {
            outputWriter.WriteErrorLine(line);
            line = "";
        }

        public override void WriteLine(string line)
        {
            outputWriter.WriteErrorLine(this.line + line);
            line = "";
        }

        public void WriteUsage(string commandName, OptionSet optionSet)
        {
            outputWriter.WriteErrorLine("Usage:");
            outputWriter.WriteErrorLine($"Console {commandName} <options>");
            optionSet.WriteOptionDescriptions(new ErrorTextWriter(outputWriter));
        }
    }
}