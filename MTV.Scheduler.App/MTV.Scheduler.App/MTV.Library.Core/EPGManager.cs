
#region Copyright Motive Television 2012
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: EPGManager.cs
//
#endregion

#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Linq;

// Custom Directive(s)
using MTV.Library.Core.Concurrency;
using MTV.Library.Core.TriggerInterface;
//using MTV.Library.Core.MESCatalog;
using MTV.Library.Core.Services;
#endregion 

namespace MTV.Library.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EPGManager {

        #region Singleton

        private static readonly EPGManager instance = new EPGManager();

        /// <summary>
        /// 
        /// </summary>
        public static EPGManager Instance {
            get {
                return instance;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private EPGManager() {

        }

        #endregion

        #region Fields(s)
        private List<EpgEntry> mEpgCollection = new List<EpgEntry>();
        private ReaderWriterObjectLocker EpgCollectionSynchronizer = new ReaderWriterObjectLocker();
        #endregion

        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<EpgEntry> EpgCollection {
            get {
                return mEpgCollection.AsReadOnly();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalEpgs {
            get {
                double total = 0;

                using (LockDownloadList(false)) {
                    for (int i = 0; i < this.EpgCollection.Count; i++) {
                        total++;
                    }
                }

                return total;
            }
        }
        #endregion

        #region Event(s)

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EPGInfoEventArgs> AddEpgEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EPGInfoEventArgs> StartEpgEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EPGInfoEventArgs> EndEpgEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<EPGInfoEventArgs> RemoveEpgEvent;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void EpgEvent_StateChanged(object sender, EventArgs e) {
            EpgEntry epgEntry = (EpgEntry)sender;

            //if (epgEntry.State == EpgStatus.Stopped) {
                OnEndEpgEvent((EpgEntry)sender);
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        protected virtual void OnEndEpgEvent(EpgEntry e) {
            if (EndEpgEvent != null) {
                EndEpgEvent(this, new EPGInfoEventArgs(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnAddEpgEvent(EpgEntry e) {
            if (AddEpgEvent != null) {
                AddEpgEvent(this, new EPGInfoEventArgs(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnStartEpgEvent(EpgEntry e) {
            if (StartEpgEvent != null) {
                StartEpgEvent(this, new EPGInfoEventArgs(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRemoveEpgEvent(EpgEntry e) {
            if (RemoveEpgEvent != null) {
                RemoveEpgEvent(this, new EPGInfoEventArgs(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(EpgEntry item, bool autoStart) {
            item.StateChanged += new EventHandler(EpgEvent_StateChanged);

            using (LockDownloadList(true)) {
                mEpgCollection.Add(item);
            }

            OnAddEpgEvent(item);

            if (autoStart) {
                item.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveEpg(int index) {
            RemoveEpg(mEpgCollection[index]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void RemoveEpg(EpgEntry item) {
            using (LockDownloadList(true)) {
                mEpgCollection.Remove(item);
            }

            OnRemoveEpgEvent(item);
        }
        #endregion

        #region Main Method(s)

        #endregion

        #region Auxiliary Method(s)

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockForWrite"></param>
        /// <returns></returns>
        public IDisposable LockDownloadList(bool lockForWrite) {
            if (lockForWrite) {
                return EpgCollectionSynchronizer.LockForWrite();
            }
            else {
                return EpgCollectionSynchronizer.LockForRead();
            }
        }

        #endregion

        #region Helpful Method(s)

        /// <summary>
        /// /
        /// </summary>
        /// <param name="itemTrigger"></param>
        /// <param name="itemEpg"></param>
        /// <returns></returns>
        public EpgEntry GetEpgEventByTriggerEntry(TriggerEntry itemTrigger) {
            EpgEntry item = null;

            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    EpgEntry tempEpgEntry = mEpgCollection.Find
                        (
                            delegate(EpgEntry param) {
                                return (param.State == EpgStatus.Running || param.State == EpgStatus.Suspended || param.State == EpgStatus.Prepared) &&
                                       string.Compare(param.Bus, itemTrigger.BUS) == 0 &&
                                       param.EventMediaType == MediaType.TV_CHANNEL;
                            }
                        );

                    item = tempEpgEntry;
                }
            }

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemTrigger"></param>
        /// <returns></returns>
        public EpgEntry GetRunningEpgEventByTriggerEntry(TriggerEntry itemTrigger) {
            EpgEntry item = null;

            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    EpgEntry tempEpgEntry = mEpgCollection.Find
                        (
                            delegate(EpgEntry param) {
                                return (param.State == EpgStatus.Running ||
                                        param.State == EpgStatus.Suspended ||
                                        param.State == EpgStatus.Prepared) &&
                                        string.Compare(param.Bus, itemTrigger.BUS, true) == 0 &&
                                        param.EventMediaType == MediaType.TV_CHANNEL;
                            }
                        );

                    item = tempEpgEntry;
                }
            }

            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="itemTrigger"></param>
        /// <returns></returns>
        public EpgEntry GetUnstartedEpgEventByTriggerEntry(TriggerEntry itemTrigger) {
            EpgEntry item = null;
            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    EpgEntry tempEpgEntry = mEpgCollection.Find
                        (
                            delegate(EpgEntry param) {
                                return param.State == EpgStatus.Unstarted &&
                                       param.EventMediaType == MediaType.TV_CHANNEL &&
                                       string.Compare(param.Bus, itemTrigger.BUS, true) == 0 &&
                                       param.LinkedSegmentCollection.Exists(element => element.VIDEO_ITEM.CompareTo(itemTrigger.VIDEO_ITEM) == 0) &&
                                       itemTrigger.TIME.CompareTo(param.EstimatedStartDateTime.Add(-TriggerEntry.TriggerGracePeriod)) >= 0 &&
                                       itemTrigger.TIME.CompareTo(param.EstimatedStopDateTime) <= 0;
                            }
                        );
                    item = tempEpgEntry;
                }
            }
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearEnded() {
            using (LockDownloadList(true)) {
                for (int i = mEpgCollection.Count - 1; i >= 0; i--) {

                    if (mEpgCollection[i].State == EpgStatus.Stopped) {
                        EpgEntry item = mEpgCollection[i];
                        item.Ended();
                        mEpgCollection.RemoveAt(i);
                        OnRemoveEpgEvent(item);
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void PauseAll() {
            using (LockDownloadList(false)) {
                for (int i = 0; i < this.mEpgCollection.Count; i++) {
                    EpgEntry occurence = this.mEpgCollection[i];

                    switch (occurence.State) {

                        case EpgStatus.Unstarted:
                            break;

                        case EpgStatus.Waiting:
                            break;

                        case EpgStatus.Running:
                        case EpgStatus.WaitingForStop:
                            MEBSCatalogProvider.EditScheduleExactStopTime(Int32.Parse(occurence.ID),
                                                                          DateTime.UtcNow,
                                                                          ScheduleStatus.FAILED_STOP);
                            break;

                        case EpgStatus.Suspended:
                            break;

                        case EpgStatus.Stopped:
                            break;

                        case EpgStatus.Aborted:
                            break;

                        case EpgStatus.Unknowned:
                            break;

                        case EpgStatus.Failed:
                            break;

                        case EpgStatus.Prepared:
                            break;

                        default:
                            break;
                    }
                    this.mEpgCollection[i].Ended();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool Exists(string ID) {
            bool res = false;
            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    EpgEntry tempEpgEntry = mEpgCollection.Find
                        (
                            delegate(EpgEntry param) {
                                return string.Compare(param.ID, ID) == 0;
                            }
                        );

                    res = (tempEpgEntry == null ? false : true);
                }
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        public void SetDummyCommandStatus(string ID, EpgDummyCommandStatus dummyCommandStatus) {
            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    EpgEntry tempEpgEntry = mEpgCollection.Find
                        (
                            delegate(EpgEntry param) {
                                return string.Compare(param.ID, ID) == 0;
                            }
                        );

                    if (tempEpgEntry != null)
                        tempEpgEntry.DummyCommandStatus = dummyCommandStatus;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EpgEntry GetLastDatacastingSentEvent() {
            EpgEntry item = null;
            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    bool exist = mEpgCollection.ToList().Where(p => p.DCCommandProcessingStatus == DCCommandStatus.ACK &&
                                                               p.EventMediaType == MediaType.DC_CHANNEL).OrderBy(p => p.EstimatedStartDateTime).Any();
                    if (exist) {
                        item = mEpgCollection.ToList().Where(p => p.DCCommandProcessingStatus == DCCommandStatus.ACK).OrderBy(p => p.EstimatedStartDateTime).First();
                    }
                }
            }
            return item ?? null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public EpgEntry GetLastARAdvExtendSentEvent() {
            EpgEntry item = null;
            using (LockDownloadList(true)) {
                if (mEpgCollection != null &&
                    mEpgCollection.Count > 0) {
                    bool exist = mEpgCollection.ToList().Where(p => p.ARExtendedProcessingStatus == ARExtendedStatus.ACK &&
                                                               p.EventMediaType == MediaType.TV_CHANNEL).OrderBy(p => p.EstimatedStartDateTime).Any();
                    if (exist) {
                        item = mEpgCollection.ToList().Where(p => p.ARExtendedProcessingStatus == ARExtendedStatus.ACK).OrderBy(p => p.EstimatedStartDateTime).First();
                    }
                }
            }
            return item ?? null;
        }

        #endregion
    }
}
