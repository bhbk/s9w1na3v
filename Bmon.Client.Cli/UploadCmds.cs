using Bmon.Client.Lib.Models;
using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        private Core.Config.v1_0_0_0.DevourConfig devourConfig = new Core.Config.v1_0_0_0.DevourConfig();
        private Core.Config.v1_0_0_0.UploadConfig uploadConfig = new Core.Config.v1_0_0_0.UploadConfig();
        private UploadMethods Decide;
        private string InputFile = null;

        public UploadCmds()
        {
            IsCommand("upload", "Do upload things...");
            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref InputFile);
            });
            HasRequiredOption("u|upload-type=", "Type of upload to perform.", arg =>
            {
                if (arg.ToLower() == UploadMethods.FileViaFtp.ToString().ToLower())
                    Decide = UploadMethods.FileViaFtp;

                else if (arg.ToLower() == UploadMethods.FileViaSftp.ToString().ToLower())
                    Decide = UploadMethods.FileViaSftp;

                else if (arg.ToLower() == UploadMethods.FileViaTftp.ToString().ToLower())
                    Decide = UploadMethods.FileViaTftp;

                else if (arg.ToLower() == UploadMethods.PostFileToBmon.ToString().ToLower())
                    Decide = UploadMethods.PostFileToBmon;

                else if (arg.ToLower() == UploadMethods.PostFileToDropbox.ToString().ToLower())
                    Decide = UploadMethods.PostFileToDropbox;

                else if (arg.ToLower() == UploadMethods.PostJsonToBmon.ToString().ToLower())
                    Decide = UploadMethods.PostJsonToBmon;

                else if (arg.ToLower() == UploadMethods.Uninitialized.ToString().ToLower())
                    Decide = UploadMethods.Uninitialized;

                else
                    throw new ConsoleHelpAsException("Invalid upload type...");
            });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                string localPath = new FileInfo(InputFile).DirectoryName;
                string localName = new FileInfo(InputFile).Name;
                string remoteName = localName;
                dynamic decision = null;

                switch (Decide)
                {
                    case UploadMethods.FileViaFtp:
                        {
                            foreach (var config in uploadConfig.MyFtp)
                            {
                                decision = new Lib.Transport.Generic.Ftp(new Uri(config.Server), config.Credential);
                                decision.UploadFileAsync(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.FileViaSftp:
                        {
                            foreach (var config in uploadConfig.MySftp)
                            {
                                decision = new Lib.Transport.Generic.Sftp(new Uri(config.Server), config.Port, config.Credential);
                                decision.UploadFile(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.FileViaTftp:
                        {
                            foreach (var config in uploadConfig.MyTftp)
                            {
                                decision = new Lib.Transport.Generic.Tftp(new Uri(config.Server));
                                decision.UploadFile(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.PostFileToBmon:
                        {
                            foreach (var config in uploadConfig.MyPostFileToBmon)
                            {
                                byte[] fileBytes = File.ReadAllBytes(InputFile);

                                decision = new Lib.Transport.Vendor.Bmon(new Uri(config.Server));
                                decision.PostAsync(config.Path, fileBytes);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.PostFileToDropbox:
                        {
                            foreach (var config in uploadConfig.MyPostFileToDropbox)
                            {
                                decision = new Lib.Transport.Vendor.Dropbox(config.Token);
                                decision.UploadFileAsync(localPath, localName, config.Path, remoteName, FileAction.OverwriteIfExist);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.PostJsonToBmon:
                        {
                            Lib.Devour.DotCsv.GenericFormatA raw = new Lib.Devour.DotCsv.GenericFormatA(InputFile);
                            MomentTuples momentTuples = new MomentTuples();
                            MomentArrays momentArrays = new MomentArrays();
                            raw.Parse(ref momentTuples);

                            Console.WriteLine(raw.Output);

                            foreach (Tuple<double, string, double> moment in momentTuples.Readings)
                                momentArrays.Readings.Add(new List<string>() { moment.Item1.ToString(), moment.Item2.ToString(), moment.Item3.ToString() });

                            foreach (var config in uploadConfig.MyPostJsonToBmon)
                            {
                                decision = new Lib.Transport.Vendor.Bmon(new Uri(config.Server), config.StoreKey);
                                decision.PostAsync(config.Path, momentArrays);

                                Console.WriteLine(decision.Output);
                            }
                        }
                        break;

                    case UploadMethods.Uninitialized:
                        break;

                    default:
                        throw new InvalidOperationException();
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
