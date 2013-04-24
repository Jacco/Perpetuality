using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Diagnostics;
using System.Reflection;

namespace JaapNL.Utilities
{
    public class EventLogger
    {
        /// <summary>
        /// Returns the name of the owner of the Ip, if available.
        /// </summary>
        public static string GetIpOwner(string aIp)
        {
            return "";
        }

        public enum EventType
        {
            Information,
            Warning,
            Error
        }

        private const string DEFAULT_LOG = "Application";
        private const string DEFAULT_SOURCE = "ESSENT";

        public static string GetEventText(Exception exception, string eventText)
        {
            //If exception-parameter is not null: add stacktrace.
            if (exception != null)
            {
                eventText += "\r\n\r\n";
                eventText += "============================\r\n";
                eventText += "====== Exception info ======\r\n";
                eventText += "============================\r\n";
                eventText += String.Format("Exception-Message:\r\n{0}\r\n\r\nException-Source:\r\n{1}\r\n\r\nException-Stacktrace:\r\n{2}\r\n\r\n", exception.Message, exception.Source, exception.StackTrace);
                if (exception.InnerException != null)
                    eventText += String.Format("InnerException-Message:\r\n{0}\r\n\r\nInnerException-Source:\r\n{1}\r\n\r\nInnerException-Stacktrace:\r\n{2}\r\n\r\n", exception.InnerException.Message, exception.InnerException.Source, exception.InnerException.StackTrace);
            }

            //Create serveruptime-string
            var lMiliSecondsUp = Environment.TickCount;
            var lDays = lMiliSecondsUp / 86400000;
            lMiliSecondsUp = lMiliSecondsUp % 86400000;
            var lHours = lMiliSecondsUp / 3600000;
            lMiliSecondsUp = lMiliSecondsUp % 3600000;
            var lMinutes = lMiliSecondsUp / 60000;
            lMiliSecondsUp = lMiliSecondsUp % 60000;
            var lSeconds = lMiliSecondsUp / 1000;
            lMiliSecondsUp = lMiliSecondsUp % 1000;
            var lUptime = String.Format("{0} days, {1} hours, {2} minutes, {3} seconds, {4} miliseconds", Convert.ToString(lDays), Convert.ToString(lHours), Convert.ToString(lMinutes), Convert.ToString(lSeconds), Convert.ToString(lMiliSecondsUp));

            //Add HttpContext-information if available
            try
            {
                var lCt = System.Web.HttpContext.Current.Request;
                var lIpOwnerName = GetIpOwner(lCt.Headers["X-Forwarded-For"]);

                eventText += "\r\n\r\n";
                eventText += "==============================\r\n";
                eventText += "====== HttpContext ===========\r\n";
                eventText += "==============================\r\n";
                eventText += String.Format("REMOTE_ADDR: {0}\r\n", lCt.Headers["X-Forwarded-For"] + " " + lIpOwnerName);
                eventText += String.Format("LOCAL_ADDR: {0}\r\n", lCt.ServerVariables["LOCAL_ADDR"]);
                eventText += String.Format("SERVER_NAME: {0}\r\n", lCt.ServerVariables["SERVER_NAME"]);
                eventText += String.Format("SCRIPT_NAME: {0}\r\n", lCt.ServerVariables["SCRIPT_NAME"]);
                eventText += String.Format("HTTP_REFERER: {0}\r\n", lCt.UrlReferrer);
                eventText += String.Format("HTTP_USER_AGENT: {0}\r\n", lCt.UserAgent);
                eventText += String.Format("QUERYSTRING: {0}\r\n", lCt.QueryString);

                eventText += "\r\n\r\n";
                eventText += "==============================\r\n";
                eventText += "====== All headers ===========\r\n";
                eventText += "==============================\r\n";
                foreach (var item in lCt.Headers.AllKeys)
                    eventText += String.Format("HEADERITEM: {0} = {1}\r\n", item, lCt.Headers[item]);
            }
            catch { }

            //Add environment-information.
            eventText += "\r\n\r\n";
            eventText += "==============================\r\n";
            eventText += "====== Environment info ======\r\n";
            eventText += "==============================\r\n";
            eventText += String.Format("MachineName: {0}\r\n", Environment.MachineName);
            eventText += String.Format("OS version: {0}\r\n", Convert.ToString(Environment.OSVersion));
            eventText += String.Format("ExecutingAssembly-version: {0}\r\n", Convert.ToString(Assembly.GetExecutingAssembly().CodeBase));
            eventText += String.Format("CLR version: {0}\r\n", Convert.ToString(Environment.Version));
            eventText += String.Format("Memory footprint (bytes): {0}\r\n", Convert.ToString(Environment.WorkingSet));
            eventText += String.Format("Server uptime: {0}\r\n", lUptime);
            eventText += String.Format("Nr of processors: {0}\r\n", Convert.ToString(Environment.ProcessorCount));
            eventText += String.Format("CommandLine: {0}\r\n", Environment.CommandLine);
            eventText += String.Format("Current directory: {0}\r\n", Environment.CurrentDirectory);
            eventText += String.Format("System directory: {0}\r\n", Environment.SystemDirectory);
            eventText += String.Format("Username: {0}\r\n", Environment.UserName);
            eventText += String.Format("User domain name: {0}\r\n", Environment.UserDomainName);

            return eventText;
        }

