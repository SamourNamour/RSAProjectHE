
#region Copyright Motive Television 2012
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: Segment.cs
//
#endregion

#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;

// Custom Directive(s)
#endregion 

namespace MTV.Library.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class Segment
    {
        #region Attribut(s)
        private string strBus;
        private string strTITLE;
        private string strSegmentID;        
        private TimeSpan tsDuration;
        private TimeSpan tsTotalDuration;
        private TimeSpan tsTime;
        private SegmentState segmentStatusField;
        private DateTime lastErrorDateTime ;
        private Exception lastError;
        private int reRunsField;
        #endregion

        #region Property(ies)

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
                return strSegmentID;
            }
            set
            {
                strSegmentID = value;
            }
        }

        /// <summary>
        /// Automatic-Recording event segment duration.
        /// </summary>
        public TimeSpan DURATION
        {
            get
            {
                return tsDuration;
            }
            set
            {
                tsDuration = value;
                this.TotalDuration += value;
            }
        }

        /// <summary>
        /// Automatic-Recording event segment duration.
        /// </summary>
        public TimeSpan TotalDuration
        {
            get
            {
                return tsTotalDuration;
            }
            set
            {
                tsTotalDuration = value;
            }
        }        

        /// <summary>
        /// Automatic-Recording event segment timespan.
        /// </summary>
        public TimeSpan TIME
        {
            get
            {
                return tsTime;
            }
            set
            {
                tsTime = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public SegmentState Status
        {
            get
            {
                return segmentStatusField;
            }
            set
            {
                segmentStatusField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime LastErrorDateTime
        {
            get
            {
                return lastErrorDateTime;
            }
            set
            {
                lastErrorDateTime = value;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public Exception LastError
        {
            get
            {
                return lastError;
            }
            set
            {
                if (value != null)
                {
                    lastErrorDateTime = DateTime.Now;
                }
                else
                {
                    lastErrorDateTime = DateTime.MinValue;
                }
                lastError = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int ReRuns
        {
            get
            {
                return reRunsField;
            }
            set
            {
                reRunsField = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string MetaData
        {
            get
            {
                return string.Format("[BUS = {0}, TITLE = {1},VIDEO_ITEM = {2}, TotalDuration = {3},DURATION = {4},TIME = {5}, Status = {6}, ReRuns = {7} ]",
                                      this.BUS,
                                      this.TITLE,
                                      this.VIDEO_ITEM,
                                      this.TotalDuration,
                                      this.DURATION,
                                      this.TIME,
                                      this.Status,
                                      this.ReRuns);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime Start_tc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MaterialType MaterialType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string PlayoutReply { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PlayoutReplyTime { get; set; }
        #endregion 

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        public Segment()
        {
            this.strBus = string.Empty;
            this.strTITLE = string.Empty;
            this.strSegmentID = string.Empty;
            this.tsDuration = TimeSpan.FromSeconds(0);
            this.tsTotalDuration = TimeSpan.FromSeconds(0);
            this.tsTime = TimeSpan.FromSeconds(0);
            this.segmentStatusField = SegmentState.Unstarted;
            this.LastErrorDateTime = DateTime.MinValue;
            this.lastError = null;
            this.reRunsField = 0;
            this.Start_tc = DateTime.MinValue;
            this.MaterialType = MaterialType.U;
            this.PlayoutReply = string.Empty;
            this.PlayoutReplyTime = default(DateTime);
        }
        #endregion 
    }
}
