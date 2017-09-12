using ManyConsole;
using System;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        private string Conf = null;

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("s|show", "Show a configuration.", arg => Conf = arg);
            HasOption("v|validate", "Validate a configuration.", arg => Conf = arg);
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
