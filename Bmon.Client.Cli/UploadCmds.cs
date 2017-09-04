using ManyConsole;
using System;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        public string csvFile { get; set; }

        public UploadCmds()
        {
            IsCommand("upload", "Do transport things...");
            HasOption("s|show=", "Show the transport information.", s => csvFile = s);
            HasOption("v|validate=", "Validate the transport information.", v => csvFile = v);
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
