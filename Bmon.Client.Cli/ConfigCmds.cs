using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        public string ConfFile { get; set; }

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("s|show=", "Show a configuration.", s => ConfFile = s);
            HasOption("v|validate=", "Validate a configuration.", v => ConfFile = v);
            HasAdditionalArguments(1, "<file>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                return (int)ExitCodes.Success;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);

                return (int)ExitCodes.Exception;
            }
        }
    }
}
