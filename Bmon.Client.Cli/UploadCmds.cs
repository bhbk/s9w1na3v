using Bmon.Client.Lib.Models;
using Bmon.Client.Lib.Transport;
using ManyConsole;
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        private UploadTypes upload;
        private string file = null;

        public UploadCmds()
        {
            IsCommand("upload", "Do upload things...");
            HasRequiredOption("f|file=", "File to parse. File must exist.", arg =>
            {
                Helpers.FileSanityChecks(ref arg, ref file);
            });
            HasRequiredOption("u|upload-type=", "Type of upload to perform.", arg =>
            {
                if (arg == UploadTypes.FileToDropbox.ToString())
                    upload = UploadTypes.FileToDropbox;

                else if (arg == UploadTypes.FileViaFtp.ToString())
                    upload = UploadTypes.FileViaFtp;

                else if (arg == UploadTypes.FileViaSftp.ToString())
                    upload = UploadTypes.FileViaSftp;

                else if (arg == UploadTypes.FileViaTftp.ToString())
                    upload = UploadTypes.FileViaTftp;

                else if (arg == UploadTypes.WebApiToBmon.ToString())
                    upload = UploadTypes.WebApiToBmon;

                else
                    throw new ConsoleHelpAsException("Invalid upload type given...");
            });
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                string localPath = new FileInfo(file).DirectoryName;
                string localName = new FileInfo(file).Name;
                string remoteName = localName;

                switch (upload)
                {
                    case UploadTypes.FileToDropbox:
                        {
                            Lib.Transport.Vendor.Dropbox dropbox = new Lib.Transport.Vendor.Dropbox(Core.Config.v1_0_0_0.MyDropboxToken);
                            dropbox.UploadFileAsync(localPath, localName, Core.Config.v1_0_0_0.MyDropboxDefaultPath, remoteName, FileAction.OverwriteIfExist);
                            Console.WriteLine(dropbox.Stdout);
                        }
                        break;

                    case UploadTypes.FileViaFtp:
                        {
                            Lib.Transport.Generic.Ftp ftp = new Lib.Transport.Generic.Ftp(Core.Config.v1_0_0_0.MyFtpHost,
                                new NetworkCredential(Core.Config.v1_0_0_0.MyFtpUser, Core.Config.v1_0_0_0.MyFtpPass));
                            ftp.UploadFileAsync(localPath, localName, Core.Config.v1_0_0_0.MyFtpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                            Console.WriteLine(ftp.Stdout);
                        }
                        break;

                    case UploadTypes.FileViaSftp:
                        {
                            Lib.Transport.Generic.Sftp sftp = new Lib.Transport.Generic.Sftp(Core.Config.v1_0_0_0.MySftpHost, Core.Config.v1_0_0_0.MySftpPort,
                                new NetworkCredential(Core.Config.v1_0_0_0.MySftpUser, Core.Config.v1_0_0_0.MySftpPass));
                            sftp.UploadFile(localPath, localName, Core.Config.v1_0_0_0.MySftpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                            Console.WriteLine(sftp.Stdout);
                        }
                        break;

                    case UploadTypes.FileViaTftp:
                        {
                            Lib.Transport.Generic.Tftp tftp = new Lib.Transport.Generic.Tftp(Core.Config.v1_0_0_0.MyTftpHost);
                            tftp.UploadFile(localPath, localName, Core.Config.v1_0_0_0.MyTftpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                            Console.WriteLine(tftp.Stdout);
                        }
                        break;

                    case UploadTypes.WebApiToBmon:
                        {
                            Lib.Devour.DotCsv.GenericFormatA csv = new Lib.Devour.DotCsv.GenericFormatA(file);
                            Lib.Models.MultipleMomentsTuples trends = new MultipleMomentsTuples();
                            csv.Parse(ref trends);
                            
                            Lib.Transport.Vendor.Bmon bmonApi = new Lib.Transport.Vendor.Bmon(Core.Config.v1_0_0_0.MyBmonHost);
                            bmonApi.PostAsync(Core.Config.v1_0_0_0.MyBmonPostPath, trends);
                            Console.WriteLine(bmonApi.Stdout);
                        }
                        break;

                    default:
                        throw new InvalidOperationException();
                }

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
