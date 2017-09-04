using System;
using System.Configuration;

namespace Bmon.Client.Core
{
    public static class Statics
    {
        public static readonly String ConfEventLogSource = "Bmon Client";
        public static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
        public static readonly string ConfCsvGenericFormatA = ConfigurationManager.AppSettings["ConfCsvGenericFormatA"];
    }
}
