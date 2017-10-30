using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;


namespace MTV.EventDequeuer.Host
{
    [RunInstaller(true)]
    public partial class EventDequeuerHostInstaller : System.Configuration.Install.Installer
    {
        public EventDequeuerHostInstaller()
        {
            Install();
        }

        public void Install()
        {
            Installers.Add(new ServiceInstaller
            {
                ServiceName = "MTV.EventDequeuer.Host",
                DisplayName = "MTV.EventDequeuer.Host",
                Description = "Primary entry point to MTV.EventDequeuer.Host Service",
                StartType = ServiceStartMode.Automatic
            });

            Installers.Add(new ServiceProcessInstaller { Account = ServiceAccount.LocalSystem });
        }
    }
}
