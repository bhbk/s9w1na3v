using Bmon.Client.Lib.Models;
using System;
using System.IO;
using System.Text;
using System.Threading;
using Tftp.Net;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Tftp
    {
        private TftpClient Session;
        private ITftpTransfer Transfer;
        private AutoResetEvent Finished = new AutoResetEvent(false);
        private StringBuilder StandardOutput;

        public StringBuilder Output
        {
            get
            {
                return StandardOutput;
            }
        }

        public Tftp(Uri host)
        {
            Session = new TftpClient(host.DnsSafeHost);
            Transfer.OnProgress += new TftpProgressHandler(transferOnProgress);
            Transfer.OnFinished += new TftpEventHandler(transferOnFinshed);
            Transfer.OnError += new TftpErrorHandler(transferOnError);
            StandardOutput = new StringBuilder();
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

                        Transfer = Session.Download(remotePath + @"\" + remoteFile);
                        Transfer.Start(ms);
                        Finished.WaitOne();
                    }
                }
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
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

                    Transfer = Session.Upload(remotePath + @"\" + remoteFile);
                    Transfer.Start(ms);
                    Finished.WaitOne();
                }
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        private void transferOnProgress(ITftpTransfer transfer, TftpTransferProgress progress)
        {
            StandardOutput.Append(string.Format("Transfer progress for {0} is {1}", transfer.Filename, progress));
        }

        private void transferOnError(ITftpTransfer transfer, TftpTransferError error)
        {
            StandardOutput.Append(string.Format("Transfer failure for {0} with error {1}", transfer.Filename, error));
            Finished.Set();
        }

        private void transferOnFinshed(ITftpTransfer transfer)
        {
            StandardOutput.Append(string.Format("Transfer success for {0}", transfer.Filename));
            Finished.Set();
        }
    }
}
