using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asynk.Library;
using Asynk.Library.Queues;
using System.Threading;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using Asynk.Library.Settings;

namespace Asynk.Application
{
    /// <summary>
    /// This is the main worker class for the Asynk application.
    /// It can take items off the queue and do the work requested
    /// </summary>
    public class Worker
    {
        /// <summary>
        /// Private static variable that thread will look for to see if they should shutdown
        /// </summary>
        private static bool _isRunning = false;

        /// <summary>
        /// A dictionary of instantiated queue 
        /// </summary>
        private static Dictionary<string, IAsynkQueue> _queues = new Dictionary<string,IAsynkQueue>();
        private static object _queuesLock = new object();

        /// <summary>
        /// Generate a guid to be use for the all category key
        /// </summary>
        private static readonly string AllCategoryKey = Guid.NewGuid().ToString("N");

        /// <summary>
        /// Works on a category
        /// </summary>
        /// <param name="arg"></param>
        public void Work(object arg)
        {
            // Set running as we enter the worker loop
            _isRunning = true;

            try
            {
                string category = arg as string;
                string key = category;
                if (category == null)
                {
                    Log.Trace("Looking at all Categories");
                    Thread.CurrentThread.Name = "AllCategoryWorker";
                    key = AllCategoryKey;
                }
                else
                {
                    Log.Trace("Looking at the {0} category", category);
                    Thread.CurrentThread.Name = string.Format("{0}Worker", category);
                }

                // Figure out if we already instanciated this queue
                if (!_queues.ContainsKey(key))
                {
                    lock (_queuesLock)
                    {
                        if (!_queues.ContainsKey(key))
                        {
                            _queues.Add(key,
                                Activator.CreateInstance(AsynkConfig.QueueSettings.DefaultQueue.QueueType, null) as IAsynkQueue);
                        }
                    }

                    if (_queues[key] == null)
                        throw new ApplicationException("Configured Queue does not implement IAsynkQueue");
                }

                // Get the queue
                IAsynkQueue queue = _queues[key];
               
                AsynkQueueItem item = null;
                try
                {
                    while (item == null)
                    {
                        // Check our running status
                        if (!_isRunning) return;

                        // Wait for an item to show up.
                        item = queue.Pop();

                        // No item, sleep for a while 
                        if (item == null) Thread.Sleep(1000);
                    }

                    Log.Trace("Received item {0}.", item.Id);

                    Assembly assembly = Assembly.Load(item.AssemblyName);
                    Log.Trace("Loaded assembly {0}", item.AssemblyName);
                    Type type = assembly.GetType(item.FullTypeName);
                    Log.Trace("Got type {0}", type.FullName);
                    MethodInfo methodInfo = type.GetMethod(item.MethodName);

                    object obj = null;
                    if (item.Obj != null)
                    {
                        obj = Deserialize(item.Obj);
                    }

                    object[] args = null;
                    if (item.Arguments != null && item.Arguments.Count > 0)
                    {
                        args = new object[item.Arguments.Count];
                        for (int x = 0; x < item.Arguments.Count; x++)
                        {
                            args[x] = Deserialize(item.Arguments[x]);
                        }
                    }

                    Log.Trace("Calling {0}, with object {1}, and args {2}", item.MethodName, obj, args);
                    item.Called();
                    methodInfo.Invoke(obj, args);
                }
                catch (Exception ex)
                {
                    // Log the error
                    Log.Error(ex.ToString());

                    // See if we can do anything
                    if (item != null)
                    {
                        Log.Trace("Error with item {0}", item.Id);

                        if (item.TimesCalled < AsynkConfig.QueueSettings.DefaultQueue.MaxFailedMessages)
                        {
                            Log.Trace("Item was only called {0} times. Pushing back on queue", item.TimesCalled);
                            queue.Push(item);
                        }
                        else
                        {
                            Log.Error("Tried too many times. Removing item from queue and putting on FailedQueue.");

                            // Get the failed queue
                            IAsynkQueue failedQueue = Activator.CreateInstance(
                                AsynkConfig.QueueSettings.FailedQueue.QueueType, 
                                AsynkConfig.QueueSettings.FailedQueue.Path) as IAsynkQueue;

                            if (failedQueue != null)
                            {
                                failedQueue.Push(item);
                            }
                        }
                    }
                }

                // Allow a new worker to start
                WaitCallback workerCallback = new WaitCallback(Work);
                ThreadPool.QueueUserWorkItem(workerCallback);
            }
            catch (Exception ex) // Outer try/catch
            {
                // Any exception here and we cannot continue
                Log.Error(ex.ToString());
                _isRunning = false;
                Log.Error("Unrecoverable error. Quiting");
            }
        }

        /// <summary>
        /// Called when stopping
        /// </summary>
        public static void Stop()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Helper method to deserialize the strings into objects
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private object Deserialize(string s)
        {
            // Currently using binary serialization since this has a better
            // chance at working then other forms of serialization. 
            // However it may be desireable to have the args in a more
            // human readable format so they can be manipulated at hand at
            // runtime

            using (MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(s)))
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    return formatter.Deserialize(ms);
                }
                catch (SerializationException)
                {
                    throw;
                }
            }
        }
    }
}
