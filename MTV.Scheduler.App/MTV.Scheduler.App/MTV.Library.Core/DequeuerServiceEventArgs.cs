using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTV.Library.Core
{
    public class DequeuerServiceEventArgs : EventArgs 
    {
             private bool dequeuerServiceStatus;
             public DequeuerServiceEventArgs(bool statusServer)
            {
                dequeuerServiceStatus = statusServer;
            }
             public bool DequeuerServiceStatus
            {
                get { return dequeuerServiceStatus; }
                set { dequeuerServiceStatus = value; }
            }
    }
}
