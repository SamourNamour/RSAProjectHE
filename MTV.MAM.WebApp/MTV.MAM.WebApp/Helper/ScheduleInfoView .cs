using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MTV.MAM.WebApp.Helper
{
    public class ScheduleInfoView
    {
        public int IdSchedule { get; set; }
        public int IdIngesta { get; set; }
        public DateTime Estimated_Start { get; set; }
        public DateTime Estimated_stop { get; set; }
        public DateTime Exact_Start { get; set; }
        public DateTime Exact_Stop { get; set; }
        public string ChannelName { get; set; }
        public int Status { get; set; }
        public string Title { get; set; }
        public bool IsExpired { get; set; }
    }
}