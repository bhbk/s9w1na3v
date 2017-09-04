using System;
using System.IO;

namespace Bmon.Client.Cli
{
    internal class Helpers
    {
        internal static int FileSanityChecks(ref string file)
        {
            if (file == null)
                file = Core.Config.v1_0_0_0.CsvGenericFormatA;

            if (!File.Exists(file))
            {
                Console.WriteLine(string.Format("The file {0} does not exist.", file));
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                return (int)ExitCodes.Failure;
            }

            return (int)ExitCodes.Success;
        }

        internal static int FondFarewell()
        {
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            return (int)ExitCodes.Success;
        }
    }
}
