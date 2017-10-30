using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using MTV.EventDequeuer.Service.Services.Contracts;
using Castle.MicroKernel.Registration;
using MTV.EventDequeuer.Service.Services.Implementation;

namespace MTV.EventDequeuer.Service.IOC
{
    public class StandardIOCInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.IConfigurationStore store)
        {
            container.Register(
                
                Component.For<IXmlParser>().ImplementedBy<XmlParser>()
            );
        }
    }
}
