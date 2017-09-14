using ManyConsole;
using System;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        private Core.Config.v1_0_0_0.UploadConfig uploadConfig = new Core.Config.v1_0_0_0.UploadConfig();
        private Core.Config.v1_0_0_0.DevourConfig devourConfig = new Core.Config.v1_0_0_0.DevourConfig();
        private bool Read = false, Write = false, Initialize = false;

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("i|initialize", "Initialize a configuration.", arg => { Initialize = true; });
            HasOption("r|read", "Read & display a configuration.", arg => { Read = true; });
            HasOption("w|write", "Write a configuration.", arg => { Write = true; });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (Initialize)
                {
                    Helpers.InitializeConfig(ref uploadConfig);
                    //Console.WriteLine(uploadConfig.ToString());
                }
                else if (Read)
                {
                    Helpers.ReadConfig(ref uploadConfig);
                    //Console.WriteLine(uploadConfig.ToString());
                }
                else if (Write)
                {
                    Helpers.WriteConfig(ref uploadConfig);
                    //Console.WriteLine(uploadConfig.ToString());
                }

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
