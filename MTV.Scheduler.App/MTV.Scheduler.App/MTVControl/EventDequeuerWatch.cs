using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Threading;
using System.ServiceModel.Channels;
using System.Timers;
using MTV.EventDequeuer.Contracts.Data;
using MTV.EventDequeuer.Contracts.Service;
using MTV.Scheduler.App.UI;
using MTV.Scheduler.App.Properties;
using MTV.Scheduler.App.MTVControl;
using MTV.Scheduler.App.MEBSCatalogServiceRef;
using MTV.Library.Core;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Proxy;
using MTV.Library.Core.Tools;
using MTV.Library.Core.Services;
using MTV.Scheduler.App.MTV.Library.Core;
using Microsoft.Win32;



namespace MTV.Scheduler.App.MTVControl
{
    //[CallbackBehavior(UseSynchronizationContext = false)]
    public class EventDequeuerWatch : IEventDequeuerCallback
    {
        private readonly static Lazy<EventDequeuerWatch> instance = new Lazy<EventDequeuerWatch>(() => new EventDequeuerWatch());
        private InstanceContext instanceContext;
        private Guid subscriptionId;
        private bool isSubscribed;
        private TimeSpan receiveTimeout;
        private System.Timers.Timer subscriberLeaseRenewalTimer;
        private  bool isRunningThread = false;
        EventDequeuerProxy proxy;


        public event EventHandler<StreamingResultEventArgs> LastVideoContentLockedOnNotificationReceived;
        public event EventHandler<StreamingResultEventArgs> LastVideoContentScheduledOnNotificationReceived;
        public event EventHandler<StreamingResultEventArgs> LastVideoContentRemovedOnNotificationReceived;
        public event EventHandler<StreamingResultEventArgs> LastCategoryItemsChangedOnNotificationReceived;
        
        /// <summary>
        ///  Constructor
        /// </summary>
        public EventDequeuerWatch()
        {

            instanceContext = new InstanceContext(this);
            //Create Proxy
            CreateProxy();
            Binding binding = proxy.Endpoint.Binding;
            receiveTimeout = binding.ReceiveTimeout;        
            subscriberLeaseRenewalTimer = new System.Timers.Timer(receiveTimeout.TotalMilliseconds);
            subscriberLeaseRenewalTimer.Enabled = true;
            subscriberLeaseRenewalTimer.Start();
            subscriberLeaseRenewalTimer.Elapsed += SubscriberLeaseRenewalTimer_Elapsed;
            //Reload Settings
            //ReloadSettings();
            DoubleCheckSettings();
            

           
        }

        /// <summary>
        /// Create Proxy
        /// </summary>
        public void CreateProxy()
        {
            proxy = new EventDequeuerProxy(instanceContext);
            
                   
        }   

        /// <summary>
        ///  Get static Instance
        /// </summary>
        public static EventDequeuerWatch Instance
        {
            get
            {
                return instance.Value;
            }
        }

        /// <summary>
        /// Beging Subscribe
        /// </summary>
        public void Subscribe()
        {

            //ThreadPool.QueueUserWorkItem(x =>
            //{
                try
                {
                    subscriptionId = Guid.NewGuid();
                    proxy.Subscribe(subscriptionId, new string[] { "LastVideoContentLockedOn", "LastVideoContentScheduledOn", "LastVideoContentRemovedOn", "LastCategoryItemsChangedOn", "LastVideoContentExpiredOn" });
                    isSubscribed = true;
                   
                   
                }
                catch(Exception ex)
                {
                    //MainForm.LogMessageToFile("|" + ex.ToString());


                    //if (proxy != null)
                    //{

                    //    if (proxy.State != CommunicationState.Faulted)
                    //    {

                    //        try
                    //        {
                    //            proxy.Close();
                    //        }
                    //        catch
                    //        {
                    //            proxy.Abort();
                    //        }
                            

                    //    }
                    //    else
                    //    {
                           
                    //        proxy.Abort();
                    //    }

                    //}

                    //proxy = null;
                }
           // });

        }

