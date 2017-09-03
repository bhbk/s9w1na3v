using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        public string TrendingFile { get; set; }

        public UploadCmds()
        {
            IsCommand("upload", "Do transport things...");
            HasOption("s|show=", "Show the transport information.", s => TrendingFile = s);
            HasOption("v|validate=", "Validate the transport information.", v => TrendingFile = v);
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