        /// <summary>
        /// Write event in specified EventLogSource with specified EventType
        /// </summary>
        /// <param name="eventText">The event-text</param>
        /// <param name="eventType">The event-type</param>
        /// <param name="eventSourceName">The EventSource-name (e.g. MyLittleSmtpService, default ESSENT)</param>
        /// <param name="eventLogName">The EventLog-name (default: Application)</param>
        /// <param name="exception">When logging an error, you can pass the exception-object</param>
        private static void WriteEvent(string eventText, EventType eventType, string eventSourceName, string eventLogName, Exception exception)
        {
            //Set default values if empty
            if (String.IsNullOrEmpty(eventSourceName))
                eventSourceName = DEFAULT_SOURCE;
            if (String.IsNullOrEmpty(eventLogName))
                eventLogName = DEFAULT_LOG;

            //If eventlog doesn't exist, try to create it. It's not likely that this is going to work, since you need Admin-privileges to perform this action...
            try
            {
                if (!EventLog.SourceExists(eventSourceName))
                    EventLog.CreateEventSource(eventSourceName, eventLogName);
            }
            catch
            {
                //Add warning and write the event using the DEFAULT_SOURCE...
                eventText = String.Format(">> Application tried to write to {0}.{1}, which does not exist. Writing to {2}.{3} for now, please add eventsource <<\r\n\r\n{4}", eventLogName, eventSourceName, DEFAULT_LOG, DEFAULT_SOURCE, eventText);
                eventSourceName = DEFAULT_SOURCE;
                eventLogName = DEFAULT_LOG;
            }

            eventText = GetEventText(exception, eventText);

            //Add event, don't raise an error if it fails.
            try
            {
                EventLogEntryType type = EventLogEntryType.Information;
                switch (eventType)
                {
                    case EventType.Information:
                        type = EventLogEntryType.Information;
                        break;
                    case EventType.Warning:
                        type = EventLogEntryType.Warning;
                        break;
                    case EventType.Error:
                        type = EventLogEntryType.Error;
                        break;
                }
                EventLog.WriteEntry(eventSourceName, eventText, type);
            }
            catch { }
        }

        /// <summary>
        /// Write event with specified EventLogSource in Application-log, and specify EventType
        /// </summary>
        /// <param name="eventText">The event-text</param>
        /// <param name="eventType">The EventType</param>
        /// <param name="eventSourceName">The EventLogSource-name (e.g. MyLittleSmtpService, will be created if it does not exist)</param>
        public static void WriteEvent(string eventText, EventType eventType, string eventSourceName)
        {
            WriteEvent(eventText, eventType, eventSourceName, DEFAULT_LOG, null);
        }

        /// <summary>
        /// Write event with specified EventLogSource in Application-log, and specify EventType
        /// </summary>
        /// <param name="eventText">The event-text</param>
        /// <param name="eventType">The EventType</param>
        /// <param name="eventSourceName">The EventLogSource-name (e.g. MyLittleSmtpService, will be created if it does not exist)</param>
        public static void WriteEvent(string eventText, EventType eventType, string eventSourceName, Exception exception)
        {
            WriteEvent(eventText, eventType, eventSourceName, DEFAULT_LOG, exception);
        }
    }
}