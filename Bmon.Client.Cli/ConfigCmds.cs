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
        private Core.Config.v1_0_0_0.TriggerConfig triggerConfig = new Core.Config.v1_0_0_0.TriggerConfig();
        private Core.Config.v1_0_0_0.UploadConfig uploadConfig = new Core.Config.v1_0_0_0.UploadConfig();
        private bool Read = false, Write = false, Initialize = false;

        public ConfigCmds()
        {
            IsCommand("config", "Do configuration things...");

            HasOption("i|initialize", "Initialize a configuration.", arg => { Initialize = true; });
            HasOption("r|read", "Read & display a configuration.", arg => { Read = true; });
            HasOption("w|write", "Write a configuration.", arg => { Write = true; });

            //if (Read == false && Write == false && Initialize == false)
            //    throw new ConsoleHelpAsException("stuff");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                if (Initialize)
                {
                    Helpers.DefaultConfig(ref devourConfig);
                    Helpers.DefaultConfig(ref triggerConfig);
                    Helpers.DefaultConfig(ref uploadConfig);

                    List<PostFileToBmonConfig> basFiles = uploadConfig.MyPostFileToBmon;
                    List<PostFileToDropboxConfig> dropboxes = uploadConfig.MyPostFileToDropbox;
                    List<PostJsonToBmonConfig> bmonMoments = uploadConfig.MyPostJsonToBmon;
                    List<TriggerModel> uploadTriggers = triggerConfig.MyTriggers;

                    foreach (DevourModel devour in devourConfig.MyLocalFiles)
                    {
                        foreach (PostFileToBmonConfig file in basFiles)
                            devour.UploadTo.Add(file.Id);

                        foreach (PostFileToDropboxConfig file in dropboxes)
                            devour.UploadTo.Add(file.Id);

                        foreach (PostJsonToBmonConfig json in bmonMoments)
                            devour.UploadTo.Add(json.Id);

                        foreach (TriggerModel trigger in uploadTriggers)
                            devour.TriggerOn.Add(trigger.Id);
                    }

                    Helpers.WriteConfig(ref devourConfig);
                    Helpers.WriteConfig(ref triggerConfig);
                    Helpers.WriteConfig(ref uploadConfig);
                }
                else if (Read)
                {
                    Helpers.ReadConfig(ref devourConfig);
                    Helpers.ReadConfig(ref triggerConfig);
                    Helpers.ReadConfig(ref uploadConfig);

                    Console.WriteLine(devourConfig.ToString());
                    Console.WriteLine(triggerConfig.ToString());
                    Console.WriteLine(uploadConfig.ToString());
                }
                else if (Write)
                {
                    Helpers.WriteConfig(ref devourConfig);
                    Helpers.WriteConfig(ref triggerConfig);
                    Helpers.WriteConfig(ref uploadConfig);

                    Console.WriteLine(devourConfig.ToString());
                    Console.WriteLine(triggerConfig.ToString());
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
