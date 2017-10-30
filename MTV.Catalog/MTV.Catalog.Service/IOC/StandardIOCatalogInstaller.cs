using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;


namespace MTV.Catalog.Service.IOC
{
    public class StandardIOCatalogInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, Castle.MicroKernel.IConfigurationStore store)
        {
            container.Register(

                //Component.For<IXmlParser>().ImplementedBy<XmlParser>()
            );
        }
    }
}
