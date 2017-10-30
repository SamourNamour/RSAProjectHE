using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace MTV.Catalog.Host
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        public ProjectInstaller()
        {
            //InitializeComponent();
            Install();
        }

        public void Install()
        {
            Installers.Add(new ServiceInstaller
            {
                ServiceName = "MTV.Catalog.Host",
                DisplayName = "MTV.Catalog.Host",
                Description = "Primary entry point toMTV.Catalog.Host Service",
                StartType = ServiceStartMode.Automatic
            });

            Installers.Add(new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem });
        }
    }
}
