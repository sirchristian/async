using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Asynk.Library.Settings;

namespace Asynk.Library.Queues
{
    /// <summary>
    /// A default queue that can be used on a text file
    /// NOT RECOMMENDED FOR PRODUCTION USE
    /// </summary>
    /// <remarks>
    /// This is not recommended for production as it does
    /// not take into account any of system locks that can
    /// be placed on a file. 
    /// </remarks>
    public class XmlQueue : IAsynkQueue
    {
        // Private variables
        private List<AsynkQueueItem> _queue;
        private DataContractSerializer _dcs;
        private DateTime _queueLastWriteTime = DateTime.MinValue;

        /// <summary>
        /// The xml file that will get serialized into. 
        /// </summary>
        /// <remarks>
        /// For the most part this should be left alone and only used in a 
        /// config file.  However unit tests are give the option to set it.
        /// Once a config file is in place though this may get marked private.
        /// </remarks>
        internal string XmlFile
        {
            get 
            {
                if (string.IsNullOrEmpty(_xmlFile))
                {
                    _xmlFile = "_queue.xml";
                }
                return _xmlFile; 
            }
            set
            {
                _xmlFile = value;

                // reinitialization is required.
                LoadQueueFromDisk();
            }
        }
        private string _xmlFile = AsynkConfig.QueueSettings.DefaultQueue.Path;

        /// <summary>
        /// Default constructor for the XmlQueue
        /// </summary>
        public XmlQueue()
        {
            LoadQueueFromDisk();
        }

        /// <summary>
        /// Public Constructor that can set the path to the Xml File
        /// </summary>
        /// <param name="queuePath"></param>
        public XmlQueue(string queuePath)
        {
            XmlFile = queuePath;
        }

        #region IAsynkQueue Members

        /// <summary>
        /// Adds an entry to the XML file
        /// </summary>
        /// <param name="item"></param>
        public void Push(AsynkQueueItem item)
        {
            // Make sure the memory queue is up to date
            LoadQueueFromDisk();

            // Add the item if the items is not in the queue already
            if (!_queue.Contains(item, new AsynkQueueEqualityComparer()))
                _queue.Add(item);

            // Save it back to disk
            SaveQueueToDisk();
        }

        /// <summary>
        /// Removes an item from the XML file
        /// </summary>
        /// <returns></returns>
        public AsynkQueueItem Pop()
        {
            // Get latest
            LoadQueueFromDisk();

            if (_queue.Count <= 0)
                return null;

            AsynkQueueItem returnItem = _queue[0];
            _queue.RemoveAt(0);

            // Save the file to disk
            SaveQueueToDisk();

            return returnItem;
        }

        /// <summary>
        /// Returns the number of items in the queue
        /// </summary>
        public int Count()
        {
            LoadQueueFromDisk();

            return _queue.Count;
        }

        /// <summary>
        /// Returns null
        /// </summary>
        public string Category
        {
            get { return null; }
            set { throw new NotImplementedException(); }
        }

        #endregion

        #region Private Methods - Read and Save from Disk

        /// <summary>
        /// Private helper method to put the queue into memory from disk
        /// </summary>
        private void LoadQueueFromDisk()
        {
            // Short circuit. No need to read from disk if we have the latest data
            if (File.GetLastWriteTime(XmlFile) <= _queueLastWriteTime)
                return;

            // Set up the serializer for the queue
            _dcs = new DataContractSerializer(typeof(List<AsynkQueueItem>));

            // First get the existing queue
            if (File.Exists(XmlFile))
            {
                // There is already a "queue" get it.
                using (FileStream fs = new FileStream(XmlFile, FileMode.OpenOrCreate))
                {
                    _queue = _dcs.ReadObject(fs) as List<AsynkQueueItem>;
                }
            }
            else
            {
                // No "queue" create a new one
                _queue = new List<AsynkQueueItem>();
            }

            _queueLastWriteTime = File.GetLastWriteTime(XmlFile);
        }

        /// <summary>
        /// Saves the queue to disk
        /// </summary>
        private void SaveQueueToDisk()
        {
            // Save the file to disk
            using (FileStream fs = new FileStream(XmlFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _dcs.WriteObject(fs, _queue);
            }
        }

        #endregion
    }
}
