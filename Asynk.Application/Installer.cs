using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceProcess;
using System.ComponentModel;
using System.Configuration.Install;
using Asynk.Library;

namespace Asynk.Application
{
    /// <summary>
    /// The installer class for the application
    /// </summary>
    [RunInstaller(true)]
    public class AsynkInstaller : Installer
    {
        /// <summary>
        /// Constructor for the installer
        /// </summary>
        public AsynkInstaller()
        {
            ServiceInstaller aSynkInstaller = new ServiceInstaller();
            aSynkInstaller.DisplayName = "ASynk Service";
            aSynkInstaller.Description = "A service that enables you to run your code in an asynchronous manner. See " + Constants.WebSite;
            aSynkInstaller.ServiceName = "asyncsvc";

            Installers.Add(aSynkInstaller);

            ServiceProcessInstaller aSynkProcessInstaller = new ServiceProcessInstaller();
            // TODO: Allow configuration of credentials. 
            aSynkProcessInstaller.Account = ServiceAccount.LocalSystem;

            Installers.Add(aSynkProcessInstaller);
        }
    }
}
