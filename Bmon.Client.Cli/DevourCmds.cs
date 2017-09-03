using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        public string TrendingFile { get; set; }

        public DevourCmds()
        {
            IsCommand("devour", "Do consumption of things...");

            HasOption("s|show=", "Show the tranding information.", s => TrendingFile = s);
            HasOption("v|validate=", "Validate the format of the trending information.", v => TrendingFile = v);
            HasAdditionalArguments(1, "<type>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                var fileContents = File.ReadAllText(TrendingFile);

                Console.Out.WriteLine(fileContents);

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
