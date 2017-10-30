using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;
using System.Data.Services.Common;
using System.ServiceModel.Web;
using System.Linq.Expressions;
using MTV.Catalog.Service;
using System.Data.Objects;
using System.Data;

namespace MTV.Catalog.Host
{
    /// <summary>
    /// 
    /// </summary>
    public enum ScheduleStatus
    {
        /// <summary>
        /// Event Published.
        /// </summary>
        PREPARED = 1000,

        /// <summary>
        /// Event Started Successfully.
        /// </summary>
        STARTED = 1001,

        /// <summary>
        /// Event Stoped Successfully.
        /// </summary>
        STOPPED = 1002,

        /// <summary>
        /// Trigger Start not received.
        /// </summary>
        MISSING_START = 1003,

        /// <summary>
        /// Trigger Stop not received.
        /// </summary>
        MISSING_STOP = 1004,

        /// <summary>
        /// Error occured during sending Start command to encapsulator.
        /// </summary>
        FAILED_START = 1005,

        /// <summary>
        /// Error occured during sending Stop command to encapsulator.
        /// </summary>
        FAILED_STOP = 1006,

        /// <summary>
        /// Event Locked X (Configurable) minutes before the Estimated start.
        /// </summary>
        LOCKED = 1007,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_OWNED = 1008,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_UNOWNED = 1009,

