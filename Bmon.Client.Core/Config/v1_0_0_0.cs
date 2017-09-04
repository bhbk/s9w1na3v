using System;
using System.Configuration;

namespace Bmon.Client.Core.Config
{
    public static class v1_0_0_0
    {
        private static readonly String csvGenericFormatA = @"..\..\..\Bmon.Client.Lib.Devour.Tests\DotCsv\GenericFormatA.csv";
        private static readonly String eventLogSource = "Bmon Client";

        private static readonly Uri myBmonHost = new Uri("https://bmon.ahfc.us");
        private static readonly string myBmonPostPath = "/readingdb/reading/store/";
        private static readonly string myBmonToken = "12345678";

        private static readonly string myDropboxToken = "12345678";
        private static readonly string myDropboxDefaultPath = "/";

        private static readonly Uri myFtpHost = new Uri("ftp://bmon.ahfc.us");
        private static readonly string myFtpUser = "ftpuser";
        private static readonly string myFtpPass = "ftppass";
        private static readonly string myFtpDefaultPath = "/";

        private static readonly Uri mySftpHost = new Uri("sftp://bmon.ahfc.us");
        private static readonly int mySftpPort = 22;
        private static readonly string mySftpUser = "sftpuser";
        private static readonly string mySftpPass = "sftppass";
        private static readonly string mySftpDefaultPath = "/";

        private static readonly Uri myTftpHost = new Uri("tftp://bmon.ahfc.us");
        private static readonly string myTftpDefaultPath = "/";

        public static string CsvGenericFormatA
        {
            get { return csvGenericFormatA; }
        }

        public static string MyEventLogSource
        {
            get { return eventLogSource; }
        }

        public static Uri MyBmonHost
        {
            get { return myBmonHost; }
        }

        public static string MyBmonPostPath
        {
            get { return myBmonPostPath; }
        }

        public static string MyBmonToken
        {
            get { return myBmonToken; }
        }

        public static string MyDropboxToken
        {
            get { return myDropboxToken; }
        }

        public static string MyDropboxDefaultPath
        {
            get { return myDropboxDefaultPath; }
        }

        public static Uri MyFtpHost
        {
            get { return myFtpHost; }
        }

        public static string MyFtpUser
        {
            get { return myFtpUser; }
        }

        public static string MyFtpPass
        {
            get { return myFtpPass; }
        }

        public static string MyFtpDefaultPath
        {
            get { return myFtpDefaultPath; }
        }

        public static Uri MySftpHost
        {
            get { return mySftpHost; }
        }

        public static int MySftpPort
        {
            get { return mySftpPort; }
        }

        public static string MySftpUser
        {
            get { return mySftpUser; }
        }

        public static string MySftpPass
        {
            get { return mySftpPass; }
        }

        public static string MySftpDefaultPath
        {
            get { return mySftpDefaultPath; }
        }

        public static Uri MyTftpHost
        {
            get { return myTftpHost; }
        }

        public static string MyTftpDefaultPath
        {
            get { return myTftpDefaultPath; }
        }
    }
}
