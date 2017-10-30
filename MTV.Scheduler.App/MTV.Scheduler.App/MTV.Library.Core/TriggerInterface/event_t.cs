
#region Copyright Motive Television 2011
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: event_t.cs
//
#endregion

#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Threading;
//using DAL = Bestv.Library.DataAccess;
//using CMEScheduler;
using MTV.Library.Core.TriggerInterface;
using MTV.Library.Core.Common;
//using Logger.HELogger;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Data;
//using CMEScheduler.Core.MESCatalog;
using System.Threading.Tasks;
using MTV.Library.Core.Services;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://www.harris.com")]
    public class event_t
    {
        #region Custorm Attribut(s)
        private EventState _state;
        private Exception lastError;
        private Thread mainThread;
        private field_t[] fieldField;
        private string statusMessage;
        #endregion

        #region Custom Property(ies)
        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public EventState EventStateProp
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Exception LastErrorProp
        {
            get
            {
                return lastError;
            }
            set
            {
                lastError = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public string StatusMessage
        {
            get
            {
                return statusMessage;
            }
            set
            {
                statusMessage = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public Thread MainThread
        {
            get
            {
                return mainThread;
            }
            set
            {
                mainThread = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("field", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public field_t[] field
        {
            get
            {
                return this.fieldField;
            }
            set
            {
                this.fieldField = value;
            }
        }
        #endregion

        #region HeadEnd Property(ies)

        #endregion

        #region Event(s)
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler StateChanged;
        #endregion

        #region Main Method(s)

        /// <summary>
        /// Set Trigger Event Status.
        /// </summary>
        /// <param name="value">EventState</param>
        private void SetState(EventState value)
        {
            _state = value;
            OnStateChanged();
        }

        /// <summary>
        /// On State Changed event.
        /// </summary>
        protected virtual void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Main method.
        /// </summary>
        public void Start() {
            string msg;
            bool bTime, bDuration;
            TimeSpan tsDuration;
            DateTime startDateTime;
            event_t EventInfo = this;
            TriggerEntry triggerMapping = null;

            //1 Set Status : EventState.Prepared
            SetState(EventState.Prepared);

            try {
                msg = string.Empty;
                bTime = bDuration = false;

                //2 Get UDP message TITLE Attribut :
                string title = UDPmsg_t_Provider.GetTITLE(EventInfo);

                //3 Get UDP message VIDEO_ITEM Attribut :
                string videoItem = UDPmsg_t_Provider.GetVIDEO_ITEM(EventInfo);

                //4 Get UDP message Time Attribut & Set trigger start datetime:
                bTime = Utils.TryParseTime(UDPmsg_t_Provider.GetTIME(EventInfo),
                                           videoItem,
                                           out startDateTime);

                //5 Get UDP message DURATION Attribut :
                bDuration = Utils.TryParseTime(UDPmsg_t_Provider.GetDURATION(EventInfo),
                                               videoItem,
                                               out tsDuration);

                //6 Get UDP message BUS Attribut :
                string bus = UDPmsg_t_Provider.GetBUS(EventInfo);

                //7 Get UDP message TYPE_MATERIAL Attribut :
                MaterialType triggerType = UDPmsg_t_Provider.GetTYPE_MATERIAL_Enum(EventInfo);

                if (triggerType == MaterialType.U) {
                    msg = string.Format("Error :: The event associated to MappingCode [{0}] and Start datetime [{1}] has an invalid TYPE_MATERIAL [{2}].",
                                         videoItem,
                                         startDateTime,
                                         triggerType);
                    //-----.DefaultLogger.STARLogger.Warn(msg);
                }

                if (bTime &&
                    bDuration) {
                    startDateTime = Utils.NextTimeOfDayAfter(startDateTime.TimeOfDay, DateTime.UtcNow, TriggerEntry.TriggerGracePeriod);
                    triggerMapping = new TriggerEntry();
                    triggerMapping.BUS = bus;
                    triggerMapping.DURATION = tsDuration;
                    triggerMapping.TIME = startDateTime;
                    triggerMapping.TITLE = title;
                    triggerMapping.TypeOfMaterial = triggerType;
                    triggerMapping.VIDEO_ITEM = videoItem;
                    try {

                        //-----.DefaultLogger.STARLogger.Warn(string.Format("TypeOfMaterial = {0}",triggerMapping.TypeOfMaterial.ToString()));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("VIDEO_ITEM = {0}", triggerMapping.VIDEO_ITEM));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("TIME = {0}", triggerMapping.TIME.ToString()));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("DURATION = {0}", triggerMapping.DURATION.ToString()));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("BUS = {0}", triggerMapping.BUS));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("VIDEO_INTIME = {0}", triggerMapping.VIDEO_INTIME.ToString()));
                        //-----.DefaultLogger.STARLogger.Warn(string.Format("TITLE = {0}", triggerMapping.TITLE));                        

                        string body = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                             triggerMapping.TypeOfMaterial.ToString().PadRight(15),
                             triggerMapping.VIDEO_ITEM.PadRight(25),
                             triggerMapping.TIME.ToString().PadRight(25),
                             triggerMapping.DURATION.ToString().PadRight(20),
                             triggerMapping.BUS.PadRight(15),
                             triggerMapping.VIDEO_INTIME.ToString().PadRight(15),
                             triggerMapping.TITLE.PadRight(100));
                        //-----.DefaultLogger.STARLogger.Info(body);
                        //-----.Log.ChannelLoger(triggerMapping.BUS, body);
                    }
                    catch (Exception exInternal) {

                        //-----.DefaultLogger.STARLogger.Error("Exception", exInternal);
                    }

                    StartPrepared(triggerMapping);
                }
                else {
                    SetState(EventState.Wrong);
                    lastError = new FormatException("String cannot be parsed to a valid TIMESPAN. Styles is not a valid DateTimeStyles value -or- styles contains an invalid combination of DateTimeStyles values.");
                }
            }
            catch (Exception ex) {
                //-----.DefaultLogger.STARLogger.Error("Exception", ex);
                SetState(EventState.Error);
            }
        }

        /// <summary>
        /// Launch SendNotificationToEBSECW treatment.
        /// </summary>
        /// <param name="sender">object : UDPmsg_t_Mapping</param>
        private void StartPrepared(object sender)
        {
            mainThread = new Thread(new ParameterizedThreadStart(EntryPointV4));
            mainThread.Start(sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        public void EntryPointV4(object sender) {
            DateTime utcNow = DateTime.UtcNow;
            string msg = string.Empty;
            EpgEntry epgEntryInstance = null;
            EpgEntry unstartedEpgEntry = null;
            TriggerEntry objTriggerEntry = sender as TriggerEntry;
            string reply = string.Empty;

            if (objTriggerEntry != null) {
                switch (objTriggerEntry.TypeOfMaterial) {

                    case MaterialType.C:
                    case MaterialType.J:
                    case MaterialType.L:
                    case MaterialType.M:
                    case MaterialType.O:
                    case MaterialType.P:
                    case MaterialType.S:
                    case MaterialType.T:
                    case MaterialType.U:                        
       
                        // A :
                        epgEntryInstance = EPGManager.Instance.GetRunningEpgEventByTriggerEntry(objTriggerEntry);
                        if (epgEntryInstance != null) {
                            if (string.Compare(epgEntryInstance.NextEpgCloserVideoItem, objTriggerEntry.VIDEO_ITEM, true) == 0) {
                                epgEntryInstance.State = EpgStatus.WaitingForStop;
                                Segment newIncomingSegment = new Segment {
                                    BUS = objTriggerEntry.BUS,
                                    TITLE = objTriggerEntry.TITLE,
                                    TIME = objTriggerEntry.TIME.TimeOfDay,
                                    VIDEO_ITEM = objTriggerEntry.VIDEO_ITEM,
                                    Start_tc = DateTime.MinValue,
                                    DURATION = objTriggerEntry.DURATION,
                                    MaterialType = objTriggerEntry.TypeOfMaterial
                                };
                                epgEntryInstance.StopEpg(newIncomingSegment);
                            }
                            else {                                
                                epgEntryInstance.LastHarrisTrigger = objTriggerEntry;                                
                                bool isDuplicateTrigger = epgEntryInstance.IsDuplicateSegment(objTriggerEntry);
                                DateTime replyTime = DateTime.MinValue;
                                if (!isDuplicateTrigger) {
                                    if (objTriggerEntry.TypeOfMaterial == MaterialType.C ||
                                        objTriggerEntry.TypeOfMaterial == MaterialType.S ||
                                        objTriggerEntry.TypeOfMaterial == MaterialType.T ||
                                        objTriggerEntry.TypeOfMaterial == MaterialType.P) {

                                        reply = PlayoutCommandProvider.SendAutomaticRecordingStartCommand(epgEntryInstance, CatchupTVCommandType.ADVERTISEMENT);
                                        replyTime = DateTime.UtcNow;
                                    }
                                }
                                else {
                                    //-----.DefaultLogger.STARLogger.Error("Duplicate Trigger is detected.");
                                }
                                
                                bool isBelongsToCurrentEpg = epgEntryInstance.LysisLinkedSegmentIDCollection.Contains(objTriggerEntry.VIDEO_ITEM);
                                if (isBelongsToCurrentEpg) {

                                    Segment unstartedSegment = epgEntryInstance.UnstartedRequestedSegment;
                                    if (unstartedSegment != null) {
                                        unstartedSegment.TITLE = objTriggerEntry.TITLE;
                                        unstartedSegment.TIME = objTriggerEntry.TIME.TimeOfDay;
                                        unstartedSegment.MaterialType = objTriggerEntry.TypeOfMaterial;
                                        unstartedSegment.PlayoutReply = reply;
                                        unstartedSegment.PlayoutReplyTime = replyTime;

                                        epgEntryInstance.ResumeEpg(unstartedSegment);
                                    }
                                    else {
                                        Segment newIncomingSegment = new Segment {
                                            BUS = objTriggerEntry.BUS,
                                            TITLE = objTriggerEntry.TITLE,
                                            TIME = objTriggerEntry.TIME.TimeOfDay,
                                            VIDEO_ITEM = objTriggerEntry.VIDEO_ITEM,
                                            Start_tc = DateTime.MinValue,
                                            MaterialType = objTriggerEntry.TypeOfMaterial,
                                            PlayoutReply = reply,
                                            PlayoutReplyTime = replyTime
                                        };
                                        if (isDuplicateTrigger)
                                            newIncomingSegment.Status = SegmentState.Duplicate;
                                        epgEntryInstance.AddNewSegment(newIncomingSegment);

                                        if (!isDuplicateTrigger) {
                                            epgEntryInstance.ResumeEpg(newIncomingSegment);
                                        }
                                    }

                                }
                                else {
                                    Segment newIncomingSegment = new Segment {
                                        BUS = objTriggerEntry.BUS,
                                        TITLE = objTriggerEntry.TITLE,
                                        TIME = objTriggerEntry.TIME.TimeOfDay,
                                        VIDEO_ITEM = objTriggerEntry.VIDEO_ITEM,
                                        Start_tc = DateTime.MinValue,
                                        MaterialType = objTriggerEntry.TypeOfMaterial,
                                        PlayoutReply = reply,
                                        PlayoutReplyTime = replyTime
                                    };
                                    if (isDuplicateTrigger)
                                        newIncomingSegment.Status = SegmentState.Duplicate;
                                    epgEntryInstance.AddNewSegment(newIncomingSegment);

                                    if (!isDuplicateTrigger) {
                                        epgEntryInstance.ResumeEpg(newIncomingSegment);
                                    }
                                }
                            }
                        }
                        
                        // B : 
                        unstartedEpgEntry = EPGManager.Instance.GetUnstartedEpgEventByTriggerEntry(objTriggerEntry);
                        if (unstartedEpgEntry != null) {

                            if (epgEntryInstance != null) {
                                Segment newIncomingSegment = new Segment { BUS = objTriggerEntry.BUS,
                                                                           TITLE = objTriggerEntry.TITLE,
                                                                           TIME = objTriggerEntry.TIME.TimeOfDay,
                                                                           VIDEO_ITEM = objTriggerEntry.VIDEO_ITEM,
                                                                           Start_tc = DateTime.MinValue,
                                                                           DURATION = objTriggerEntry.DURATION,
                                                                           MaterialType = objTriggerEntry.TypeOfMaterial
                                                                         };
                                epgEntryInstance.StopEpg(newIncomingSegment);
                            }
                            bool lockAcquired = MEBSCatalogProvider.AttemptAcquireScheduleLock(Int32.Parse(unstartedEpgEntry.ID));

                            //-----.DefaultLogger.CONCURRENCYLogger.Debug(string.Format("Automatic-recording :: lockAcquired {0} for Schedule {1}",
                            //-----.                                                     lockAcquired,
                            //-----.                                                    unstartedEpgEntry.ID));
                            if (!lockAcquired) {
                                // Update Schedule Status :
                                unstartedEpgEntry.State = EpgStatus.Aborted;

                                // Update Trigger Satus
                                SetState(EventState.Failed);                              
                                return;                                
                            }

                            unstartedEpgEntry.LastHarrisTrigger = objTriggerEntry;
                            unstartedEpgEntry.FirstTriggerReachHETime = utcNow;

                            reply = PlayoutCommandProvider.SendAutomaticRecordingStartCommand(unstartedEpgEntry,
                                                                                              CatchupTVCommandType.ACTUAL);
                            //-----.DefaultLogger.TRIGGERLogger.Debug("AutorecCmdProvider.SendAutomaticRecordingStartCommand(epgEntryInstance) = " + reply);
                            if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) == 0) {
                                unstartedEpgEntry.ExactStartDateTime = objTriggerEntry.TIME;
                                unstartedEpgEntry.Start();
                                MEBSCatalogProvider.EditSchedule(Int32.Parse(unstartedEpgEntry.ID),
                                                                 DateTime.UtcNow,
                                                                 ScheduleStatus.STARTED,
                                                                 PlayoutCommandProvider.IsAutomaticTrigger(objTriggerEntry.TIME));
                                SetState(EventState.succes);
                            }
                            else {
                                unstartedEpgEntry.State = EpgStatus.Failed;
                                MEBSCatalogProvider.EditSchedule(Int32.Parse(unstartedEpgEntry.ID),
                                                                 DateTime.MinValue,
                                                                 ScheduleStatus.FAILED_START,
                                                                 PlayoutCommandProvider.IsAutomaticTrigger(objTriggerEntry.TIME));
                                SetState(EventState.Failed);
                            }

                        }
                        else {                            
                            SetState(EventState.Failed);
                        }
                        break;

                    default:
                        SetState(EventState.Failed);
                        break;
                }
            }
            else {
                msg = "Cannot parse object to CMEScheduler.Core.TriggerEntry";
                //-----.DefaultLogger.TRIGGERLogger.Error(msg);
            }
        }

        /// <summary>
        /// Kill ended trigger either successfully or with error.
        /// </summary>
        public void Ended()
        {
            if (mainThread != null)
            {
                mainThread.Abort();

                // Call Abort a second time if the threads have not aborted.
                if ((mainThread.ThreadState &
                    (ThreadState.Aborted | ThreadState.Stopped)) == 0)
                {
                    mainThread.Abort();
                    //-----.DefaultLogger.GARBAGELogger.Debug(string.Format("Trigger Thread |{0}| is cleaned up",
                    //-----.                                                 mainThread.Name));
                }
                // Wait for the threads to terminate.
                mainThread.Join(TimeSpan.FromSeconds(1));
                mainThread = null;
            }
        }

        #endregion
    }
}
