using System;
using System.Configuration;

namespace Bmon.Client.Lib.Devour
{
    public enum OutputFormat
    {
        Csv,
        Json
    }

    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
    }
}
