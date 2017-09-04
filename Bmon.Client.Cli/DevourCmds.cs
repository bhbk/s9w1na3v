using ManyConsole;
using System;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        public string csvFile { get; set; }

        public DevourCmds()
        {
            IsCommand("devour", "Do consumption of things...");

            HasOption("s|show=", "Show the tranding information.", s => csvFile = s);
            HasOption("v|validate=", "Validate the format of the trending information.", v => csvFile = v);
            HasAdditionalArguments(1, "<type>");
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