        /// <summary>
        /// On Receiving Stream From MTV.Dequeur.Host
        /// </summary>
        /// <param name="streamingResult">Obj RealTimeEventMsg</param>
        public void ReceiveStreamingResult(RealTimeEventMsg streamingResult)
        {
          
            // Ignore Notification if the task is still running.
            if (isRunningThread) return;

            switch (streamingResult.EventName)
            {

                case "LastVideoContentLockedOn":

                   // Notify
                    OnLastVideoContentLockedOnNotificationReceived(streamingResult);    
                  // Execute LastVideoContentLockedOn Method
                    if (streamingResult.Date != null)
                    {
                        DateTime _dtLastVideoContentLockedOn = Settings.Default.LastVideoContentLockedOn;
                        if (DateTime.Compare(streamingResult.Date.Value, _dtLastVideoContentLockedOn) > 0)
                        {

                            mebsEntities _context = new mebsEntities(Configuration.MTVCatalogLocation);
                            string queryString = string.Format(Configuration.GetSchedulesByStartTime,
                                                              Convert.ToInt32(ScheduleStatus.LOCKED),
                                                              DateTimeUtils.ConvertDateTimeToEDMFormat(DateTime.UtcNow),
                                                              DateTimeUtils.ConvertDateTimeToEDMFormat(DateTime.UtcNow.AddHours(24)));
                            isRunningThread = true;
                            
                            var results =
                               _context.BeginExecute<mebs_schedule>(new Uri(queryString, UriKind.Relative),
                                                                    OnAsyncExecutionComplete,
                                                                    Tuple.Create(_context, streamingResult));


                        }
                    }


                    break;

                case "LastVideoContentScheduledOn":

                    //Notify
                    OnLastVideoContentScheduledOnNotificationReceived(streamingResult);
                    if (streamingResult.Date != null)
                    {
                        DateTime _dtLastVideoContentScheduledOn = Settings.Default.LastVideoContentScheduledOn;
                        if (DateTime.Compare(streamingResult.Date.Value, _dtLastVideoContentScheduledOn) > 0)
                        {
                           
                            SendFutureDClist(streamingResult);
                        }
                    }

                    break;


                case "LastVideoContentRemovedOn":

                    //Notify
                    OnLastVideoContentRemovedOnNotificationReceived(streamingResult);
                    if (streamingResult.Date != null)
                    {
                        DateTime _dtLastVideoContentRemovedOn = Settings.Default.LastVideoContentRemovedOn;
                        if (DateTime.Compare(streamingResult.Date.Value, _dtLastVideoContentRemovedOn) > 0)
                        {
                           
                            SendFutureDClist(streamingResult);

                        }
                    }

                    break;

                case "LastCategoryItemsChangedOn":

                    //Notify
                    OnLastCategoryItemsChangedOnNotificationReceived(streamingResult);
                    if (streamingResult.Date != null)
                    {
                        DateTime _dtLastCategoryItemsChangedOn = Settings.Default.LastCategoryItemsChangedOn;
                        if (DateTime.Compare(streamingResult.Date.Value, _dtLastCategoryItemsChangedOn) > 0)
                        {
                            
                            SendCategorizationFile(streamingResult);

                        }
                    }
                    break;

                case "LastVideoContentExpiredOn":

                    if (streamingResult.Date != null)
                    {
                        DateTime _dLastVideoContentExpiredOn = Settings.Default.LastVideoContentExpiredOn;
                        if (DateTime.Compare(streamingResult.Date.Value, _dLastVideoContentExpiredOn) > 0)
                        {
                            ClearTheExpiredContent(streamingResult);
                        }
                    }

                    break;

                default:
                    break;

            }
        }

