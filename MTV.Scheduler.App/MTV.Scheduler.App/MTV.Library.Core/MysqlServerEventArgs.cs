using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTV.Library.Core
{
    public  class MysqlServerEventArgs : EventArgs 
    {
             private bool mysqlserverStatus;
             public MysqlServerEventArgs(bool statusServer)
            {
                mysqlserverStatus = statusServer;
            }
            public bool MysqlServerStatus
            {
                get { return mysqlserverStatus; }
                set { mysqlserverStatus = value; }
            }
    }
}
