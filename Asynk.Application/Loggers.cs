using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;

namespace Asynk.Application
{
    /// <summary>
    /// Logs to an output that can be set
    /// </summary>
    /// <param name="s">The string to log</param>
    /// <param name="args">Optional arguments to use</param>
    public delegate void LogSignature(string s, params object[] args);

    /// <summary>
    /// Static class that allows you to set where the information should be logged
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Used to log a trace message
        /// </summary>
        public static LogSignature Trace { get; set; }

        /// <summary>
        /// Used to log a warning message
        /// </summary>
        public static LogSignature Warning { get; set; }

        /// <summary>
        /// Use to log an error message
        /// </summary>
        public static LogSignature Error { get; set; }

        /// <summary>
        /// Sets up the logs to point to the console
        /// </summary>
        public static void SetLogsToConsole()
        {
            Trace = ConsoleOut;
            Warning = ConsoleOut;
            Error = ConsoleOut;
        }

        /// <summary>
        /// Sets up the log files to point to a log file
        /// </summary>
        public static void SetLogsToFile()
        {
            Trace = LogFileOut;
            Warning = LogFileOut;
            Error = LogFileOut;
        }

        /// <summary>
        /// Default Console Output
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        private static void ConsoleOut(string s, params object[] args)
        {
            string formattedString = string.Format(s, args);

            // Log to the console with thread and date information
            System.Console.WriteLine("[{3}] - {0}({1}):  {2}",
                Thread.CurrentThread.Name,
                Thread.CurrentThread.ManagedThreadId,
                formattedString,
                DateTime.Now.ToString("hhmmssff"));
        }

        /// <summary>
        /// Default Log file output
        /// </summary>
        /// <param name="s"></param>
        /// <param name="args"></param>
        private static void LogFileOut(string s, params object[] args)
        {
            string formattedString = string.Format(s, args);

            // TODO: Configuration of where the log file is
            // TODO: Allow log file rollover/deletion
            using (FileStream fs = new FileStream("out.log", FileMode.Append))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.WriteLine("[{3}] - {0}({1}):  {2}",
                        Thread.CurrentThread.Name,
                        Thread.CurrentThread.ManagedThreadId,
                        formattedString,
                        DateTime.Now.ToString("hhmmssff"));
                }
            }
        }
    }
}
