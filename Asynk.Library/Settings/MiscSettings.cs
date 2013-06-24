using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Asynk.Library.Settings
{
    /// <summary>
    /// Miscellaneous config options 
    /// </summary>
    public class MiscSettings : ConfigurationSection
    {
        /// <summary>
        /// A regular expression that once matched will be ignored when searching
        /// for assemblies that Asynker can call.  By Default System and Microsoft
        /// assemblies are ignored
        /// </summary>
        [ConfigurationProperty("IgnoreFilter", DefaultValue = "(^System.*|^Microsoft.*)")]
        public string IgnoreFilter
        {
            get { return (string)this["IgnoreFilter"]; }
            set { this["IgnoreFilter"] = value; }
        }
    }
}
