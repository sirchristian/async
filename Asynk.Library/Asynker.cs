using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Runtime.Serialization;
using Asynk.Library.Queues;
using Asynk.Library.Settings;
using System.Text.RegularExpressions;

namespace Asynk.Library
{
    /// <summary>
    /// Static class that provides some methods that can be used with Asynk
    /// TODO: Give this a better name
    /// </summary>
    public static class Asynker
    {
        /// <summary>
        /// Custom type to hold all the information about a method that 
        /// will be in the method registry
        /// </summary>
        private struct AsynkMethodInfo
        {
            public Assembly assembly;
            public Type type;
            public MethodInfo methodInfo;
        }

        /// <summary>
        /// The method registry contains the method name and the full method name. 
        /// This allows use to be called easier from the client.
        /// </summary>
        private static Dictionary<string, AsynkMethodInfo> _methodRegistry;


        /// <summary>
        /// Static constructor. Creates a registry of method names that can be called.
        /// </summary>
        static Asynker()
        {
            _methodRegistry = new Dictionary<string, AsynkMethodInfo>();

            // Loop through all loaded assemblies
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                // Short circuit assemblies that match an ignore filter
                if (Regex.IsMatch(assembly.FullName, AsynkConfig.Misc.IgnoreFilter))
                    continue;

                // ..And each type in each assembly
                foreach (Type type in assembly.GetTypes())
                {
                    // ..And each method in each type
                    foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
                    {
                        object[] attributes = methodInfo.GetCustomAttributes(false);
                        if (attributes.Length > 0)
                        {
                            // ..And any attributes defined
                            foreach (object o in attributes)
                            {
                                AsynkCallableAttribute attr = o as AsynkCallableAttribute;
                                if (attr != null)
                                {
                                    // Method has the AsynkCallable attribute.
                                    // Adding to method registry.
                                    AsynkMethodInfo ami = new AsynkMethodInfo();
                                    ami.assembly = assembly;
                                    ami.type = type;
                                    ami.methodInfo = methodInfo;
                                    _methodRegistry.Add(methodInfo.Name, ami);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// This is the main entry point to call a method asynchrously. This
        /// call will find the method from the calling assembly.
        /// </summary>
        /// <example>
        /// // Change:
        /// method01(arg1, arg2);
        /// // to:
        /// Asynker.Process(this, "method01", arg1, arg2);
        /// </example>
        /// <param name="obj">The object to work on, use null for static method</param>
        /// <param name="methodName"></param>
        /// <param name="args"></param>
        public static Guid Process(object obj, string methodName, params object[] args)
        {
            // Make sure we have the method.
            if (!_methodRegistry.Keys.Contains(methodName))
                throw new ApplicationException(
                    string.Format("Cannot find {0} in the method registry. Does {0} have the AsynkCallableAttribute?", 
                        methodName));

            // Get the method
            AsynkMethodInfo ami = _methodRegistry[methodName];

            // Create an item to put on the queue
            AsynkQueueItem queueItem = new AsynkQueueItem();

            queueItem.AssemblyName = ami.assembly.FullName;
            queueItem.MethodName = ami.methodInfo.Name;
            queueItem.FullTypeName = ami.type.FullName;

            // Serialize the object 
            if (obj != null)
            {
                queueItem.Obj = Serialize(obj);
            }

            // Serialize the arguments
            if (args != null && args.Length > 0)
            {
                queueItem.Arguments = new List<string>();

                foreach (object arg in args)
                {
                    string s = Serialize(arg);
                    queueItem.Arguments.Add(s);
                }
            }

            // Get the queue we should be using
            IAsynkQueue queue = Activator.CreateInstance(AsynkConfig.QueueSettings.DefaultQueue.QueueType, null) as IAsynkQueue;
            if (queue == null)
                throw new ApplicationException("Configured Queue does not implemente IAsynkQueue");

            // Push the item onto the queue
            queue.Push(queueItem);

            // Return the Id just in case the caller wants to keep track
            return queueItem.Id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">The object to work on, use null for static method</param>
        /// <param name="methodName"></param>
        /// <param name="containingAssembly"></param>
        /// <param name="args"></param>
        public static Guid Process(object obj, string methodName, Assembly containingAssembly, params object[] args)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Helper method to serialize an object
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        private static string Serialize(object o)
        {
            // Currently using binary serialization since this has a better
            // chance at working then other forms of serialization. 
            // However it may be desireable to have the args in a more
            // human readable format so they can be manipulated at hand at
            // runtime

            using (MemoryStream ms = new MemoryStream())
            {
                try
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(ms, o);
                    ms.Position = 0;
                    byte[] b = new byte[ms.Length];
                    ms.Read(b, 0, b.Length);

                    return Encoding.Default.GetString(b);
                }
                catch (SerializationException e)
                {
                    string message = string.Format(
                        "Cannot serialize {0}. All arguments and obj must be serializable", o.ToString());
                    throw new SerializationException(message, e);
                }
            }
        }
    }

}
