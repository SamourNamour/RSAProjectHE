
#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: DummyCommandIntegrationExtension.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.-.- Using Directive(s) -.-.-.-.-.-.-.-.-.-.-
using System;
using System.Linq;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using MTV.Library.Core.Extensions;
using MTV.Library.Core;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Data;
using MTV.Library.Core.Instrumentation;
using MTV.Library.Core.Services;
using MTV.Scheduler.App.UI;
#endregion 

namespace MTV.EventDispatcher.Service.Extensions.DummyCmdExtension
{
    /// <summary>
    /// 
    /// </summary>
    public class DummyCommandIntegrationExtension : IExtension
    {
        #region -.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-

        private bool parameters = true;
        #endregion

        #region -.-.-.-.-.-.-.-.-.- Class : Constructor(s) / Finalizer(s) -.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        public DummyCommandIntegrationExtension() {

            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(manager_EPGAdd);
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void manager_EPGAdd(object sender, EPGInfoEventArgs e) {

           if (parameters)
            {
               
                int _ischeduleID = 0;
                string msg = string.Empty;

                // check if string is NULL OR Empty
                if (string.IsNullOrEmpty(e.EpgItem.ID))
                {
                    e.EpgItem.DummyCommandStatus = EpgDummyCommandStatus.NAK;
                    msg = string.Format(@"Invalid Dummy-Command ! ScheduleID is NULL OR EMPTY");
                    MainForm.LogWarningToFile(msg);
                    return;
                }

                // Try to convert
                try
                {
                    _ischeduleID = Int32.Parse(e.EpgItem.ID);
                }
                catch
                {
                    _ischeduleID = -1;
                }

                // Well converted ??
                if (_ischeduleID < 0)
                {
                    e.EpgItem.DummyCommandStatus = EpgDummyCommandStatus.NAK;
                    msg = string.Format(@"Invalid Dummy-Command! ScheduleID < 0 Parse ERROR.");
                    MainForm.LogWarningToFile(msg);
                    return;
                }

                bool lockAcquired = MEBSCatalogProvider.AttemptAcquireDummyCommandLock(_ischeduleID);
                
                if (!lockAcquired) return;

                string reply = PlayoutCommandProvider.SendAutomaticRecordingStartCommand(e.EpgItem, CatchupTVCommandType.DUMMY);

                e.EpgItem.DummyCommandStatus = (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) == 0 ?
                                                EpgDummyCommandStatus.ACK :
                                                EpgDummyCommandStatus.NAK);

                MEBSCatalogProvider.UpdateDummyCommandStatus(_ischeduleID, e.EpgItem.DummyCommandStatus);
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.- Class : IExtension Member(s) -.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "Dummy-Command integration Extension."; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IUIExtension UIExtension
        {
            get
            {
                return null;
            }
        }
        #endregion
    }
}
