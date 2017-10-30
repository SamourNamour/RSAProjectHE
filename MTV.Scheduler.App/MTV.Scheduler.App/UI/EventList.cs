
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: EventList.cs
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
using CMEScheduler.Core;
using CMEScheduler.Core.TriggerInterface;
using CMEScheduler.Core.Common;
using CMEScheduler.Extension.WatcherServiceExtension.Interfaces;
using CMEScheduler.Extension.DBNotificationExtension;
using CMEScheduler;
using Logger.HELogger;
using CMEScheduler.ODATA;
using CMEScheduler.ODataService;
using System.Data.Services.Client;
using CMEScheduler.UtilityAPI;
using CMEScheduler.UtilityAPI.Utils;
using CMEScheduler.Extension.DBNotificationExtension.EventArgs;
#endregion 

namespace CMESchedulerApp.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class EventList : UserControl, IMainView {
        #region - Field(s) -
        Hashtable mapItemToEpgEntry = new Hashtable();
        Hashtable mapEpgEntryToItem = new Hashtable();
        ListViewItem lastSelection = null;
        public event EventHandler<EventArgs> ViewLog;
        #endregion

        #region - Event(s) -
        delegate void ActionEpgEntry(EpgEntry e, ListViewItem item);
        #endregion

        #region - Class Member(s) -

        #endregion

        #region - Property(ies) -

        #endregion

