#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerInfoEventArgs : EventArgs
    {
        #region Fields
        private event_t eventInfo_t;
        private bool willStart;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public TriggerInfoEventArgs(event_t e)
        {
            this.eventInfo_t = e;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="willStart"></param>
        public TriggerInfoEventArgs(event_t e, bool willStart)
            : this(e)
        {
            this.willStart = willStart;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 
        /// </summary>
        public event_t EventInfo
        {
            get { return eventInfo_t; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WillStart
        {
            get { return willStart; }
        }
        #endregion
    }
}
