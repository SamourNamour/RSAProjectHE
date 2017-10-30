
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: DateTimeUtils.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

// Custom Directive(s)
//using Logger.HELogger;
#endregion 

namespace MTV.Library.Core.Tools
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeUtils {
        #region - Static Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="formattedTime"></param>
        /// <param name="objTimeSpan"></param>
        /// <returns></returns>
        public static bool TryParseFormattedTime(string formattedTime,
                                                 out TimeSpan objTimeSpan) {
            objTimeSpan = new TimeSpan();
            try {

                DateTime dt;
                bool ok = DateTime.TryParseExact(
                                                formattedTime,
                                                "HH:mm:ss:ff",
                                                null,
                                                System.Globalization.DateTimeStyles.NoCurrentDateDefault,
                                                out dt);
                objTimeSpan = new TimeSpan(ok ? dt.Ticks : 0);
                if (ok == false) {
                    //-----.DefaultLogger.JADELogger.Error(string.Format(@"Wrong InputStringTimeSpan = [{0}] : Either VIDEO_INTIME, DURATION cannot be parsed to a valid TIMESPAN based on the MEBS stored format (yyyy-mm-dd hh:mm:ss). Styles is not a valid DateTimeStyles value -or- styles contains an invalid combination of DateTimeStyles values.",
                    //-----.                              formattedTime));
                }
                return ok;
            }
            catch (Exception ex) {
                //-----.DefaultLogger.STARLogger.Error(ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formattedTime"></param>
        /// <param name="objDateTime"></param>
        /// <returns></returns>
        public static bool TryParseFormattedTime(string formattedTime,
                                                 out DateTime objDateTime) {
            objDateTime = new DateTime();
            try {
                objDateTime = DateTime.Parse(formattedTime).ToUniversalTime();
                return true;
            }
            catch (Exception ex) {
                //-----.DefaultLogger.JADELogger.Error(string.Format(@"Wrong InputStringTimeSpan = [{0}] : Either VIDEO_INTIME, DURATION cannot be parsed to a valid TIMESPAN based on the MEBS stored format (yyyy-mm-dd hh:mm:ss). Styles is not a valid DateTimeStyles value -or- styles contains an invalid combination of DateTimeStyles values.",
                //-----.                                               formattedTime), ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ConvertDateTimeToEDMFormat(DateTime objDateTime) {
            return objDateTime.ToString("yyyy-MM-ddTHH:mm:ss");
        }

        /// <summary>
        /// Convert DateTime to string (Short DateTime format yyyy-mm-dd).
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateStringDate(DateTime? objDateTime) {
            string dateOnly = string.Format("{0}-{1:00}-{2:00}", objDateTime.Value.Year, objDateTime.Value.Month, objDateTime.Value.Day);
            return dateOnly;
        }

        /// <summary>
        /// Convert DateTime to string (Short DateTime format yyyy-mm-dd).
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateStringTime(DateTime? objDateTime) {
            string timeOnly = string.Format("{0:00}:{1:00}:{2:00}", objDateTime.Value.Hour, objDateTime.Value.Minute, objDateTime.Value.Second);
            return timeOnly;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objDateTime"></param>
        /// <returns></returns>
        public static string GenerateMPEG7StringTime(DateTime? objDateTime) {
            string timeOnly = string.Format("{0:00}:{1:00}", 
                                             objDateTime.Value.Hour, 
                                             objDateTime.Value.Minute);
            return timeOnly;
        }

        /// <summary>
        /// Convert DateTime to string (Short DateTime format yyyy-mm-dd).
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateStringTime(DateTime? objDateTime, bool includeMillisecondPrecision) {

            return (!includeMillisecondPrecision ?
                    GenerateStringTime(objDateTime) :
                    string.Format("{0:00}:{1:00}:{2:00}.{3:000}", 
                                   objDateTime.Value.Hour, 
                                   objDateTime.Value.Minute, 
                                   objDateTime.Value.Second,
                                   objDateTime.Value.Millisecond)
                   );
        }

        /// <summary>
        /// Convert TimeSpan to string (Short TimeSpan format HH:mm:ss).
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string GenerateStringTime(TimeSpan objTimeSpan) {
            string timeOnly = string.Format("{0:00}:{1:00}:{2:00}", objTimeSpan.Hours, objTimeSpan.Minutes, objTimeSpan.Seconds);
            return timeOnly;
        }


        /// <summary>
        /// Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp</returns>
        public static int CurrentDateTime_UnixTimestamp()
        {
            return (int)Math.Truncate((DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        /// <summary>
        /// Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp</returns>
        public static int DateTime_UnixTimestamp(DateTime dt)
        {
            return (int)Math.Truncate((dt.Subtract(new DateTime(1970, 1, 1))).TotalSeconds);
        }

        public static DateTime ConvertValueToDateTime(DateTime? _dtLastSendFuturList)
        {
            try
            {
                return DateTime.Parse(_dtLastSendFuturList.Value.ToString());
            }
            catch (Exception ex)
            {
                return DateTime.UtcNow;
            }
        }
        #endregion


    }
}
