
#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// Class used to communicate required EBS ECW Web Service information to start an Automatic-Recording.
    /// TITLE, VIDEO_ITEM, VIDEO_INTIME (Start Time), DURATION  (Stop Time),BUS and TIME (Exact Start Time).
    /// </summary>
    public class UDPmsg_t_Mapping
    {
        /*
         <UDPmsg schemaVersion="1.0" msgNumber="20120">
         <event>
         <field name="BUS">CHNL_23</field>
         <field name="TITLE">2012 (1/2 HD)</field>
         <field name="VIDEO_ITEM">LY91066991</field>
         <field name="VIDEO_INTIME">00:02:00:00</field>
         <field name="DURATION">01:15:53:07</field>
         <field name="TIME">08:56:05</field>
         </event></UDPmsg>
         */

        #region Attribut(s)
        private string strTITLE;
        private string strVIDEO_ITEM;
        private DateTime dtVIDEO_INTIME;
        private DateTime dtVIDEO_INTIME_STOP;
        private string strBus;
        #endregion 

        #region Property(ies)
        /// <summary>
        /// Automatic-Recording event title.
        /// </summary>
        public string TITLE
        {
            get
            {
                return strTITLE;
            }
            set
            {
                strTITLE = value;
            }
        }

        /// <summary>
        /// Automatic-Recording event mapping code (Lysis Code).
        /// </summary>
        public string VIDEO_ITEM
        {
            get
            {
                return strVIDEO_ITEM;
            }
            set
            {
                strVIDEO_ITEM = value;
            }
        }

        /// <summary>
        /// utomatic-Recording event exact start time.
        /// </summary>
        public DateTime VIDEO_INTIME
        {
            get
            {
                return dtVIDEO_INTIME;
            }
            set
            {
                dtVIDEO_INTIME = value;
            }
        }

        /// <summary>
        /// Automatic-Recording event exact stop time.
        /// </summary>
        public DateTime VIDEO_INTIME_STOP
        {
            get
            {
                return dtVIDEO_INTIME_STOP;
            }
            set
            {
                dtVIDEO_INTIME_STOP = value;
            }
        }

        /// <summary>
        /// Automatic-Recording event channel Harris Interface Name.
        /// </summary>
        public string BUS
        {
            get
            {
                return strBus;
            }
            set
            {
                strBus = value;
            }
        }
        #endregion 
    }
}
