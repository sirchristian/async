using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Asynk.Library.Queues;

namespace Asynk.Library.Settings
{
    /// <summary>
    /// Defines the base class for the queue configuration
    /// </summary>
    public abstract class Queues : ConfigurationSection
    {
        /// <summary>
        /// The Full string of the type of queue that should be used
        /// </summary>
        [ConfigurationProperty("QueueType", DefaultValue = "Asynk.Library.Queues.XmlQueue, Asynk.Library, Version=0.1.0.0, Culture=neutral, PublicKeyToken=791535ff581e7a7b")]
        public String QueueTypeValue
        {
            get { return (string)this["QueueType"]; }
            set { this["QueueType"] = value; }
        }

        /// <summary>
        /// The Type of the type of queue that should be used
        /// </summary>
        public Type QueueType
        {
            get { return Type.GetType((string)this["QueueType"]); }
            set { this["QueueType"] = value.FullName; }
        }

        /// <summary>
        /// The path where to find the queue. The format of the Path will depend
        /// on the type of queue being used. See examples for typical Paths
        /// </summary>
        /// <example>
        /// XmlQueue: Path to the file - "C:\XmlQueue.queue"
        /// TODO: NOT YET IMPLEMENTED: MSMQ: Name of the MSMQ: ".\Private$\MSMQQueue"
        /// TODO: NOT YET IMPLEMENTED: Service Broker: Modified connection string
        /// </example>
        [ConfigurationProperty("Path")]
        public string Path
        {
            get { return (string)this["Path"]; }
            set { this["Path"] = value; }
        }
    }

    /// <summary>
    /// The Read Queue configuration
    /// </summary>
    public class DefaultQueue : Queues 
    {
        /// <summary>
        /// Maximum number of failed messages that will be push back 
        /// on the queue before going to the failed queue
        /// </summary>
        [ConfigurationProperty("MaxFailedMessages", DefaultValue = 5)]
        public int MaxFailedMessages
        {
            get { return (int)this["MaxFailedMessages"]; }
            set { this["MaxFailedMessages"] = value; }
        }
    }

    /// <summary>
    /// The Failed Queue configuration
    /// </summary>
    public class FailedQueue : Queues { }
}
