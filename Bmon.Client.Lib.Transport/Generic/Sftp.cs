using Bmon.Client.Lib.Models;
using Renci.SshNet;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Sftp
    {
        private ConnectionInfo Info;
        private SshClient Cmds;
        private SftpClient Session;
        private StringBuilder StandardOutput;

        public StringBuilder Output
        {
            get
            {
                return StandardOutput;
            }
        }

        public Sftp(Uri host, int port, NetworkCredential credential)
        {
            //use password authentication
            Info = new ConnectionInfo(host.DnsSafeHost, port, credential.UserName,
                new AuthenticationMethod[] {
                    new PasswordAuthenticationMethod(credential.UserName, credential.Password)
                });

            Cmds = new SshClient(Info);
            Session = new SftpClient(Info);
            StandardOutput = new StringBuilder();
        }

        public Sftp(Uri host, int port, string userName, string filePathAndName, string passPhrase)
        {
            //use key authentication (using keys in openssh Format)
            using (var fs = File.OpenRead(filePathAndName))
            {
                Info = new ConnectionInfo(host.DnsSafeHost, port, userName,
                    new AuthenticationMethod[]{
                        new PrivateKeyAuthenticationMethod(userName, new PrivateKeyFile[]
                        {
                            new PrivateKeyFile(fs, passPhrase) 
                        }),
                    });

                Cmds = new SshClient(Info);
                Session = new SftpClient(Info);
                StandardOutput = new StringBuilder();
            }
        }

        public void ExecuteCommand(string command)
        {
            try
            {
                Cmds.Connect();

                using (var cmd = Cmds.CreateCommand(command))
                {
                    cmd.Execute();

                    StandardOutput.Append(string.Format("Command = {0}", cmd.CommandText + Environment.NewLine));
                    StandardOutput.Append(string.Format("Return Value = {0}", cmd.ExitStatus + Environment.NewLine));
                }

                Cmds.Disconnect();
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public void DownloadFile(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            //make this async in future...
            try
            {
                Session.Connect();

                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    using (FileStream fs = File.Create(localPath + @"\" + localFile))
                    {
                        Session.DownloadFile(remotePath + @"/" + remoteFile, fs, null);

                        StandardOutput.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                    }
                }

                Session.Disconnect();
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public void UploadFile(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            //make this async in the future...
            try
            {
                Session.Connect();

                using (var fs = File.OpenRead(localPath + @"\" + localFile))
                {
                    if (Session.Exists(remotePath + @"/" + remoteFile) && action == FileAction.OverwriteIfExist
                        || (!Session.Exists(remotePath + @"/" + remoteFile)))
                    {
                        Session.UploadFile(fs, remotePath + @"/" + remoteFile, true);

                        StandardOutput.Append(string.Format("Transfer success for {0}", remotePath + @"/" + remoteFile));
                    }
                }

                Session.Disconnect();
            }
            catch (Exception ex)
            {
                StandardOutput.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
