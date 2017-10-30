using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MTV.MAM.WebApp.MEBSCatalog;

namespace MTV.MAM.WebApp.Helper
{
    public class CPintaTaula
    {

        public bool FreeSpace { get; set; }
        public bool SelectedSpace { get; set; }
        public bool EnableSchedule { get; set; }
        public int SecondStart { get; set; }
        public int SecondEnd { get; set; }
        public int SecondStartHmax { get; set; }
        public int SecondEndHmax { get; set; }       
        public int Type { get; set; }
        public DateTime StartTime { get; set; } //---- Start Time of a Selected Block
        public DateTime EndTime { get; set; } //------- End Time of a Selected Block       
        public int IdSchedule { get; set; }
        public int IdIngesta { get; set; }
        public bool IsExpired { get; set; }
        public string Title { get; set; }
        public int PackageStatus { get; set; }
        public string StatusName { get; set; }
        public string ImageStatus { get; set; }
        //public string ItemDetails { get; set; }
        public long RedundancyFileSize { get; set; }
        public int ContentID { get; set; }
        public string Code_Package { get; set; }
        public string EventID { get; set; }
        public int Duration { get; set; }
        public DateTime Content_StartTime { get; set; }
        public DateTime Content_EndTime { get; set; }
        public DateTime Immortality_time { get; set; }
        public DateTime Expiration_time { get; set; }


        #region Contructor (s)
        public CPintaTaula()
        {
            FreeSpace = true;
            SelectedSpace = false;
            EnableSchedule = false;
            SecondStart = -1;
            SecondEnd = -1;
            SecondStartHmax = -1;
            SecondEndHmax = -1;
            Type = Convert.ToInt32(MTV.Library.Common.MediaType.DC_CHANNEL);
            StartTime = DateTime.MinValue;
            EndTime = DateTime.MinValue;
            IdSchedule = -1;
            IdIngesta = -1;
            IsExpired = false;
            Title = string.Empty;
            PackageStatus = Convert.ToInt32(MTV.Library.Common.ScheduleStatus.PREPARED);
            StatusName = string.Empty;
            ImageStatus = string.Empty;
            RedundancyFileSize = -1;
            ContentID = -1;
            Code_Package = string.Empty;
            EventID = string.Empty;
            Content_EndTime = DateTime.MinValue;
            Content_StartTime = DateTime.MinValue;
            Duration = 0;
        }
        #endregion

    }
}