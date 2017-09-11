using System;
using System.Diagnostics;
using System.Text;

namespace Bmon.Client.Core.Echo
{
    public class Proxy
    {
        [Flags]
        public enum DebugLevels
        {
            None = 0x01,
            AuditSuccess = 0x03,
            AuditFail = 0x05,
            Info = 0x07,
            Debug = 0x9,
        }

        public class Audit
        {
            public static void Msg(String executingassembly, String method, StringBuilder msg, DebugLevels level)
            {
                switch (level)
                {
                    case DebugLevels.Debug:
                        Core.Echo.EventLogs.Record(Config.Globals.MyEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.Information);
                        break;
                    case DebugLevels.Info:
                        Core.Echo.EventLogs.Record(Config.Globals.MyEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.Information);
                        break;
                    case DebugLevels.AuditFail:
                        Core.Echo.EventLogs.Record(Config.Globals.MyEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.FailureAudit);
                        break;
                    case DebugLevels.AuditSuccess:
                        Core.Echo.EventLogs.Record(Config.Globals.MyEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.SuccessAudit);
                        break;
                    case DebugLevels.None:
                        break;
                    default:
                        break;
                };

            }
        }
        public class Caught
        {
            public static void Msg(String executingassembly, String method, Exception ex)
            {
                Core.Echo.EventLogs.Record(Config.Globals.MyEventLogSource, executingassembly, method, ex);
            }
        }
    }
}
