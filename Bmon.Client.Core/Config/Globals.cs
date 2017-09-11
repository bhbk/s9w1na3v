using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bmon.Client.Core.Config
{
    public static class Globals
    {
        private static readonly String eventLogSource = "Bmon Client";

        public static string MyEventLogSource
        {
            get { return eventLogSource; }
        }
    }
}
