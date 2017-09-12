using System;
using System.Net;

namespace Bmon.Client.Lib.Models
{
    //[Serializable]
    public class FileToDropboxConfig
    {
        public Guid Id;
        public string Token;
        public string Path;
        public FileToDropboxConfig() : base() { }
        public FileToDropboxConfig(string token, string path)
        {
            Id = Guid.NewGuid();
            Token = token;
            Path = path;
        }
    }

    //[Serializable]
    public class FileViaFtpConfig
    {
        public Guid Id;
        public string Server;
        public NetworkCredential Credential;
        public string Path;
        public FileViaFtpConfig() { }
        public FileViaFtpConfig(string server, NetworkCredential credential, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Credential = credential;
            Path = path;
        }
    }

    //[Serializable]
    public class FileViaSftpConfig
    {
        public Guid Id;
        public string Server;
        public ushort Port;
        public NetworkCredential Credential;
        public string Path;
        public FileViaSftpConfig() { }
        public FileViaSftpConfig(string server, ushort port, NetworkCredential credential, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Port = port;
            Credential = credential;
            Path = path;
        }
    }

    //[Serializable]
    public class FileViaTftpConfig
    {
        public Guid Id;
        public string Server;
        public string Path;
        public FileViaTftpConfig() { }
        public FileViaTftpConfig(string server, string path)
        {
            Id = Guid.NewGuid();
            Server = server;
            Path = path;
        }
    }

    //[Serializable]
    public class WebApiToBmonConfig
    {
        public Guid Id;
        public string Server;
        public string Path;
        public string StoreKey;
        public WebApiToBmonConfig() { }
        public WebApiToBmonConfig(string server, string storeKey, string path)
        {
            //The "StoreKey" is part of the MomentArrays dataset. Not needed here.
            Id = Guid.NewGuid();
            Server = server;
            StoreKey = storeKey;
            Path = path;
        }
    }

    public enum UploadMethods
    {
        FileViaFtp,
        FileViaSftp,
        FileViaTftp,
        FileToDropbox,
        WebApiToBmon,
        DoNothing
    }
}
