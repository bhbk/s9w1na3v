using Bmon.Client.Lib.Models;
using ManyConsole;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Bmon.Client.Cli
{
    public class ConfigCmds : ConsoleCommand
    {
        private Core.Config.v1_0_0_0.DevourConfig devourConfig = new Core.Config.v1_0_0_0.DevourConfig();
        private Core.Config.v1_0_0_0.UploadConfig uploadConfig = new Core.Config.v1_0_0_0.UploadConfig();
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
                    Helpers.DefaultConfig(ref devourConfig);
                    Helpers.DefaultConfig(ref uploadConfig);

                    List<FileToDropboxConfig> dropboxes = uploadConfig.MyDropbox;
                    List<WebApiToBmonConfig> bmons = uploadConfig.MyWebApiToBmon;

                    foreach (LocalFileConfig file in devourConfig.MyLocalFiles)
                    {
                        foreach (FileToDropboxConfig dropbox in dropboxes)
                            file.UploadTo.Add(dropbox.Id);

                        foreach (WebApiToBmonConfig bmon in bmons)
                            file.UploadTo.Add(bmon.Id);
                    }

                    Helpers.WriteConfig(ref devourConfig);
                }
                else if (Read)
                {
                    Helpers.ReadConfig(ref devourConfig);
                    Helpers.ReadConfig(ref uploadConfig);

                    Console.WriteLine(devourConfig.ToString());
                    Console.WriteLine(uploadConfig.ToString());
                }
                else if (Write)
                {
                    Helpers.WriteConfig(ref devourConfig);
                    Helpers.WriteConfig(ref uploadConfig);

                    Console.WriteLine(devourConfig.ToString());
                    Console.WriteLine(uploadConfig.ToString());
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
