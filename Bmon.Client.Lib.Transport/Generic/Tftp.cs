using System;
using System.IO;
using System.Text;
using System.Threading;
using Tftp.Net;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Tftp
    {
        private TftpClient session;
        private ITftpTransfer transfer;
        private AutoResetEvent finished = new AutoResetEvent(false);
        private StringBuilder stdout;

        public StringBuilder Stdout
        {
            get
            {
                return stdout;
            }
        }

        public Tftp(Uri host)
        {
            session = new TftpClient(host.DnsSafeHost);
            transfer.OnProgress += new TftpProgressHandler(transferOnProgress);
            transfer.OnFinished += new TftpEventHandler(transferOnFinshed);
            transfer.OnError += new TftpErrorHandler(transferOnError);
            stdout = new StringBuilder();
        }

        public void GetFolderContents(string remoteDirAndPath)
        {
            /*
             * tftp doesn't support directory listing. it is a very
             * trivial protocol. 
             */
        }

        public void DownloadFile(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            try
            {
                //tftp doesn't have ability to see if remote file exists
                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    using (var fs = File.OpenRead(localPath + @"\" + localFile))
                    {
                        MemoryStream ms = new MemoryStream();
                        fs.CopyTo(ms);

                        transfer = session.Download(remotePath + @"\" + remoteFile);
                        transfer.Start(ms);
                        finished.WaitOne();
                    }
                }
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public void UploadFile(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            try
            {
                //tftp doesn't have ability to see if remote file exists
                using (var fs = File.OpenRead(localPath + @"\" + localFile))
                {
                    MemoryStream ms = new MemoryStream();
                    fs.CopyTo(ms);

                    transfer = session.Upload(remotePath + @"\" + remoteFile);
                    transfer.Start(ms);
                    finished.WaitOne();
                }
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        private void transferOnProgress(ITftpTransfer transfer, TftpTransferProgress progress)
        {
            stdout.Append(string.Format("Transfer progress for {0} is {1}", transfer.Filename, progress));
        }

        private void transferOnError(ITftpTransfer transfer, TftpTransferError error)
        {
            stdout.Append(string.Format("Transfer failure for {0} with error {1}", transfer.Filename, error));
            finished.Set();
        }

        private void transferOnFinshed(ITftpTransfer transfer)
        {
            stdout.Append(string.Format("Transfer success for {0}", transfer.Filename));
            finished.Set();
        }
    }
}
