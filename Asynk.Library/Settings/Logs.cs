using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Asynk.Library.Settings
{
    /// <summary>
    /// Defines the base class for the logging configuration
    /// </summary>
    public abstract class Logs : ConfigurationSection
    {
    }

    /// <summary>
    /// The Log to use for tracing
    /// </summary>
    public class TraceLog : Logs { }

    /// <summary>
    /// The Log to use for errors
    /// </summary>
    public class ErrorLog : Logs { }

    /// <summary>
    /// The Log to use for warnings
    /// </summary>
    public class WarningLog : Logs { }
}
