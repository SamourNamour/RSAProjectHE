using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using MTV.Catalog.Service;
using System.Data.Services.Client;
using MTV.Library.Core.Tools;
using MTV.Library.Core.Common;
using MTV.Scheduler.App.MEBSCatalogServiceRef;
using MTV.Scheduler.App.UI;

namespace MTV.Library.Core.Services
{
    public class MEBSCatalogProvider
    {

        #region - CRUD data through  WCF Data Service client library -

        #region - mebs_channel | mebs_channeltuning -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelItem">ProcessTagInfo.Channel</param>
        //public static ChannelCreateStatus AddNewChannelEntity(ProcessTagInfo.Channel channelItem)
        public static ChannelCreateStatus AddNewChannelEntity(ProcessTagInfo.Channel channelItem)
        {
            try
            {
                //var svcURI = Configuration.MEBSCatalogLocation;
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var exists = default(Boolean);

                if (string.IsNullOrEmpty(channelItem._key))
                {
                    return ChannelCreateStatus.InvalidChannelKey;
                }

                if (string.IsNullOrEmpty(channelItem._id))
                {
                    return ChannelCreateStatus.InvalidChannelBus;
                }

                exists = proxy.mebs_channel.ToList().Where(mebsChannelParama => string.Compare(mebsChannelParama.ChannelKey, channelItem._key) == 0).Any<mebs_channel>();

                if (exists)
                {
                    
                    MainForm.LogWarningToFile(string.Format("Channel [ChannelKey = {0}] already exists in SA_MEBS database repository",
                                                                             channelItem._key));
                    return ChannelCreateStatus.ChannelRejected;
                }

                // Call factory method to create new entity instance :
                var mebsChannelItem =
                    mebs_channel.Createmebs_channel(default(int), default(DateTime));

                // Add mebs_channel : 
                mebsChannelItem.ChannelType = sbyte.Parse(channelItem._type);
                mebsChannelItem.DateCreation = DateTime.UtcNow;
                mebsChannelItem.LongName =
                mebsChannelItem.ShortName = channelItem._name;
                mebsChannelItem.Bus =
                mebsChannelItem.ChannelKey = channelItem._key;
                proxy.AddObject("mebs_channel", mebsChannelItem);
                proxy.SaveChanges();

                // Call factory method to create new entity instance :
                var mebsChannelTuningItem =
                    mebs_channeltuning.Createmebs_channeltuning(default(int), default(int), default(long), default(long), default(long));

                // Add mebs_channeltuning :
                mebsChannelTuningItem.IdChannel = mebsChannelItem.IdChannel;
                mebsChannelTuningItem.OriginalNetworkID = int.Parse(channelItem._onid);
                mebsChannelTuningItem.ServiceID = int.Parse(channelItem._id);
                mebsChannelTuningItem.TransportStreamID = int.Parse(channelItem._tsID);
                proxy.AddObject("mebs_channeltuning", mebsChannelTuningItem);

                proxy.SaveChanges();

                exists = proxy.mebs_channel.ToList().Where(mebsChannelParama => string.Compare(mebsChannelParama.ChannelKey, channelItem._key) == 0).Any<mebs_channel>();

                return (exists ?
                        ChannelCreateStatus.Success :
                        ChannelCreateStatus.OperationFailed);
            }
            catch (Exception ex)
            {


                if (ex.InnerException is DataServiceClientException)
                {
                    DataServiceErrorInfo innerException = DataServiceClientExceptionHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    if (innerException != null)
                    {
                        MainForm.LogErrorToFile(string.Format("AddNewChannelEntity SaveInfo error : {0}", innerException.Message));
                    }
                    else
                    {
                        MainForm.LogErrorToFile(string.Format("AddNewChannelEntity SaveInfo error : {0}", ex.InnerException.Message));
                       
                    }
                }
                else
                {
                    
                    MainForm.LogErrorToFile(string.Format("AddNewChannelEntity SaveInfo Global error : {0}", ex));
                }

                return ChannelCreateStatus.ProviderError;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static List<mebs_channeltuning> ChannelDVBTripletCollection(int channelId)
        {
            try
            {

                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var exists = default(Boolean);

                exists = proxy.mebs_channeltuning.Where(item => item.IdChannel == channelId).ToList().Any<mebs_channeltuning>();
                return (exists ?
                        proxy.mebs_channeltuning.Where(item => item.IdChannel == channelId).ToList() :
                        null);
            }
            catch (Exception ex)
            {
               
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        #endregion

        #region  - mebs_ingesta | mebs_ingestadetails | mebs_videoitem -

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelKey">string</param>
        /// <param name="eventItem">ProcessTagInfo.Programme</param>
        /// <returns>EpgCreateStatus</returns>
        public static EpgCreateStatus AddNewEpgEntityCascade(string channelKey,
                                                             ProcessTagInfo.Programme eventItem)
        {
            try
            {
                List<mebs_ingestadetails> mebsIngestaDetailsCollection = null;
                Guid eventGuid = (eventItem.EventMediaType == MediaType.TV_CHANNEL ?
                                  default(Guid) :
                                  Guid.NewGuid());

                
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var mebsChannelItem =
                    mebs_channel.Createmebs_channel(default(int), default(DateTime));
                var mebsChannelTuningItem =
                    mebs_channeltuning.Createmebs_channeltuning(default(int), default(int), default(long), default(long), default(long));
                var exists = default(Boolean);

                exists = proxy.mebs_channel.ToList().Where(mebsChannelParam => string.Compare(mebsChannelParam.ChannelKey, channelKey) == 0).Any<mebs_channel>();
               
                if (exists)
                {
                    mebsChannelItem = proxy.mebs_channel.ToList().Where(mebsChannelParama => string.Compare(mebsChannelParama.ChannelKey, channelKey) == 0).FirstOrDefault<mebs_channel>();
                    if (mebsChannelItem == null)
                    {
                        throw new ArgumentException(string.Format("Channel [ChannelKey = {0}] not found in SA_MEBS database repository",
                                                                   channelKey));
                    }
                    
                }

                var mebsIngestaItem =
                    mebs_ingesta.Createmebs_ingesta(default(int),
                                                                        default(int),
                                                                        default(int),
                                                                        default(int),
                                                                        default(Boolean),
                                                                        string.Empty,
                                                                        default(decimal));
                var mebsIngestaDetailsItem =
                    mebs_ingestadetails.Createmebs_ingestadetails(default(int), default(int));

                mebsIngestaItem.EventId = eventItem.ContentRef;
                mebsIngestaItem.Code_Package = eventGuid.ToString();
                mebsIngestaItem.Type = Convert.ToSByte(eventItem.EventMediaType);
                mebsIngestaItem.IdChannel = mebsChannelItem.IdChannel;
                mebsIngestaItem.Date_Creation = DateTime.UtcNow;
                mebsIngestaItem.Estimated_Start = DateTime.Parse(eventItem.StartTime);
                mebsIngestaItem.Estimated_Stop = mebsIngestaItem.Estimated_Start.Value.Add(TimeSpan.Parse(eventItem.Duration));
                mebsIngestaItem.Expiration_time = eventItem.ExpirationDate;
                mebsIngestaItem.Immortality_time = eventItem.ExpirationDate; //(DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6)).ConvertFrom(eventItem.ExpirationDate));//new DateTime(1971, 11, 6, 23, 59, 59);//default(DateTime); // ToDo
                mebsIngestaItem.Validity_time = eventItem.ValidityDate;
                mebsIngestaItem.AvailableAfter_time = default(int); // ToDo                
                mebsIngestaItem.Title = eventItem.Name;
                mebsIngestaItem.ParentalRating = eventItem.ParentalRating; // ToDo
                mebsIngestaItem.PosterFileExtension = eventItem.PosterFileName;
                mebsIngestaItem.Last_Update = default(DateTime); // ToDo
                mebsIngestaItem.IsExpired = default(bool); // ToDo                                
                mebsIngestaItem.Poster = eventItem.PosterImage;
                mebsIngestaItem.Duration = eventItem.Duration;
                mebsIngestaItem.IsPublished = default(Boolean); // ToDo
                mebsIngestaItem.XmlFileName = eventItem.XmlFileName;
                mebsIngestaItem.MinLifeAfterFirstAccess = default(int);
                mebsIngestaItem.LifeAfterFirstAccess = default(int);
                mebsIngestaItem.MinLifeAfterActivation = default(int);
                mebsIngestaItem.LifeAfterActivation = default(int);
                mebsIngestaItem.DisableAccess = default(bool); // ToDo
                mebsIngestaItem.ActiveSince = string.Empty; // ToDo
                mebsIngestaItem.ActiveDuring = default(int); // ToDo
                mebsIngestaItem.ActiveTimeAfterFirstAccess = default(int); // ToDo
                mebsIngestaItem.MinActiveTimeAfterFirstAccess = default(int); // ToDo
                mebsIngestaItem.DrmProtected = default(bool); // ToDo                
                mebsIngestaItem.MaxAccesses = default(int); // ToDo
                mebsIngestaItem.Hidden = default(bool); // ToDo
                mebsIngestaItem.PublishAfter = eventItem.PublishAfter;
                mebsIngestaItem.SelfCommercial = Convert.ToInt32(SelfCommercial.NotLinked);
                mebsIngestaItem.OriginalFileName = eventItem.MediaFileName;
                mebsIngestaItem.PreservationPriority = 0;
                proxy.AddTomebs_ingesta(mebsIngestaItem);

                if (eventItem.extendInfo != null &&
                    eventItem.extendInfo.Count > 0)
                {
                    mebsIngestaDetailsCollection = new List<mebs_ingestadetails>(eventItem.extendInfo.Count);
                    foreach (var key in eventItem.extendInfo.AllKeys)
                    {
                        var value = eventItem.extendInfo[key];
                        mebsIngestaDetailsCollection.Add(new mebs_ingestadetails
                        {
                            IdIngesta = mebsIngestaItem.IdIngesta,
                            DetailsName = key,
                            DetailsValue = value
                        }
                                                         );
                    }
                }

                switch (eventItem.EventMediaType)
                {
                    case MediaType.UNKNOWN_CHANNEL:
                        break;
                    case MediaType.TV_CHANNEL: //---- Disabled by Karima
                        //mebsIngestaItem.CopyControl = EnumUtils.GetDescriptionFromEnumValue(CopyControl.CopyOnce);
                        //foreach (ProcessTagInfo.PROGRAMME_ID item in eventItem.programIDs)
                        //{
                            
                        //    var mebsVideoItemItem = mebs_videoitem.Createmebs_videoitem(default(int),default(int));//, default(DateTime)
                        //    TimeSpan videoItemEstimatedStartTime;
                        //    bool wellParsedToTimeSpan = DateTimeUtils.TryParseFormattedTime(item.Start_tc, out videoItemEstimatedStartTime);
                        //    if (!wellParsedToTimeSpan)
                        //    {
                        //        return EpgCreateStatus.OperationFailed;
                        //    }
                        //    DateTime dt = mebsIngestaItem.Estimated_Start.Value;
                        //    dt = dt.Date + videoItemEstimatedStartTime;
                        //    mebsVideoItemItem.ItemValue = item.Value;
                        //    mebsVideoItemItem.Start_tc = Utils.NextTimeOfDayAfter(dt.TimeOfDay, DateTime.UtcNow, TriggerEntry.TriggerGracePeriod);
                        //    mebsIngestaItem.mebs_videoitem.Add(mebsVideoItemItem);
                        //    proxy.AddRelatedObject(mebsIngestaItem, "mebs_videoitem", mebsVideoItemItem);
                        //}
                        break;

                    case MediaType.DC_CHANNEL:
                        mebsIngestaItem.CopyControl = EnumUtils.GetDescriptionFromEnumValue(CopyControl.Allow);
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

                foreach (var occurrence in mebsIngestaDetailsCollection)
                {
                    mebsIngestaItem.mebs_ingestadetails.Add(occurrence);
                    proxy.AddRelatedObject(mebsIngestaItem, "mebs_ingestadetails", occurrence);
                }

                proxy.SaveChanges(SaveChangesOptions.Batch);

                return EpgCreateStatus.Success;
            }
            catch (Exception ex)
            {

                if (ex.InnerException is DataServiceClientException)
                {
                    DataServiceErrorInfo innerException = DataServiceClientExceptionHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    if (innerException != null)
                    {
                        
                        MainForm.LogErrorToFile(string.Format("AddNewEpgEntityCascade SaveInfo error : {0}", innerException.Message));
                    }
                    else
                    {
                        
                        MainForm.LogErrorToFile(string.Format("AddNewEpgEntityCascade SaveInfo error : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                   
                    MainForm.LogErrorToFile(string.Format("AddNewEpgEntityCascade SaveInfo Global error : {0}", ex));
                }

                return EpgCreateStatus.ProviderError;


            }
        }

        
        /// <summary>
        /// Disabled by Karima
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        //public static string GetNextEpgCloserVideoItem(EpgEntry item)
        //{
        //    string nextEpgCloserVideoItem = string.Empty;
        //    try
        //    {
        //        //var svcURI = Configuration.MEBSCatalogLocation;
        //        var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
        //        bool isExistsNextEpg = proxy.mebs_ingesta.Expand("mebs_videoitem,mebs_channel")
        //                                     .AddQueryOption("$filter", string.Format("Estimated_Start gt DateTime'{0}' and mebs_channel/Bus eq '{1}'",
        //                                                                               DateTimeUtils.ConvertDateTimeToEDMFormat(item.EstimatedStartDateTime),
        //                                                                               item.Bus))
        //                                     .AddQueryOption("$orderby", "Estimated_Start")
        //                                     .ToList().Any();

                
        //        if (isExistsNextEpg)
        //        {
        //            nextEpgCloserVideoItem = proxy.mebs_ingesta.Expand("mebs_videoitem,mebs_channel")
        //                                          .AddQueryOption("$filter", string.Format("Estimated_Start gt DateTime'{0}' and mebs_channel/Bus eq '{1}'",
        //                                                                                    DateTimeUtils.ConvertDateTimeToEDMFormat(item.EstimatedStartDateTime), item.Bus))
        //                                          .AddQueryOption("$orderby", "Estimated_Start")
        //                                          .First().mebs_videoitem.OrderBy(p => p.Start_tc).First().ItemValue;
        //        }
        //        return nextEpgCloserVideoItem;
        //    }
        //    catch (Exception ex)
        //    {
        //        MainForm.LogExceptionToFile(ex);
        //        return null;
        //    }
        //}
        #endregion

        #region mebs_category -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static mebs_category GetCategoryByID(int categoryID)
        {
            try
            {
                //var svcURI = Configuration.MEBSCatalogLocation;
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var exists = default(Boolean);

                exists = proxy.mebs_category.Where(item => item.IdCategory == categoryID).ToList().Any<mebs_category>();
                return (exists ?
                        proxy.mebs_category.Where(item => item.IdCategory == categoryID).First<mebs_category>() :
                        null);
            }
            catch (Exception ex)
            {
                //DefaultLogger.STARLogger.Error(ex);
                return null;
            }
        }
        #endregion

        #region - mebs_schedule -

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSchedule"></param>
        /// <param name="exactStartTime"></param>
        /// <param name="operationStatus"></param>
        /// <param name="triggerType"></param>
        public static void EditSchedule(int idSchedule,
                                        DateTime exactStartTime,
                                        ScheduleStatus operationStatus,
                                        bool triggerType)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var scheduleEntity = proxy.mebs_schedule.Where(sch => sch.IdSchedule == idSchedule).First();
                if (scheduleEntity != null)
                {
                    scheduleEntity.Exact_Start = exactStartTime;
                    scheduleEntity.Status = Convert.ToInt32(operationStatus);
                    scheduleEntity.Trigger_Type = Convert.ToSByte(triggerType);
                    proxy.UpdateObject(scheduleEntity);
                    proxy.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSchedule"></param>
        /// <param name="exactStartTime"></param>
        public static void EditScheduleExactStartTime(int idSchedule, DateTime exactStartTime)
        {
            try
            {
                
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var scheduleEntity = proxy.mebs_schedule.Where(sch => sch.IdSchedule == idSchedule).First();
                if (scheduleEntity != null)
                {
                    scheduleEntity.Exact_Start = exactStartTime;
                    proxy.UpdateObject(scheduleEntity);
                    proxy.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="idSchedule"></param>
       /// <param name="exactStopTime">Stop exact time</param>
       /// <param name="operationStatus"></param>
        public static void EditScheduleExactStopTime(int idSchedule,
                                                     DateTime exactStopTime,
                                                     ScheduleStatus operationStatus)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var scheduleEntity = proxy.mebs_schedule.Where(sch => sch.IdSchedule == idSchedule).First();
                if (scheduleEntity != null)
                {
                    scheduleEntity.Exact_Stop = exactStopTime;
                    scheduleEntity.Status = Convert.ToInt32(operationStatus);
                    proxy.UpdateObject(scheduleEntity);
                    proxy.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public static mebs_schedule GetScheduleEntityTreeByScheduleID(int ID)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var uriString = string.Format(Configuration.GetScheduleEntityTreeByScheduleID, ID);
                var operationUri = new Uri(uriString, UriKind.Relative);
                return (proxy.Execute<mebs_schedule>(operationUri).Any() ?
                        proxy.Execute<mebs_schedule>(operationUri).ToList().First() :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryIteme"></param>
        public static void SetSchedulePosterTransferMetadata(EpgEntry epgEntryIteme)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var scheduleEntity = proxy.mebs_schedule.Where(sch => sch.IdSchedule == int.Parse(epgEntryIteme.ID)).First();
                if (scheduleEntity != null)
                {
                    scheduleEntity.Poster_DateSent = epgEntryIteme.PosterTransferDateTime;
                    scheduleEntity.Poster_Status = Convert.ToInt32(epgEntryIteme.PosterTransferStatus);
                    proxy.UpdateObject(scheduleEntity);
                    proxy.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDSchedule"></param>
        /// <returns></returns>
        public static bool AttemptAcquireScheduleLock(int IDSchedule)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var uriString = string.Format(Configuration.AttemptToAcquireScheduleLock, IDSchedule);
                var operationUri = new Uri(uriString, UriKind.Relative);
                bool res = proxy.Execute<bool>(operationUri).Single();
                return res;
            }
            catch (Exception ex)
            {
               
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDSchedule"></param>
        /// <returns></returns>
        public static bool AttemptAcquireDummyCommandLock(int IDSchedule)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var uriString = string.Format(Configuration.AttemptAcquireDummyCommandLock, IDSchedule);
                var operationUri = new Uri(uriString, UriKind.Relative);
                return proxy.Execute<bool>(operationUri).Single();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idSchedule"></param>
        /// <param name="exactStartTime"></param>
        public static void UpdateDummyCommandStatus(int idSchedule,
                                        EpgDummyCommandStatus dummyStatus)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var scheduleEntity = proxy.mebs_schedule.Where(sch => sch.IdSchedule == idSchedule).First();
                if (scheduleEntity != null)
                {
                    scheduleEntity.Dummy_Status = Convert.ToInt32(dummyStatus);
                    proxy.UpdateObject(scheduleEntity);
                    proxy.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        public static bool AttemptAcquirePosterLock(EpgEntry item)
        {
            try
            {
                if (item == null) return false; // item is null ??
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var uriString = string.Format(Configuration.AttemptAcquirePosterLock, int.Parse(item.ID));
                var operationUri = new Uri(uriString, UriKind.Relative);
                return proxy.Execute<bool>(operationUri).Single();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }
        #endregion

        #region -- mebs_settings -
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetSettingByKeyName(string key)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                mebs_settings item = proxy.mebs_settings.Where(occurrence => string.Compare(occurrence.SettingName, key) == 0).FirstOrDefault();

                return (item != null ?
                        item.SettingValue :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }
        #endregion

        #region -- mebs_mediafile --
        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelItem">ProcessTagInfo.Channel</param>
        public static MediaFileCreateStatus AddNewMediaFileEntity(string originalFileName,
                                                                  long fileSize,
                                                                  long fileSizeAfterRedundancy,
                                                                  int redundancyStatus,
                                                                  string RedundancyFileName)
        {
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                var exists = default(Boolean);

                if (string.IsNullOrEmpty(originalFileName))
                {
                    return MediaFileCreateStatus.InvalidFileName;
                }

                if (fileSize == default(decimal))
                {
                    return MediaFileCreateStatus.InvalidFileSize;
                }

                if (fileSizeAfterRedundancy == default(decimal))
                {
                    return MediaFileCreateStatus.InvalidFileSize;
                }

                exists = proxy.mebs_mediafile.ToList().Where(mediafileParama => string.Compare(mediafileParama.OriginalFileName, originalFileName) == 0).Any<mebs_mediafile>();

                if (exists)
                {
                    
                    MainForm.LogWarningToFile(string.Format("AddNewMediaFileEntity SaveInfo [OriginalFileName = {0}] already exists in MEBS database repository",
                                                                 originalFileName));
                    return MediaFileCreateStatus.MediaFileRejected;
                }

                // Call factory method to create new entity instance :
                var mebsMediafileItem =
                    mebs_mediafile.Createmebs_mediafile(default(long),
                                                        Guid.NewGuid().ToString(),
                                                        string.Empty,
                                                        default(decimal),
                                                        default(decimal),
                                                        DateTime.UtcNow,
                                                        default(long),
                                                        string.Empty);

                // Add mebs_mediafile : 
                mebsMediafileItem.OriginalFileName = originalFileName;
                mebsMediafileItem.FileSize = fileSize;
                mebsMediafileItem.FileSizeAfterRedundancy = fileSizeAfterRedundancy;
                mebsMediafileItem.RedundancyStatus = redundancyStatus;
                mebsMediafileItem.RedundancyFileName = RedundancyFileName;

                proxy.AddObject("mebs_mediafile", mebsMediafileItem);
                proxy.SaveChanges();

                exists = proxy.mebs_mediafile.ToList().Where(mediafileParama => string.Compare(mediafileParama.OriginalFileName, originalFileName) == 0).Any<mebs_mediafile>();

                return (exists ?
                        MediaFileCreateStatus.Success :
                        MediaFileCreateStatus.OperationFailed);
            }
            catch (Exception ex)
            {

                if (ex.InnerException is DataServiceClientException)
                {
                    DataServiceErrorInfo innerException = DataServiceClientExceptionHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    if (innerException != null)
                    {
                       
                        MainForm.LogErrorToFile(string.Format("AddNewMediaFileEntity SaveInfo error : {0}", innerException.Message));

                    }
                    else
                    {
                       
                        MainForm.LogErrorToFile(string.Format("AddNewMediaFileEntity SaveInfo error : {0}", ex.InnerException.Message));
                       
                    }
                }
                else
                {
                    
                    MainForm.LogErrorToFile(string.Format("AddNewMediaFileEntity SaveInfo Global error : {0}", ex));
                }

                return MediaFileCreateStatus.ProviderError;
            }
        }
        #endregion

        #endregion


    }
}
