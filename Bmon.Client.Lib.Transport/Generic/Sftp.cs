using Renci.SshNet;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Sftp
    {
        private ConnectionInfo info;
        private SshClient cmds;
        private SftpClient session;
        private StringBuilder stdout;

        public StringBuilder Stdout
        {
            get
            {
                return stdout;
            }
        }

        public Sftp(Uri host, int port, NetworkCredential credential)
        {
            //use password authentication
            info = new ConnectionInfo(host.DnsSafeHost, port, credential.UserName,
                new AuthenticationMethod[] {
                    new PasswordAuthenticationMethod(credential.UserName, credential.Password)
                });

            cmds = new SshClient(info);
            session = new SftpClient(info);
            stdout = new StringBuilder();
        }

        public Sftp(Uri host, int port, string userName, string filePathAndName, string passPhrase)
        {
            //use key authentication (using keys in openssh Format)
            using (var fs = File.OpenRead(filePathAndName))
            {
                info = new ConnectionInfo(host.DnsSafeHost, port, userName,
                    new AuthenticationMethod[]{
                        new PrivateKeyAuthenticationMethod(userName, new PrivateKeyFile[]
                        {
                            new PrivateKeyFile(fs, passPhrase) 
                        }),
                    });

                cmds = new SshClient(info);
                session = new SftpClient(info);
                stdout = new StringBuilder();
            }
        }

        public void ExecuteCommand(string command)
        {
            try
            {
                cmds.Connect();

                using (var cmd = cmds.CreateCommand(command))
                {
                    cmd.Execute();

                    stdout.Append(string.Format("Command = {0}", cmd.CommandText + Environment.NewLine));
                    stdout.Append(string.Format("Return Value = {0}", cmd.ExitStatus + Environment.NewLine));
                }

                cmds.Disconnect();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public void DownloadFile(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            //make this async in future...
            try
            {
                session.Connect();

                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    using (FileStream fs = File.Create(localPath + @"\" + localFile))
                    {
                        session.DownloadFile(remotePath + @"/" + remoteFile, fs, null);

                        stdout.Append(string.Format("Transfer success for {0}", localPath + @"\" + localFile));
                    }
                }

                session.Disconnect();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }

        public void UploadFile(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            //make this async in the future...
            try
            {
                session.Connect();

                using (var fs = File.OpenRead(localPath + @"\" + localFile))
                {
                    if (session.Exists(remotePath + @"/" + remoteFile) && action == FileAction.OverwriteIfExist
                        || (!session.Exists(remotePath + @"/" + remoteFile)))
                    {
                        session.UploadFile(fs, remotePath + @"/" + remoteFile, true);

                        stdout.Append(string.Format("Transfer success for {0}", remotePath + @"/" + remoteFile));
                    }
                }

                session.Disconnect();
            }
            catch (Exception ex)
            {
                stdout.Append(ex.Message + Environment.NewLine
                    + ex.StackTrace + Environment.NewLine);
            }
        }
    }
}
