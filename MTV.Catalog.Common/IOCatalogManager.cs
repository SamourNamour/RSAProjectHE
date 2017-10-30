using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;

namespace MTV.Catalog.Common
{
    public sealed class IOCatalogManager
    {
        #region Data
        static readonly IOCatalogManager instance = new IOCatalogManager();
        private volatile object syncLock = new object();

        /// <summary>
        /// Castle container
        /// </summary>
        private static IWindsorContainer container = null;


        #endregion

        #region Ctor
        //DO NOT REMOVE
        //presence of the static constructor will remove the BeforeFieldInit 
        //attribute on the class and guarantee that no fields are initialized 
        //until the first static field, property, or method is called.
        //
        //Which is what makes the singleton instance work
        static IOCatalogManager()
        {

        }

        private IOCatalogManager()
        {

        }
        #endregion

        #region Public Properties
        public static IOCatalogManager Instance
        {
            get
            {
                if (container == null)
                {
                    container =
                        new WindsorContainer();
                }
                return instance;
            }
        }

        public IWindsorContainer Container
        {
            get
            {
                return container;
            }
        }

        #endregion
    }
}
