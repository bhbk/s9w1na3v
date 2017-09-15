using System;

namespace Bmon.Client.Core.Config
{
    public static class Globals
    {
        public static readonly String DevourConfigFile = "ConfigForDevour.xml";
        public static readonly String UploadConfigFile = "ConfigForUpload.xml";
        private static readonly String eventLogSource = "Bmon Client";

        public static string MyEventLogSource
        {
            get { return eventLogSource; }
        }
    }
}
