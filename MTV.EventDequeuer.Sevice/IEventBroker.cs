using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using MTV.EventDequeuer.Contracts.Service;
using MTV.EventDequeuer.Service.Data;
using System.Threading.Tasks;
using System.Configuration;
using MTV.EventDequeuer.Contracts.Faults;
using System.Messaging;
using MTV.EventDequeuer.Common;
using MTV.EventDequeuer.Contracts.Data;
using System.Threading;
using MTV.EventDequeuer.Service.Services.Contracts;
using MTV.EventDequeuer.Service.Catalog;
using System.Globalization;


namespace MTV.EventDequeuer.Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class EventDequeuer : IEventDequeuer
    {
        Dictionary<string, List<UniqueCallbackHandle>> eventNameToCallbackLookups = new Dictionary<string, List<UniqueCallbackHandle>>();
        private static Object syncObj = new Object();
        private bool shouldRun = true;
        private IXmlParser xmlParser = null;
       
        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EventDequeuer()
        {
           
            StartCollectingMessages();
            xmlParser = IOCManager.Instance.Container.Resolve<IXmlParser>();
           

        }

        #region public Methods
       /// <summary>
       /// 
       /// </summary>
        public void StartCollectingMessages()
        {
            try
            {
                GetMessageFromCatalog();
            }
            catch (Exception ex)
            {
                throw new FaultException<EventDequeuerException>(new EventDequeuerException(ex.Message), new FaultReason(ex.Message));
            }
        }
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="eventNames"></param>
        public void Subscribe(Guid subscriptionId, string[] eventNames)
        {
            try
            {
                CreateSubscription(subscriptionId, eventNames);
            }
            catch (Exception ex)
            {
                throw new FaultException<EventDequeuerException>(new EventDequeuerException(ex.Message), new FaultReason(ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId"></param>
        public void EndSubscription(Guid subscriptionId)
        {
            lock (syncObj)
            {
                //create new dictionary that will be populated by those remaining
                Dictionary<string, List<UniqueCallbackHandle>> remainingEventNameToCallbackLookups =
                    new Dictionary<string, List<UniqueCallbackHandle>>();

                foreach (KeyValuePair<string, List<UniqueCallbackHandle>> kvp in eventNameToCallbackLookups)
                {
                    //get all the remaining subscribers whos session id is not the same as the one we wish to remove
                    List<UniqueCallbackHandle> remainingMessageSubscriptions =
                        kvp.Value.Where(x => x.CallbackSessionId != subscriptionId).ToList();
                    if (remainingMessageSubscriptions.Any())
                    {
                        remainingEventNameToCallbackLookups.Add(kvp.Key, remainingMessageSubscriptions);
                    }
                }
                //now left with only the subscribers that are subscribed
                eventNameToCallbackLookups = remainingEventNameToCallbackLookups;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        private void GetMessageFromCatalog()
        {

            try
            {
                Task messageCatalogReaderTask = Task.Factory.StartNew(() =>
                {

                    while (shouldRun)
                    {
                        try
                        {
                            string sLastVideoContentLockedOn = null;
                            string sLastVideoContentScheduledOn = null;
                            string sLastVideoContentRemovedOn = null;
                            string sLastCategoryItemsChangedOn = null;

                            // Lock Notification.
                            sLastVideoContentLockedOn = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>LastVideoContentLockedOn</eventName>" +
                            "<date>{0}</date>" +
                            "</realtimeEvent>", Select("LastNotificationDate_Lock").SettingValue);

                            // Insert Notification. 
                            sLastVideoContentScheduledOn = string.Format(
                           "<realtimeEvent>" +
                           "<eventName>LastVideoContentScheduledOn</eventName>" +
                           "<date>{0}</date>" +
                           "</realtimeEvent>", Select("LastVideoContentScheduledOn").SettingValue);

                            // Remove Notification. 
                            sLastVideoContentRemovedOn = string.Format(
                            "<realtimeEvent>" +
                            "<eventName>LastVideoContentRemovedOn</eventName>" +
                            "<date>{0}</date>" +
                            "</realtimeEvent>", Select("LastVideoContentRemovedOn").SettingValue);

                            // Publish All Messages (LastVideoContentLockedOn , LastVideoContentScheduledOn , LastVideoContentRemovedOn).
                            ProcessMessage(sLastVideoContentLockedOn);
                            ProcessMessage(sLastVideoContentScheduledOn);
                            ProcessMessage(sLastVideoContentRemovedOn);



                            // Publish LastCategoryItemsChangedOn only if LastCategoryItemsChangedOn setting != NULL
                            string sCategoryChangedOn = Select("LastCategoryItemsChangedOn").SettingValue;
                            
                            if (!string.IsNullOrEmpty(sCategoryChangedOn))
                            {
                                DateTime dtCategoryChangedOn = DateTime.Parse(sCategoryChangedOn);
                                IFormatProvider mmddFormat = new CultureInfo(String.Empty, false);
                                sCategoryChangedOn = dtCategoryChangedOn.ToString(GetDateTimeString(), mmddFormat);
                                
                                // Build the message
                                sLastCategoryItemsChangedOn = string.Format(
                                 "<realtimeEvent>" +
                                 "<eventName>LastCategoryItemsChangedOn</eventName>" +
                                 "<date>{0}</date>" +
                                 "</realtimeEvent>", sCategoryChangedOn);
                                //Process the message
                                ProcessMessage(sLastCategoryItemsChangedOn);

                            }
                          

                            
                            LockSchedulesIfAny();
                            
                            ExpireContentsIdAny();

                            //Sleep 10 sec
                            Thread.Sleep(10000);


                        }
                        catch (Exception e)
                        {
                            // Write the message details to the Error Manager
                            LogManager.Log.Warn("Exception occured:", e);
                        }

                    }
                    
                    
                }, TaskCreationOptions.LongRunning);
            }
            catch (AggregateException ex)
            {
                throw;
            }
            
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="Msg"></param>
        private void ProcessMessage(string Msg)
        {
            
            LogManager.Log.DebugFormat("ProcessMessage : {0}", Msg);

            RealTimeEventMsg messageToSendToSubscribers = xmlParser.ParseXmlMsg(Msg);


            if (messageToSendToSubscribers != null)
            {
                lock (syncObj)
                {
                    List<Guid> deadSubscribers = new List<Guid>();

                    if (eventNameToCallbackLookups.ContainsKey(messageToSendToSubscribers.EventName))
                    {
                        List<UniqueCallbackHandle> uniqueCallbackHandles =
                            eventNameToCallbackLookups[messageToSendToSubscribers.EventName];
                        foreach (UniqueCallbackHandle uniqueCallbackHandle in uniqueCallbackHandles)
                        {
                            try
                            {
                                uniqueCallbackHandle.Callback.ReceiveStreamingResult(messageToSendToSubscribers);

                            }
                            catch (CommunicationObjectAbortedException coaex)
                            {
                                deadSubscribers.Add(uniqueCallbackHandle.CallbackSessionId);
                            }
                        }
                    }

                    //end all subcriptions 
                    foreach (Guid deadSubscriberId in deadSubscribers)
                    {
                        EndSubscription(deadSubscriberId);
                    }
                }
            }



        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="subscriptionId"></param>
        /// <param name="eventNames"></param>
        private void CreateSubscription(Guid subscriptionId, string[] eventNames)
        {

            //Ensure that a subscription is created for each message type the subscriber wants to receive
            lock (syncObj)
            {
                foreach (string eventName in eventNames)
                {
                    if (!eventNameToCallbackLookups.ContainsKey(eventName))
                    {
                        List<UniqueCallbackHandle> currentCallbacks = new List<UniqueCallbackHandle>();
                        eventNameToCallbackLookups[eventName] = currentCallbacks;
                    }
                    eventNameToCallbackLookups[eventName].Add(
                        new UniqueCallbackHandle(subscriptionId, OperationContext.Current.GetCallbackChannel<IEventDequeuerCallback>()));
                }
            }
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="key">Setting Name</param>
        /// <returns></returns>
        private mebs_settings Select(string key)
        {
            
            
            try
            {
                mebsEntities mEbsEntities = new mebsEntities();
                bool isExists = mEbsEntities.mebs_settings.ToList().Where(n => n.SettingName.CompareTo(key) == 0).Any();
                return (isExists ?
                        mEbsEntities.mebs_settings.ToList().Where(n => n.SettingName.CompareTo(key) == 0).First() :
                        null);
            }
            catch (Exception ex)
            {
              
                LogManager.Log.ErrorFormat("Setting Retrieve Exception: {0}", ex.ToString()); 
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void LockSchedulesIfAny()
        {
            try
            {
                using (mebsEntities mEbsEntities = new mebsEntities())
                {
                    mEbsEntities.ScheduleSetLockValue();
                }
            }
            catch (Exception ex)
            {
                LogManager.Log.ErrorFormat(ex.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ExpireContentsIdAny()
        {
            try
            {
                using (mebsEntities mEbsEntities = new mebsEntities())
                {
                    mEbsEntities.IngestaSetIsExpiredValue();
                }
            }
            catch (Exception ex)
            {
                LogManager.Log.ErrorFormat(ex.ToString());
            }
        }

        /// <summary>
        ///  Format to mysql datetime
        /// </summary>
        /// <returns></returns>
        private string GetDateTimeString()
        {
            return "yyyy-MM-dd HH:mm:ss";
        }
        #endregion

    }
}
