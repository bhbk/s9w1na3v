using System;
using System.Configuration;

namespace Bmon.Client.Core
{
    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
        internal static readonly String CsvGenericFormatA = @"..\..\..\Bmon.Client.Lib.Devour.Tests\DotCsv\GenericFormatA.csv";
    }
}
