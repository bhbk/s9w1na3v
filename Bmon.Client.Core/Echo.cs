using System;
using System.Diagnostics;
using System.Text;

namespace Bmon.Client.Core
{
    public class Echo
    {
        [Flags]
        public enum levels
        {
            none = 0x01,
            audit_success = 0x03,
            audit_fail = 0x05,
            info = 0x07,
            debug = 0x9,
        }

        public class Audit
        {
            public static void Msg(String executingassembly, String method, StringBuilder msg, levels lvl)
            {
                switch (lvl)
                {
                    case levels.debug:
                        Bmon.Client.Lib.Echo.EventLogs.write(Bmon.Client.Core.Statics.ConfEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.Information);
                        break;
                    case levels.info:
                        Bmon.Client.Lib.Echo.EventLogs.write(Bmon.Client.Core.Statics.ConfEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.Information);
                        break;
                    case levels.audit_fail:
                        Bmon.Client.Lib.Echo.EventLogs.write(Bmon.Client.Core.Statics.ConfEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.FailureAudit);
                        break;
                    case levels.audit_success:
                        Bmon.Client.Lib.Echo.EventLogs.write(Bmon.Client.Core.Statics.ConfEventLogSource, executingassembly, method, msg.ToString(), EventLogEntryType.SuccessAudit);
                        break;
                    case levels.none:
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
                Bmon.Client.Lib.Echo.EventLogs.write(Bmon.Client.Core.Statics.ConfEventLogSource, executingassembly, method, ex);
            }
        }
    }
}
