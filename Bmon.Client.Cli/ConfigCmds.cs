using ManyConsole;
using System;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        private string confFile { get; set; }

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("s|show=", "Show a configuration.", s => confFile = s);
            HasOption("v|validate=", "Validate a configuration.", v => confFile = v);
            HasAdditionalArguments(1, "<file>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
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
