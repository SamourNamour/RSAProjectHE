#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: DataCastCommandScheduleIntegrationExtension.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.- Class : Namespace (s) -.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using MTV.Library.Core;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Data;
using MTV.Library.Core.Services;
using MTV.Library.Core.Extensions;
using MTV.Scheduler.App.UI;
#endregion

namespace MTV.Scheduler.App.MTV.EventDispatcher.Service.Extensions.DataCastCommandScheduleExtension
{
    public class DataCastCommandScheduleIntegrationExtension : IExtension
    {
        #region -.-.-.-.-.-.-.-.-.- Class : Constructor(s) / Finalizer(s) -.-.-.-.-.-.-.-.-.-
        public DataCastCommandScheduleIntegrationExtension()
        {
            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(manager_EPGAdd);
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void manager_EPGAdd(object sender, EPGInfoEventArgs e)
        {

            switch (e.EpgItem.EventMediaType)
            {
                case MediaType.UNKNOWN_CHANNEL:
                    break;

                case MediaType.TV_CHANNEL:
                    break;

                case MediaType.DC_CHANNEL:
                    string reply = string.Empty;
                    e.EpgItem.StateChanged += delegate(object s, EventArgs eventArgs)
                    {
                        switch (e.EpgItem.State)
                        {
                            case EpgStatus.Preparing: //------ OK
                                bool result = PlayoutCommandProvider.UploadMPEG7File(e.EpgItem);
                                if (result)
                                {
                                    e.EpgItem.MEPG7ProcessingStatus = MEPG7Status.UPLOADED;
                                    result = PlayoutCommandProvider.UploadDCCommandFile(e.EpgItem);
                                    if (result)
                                        e.EpgItem.DCCommandProcessingStatus = DCCommandStatus.UPLOADED;
                                    else
                                        e.EpgItem.DCCommandProcessingStatus = DCCommandStatus.NAK;
                                }
                                else
                                {
                                    e.EpgItem.MEPG7ProcessingStatus = MEPG7Status.NAK;
                                }

                                break;

                            case EpgStatus.Running: //----- OK
                                e.EpgItem.ExactStartDateTime = DateTime.UtcNow;
                                MEBSCatalogProvider.EditSchedule(Int32.Parse(e.EpgItem.ID),
                                                                 DateTime.UtcNow,
                                                                 ScheduleStatus.STARTED,
                                                                 true);
                                break;
                            case EpgStatus.Stopped: //----- OK
                                    MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                                  DateTime.UtcNow,
                                                                                  ScheduleStatus.STOPPED);

                              
                                break;
                            case EpgStatus.Prepared: //------ OK
                                reply = PlayoutCommandProvider.CreateEncapsulatorMPEG7XmlFile(e.EpgItem);
                                if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK, true) == 0)
                                {
                                    e.EpgItem.MEPG7ProcessingStatus = MEPG7Status.ACK;
                                }
                                else
                                {
                                   e.EpgItem.MEPG7ProcessingStatus = MEPG7Status.NAK;
                                    PlayoutCommandProvider.DeleteMPEG7XmlFile(e.EpgItem);
                                }
            
                                break;
                            case EpgStatus.Waiting: //------ OK
                                reply = PlayoutCommandProvider.SendDatacastCommand(e.EpgItem);
                                e.EpgItem.LastDatacastCmdSent = DateTime.UtcNow;
                                e.EpgItem.FirstTriggerReachHETime = DateTime.UtcNow;
                                e.EpgItem.TypeOfTrigger = TriggerType.Automatic;
                                if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK, true) == 0)
                                {
                                    e.EpgItem.DCCommandProcessingStatus = DCCommandStatus.ACK;
                                }
                                else
                                {
                                    e.EpgItem.DCCommandProcessingStatus = DCCommandStatus.NAK;
                                    // Remove ScheduleItem from Encapsolator (PID = 86)
                                    PlayoutCommandProvider.DeleteMPEG7XmlFile(e.EpgItem); 
                                }
                                break;
                            case EpgStatus.Aborted: //----- OK
                                MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                              DateTime.MinValue,
                                              ScheduleStatus.FAILED_START);
                                break;
                            default:
                                break;
                        }
                    };

                    break;

                case MediaType.TRAILER_CHANNEL:
                    break;

                case MediaType.BONUS_CHANNEL:
                    break;

                case MediaType.ADVERTISEMENT_CHANNEL:
                    break;

                default:
                    break;
            }
            
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.- Class : IExtension Members -.-.-.-.-.-.-.-.-.-
        public string Name
        {
            get { return "DataCast-Command integration Extension."; }
        }

        public IUIExtension UIExtension
        {
            get {
                return null;
            }
        }
        #endregion
    }
}
