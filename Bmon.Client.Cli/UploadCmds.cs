using Bmon.Client.Lib.Models;
using Bmon.Client.Lib.Transport;
using ManyConsole;
using System;
using System.IO;
using System.Reflection;
using System.Net;

namespace Bmon.Client.Cli
{
    public class UploadCmds : ConsoleCommand
    {
        private string file;
        private string fileFormat;
        private UploadTypes uploadType;

        public UploadCmds()
        {
            IsCommand("upload", "Do upload things...");
            //HasRequiredOption("ut|upload-type=", "Method for uploading data.", ut => uploadType = ut);
            HasRequiredOption("f|file=", "File that contains trending data.", f => file = f);
            HasRequiredOption("ff|file-format=", "File format for the trending data.", ff => fileFormat = ff);
            HasAdditionalArguments(1, "<type>");
        }

        public override int Run(string[] remainingArguments)
        {
            try
            {
                Helpers.FileSanityChecks(ref file);
                string localPath = new FileInfo(file).DirectoryName;
                string localName = new FileInfo(file).Name;
                string remoteName = localName;

                switch (uploadType)
                {
                    case UploadTypes.FileToDropbox:
                        {
                            Lib.Transport.Vendor.Dropbox dropbox = new Lib.Transport.Vendor.Dropbox(Core.Config.v1_0_0_0.MyDropboxToken);
                            dropbox.UploadFileAsync(localPath, localName, Core.Config.v1_0_0_0.MyDropboxDefaultPath, remoteName, FileAction.OverwriteIfExist);
                        }
                        break;

                    case UploadTypes.FileViaFtp:
                        {
                            Lib.Transport.Generic.Ftp ftp = new Lib.Transport.Generic.Ftp(Core.Config.v1_0_0_0.MyFtpHost,
                                new NetworkCredential(Core.Config.v1_0_0_0.MyFtpUser, Core.Config.v1_0_0_0.MyFtpPass));
                            ftp.UploadFileAsync(localPath, localName, Core.Config.v1_0_0_0.MyFtpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                        }
                        break;

                    case UploadTypes.FileViaSftp:
                        {
                            Lib.Transport.Generic.Sftp sftp = new Lib.Transport.Generic.Sftp(Core.Config.v1_0_0_0.MySftpHost, Core.Config.v1_0_0_0.MySftpPort,
                                new NetworkCredential(Core.Config.v1_0_0_0.MySftpUser, Core.Config.v1_0_0_0.MySftpPass));
                            sftp.UploadFile(localPath, localName, Core.Config.v1_0_0_0.MySftpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                        }
                        break;

                    case UploadTypes.FileViaTftp:
                        {
                            Lib.Transport.Generic.Tftp tftp = new Lib.Transport.Generic.Tftp(Core.Config.v1_0_0_0.MyTftpHost);
                            tftp.UploadFile(localPath, localName, Core.Config.v1_0_0_0.MyTftpDefaultPath, remoteName, FileAction.OverwriteIfExist);
                        }
                        break;

                    case UploadTypes.WebApiToBmon:
                        {
                            Lib.Devour.DotCsv.GenericFormatA csv = new Lib.Devour.DotCsv.GenericFormatA(file);
                            Lib.Models.BmonPostTrendMultiple trends = csv.ParseTrends();
                            
                            Lib.Transport.Vendor.Bmon bmonApi = new Lib.Transport.Vendor.Bmon(Core.Config.v1_0_0_0.MyBmonHost);
                            bmonApi.PostAsync(Core.Config.v1_0_0_0.MyBmonPostPath, trends);
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

                return (int)ExitCodes.Exception;
            }
        }
    }
}
