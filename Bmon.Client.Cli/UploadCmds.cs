using Bmon.Client.Lib.Models;
using Bmon.Client.Lib.Transport;
using ManyConsole;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
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
                    case UploadMethods.FileToDropbox:
                        {
                            decision = new Lib.Transport.Vendor.Dropbox(Core.Config.v0_1_0_0.MyDropboxToken);
                            decision.UploadFileAsync(localPath, localName, Core.Config.v0_1_0_0.MyDropboxDefaultPath, remoteName, FileAction.OverwriteIfExist);

                            Console.WriteLine(decision.Stdout);
                        }
                        break;

                    case UploadMethods.FileViaFtp:
                        {
                            decision = new Lib.Transport.Generic.Ftp(Core.Config.v0_1_0_0.MyFtpHost,
                                new NetworkCredential(Core.Config.v0_1_0_0.MyFtpUser, Core.Config.v0_1_0_0.MyFtpPass));
                            decision.UploadFileAsync(localPath, localName, Core.Config.v0_1_0_0.MyFtpDefaultPath, remoteName, FileAction.OverwriteIfExist);

                            Console.WriteLine(decision.Stdout);
                        }
                        break;

                    case UploadMethods.FileViaSftp:
                        {
                            decision = new Lib.Transport.Generic.Sftp(Core.Config.v0_1_0_0.MySftpHost, Core.Config.v0_1_0_0.MySftpPort,
                                new NetworkCredential(Core.Config.v0_1_0_0.MySftpUser, Core.Config.v0_1_0_0.MySftpPass));
                            decision.UploadFile(localPath, localName, Core.Config.v0_1_0_0.MySftpDefaultPath, remoteName, FileAction.OverwriteIfExist);

                            Console.WriteLine(decision.Stdout);
                        }
                        break;

                    case UploadMethods.FileViaTftp:
                        {
                            decision = new Lib.Transport.Generic.Tftp(Core.Config.v0_1_0_0.MyTftpHost);
                            decision.UploadFile(localPath, localName, Core.Config.v0_1_0_0.MyTftpDefaultPath, remoteName, FileAction.OverwriteIfExist);

                            Console.WriteLine(decision.Stdout);
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

                            decision = new Lib.Transport.Vendor.Bmon(Core.Config.v0_1_0_0.MyBmonHost, Core.Config.v0_1_0_0.MyBmonStoreKey);
                            decision.PostAsync(Core.Config.v0_1_0_0.MyBmonPostPath, momentArrays);

                            Console.WriteLine(decision.Stdout);
                        }
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
