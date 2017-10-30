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
using MTV.Library.Core.Services;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Common;
using MTV.Scheduler.App.MTVControl;
using MTV.Scheduler.App.MTV.Library.Core;
#endregion

namespace MTV.Scheduler.App.UI
{
    public partial class MainUserControl : UserControl
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveCompleted();
        }
        
        public MainUserControl()
        {
            InitializeComponent();
            ListViewAutorec.SetPrettyStyle();
            
            // Content Manager
            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGAdded);
            EPGManager.Instance.StartEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGStarted);
            EPGManager.Instance.RemoveEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGRemoved);
            EPGManager.Instance.EndEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGEnded);
            
            // Product Manager
            ProcessTagInfoManager.Instance.ProcessAdded += new EventHandler<ProcessEventArgs>(Instance_ProcessAdded);
            ProcessTagInfoManager.Instance.ProcessEnded += new EventHandler<ProcessEventArgs>(Instance_ProcessEnded);
            ProcessTagInfoManager.Instance.ProcessFailed += new EventHandler<ProcessEventArgs>(Instance_ProcessFailed);

            // EventDequeuerWatch Manager
            EventDequeuerWatch.Instance.LastVideoContentLockedOnNotificationReceived += new EventHandler<StreamingResultEventArgs>(Instance_LastVideoContentLockedOnNotificationReceived);
            EventDequeuerWatch.Instance.LastVideoContentScheduledOnNotificationReceived += new EventHandler<StreamingResultEventArgs>(Instance_LastVideoContentScheduledOnNotificationReceived);
            EventDequeuerWatch.Instance.LastVideoContentRemovedOnNotificationReceived += new EventHandler<StreamingResultEventArgs>(Instance_LastVideoContentRemovedOnNotificationReceived);
            EventDequeuerWatch.Instance.LastCategoryItemsChangedOnNotificationReceived += new EventHandler<StreamingResultEventArgs>(Instance_LastCategoryItemsChangedOnNotificationReceived);


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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_LastVideoContentLockedOnNotificationReceived(object sender, StreamingResultEventArgs e)
        {
            string info = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>{0}</eventName>" +
                            "<date>{1}</date>" +
                            "</realtimeEvent>", e.Msg.EventName.ToString(), e.Msg.Date.ToString());

            LogNotification(info, LogMode.Information);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_LastVideoContentScheduledOnNotificationReceived(object sender, StreamingResultEventArgs e)
        {
            string info = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>{0}</eventName>" +
                            "<date>{1}</date>" +
                            "</realtimeEvent>", e.Msg.EventName.ToString(), e.Msg.Date.ToString());

            LogNotification(info, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_LastVideoContentRemovedOnNotificationReceived(object sender, StreamingResultEventArgs e)
        {
            string info = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>{0}</eventName>" +
                            "<date>{1}</date>" +
                            "</realtimeEvent>", e.Msg.EventName.ToString(), e.Msg.Date.ToString());

            LogNotification(info, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_LastCategoryItemsChangedOnNotificationReceived(object sender, StreamingResultEventArgs e)
        {
            string info = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>{0}</eventName>" +
                            "<date>{1}</date>" +
                            "</realtimeEvent>", e.Msg.EventName.ToString(), e.Msg.Date.ToString());

            LogNotification(info, LogMode.Information);
        }

        private void popUpContextMenu_Opening(object sender, CancelEventArgs e)
        {
            UpdateUI();
        }

        private void ListViewAutorec_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            
            UpdateSegments();
            UpdateUI();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGAdded(object sender, EPGInfoEventArgs e)
        {
            string msg = string.Format("New {0} entry [Title :: {1}] - [ID :: {2}] - [{3}-{4}] just been added.",
                                        e.EpgItem.EventMediaType.ToString(),
                                        e.EpgItem.OriginalTitle,
                                        e.EpgItem.ID,
                                        e.EpgItem.EstimatedStartDateTime,
                                        e.EpgItem.EstimatedStopDateTime
                                        );

            MainForm.LogMessageToFile(msg);

            
            LogFileNotfication(msg, LogMode.Information);
            
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
                            //ListViewSegment.Items.Clear();
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
        void Instance_EPGEnded(object sender, EPGInfoEventArgs e)
        {
            lock (verrou)
            {
                
                switch (e.EpgItem.EventMediaType)
                {

                    case MediaType.UNKNOWN_CHANNEL:
                        break;

                    case MediaType.TV_CHANNEL:
                        //LogAutomationSystemNotification(String.Format("Sending Stop command for Event {0} ({1}).",
                        //                                               MediaType.TV_CHANNEL.ToString(),
                        //                                               e.EpgItem.ID),
                        //                                LogMode.Information);

                        string reply = string.Empty;
                        string msg = string.Empty;
                        try
                        {
                            reply = PlayoutCommandProvider.SendAutomaticRecordingStopCommand(e.EpgItem);
                        }
                        finally
                        {
                            if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) == 0)
                            {
                                e.EpgItem.ExactStopDateTime = DateTime.UtcNow;
                                msg = string.Format("Sending TV channel stop command for {0}. (Recording time {1} m).",
                                                     e.EpgItem.ContentID,
                                                     (int)e.EpgItem.ExactStopDateTime.Subtract(e.EpgItem.ExactStartDateTime).TotalMinutes);

                                //DefaultLogger.STARLogger.Debug(msg);

                                //LogAutomationSystemNotification(msg, LogMode.Information);
                                MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                              DateTime.UtcNow,
                                                                              ScheduleStatus.STOPPED);
                            }
                            else
                            {
                                msg = string.Format("An error has been occured during trying to stop Event : {0} ({1}) - [{2}]",
                                                      e.EpgItem.OriginalTitle,
                                                      e.EpgItem.ID,
                                                      reply);
                                //DefaultLogger.STARLogger.Error(msg);
                                //LogAutomationSystemNotification(msg, LogMode.Error);
                                MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                                                                              DateTime.UtcNow,
                                                                              ScheduleStatus.FAILED_STOP);
                            }
                        }
                        break;

                    case MediaType.DC_CHANNEL:

                        switch (e.EpgItem.State)
                        {
                            case EpgStatus.Aborted:
                                if (e.EpgItem.LastError != null)
                                {
                                    ShowSchedulesNotifications(string.Format("{0} ", e.EpgItem.LastError.Message), LogMode.Error);
                                }
                                else
                                {
                                    ShowSchedulesNotifications(string.Format("Schedule '[{0}] - [{1}] - [{2}]' Aborted! - See Log File.", e.EpgItem.ID, e.EpgItem.ContentID.ToString(), e.EpgItem.OriginalTitle), LogMode.Error);
                                }
                                break;
                            case EpgStatus.Prepared:
                                //ShowSchedulesNotifications(string.Format("Schedule '{0}-{1}' Prepared.", e.EpgItem.ID, e.EpgItem.OriginalTitle), LogMode.Information);
                                break;
                            case EpgStatus.Preparing:
                                //ShowSchedulesNotifications(string.Format("Schedule '{0}-{1}' Preparing.", e.EpgItem.ID, e.EpgItem.OriginalTitle), LogMode.Information);
                                break;
                            case EpgStatus.Running:
                                //ShowSchedulesNotifications(string.Format("Schedule '{0}-{1}' Running.", e.EpgItem.ID, e.EpgItem.OriginalTitle), LogMode.Information);
                                break;
                            case EpgStatus.Stopped:
                                //ShowSchedulesNotifications(string.Format("Schedule '{0}-{1}' Stoped.", e.EpgItem.ID, e.EpgItem.OriginalTitle), LogMode.Information);
                                break;
                            case EpgStatus.Waiting:
                                //ShowSchedulesNotifications(string.Format("Schedule '{0}-{1}' Waiting.", e.EpgItem.ID, e.EpgItem.OriginalTitle), LogMode.Information);
                                break;
                            default:
                                break;
                        }

                        //if (e.EpgItem.MEPG7ProcessingStatus == MEPG7Status.ACK &&
                        //    e.EpgItem.DCCommandProcessingStatus == DCCommandStatus.ACK)
                        //{
                        //    //LogAutomationSystemNotification(String.Format("Encapsulator Streaming for Event {0} ({1} - {2}) is ended.",
                        //    //                                               MediaType.DC_CHANNEL.ToString(),
                        //    //                                               e.EpgItem.ID,
                        //    //                                               e.EpgItem.OriginalTitle),
                        //    //                                               LogMode.Information);
                        //    MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                        //                                                  DateTime.UtcNow,
                        //                                                  ScheduleStatus.STOPPED);
                        //}
                        //else
                        //{
                        //    //LogAutomationSystemNotification(String.Format("Event {0} ({1} - {2}) streaming process failed.",
                        //    //                                               MediaType.DC_CHANNEL.ToString(),
                        //    //                                               e.EpgItem.ID,
                        //    //                                               e.EpgItem.OriginalTitle),
                        //    //                                LogMode.Error);
                        //    MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(e.EpgItem.ID),
                        //                                                  DateTime.UtcNow,
                        //                                                  ScheduleStatus.FAILED_START);
                        //}

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
        void Instance_ProcessAdded(object sender, ProcessEventArgs e)
        {
            string msg = string.Format("New Product received | {0} ", e.ProcessTagInfo.FileName);
            LogFileNotfication(msg, LogMode.Information);
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessEnded(object sender, ProcessEventArgs e)
        {
            string msg = string.Format("Product | {0} - (the process has been successfully Ended)", e.ProcessTagInfo.FileName);
            
            LogFileNotfication(msg, LogMode.Information);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessFailed(object sender, ProcessEventArgs e)
        {
            string msg = string.Empty;

            if (e.ProcessTagInfo.LastError != null)
            {
                msg = string.Format("{0}", e.ProcessTagInfo.LastError.ToString());

            }

            else
                msg = string.Format("Unknown Error | {0} (System cannot import the product specified) - See log file.", e.ProcessTagInfo.FileName);
            
            LogFileNotfication(msg, LogMode.Error);
        }


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
            catch
            {
                
            }
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
                          richTextBoxFilesNotification.SelectionColor = Color.Green;
                          
                      }

                      if (richTextBoxFilesNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxFilesNotification.Clear();

                      richTextBoxFilesNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        public void ShowSchedulesNotifications(string msg, LogMode m)
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
                      else if (m == LogMode.Information)
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Green;
                      }
                      else if(m == LogMode.Warning)
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Orange;
                      }

                      if (richTextBoxFilesNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxFilesNotification.Clear();

                      richTextBoxFilesNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch 
            {
                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        public void LogInputOutputMsg(string msg, LogMode m)
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
                      else if (m == LogMode.Information)
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Green;
                      }
                      else if (m == LogMode.Warning)
                      {
                          richTextBoxFilesNotification.SelectionColor = Color.Orange;
                      }

                      if (richTextBoxFilesNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxFilesNotification.Clear();

                      richTextBoxFilesNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch 
            {
              
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        public void MonitroingSystemBoxAddMsg(string msg, LogMode m)
        {
            try
            {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate()
                  {
                      int len = richTextBoxMonitroingSystem.Text.Length;
                      if (len > 0)
                      {
                          richTextBoxMonitroingSystem.SelectionStart = len;
                      }

                      if (m == LogMode.Error)
                      {
                          richTextBoxMonitroingSystem.SelectionColor = Color.Red;
                      }
                      else if (m == LogMode.Information)
                      {
                          richTextBoxMonitroingSystem.SelectionColor = Color.Green;
                      }
                      else if (m == LogMode.Warning)
                      {
                          richTextBoxMonitroingSystem.SelectionColor = Color.Orange;
                      }

                      if (richTextBoxMonitroingSystem.Lines.Length >= Settings.Default.RichLogMaxLenght) richTextBoxMonitroingSystem.Clear();

                      richTextBoxMonitroingSystem.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch 
            {
                
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEpgEntry"></param>
        void AddEPG(EpgEntry objEpgEntry)
        {
           
         
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
            //item.SubItems.Add(objEpgEntry.NextEpgCloserVideoItem);

            // MEPG7ProcessingStatus:
            item.SubItems.Add(objEpgEntry.MEPG7ProcessingStatus.ToString());

            // DCCommandProcessingStatus:
            item.SubItems.Add(objEpgEntry.DCCommandProcessingStatus.ToString());

            // ARExtendedProcessingStatus:
            //item.SubItems.Add(objEpgEntry.ARExtendedProcessingStatus.ToString());

            mapEpgEntryToItem[objEpgEntry] = item;
            mapItemToEpgEntry[item] = objEpgEntry;
            ListViewAutorec.Items.Add(item);

            SortEpgCollectionByStartTC();
        }

      



        

       

        /// <summary>
        /// 
        /// </summary>
        void UpdateSegments()
        {
            
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="epgEntryOccurence"></param>
       /// <param name="newSelection"></param>
        void UpdateSegmentsInserting(EpgEntry epgEntryOccurence, ListViewItem newSelection)
        {
           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        void UpdateSegmentsWithoutInsert(EpgEntry d)
        {
            
        }


        /// <summary>
        /// 
        /// </summary>
        void SortSegmentCollectionByStartTC()
        {
            ListViewSorter Sorter = new ListViewSorter();
            ListViewAutorec.SetPrettyStyle();
           
        }

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
           
        }

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
                //Prevents the ListViewAutorec control from drawing until the EndUpdate method is called.
                ListViewAutorec.EndUpdate();
            }
        }

        /// <summary>
        ///  Update Schedule view List 
        /// </summary>
        public void UpdateList()
        {

            
            for (int i = 0; i < ListViewAutorec.Items.Count; i++)
            {
                ListViewItem item = ListViewAutorec.Items[i];
                if (item == null) return;
                
                item.UseItemStyleForSubItems = false;
                
                EpgEntry d = mapItemToEpgEntry[item] as EpgEntry;
                if (d == null) return;

                EpgStatus state;

                if (item.Tag == null) state = EpgStatus.Running;
                else state = (EpgStatus)item.Tag;

                if (state != d.State ||
                    state == EpgStatus.Running 
                   )
                {
                    
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(d.RealDuration);
                    item.SubItems[5].Text = TimeSpanFormatter.ToString(d.ElapsedDuration);
                    if (d.LastError != null)
                    {
                        item.SubItems[6].Text = d.State.ToString().ToUpper() + ", " + d.LastError.Message;
                        item.SubItems[6].Font = new System.Drawing.Font(
                        "KF-Kiran", 7, System.Drawing.FontStyle.Bold);
                    }
                    else
                    {
                        if (String.IsNullOrEmpty(d.StatusMessage))
                        {
                            item.SubItems[6].Text = d.State.ToString().ToUpper();
                            item.SubItems[6].Font = new System.Drawing.Font(
                        "KF-Kiran", 7, System.Drawing.FontStyle.Bold);
                        }
                        else
                        {
                            item.SubItems[6].Text = d.State.ToString().ToUpper() + ", " + d.StatusMessage;
                        }
                    }

                    switch (d.State)
                    {
                        case EpgStatus.Aborted:
                            item.SubItems[6].ForeColor = Color.Violet;//Color.Silver;
                            
                            break;

                        case EpgStatus.Failed:
                            item.SubItems[6].ForeColor = Color.Violet;
                           
                            break;

                        case EpgStatus.Running:
                            item.SubItems[6].ForeColor = Color.Red;
                            break;

                        case EpgStatus.Stopped:
                            item.SubItems[6].ForeColor = Color.Green;
                           
                            break;

                        case EpgStatus.Unstarted:
                            item.SubItems[6].ForeColor = Color.Brown;
                            break;

                        case EpgStatus.Waiting:
                            item.SubItems[6].ForeColor = Color.Brown;
                            break;

                        case EpgStatus.Suspended:
                            break;

                        case EpgStatus.Unknowned:
                            break;

                        case EpgStatus.Prepared:
                            item.SubItems[6].ForeColor = Color.Brown;
                            break;

                        case EpgStatus.WaitingForStop:
                            item.SubItems[6].ForeColor = Color.Red;
                            break;

                        default:
                            break;
                    }

                  
                    switch (d.TypeOfTrigger)
                    {
                        case TriggerType.Default:
                                                  item.SubItems[10].Text = d.TypeOfTrigger.ToString().ToUpper();
                                                   item.SubItems[10].Font = new System.Drawing.Font(
                                                   "KF-Kiran", 7, System.Drawing.FontStyle.Bold);
                                                   item.SubItems[10].ForeColor = Color.Silver;
                            
                            break;

                        case TriggerType.Automatic:

                                                    
                                                    item.SubItems[10].Text = string.Format("{0}",
                                                                                            d.TypeOfTrigger.ToString().ToUpper()
                                                                                            );
                                                    item.SubItems[10].Font = new System.Drawing.Font(
                                                    "KF-Kiran", 7, System.Drawing.FontStyle.Bold);

                                                    item.SubItems[10].ForeColor = Color.Silver;
                            break;

                        case TriggerType.Manual:
                                              
                            break;

                        default:
                            break;
                    }

          

                    item.Tag = d.State;
                }

               
                item.SubItems[11].Text = d.PosterTransferStatus.ToString().ToString();
                item.SubItems[12].Text = d.DummyCommandStatus.ToString();

                switch (d.EventMediaType)
                {
                    case MediaType.UNKNOWN_CHANNEL:
                        break;
                    case MediaType.TV_CHANNEL:
                        //item.SubItems[13].Text = d.NextEpgCloserVideoItem.ToString();
                        item.SubItems[13].Text = "-";
                        item.SubItems[14].Text = "-";
                        //item.SubItems[16].Text = d.ARExtendedProcessingStatus.ToString();
                        break;

                    case MediaType.DC_CHANNEL:
                        //item.SubItems[13].Text = "-";
                        item.SubItems[13].Text = d.MEPG7ProcessingStatus.ToString();
                        item.SubItems[14].Text = d.DCCommandProcessingStatus.ToString();
                        //item.SubItems[16].Text = "-";
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

            SortEpgCollectionByStartTC();

           
           
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
        ///  Load Settings View
        /// </summary>
        public void LoadSettingsView()
        {
            ListViewAutorec.GridLines = Settings.Default.ViewGrid;
            splitContainer1.Panel2Collapsed = !Settings.Default.ViewNotifications;
        }

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
                if (lvi1.ListView.Sorting == SortOrder.Ascending)
                {
                    if (DateTime.TryParse(str1, out dateX) &&
                        DateTime.TryParse(str2, out dateY))
                    {
                        compareResult = DateTime.Compare(dateX, dateY);
                    }
                    else
                    {
                        compareResult = String.Compare(str1, str2);
                    }
                }
                else
                {
                    if (DateTime.TryParse(str1, out dateX) &&
                       DateTime.TryParse(str2, out dateY))
                    {
                        compareResult = DateTime.Compare(dateY, dateX);
                    }
                    else
                    {
                        compareResult = String.Compare(str2, str1);
                    }
                }

                return (compareResult);
            }
        }


    }
}
