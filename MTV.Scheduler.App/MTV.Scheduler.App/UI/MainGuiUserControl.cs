
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: MainGuiUserControl.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Threading;

// Custom Directive(s)
using System.Data.Services.Client;
using System.Runtime.Remoting.Messaging;
using System.Collections.ObjectModel;
using MTV.Scheduler.App.Properties;
using MTV.Library.Core;
using MTV.Library.Core.Tools;
using MTV.Library.Core.Data;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Services;
using MTV.Library.Core.Common;
using MTV.EventDispatcher.Service.Extensions.DuplexCallBackNotificationExtension;
#endregion

namespace MTV.Scheduler.App.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainGuiUserControl : UserControl
    {
        #region - Field(s) -
        Hashtable mapItemToEpgEntry = new Hashtable();
        Hashtable mapEpgEntryToItem = new Hashtable();
        ListViewItem lastSelection = null;
        delegate string SendAutomaticRecordingCommandEventHandler(EpgEntry epgEntryItem);
        static object verrou = new object();
        #endregion

        #region - Event(s) -

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler SelectionChange;
        event AsyncCompletedEventHandler SendAutomaticRecordingCommandCompleted;
        #endregion

        #region - Control(s) Event(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popUpContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListViewAutorec_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            UpdateAdvertisements();
            UpdateSegments();
            UpdateUI();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //richTextBoxStartNotification.Clear();
        }

        #endregion

        #region - Class Member(s) -

        #endregion

        #region - Property(ies) -

        #endregion

        #region - Constructor(s) / Finalizer(s) -

        /// <summary>
        /// 
        /// </summary>
        public MainGuiUserControl()
        {

            InitializeComponent();
            ListViewAutorec.SetPrettyStyle();
           // ListViewSegment.SetPrettyStyle();

            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGAdded);
            EPGManager.Instance.StartEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGStarted);
            EPGManager.Instance.RemoveEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGRemoved);
            EPGManager.Instance.EndEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGEnded);
            ProcessTagInfoManager.Instance.ProcessAdded += new EventHandler<ProcessEventArgs>(Instance_ProcessAdded);
            ProcessTagInfoManager.Instance.ProcessEnded += new EventHandler<ProcessEventArgs>(Instance_ProcessEnded);
            ProcessTagInfoManager.Instance.ProcessFailed += new EventHandler<ProcessEventArgs>(Instance_ProcessFailed);
#if Duplex_V2
            //DuplexCallBackNotificationIntegrationExtension.CallBackEvent += new EventHandler<CallBackEventArgs>(Instance_CallBackEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextOpeningEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpeningEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextOpenedEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpenedEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextClosingEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosingEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextClosedEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosedEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextFaultedEvent += new EventHandler<EventArgs>(Instance_InstanceContextFaultedEvent);
#else
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextOpeningEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpeningEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextOpenedEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpenedEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextClosingEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosingEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextClosedEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosedEvent);
            //DuplexCallBackNotificationIntegrationExtension.InstanceContextFaultedEvent += new EventHandler<EventArgs>(Instance_InstanceContextFaultedEvent);

            //DBNotificationReceiver.Instance.CallBackEvent += new EventHandler<CMEScheduler.Extension.DBNotificationExtension.EventArgs.CallBackEventArgs>(Instance_CallBackEvent);
            //DBNotificationReceiver.Instance.InstanceContextOpeningEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpeningEvent);
            //DBNotificationReceiver.Instance.InstanceContextOpenedEvent += new EventHandler<EventArgs>(Instance_InstanceContextOpenedEvent);
            //DBNotificationReceiver.Instance.InstanceContextClosingEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosingEvent);
            //DBNotificationReceiver.Instance.InstanceContextClosedEvent += new EventHandler<EventArgs>(Instance_InstanceContextClosedEvent);
            //DBNotificationReceiver.Instance.InstanceContextFaultedEvent += new EventHandler<EventArgs>(Instance_InstanceContextFaultedEvent);
