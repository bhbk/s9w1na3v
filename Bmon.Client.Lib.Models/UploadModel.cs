using System;
using System.Net;

namespace Bmon.Client.Lib.Models
{
    [Serializable]
    public class FileViaFtpConfig
    {
        public Guid Id;
        public string Server;
        public NetworkCredential Credential;
        public string Path;

        public FileViaFtpConfig()
        {
            Id = new Guid();
            Server = string.Empty;
            Credential = new NetworkCredential();
            Path = string.Empty;
        }

        public FileViaFtpConfig(string server, NetworkCredential credential, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Credential = credential;
            Path = path;
        }
    }

    [Serializable]
    public class FileViaSftpConfig
    {
        public Guid Id;
        public string Server;
        public ushort Port;
        public NetworkCredential Credential;
        public string Path;

        public FileViaSftpConfig()
        {
            Id = new Guid();
            Server = string.Empty;
            Port = ushort.MinValue;
            Path = string.Empty;
        }

        public FileViaSftpConfig(string server, ushort port, NetworkCredential credential, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Port = port;
            Credential = credential;
            Path = path;
        }
    }

    [Serializable]
    public class FileViaTftpConfig
    {
        public Guid Id;
        public string Server;
        public string Path;

        public FileViaTftpConfig()
        {
            Id = new Guid();
            Server = string.Empty;
            Path = string.Empty;
        }

        public FileViaTftpConfig(string server, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Path = path;
        }
    }

    [Serializable]
    public class PostFileToDropboxConfig
    {
        public Guid Id;
        public string Token;
        public string Path;

        public PostFileToDropboxConfig()
        {
            Id = new Guid();
            Token = string.Empty;
            Path = string.Empty;
        }

        public PostFileToDropboxConfig(string token, string path)
        {
            Id = Guid.NewGuid();
            Token = token;
            Path = path;
        }
    }

    [Serializable]
    public class PostFileToBmonConfig
    {
        public Guid Id;
        public string Server;
        public string Path;
        public string LocalDir;
        public string LocalFile;

        public PostFileToBmonConfig()
        {
            Id = new Guid();
            Server = string.Empty;
            Path = string.Empty;
            LocalDir = string.Empty;
            LocalFile = string.Empty;
        }

        public PostFileToBmonConfig(string server, string path, string localDir, string localFile)
        {
            Id = Guid.NewGuid();
            Server = server;
            Path = path;
            LocalDir = localDir;
            LocalFile = localFile;
        }
    }

    [Serializable]
    public class PostJsonToBmonConfig
    {
        public Guid Id;
        public string Server;
        public string Path;
        public string StoreKey;

        public PostJsonToBmonConfig()
        {
            //The "StoreKey" is part of the MomentArrays dataset. Not needed here.
            Id = new Guid();
            Server = string.Empty;
            Path = string.Empty;
            StoreKey = string.Empty;
        }

        public PostJsonToBmonConfig(string server, string path, string storeKey)
        {
            //The "StoreKey" is part of the MomentArrays dataset. Not needed here.
            Id = Guid.NewGuid();
            Server = server;
            Path = path;
            StoreKey = storeKey;
        }
    }

    public enum UploadMethods
    {
        FileViaFtp,
        FileViaSftp,
        FileViaTftp,
        PostFileToBmon,
        PostFileToDropbox,
        PostJsonToBmon,
        Uninitialized
    }
}
