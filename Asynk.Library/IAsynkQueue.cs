using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Asynk.Library
{
    /// <summary>
    /// Interface that is used by the Asynk.Application to work with a Queue
    /// </summary>
    public interface IAsynkQueue
    {
        /// <summary>
        /// Push an item on the Queue
        /// </summary>
        /// <param name="item"></param>
        void Push(AsynkQueueItem item);

        /// <summary>
        /// Pop an item from the Queue
        /// </summary>
        /// <returns></returns>
        AsynkQueueItem Pop();

        /// <summary>
        /// Returns the number of items in the queue
        /// </summary>
        /// <returns></returns>
        int Count();

        /// <summary>
        /// Defines an optional category of items in this queue, the queue worker can be given a 
        /// category name to work with. Set to null or empty to have any available queue work on it.
        /// </summary>
        string Category { get; set; }
    }
}