#endif

            SendAutomaticRecordingCommandCompleted += new AsyncCompletedEventHandler(MainGuiUserControl_SendAutomaticRecordingCommandCompleted);

            using (EPGManager.Instance.LockDownloadList(false))
            {
                if (EPGManager.Instance.EpgCollection != null &&
                    EPGManager.Instance.EpgCollection.Count > 0)
                {
                    foreach (EpgEntry item in EPGManager.Instance.EpgCollection)
                    {
                        AddEPG(item);
                    }
                }
            }
        }

        #endregion

        #region - Callback(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_CallBackEvent(object sender,
                                    CallBackEventArgs e) {
            string msg = string.Empty;
            if (e.objNotification != null) {
                msg = String.Format("Last Locked Schedule Notification Time (from Callback Pusher Service) = {0}.",
                                     e.objNotification.LastLockedScheduleNotificationTime);
                //DefaultLogger.DUPLEXLogger.Debug(msg);
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnSelectionChange()
        {
            if (SelectionChange != null)
            {
                SelectionChange(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryItem"></param>
        void SendAutomaticRecordingStopCommandAsynch(EpgEntry epgEntryItem)
        {
            SendAutomaticRecordingCommandEventHandler worker = PlayoutCommandProvider.SendAutomaticRecordingStopCommand;
            AsyncCallback completedCallback = new AsyncCallback(SendAutomaticRecordingStopCommandCallback);
            AsyncOperation async = AsyncOperationManager.CreateOperation(null);
            worker.BeginInvoke(epgEntryItem,
                               completedCallback,
                               async);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ar"></param>
        void SendAutomaticRecordingStopCommandCallback(IAsyncResult ar)
        {
            // get the original worker delegate and the AsyncOperation instance
            SendAutomaticRecordingCommandEventHandler worker = (SendAutomaticRecordingCommandEventHandler)((AsyncResult)ar).AsyncDelegate;
            AsyncOperation async = (AsyncOperation)ar.AsyncState;

            // finish the asynchronous operation
            string reply = worker.EndInvoke(ar);

            // raise the completed event
            AsyncCompletedEventArgs completedArgs = new AsyncCompletedEventArgs(null, false, null);
            async.PostOperationCompleted(delegate(object e)
            {
                OnMyTaskCompleted((AsyncCompletedEventArgs)e);
            },
                                         completedArgs
                                         );
        }

        /// <summary>
        /// /
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMyTaskCompleted(AsyncCompletedEventArgs e)
        {
            if (SendAutomaticRecordingCommandCompleted != null)
            {
                SendAutomaticRecordingCommandCompleted(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainGuiUserControl_SendAutomaticRecordingCommandCompleted(object sender, AsyncCompletedEventArgs e)
        {
            // check on status
            /*
                       LogAutomationSystemNotification(String.Format("Event  ({0}) Stoped", e.EpgItem.ID.ToString()),
                                            LogMode.Information);
            LogAutomationSystemNotification(String.Format("Sending Stop command for Event  ({0})", e.EpgItem.ID.ToString()),
                                            LogMode.Information);

            string reply = string.Empty;
            string msg = string.Empty;
            try {

                reply = AutorecCmdProvider.SendAutomaticRecordingStopCommand(e.EpgItem);

            }
            finally {
                if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK)==0) {
                    msg = string.Format("Event : {0} - has been successfully stoped ({1}) - [{2}]",
                                         e.EpgItem.OriginalTitle,
                                         e.EpgItem.ID,
                                         reply);

                    DefaultLogger.logger.Debug(msg);
                    e.EpgItem.ExactStopDateTime = DateTime.UtcNow;
                    LogAutomationSystemNotification(msg, LogMode.Information);
                    MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                  DateTime.UtcNow,
                                                                  ScheduleStatus.STOPPED);
                }
                else {
                    msg = string.Format("An error has been occured during trying to stop Event : {0} ({1}) - [{2}]",
                                          e.EpgItem.OriginalTitle,
                                          e.EpgItem.ID,
                                          reply);
                    DefaultLogger.logger.Error(msg);
                    LogAutomationSystemNotification(msg, LogMode.Error);
                    MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                              DateTime.MinValue,
                                              ScheduleStatus.FAILED_STOP);
                }
            }
             */
        }

        #endregion

        #region - Delegate Invoker(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGAdded(object sender, EPGInfoEventArgs e)
        {            
            string msg = string.Format("New {0} Entry [Title :: {1}] - [ID :: {2}] - [{3}-{4}] just been added.",
                                        e.EpgItem.EventMediaType.ToString(),
                                        e.EpgItem.OriginalTitle,
                                        e.EpgItem.ID,
                                        e.EpgItem.EstimatedStartDateTime,
                                        e.EpgItem.EstimatedStopDateTime
                                        );
          
            MainForm.LogMessageToFile(msg);

            LogNotification(msg, LogMode.Information);
            if (IsHandleCreated)
            {
                this.BeginInvoke((MethodInvoker)delegate() { AddEPG(e.EpgItem); });
            }
            else
            {
                AddEPG(e.EpgItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGStarted(object sender, EPGInfoEventArgs e)
        {
            string msg = string.Format("Sending TV manual recording command {0}.",                                                                                
                                        e.EpgItem.ContentID                                   
                                      );
            //DefaultLogger.STARLogger.Debug(msg);

           // LogAutomationSystemNotification(msg, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGRemoved(object sender, EPGInfoEventArgs e)
        {
            this.BeginInvoke(
                (MethodInvoker)delegate()
            {
                ListViewItem item = mapEpgEntryToItem[e.EpgItem] as ListViewItem;
                if (item != null)
                {
                    if (item.Selected)
                    {
                        lastSelection = null;
                        ListViewSegment.Items.Clear();
                    }
                    mapEpgEntryToItem[e.EpgItem] = null;
                    mapItemToEpgEntry[item] = null;
                    item.Remove();
                }
            }
          );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGEnded(object sender, EPGInfoEventArgs e) {
            lock (verrou) {
                switch (e.EpgItem.State) {
                    case EpgStatus.Unstarted:
                        break;

                    case EpgStatus.Waiting:
                        break;

                    case EpgStatus.Running:
                        break;

                    case EpgStatus.Suspended:
                        break;

                    case EpgStatus.Stopped:
                        if (e.EpgItem.ExactStopDateTime != DateTime.MinValue) {
                            //LogAutomationSystemNotification(String.Format("{0} Event  ({1}) has been already stopped.",
                            //                                               e.EpgItem.EventMediaType.ToString(),
                            //                                               e.EpgItem.ID),
                            //                                LogMode.Error);
                            return;
                        }
                        break;

                    case EpgStatus.Aborted:
                        break;

                    case EpgStatus.Unknowned:
                        break;

                    case EpgStatus.Failed:
                        break;

                    case EpgStatus.Prepared:
                        break;

                    case EpgStatus.WaitingForStop:
                        break;

                    default:
                        break;
                }

                //LogAutomationSystemNotification(String.Format("{0} Event  ({1}) Stoped.",
                //                                               e.EpgItem.EventMediaType.ToString(),
                //                                               e.EpgItem.ID),
                //                                LogMode.Information);

                switch (e.EpgItem.EventMediaType) {

                    case MediaType.UNKNOWN_CHANNEL:
                        break;

                    case MediaType.TV_CHANNEL:
                        //LogAutomationSystemNotification(String.Format("Sending Stop command for Event {0} ({1}).",
                        //                                               MediaType.TV_CHANNEL.ToString(),
                        //                                               e.EpgItem.ID),
                        //                                LogMode.Information);

                        string reply = string.Empty;
                        string msg = string.Empty;
                        try {
                            reply = PlayoutCommandProvider.SendAutomaticRecordingStopCommand(e.EpgItem);
                        }
                        finally {
                            if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) == 0) {
                                e.EpgItem.ExactStopDateTime = DateTime.UtcNow;
                                msg = string.Format("Sending TV channel stop command for {0}. (Recording time {1} m).",
                                                     e.EpgItem.ContentID,
                                                     (int)e.EpgItem.ExactStopDateTime.Subtract(e.EpgItem.ExactStartDateTime).TotalMinutes);

                                //DefaultLogger.STARLogger.Debug(msg);

                                LogAutomationSystemNotification(msg, LogMode.Information);
                                MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                              DateTime.UtcNow,
                                                                              ScheduleStatus.STOPPED);
                            }
                            else {
                                msg = string.Format("An error has been occured during trying to stop Event : {0} ({1}) - [{2}]",
                                                      e.EpgItem.OriginalTitle,
                                                      e.EpgItem.ID,
                                                      reply);
                                //DefaultLogger.STARLogger.Error(msg);
                                LogAutomationSystemNotification(msg, LogMode.Error);
                                MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                              DateTime.UtcNow,
                                                                              ScheduleStatus.FAILED_STOP);
                            }
                        }
                        break;

                    case MediaType.DC_CHANNEL:

                        if (e.EpgItem.MEPG7ProcessingStatus == MEPG7Status.ACK &&
                            e.EpgItem.DCCommandProcessingStatus == DCCommandStatus.ACK) {
                            //LogAutomationSystemNotification(String.Format("Encapsulator Streaming for Event {0} ({1} - {2}) is ended.",
                            //                                               MediaType.DC_CHANNEL.ToString(),
                            //                                               e.EpgItem.ID,
                            //                                               e.EpgItem.OriginalTitle),
                            //                                               LogMode.Information);
                            MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                          DateTime.UtcNow,
                                                                          ScheduleStatus.STOPPED);
                        }
                        else {
                            //LogAutomationSystemNotification(String.Format("Event {0} ({1} - {2}) streaming process failed.",
                            //                                               MediaType.DC_CHANNEL.ToString(),
                            //                                               e.EpgItem.ID,
                            //                                               e.EpgItem.OriginalTitle),
                            //                                LogMode.Error);
                            MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                          DateTime.UtcNow,
                                                                          ScheduleStatus.FAILED_START);
                        }

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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentEnded(object sender, SegmentEventArgs e)
        {
            //LogAutomationSystemNotification(String.Format("{0} Event segment ({1}) ended.", 
            //                                               MediaType.TV_CHANNEL.ToString(),
            //                                               e.Segment.VIDEO_ITEM),
            //                                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentFailed(object sender, SegmentEventArgs e)
        {
            //LogAutomationSystemNotification(String.Format("{0} Event segment ({1}) Failed.", 
            //                                               MediaType.TV_CHANNEL.ToString(),
            //                                               e.Segment.VIDEO_ITEM),
            //                                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentStarted(object sender, SegmentEventArgs e)
        {
            //LogAutomationSystemNotification(String.Format("{0} Event segment ({1}) started.", 
            //                                               MediaType.TV_CHANNEL.ToString(),
            //                                               e.Segment.VIDEO_ITEM),
            //                                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessAdded(object sender, ProcessEventArgs e)
        {
            string msg = string.Format("New Product received: {0} - the process has been successfully started.", e.ProcessTagInfo.FileName);
            //DefaultLogger.JADELogger.Debug(msg);
            LogFileNotfication(msg, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessEnded(object sender, ProcessEventArgs e)
        {
            string msg = string.Format("Product : {0} - the process has been successfully Ended.", e.ProcessTagInfo.FileName);
            //DefaultLogger.JADELogger.Debug(msg);
            LogFileNotfication(msg, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessFailed(object sender, ProcessEventArgs e)
        {
            string msg = string.Format("Product : {0} - the process has Failed : {1}", e.ProcessTagInfo.FileName, e.ProcessTagInfo.LastError.ToString());
            LogFileNotfication(msg, LogMode.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_InstanceContextFaultedEvent(object sender, EventArgs e)
        {
            LogNotification("InstanceContext Faulted.", LogMode.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_InstanceContextOpeningEvent(object sender, EventArgs e)
        {
            LogNotification("InstanceContext Opening.", LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_InstanceContextOpenedEvent(object sender, EventArgs e)
        {
            LogNotification("InstanceContext Opened.", LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_InstanceContextClosingEvent(object sender, EventArgs e)
        {
            LogNotification("InstanceContext Closing.", LogMode.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_InstanceContextClosedEvent(object sender, EventArgs e)
        {
            LogNotification("InstanceContext Closed.", LogMode.Error);
        }
        #endregion

        #region - Private Method(s) -

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        public void LogNotification(string msg, LogMode m)
        {
            try
            {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate()
                  {
                      int len = richTextBoxDuplexPushNotification.Text.Length;
                      if (len > 0)
                      {
                          richTextBoxDuplexPushNotification.SelectionStart = len;
                      }

                      if (m == LogMode.Error)
                      {
                          richTextBoxDuplexPushNotification.SelectionColor = Color.Red;
                      }
                      else
                      {
                          richTextBoxDuplexPushNotification.SelectionColor = Color.Green;
                      }

                      if (richTextBoxDuplexPushNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxDuplexPushNotification.Clear();

                      richTextBoxDuplexPushNotification.AppendText(string.Format("{0} - {1}{2}", DateTime.Now, msg, Environment.NewLine));
                  }
              );
            }
            catch (Exception ex)
            {
                //DefaultLogger.DUPLEXLogger.Error(ex);
            }
        }


       

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        void LogAutomationSystemNotification(string msg, LogMode m)
        {
            //try
            //{
            //    this.BeginInvoke(
            //        (MethodInvoker)
            //      delegate()
            //      {
            //          int len = richTextBoxStartNotification.Text.Length;
            //          if (len > 0)
            //          {
            //              richTextBoxStartNotification.SelectionStart = len;
            //          }

            //          if (m == LogMode.Error)
            //          {
            //              richTextBoxStartNotification.SelectionColor = Color.Red;
            //          }
            //          else
            //          {
            //              richTextBoxStartNotification.SelectionColor = Color.Blue;
            //          }

            //          if (richTextBoxStartNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxStartNotification.Clear();

            //          richTextBoxStartNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
            //      }
            //  );
            //}
            //catch (Exception ex)
            //{
            //    //DefaultLogger.STARLogger.Error(ex);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        void LogFileNotfication(string msg, LogMode m)
        {
            try
            {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate()
                  {
                      int len = richTextBoxFilesNotification.Text.Length;
                      if (len > 0)
                      {
                          richTextBoxFilesNotification.SelectionStart = len;
                      }

                      if (m == LogMode.Error)
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Red;
                      }
                      else
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Blue;
                      }

                      if (richTextBoxFilesNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxFilesNotification.Clear();

                      richTextBoxFilesNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch (Exception ex)
            {
                //DefaultLogger.JADELogger.Error(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEpgEntry"></param>
        void AddEPG(EpgEntry objEpgEntry)
        {
            objEpgEntry.SegmentStoped += new EventHandler<SegmentEventArgs>(epg_SegmentEnded);
            objEpgEntry.SegmentFailed += new EventHandler<SegmentEventArgs>(epg_SegmentFailed);
            objEpgEntry.SegmentStarted += new EventHandler<SegmentEventArgs>(epg_SegmentStarted);
            objEpgEntry.AdvertisementReceptionEvent += new EventHandler<AdvertisementEventArgs>(objEpgEntry_AdvertisementReceptionEvent);
            objEpgEntry.AdvertisementStartEvent += new EventHandler<AdvertisementEventArgs>(objEpgEntry_AdvertisementStartEvent);
            objEpgEntry.AdvertisementEndEvent += new EventHandler<AdvertisementEventArgs>(objEpgEntry_AdvertisementEndEvent);

            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = false;
            item.Text = objEpgEntry.OriginalTitle;

            // EstimatedStart:
            item.SubItems.Add(objEpgEntry.EstimatedStartDateTime.ToString());

            // EstimatedStop:
            item.SubItems.Add(objEpgEntry.EstimatedStopDateTime.ToString());

            // Channel:
            item.SubItems.Add(objEpgEntry.ChannelDesignation);

            // RealDuration:
            item.SubItems.Add(TimeSpanFormatter.ToString(objEpgEntry.RealDuration));

            // ElapsedDuration:
            item.SubItems.Add(TimeSpanFormatter.ToString(objEpgEntry.ElapsedDuration));

            // State:
            item.SubItems.Add(objEpgEntry.State.ToString());

            // CreatedDateTime:
            item.SubItems.Add(objEpgEntry.CreatedDateTime.ToString());

            // ContentID:
            item.SubItems.Add(objEpgEntry.ContentID.ToString());

            //MediaType:
            item.SubItems.Add(((MediaType)objEpgEntry.nMediaType.Value).ToString());

            //TriggerType:
            item.SubItems.Add(objEpgEntry.TypeOfTrigger.ToString());

            // PosterTransferState:
            item.SubItems.Add(objEpgEntry.PosterTransferStatus.ToString());

            // DummyCommandStatus:
            item.SubItems.Add(objEpgEntry.DummyCommandStatus.ToString());

            // NextEpgCloserVideoItem:
            item.SubItems.Add(objEpgEntry.NextEpgCloserVideoItem);

            // MEPG7ProcessingStatus:
            item.SubItems.Add(objEpgEntry.MEPG7ProcessingStatus.ToString());

            // DCCommandProcessingStatus:
            item.SubItems.Add(objEpgEntry.DCCommandProcessingStatus.ToString());

            // ARExtendedProcessingStatus:
            item.SubItems.Add(objEpgEntry.ARExtendedProcessingStatus.ToString());

            mapEpgEntryToItem[objEpgEntry] = item;
            mapItemToEpgEntry[item] = objEpgEntry;
            ListViewAutorec.Items.Add(item);

            SortEpgCollectionByStartTC();            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void objEpgEntry_AdvertisementEndEvent(object sender, AdvertisementEventArgs e)
        {
            LogAutomationSystemNotification(String.Format("Advertisement ({0}) ended", e.Advertisement.VIDEO_ITEM),
                                            LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void objEpgEntry_AdvertisementReceptionEvent(object sender, AdvertisementEventArgs e)
        {
            LogAutomationSystemNotification(String.Format("Advertisement ({0}) received", e.Advertisement.VIDEO_ITEM),
                                            LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void objEpgEntry_AdvertisementStartEvent(object sender, AdvertisementEventArgs e)
        {
            LogAutomationSystemNotification(String.Format("Advertisement ({0}) started", e.Advertisement.VIDEO_ITEM),
                                            LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateSegments()
        {
            try
            {
                ListViewSegment.BeginUpdate();

                if (ListViewAutorec.SelectedItems.Count == 1)
                {
                    ListViewItem newSelection = ListViewAutorec.SelectedItems[0];
                    EpgEntry e = mapItemToEpgEntry[newSelection] as EpgEntry;

                    if (lastSelection == newSelection)
                    {
                        if (e.LinkedSegmentCollection != null &&
                            e.LinkedSegmentCollection.Count > 0 &&
                            e.LinkedSegmentCollection.Count == ListViewSegment.Items.Count)
                        {
                            UpdateSegmentsWithoutInsert(e);
                        }
                        else
                        {
                            UpdateSegmentsInserting(e, newSelection);
                        }
                    }
                    else
                    {
                        UpdateSegmentsInserting(e, newSelection);
                    }
                }
                else
                {
                    lastSelection = null;

                    ListViewSegment.Items.Clear();
                }
            }
            catch (Exception ex)
            {
                LogNotification(ex.ToString(), LogMode.Error);
            }
            finally
            {
                ListViewSegment.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newSelection"></param>
        void UpdateSegmentsInserting(EpgEntry epgEntryOccurence, ListViewItem newSelection)
        {
            lastSelection = newSelection;
            ListViewSegment.Items.Clear();
            if (epgEntryOccurence.LinkedSegmentCollection != null &&
                epgEntryOccurence.LinkedSegmentCollection.Count > 0)
            {
                for (int i = 0; i < epgEntryOccurence.LinkedSegmentCollection.Count; i++)
                {
                    ListViewItem item = new ListViewItem();
                    string videoItem = epgEntryOccurence.LinkedSegmentCollection[i].VIDEO_ITEM;
                    string title = epgEntryOccurence.LinkedSegmentCollection[i].TITLE;
                    TimeSpan time = epgEntryOccurence.LinkedSegmentCollection[i].TIME;
                    TimeSpan duration = epgEntryOccurence.LinkedSegmentCollection[i].DURATION;
                    SegmentState state = epgEntryOccurence.LinkedSegmentCollection[i].Status;
                    TimeSpan totalDuration = epgEntryOccurence.LinkedSegmentCollection[i].TotalDuration;
                    int reRuns = epgEntryOccurence.LinkedSegmentCollection[i].ReRuns;
                    DateTime start_TC = epgEntryOccurence.LinkedSegmentCollection[i].Start_tc;
                    string MaterialType = EnumUtils.GetDescriptionFromEnumValue(epgEntryOccurence.LinkedSegmentCollection[i].MaterialType);
                    string playoutReply = epgEntryOccurence.LinkedSegmentCollection[i].PlayoutReply;
                    DateTime playoutReplyTime = epgEntryOccurence.LinkedSegmentCollection[i].PlayoutReplyTime;
                    item.Text = videoItem; 
                    item.SubItems.Add(title);
                    item.SubItems.Add(time.ToString());
                    item.SubItems.Add(TimeSpanFormatter.ToString(duration));
                    item.SubItems.Add(state.ToString());
                    item.SubItems.Add(TimeSpanFormatter.ToString(totalDuration));
                    item.SubItems.Add(reRuns.ToString());
                    item.SubItems.Add(start_TC.ToString());
                    item.SubItems.Add(MaterialType);
                    item.SubItems.Add(playoutReply);
                    item.SubItems.Add(playoutReplyTime.ToString());

                    ListViewSegment.Items.Add(item);
                }
                SortSegmentCollectionByStartTC();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        void UpdateSegmentsWithoutInsert(EpgEntry d) {
            using (EPGManager.Instance.LockDownloadList(false)) {

                ReadOnlyCollection<Segment> temp = d.LinkedSegmentCollectionReadOnly;
                for (int i = 0; i < ListViewSegment.Items.Count; i++) {

                    ListViewSegment.Items[i].UseItemStyleForSubItems = false;

                    ListViewSegment.Items[i].SubItems[0].Text = temp[i].VIDEO_ITEM.ToString();
                    ListViewSegment.Items[i].SubItems[1].Text = temp[i].TITLE.ToString();
                    ListViewSegment.Items[i].SubItems[2].Text = temp[i].TIME.ToString();
                    ListViewSegment.Items[i].SubItems[3].Text = TimeSpanFormatter.ToString(temp[i].DURATION);

                    if (temp[i].LastError != null) {
                        ListViewSegment.Items[i].SubItems[4].Text = temp[i].Status.ToString() + ", " + temp[i].LastError.Message;
                    }
                    else {
                        ListViewSegment.Items[i].SubItems[4].Text = temp[i].Status.ToString();
                    }

                    ListViewSegment.Items[i].SubItems[5].Text = TimeSpanFormatter.ToString(temp[i].TotalDuration);
                    ListViewSegment.Items[i].SubItems[6].Text = temp[i].ReRuns.ToString();
                    ListViewSegment.Items[i].SubItems[7].Text = temp[i].Start_tc.ToString();
                    ListViewSegment.Items[i].SubItems[8].Text = EnumUtils.GetDescriptionFromEnumValue(temp[i].MaterialType);

                    string playoutReply = temp[i].PlayoutReply;
                    ListViewSegment.Items[i].SubItems[9].Text = playoutReply;
                    if (string.Compare(playoutReply, DefaultValues.WS_APP_RESULT_OK)==0) {
                        ListViewSegment.Items[i].SubItems[9].ForeColor = Color.Green;
                    }
                    else {
                        ListViewSegment.Items[i].SubItems[9].ForeColor = Color.Red;
                    }

                    ListViewSegment.Items[i].SubItems[10].Text = temp[i].PlayoutReplyTime.ToString();

                    switch (temp[i].Status) {
                        case SegmentState.Unstarted:
                            ListViewSegment.Items[i].SubItems[4].ForeColor = Color.Silver;
                            break;

                        case SegmentState.Waiting:
                            break;

                        case SegmentState.Running:
                            ListViewSegment.Items[i].SubItems[4].ForeColor = Color.Blue;
                            break;

                        case SegmentState.Suspended:
                            break;

                        case SegmentState.Stopped:
                            ListViewSegment.Items[i].SubItems[4].ForeColor = Color.Green;
                            break;

                        case SegmentState.Aborted:
                            break;

                        case SegmentState.Unknowned:
                            break;

                        case SegmentState.Prepared:
                            ListViewSegment.Items[i].SubItems[4].ForeColor = Color.Violet;
                            break;

                        case SegmentState.Duplicate:
                            ListViewSegment.Items[i].SubItems[4].ForeColor = Color.Brown;
                            break;

                        default:
                            break;
                    }
                }
            }
            SortSegmentCollectionByStartTC();
        }

        /// <summary>
        /// 
        /// </summary>
        void UpdateAdvertisements()
        {
            //try {
            //    listViewCommericalBreak.BeginUpdate();
            //    if (ListViewAutorec.SelectedItems.Count == 1) {
            //        ListViewItem newSelection = ListViewAutorec.SelectedItems[0];
            //        EpgEntry e = mapItemToEpgEntry[newSelection] as EpgEntry;
            //        if (lastSelection == newSelection) {
            //            if (e.AdvertisementCollection != null &&
            //              e.AdvertisementCollection.Count > 0 &&
            //              e.AdvertisementCollection.Count == listViewCommericalBreak.Items.Count) {
            //                  UpdateAdvertisementsWithoutInsert(e);
            //            }
            //            else {
            //                UpdateAdvertisementsInserting(e, newSelection);
            //            }
            //        }
            //        else {
            //            UpdateAdvertisementsInserting(e, newSelection);
            //        }
            //    }
            //    else {
            //        lastSelection = null;
            //        listViewCommericalBreak.Items.Clear();
            //    }
            //}
            //finally {
            //    listViewCommericalBreak.EndUpdate();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryItem"></param>
        /// <param name="newSelection"></param>
        void UpdateAdvertisementsInserting(EpgEntry epgEntryItem, ListViewItem newSelection)
        {
            //lastSelection = newSelection;
            //listViewCommericalBreak.Items.Clear();
            //if (epgEntryItem.AdvertisementCollection != null &&
            //    epgEntryItem.AdvertisementCollection.Count > 0) {
            //    for (int i = 0; i < epgEntryItem.AdvertisementCollection.Count; i++) {
            //        ListViewItem item = new ListViewItem();
            //        item.Text = epgEntryItem.AdvertisementCollection[i].VIDEO_ITEM.ToString();
            //        item.SubItems.Add(epgEntryItem.AdvertisementCollection[i].TITLE.ToString());
            //        item.SubItems.Add(epgEntryItem.AdvertisementCollection[i].TIME.ToString());
            //        item.SubItems.Add(TimeSpanFormatter.ToString(epgEntryItem.AdvertisementCollection[i].DURATION));
            //        item.SubItems.Add(epgEntryItem.AdvertisementCollection[i].Status.ToString());
            //        item.SubItems.Add(epgEntryItem.AdvertisementCollection[i].PlayoutReply);
            //        item.SubItems.Add(epgEntryItem.AdvertisementCollection[i].PlayoutReplyAt.ToString());

            //        listViewCommericalBreak.Items.Add(item);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryItem"></param>
        void UpdateAdvertisementsWithoutInsert(EpgEntry epgEntryItem)
        {
            //for (int i = 0; i < epgEntryItem.AdvertisementCollection.Count; i++) {
            //    listViewCommericalBreak.Items[i].SubItems[0].Text = epgEntryItem.AdvertisementCollection[i].VIDEO_ITEM;
            //    listViewCommericalBreak.Items[i].SubItems[1].Text = epgEntryItem.AdvertisementCollection[i].TITLE;
            //    listViewCommericalBreak.Items[i].SubItems[2].Text = epgEntryItem.AdvertisementCollection[i].TIME.ToString();
            //    listViewCommericalBreak.Items[i].SubItems[3].Text = TimeSpanFormatter.ToString(epgEntryItem.AdvertisementCollection[i].DURATION);
            //    if (epgEntryItem.LinkedSegmentCollection[i].LastError != null) {
            //        listViewCommericalBreak.Items[i].SubItems[4].Text = epgEntryItem.AdvertisementCollection[i].Status.ToString() + ", " + epgEntryItem.AdvertisementCollection[i].LastError.Message;
            //    }
            //    else {
            //        listViewCommericalBreak.Items[i].SubItems[4].Text = epgEntryItem.AdvertisementCollection[i].Status.ToString();
            //    }
            //    listViewCommericalBreak.Items[i].SubItems[5].Text = epgEntryItem.AdvertisementCollection[i].PlayoutReply;
            //    listViewCommericalBreak.Items[i].SubItems[6].Text = epgEntryItem.AdvertisementCollection[i].PlayoutReplyAt.ToString();
            //}            
        }

        /// <summary>
        /// 
        /// </summary>
        void SortSegmentCollectionByStartTC()
        {
            ListViewSorter Sorter = new ListViewSorter();
            ListViewSegment.ListViewItemSorter = Sorter;
            if (!(ListViewSegment.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)ListViewSegment.ListViewItemSorter;

            Sorter.ByColumn = 2;
            ListViewSegment.Sorting = SortOrder.Descending;

            ListViewSegment.Sort();
            ListViewAutorec.SetPrettyStyle();
            ListViewSegment.SetPrettyStyle();
        }

        /// <summary>
        /// 
        /// </summary>
        void SortEpgCollectionByStartTC()
        {
            ListViewSorter Sorter = new ListViewSorter();
            ListViewAutorec.ListViewItemSorter = Sorter;
            if (!(ListViewAutorec.ListViewItemSorter is ListViewSorter))
                return;
            Sorter = (ListViewSorter)ListViewAutorec.ListViewItemSorter;

            Sorter.ByColumn = 1;
            ListViewAutorec.Sorting = SortOrder.Descending;

            ListViewAutorec.Sort();
            ListViewAutorec.SetPrettyStyle();
            ListViewSegment.SetPrettyStyle();
        }

        #endregion

        #region - Internal Method(s) -

        #endregion

        #region - Public Method(s) -

        /// <summary>
        /// 
        /// </summary>
        public void UpdateUI()
        {
            bool isSelected = ListViewAutorec.SelectedItems.Count > 0;
            isSelected = ListViewAutorec.SelectedItems.Count == 1;
            OnSelectionChange();
        }

        /// <summary>
        /// 
        /// </summary>
        public void RemoveCompleted()
        {
            ListViewAutorec.BeginUpdate();
            try
            {
                EPGManager.Instance.ClearEnded();
                UpdateList();
            }
            finally
            {
                ListViewAutorec.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateList()
        {

            for (int i = 0; i < ListViewAutorec.Items.Count; i++)
            {
                ListViewItem item = ListViewAutorec.Items[i];
                if (item == null) return;

                EpgEntry d = mapItemToEpgEntry[item] as EpgEntry;
                if (d == null) return;

                EpgStatus state;

                if (item.Tag == null) state = EpgStatus.Running;
                else state = (EpgStatus)item.Tag;

                if (state != d.State ||
                    state == EpgStatus.Running ||
                    state == EpgStatus.WaitingForStop ||
                    state == EpgStatus.Unstarted ||
                    state == EpgStatus.Prepared ||
                    state == EpgStatus.Waiting
                    )
                {
                    item.SubItems[0].Text = d.OriginalTitle.ToString();
                    item.SubItems[1].Text = d.EstimatedStartDateTime.ToString();
                    item.SubItems[2].Text = d.EstimatedStopDateTime.ToString();
                    item.SubItems[3].Text = d.ChannelDesignation.ToString();
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(d.RealDuration);
                    item.SubItems[5].Text = TimeSpanFormatter.ToString(d.ElapsedDuration);
                    if (d.LastError != null)
                    {
                        item.SubItems[6].Text = d.State.ToString() + ", " + d.LastError.Message;
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(d.StatusMessage))
                        {
                            item.SubItems[6].Text = d.State.ToString();
                        }
                        else
                        {
                            item.SubItems[6].Text = d.State.ToString() + ", " + d.StatusMessage;
                        }
                    }

                    switch (d.State)
                    {
                        case EpgStatus.Aborted:
                            item.SubItems[6].ForeColor = Color.Silver;
                            break;

                        case EpgStatus.Failed:
                            item.SubItems[6].ForeColor = Color.Red;
                            break;

                        case EpgStatus.Running:
                            item.SubItems[6].ForeColor = Color.Blue;
                            break;

                        case EpgStatus.Stopped:
                            item.SubItems[6].ForeColor = Color.Green;
                            break;

                        case EpgStatus.Unstarted:
                            item.SubItems[6].ForeColor = Color.Silver;
                            break;

                        case EpgStatus.Waiting:
                            item.SubItems[6].ForeColor = Color.Violet;
                            break;

                        case EpgStatus.Suspended:
                            break;

                        case EpgStatus.Unknowned:
                            break;

                        case EpgStatus.Prepared:
                            item.SubItems[6].ForeColor = Color.Violet;
                            break;

                        case EpgStatus.WaitingForStop:
                            item.SubItems[6].ForeColor = Color.Orange;
                            break;

                        default:
                            break;
                    }

                    item.SubItems[7].Text = d.CreatedDateTime.ToString();
                    item.SubItems[8].Text = d.ContentID.ToString();
                    item.SubItems[9].Text = ((MediaType)d.nMediaType.Value).ToString();
                    TimeSpan offset = default(TimeSpan);

                    switch (d.TypeOfTrigger)
                    {
                        case TriggerType.Default:
                            item.SubItems[10].Text = d.TypeOfTrigger.ToString();
                            break;

                        case TriggerType.Automatic:

                            offset =(d.ExactStartDateTime != DateTime.MinValue ?
                                     d.FirstTriggerReachHETime.Subtract(d.ExactStartDateTime) :
                                     TimeSpan.FromMinutes(-1));

                            item.SubItems[10].Text = string.Format("{0} ({1})",
                                                                    d.TypeOfTrigger.ToString(),
                                                                    DateTimeUtils.GenerateStringTime(offset));
                            item.SubItems[10].ForeColor = Color.Blue;
                            break;

                        case TriggerType.Manual:
                            offset = d.FirstTriggerReachHETime.Subtract(d.ExactStartDateTime);
                            item.SubItems[10].Text = string.Format("{0} ({1})",
                                                                    d.TypeOfTrigger.ToString(),
                                                                    DateTimeUtils.GenerateStringTime(offset));
                            item.SubItems[10].ForeColor = Color.Red;
                            break;

                        default:
                            break;
                    }

                    item.SubItems[11].Text = d.PosterTransferStatus.ToString().ToString();

                    item.SubItems[12].Text = d.DummyCommandStatus.ToString();

                    switch (d.EventMediaType)
                    {
                        case MediaType.UNKNOWN_CHANNEL:
                            break;
                        case MediaType.TV_CHANNEL:
                            item.SubItems[13].Text = d.NextEpgCloserVideoItem.ToString();
                            item.SubItems[14].Text = "-";
                            item.SubItems[15].Text = "-";
                            item.SubItems[16].Text = d.ARExtendedProcessingStatus.ToString();
                            break;

                        case MediaType.DC_CHANNEL:
                            item.SubItems[13].Text = "-";
                            item.SubItems[14].Text = d.MEPG7ProcessingStatus.ToString();
                            item.SubItems[15].Text = d.DCCommandProcessingStatus.ToString();
                            item.SubItems[16].Text = "-";
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



                    item.Tag = d.State;
                }
            }

            SortEpgCollectionByStartTC();

            UpdateSegments();
            UpdateAdvertisements();
        }

        /// <summary>
        /// 
        /// </summary>
        public void LoadSettingsView()
        {
            ListViewAutorec.GridLines = Settings.Default.ViewGrid;
            ListViewSegment.GridLines = Settings.Default.ViewGrid;
            splitContainer2.Panel2Collapsed = !Settings.Default.ViewScheduleDetails;
            //splitContainer2.Panel1Collapsed = !Settings.Default.ViewNotifications;
        }

        #endregion

        #region - IDisposable Member(s) -

        #endregion

        #region - Nested Class -

        /// <summary>
        /// 
        /// </summary>
        public class ListViewSorter : System.Collections.IComparer
        {
            int Column = 0;
            
            /// <summary>
            /// 
            /// </summary>
            public int ByColumn
            {
                get { return Column; }
                set { Column = value; }
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="left"></param>
            /// <param name="right"></param>
            /// <returns></returns>
            public int Compare(object left, object right)
            {
                if (!(left is ListViewItem))
                    return (0);
                if (!(right is ListViewItem))
                    return (0);

                ListViewItem lvi1 = (ListViewItem)right;
                string str1 = lvi1.SubItems[ByColumn].Text;

                ListViewItem lvi2 = (ListViewItem)left;
                string str2 = lvi2.SubItems[ByColumn].Text;

                DateTime dateX;
                DateTime dateY;

                int compareResult;
                if (lvi1.ListView.Sorting == SortOrder.Ascending) {
                    if (DateTime.TryParse(str1, out dateX) &&
                        DateTime.TryParse(str2, out dateY)) {
                        compareResult = DateTime.Compare(dateX, dateY);
                    }
                    else {
                        compareResult = String.Compare(str1, str2);
                    }
                }
                else {
                    if (DateTime.TryParse(str1, out dateX) &&
                       DateTime.TryParse(str2, out dateY)) {
                        compareResult = DateTime.Compare(dateY, dateX);
                    }
                    else {
                        compareResult = String.Compare(str2, str1);
                    }
                }

                return (compareResult);
            }
        }
        #endregion
    }
}