        /// <summary>
        /// End Subscription
        /// </summary>
        public void EndSubscription()
        {

            try
            {
                if (isSubscribed)
                {
                   // ThreadPool.QueueUserWorkItem(x =>
                    //{
                        try
                        {
                            if (proxy != null)
                            {

                                try
                                {
                                    if (proxy.State != CommunicationState.Faulted)
                                    {
                                        proxy.EndSubscription(subscriptionId);
                                        proxy.Close();
                                        //MainForm.LogMessageToFile("Closed.");
                                    }
                                    else
                                    {
                                       // MainForm.LogMessageToFile("Aborted. case = faulted");
                                        proxy.Abort();
                                    }

                                    proxy = null;

                                }
                                catch (Exception ex)
                                {
                                    //MainForm.LogExceptionToFile(ex);
                                    //MainForm.LogMessageToFile("aborted with exception.");
                                    proxy.Abort();
                                }
                                
                                finally
                                {

                                    proxy = null;

                                }



                            }

                           

                            //EventDequeuerProxy proxy = new EventDequeuerProxy(instanceContext);
                            //proxy = new EventDequeuerProxy(instanceContext);
                            //proxy.EndSubscription(subscriptionId);
                         
                            //if (proxy != null)
                            //{

                            //    if (proxy.State != CommunicationState.Faulted)
                            //    {

                            //        try
                            //        {
                            //            proxy.Close();
                            //        }
                            //        catch
                            //        {
                            //            proxy.Abort();
                            //        }
                                  
                                    
                            //    }
                            //    else
                            //    {
                                   
                            //        proxy.Abort();
                            //    }
                                
                            //}

                            //proxy = null;
                            
                        }
                        catch(Exception ex)
                        {
                            MainForm.LogExceptionToFile(ex);

                            //if (proxy != null)
                            //{
                            //    if (proxy.State != CommunicationState.Faulted)
                            //    {
                            //        try
                            //        {
                            //            proxy.Close();
                            //        }
                            //        catch
                            //        {
                            //            proxy.Abort();
                            //        }
                            //    }
                            //    else
                            //    {
                            //        proxy.Abort();
                            //    }
                            //}

                            //proxy = null;
                           
                        }
                   // });

                    isSubscribed = false;

                  
                    
                }

               

                
                
            }
            catch (Exception ex)
            {

                MainForm.LogExceptionToFile(ex);
                
                //if (proxy != null)
                //{

                //    if (proxy.State != CommunicationState.Faulted)
                //    {

                //        try
                //        {
                //            proxy.Close();
                //        }
                //        catch
                //        {
                //            proxy.Abort();
                //        }

                //    }
                //    else
                //    {
                       
                //        proxy.Abort();
                //    }

                //}

                //proxy = null;
               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubscriberLeaseRenewalTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            subscriberLeaseRenewalTimer.Enabled = false;
            subscriberLeaseRenewalTimer.Stop();
            EndSubscription();
            CreateProxy();
            Subscribe();
            subscriberLeaseRenewalTimer.Enabled = true;
            subscriberLeaseRenewalTimer.Start();
        }

        /// <summary>
        /// On Get Locked schedule List Async operation Complete
        /// </summary>
        /// <param name="result"></param>
        private void OnAsyncExecutionComplete(IAsyncResult result)
        {
            Tuple<mebsEntities, RealTimeEventMsg> state = result.AsyncState as Tuple<mebsEntities, RealTimeEventMsg>;
            mebsEntities _context = state.Item1;
            string msg = string.Empty;

            try
            {

                IEnumerable<mebs_schedule> publishedEventCollection = _context.EndExecute<mebs_schedule>(result);
                if (publishedEventCollection == null) return;

                //isRunningThread = true;

                foreach (mebs_schedule objSchedule in publishedEventCollection)
                {
                    MediaType eventMediaType = (MediaType)objSchedule.mebs_ingesta.Type.Value;
                    if (objSchedule.Estimated_Stop.HasValue &&
                        objSchedule.Estimated_Stop.Value.CompareTo(DateTime.UtcNow) < 0)
                    {
                        continue;
                    }
                    EpgEntry objEpgEntry = new EpgEntry();
                    objEpgEntry.State = EpgStatus.Unstarted;
                    objEpgEntry.CreatedDateTime = DateTime.UtcNow;
                    objEpgEntry.ContentID = (int)objSchedule.ContentID;
                    objEpgEntry.EstimatedStartDateTime = objSchedule.Estimated_Start.Value;
                    objEpgEntry.EstimatedStopDateTime = objSchedule.Estimated_Stop.Value;
                    objEpgEntry.ID = objSchedule.IdSchedule.ToString();
                    objEpgEntry.OriginalTitle = objSchedule.mebs_ingesta.Title;
                    objEpgEntry.nMediaType = objSchedule.mebs_ingesta.Type;
                    objEpgEntry.Bus = objSchedule.mebs_ingesta.mebs_channel.Bus;
                    objEpgEntry.ChannelDesignation = objSchedule.mebs_ingesta.mebs_channel.LongName;
                    objEpgEntry.Poster = objSchedule.mebs_ingesta.Poster;
                    objEpgEntry.PosterFileExtension = objSchedule.mebs_ingesta.PosterFileExtension;
                    objEpgEntry.PosterTransferStatus = ((PosterTransferStatus)objSchedule.Poster_Status);

                    using (EPGManager.Instance.LockDownloadList(true))
                    {
                        if (!EPGManager.Instance.Exists(objEpgEntry.ID.ToString()))
                            EPGManager.Instance.Add(objEpgEntry, true);
                    }
                }

                Settings.Default.LastVideoContentLockedOn = state.Item2.Date.Value;
                Settings.Default.Save();


            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            finally
            {
                isRunningThread = false;
            }

        }

        /// <summary>
        /// On Sending Future Schedule List Async operation Complete
        /// </summary>
        /// <param name="result">The status of asynchronous operation</param>
        private void FuturSchedules_OnAsyncExecutionComplete(IAsyncResult result)
        {
            Tuple<mebsEntities, RealTimeEventMsg> state = result.AsyncState as Tuple<mebsEntities, RealTimeEventMsg>;
            mebsEntities _context = state.Item1;
            string msg = string.Empty;

            try
            {

                List<mebs_schedule> _futurschedulesCollection = _context.EndExecute<mebs_schedule>(result).ToList();
                if (_futurschedulesCollection == null || _futurschedulesCollection.Count() <= 0)
                    return;

                //isRunningThread = true;

                string reply = PlayoutCommandProvider.SendFuturSchedulesEntitiesCommand(_futurschedulesCollection, state.Item2.Date);

                if (reply == FuturDCListStatus.ACK.ToString())
                {

                    if (state.Item2.EventName == "LastVideoContentScheduledOn")
                        Settings.Default.LastVideoContentScheduledOn = state.Item2.Date.Value;
                    else if (state.Item2.EventName == "LastVideoContentRemovedOn")
                        Settings.Default.LastVideoContentRemovedOn = state.Item2.Date.Value;

                    Settings.Default.Save();
                }


            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            finally
            {
                isRunningThread = false;
            }
        }

        /// <summary>
        /// Start Process of Sending Future Schedule File (XML)
        /// </summary>
        /// <param name="streamingResult">Obj RealTimeEventMsg</param>
        private void SendFutureDClist(RealTimeEventMsg streamingResult)
        {
            try
            {
                mebsEntities _context = new mebsEntities(Configuration.MTVCatalogLocation);
                string queryString = string.Format(Configuration.GetFuturSchedulesEntitiesTree,
                                                  DateTimeUtils.ConvertDateTimeToEDMFormat(DateTime.UtcNow));

                isRunningThread = true;
                
                var results =
                   _context.BeginExecute<mebs_schedule>(new Uri(queryString, UriKind.Relative),
                                                        FuturSchedules_OnAsyncExecutionComplete,
                                                        Tuple.Create(_context, streamingResult));
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        /// <summary>
        /// Start Process of sending Categorization file (XML)
        /// </summary>
        /// <param name="streamingResult">Obj RealTimeEventMsg</param>
        private void SendCategorizationFile(RealTimeEventMsg streamingResult)
        {

            mebsEntities _context = new mebsEntities(Configuration.MTVCatalogLocation);
            string queryString = Configuration.GetCategorizationEntities;

            isRunningThread = true;
            
            var results =
               _context.BeginExecute<mebs_category>(new Uri(queryString, UriKind.Relative),
                                                    CategorizationList_OnAsyncExecutionComplete,
                                                    Tuple.Create(_context, streamingResult));

        }

        /// <summary>
        ///  On sending catagorization List Async operation Complete
        /// </summary>
        /// <param name="result">The status of asynchronous operation</param>
        private void CategorizationList_OnAsyncExecutionComplete(IAsyncResult result)
        {
            Tuple<mebsEntities, RealTimeEventMsg> state = result.AsyncState as Tuple<mebsEntities, RealTimeEventMsg>;
            mebsEntities _context = state.Item1;
            

            try
            {
                List<mebs_category> _categoryCollection = _context.EndExecute<mebs_category>(result).ToList();
                if (_categoryCollection == null || _categoryCollection.Count() <= 0)
                    return;


                string reply = PlayoutCommandProvider.SendCategorizationEntitiesCommand(_categoryCollection, state.Item2.Date);

                if (reply == SendCategoryStatus.ACK.ToString())
                {
                    Settings.Default.LastCategoryItemsChangedOn = state.Item2.Date.Value;
                    Settings.Default.Save();
                }

            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            finally
            {
                isRunningThread = false;
            }
        }

        /// <summary>
        ///  Exectute Clear The Expired Content Process.
        /// </summary>
        /// <param name="streamingResult">Obj RealTimeEventMsg</param>
        private void ClearTheExpiredContent(RealTimeEventMsg streamingResult)
        {
            mebsEntities _context = new mebsEntities(Configuration.MTVCatalogLocation);
            string queryString = Configuration.GetExpiredVideoContents;

            isRunningThread = true;
            
            var results =
               _context.BeginExecute<mebs_category>(new Uri(queryString, UriKind.Relative),
                                                    ExpiredContentList_OnAsyncExecutionComplete,
                                                    Tuple.Create(_context, streamingResult));
        }

        /// <summary>
        ///  On Expired Content List Async Operation Complete
        /// </summary>
        /// <param name="result">The status of asynchronous operation</param>
        private void ExpiredContentList_OnAsyncExecutionComplete(IAsyncResult result)
        {
            Tuple<mebsEntities, RealTimeEventMsg> state = result.AsyncState as Tuple<mebsEntities, RealTimeEventMsg>;
            mebsEntities _context = state.Item1;


            try
            {
                List<mebs_ingesta> _ingestaCollection = _context.EndExecute<mebs_ingesta>(result).ToList();
                if (_ingestaCollection == null || _ingestaCollection.Count() <= 0)
                    return;

                
                //----> Here your treatement to clear associated files.
                
                string reply = string.Empty;

                if (reply == SendCategoryStatus.ACK.ToString())
                {
                    Settings.Default.LastVideoContentExpiredOn = state.Item2.Date.Value;
                    Settings.Default.Save();
                }

            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            finally
            {
                isRunningThread = false;
            }
        }

        /// <summary>
        ///  Close the proxy because the Application terminated
        /// </summary>
        public void Close()
        {
            ReloadSettings();

            // Update Registry
            SetRegistrySetting("LastVideoContentScheduledOn", Settings.Default.LastVideoContentScheduledOn.ToString());
            SetRegistrySetting("LastVideoContentRemovedOn",   Settings.Default.LastVideoContentRemovedOn.ToString());
            SetRegistrySetting("LastCategoryItemsChangedOn",  Settings.Default.LastCategoryItemsChangedOn.ToString());
            
            //Dispose Timer
            if (subscriberLeaseRenewalTimer != null)
                subscriberLeaseRenewalTimer.Dispose();

            if (proxy != null)
            {
                try
                {
                    if (proxy.State != CommunicationState.Faulted)
                    {
                        proxy.EndSubscription(subscriptionId);
                        proxy.Close();
                    }
                }
                catch
                {
                    proxy.Abort();
                }
            }

           


        }
        /// <summary>
        /// Close the proxy because it was faulted.
        /// </summary>
        private void CloseProxyBecauseOfException()
        {
            if (proxy != null)
            {
                var wcfProxy = proxy as ICommunicationObject;

                 try
                    {
                        if (wcfProxy != null)
                        {
                            if (wcfProxy.State != CommunicationState.Faulted)
                            {
                                wcfProxy.Close();
                            }
                            else
                            {
                                wcfProxy.Abort();
                            }
                        }
                    }

                 catch (CommunicationException)
                 {
                         if (wcfProxy != null)
                         {
                             wcfProxy.Abort();
                         }
                 }

                 catch (TimeoutException)
                 {
                         if (wcfProxy != null)
                         {
                             wcfProxy.Abort();
                         }
                 }

                 catch
                 {
                     if (wcfProxy != null)
                     {
                         wcfProxy.Abort();
                     }

                     throw;
                 }
                 
                 finally
                 {
                     proxy = null;
                 }


            }
        }


        private void DoubleCheckSettings()
        {
            try
            {
                if (CheckRegistrySetting("LastVideoContentScheduledOn"))
                    Settings.Default.LastVideoContentScheduledOn = DateTime.Parse(GetRegistrySetting("LastVideoContentScheduledOn"));
                else
                    Settings.Default.LastVideoContentScheduledOn = GetSettingbyName("LastVideoContentScheduledOn");

                if (CheckRegistrySetting("LastVideoContentRemovedOn"))
                     Settings.Default.LastVideoContentRemovedOn = DateTime.Parse(GetRegistrySetting("LastVideoContentRemovedOn"));
                else
                     Settings.Default.LastVideoContentRemovedOn = GetSettingbyName("LastVideoContentRemovedOn");

                if (CheckRegistrySetting("LastCategoryItemsChangedOn"))
                    Settings.Default.LastCategoryItemsChangedOn = DateTime.Parse(GetRegistrySetting("LastCategoryItemsChangedOn")); 
                else
                    Settings.Default.LastCategoryItemsChangedOn = GetSettingbyName("LastCategoryItemsChangedOn");
                
                
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

         }
        

        
        /// <summary>
        ///  Saves the given DB settings (LastVideoContentScheduledOn , LastVideoContentRemovedOn , LastCategoryItemsChangedOn ) into an App Settings
        /// </summary>
        private void ReloadSettings()
        {

           try
            {

                Settings.Default.LastVideoContentScheduledOn = GetSettingbyName("LastVideoContentScheduledOn");
                Settings.Default.LastVideoContentRemovedOn = GetSettingbyName("LastVideoContentRemovedOn");
                Settings.Default.LastCategoryItemsChangedOn = GetSettingbyName("LastCategoryItemsChangedOn");
                
                Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

          }

        /// <summary>
        /// Get setting by name from MTV.Catalog.Host
        /// </summary>
        /// <param name="_settingName">Name of the setting</param>
        /// <returns>Setting value as DateTime</returns>
        public DateTime GetSettingbyName (string _settingName)
        {
            try
            {
                mebsEntities mEbsEntities = new mebsEntities(Configuration.MTVCatalogLocation);
                bool isExists = mEbsEntities.mebs_settings.ToList().Where(n => n.SettingName.CompareTo(_settingName) == 0).Any();
                return (isExists ?
                        DateTime.Parse(mEbsEntities.mebs_settings.ToList().Where(n => n.SettingName.CompareTo(_settingName) == 0).First().SettingValue) :
                        DateTime.MinValue);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private string GetRegistrySetting(string settingParam)
        {

            string scheduledOn = string.Empty;
            try
            {
                using (RegistryKey rkey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\MTV.Scheduler.App"))
                     if (rkey != null)
                         scheduledOn = string.Format("{0}", rkey.GetValue(settingParam));
            }
            catch (Exception ex)
            {
                MainForm.LogErrorToFile(string.Format("Error checking registry for registered setting: LastVideoContentScheduledOn - {0}", ex.Message));
            }

            return scheduledOn;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheduledOnValue"></param>
        private void SetRegistrySetting(string settingName , string settingValue)
        {

            string scheduledOn = string.Empty;
            try
            {

                using (RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\MTV.Scheduler.App"))
                {
                   
                    
                    if (registryKey != null)
                    {
                        registryKey.SetValue(settingName, settingValue);
                    }
                }
                
               
            }
            catch (Exception ex)
            {
                MainForm.LogErrorToFile(string.Format("Error set registry for setting: LastVideoContentScheduledOn - {0}", ex.Message));
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CheckRegistrySetting(string settingToCheck)
        {
            bool bFoundSettingScheduleOn = false;

            using (RegistryKey rkey = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\MTV.Scheduler.App", false))
                if (rkey != null)
                {
                    string val = string.Format("{0}", rkey.GetValue(settingToCheck));

                    if (!string.IsNullOrEmpty(val))
                    bFoundSettingScheduleOn = true;
                }

             return bFoundSettingScheduleOn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objNotification"></param>
        protected virtual void OnLastVideoContentLockedOnNotificationReceived(RealTimeEventMsg obj)
        {
            if (LastVideoContentLockedOnNotificationReceived != null)
            {
                LastVideoContentLockedOnNotificationReceived(this, new StreamingResultEventArgs(obj));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnLastVideoContentScheduledOnNotificationReceived(RealTimeEventMsg obj)
        {
            if (LastVideoContentScheduledOnNotificationReceived != null)
            {
                LastVideoContentScheduledOnNotificationReceived(this, new StreamingResultEventArgs(obj));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnLastVideoContentRemovedOnNotificationReceived(RealTimeEventMsg obj)
        {
            if (LastVideoContentRemovedOnNotificationReceived != null)
            {
                LastVideoContentRemovedOnNotificationReceived(this, new StreamingResultEventArgs(obj));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        protected virtual void OnLastCategoryItemsChangedOnNotificationReceived(RealTimeEventMsg obj)
        {
            if (LastCategoryItemsChangedOnNotificationReceived != null)
            {
                LastCategoryItemsChangedOnNotificationReceived(this, new StreamingResultEventArgs(obj));
            }
        }


    }
}
