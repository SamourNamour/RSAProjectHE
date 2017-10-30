using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTV.Library.Core
{
    public class CatalogServiceEventArgs : EventArgs
    {
            private bool catalogServiceStatus;
            
            public CatalogServiceEventArgs(bool statusServer)
            {
                catalogServiceStatus = statusServer;
            }
             public bool CatalogServiceStatus
            {
                get { return catalogServiceStatus; }
                set { catalogServiceStatus = value; }
            }

    }
}
