using Bmon.Client;
using ManyConsole;
using System;
using System.Reflection;
using System.IO;

namespace Bmon.Client.Cli
{
    public class DevourCmds : ConsoleCommand
    {
        private string file;

        public DevourCmds()
        {
            IsCommand("devour", "Do consumption of things...");

            HasOption("s|show=", "Show the tranding information.", s => file = s);
            HasOption("v|validate=", "Validate the format of the trending information.", v => file = v);
            HasAdditionalArguments(1, "<type>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Helpers.FileSanityChecks(ref file);

                Lib.Devour.DotCsv.GenericFormatA csv = new Lib.Devour.DotCsv.GenericFormatA(file);
                Lib.Models.BmonPostTrendMultiple trends = csv.ParseTrends();

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                Bmon.Client.Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);

                return (int)ExitCodes.Exception;
            }
        }
    }
}