        /// <summary>
        /// 
        /// </summary>
        TOKEN_ATTEMPT = 1010,
    };

    
    public class MEBSCatalog : DataService<mebsEntities>
    {
        public static void InitializeService(DataServiceConfiguration config)
        {
           
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("GetVODChannel", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetServices", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("GetIngestedEventsByType", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetOnairNow", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetPrograms", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetProgramsByStartTime", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetProgramsByStartTimeAndStopTime", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetProgramExists", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetProgramsByTitle", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetProgramsToSchedule", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("GetAllSchedules", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetSchedule", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetShedulesByFilters", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("GetPackagesByDateCreation", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetPackagesByTitle", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetPackageByCode", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetPackagesToSchedule", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("GetUserByName", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetUserByNameAndPassword", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetAllUsers", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetSessionByGuid", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetUserSession", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetListScheduleByDate", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("GetMixedCategoryElements", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetAvailableCategoryElementsToBeMixed", ServiceOperationRights.AllRead);

            config.SetServiceOperationAccessRule("SearchSettingsByName", ServiceOperationRights.AllRead);
            config.SetServiceOperationAccessRule("GetRedundancyParameters", ServiceOperationRights.AllRead);


            config.SetServiceOperationAccessRule("AcquireScheduleLock", ServiceOperationRights.AllRead);

            config.DataServiceBehavior.MaxProtocolVersion = DataServiceProtocolVersion.V2;
            config.UseVerboseErrors = true; 
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="IDSchedule"></param>
        /// <returns></returns>
        [WebGet]
        public bool AcquireScheduleLock(int IDSchedule)
        {

            var scheduleEvent = this.CurrentDataSource.mebs_schedule.ToList().Where(item => item.IdSchedule == IDSchedule).FirstOrDefault();

            if (scheduleEvent == null)
            {
                throw new DataServiceException(105,
                                               string.Format("No Schedule event found associated to the giving Schedule ID = {0}", IDSchedule));
            }

            if (scheduleEvent.Status != Convert.ToInt32(ScheduleStatus.LOCKED))
            {
                return false;
            }

            if (scheduleEvent.Status == Convert.ToInt32(ScheduleStatus.TOKEN_OWNED))
            {
                return false;
            }

            scheduleEvent.Status = Convert.ToInt32(ScheduleStatus.TOKEN_OWNED);
            this.CurrentDataSource.SaveChanges();

            scheduleEvent = this.CurrentDataSource.mebs_schedule.ToList().Where(item => item.IdSchedule == IDSchedule).FirstOrDefault();

            return (scheduleEvent.Status == Convert.ToInt32(ScheduleStatus.TOKEN_OWNED) ?
                    true :
                    false);
        }

        // On définit un intercepteur pour la collection mebs_channel.
        //[QueryInterceptor("mebs_channel")]
        //public Expression<Func<mebs_channel, bool>> OnQueryChannels()
        //{
        //    // on ne renvoie une Chaine que si elle a au moins un program attaché
        //    //return c => c.mebs_ingesta.Any();
        //    //return c => c.mebs_channeltuning.Count() > 0;
        //}


        // On définit un intercepteur pour la collection mebs_ingesta.
        [QueryInterceptor("mebs_ingesta")]
        public Expression<Func<mebs_ingesta, bool>> OnQueryIngesta()
        {
            // on ne renvoie un program que si elle a non expiré
            //return p => p.IsExpired == false;
            return p => p.mebs_channel != null;
        }

        [QueryInterceptor("mebs_schedule")]
        public Expression<Func<mebs_schedule, bool>> OnQuerySchedule()
        {
            // on ne renvoie un program que si elle a non expiré
            //return p => p.IsExpired == false;
            return s => s.mebs_ingesta != null;
        }

        // On définit un change interceptor pour la collection mebs_ingesta.
        [ChangeInterceptor("mebs_ingesta")]
        public void OnChangeIngesta(mebs_ingesta ingesta, UpdateOperations operations)
        {
            #region -.-.-.-.-.-.- Overlapping -.-.-.-.-.-.-
            //--- Vérifier le Overlapping d'une nouvelle entrie.
            if (operations == UpdateOperations.Add)
            {
                //--- Get La liste de tout les AR de la même channel que le nouveau event (ingesta).
                List<mebs_ingesta> ListOverlappingIngst = this.CurrentDataSource.mebs_ingesta.Where
                                                                                             (x => (x.IdChannel == ingesta.IdChannel) &&
                                                                                                   (x.IsPublished == false) &&
                                                                                                   (x.Estimated_Start >= ingesta.Estimated_Start && x.Estimated_Start < ingesta.Estimated_Stop) ||
                                                                                                   (ingesta.Estimated_Start >= x.Estimated_Start && ingesta.Estimated_Start < x.Estimated_Stop)).ToList();
                if (ListOverlappingIngst != null && ListOverlappingIngst.Count > 0)
                {
                    //--- Supprimer l'ancienne entrée et inserer la nouvelle.
                    foreach (mebs_ingesta item in ListOverlappingIngst)
                    {
                        this.CurrentDataSource.DeleteObject(item);
                        this.CurrentDataSource.SaveChanges(SaveOptions.AcceptAllChangesAfterSave);
                    }

                    //mebs_settings ObjSetting = this.CurrentDataSource.mebs_settings.Where(s => s.SettingName == "LastNotificationDate_Insert").FirstOrDefault();
                    //if (ObjSetting != null)
                    //{
                    //    ObjSetting.SettingValue = DateTime.UtcNow.ToString();
                    //    this.CurrentDataSource.SaveChanges();
                    //}

                }


                mebs_ingesta ObjIngesta = this.CurrentDataSource.mebs_ingesta.Where(x => (x.Type == 1 && x.EventId == ingesta.EventId)).FirstOrDefault();
                if (ObjIngesta != null)
                {
                    throw new DataServiceException(107, "PRODUCT_ALREADY_EXIST");
                }
            }
            #endregion

            if ((operations & UpdateOperations.Change) == UpdateOperations.Change)
            {
                ObjectStateEntry entry;
                if (this.CurrentDataSource.ObjectStateManager.TryGetObjectStateEntry(ingesta, out entry))
                {
                    if (entry.State == EntityState.Detached && (bool)entry.OriginalValues["IsExpired"])
                    {
                        throw new DataServiceException(102,
                                    "A Expired Event , cannot be modified.");
                    }
                }
            }

        }

        // On définit un change interceptor pour la collection mebs_schedule.
        [ChangeInterceptor("mebs_schedule")]
        public void OnChangeSchedule(mebs_schedule schedule, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Add)
            {
                mebs_ingesta currentIngesta = this.CurrentDataSource.mebs_ingesta.Where(x => x.IdIngesta == schedule.IdIngesta && x.Type != 6).FirstOrDefault();
                if (currentIngesta != null && string.IsNullOrEmpty(currentIngesta.Title))
                {
                    throw new DataServiceException(103,
                     "VALIDATION_ERROR_TITLE_NOT_FOUND");
                }


                if (currentIngesta != null && (currentIngesta.Poster == null || currentIngesta.Poster.Length <= 0))
                {
                    throw new DataServiceException(104,
                     "VALIDATION_ERROR_POSTER_NOT_FOUND");
                }

                if (currentIngesta != null)
                {
                    List<mebs_ingesta_category_mapping> listCategoryMapping = this.CurrentDataSource.mebs_ingesta_category_mapping.Where(x => x.IdIngesta == schedule.IdIngesta).ToList();
                    if (listCategoryMapping == null || listCategoryMapping.Count <= 0)
                    {
                        throw new DataServiceException(105,
                        "VALIDATION_ERROR_CATEGORY_NOT_FOUND");
                    }
                }

                if (currentIngesta != null && currentIngesta.IsPublished.Value && currentIngesta.Type == 0)
                {
                    throw new DataServiceException(106,
                    "EVENT_ALREADY_PUBLISHED");
                }
            }
        }

        // On définit un change interceptor pour la collection mebs_login.
        [ChangeInterceptor("mebs_login")]
        public void OnChangeLogin(mebs_login login, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Add)
            {
                mebs_login ObjLogin = this.CurrentDataSource.mebs_login.Where(x => x.Login.ToLower() == login.Login.ToLower()).FirstOrDefault();

                if (ObjLogin != null)
                {
                    throw new DataServiceException(108,
                     "VALIDATION_ERROR_LOGIN_ALREADY_EXIST");
                }
            }

            if (operations == UpdateOperations.Change)
            {

                System.Data.Objects.ObjectStateEntry entry;
                if (this.CurrentDataSource.ObjectStateManager.TryGetObjectStateEntry(login, out entry))
                {
                    if (entry.State == EntityState.Detached && entry.OriginalValues["Login"].ToString().ToLower() != login.Login.ToLower())
                    {
                        mebs_login ObjLogin = this.CurrentDataSource.mebs_login.Where(x => x.Login.ToLower() == login.Login.ToLower()).FirstOrDefault();
                        if (ObjLogin != null)
                        {
                            throw new DataServiceException(108,
                             "VALIDATION_ERROR_LOGIN_ALREADY_EXIST");
                        }
                    }
                }
            }
        }

        // On définit un change interceptor pour la collection mebs_mediafile.
        [ChangeInterceptor("mebs_mediafile")]
        public void OnChangeMediaFile(mebs_mediafile mediafile, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Add)
            {
                mebs_mediafile ObjMediaFile = this.CurrentDataSource.mebs_mediafile.Where(x => x.OriginalFileName.ToLower() == mediafile.OriginalFileName.ToLower()).FirstOrDefault();

                if (ObjMediaFile != null)
                {
                    throw new DataServiceException(109,
                     "VALIDATION_ERROR_ORIGINALFILENAME_ALREADY_EXIST");
                }
            }
        }

        // On définit un change interceptor pour la collection mebs_channel.
        [ChangeInterceptor("mebs_channel")]
        public void OnChangeChannel(mebs_channel channel, UpdateOperations operations)
        {
            if (operations == UpdateOperations.Add)
            {
                mebs_channel objChannel = this.CurrentDataSource.mebs_channel.Where(x => x.ShortName.ToLower() == channel.ShortName.ToLower()).FirstOrDefault();
                if (objChannel != null)
                {
                    throw new DataServiceException(110,
                        "A Channel with the same iDTV3 Interface Name already exists in MEBS database repository");
                }
            }

            if (operations == UpdateOperations.Change)
            {
                System.Data.Objects.ObjectStateEntry entry;
                if (this.CurrentDataSource.ObjectStateManager.TryGetObjectStateEntry(channel, out entry))
                {
                    if (entry.State == EntityState.Detached && entry.OriginalValues["ShortName"].ToString().ToLower() != channel.ShortName.ToLower())
                    {
                        throw new DataServiceException(110,
                            "A Channel with the same iDTV3 Interface Name already exists in MEBS database repository");
                    }
                }
                //else
                //{
                //    throw new DataServiceException(101,
                //        "The requested Event could not be found in the data source.");
                //}  
            }
        }

        #region ===================== mebs_channel =====================
        [WebGet]
        public IQueryable<mebs_channel> GetVODChannel()
        {


            try
            {

                var ars = from mebs_channel in this.CurrentDataSource.mebs_channel
                          where mebs_channel.ChannelType == 1
                          && mebs_channel.Enabled.Value == true
                          orderby mebs_channel.LongName descending
                          select mebs_channel;

                return ars;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_channel> GetServices()
        {
            try
            {
                // change to support Any()
                var ars = from mebs_channel in this.CurrentDataSource.mebs_channel
                          where mebs_channel.ChannelType == 0
                          && mebs_channel.Enabled.Value == true
                          orderby mebs_channel.LongName descending
                          select mebs_channel;

                return ars;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }
        #endregion

        #region ===================== mebs_ingesta =====================
        [WebGet]
        public IQueryable<mebs_ingesta> GetIngestedEventsByType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                throw new ArgumentNullException("type of media",
                    "You must provide a value for the parameter'type'.");
            }


            int mediatype = ConvertToInt32(type, 0);


            if (mediatype != 0 && mediatype != 1)
            {
                throw new ArgumentException("Bad value for the parameter 'type'.", "type");
            }


            try
            {

                var ars = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where mebs_ingesta.Type == mediatype
                          && mebs_ingesta.IsExpired == false
                          orderby mebs_ingesta.Date_Creation descending
                          select mebs_ingesta;

                return ars;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetOnairNow()
        {

            try
            {
                DateTime dt = DateTime.Now.ToUniversalTime();
                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where mebs_ingesta.Estimated_Start <= dt && mebs_ingesta.Estimated_Stop >= dt && mebs_ingesta.Type == 0 && mebs_ingesta.IsExpired == false
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetPrograms(string channelID)
        {


            if (string.IsNullOrEmpty(channelID))
            {
                throw new ArgumentNullException("Channel ID",
                    "You must provide a value for the parameter'Channel ID'.");
            }


            int id = ConvertToInt32(channelID, 0);

            if (id == 0)
            {
                throw new ArgumentNullException("Channel ID",
                    "Bad value for the parameter for the parameter'Channel ID'.");
            }

            try
            {
                DateTime dt = DateTime.Now.ToUniversalTime();

                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where mebs_ingesta.Estimated_Start >= dt && mebs_ingesta.IdChannel == id && mebs_ingesta.Type == 0 && mebs_ingesta.IsExpired == false
                          orderby mebs_ingesta.Estimated_Start descending
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetProgramsByStartTime(string channelID, string startTime)
        {

            if (string.IsNullOrEmpty(channelID))
            {
                throw new ArgumentNullException("Channel ID",
                    "You must provide a value for the parameter'Channel ID'.");
            }


            int id = ConvertToInt32(channelID, 0);


            if (id == 0)
            {
                throw new ArgumentNullException("Channel ID",
                    "Bad value for the parameter for the parameter'Channel ID'.");
            }


            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }

            try
            {

                DateTime dt = DateTime.Parse(startTime);

                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where mebs_ingesta.Estimated_Start >= dt && mebs_ingesta.IdChannel == id && mebs_ingesta.Type == 0 && mebs_ingesta.IsPublished == false && mebs_ingesta.IsExpired == false
                          orderby mebs_ingesta.Estimated_Start descending
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetProgramsByStartTimeAndStopTime(string channelID, string startTime, string stopTime)
        {


            if (string.IsNullOrEmpty(channelID))
            {
                throw new ArgumentNullException("Channel ID",
                    "You must provide a value for the parameter'Channel ID'.");
            }


            int id = ConvertToInt32(channelID, 0);


            if (id == 0)
            {
                throw new ArgumentNullException("Channel ID",
                    "Bad value for the parameter for the parameter'Channel ID'.");
            }


            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }

            try
            {

                DateTime dtStart = DateTime.Parse(startTime);

                DateTime dtStop = DateTime.Parse(stopTime);

                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where ((mebs_ingesta.Estimated_Stop > dtStart && mebs_ingesta.Estimated_Stop < dtStop) || (mebs_ingesta.Estimated_Start >= dtStart && mebs_ingesta.Estimated_Start <= dtStop) || (mebs_ingesta.Estimated_Start <= dtStart && mebs_ingesta.Estimated_Stop >= dtStop)) && mebs_ingesta.IdChannel == id && mebs_ingesta.Type == 0 && mebs_ingesta.IsPublished == false && mebs_ingesta.IsExpired == false
                          orderby mebs_ingesta.Estimated_Start descending
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetProgramExists(string channelID, string startTime, string stopTime)
        {

            if (string.IsNullOrEmpty(channelID))
            {
                throw new ArgumentNullException("Channel ID",
                    "You must provide a value for the parameter'Channel ID'.");
            }


            int id = ConvertToInt32(channelID, 0);


            if (id == 0)
            {
                throw new ArgumentNullException("Channel ID",
                    "Bad value for the parameter for the parameter'Channel ID'.");
            }


            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }



            try
            {

                DateTime dtStart = DateTime.Parse(startTime);

                DateTime dtStop = DateTime.Parse(stopTime);


                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where ((mebs_ingesta.Estimated_Start >= dtStart && mebs_ingesta.Estimated_Start < dtStop) || (mebs_ingesta.Estimated_Stop > dtStart && mebs_ingesta.Estimated_Stop <= dtStop) || (mebs_ingesta.Estimated_Start < dtStart && mebs_ingesta.Estimated_Stop > dtStop)) && mebs_ingesta.IdChannel == id && mebs_ingesta.Type == 0 && mebs_ingesta.IsExpired == false
                          orderby mebs_ingesta.Estimated_Start descending
                          select mebs_ingesta;


                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetProgramsByTitle(string channelID, string startTime, string stopTime, string title)
        {


            if (string.IsNullOrEmpty(channelID))
            {
                throw new ArgumentNullException("Channel ID",
                    "You must provide a value for the parameter'Channel ID'.");
            }


            int id = ConvertToInt32(channelID, 0);


            if (id == 0)
            {
                throw new ArgumentNullException("Channel ID",
                    "Bad value for the parameter for the parameter'Channel ID'.");
            }


            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }



            try
            {

                DateTime dtStart = DateTime.Parse(startTime);

                DateTime dtStop = DateTime.Parse(stopTime);

                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where ((mebs_ingesta.Estimated_Stop > dtStart && mebs_ingesta.Estimated_Stop < dtStop) || (mebs_ingesta.Estimated_Start >= dtStart && mebs_ingesta.Estimated_Start <= dtStop) || (mebs_ingesta.Estimated_Start <= dtStart && mebs_ingesta.Estimated_Stop >= dtStop)) && mebs_ingesta.Title.Contains(title) && mebs_ingesta.IdChannel == id && mebs_ingesta.Type == 0 && mebs_ingesta.IsPublished == false && mebs_ingesta.IsExpired == false  // Contains
                          orderby mebs_ingesta.Estimated_Start descending
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetProgramsToSchedule(string startTime, string idChannel)
        {
            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }

            if (string.IsNullOrEmpty(idChannel))
            {
                throw new ArgumentException("ID Channel",
                    "You must provide a value for the parameter'idChannel'.");
            }

            try
            {
                DateTime dt = DateTime.Parse(startTime);
                int _Idchannel = Convert.ToInt32(idChannel);

                var epg = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                          where mebs_ingesta.Estimated_Start > dt
                          && mebs_ingesta.Type == 0 && mebs_ingesta.IsPublished == false && mebs_ingesta.IsExpired == false
                          && (mebs_ingesta.SelfCommercial.Value == -1 || mebs_ingesta.SelfCommercial.Value == 1)
                          && mebs_ingesta.IdChannel == _Idchannel
                          orderby mebs_ingesta.Estimated_Start
                          select mebs_ingesta;

                return epg;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetPackagesByDateCreation(string startTime, string stopTime)
        {

            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }

            try
            {

                DateTime dtStart = DateTime.Parse(startTime);

                DateTime dtStop = DateTime.Parse(stopTime);

                var package = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                              where (mebs_ingesta.Date_Creation >= dtStart && mebs_ingesta.Date_Creation <= dtStop)
                              && mebs_ingesta.Type == 1 && mebs_ingesta.IsExpired == false
                              && mebs_ingesta.MediaFileSizeAfterRedundancy != 0
                              orderby mebs_ingesta.Date_Creation descending
                              select mebs_ingesta;

                return package;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetPackagesByTitle(string startTime, string stopTime, string title)
        {

            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }

            try
            {
                DateTime dtStart = DateTime.Parse(startTime);
                DateTime dtStop = DateTime.Parse(stopTime);

                var package = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                              where (mebs_ingesta.Date_Creation >= dtStart && mebs_ingesta.Date_Creation <= dtStop)
                              && mebs_ingesta.Title.Contains(title)
                              && mebs_ingesta.Type == 1 && mebs_ingesta.IsExpired == false  // Contains
                              && mebs_ingesta.MediaFileSizeAfterRedundancy != 0
                              orderby mebs_ingesta.Date_Creation descending
                              select mebs_ingesta;

                return package;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetPackageByCode(string code)
        {

            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException("code Package",
                    "You must provide a value for the parameter'code'.");
            }

            try
            {
                var package = from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                              where mebs_ingesta.Code_Package == code && mebs_ingesta.IsExpired == false  // Contains
                              orderby mebs_ingesta.Date_Creation descending
                              select mebs_ingesta;

                return package;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_ingesta> GetPackagesToSchedule(int Type)
        {
            try
            {
                if (Type == -1)
                {
                    return from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                           where (mebs_ingesta.Type == 1 || mebs_ingesta.Type == 6) && mebs_ingesta.IsExpired == false
                           && (mebs_ingesta.SelfCommercial.Value == -1 || mebs_ingesta.SelfCommercial.Value == 1)
                           && mebs_ingesta.MediaFileSizeAfterRedundancy != 0
                           orderby mebs_ingesta.Date_Creation descending
                           select mebs_ingesta;
                }
                else
                {
                    return from mebs_ingesta in this.CurrentDataSource.mebs_ingesta
                           where mebs_ingesta.Type == Type && mebs_ingesta.IsExpired == false
                           && (mebs_ingesta.SelfCommercial.Value == -1 || mebs_ingesta.SelfCommercial.Value == 1)
                           && mebs_ingesta.MediaFileSizeAfterRedundancy != 0
                           orderby mebs_ingesta.Date_Creation descending
                           select mebs_ingesta;
                }

            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        #endregion

        #region ===================== mebs_schedule =====================
        [WebGet]
        public IQueryable<mebs_schedule> GetShedulesByFilters(int contentType, string startTime, string stopTime, int status, int idChannel)
        {
            if (contentType < 0)
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'Content Type'.");
            }

            if (string.IsNullOrEmpty(startTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'startTime'.");
            }


            if (string.IsNullOrEmpty(stopTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'stopTime'.");
            }

            try
            {
                DateTime dtStart = DateTime.Parse(startTime);
                DateTime dtStop = DateTime.Parse(stopTime);

                var schedules = from mebs_schedule in this.CurrentDataSource.mebs_schedule
                                where (
                                (mebs_schedule.Estimated_Stop > dtStart && mebs_schedule.Estimated_Stop < dtStop) ||
                                (mebs_schedule.Estimated_Start >= dtStart && mebs_schedule.Estimated_Start <= dtStop) ||
                                (mebs_schedule.Estimated_Start <= dtStart && mebs_schedule.Estimated_Stop >= dtStop))
                                && (mebs_schedule.mebs_ingesta.Type.Value == contentType)
                                && (mebs_schedule.IsDeleted.Value == -1)
                                select mebs_schedule;

                if (status != -1)
                {
                    if (status == -1008)
                    {
                        schedules = schedules.Where(s => s.Status == 1004 || s.Status == 1005 || s.Status == 1006);
                    }
                    else
                    {
                        schedules = schedules.Where(s => s.Status == status);
                    }
                }

                if (idChannel > 0)
                {
                    schedules = schedules.Where(s => s.mebs_ingesta.IdChannel == idChannel);
                }

                return schedules;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_schedule> GetSchedule(string idIngesta)
        {

            int id = ConvertToInt32(idIngesta, 0);

            if (id == 0)
            {
                throw new ArgumentNullException("Ingesta ID",
                    "Bad value for the parameter for the parameter'Ingesta ID'.");
            }


            var schedules = from mebs_schedule in this.CurrentDataSource.mebs_schedule
                            where mebs_schedule.IdIngesta == id
                            orderby mebs_schedule.Estimated_Start descending
                            select mebs_schedule;

            return schedules;

        }

        [WebGet]
        public IQueryable<mebs_schedule> GetAllSchedules()
        {
            var schedules = from mebs_schedule in this.CurrentDataSource.mebs_schedule
                            orderby mebs_schedule.Estimated_Start descending
                            select mebs_schedule;

            return schedules;

        }

        [WebGet]
        public IQueryable<mebs_schedule> GetListScheduleByDate(string dateTime)
        {

            if (string.IsNullOrEmpty(dateTime))
            {
                throw new ArgumentNullException("Date Time",
                    "You must provide a value for the parameter'dateTime'.");
            }

            try
            {
                DateTime dtStart = DateTime.Parse(dateTime);
                DateTime dtStop = dtStart.AddDays(1);
                var package = from mebs_schedule in this.CurrentDataSource.mebs_schedule
                              where mebs_schedule.IsDeleted.Value == -1 && (mebs_schedule.mebs_ingesta.Type.Value == 1 || mebs_schedule.mebs_ingesta.Type.Value == 6) &&
                                ((mebs_schedule.Estimated_Stop >= dtStart && mebs_schedule.Estimated_Stop < dtStop) ||
                                (mebs_schedule.Estimated_Start <= dtStart && mebs_schedule.Estimated_Stop >= dtStop) ||
                                (mebs_schedule.Estimated_Start >= dtStart && mebs_schedule.Estimated_Start <= dtStop) &&
                                (mebs_schedule.Estimated_Start < dtStop))
                              orderby mebs_schedule.Estimated_Start ascending
                              select mebs_schedule;

                return package;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }

        }
        #endregion

        #region ===================== mebs_category =====================
        [WebGet]
        public IQueryable<mebs_category> GetMixedCategoryElements(int IdParentCategory)
        {

            try
            {
                var lcat = from c in this.CurrentDataSource.mebs_category
                           where (from m in this.CurrentDataSource.mebs_mixedcategory
                                  where c.IdCategory == m.IdChildCategory
                                  select m.IdParentCategory).Contains(IdParentCategory)
                           select c;

                return lcat;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_category> GetAvailableCategoryElementsToBeMixed(int IdParentCategory)
        {

            try
            {
                var lcat = from c in this.CurrentDataSource.mebs_category
                           where !(from m in this.CurrentDataSource.mebs_mixedcategory where c.IdCategory == m.IdChildCategory select m.IdParentCategory).Contains(IdParentCategory)
                           && c.IdCategory != IdParentCategory && c.Value != "1" && c.Value != "2"
                           select c;

                return lcat;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }
        #endregion

        #region ===================== mebs_settings =====================
        [WebGet]
        public IQueryable<mebs_settings> SearchSettingsByName(string SettingName)
        {

            try
            {
                var lcat = from c in this.CurrentDataSource.mebs_settings
                           where c.Visibility == "Y" && c.SettingName.Contains(SettingName)
                           select c;

                return lcat;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        [WebGet]
        public IQueryable<mebs_settings> GetRedundancyParameters()
        {

            try
            {
                var lSettings = from c in this.CurrentDataSource.mebs_settings
                                where c.SettingName.Contains("Redundancy_BlockSize")
                                || c.SettingName.Contains("Redundancy_RepetitionSpace")
                                || c.SettingName.Contains("Redundancy_RedundancyRate")
                                || c.SettingName.Contains("Redundancy_RedundancyMatrix")
                                select c;

                return lSettings;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occured: {0}", ex);
            }
        }

        #endregion

        #region ===================== Authentification =====================
        [WebGet]
        public IQueryable<mebs_login> GetUserByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name",
                    "You must provide a value for the parameter'User Name'.");
            }

            try
            {

                var user = from mebs_login in this.CurrentDataSource.mebs_login
                           where mebs_login.Login == name
                           select mebs_login;

                return user;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_login> GetUserByNameAndPassword(string name, string password)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException("Name",
                    "You must provide a value for the parameter'User Name'.");
            }

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password",
                    "You must provide a value for the parameter'User password'.");
            }

            try
            {

                var user = from mebs_login in this.CurrentDataSource.mebs_login
                           where mebs_login.Login == name && mebs_login.Password == password
                           select mebs_login;

                return user;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_login> GetAllUsers()
        {
            try
            {
                var users = from mebs_login in this.CurrentDataSource.mebs_login
                            select mebs_login;

                return users;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebGet]
        public IQueryable<mebs_session> GetSessionByGuid(string SessionId)
        {
            if (string.IsNullOrEmpty(SessionId))
            {
                throw new ArgumentNullException("Name",
                    "You must provide a value for the parameter'User Name'.");
            }

            try
            {
                var session = from mebs_session in this.CurrentDataSource.mebs_session
                              where mebs_session.SessionId == SessionId
                              select mebs_session;

                return session;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        [WebGet]
        public IQueryable<mebs_session> GetUserSession(string userGUID)
        {
            if (string.IsNullOrEmpty(userGUID))
            {
                throw new ArgumentNullException("userGUID",
                    "You must provide a value for the parameter'User GUID'.");
            }

            try
            {
                var session = from mebs_session in this.CurrentDataSource.mebs_session
                              where mebs_session.UserGuid == userGUID
                              select mebs_session;

                return session;
            }
            catch (Exception ex)
            {
                return null;
                //throw new ApplicationException("An error occured: {0}", ex);
            }

        }

        #endregion

        protected override void OnStartProcessingRequest(ProcessRequestArgs args)
        {
            base.OnStartProcessingRequest(args);


        }

        protected override void HandleException(HandleExceptionArgs args)
        {
            if (args.Exception.InnerException is ArgumentException)
            {
                ArgumentException e = (ArgumentException)
                            args.Exception.InnerException;
                args.Exception = new DataServiceException(400,
                        "Error Property:" + e.ParamName,
                        "Description of the problem :",
                        "en-US",
                            e);
            }
            //else if (args.Exception.InnerException is CustomException)
            //{
            //    throw new DataServiceException(400, args.Exception.Message);
            //}

        }

        /// <summary>
        /// Convert string value to int32  
        /// </summary>
        /// <param name="value"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static int ConvertToInt32(string value, int DefaultValue)
        {
            int _value = DefaultValue;
            try
            {
                _value = Convert.ToInt32(value);
            }
            catch
            {
            }
            return _value;
        }


        //DateTime dt2 = DateTime.Parse(date, culture, System.Globalization.DateTimeStyles.AssumeLocal);


        /// <summary>
        /// Generate random digit code
        /// </summary>
        /// <param name="Length">Length</param>
        /// <returns>Result string</returns>
        public static string GenerateRandomDigitCode(int Length)
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < Length; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        public string GetDateTimeString()
        {

            return "yyyy-MM-dd HH:mm:ss";

        }

        #region  ._._._._._._._._. Private Methodes._._._._._._._._._.
        ///// <summary>
        ///// Genarate a Codi_ID for a specific content.
        ///// </summary>
        //private static readonly Random getrandom = new Random();
        //private static readonly object syncLock = new object();
        //public static int GeneratContentID()
        //{
        //    int nCODI_ID_MIN = 256;
        //    int nCODI_ID_MAX = 65280;
        //    lock (syncLock)
        //    { // synchronize
        //        return getrandom.Next(nCODI_ID_MIN, nCODI_ID_MAX);
        //    }
        //}
        #endregion

        // Override to manage returned exceptions. 
        //protected override void HandleException(HandleExceptionArgs args) 
        //{ 
        //    // Handle exceptions raised in service operations. 
        //    if (args.Exception is System.Reflection.TargetInvocationException 
        //        && args.Exception.InnerException != null) 
        //    { 
        //        if (args.Exception.InnerException.GetType() 
        //            == typeof(DataServiceException)) 
        //        { 
        //            // Unpack the DataServiceException. 
        //            args.Exception = args.Exception.InnerException; 
        //        } 
        //        else 
        //        { 
        //            // Return a new DataServiceException as "400: bad request." 
        //            args.Exception = 
        //                new DataServiceException(400, 
        //                    args.Exception.InnerException.Message); 
        //        } 
        //    } 
        //}


    }
}
