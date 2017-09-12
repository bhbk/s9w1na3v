using Bmon.Client.Lib.Models;
using Bmon.Client.Lib.Transport;
using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Xml.Serialization;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        private UploadMethods Decide;
        private string InputFile = null;
        private string ConfigFile = "UploadConfig.xml";

        public UploadCmds()
        {
            IsCommand("upload", "Do upload things...");
            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref InputFile);
            });
            HasRequiredOption("u|upload-type=", "Type of upload to perform.", arg =>
            {
                if (arg.ToLower() == UploadMethods.FileToDropbox.ToString().ToLower())
                    Decide = UploadMethods.FileToDropbox;

                else if (arg.ToLower() == UploadMethods.FileViaFtp.ToString().ToLower())
                    Decide = UploadMethods.FileViaFtp;

                else if (arg.ToLower() == UploadMethods.FileViaSftp.ToString().ToLower())
                    Decide = UploadMethods.FileViaSftp;

                else if (arg.ToLower() == UploadMethods.FileViaTftp.ToString().ToLower())
                    Decide = UploadMethods.FileViaTftp;

                else if (arg.ToLower() == UploadMethods.WebApiToBmon.ToString().ToLower())
                    Decide = UploadMethods.WebApiToBmon;

                else if (arg.ToLower() == UploadMethods.DoNothing.ToString().ToLower())
                    Decide = UploadMethods.DoNothing;

                else
                    throw new ConsoleHelpAsException("Invalid upload type...");
            });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Core.Config.v1_0_0_0.UploadConfig MyConfigs = new Core.Config.v1_0_0_0.UploadConfig();
                MyConfigs.MyDropbox.Add(new FileToDropboxConfig("12345", "/"));
                MyConfigs.MyFtp.Add(new FileViaFtpConfig("https://bmon.ahfc.us", new NetworkCredential("username", "password"), "/"));
                MyConfigs.MySftp.Add(new FileViaSftpConfig("https://bmon.ahfc.us", 22, new NetworkCredential("username", "password"), "/"));
                MyConfigs.MyTftp.Add(new FileViaTftpConfig("https://bmon.ahfc.us", "/"));
                MyConfigs.MyWebApiToBmon.Add(new WebApiToBmonConfig("https://bmon.ahfc.us", "/readingdb/reading/store/", "12345678"));

                XmlSerializer x = new XmlSerializer(MyConfigs.GetType());
                StreamWriter writer = new StreamWriter(ConfigFile);
                x.Serialize(writer, MyConfigs);

                string localPath = new FileInfo(InputFile).DirectoryName;
                string localName = new FileInfo(InputFile).Name;
                string remoteName = localName;
                dynamic decision = null;

                switch (Decide)
                {
                    case UploadMethods.FileToDropbox:
                        {
                            foreach (var config in MyConfigs.MyDropbox)
                            {
                                decision = new Lib.Transport.Vendor.Dropbox(config.Token);
                                decision.UploadFileAsync(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.FileViaFtp:
                        {
                            foreach (var config in MyConfigs.MyFtp)
                            {
                                decision = new Lib.Transport.Generic.Ftp(new Uri(config.Server), config.Credential);
                                decision.UploadFileAsync(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.FileViaSftp:
                        {
                            foreach (var config in MyConfigs.MySftp)
                            {
                                decision = new Lib.Transport.Generic.Sftp(new Uri(config.Server), config.Port, config.Credential);
                                decision.UploadFile(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.FileViaTftp:
                        {
                            foreach (var config in MyConfigs.MyTftp)
                            {
                                decision = new Lib.Transport.Generic.Tftp(new Uri(config.Server));
                                decision.UploadFile(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.WebApiToBmon:
                        {
                            Lib.Devour.DotCsv.GenericFormatA raw = new Lib.Devour.DotCsv.GenericFormatA(InputFile);
                            MomentTuples momentTuples = new MomentTuples();
                            MomentArrays momentArrays = new MomentArrays();
                            raw.Parse(ref momentTuples);

                            Console.WriteLine(raw.Output);

                            foreach (Tuple<double, string, double> t in momentTuples.Readings)
                                momentArrays.Readings.Add(new List<string>() { t.Item1.ToString(), t.Item2.ToString(), t.Item3.ToString() });

                            foreach (var config in MyConfigs.MyWebApiToBmon)
                            {
                                decision = new Lib.Transport.Vendor.Bmon(new Uri(config.Server), config.StoreKey);
                                decision.PostAsync(config.Path, momentArrays);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;
                    case UploadMethods.DoNothing:
                        break;

                    default:
                        throw new InvalidOperationException();
                }

                return Helpers.FondFarewell();
            }
            catch (Exception ex)
            {
                Core.Echo.Proxy.Caught.Msg(Assembly.GetExecutingAssembly().GetName().Name, MethodBase.GetCurrentMethod().ToString(), ex);
                return Helpers.AngryFarewell(ex);
            }
        }
    }
}
