﻿using System;
using System.Configuration;

namespace Bmon.Client.Ui
{
    internal static class Statics
    {
        internal static readonly bool ConfDebug = Boolean.Parse(ConfigurationManager.AppSettings["Debug"]);
    }
}
