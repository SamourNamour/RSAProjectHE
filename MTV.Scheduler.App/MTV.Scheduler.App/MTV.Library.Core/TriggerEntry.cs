
#region Copyright Motive Television 2012
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: TriggerEntry.cs
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
    /// The TriggerEntry is an abstraction of Harris XML based protocol trigger messages.
    /// </summary>
    public class TriggerEntry
    {
        #region Field(s)

        private string strBus;
        private string strTITLE;
        private string strVIDEO_ITEM;
        private TimeSpan tsVIDEO_INTIME;
        private TimeSpan tsDuration;
        private DateTime tsTime;
        private MaterialType typeOfMaterialField;
        private DateTime createdDateTime;     

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
                return strVIDEO_ITEM;
            }
            set
            {
                strVIDEO_ITEM = value;
            }
        }

        /// <summary>
        /// Automatic-Recording event tape start or re-start time.
        /// </summary>
        public TimeSpan VIDEO_INTIME
        {
            get
            {
                return tsVIDEO_INTIME;
            }
            set
            {
                tsVIDEO_INTIME = value;
            }
        }

        /// <summary>
        /// Automatic-Recording segment duration.
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
            }
        }

        /// <summary>
        /// Automatic-Recording segment start date & time.
        /// </summary>
        public DateTime TIME
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
        /// Type of the Event (Movie / Series / Commercial / Trailer).
        /// </summary>
        public MaterialType TypeOfMaterial
        {
            get
            {
                return typeOfMaterialField;
            }
            set 
            {
                typeOfMaterialField = value;
            }                
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedDateTime
        {
            get
            {
                return createdDateTime;
            }
            private set
            {
                createdDateTime = value;
            }
        }

        /// <summary>
        /// Get object Metadata as string.
        /// </summary>
        public string MetaData
        {
            get
            {
                return string.Format("[BUS = {0}, TITLE = {1},VIDEO_ITEM = {2}, VIDEO_INTIME = {3},DURATION = {4},TIME = {5}, TypeOfMaterial = {6} ]",
                                      this.BUS,
                                      this.TITLE,
                                      this.VIDEO_ITEM,
                                      this.VIDEO_INTIME,
                                      this.DURATION,
                                      this.TIME,
                                      this.TypeOfMaterial);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan TriggerGracePeriod
        {
            get
            {
                return TimeSpan.FromMinutes(30);
            }
        }

        #endregion 

        #region Constructor(s)

        // Logic Respected        = ACK
        // Code Checked           = ACK
        // Code Commented         = ACK
        // Best pratice           = ACK
        // Premiminary test(s)    = ACK
        // Unit Test(s)           = NAK
        /// <summary>
        /// Default TriggerEntry constructor.
        /// </summary>
        public TriggerEntry()
        { 
            this.strBus = string.Empty;
            this.strTITLE = string.Empty;
            this.strVIDEO_ITEM = string.Empty;
            this.tsVIDEO_INTIME = TimeSpan.FromSeconds(0);
            this.tsDuration = TimeSpan.FromSeconds(0);
            this.tsTime = DateTime.MinValue;
            this.typeOfMaterialField = MaterialType.U;
            this.createdDateTime = DateTime.UtcNow;
        }

        #endregion 
    }
}
