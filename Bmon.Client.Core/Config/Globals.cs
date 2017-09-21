using System;

namespace Bmon.Client.Core.Config
{
    public static class Globals
    {
        public static readonly String DevourConfigFile = "DevourConfig.xml";
        public static readonly String TriggerConfigFile = "TriggerConfig.xml";
        public static readonly String UploadConfigFile = "UploadConfig.xml";
        private static readonly String eventLogSource = "Bmon Client";

        public static string MyEventLogSource
        {
            get { return eventLogSource; }
        }
    }
}
