
#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: EpgEntry.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.- Class : Namespace (s) -.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Threading;

using MTV.Library.Core.Data;
using MTV.Library.Core.Common;
using MTV.Library.Core.Concurrency;
using MTV.Library.Core.CommerialBreaks;
using MTV.Library.Core.PlayoutCommandManager;
using System.Runtime.CompilerServices;
using MTV.Library.Core.Services;
using MTV.Scheduler.App.UI;
#endregion 

namespace MTV.Library.Core
{
    /// <summary>
    /// Main EBSEventListener entity, holds all ongoing Digiturk EPG.
    /// </summary>
    public class EpgEntry
    {        
        #region Field(s)
        ReaderWriterLockSlim locker = new ReaderWriterLockSlim();
        private object syncRoot = new object();
        private string idField;
        private DateTime estimatedStartDateTimeField;
        private DateTime estimatedStopDateTimeField;
        private DateTime exactStartDateTimeField;
        private DateTime exactStopDateTimeField;
        private DateTime createdDateTime;     
        private string channelDesignationField;
        private string originalTitleField;
        private string busField;
        private string requestedVideoItemField;
        private string statusMessage;
        private TriggerEntry lastHarrisTriggerField;        
        private EpgStatus stateField;
        private Exception lastError;                  
        private StringCollection lysisLinkedSegmentIDCollectionField;
        private TimeSpan realDurationField;
        private List<Segment> linkedSegmentCollectionField;
        private List<CommerialBreak> advertisements;
        private List<Thread> threads;
        private int contentIDField;
        private PosterTransferStatus posterTransferStatusField;
        private DateTime posterTransferDateTimeField;
        private sbyte posterTransferTriesField;
        static object locker1 = new object();
        #endregion 

        #region Property(ies)

