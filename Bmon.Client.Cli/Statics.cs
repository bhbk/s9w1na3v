using System;
using System.Configuration;

namespace Bmon.Client.Cli
{
    internal enum ExitCodes : int
    {
        Success = 0,
        Failure = 1,
        Exception = 2
    }

    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
    }
}
