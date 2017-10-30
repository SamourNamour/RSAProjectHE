using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Library.Core
{
    public class ProcessEventArgs : EventArgs
    {
        #region Fields

        private ProcessTagInfo processTagInfo;
        private bool willStart;

        #endregion

         public ProcessEventArgs(ProcessTagInfo processTagInformation)
        {
            this.processTagInfo = processTagInformation;
        }

         public ProcessEventArgs(ProcessTagInfo processTagInformation, bool willStart)
             : this(processTagInformation)
        {
            this.willStart = willStart;
        }

         public ProcessTagInfo ProcessTagInfo
        {
            get { return processTagInfo; }
        }

        public bool WillStart
        {
            get { return willStart; }
        }	

    }
}
