using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Asynk.Library.Settings
{
    /// <summary>
    /// Static methods to access the config settings
    /// </summary>
    public static class AsynkConfig
    {
        /// <summary>
        /// Holds information about the queues
        /// </summary>
        public static class QueueSettings
        {
            /// <summary>
            /// Gets the queue that will be used for reading
            /// </summary>
            public static DefaultQueue DefaultQueue = ConfigurationManager.GetSection("Asynk.Settings/Queues/DefaultQueue") as DefaultQueue
                ?? new DefaultQueue();

            /// <summary>
            /// Gets the queue where failed request will go
            /// </summary>
            public static FailedQueue FailedQueue = ConfigurationManager.GetSection("Asynk.Settings/Queues/FailedQueue") as FailedQueue
                ?? new FailedQueue();
        }

        /// <summary>
        /// Holds configuration values as to where to log
        /// </summary>
        public static class LogSettings
        {
            /// <summary>
            /// Where to log trace information
            /// </summary>
            public static TraceLog TraceLog = ConfigurationManager.GetSection("Asynk.Settings/Logs/TraceLog") as TraceLog
                ?? new TraceLog();

            /// <summary>
            /// Where errors should be logged
            /// </summary>
            public static ErrorLog ErrorLog = ConfigurationManager.GetSection("Asynk.Settings/Logs/ErrorLog") as ErrorLog
                ?? new ErrorLog();

            /// <summary>
            /// Where warnings should be logged
            /// </summary>
            public static WarningLog WarningLog = ConfigurationManager.GetSection("Asynk.Settings/Logs/WarningLog") as WarningLog
                ?? new WarningLog();
        }

        /// <summary>
        /// Holds Miscellaneous config options
        /// </summary>
        public static MiscSettings Misc = ConfigurationManager.GetSection("Asynk.Settings/Misc") as MiscSettings
            ?? new MiscSettings();
    }
}
