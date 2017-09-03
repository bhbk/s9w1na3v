using FluentFTP;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Ftp
    {
        private FtpClient session;

        public Ftp(Uri host, NetworkCredential credential)
        {
            session = new FtpClient(host.DnsSafeHost, credential);
        }

        public async Task GetFolderContentsAsync(string remotePath)
        {
            try
            {
                await session.ConnectAsync();

                FtpListItem[] items = session.GetListing(remotePath);

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.Directory)
                        Console.WriteLine("D  {0}/", item.Name);

                foreach (FtpListItem item in items)
                    if (item.Type == FtpFileSystemObjectType.File)
                        Console.WriteLine("F{0,8} {1}", item.Size, item.Name);

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
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

                    Console.WriteLine("Transfer success for {0}", localPath + @"\" + localFile);
                }

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
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

                    Console.WriteLine("Transfer success for {0}", remotePath + @"/" + remoteFile);
                }

                await session.DisconnectAsync();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }
    }
}
