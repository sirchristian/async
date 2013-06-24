using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.Reflection;
using System.IO;
using System.Collections;
using System.Threading;
using System.Configuration.Install;

namespace Asynk.Application
{
    /// <summary>
    /// Main class for the Asynk application
    /// </summary>
    public class Console
    {
        /// <summary>
        /// Service class for the Aynk application. 
        /// </summary>
        public class Service : ServiceBase
        {
            /// <summary>
            /// Called on service start
            /// </summary>
            /// <param name="args"></param>
            protected override void OnStart(string[] args)
            {
                Log.SetLogsToFile();
                Worker worker = new Worker();
                WaitCallback workerCallback = new WaitCallback(worker.Work);
                ThreadPool.QueueUserWorkItem(workerCallback);
            }

            /// <summary>
            /// Called on service stop
            /// </summary>
            protected override void OnStop()
            {
                Worker.Stop();
            }
        }

        /// <summary>
        /// Main entry point to the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args == null || args.Length == 0)
            {
                System.ServiceProcess.ServiceBase[] ServicesToRun;

                ServicesToRun = new
                    System.ServiceProcess.ServiceBase[] { new Service() };
                System.ServiceProcess.ServiceBase.Run(ServicesToRun);  
            }
            else if (args.Length == 1)
            {
                // strip the first char and make sure string is lower case.
                string argument = args[0].Substring(1).ToLower();

                // if the argument does not sart with - or / show the help
                if (args[0][0] != '/' && args[0][0] != '-')
                {
                    argument = "help";
                }

                switch (argument)
                {
                    case "debug":
                        Log.SetLogsToConsole();
                        Worker worker = new Worker();
                        WaitCallback workerCallback = new WaitCallback(worker.Work);
                        ThreadPool.QueueUserWorkItem(workerCallback);
                        WaitForKeyPress();
                        Worker.Stop();
                        break;
                    case "install":
                        IDictionary installState = new Hashtable();
                        TransactedInstaller installer = GetInstaller();
                        installer.Install(installState);
                        break;
                    case "uninstall":
                        TransactedInstaller uninstaller = GetInstaller();
                        uninstaller.Uninstall(null);
                        break;
                    case "cat":
                        // split the string
                        string[] categories = args[0].Split('=')[1].Split(new char[] { ',', ';' });

                        // start a thread for each category
                        foreach (string category in categories)
                        {
                            Worker catWorker = new Worker();
                            WaitCallback catWorkerCallback = new WaitCallback(catWorker.Work);
                            ThreadPool.QueueUserWorkItem(catWorkerCallback, categories);
                        }
                        WaitForKeyPress();
                        Worker.Stop();
                        break;
                    default:
                        PrintHelp();
                        break;
                }
            }
            else
            {
                PrintHelp();
            }
        }

        /// <summary>
        /// When running in console mode we need to sit in a loop
        /// </summary>
        private static void WaitForKeyPress()
        {
            string _title = System.Console.Title;
            System.Console.Title = "PRESS A KEY TO STOP RUNNING";
            System.Console.ReadKey(true);
            System.Console.Title = _title;
        }

        /// <summary>
        /// Construct and return an the installer for this exe
        /// </summary>
        /// <returns></returns>
        private static TransactedInstaller GetInstaller()
        {
            TransactedInstaller installer = new TransactedInstaller();
            installer.Installers.Add(new AsynkInstaller());
            String path = String.Format("/assemblypath={0}", Assembly.GetExecutingAssembly().Location);
            installer.Context = new InstallContext("", new string[] { path });

            return installer;
        }

        /// <summary>
        /// Prints the help for this application
        /// </summary>
        private static void PrintHelp()
        {
            Assembly current = Assembly.GetExecutingAssembly();

            using (Stream helpTxt = current.GetManifestResourceStream("Asynk.Application.Help.txt"))
            {
                using (StreamReader sr = new StreamReader(helpTxt))
                {
                    // Find the text we said we would ignore
                    string txt = sr.ReadToEnd();
                    int idx = txt.IndexOf("|----------- DO NOT TYPE ANYTHING LONGER THEN THIS LINE (80 CHARS) ------------|") + 80;

                    // Print only the text we care about
                    System.Console.WriteLine(txt.Substring(idx));
                }
            }   
        }
    }
}