        /// <summary>
        /// 
        /// </summary>
        public List<Thread> monoThread
        {
            get
            {
                return threads;
            }
            set
            {
                this.threads = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ID
        {
            get 
            { 
                return idField; 
            }
            set
            {
                this.idField = value;
            }
        }

        /// <summary>
        /// EPG estimated start date & time.
        /// </summary>
        public DateTime EstimatedStartDateTime
        {
            get { return estimatedStartDateTimeField; }
            set { estimatedStartDateTimeField = value; }
        }        

        /// <summary>
        /// EPG estimated stop date & time.
        /// </summary>
        public DateTime EstimatedStopDateTime
        {
            get { return estimatedStopDateTimeField; }
            set { estimatedStopDateTimeField = value; }
        }        

        /// <summary>
        /// EPG accurate start date & time.
        /// </summary>
        public DateTime ExactStartDateTime {
            get {
                return exactStartDateTimeField;
            }
            set {
                exactStartDateTimeField = value;
                this.TypeOfTrigger = TriggerType.Automatic;

                //this.TypeOfTrigger = (value.CompareTo(DateTime.UtcNow.AddMinutes(DefaultValues.AUTO_RECORDING_AUTOMATIC_OFFSET)) > 0 ?
                //                        TriggerType.Automatic :
                //                        TriggerType.Manual);
            }
        }        

        /// <summary>
        /// EPG accurate stop date & time.
        /// </summary>
        public DateTime ExactStopDateTime
        {
            get { return exactStopDateTimeField; }
            set { exactStopDateTimeField = value; }
        }        

        /// <summary>
        /// EPG associated Linked Segment ID(s).
        /// </summary>
        public StringCollection LysisLinkedSegmentIDCollection {
            get {
                return lysisLinkedSegmentIDCollectionField;
            }
            set {
                lysisLinkedSegmentIDCollectionField = value;
            }
        }        

        /// <summary>
        /// EPG estimated duration.
        /// </summary>
        public TimeSpan EstimatedDuration {
            get {
                return this.estimatedStopDateTimeField.Subtract(this.estimatedStartDateTimeField);
            }
        }        

        /// <summary>
        /// EPG current duration.
        /// </summary>
        public TimeSpan RealDuration {
            get { return realDurationField; }
            set { realDurationField = value; }
        }

        /// <summary>
        /// EPG elapsed duration.
        /// </summary>
        public TimeSpan ElapsedDuration
        {
            get
            {
                TimeSpan ts = TimeSpan.FromSeconds(0);
                if (this.ExactStartDateTime == DateTime.MinValue &&
                    this.ExactStopDateTime == DateTime.MinValue)
                {
                    return ts;
                }
                if (this.ExactStartDateTime != DateTime.MinValue)
                {
                    if (this.ExactStopDateTime != DateTime.MinValue)
                    {
                        ts = this.ExactStopDateTime.Subtract(exactStartDateTimeField);
                    }
                    else
                    {
                        if (DateTime.UtcNow >= this.ExactStartDateTime)
                        {
                            ts = DateTime.UtcNow.Subtract(exactStartDateTimeField);
                        }
                    }
                }
                return ts;
            }
        }

        /// <summary>
        /// EPG associated Linked Segment ID(s) Harris Metadata.
        /// </summary>
        public List<Segment> LinkedSegmentCollection
        {
            get { return linkedSegmentCollectionField; }
            set { linkedSegmentCollectionField = value; }
        }

        /// <summary>
        /// Last received EPG harris trigger.
        /// </summary>
        public TriggerEntry LastHarrisTrigger
        {
            get 
            { 
                return lastHarrisTriggerField; 
            }
            set 
            { 
                lastHarrisTriggerField = value;
                switch (this.lastHarrisTriggerField.TypeOfMaterial)
                {
                    case MaterialType.C:
                    case MaterialType.J:
                    case MaterialType.L:
                    case MaterialType.M:
                    case MaterialType.O:
                    case MaterialType.P:
                    case MaterialType.S:
                    case MaterialType.T:
                    case MaterialType.U:
                        if (this.LysisLinkedSegmentIDCollection.Contains(this.LastHarrisTrigger.VIDEO_ITEM))
                        {
                            this.RequestedVideoItem = this.LastHarrisTrigger.VIDEO_ITEM;
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// EPG channel long name.
        /// </summary>
        public string ChannelDesignation
        {
            get 
            { 
                return channelDesignationField; 
            }
            set 
            { 
                channelDesignationField = value; 
            }
        }

        /// <summary>
        /// EPG lysis original title.
        /// </summary>
        public string OriginalTitle
        {
            get { return originalTitleField; }
            set { originalTitleField = value; }
        }

        /// <summary>
        /// Automatic-Recording event channel Harris Interface Name.
        /// </summary>
        public string Bus {
            get { return busField; }
            set { busField = value; }
        }

        /// <summary>
        /// Automatic-Recording event status.
        /// </summary>
        public EpgStatus State {
            get { return stateField; }
            set { stateField = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string StatusMessage {
            get { return statusMessage; }
            set { statusMessage = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public Exception LastError {
            get { return lastError; }
            set { lastError = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime {
            get {
                return createdDateTime;
            }
            set {
                createdDateTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string RequestedVideoItem {
            get {
                return this.requestedVideoItemField;
            }
            private set {
                this.requestedVideoItemField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Segment RequestedSegment {
            get {
                Segment temp = this.LinkedSegmentCollection.Find
               (
                   delegate(Segment param) {
                       return string.Compare(param.VIDEO_ITEM, this.RequestedVideoItem) == 0;

                   }
               );

                return temp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Segment> UnstoppedSegment {
            get {
                List<Segment> unstoppedSegmentCollection = null;
                lock (this.syncRoot) {
                    unstoppedSegmentCollection = this.LinkedSegmentCollection.FindAll
                    (
                        delegate(Segment param) {
                            return param.Status != SegmentState.Stopped;
                        }
                    );
                }
                return unstoppedSegmentCollection;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<CommerialBreak> AdvertisementCollection {
            get {
                return advertisements;
            }
            set {
                advertisements = value;
            }
        }
                
        /// <summary>
        /// 
        /// </summary>
        public int ContentID {
            get {
                return this.contentIDField;
            }
            set {
                this.contentIDField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TimeSpan EventGracePeriod {
            get {
                return TimeSpan.FromSeconds(1);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Int32 ErrorCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public sbyte? nMediaType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EpgDummyCommandStatus DummyCommandStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string NextEpgCloserVideoItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Segment UnstartedRequestedSegment {
            get {
                Segment temp = this.LinkedSegmentCollection.Find
               (
                   delegate(Segment param) {
                       return string.Compare(param.VIDEO_ITEM, this.RequestedVideoItem) == 0 &&
                              param.Status == SegmentState.Unstarted;
                   }
               );
                return temp;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public TriggerType TypeOfTrigger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FirstTriggerReachHETime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public byte[] Poster { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PosterFileExtension { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public PosterTransferStatus PosterTransferStatus {
            get {
                return this.posterTransferStatusField;
            }
            set {
                posterTransferStatusField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PosterTransferDateTime {
            get {
                return this.posterTransferDateTimeField;
            }
            set {
                posterTransferDateTimeField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sbyte PosterTransferTries {
            get {
                return this.posterTransferTriesField;
            }
            set {
                this.posterTransferTriesField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsAnyCommercialBreaksSendFailed {
            get {
                Segment temp = this.LinkedSegmentCollection.Find
               (
                   delegate(Segment param) {
                       return string.Compare(param.PlayoutReply, DefaultValues.WS_APP_RESULT_OK) != 0 && 
                           (param.MaterialType == MaterialType.C || 
                            param.MaterialType == MaterialType.P ||
                            param.MaterialType == MaterialType.T ||
                            param.MaterialType == MaterialType.S);
                   }
               );
                return (temp != null ? true : false);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MediaType EventMediaType {
            get {
                return (MediaType)nMediaType;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MEPG7Status MEPG7ProcessingStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DCCommandStatus DCCommandProcessingStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastDatacastCmdSent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ARExtendedStatus ARExtendedProcessingStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastARExtendSent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Boolean IsCommercialBreaksAssociated { get; set; }
        #endregion 

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        public EpgEntry()
        {
            this.idField = string.Empty;
            this.estimatedStartDateTimeField = DateTime.MinValue;
            this.estimatedStopDateTimeField = DateTime.MinValue;
            this.exactStartDateTimeField = DateTime.MinValue;
            this.exactStopDateTimeField = DateTime.MinValue;
            this.createdDateTime = DateTime.UtcNow;
            this.channelDesignationField = string.Empty;
            this.originalTitleField = string.Empty;
            this.busField = string.Empty;
            this.requestedVideoItemField = string.Empty;
            this.statusMessage = string.Empty;
            this.stateField = EpgStatus.Unstarted;
            this.lysisLinkedSegmentIDCollectionField = new StringCollection();
            this.realDurationField = TimeSpan.FromSeconds(0);
            this.linkedSegmentCollectionField = new List<Segment>();
            this.threads = new List<Thread>(); ;
            this.contentIDField = int.MinValue;
            this.DummyCommandStatus = EpgDummyCommandStatus.PREPARED;
            this.TypeOfTrigger = TriggerType.Default;
            this.FirstTriggerReachHETime = default(DateTime);
            this.posterTransferDateTimeField = default(DateTime);
            this.posterTransferStatusField = PosterTransferStatus.PREPARED;
            this.posterTransferTriesField = default(sbyte);
            this.NextEpgCloserVideoItem = string.Empty;
            this.MEPG7ProcessingStatus = MEPG7Status.PREPARED;
            this.DCCommandProcessingStatus = DCCommandStatus.PREPARED;
            this.LastDatacastCmdSent = DateTime.MinValue;
            this.ARExtendedProcessingStatus = ARExtendedStatus.PREPARED;
        }
        #endregion 

        #region Event(s)

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Ending;                

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler StateChanged;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SegmentEventArgs> SegmentStarted;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SegmentEventArgs> SegmentStoped;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<SegmentEventArgs> SegmentFailed;
        
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AdvertisementEventArgs> AdvertisementReceptionEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AdvertisementEventArgs> AdvertisementStartEvent;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<AdvertisementEventArgs> AdvertisementEndEvent;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetState(EpgStatus value)
        {
            stateField = value;

            OnStateChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        protected virtual void OnSegmentStarted(Segment segment) {
            if (SegmentStarted != null) {
                SegmentStarted(this, new SegmentEventArgs(this, segment));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        protected virtual void OnSegmentStoped(Segment segment)
        {
            if (SegmentStoped != null)
            {
                SegmentStoped(this, new SegmentEventArgs(this, segment));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnEnding()
        {
            if (Ending != null)
            {
                Ending(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        protected virtual void OnSegmentFailed(Segment segment)
        {
            if (SegmentFailed != null)
            {
                SegmentFailed(this, new SegmentEventArgs(this, segment));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="segment"></param>
        protected virtual void OnAdvertisementReception(CommerialBreak item) {
            if (AdvertisementReceptionEvent != null) {
                AdvertisementReceptionEvent(this, new AdvertisementEventArgs(this, item));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnAdvertisementStart(CommerialBreak item) {
            if (AdvertisementStartEvent != null) {
                AdvertisementStartEvent(this, new AdvertisementEventArgs(this, item));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        protected virtual void OnAdvertisementEnd(CommerialBreak item) {
            if (AdvertisementEndEvent != null) {
                AdvertisementEndEvent(this, new AdvertisementEventArgs(this, item));
            }
        }

        #endregion 

        #region Finalizer(s)

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IDisposable LockSegments()
        {
            return new ObjectLocker(this.linkedSegmentCollectionField);
        }

        /// <summary>
        /// Kill all ended trigger threads either successfully or with error.
        /// </summary>
        public void Ended()
        {
            if (monoThread != null &&
                monoThread.Count > 0)
            {
                foreach (Thread mainThread in monoThread)
                {
                    mainThread.Abort();

                    // Call Abort a second time if the threads have not aborted.
                    if ((mainThread.ThreadState &
                        (ThreadState.Aborted | ThreadState.Stopped)) == 0)
                    {
                        mainThread.Abort();
                    }
                    // Wait for the threads to terminate.
                    mainThread.Join(TimeSpan.FromSeconds(1));                    
                }                
                monoThread.Clear();
                monoThread = null;
            }

        }
 
        #endregion 

        #region - Private Method(s) -

        #endregion

        #region - Public Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void AddNewSegment(Segment item) {
            locker.EnterWriteLock();

            try {
                this.linkedSegmentCollectionField.Add(item);
            }
            finally {
                locker.ExitWriteLock();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<Segment> LinkedSegmentCollectionReadOnly {
            get {
                locker.EnterWriteLock();
                try {
                    return this.linkedSegmentCollectionField.AsReadOnly();
                }
                finally {
                    locker.ExitWriteLock();
                }
            }
        }
        #endregion

        #region Main Method(s)

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            switch (this.EventMediaType) {
                case MediaType.UNKNOWN_CHANNEL:
                    break;

                case MediaType.TV_CHANNEL:
                    switch (this.stateField) {
                        case EpgStatus.Unstarted:
                            RunEpg();
                            break;

                        case EpgStatus.Prepared:
                            break;

                        case EpgStatus.Waiting:
                            break;

                        case EpgStatus.Running:
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

                        default:
                            break;
                    }
                    break;

                case MediaType.DC_CHANNEL:
                    MonitorDatacastLifeCycle();
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

        // Logic Respected        = ACK
        // Code Checked           = ACK
        // Code Commented         = ACK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// Resume an ongoing Automatic-recording event either by resuming an already pausing segment or starting next segment.
        /// </summary>
        public void ResumeEpg(Segment segmentItem)
        {
            ResumeSegment(segmentItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segmentItem"></param>
        public void StopEpg(Segment segmentItem) {
            switch (this.EventMediaType) {
                case MediaType.UNKNOWN_CHANNEL:
                    break;
                case MediaType.TV_CHANNEL:
                    StopSegment(segmentItem);
                    break;
                case MediaType.DC_CHANNEL:
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

        // Logic Respected        = NAK
        // Code Checked           = NAK
        // Code Commented         = NAK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// Start Automatic-recording event.
        /// </summary>
        public void RunEpg()
        {
            RunSegment();
        }

        // Logic Respected        = NAK
        // Code Checked           = NAK
        // Code Commented         = NAK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// Start Automatic-recording Event Segment.
        /// </summary>
        public void RunSegment()
        {
            PrepareDoWork(GetSegmentByVIDEO_ITEM(this.RequestedVideoItem));
        }

        /// <summary>
        /// Resume Automatic-recording Event Segment.
        /// </summary>
        /// <param name="segmentItem"></param>
        public void ResumeSegment(Segment segmentItem) {
            PrepareDoWork(segmentItem);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="segmentItem"></param>
        public void StopSegment(Segment segmentItem) {
            EndDoWork(segmentItem);
        }

        // Logic Respected        = NAK
        // Code Checked           = NAK
        // Code Commented         = NAK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// 
        /// </summary>
        public void StopPreviousSegment()
        {
            lock (this.syncRoot)
            {
                Segment temp = this.LinkedSegmentCollection.Find
                    (
                        delegate(Segment param)
                        {
                            return param.Status == SegmentState.Running || param.Status == SegmentState.Suspended;
                        }
                    );

                if (temp != null)
                {
                    temp.Status = SegmentState.Stopped;
                }
            }
        }

        // Logic Respected        = NAK
        // Code Checked           = NAK
        // Code Commented         = NAK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="video_Item">string</param>
        /// <returns>Segment</returns>
        public Segment GetSegmentByVIDEO_ITEM(string video_Item)
        {
            lock (this.syncRoot)
            {
                Segment temp = this.LinkedSegmentCollection.Find
               (
                   delegate(Segment param)
                   {
                       return param.VIDEO_ITEM == video_Item;
                   }
               );

                return temp ?? null;
            }
        }

        // Logic Respected        = ACK
        // Code Checked           = ACK
        // Code Commented         = ACK
        // Best pratice           = NAK
        // Premiminary test(s)    = NAK
        // Unit Test(s)           = NAK
        /// <summary>
        /// Start Automatic-recording event segment.
        /// </summary>
        /// <summary>
        /// Represents the method that triggers a giving Segment Thread (Handle Segement life time and state).
        /// </summary>
        /// <param name="sender">An object that contains data (typeof(Segment)) for the thread procedure.</param>
        private void PrepareDoWork(object sender) {
            Thread segmentThread = new Thread(new ParameterizedThreadStart(DoWorkRun));
            Segment tempSegment = sender as Segment;
            segmentThread.Name = string.Format("{0}|{1}|{2}",
                                                DateTime.Now.Ticks,
                                                tempSegment.VIDEO_ITEM,
                                                tempSegment.TITLE);                     // DateTime.Now.Ticks|VIDEO_ITEM|TIME
            lock (this.syncRoot) {
                threads.Add(segmentThread);
            }
            segmentThread.Start(sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void EndDoWork(object sender) {
            Thread segmentThread = new Thread(new ParameterizedThreadStart(DoWorkEnd));
            Segment tempSegment = sender as Segment;
            segmentThread.Name = string.Format("{0}|{1}|{2}",
                                                DateTime.Now.Ticks,
                                                tempSegment.VIDEO_ITEM,
                                                tempSegment.TITLE);                     // DateTime.Now.Ticks|VIDEO_ITEM|TIME
            lock (this.syncRoot) {
                threads.Add(segmentThread);
            }
            segmentThread.Start(sender);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void DoWorkRun(object sender) {            
            if (sender is Segment) {
                string msg = string.Empty;
                Segment item = sender as Segment;
                if (item != null) {
                    TriggerEntry triggerEntryTemp = this.LastHarrisTrigger;

                    item.Status = SegmentState.Prepared;
                    if (this.State == EpgStatus.Unstarted)
                        SetState(EpgStatus.Prepared);

                    DateTime segmentStopDateTime = DateTime.Now.ToUniversalTime().Date;
                    DateTime segmentStartDateTime = DateTime.Now.ToUniversalTime().Date;

                    item.DURATION = triggerEntryTemp.DURATION;
                    item.TIME = this.LastHarrisTrigger.TIME.TimeOfDay;
                    item.TITLE = triggerEntryTemp.TITLE;
                    item.BUS = triggerEntryTemp.BUS;                    
                    this.RealDuration += item.DURATION;
                    item.VIDEO_ITEM = triggerEntryTemp.VIDEO_ITEM;
                    item.MaterialType = triggerEntryTemp.TypeOfMaterial;

                    segmentStartDateTime = segmentStartDateTime + item.TIME;
                    segmentStopDateTime = segmentStopDateTime + item.TIME;
                    segmentStopDateTime = segmentStopDateTime.Add(item.DURATION);
                    segmentStopDateTime = Utils.NextTimeOfDayAfter(segmentStopDateTime.TimeOfDay, DateTime.UtcNow, TriggerEntry.TriggerGracePeriod);

                    do {
                        if (DateTime.Now.ToUniversalTime().CompareTo(segmentStartDateTime) > 0) {

                            if (this.State == EpgStatus.Prepared) {
                                SetState(EpgStatus.Running);
                            }

                            if (item.Status != SegmentState.Running) {
                                item.Status = SegmentState.Running;
                                OnSegmentStarted(item);
                            }
                        }

                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    } while (segmentStopDateTime.CompareTo(DateTime.Now.ToUniversalTime()) > 0);

                    if (item.Status == SegmentState.Running) {
                        item.Status = SegmentState.Stopped;
                        OnSegmentStoped(item);
                    }
                }
                else {
                    //-----.DefaultLogger.STARLogger.Error("Segment referenece is null.", new NullReferenceException());
                    SetState(EpgStatus.Failed);
                }
            }
            else {
                //-----.DefaultLogger.STARLogger.Error("Cannot parse object to Segment.");
            }
            //-----.DefaultLogger.STARLogger.Info(string.Format("Thread.Name = {0} Finished", Thread.CurrentThread.Name));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        private void DoWorkEnd(object sender) {
            if (sender is Segment) {
                string msg = string.Empty;
                Segment item = sender as Segment;
                if (item != null) {                    

                    item.Status = SegmentState.Prepared;

                    DateTime segmentStopDateTime = DateTime.Now.ToUniversalTime().Date;
                    DateTime segmentStartDateTime = DateTime.Now.ToUniversalTime().Date;
                                        
                    segmentStartDateTime = segmentStartDateTime + item.TIME;
                    segmentStopDateTime = segmentStopDateTime + item.TIME;
                    segmentStopDateTime = segmentStopDateTime.Add(item.DURATION);
                    segmentStopDateTime = Utils.NextTimeOfDayAfter(segmentStopDateTime.TimeOfDay, DateTime.UtcNow, TriggerEntry.TriggerGracePeriod);

                    do {
                        if (DateTime.Now.ToUniversalTime().CompareTo(segmentStartDateTime) > 0) {
                            //-----.DefaultLogger.STARLogger.Error("Salut Stop the fucking EPG = " + this.OriginalTitle);
                            SetState(EpgStatus.Stopped);
                            break;                            
                        }

                        Thread.Sleep(TimeSpan.FromMilliseconds(50));
                    } while (segmentStopDateTime.CompareTo(DateTime.Now.ToUniversalTime()) > 0);
                }
                else {
                    //-----.DefaultLogger.STARLogger.Error("Segment referenece is null.", new NullReferenceException());
                    SetState(EpgStatus.Failed);
                }
            }
            else {
                //-----.DefaultLogger.STARLogger.Error("Cannot parse object to Segment.");
            }
            //-----.DefaultLogger.STARLogger.Info(string.Format("Thread.Name = {0} Finished", Thread.CurrentThread.Name));
        }

        #endregion

        #region - Auxiliary Method(s)

        /// <summary>
        /// 
        /// </summary>
        public bool IsDuplicateSegment(TriggerEntry triggerEntryItem) {

            Segment temp = this.LinkedSegmentCollection.Find
           (
               delegate(Segment param) {
                   return string.Compare(param.VIDEO_ITEM, triggerEntryItem.VIDEO_ITEM) == 0 &&
                          string.Compare(param.TITLE, triggerEntryItem.TITLE) == 0 &&
                          param.TIME.CompareTo(triggerEntryItem.TIME.TimeOfDay)== 0 &&
                          param.DURATION.CompareTo(triggerEntryItem.DURATION) == 0;
               }
           );
            return (temp != null ? 
                    true : 
                    false);
        }

        #endregion 

        #region - VOD :: Private Method(s) -

        void MonitorDatacastLifeCycleWorkerThread() 
        {
            Thread datacastLifeCycleWorkerThread = new Thread(new ThreadStart(MonitorDatacastLifeCycleRoutine));
            datacastLifeCycleWorkerThread.Name = string.Format("{0}|{1}|{2}",
                                                                DateTime.Now.Ticks,
                                                                this.OriginalTitle,
                                                                this.EstimatedStopDateTime);
            lock (this.syncRoot) {
                threads.Add(datacastLifeCycleWorkerThread);
            }
            datacastLifeCycleWorkerThread.Start();
        }

        void MonitorDatacastLifeCycleRoutine()
        {
            string msg = string.Empty;
            lastError = null;

            if (this.EstimatedStartDateTime >= this.EstimatedStopDateTime)
            {
                msg = string.Format("Schedule {0}|{1}|{2}|{3} Start/Stop DateTimes are corrupted and it will not be processed, schedule will be Aborted",
                                     this.ID,
                                     this.ContentID,
                                     this.OriginalTitle,
                                     this.EstimatedStartDateTime);
                MainForm.LogWarningToFile(msg);
                lastError = new Exception(msg);
                SetState(EpgStatus.Aborted);
                return;
            }

            if (this.EstimatedStartDateTime < DateTime.UtcNow)
            {
                msg = string.Format("Schedule {0}|{1}|{2}|{3} is outdated and it will not be processed, schedule will be Aborted",
                                     this.ID,
                                     this.ContentID,
                                     this.OriginalTitle,
                                     this.EstimatedStartDateTime);
                MainForm.LogWarningToFile(msg);
                lastError = new Exception(msg);
                SetState(EpgStatus.Aborted);
                return;
            }

            SetState(EpgStatus.Preparing);

            do
            {
                lastError = null;
                if (this.State == EpgStatus.Aborted)
                {
                    //lastError = new Exception("See Log File");
                    return;
                }

                switch (this.State)
                {
                    case EpgStatus.Preparing: //---- OK
                        if (this.MEPG7ProcessingStatus == MEPG7Status.NAK)
                        {
                            msg = string.Format("Schedule {0} - {1} - {2} - {3} MPEG7 failed [NAK] ! it will not be processed, schedule will be Aborted.",
                                                 this.ID,
                                                 this.ContentID,
                                                 this.OriginalTitle,
                                                 this.EstimatedStartDateTime);
                            MainForm.LogErrorToFile(msg);
                            lastError = new Exception(msg);
                            SetState(EpgStatus.Aborted);
                            return;
                        }

                        if (this.DCCommandProcessingStatus == DCCommandStatus.NAK)
                        {
                            msg = string.Format("Schedule {0} - {1} - {2} - {3} DCCommand failed ! [NAK] it will not be processed, schedule will be Aborted!",
                                                 this.ID,
                                                 this.ContentID,
                                                 this.OriginalTitle,
                                                 this.EstimatedStartDateTime);
                            MainForm.LogErrorToFile(msg);
                            lastError = new Exception(msg);
                            SetState(EpgStatus.Aborted);
                            return;
                        }

                        //------ Try to Owned the Schedule
                        bool lockAcquired = MEBSCatalogProvider.AttemptAcquireScheduleLock(Int32.Parse(this.ID));

                        if (!lockAcquired)
                        {
                            msg = string.Format("DataCasting :: lockAcquired {0} for Schedule {1}, schedule will be Aborted!", lockAcquired, this.ID);
                            MainForm.LogWarningToFile(msg);
                            lastError = new Exception(msg);
                            SetState(EpgStatus.Aborted);
                            return;
                        }
                        else
                        {
                            SetState(EpgStatus.Prepared);
                        }
                        break;

                    case EpgStatus.Prepared: //----- OK
                        if (this.MEPG7ProcessingStatus == MEPG7Status.NAK)
                        {
                            msg = string.Format("Schedule {0} - {1} - {2} - {3} MPEG7 failed [NAK] ! it will not be processed, schedule will be Aborted!",
                                     this.ID,
                                     this.ContentID,
                                     this.OriginalTitle,
                                     this.EstimatedStartDateTime);
                            MainForm.LogErrorToFile(msg);
                            lastError = new Exception(msg);
                            SetState(EpgStatus.Aborted);
                            return;
                        }

                        bool isTimeToSendDatacastCommand = this.IsTimeToSendDatacastCommand();

                        if (isTimeToSendDatacastCommand)
                        {
                            EpgEntry lastDCSend = EPGManager.Instance.GetLastDatacastingSentEvent();
                            if (lastDCSend != null)
                            {
                                isTimeToSendDatacastCommand = this.IsTimeToSendDatacastCommand(lastDCSend.LastDatacastCmdSent);
                                if (isTimeToSendDatacastCommand)
                                {
                                    if (this.MEPG7ProcessingStatus == MEPG7Status.ACK &&
                                        this.DCCommandProcessingStatus == DCCommandStatus.UPLOADED)
                                    {
                                        SetState(EpgStatus.Waiting);
                                    }
                                }
                            }
                            else
                            {
                                if (this.MEPG7ProcessingStatus == MEPG7Status.ACK &&
                                    this.DCCommandProcessingStatus == DCCommandStatus.UPLOADED)
                                {
                                    SetState(EpgStatus.Waiting);
                                }
                            }
                        }
                        break;
                    case EpgStatus.Waiting: //----- OK
                        if (this.DCCommandProcessingStatus == DCCommandStatus.NAK)
                        {
                            msg = string.Format("Schedule {0} - {1} - {2} - {3} DCCommand failed [NAK] ! it will not be processed, schedule will be Aborted!",
                                                 this.ID,
                                                 this.ContentID,
                                                 this.OriginalTitle,
                                                 this.EstimatedStartDateTime);
                            lastError = new Exception(msg);
                            SetState(EpgStatus.Aborted);
                            return;
                        }
                        if (DateTime.Compare(DateTime.UtcNow, this.EstimatedStartDateTime) >= 0) //.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET)
                        {
                            SetState(EpgStatus.Running);
                        }
                        break;

                    default:
                        break;
                }
                Thread.Sleep(1000);
            } while (this.EstimatedStopDateTime.CompareTo(DateTime.UtcNow) > 0);
            SetState(EpgStatus.Stopped);
        }
   
        void MonitorARExtendedLifeCycleWorkerThread() {
            Thread arExtendedLifeCycleWorkerThread = new Thread(new ThreadStart(MonitorARExtendedLifeCycleRoutine));
            arExtendedLifeCycleWorkerThread.Name = string.Format("{0}|{1}|{2}",
                                                                DateTime.Now.Ticks,
                                                                this.OriginalTitle,
                                                                this.EstimatedStopDateTime);
            lock (this.syncRoot) {
                threads.Add(arExtendedLifeCycleWorkerThread);
            }
            arExtendedLifeCycleWorkerThread.Start();
        }
       
        void MonitorARExtendedLifeCycleRoutine() {
            lock (locker1) {
                string msg = string.Empty;
                string reply = string.Empty;

                do {
                    switch (this.ARExtendedProcessingStatus) {
                        case ARExtendedStatus.PREPARED:
                            bool result = PlayoutCommandProvider.UploadARExtendedFile(this);
                            if (result) {
                                this.ARExtendedProcessingStatus = ARExtendedStatus.UPLOADED;
                            }
                            else {
                                this.ARExtendedProcessingStatus = ARExtendedStatus.NAK;
                            }
                            break;

                        case ARExtendedStatus.UPLOADED:
                            bool isTimeToSendARExtendedXmlFile = this.IsTimeToSendARExtendedXmlFile();
                            //-----.DefaultLogger.STARLogger.Info(this.ID + " isTimeToSendARExtendedXmlFile = " + isTimeToSendARExtendedXmlFile);
                            if (isTimeToSendARExtendedXmlFile) {
                                //-----.DefaultLogger.STARLogger.Info(this.ID + " Check previous ");
                                EpgEntry lastARExtendedSend = EPGManager.Instance.GetLastARAdvExtendSentEvent();
                                if (lastARExtendedSend != null) {
                                    //-----.DefaultLogger.STARLogger.Info("LastARExtendedSend ID = " + lastARExtendedSend.ID);
                                    //-----.DefaultLogger.STARLogger.Info("LastARExtendedSend OriginalTitle = " + lastARExtendedSend.OriginalTitle);
                                    //-----.DefaultLogger.STARLogger.Info("LastARExtendedSend LastARExtendSent = " + lastARExtendedSend.LastARExtendSent);
                                    isTimeToSendARExtendedXmlFile = this.IsTimeToSendARExtendedXmlFile(lastARExtendedSend.LastARExtendSent);
                                    if (isTimeToSendARExtendedXmlFile) {
                                        this.ARExtendedProcessingStatus = ARExtendedStatus.WAITING;
                                    }
                                    else {
                                        this.ARExtendedProcessingStatus = ARExtendedStatus.BLOCKED;
                                    }
                                }
                                else {
                                    this.ARExtendedProcessingStatus = ARExtendedStatus.WAITING;
                                }

                            }
                            else {
                                //-----.DefaultLogger.STARLogger.Info(this.ID + "Still not time to send AR Extended Xml File.");
                            }

                            break;

                        case ARExtendedStatus.WAITING:
                            reply = PlayoutCommandProvider.SendARExtendedCommand(this);
                            if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK, true) == 0) {
                                this.ARExtendedProcessingStatus = ARExtendedStatus.ACK;
                                this.LastARExtendSent = DateTime.UtcNow;
                                //-----.DefaultLogger.STARLogger.Info(this.ID + " " + reply);
                            }
                            else {
                                this.ARExtendedProcessingStatus = ARExtendedStatus.NAK;
                            }
                            break;

                        case ARExtendedStatus.BLOCKED:
                            //-----.DefaultLogger.STARLogger.Warn(this.ID + "Bloqued Still not time to send AR Extended Xml File.");
                            EpgEntry lastARExtendedSend1 = EPGManager.Instance.GetLastARAdvExtendSentEvent();
                            if (lastARExtendedSend1 != null) {
                                isTimeToSendARExtendedXmlFile = this.IsTimeToSendARExtendedXmlFile(lastARExtendedSend1.LastARExtendSent);
                                if (isTimeToSendARExtendedXmlFile) {
                                    this.ARExtendedProcessingStatus = ARExtendedStatus.WAITING;
                                }
                            }
                            break;

                        case ARExtendedStatus.ACK:
                            return;
                            break;

                        case ARExtendedStatus.NAK:
                            return;
                            break;

                        default:
                            break;
                    }
                    Thread.Sleep(1000);
                } while (this.EstimatedStartDateTime.CompareTo(DateTime.UtcNow) > 0);
            }
        }

        #endregion

        #region - VOD :: Public Method(s) -
        /// <summary>
        /// 
        /// </summary>
        public void MonitorDatacastLifeCycle() {
            MonitorDatacastLifeCycleWorkerThread();
        }

        /// <summary>
        /// 
        /// </summary>
        public void MonitorARExtendedLifeCycle() {
            //MonitorARExtendedLifeCycleWorkerThread();
        }
        #endregion

        #region - VOD :: Auxiliary Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTimeToSendDatacastCommand() {
            return DateTime.UtcNow.CompareTo(this.EstimatedStartDateTime.Add(PlayoutCommandProvider.DCCommand_Sending_Offset)) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTimeToSendDatacastCommand(DateTime lastDatacastCmdSent) {
            return DateTime.UtcNow.Subtract(lastDatacastCmdSent) > PlayoutCommandProvider.DC_Inter_Command_Time_Gap;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTimeToSendARExtendedXmlFile() {
            return DateTime.UtcNow.CompareTo(this.EstimatedStartDateTime.Add(PlayoutCommandProvider.ExtendedAdv_Sending_Offset)) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsTimeToSendARExtendedXmlFile(DateTime lastARExtendedFileSent) {
            return DateTime.UtcNow.Subtract(lastARExtendedFileSent) > PlayoutCommandProvider.AR_Extended_Command_Time_Gap;
        }
        #endregion 
    }
}
