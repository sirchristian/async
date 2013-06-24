using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Asynk.Tests
{
    class Program
    {
        /// <summary>
        /// Main entry point to the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Provided so unit tests can be run from the debugger. 
            // This is needed because the expression edition of C# does not
            // provide the "attach to process" feature.

            NUnit.ConsoleRunner.Runner.Main(args);
        }
    }
}
