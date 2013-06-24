using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Asynk.Library;
using System.Reflection;

namespace Asynk.Tests
{
    /// <summary>
    /// Constains methods to assist in testing
    /// </summary>
    public static class TestHelpers
    {
        /// <summary>
        /// Constructs a test Queue Item
        /// </summary>
        /// <returns></returns>
        public static AsynkQueueItem GenerateNewTestItem()
        {
            AsynkQueueItem _myItem = new AsynkQueueItem();

            // Get the current executing assmbly to use at the assembly name
            _myItem.AssemblyName = Assembly.GetExecutingAssembly().FullName;

            // Walk up the call stack and find the caller's method name
            _myItem.MethodName = new System.Diagnostics.StackFrame(2).GetMethod().Name;

            // Use dummy arguments
            _myItem.Arguments = new List<string>();
            _myItem.Arguments.Add("<Value>value1</Value>");

            // Use this type name
            _myItem.FullTypeName = typeof(TestHelpers).FullName;

            return _myItem;
        }
    }
}
