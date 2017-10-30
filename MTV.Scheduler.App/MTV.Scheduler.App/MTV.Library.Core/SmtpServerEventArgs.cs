using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTV.Library.Core
{
    public class SmtpServerEventArgs : EventArgs
    {
        private bool smtpServerStatus;

          public SmtpServerEventArgs(bool statusServer)
            {
                smtpServerStatus = statusServer;
            }
            public bool SmtpServerStatus
            {
                get { return smtpServerStatus; }
                set { smtpServerStatus = value; }
            }
    }
}
