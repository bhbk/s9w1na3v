using System;
using System.Configuration;

namespace Bmon.Client.Lib.Transport
{
    public enum FileAction
    {
        AbortIfExist,
        AppendIfExist,
        OverwriteIfExist
    }

    public enum UploadTypes
    {
        FileViaFtp,
        FileViaSftp,
        FileViaTftp,
        FileToDropbox,
        WebApiToBmon
    }

    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
    }
}
