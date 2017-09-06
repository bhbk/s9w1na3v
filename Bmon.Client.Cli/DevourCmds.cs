using Bmon.Client;
using ManyConsole;
using System;
using System.Reflection;
using System.IO;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        private string file = null;
        private bool validate = false, guess = false, show = false;

        public DevourCmds()
        {
            IsCommand("devour", "Do things with a trend file...");

            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref file);
            });
            HasOption("g|guess", "Guess file format.", arg => { guess = true; });
            HasOption("v|validate", "Validate file.", arg => { validate = true; });
            HasOption("s|show", "Show parsed trend data.", arg => { show = true; });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Lib.Devour.DotCsv.GenericFormatA csv = new Lib.Devour.DotCsv.GenericFormatA(file);
                Lib.Models.BmonPostTrendMultiple trends = new Lib.Models.BmonPostTrendMultiple();

                if (guess)
                {

                }

                if (show)
                {
                    csv.Parse(ref trends);
                    Console.WriteLine(trends.ToString());
                }

                if (validate)
                {
                    csv.debug = false;

                    if (csv.TryParse(ref trends))
                        Console.WriteLine("File has a valid format.");
                    else
                        Console.WriteLine("File has an invalid format.");
                }

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                Bmon.Client.Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
