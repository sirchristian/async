using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Asynk.Library;
using Asynk.Library.Queues;

using NUnit.Framework;

namespace Asynk.Tests.Application.Queues
{
    /// <summary>
    /// Tests the XmlQueue
    /// </summary>
    [TestFixture]
    public class XmlQueueTests
    {
        private static XmlQueue _queue = new XmlQueue();
        private readonly string testFile = "testqueue.xml";

        /// <summary>
        /// Setup the test queue
        /// </summary>
        [TestFixtureSetUp]
        public void Setup()
        {
            // To make sure we are nice and clean before running the test we
            // will just call TearDown
            TearDown();

            // Now setup 
            _queue.XmlFile = testFile;
        }

        /// <summary>
        /// Clean up the queue
        /// </summary>
        [TestFixtureTearDown]
        public void TearDown()
        {
            if (File.Exists(testFile))
                File.Delete(testFile);
        }

        /// <summary>
        /// Tests the Pop operation
        /// </summary>
        [Test]
        public void PopTest()
        {
            AsynkQueueItem _myItem = new AsynkQueueItem();
            
            _myItem.AssemblyName = Assembly.GetExecutingAssembly().FullName;
            _myItem.MethodName = "PushTest";
            _myItem.Arguments = new List<string>();
            _myItem.Arguments.Add("<Value>value1</Value>");

            _queue.Push(_myItem);

            Assert.That(_queue.Count() == 1);

            AsynkQueueItem returnedItem = _queue.Pop();

            Assert.That(returnedItem == _myItem);
            Assert.That(_queue.Count() == 0);
        }

        /// <summary>
        /// Tests the Push operation
        /// </summary>
        [Test]
        public void PushTest()
        {
            // Make sure we have an empty queue to start
            Assert.That(_queue.Count() == 0);

            AsynkQueueItem _myItem = TestHelpers.GenerateNewTestItem();

            // Make sure we have one item
            _queue.Push(_myItem);
            Assert.That(_queue.Count() == 1);

            // Try to push the same item. Still should have 1 item
            _queue.Push(_myItem);
            Assert.That(_queue.Count() == 1);

            // Push a new item. Queue should have 2
            _queue.Push(TestHelpers.GenerateNewTestItem());
            Assert.That(_queue.Count() == 2);
        }

        /// <summary>
        /// Tests that the xml queue does not work on a category
        /// </summary>
        [Test, ExpectedException(typeof(NotImplementedException))]
        public void TestNullCategory()
        {
            Assert.That(_queue.Category == null);

            _queue.Category = "ThisThrowsAnException";
        }
    }
}
