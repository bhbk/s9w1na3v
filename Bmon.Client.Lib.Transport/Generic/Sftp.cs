using Renci.SshNet;
using Renci.SshNet.Async;
using System;
using System.IO;
using System.Net;

namespace Bmon.Client.Lib.Transport.Generic
{
    public class Sftp
    {
        private ConnectionInfo info;
        private SshClient cmds;
        private SftpClient session;

        public Sftp(Uri host, int port, NetworkCredential credential)
        {
            //use password authentication
            info = new ConnectionInfo(host.DnsSafeHost, port, credential.UserName,
                new AuthenticationMethod[] {
                    new PasswordAuthenticationMethod(credential.UserName, credential.Password)
                });

            cmds = new SshClient(info);
            session = new SftpClient(info);
        }

        public Sftp(Uri host, int port, string username, string file, string passphrase)
        {
            //use key authentication (using keys in openssh Format)
            using (var fs = File.OpenRead(file))
            {
                info = new ConnectionInfo(host.DnsSafeHost, port, username,
                    new AuthenticationMethod[]{
                        new PrivateKeyAuthenticationMethod(username, new PrivateKeyFile[]
                        {
                            new PrivateKeyFile(fs, passphrase) 
                        }),
                    });

                cmds = new SshClient(info);
                session = new SftpClient(info);
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
                    Console.WriteLine("Command>" + cmd.CommandText);
                    Console.WriteLine("Return Value = {0}", cmd.ExitStatus);
                }

                cmds.Disconnect();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }


        public void DownloadFile(string remotePath, string remoteFile, string localPath, string localFile, FileAction action)
        {
            try
            {
                session.Connect();

                if (File.Exists(localPath + @"\" + localFile) && action == FileAction.OverwriteIfExist
                    || (!File.Exists(localPath + @"\" + localFile)))
                {
                    using (FileStream fs = File.Create(localPath + @"\" + localFile))
                    {
                        session.DownloadFile(remotePath + @"/" + remoteFile, fs, null);
                        fs.Close();

                        Console.WriteLine("Transfer success for {0}", localPath + @"\" + localFile);
                    }
                }

                session.Disconnect();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }

        public void UploadFile(string localPath, string localFile, string remotePath, string remoteFile, FileAction action)
        {
            try
            {
                session.Connect();

                using (var fs = File.OpenRead(localPath + @"\" + localFile))
                {
                    if (session.Exists(remotePath + @"\" + remoteFile) && action == FileAction.OverwriteIfExist
                        || (!session.Exists(remotePath + @"\" + remoteFile)))
                    {
                        session.UploadFile(fs, remotePath + @"\" + remoteFile, true);

                        Console.WriteLine("Transfer success for {0}", remotePath + @"\" + remoteFile);
                    }
                }

                session.Disconnect();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
            }
        }
    }
}
