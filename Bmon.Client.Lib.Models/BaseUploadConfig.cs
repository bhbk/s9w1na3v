using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Lib.Models
{
    public abstract class BaseUploadConfig
    {
        internal Guid Id;
        internal UploadMethods Upload;
        internal string Token;
        internal Uri Server;
        internal ushort Port;
        internal NetworkCredential Cred;
        internal string Path;

        internal BaseUploadConfig(UploadMethods upload)
        {
            Id = new Guid();
            Upload = upload;
            Token = null;
            Server = null;
            Port = ushort.MinValue;
            Cred = null;
            Path = null;
        }

        public class FileToDropboxConfig : BaseUploadConfig
        {
            public FileToDropboxConfig(string token, string path)
                : base(UploadMethods.FileToDropbox)
            {
                base.Token = token;
                base.Path = path;
            }
        }

        public class FileViaSftpConfig : BaseUploadConfig
        {
            public FileViaSftpConfig(Uri server, ushort port, NetworkCredential cred, string path)
                : base(UploadMethods.FileViaSftp)
            {
                base.Server = server;
                base.Port = port;
                base.Cred = cred;
                base.Path = path;
            }
        }

        public class FileViaFtpConfig : BaseUploadConfig
        {
            public FileViaFtpConfig(Uri server, NetworkCredential cred, string path)
                : base(UploadMethods.FileViaFtp)
            {
                base.Server = server;
                base.Cred = cred;
                base.Path = path;
            }
        }

        public class FileViaTftpConfig : BaseUploadConfig
        {
            public FileViaTftpConfig(Uri server, string path)
                : base(UploadMethods.FileViaTftp)
            {
                base.Server = server;
                base.Path = path;
            }
        }

        public class WebApiToBmonConfig : BaseUploadConfig
        {
            public WebApiToBmonConfig(Uri server, string path)
                : base(UploadMethods.WebApiToBmon)
            {
                //The "StoreKey" is part of the MomentArrays dataset. Not needed here.
                base.Server = server;
                base.Path = path;
            }
        }
    }

    public enum UploadMethods
    {
        FileViaFtp,
        FileViaSftp,
        FileViaTftp,
        FileToDropbox,
        WebApiToBmon
    }
}
