using ManyConsole;
using System;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        private string conf = null;

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("s|show=", "Show a configuration.", arg => conf = arg);
            HasOption("v|validate=", "Validate a configuration.", arg => conf = arg);
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
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