        #region - Constructor(s) / Finalizer(s) -
        /// <summary>
        /// 
        /// </summary>
        public EventList() {
            InitializeComponent();
            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGAdded);
            EPGManager.Instance.StartEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGStarted);
            EPGManager.Instance.RemoveEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGRemoved);
            EPGManager.Instance.EndEpgEvent += new EventHandler<EPGInfoEventArgs>(Instance_EPGEnded);
            ProcessTagInfoManager.Instance.ProcessAdded += new EventHandler<ProcessEventArgs>(Instance_ProcessAdded);
            ProcessTagInfoManager.Instance.ProcessEnded += new EventHandler<ProcessEventArgs>(Instance_ProcessEnded);
            ProcessTagInfoManager.Instance.ProcessFailed += new EventHandler<ProcessEventArgs>(Instance_ProcessFailed);
            //DBNotificationReceiver.Instance.CallBackEvent += new EventHandler<CMEScheduler.Extension.DBNotificationExtension.EventArgs.CallBackEventArgs>(Instance_CallBackEvent);

            using (EPGManager.Instance.LockDownloadList(false)) {
                if (EPGManager.Instance.EpgCollection != null &&
                    EPGManager.Instance.EpgCollection.Count > 0) {
                    foreach (EpgEntry item in EPGManager.Instance.EpgCollection) {
                        AddEPG(item);
                    }
                }
            }
        }
        #endregion

        #region - Callback(s) -

        void Instance_CallBackEvent(object sender,
                                    CallBackEventArgs e) {
            DateTime LastScheduleChangeTime = Settings.Default.LastScheduleChangeDateTime;
            DefaultLogger.logger.Debug(string.Format("Last stored Schedule Change Time (from Settings.settings file) = {0}", LastScheduleChangeTime));
            if (e.objNotification != null) {
                if (DateTime.Compare(e.objNotification.LastLockedScheduleNotificationTime, LastScheduleChangeTime) > 0) {
                    LogNotification(String.Format("Last Locked Schedule Notification Time (from Callback Pusher) = {0}, Action To Do = {1} ",
                                                   e.objNotification.LastLockedScheduleNotificationTime,
                                                   e.objNotification.LastScheduleAction),
                                    LogMode.Information);

                    // Call Web Service to get published Autorec Event(s) :
                    //1- Create the DataServiceContext using the service URI.
                    mebsEntities _context = new mebsEntities(Config.MEBSCatalogLocation);

                    //2- Define the query URI to access the service operation with specific 
                    // query options relative to the service URI.
                    string queryString = string.Format(Config.GetSchedulesByStartTime,
                                                       Convert.ToInt32(ScheduleStatus.PREPARED),
                                                       Convert.ToInt32(MediaType.TV_CHANNEL));
                    // V1 : Asynchronous
                    var results = _context.BeginExecute<mebs_schedule>(
                        new Uri(queryString, UriKind.Relative),
                        OnAsyncExecutionComplete,
                        Tuple.Create(_context, e));
                }
            }
        }

        static void OnAsyncExecutionComplete(IAsyncResult result) {
            // Get the context back from the stored state.
            Tuple<mebsEntities, CallBackEventArgs> state = result.AsyncState as Tuple<mebsEntities, CallBackEventArgs>;
            mebsEntities _context = state.Item1;

            try {
                // Execute the service operation that returns all newest published autorec event(s) from now.
                IEnumerable<mebs_schedule> publishedAutorecCollection = _context.EndExecute<mebs_schedule>(result);

                // Write out newest published autorec event(s) information.
                foreach (mebs_schedule objSchedule in publishedAutorecCollection) {
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.IdSchedule: {0}", objSchedule.IdSchedule));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.IdIngesta: {0}", objSchedule.IdIngesta));
                    //DefaultLogger.logger.Debug(string.Format("mebs_schedule.Code_Package: {0}", objSchedule.Code_Package));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.ContentID: {0}", objSchedule.ContentID));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Date_Schedule: {0}", objSchedule.Date_Schedule));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Estimated_Start: {0}", objSchedule.Estimated_Start));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Estimated_Stop: {0}", objSchedule.Estimated_Stop));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Exact_Start: {0}", objSchedule.Exact_Start));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Exact_Stop: {0}", objSchedule.Exact_Stop));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Locked: {0}", objSchedule.Locked));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.IsActive: {0}", objSchedule.IsActive));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Status: {0}", objSchedule.Status));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Poster_Status: {0}", objSchedule.Poster_Status));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Poster_DateSent: {0}", objSchedule.Poster_DateSent));
                    DefaultLogger.logger.Debug(string.Format("mebs_schedule.Poster_SentTries: {0}", objSchedule.Poster_SentTries));

                    if (EPGManager.Instance.Exists(objSchedule.IdSchedule.ToString())) {

                        string msg = string.Format(@"Automatic-recording already been added to Published Event Collection. 
                                                     [Title :: {0}] - [ID :: {1}] - [{2}-{3}].",
                                                     objSchedule.mebs_ingesta.Title,
                                                     objSchedule.IdSchedule,
                                                     objSchedule.Estimated_Start,
                                                     objSchedule.Estimated_Stop);
                        continue;
                    }

                    EpgEntry objEpgEntry = new EpgEntry();
                    objEpgEntry.Bus = objSchedule.mebs_ingesta.mebs_channel.Bus;
                    objEpgEntry.ChannelDesignation = objSchedule.mebs_ingesta.mebs_channel.LongName;
                    objEpgEntry.ContentID = (int)objSchedule.ContentID;
                    objEpgEntry.CreatedDateTime = DateTime.UtcNow;
                    objEpgEntry.EstimatedStartDateTime = objSchedule.Estimated_Start.Value;
                    objEpgEntry.EstimatedStopDateTime = objSchedule.Estimated_Stop.Value;
                    objEpgEntry.ID = objSchedule.IdSchedule.ToString();
                    objEpgEntry.OriginalTitle = objSchedule.mebs_ingesta.Title;
                    objEpgEntry.State = EpgStatus.Unstarted;
                    if (objSchedule.mebs_ingesta.mebs_videoitem != null &&
                        objSchedule.mebs_ingesta.mebs_videoitem.Count > 0) {
                        objEpgEntry.LinkedSegmentCollection = new List<Segment>(objSchedule.mebs_ingesta.mebs_videoitem.Count);
                        foreach (mebs_videoitem item in objSchedule.mebs_ingesta.mebs_videoitem) {
                            objEpgEntry.LinkedSegmentCollection.Add(new Segment {
                                BUS = objSchedule.mebs_ingesta.mebs_channel.Bus,
                                VIDEO_ITEM = item.ItemValue
                            });
                        }
                    }
                    EPGManager.Instance.Add(objEpgEntry, false);
                }

                Settings.Default.LastScheduleChangeDateTime = state.Item2.objNotification.LastLockedScheduleNotificationTime;
                Settings.Default.Save();
            }
            catch (DataServiceQueryException dataServiceException) {
                DefaultLogger.logger.Error(dataServiceException);
            }
            catch (Exception ex) {
                DefaultLogger.logger.Error(ex);
            }
        }
        #endregion

        #region - Delegate Invoker(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGAdded(object sender, EPGInfoEventArgs e) {
            string msg = string.Format("New automatic-recording [Title :: {0}] - [ID :: {1}] - [{2}-{3}] just been added.",
                                        e.EpgItem.OriginalTitle,
                                        e.EpgItem.ID,
                                        e.EpgItem.EstimatedStartDateTime,
                                        e.EpgItem.EstimatedStopDateTime
                                        );
            LogHelper.logger.Debug(msg);

            Log(msg, LogMode.Information);

            if (IsHandleCreated) {
                this.BeginInvoke((MethodInvoker)delegate() { AddEPG(e.EpgItem); });
            }
            else {
                AddEPG(e.EpgItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGStarted(object sender, EPGInfoEventArgs e) {
            string msg = string.Format("Automatic-recording [Title :: {0}] - [ID :: {1}] - [{2}-{3}] is recording now.",
                             e.EpgItem.OriginalTitle,
                             e.EpgItem.ID,
                             e.EpgItem.EstimatedStartDateTime,
                             e.EpgItem.EstimatedStopDateTime
                             );
            LogHelper.logger.Debug(msg);

            Log(msg, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_EPGRemoved(object sender, EPGInfoEventArgs e) {
            this.BeginInvoke(
                (MethodInvoker)delegate() {
                ListViewItem item = mapEpgEntryToItem[e.EpgItem] as ListViewItem;
                if (item != null) {
                    if (item.Selected) {
                        lastSelection = null;
                        lvwSegments.Items.Clear();
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
            Log(

                String.Format(
                "Event  ({0}) Stoped",
                e.EpgItem.ID.ToString()
                ),
                LogMode.Information);

            Log(

                String.Format(
                "Sending Stop command for Event  ({0})",
                e.EpgItem.ID.ToString()
                ),
                LogMode.Information);



            //byte iMaxTry = 5;
            //EPGStopResponse replyStopCommand = null;
            //string msg = string.Empty;
            //try {
            //    do {
            //        try {
            //            replyStopCommand = EpgBreakerSender.StopEPG(e.EpgItem);
            //            break;
            //        }
            //        catch (Exception ex) {
            //            msg = string.Format("Tentative {0} To Stop Event : {1}",
            //                                 iMaxTry,
            //                                 e.EpgItem.ID);

            //            iMaxTry--;
            //            LogHelper.logger.Error(msg, ex);
            //            Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
            //            continue;
            //        }

            //    } while (iMaxTry > 0);
            //}
            //finally {
            //    if (replyStopCommand.CodeResponse == 2000) {
            //        msg = string.Format("Event : {0} - has been successfully stoped ({1}) - [{2}]",
            //                             e.EpgItem.OriginalTitle,
            //                             e.EpgItem.ID,
            //                             replyStopCommand.MetaData);

            //        LogHelper.logger.Debug(msg);
            //        e.EpgItem.ExactStopDateTime = DateTime.UtcNow;

            //        Log(msg, LogMode.Information);
            //    }
            //    else {
            //        msg = string.Format("An error has been occured during trying to stop Event : {0} ({1}) - [{2}]",
            //                              e.EpgItem.OriginalTitle,
            //                              e.EpgItem.ID,
            //                              replyStopCommand.MetaData);
            //        LogHelper.logger.Error(msg);

            //        Log(msg, LogMode.Error);
            //    }
            //}
        }
        #endregion

        #region - Private Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        void AddEPG(EpgEntry d) {
            d.SegmentStoped += new EventHandler<SegmentEventArgs>(epg_SegmentEnded);
            d.SegmentFailed += new EventHandler<SegmentEventArgs>(epg_SegmentFailed);
            d.SegmentStarted += new EventHandler<SegmentEventArgs>(epg_SegmentStarted);

            ListViewItem item = new ListViewItem();
            item.UseItemStyleForSubItems = false;
            item.Text = d.OriginalTitle;

            // EstimatedStart:
            item.SubItems.Add(d.EstimatedStartDateTime.ToString());

            // EstimatedStop:
            item.SubItems.Add(d.EstimatedStopDateTime.ToString());

            // Channel:
            item.SubItems.Add(d.ChannelDesignation.ToString());

            // RealDuration:
            item.SubItems.Add(TimeSpanFormatter.ToString(d.RealDuration));

            // ElapsedDuration:
            item.SubItems.Add(TimeSpanFormatter.ToString(d.ElapsedDuration));

            // State:
            item.SubItems.Add(d.State.ToString());

            // CreatedDateTime:
            item.SubItems.Add(d.CreatedDateTime.ToString());

            // ContentID:
            item.SubItems.Add(d.ContentID.ToString());

            mapEpgEntryToItem[d] = item;
            mapItemToEpgEntry[item] = d;
            lvwPrograms.Items.Add(item);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        void Log(string msg, LogMode m) {
            try {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate() {
                      int len = richLog.Text.Length;
                      if (len > 0) {
                          richLog.SelectionStart = len;
                      }

                      if (m == LogMode.Error) {
                          richLog.SelectionColor = Color.Red;
                      }
                      else {
                          richLog.SelectionColor = Color.Blue;
                      }

                      if (richLog.Lines.Length >= Settings.Default.RichLogMaxLenght) richLog.Clear();

                      richLog.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch (Exception ex) {
                DefaultLogger.logger.Error(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        void LogFileNotfication(string msg, LogMode m) {
            try {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate() {
                      int len = richTxtFilesNotification.Text.Length;
                      if (len > 0) {
                          richTxtFilesNotification.SelectionStart = len;
                      }

                      if (m == LogMode.Error) {
                          richTxtFilesNotification.SelectionColor = Color.Red;
                      }
                      else {
                          richTxtFilesNotification.SelectionColor = Color.Blue;
                      }

                      if (richTxtFilesNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTxtFilesNotification.Clear();

                      richTxtFilesNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch (Exception ex) {
                DefaultLogger.logger.Error(ex);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="m"></param>
        void LogNotification(string msg, LogMode m) {
            try {
                this.BeginInvoke(
                    (MethodInvoker)
                  delegate() {
                      int len = richTxtNotification.Text.Length;
                      if (len > 0) {
                          richTxtNotification.SelectionStart = len;
                      }

                      if (m == LogMode.Error) {
                          richTxtNotification.SelectionColor = Color.Red;
                      }
                      else {
                          richTxtNotification.SelectionColor = Color.Green;
                      }

                      if (richTxtNotification.Lines.Length >= Settings.Default.RichLogMaxLenght) richTxtNotification.Clear();

                      richTxtNotification.AppendText(DateTime.Now + " - " + msg + Environment.NewLine);
                  }
              );
            }
            catch (Exception ex) {
                DefaultLogger.logger.Error(ex);
            }
        }
        #endregion

        #region - Internal Method(s) -

        #endregion

        #region - Public Method(s) -

        #endregion

        #region - IDisposable Member(s) -

        #endregion

        #region - Watcher services -


        #endregion

        #region  Old Code To Manage
        private void EpgEntryAction(ActionEpgEntry action) {
            if (lvwPrograms.SelectedItems.Count > 0) {
                try {
                    lvwPrograms.BeginUpdate();

                    lvwPrograms.ItemSelectionChanged -= new ListViewItemSelectionChangedEventHandler(lvwPrograms_ItemSelectionChanged);

                    for (int i = lvwPrograms.SelectedItems.Count - 1; i >= 0; i--) {
                        ListViewItem item = lvwPrograms.SelectedItems[i];
                        action((EpgEntry)mapItemToEpgEntry[item], item);
                    }

                    lvwPrograms.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvwPrograms_ItemSelectionChanged);
                    lvwPrograms_ItemSelectionChanged(null, null);
                }
                finally {
                    lvwPrograms.EndUpdate();
                    UpdateSegments();
                }
            }
        }

        private void lvwPrograms_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e) {
            UpdateSegments();

            UpdateUI();
        }

        public void UpdateUI() {
            bool isSelected = lvwPrograms.SelectedItems.Count > 0;


            isSelected = lvwPrograms.SelectedItems.Count == 1;




            OnSelectionChange();
        }

        public void UpdateList() {

            for (int i = 0; i < lvwPrograms.Items.Count; i++) {
                ListViewItem item = lvwPrograms.Items[i];
                if (item == null) return;



                EpgEntry d = mapItemToEpgEntry[item] as EpgEntry;
                if (d == null) return;

                EpgStatus state;

                if (item.Tag == null) state = EpgStatus.Running;
                else state = (EpgStatus)item.Tag;

                if (state != d.State ||
                    state == EpgStatus.Running
                    ) {
                    item.SubItems[0].Text = d.OriginalTitle.ToString();
                    item.SubItems[1].Text = d.EstimatedStartDateTime.ToString();
                    item.SubItems[2].Text = d.EstimatedStopDateTime.ToString();
                    item.SubItems[3].Text = d.ChannelDesignation.ToString();
                    item.SubItems[4].Text = TimeSpanFormatter.ToString(d.RealDuration);
                    item.SubItems[5].Text = TimeSpanFormatter.ToString(d.ElapsedDuration);
                    if (d.LastError != null) {
                        item.SubItems[6].Text = d.State.ToString() + ", " + d.LastError.Message;
                    }
                    else {
                        if (String.IsNullOrEmpty(d.StatusMessage)) {
                            item.SubItems[6].Text = d.State.ToString();
                        }
                        else {
                            item.SubItems[6].Text = d.State.ToString() + ", " + d.StatusMessage;
                        }
                    }

                    switch (d.State) {
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

                        default:
                            break;
                    }

                    item.SubItems[7].Text = d.CreatedDateTime.ToString();
                    item.SubItems[8].Text = d.ContentID.ToString();

                    item.Tag = d.State;
                }
            }

            UpdateSegments();
        }

        private void splitContainer2_Panel1_Paint(object sender, PaintEventArgs e) {

        }

        private void lvwSegments_SelectedIndexChanged(object sender, EventArgs e) {

        }

        public void LoadSettingsView() {
            lvwPrograms.GridLines = Settings.Default.ViewGrid;
            lvwSegments.GridLines = Settings.Default.ViewGrid;
            splitContainer2.Panel2Collapsed = !Settings.Default.ViewTransDetails;
        }

        public event EventHandler SelectionChange;

        protected virtual void OnSelectionChange() {
            if (SelectionChange != null) {
                SelectionChange(this, EventArgs.Empty);
            }
        }

        public void RemoveCompleted() {
            lvwPrograms.BeginUpdate();
            try {
                EPGManager.Instance.ClearEnded();
                UpdateList();
            }
            finally {
                lvwPrograms.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e) {
            richLog.Clear();
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessAdded(object sender, ProcessEventArgs e) {
            string msg = string.Format("Process : {0} - has been successfully started.",
                                          e.ProcessTagInfo.FileName
                                          );

            LogHelper.logger.Debug(msg);

            LogFileNotfication(msg, LogMode.Information);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessEnded(object sender, ProcessEventArgs e) {
            string msg = string.Format("Process : {0} - has been successfully Ended.",
                                          e.ProcessTagInfo.FileName
                                          );

            LogHelper.logger.Debug(msg);

            LogFileNotfication(msg, LogMode.Information);


        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Instance_ProcessFailed(object sender, ProcessEventArgs e) {
            string msg = string.Format("Process : {0} - has Failed : {1}",
                                          e.ProcessTagInfo.FileName, e.ProcessTagInfo.LastError.ToString()
                                          );

            LogHelper.logger.Debug(msg);

            LogFileNotfication(msg, LogMode.Error);


        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentEnded(object sender, SegmentEventArgs e) {
            Log(

                String.Format(
                "Event segment ({0}) ended",
                e.Segment.VIDEO_ITEM
                ),
                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentFailed(object sender, SegmentEventArgs e) {
            Log(

                String.Format(
                "Event segment ({0}) Failed",
                e.Segment.VIDEO_ITEM
                ),
                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void epg_SegmentStarted(object sender, SegmentEventArgs e) {
            Log(

                String.Format(
                "Event segment ({0}) started",
                e.Segment.VIDEO_ITEM
                ),
                LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateSegments() {
            try {
                lvwSegments.BeginUpdate();

                if (lvwPrograms.SelectedItems.Count == 1) {
                    ListViewItem newSelection = lvwPrograms.SelectedItems[0];
                    EpgEntry e = mapItemToEpgEntry[newSelection] as EpgEntry;

                    if (lastSelection == newSelection) {
                        if (e.LinkedSegmentCollection != null &&
                            e.LinkedSegmentCollection.Count > 0 &&
                            e.LinkedSegmentCollection.Count == lvwSegments.Items.Count) {
                            UpdateSegmentsWithoutInsert(e);
                        }
                        else {
                            UpdateSegmentsInserting(e, newSelection);
                        }
                    }
                    else {
                        UpdateSegmentsInserting(e, newSelection);
                    }
                }
                else {
                    lastSelection = null;

                    lvwSegments.Items.Clear();
                }
            }
            finally {
                lvwSegments.EndUpdate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        private void UpdateSegmentsWithoutInsert(EpgEntry d) {
            for (int i = 0; i < d.LinkedSegmentCollection.Count; i++) {
                lvwSegments.Items[i].SubItems[0].Text = d.LinkedSegmentCollection[i].VIDEO_ITEM.ToString();
                lvwSegments.Items[i].SubItems[1].Text = d.LinkedSegmentCollection[i].TITLE.ToString();
                lvwSegments.Items[i].SubItems[2].Text = d.LinkedSegmentCollection[i].TIME.ToString();
                lvwSegments.Items[i].SubItems[3].Text = TimeSpanFormatter.ToString(d.LinkedSegmentCollection[i].DURATION);


                if (d.LinkedSegmentCollection[i].LastError != null) {
                    lvwSegments.Items[i].SubItems[4].Text = d.LinkedSegmentCollection[i].Status.ToString() + ", " + d.LinkedSegmentCollection[i].LastError.Message;
                }
                else {
                    lvwSegments.Items[i].SubItems[4].Text = d.LinkedSegmentCollection[i].Status.ToString();
                }

                lvwSegments.Items[i].SubItems[5].Text = TimeSpanFormatter.ToString(d.LinkedSegmentCollection[i].TotalDuration);
                lvwSegments.Items[i].SubItems[6].Text = d.LinkedSegmentCollection[i].ReRuns.ToString();



            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="newSelection"></param>
        private void UpdateSegmentsInserting(EpgEntry d, ListViewItem newSelection) {
            lastSelection = newSelection;

            lvwSegments.Items.Clear();


            if (d.LinkedSegmentCollection != null &&
                d.LinkedSegmentCollection.Count > 0) {
                for (int i = 0; i < d.LinkedSegmentCollection.Count; i++) {
                    ListViewItem item = new ListViewItem();

                    item.Text = d.LinkedSegmentCollection[i].VIDEO_ITEM.ToString();
                    item.SubItems.Add(d.LinkedSegmentCollection[i].TITLE.ToString());
                    item.SubItems.Add(d.LinkedSegmentCollection[i].TIME.ToString());
                    item.SubItems.Add(TimeSpanFormatter.ToString(d.LinkedSegmentCollection[i].DURATION));
                    item.SubItems.Add(d.LinkedSegmentCollection[i].Status.ToString());
                    item.SubItems.Add(TimeSpanFormatter.ToString(d.LinkedSegmentCollection[i].TotalDuration));
                    item.SubItems.Add(d.LinkedSegmentCollection[i].ReRuns.ToString());


                    lvwSegments.Items.Add(item);


                }
            }
        }



        /// <summary>
        /// 
        /// </summary>
        public int SelectedCount {
            get {
                return lvwPrograms.SelectedItems.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public EpgEntry[] SelectedEpgEntrys {
            get {
                if (lvwPrograms.SelectedItems.Count > 0) {
                    EpgEntry[] epgEntrys = new EpgEntry[lvwPrograms.SelectedItems.Count];
                    for (int i = 0; i < epgEntrys.Length; i++) {
                        epgEntrys[i] = mapItemToEpgEntry[lvwPrograms.SelectedItems[i]] as EpgEntry;
                    }
                    return epgEntrys;
                }

                return null;
            }
        }



        /// <summary>
        /// Updates view.
        /// </summary>
        /// <param name="logMessages">Log messages.</param>
        public void Update(string logMessages) {
            if (logMessages == null) {
                throw new ArgumentNullException("logMessages",
                                                "");
            }
            //textBoxLogMessages.Text = logMessages;

            LogFileNotfication(logMessages, LogMode.Information);
        }

        /// <summary>
        /// 
        /// </summary>
        enum LogMode {
            Error,
            Information
        }




        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventList_Load(object sender, EventArgs e) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void richLog_TextChanged(object sender, EventArgs e) {
            if (richLog.Lines.Length >= Settings.Default.RichLogMaxLenght) 
                richLog.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lvwPrograms_SelectedIndexChanged(object sender, EventArgs e) {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void removeToolStripMenuItem_Click(object sender, EventArgs e) {
            RemoveCompleted();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void popUpContextMenu_Opening(object sender, CancelEventArgs e) {
            UpdateUI();
        }
        #endregion
    }
}
