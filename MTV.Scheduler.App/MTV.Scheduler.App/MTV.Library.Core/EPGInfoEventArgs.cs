

#region Copyright Motive Television 2012
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: EpgEntry.cs
//
#endregion

#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;

// Custom Directive(s)
using MTV.Library.Core.CommerialBreaks;
#endregion 

namespace MTV.Library.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class EPGInfoEventArgs : EventArgs
    {
        #region Attribut(s)
        private EpgEntry item;
        private bool willStart;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public EPGInfoEventArgs(EpgEntry e)
        {
            this.item = e;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="willStart"></param>
        public EPGInfoEventArgs(EpgEntry e, bool willStart)
            : this(e)
        {
            this.willStart = willStart;
        }
        #endregion

        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public EpgEntry EpgItem
        {
            get
            {
                return item;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WillStart
        {
            get { return willStart; }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class SegmentEventArgs : EPGInfoEventArgs
    {
        #region Fields
        private Segment segment;
        private bool willStart;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="segment"></param>
        public SegmentEventArgs(EpgEntry e, Segment segment)
            : base(e)
        {
            this.segment = segment;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="segment"></param>
        public SegmentEventArgs(EpgEntry e, Segment segment, bool willStart)
            : base(e, willStart)
        {
            this.segment = segment;
        }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public Segment Segment
        {
            get { return segment; }
            set { segment = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WillStart
        {
            get { return willStart; }
        }

        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public class AdvertisementEventArgs : EPGInfoEventArgs {
        #region Fields
        private CommerialBreak objCommerialBreak;
        private bool willStart;
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="segment"></param>
        public AdvertisementEventArgs(EpgEntry e, CommerialBreak item)
            : base(e) {
            this.objCommerialBreak = item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="segment"></param>
        public AdvertisementEventArgs(EpgEntry e, CommerialBreak item, bool willStart)
            : base(e, willStart) {
                this.objCommerialBreak = item;
        }
        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public CommerialBreak Advertisement {
            get { return objCommerialBreak; }
            set { objCommerialBreak = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool WillStart {
            get { return willStart; }
        }

        #endregion
    }
}
