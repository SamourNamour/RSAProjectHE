using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Scheduler.App.MTVControl
{
    public class Default
    {

        /// <summary>
        /// 
        /// </summary>
        public static Uri CatalogLocation = new Uri(@"http://127.0.0.1:8085/MEBSCatalog/");            /* Casablanca Server : MarocTelecom */
        
        //public static Uri MEBSCatalogLocation = new Uri(@"http://127.0.0.1/MEBSCatalog/MEBSCatalog.svc/");        /* Loopback IP Address */

        /// <summary>
        /// Get List Programs By Title (AR || DC)
        /// </summary>
        public static string ListIngestedEventsByType = "GetIngestedEventsByType?type='{0}'";

        /// <summary>
        /// Get All Programs for a specific Channel
        /// </summary>
        public const string GetPrograms = "GetPrograms?channelID='{0}'";

        /// <summary>
        /// Get List Programs By StartTime for a specific Channel
        /// </summary>
        public const string GetProgramsByStartTime = "GetProgramsByStartTime?channelID='{0}'&startTime='{1}'";

        /// <summary>
        /// Get List Programs By StartTime,StopTime for a specific Channel
        /// </summary>
        public const string GetProgramsByStartTimeAndStopTime = "GetProgramsByStartTimeAndStopTime?channelID='{0}'&startTime='{1}'&stopTime='{2}'";

        /// <summary>
        /// Get List Programs By StartTime,StopTime and Title for a specific Channel
        /// </summary>
        public const string GetProgramsByTitle = "GetProgramsByTitle?channelID='{0}'&startTime='{1}'&stopTime='{2}'&title='{3}'";

        /// <summary>
        /// Get List PVOD Channels
        /// </summary>
        public const string GetVODChannel = "GetVODChannel";

        /// <summary>
        /// Get List AutoRecording Channels
        /// </summary>
        public const string GetServices = "GetServices";

        /// <summary>
        /// Get List Categories
        /// </summary>
        public const string GetCategories = "GetCategories";

        /// <summary>
        /// Get List of Schedule by Extimated Start Date Time.
        /// </summary>        
       // public const string GetSchedulesByStartTime = @"mebs_schedule()?$filter=Status eq {0}                                                         
                                                    //    and Estimated_Start gt datetime'{1}' 
                                                    //    and Estimated_Start lt datetime'{2}'
                                                    //    &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails,mebs_ingesta/mebs_videoitem&$orderby=Estimated_Start";


        public const string GetSchedulesByStartTime = @"mebs_schedule()?$filter=Status eq {0}                                                         
                                                        and Estimated_Start gt datetime'{1}' 
                                                        and Estimated_Start lt datetime'{2}'
                                                        &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails&$orderby=Estimated_Start";



        /*
         mebs_schedule()?$filter=Status eq 1007 and mebs_ingesta/Type eq 1  and Estimated_Start gt datetime'0001-01-01T00:00:00'       and Estimated_Start lt datetime'2013-05-05T10:30:18'   &$expand=mebs_ingesta/mebs_channel,mebs_ingesta/mebs_ingestadetails,mebs_ingesta/mebs_videoitem&$orderby=Estimated_Start
         */

    }
}
