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
        private FtpClient session;
        private StringBuilder stdout;

        public StringBuilder Stdout
        {
            get
            {
                return stdout;
            }
        }

        public Ftp(Uri host, NetworkCredential credential)
        {
            session = new FtpClient(host.DnsSafeHost, credential);
            stdout = new StringBuilder();
        }

        public async Task GetFolderContentsAsync(string remotePath)
        {
            try
            {
                await session.ConnectAsync();

                FtpListItem[] items = session.GetListing(remotePath);

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.Directory)
                        stdout.Append(string.Format("D  {0}/", item.Name));

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.File)
                        stdout.Append(string.Format("F{0,8} {1}", item.Size, item.Name));

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task DownloadFileAsync(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            try
            {
                await session.ConnectAsync();

                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    await session.DownloadFileAsync(localPath + @"\" + localFile, remotePath + @"/" + remoteFile, true);

                    stdout.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                }

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public async Task UploadFileAsync(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            try
            {
                await session.ConnectAsync();

                if (await session.FileExistsAsync(remotePath + @"/" + remoteFile) && action == FileAction.OverwriteIfExist
                    || (!await session.FileExistsAsync(remotePath + @"/" + remoteFile)))
                {
                    await session.UploadFileAsync(localPath + @"\" + localFile, remotePath + @"/" + remoteFile);

                    stdout.Append(string.Format("Transfer success for {0}", remotePath + @"/" + remoteFile));
                }

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
