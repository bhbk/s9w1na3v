using System;
using System.Diagnostics;
using System.Text;

namespace Bmon.Client.Core.Echo
{
    internal class EventLogs
    {
        internal static void Record(String executingassembly, String method, String msg, EventLogEntryType level)
        {
            if (EventLog.SourceExists(Config.Globals.MyEventLogSource))
            {
                EventLog evtLog = new EventLog("Application", ".", executingassembly);
                StringBuilder evtEntry = new StringBuilder(executingassembly);
                evtEntry.Append(Environment.NewLine + method);
                evtEntry.Append(Environment.NewLine + msg);
                evtLog.WriteEntry(evtEntry.ToString(), level);
                evtLog.Close();
            }
            else
                EventLog.CreateEventSource(Config.Globals.MyEventLogSource, "Application");
        }
        internal static void Record(String appdomain, String executingassembly, String method, String msg, EventLogEntryType level)
        {
            if (EventLog.SourceExists(Config.Globals.MyEventLogSource))
            {
                EventLog evtLog = new EventLog("Application", ".", appdomain);
                StringBuilder evtEntry = new StringBuilder(appdomain);
                evtEntry.Append(Environment.NewLine + executingassembly);
                evtEntry.Append(Environment.NewLine + method);
                evtEntry.Append(Environment.NewLine + msg);
                evtLog.WriteEntry(evtEntry.ToString(), level);
                evtLog.Close();
            }
            else
                EventLog.CreateEventSource(Config.Globals.MyEventLogSource, "Application");
        }
        internal static void Record(String executingassembly, String method, Exception ex)
        {
            if (EventLog.SourceExists(Config.Globals.MyEventLogSource))
            {
                EventLog evtLog = new EventLog("Application", ".", executingassembly);
                StringBuilder evtEntry = new StringBuilder(executingassembly);
                evtEntry.Append(Environment.NewLine + method);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                evtLog.WriteEntry(evtEntry.ToString(), EventLogEntryType.Error);
                evtLog.Close();
            }
            else
                EventLog.CreateEventSource(Config.Globals.MyEventLogSource, "Application");
        }

        internal static void Record(String appdomain, String executingassembly, String method, Exception ex)
        {
            if (EventLog.SourceExists(Config.Globals.MyEventLogSource))
            {
                EventLog evtLog = new EventLog("Application", ".", appdomain);
                StringBuilder evtEntry = new StringBuilder(appdomain);
                evtEntry.Append(Environment.NewLine + executingassembly);
                evtEntry.Append(Environment.NewLine + method);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                evtLog.WriteEntry(evtEntry.ToString(), EventLogEntryType.Error);
                evtLog.Close();
            }
            else
                EventLog.CreateEventSource(Config.Globals.MyEventLogSource, "Application");
        }

        internal static void Record(String appdomain, String entryassembly, String callingassembly, String executingassembly, String method, Exception ex)
        {
            if (EventLog.SourceExists(Config.Globals.MyEventLogSource))
            {
                EventLog evtLog = new EventLog("Application", ".", appdomain);
                StringBuilder evtEntry = new StringBuilder(appdomain);
                evtEntry.Append(Environment.NewLine + entryassembly);
                evtEntry.Append(Environment.NewLine + callingassembly);
                evtEntry.Append(Environment.NewLine + executingassembly);
                evtEntry.Append(Environment.NewLine + method);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + "Exception: " + ex.Message);
                evtEntry.Append(Environment.NewLine + Environment.NewLine + "Stack Trace: " + ex.StackTrace);
                evtLog.WriteEntry(evtEntry.ToString(), EventLogEntryType.Error);
                evtLog.Close();
            }
            else
                EventLog.CreateEventSource(Config.Globals.MyEventLogSource, "Application");
        }
    }
}
