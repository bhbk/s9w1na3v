using Bmon.Client.Lib.Models;
using FluentFTP;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Ftp
    {
        private FtpClient Session;
        private StringBuilder StandardOut;

        public StringBuilder Output
        {
            get
            {
                return StandardOut;
            }
        }

        public Ftp(Uri host, NetworkCredential credential)
        {
            Session = new FtpClient(host.DnsSafeHost, credential);
            StandardOut = new StringBuilder();
        }

        public async Task GetFolderContentsAsync(string remotePath)
        {
            try
            {
                await Session.ConnectAsync();

                FtpListItem[] items = Session.GetListing(remotePath);

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.Directory)
                        StandardOut.Append(string.Format("D  {0}/", item.Name));

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.File)
                        StandardOut.Append(string.Format("F{0,8} {1}", item.Size, item.Name));

                await Session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                StandardOut.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task DownloadFileAsync(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            try
            {
                await Session.ConnectAsync();

                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    await Session.DownloadFileAsync(localPath + @"\" + localFile, remotePath + @"/" + remoteFile, true);

                    StandardOut.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                }

                await Session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                StandardOut.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task UploadFileAsync(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            try
            {
                await Session.ConnectAsync();

                if (await Session.FileExistsAsync(remotePath + @"/" + remoteFile) && action == FileAction.OverwriteIfExist
                    || (!await Session.FileExistsAsync(remotePath + @"/" + remoteFile)))
                {
                    await Session.UploadFileAsync(localPath + @"\" + localFile, remotePath + @"/" + remoteFile);

                    StandardOut.Append(string.Format("Transfer success for {0}", remotePath + @"/" + remoteFile));
                }

                await Session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                StandardOut.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
