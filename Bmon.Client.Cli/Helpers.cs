using ManyConsole;
using System;
using System.IO;

namespace Bmon.Client.Cli
{
    internal class Helpers
    {
        internal static void FileSanityChecks(ref string arg, ref string file)
        {
            if (arg == string.Empty)
                throw new ConsoleHelpAsException(string.Format("No file name was given.", arg));

            else if (!File.Exists(arg))
                throw new ConsoleHelpAsException(string.Format("The file {0} does not exist.", arg));

            else
                file = arg;
        }

        internal static int FondFarewell()
        {
            //Console.Error.WriteLine();
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            return (int)ExitCodes.Success;
        }

        internal static int AngryFarewell(Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            //Console.Error.WriteLine();
            //Console.Error.WriteLine("Press any key to exit...");
            //Console.ReadKey();

            return (int)ExitCodes.Exception;
        }
    }
}
