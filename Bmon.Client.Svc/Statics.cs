using System;
using System.Configuration;

namespace Bmon.Client.Svc
{
    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
    }
}
