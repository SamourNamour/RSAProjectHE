using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.EventDequeuer.Contracts.Data;

namespace MTV.Scheduler.App.MTV.Library.Core
{
    public class StreamingResultEventArgs : System.EventArgs
    {
        /// <summary>
        /// 
        /// </summary>
        public RealTimeEventMsg Msg
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notification"></param>
        public StreamingResultEventArgs(RealTimeEventMsg notification)
        {
            this.Msg = notification;
        }
    }
}
