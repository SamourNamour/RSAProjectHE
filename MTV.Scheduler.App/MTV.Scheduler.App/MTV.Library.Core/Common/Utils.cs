
#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
using MTV.Library.Core.TriggerInterface;
//using CMEScheduler;
using System.IO;
using System.Configuration;
using System.ServiceProcess;
using MTV.Scheduler.App.UI;
using MTV.Library.Core.Data;
#endregion 

namespace MTV.Library.Core.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class Utils
    {
        static ServiceController _ehListnerService = new ServiceController("MTV.Scheduler");
        static bool useProdSystem = true;
        #region HeadEnd Property(ies)
        /// <summary>
        /// Formatting operations convert the Harris UDP msg (VIDEO_INTIME / DURATION) to valid TimeSpan value.
        /// </summary>
        public static string HarrisTriggerDateTimeFormatProvider
        {
            get
            {
                // Set default value:
                string formatProvider = "HH:mm:ss:ff";
                try
                {
                    formatProvider = ConfigurationManager.AppSettings["HarrisTriggerDateTimeFormatProvider"];
                }
                catch (Exception ex)
                {
                    //----.LogHelper.logger.Error(null, ex);
                }
                return formatProvider;
            }
        }
        #endregion 

        #region Auxliary Method(s)
        /// <summary>
        /// Used to parse UDP message "DURATION" element to TimeSpan DateTime.
        /// </summary>
        /// <param name="txt">string</param>
        /// <param name="VIDEO_ITEM">string</param>
        /// <param name="ts">out TimeSpan</param>
        /// <returns>bool</returns>
        public static bool TryParseTime(string txt, string VIDEO_ITEM, out TimeSpan ts)
        {
            /*
             Urgent : after check the real log of Digiturk Harris system it seems it use the 24 format 
             BTW : it's useful to store object that defines the format of date and time data into setting table.
             Useful URL : http://www.csharp-examples.net/string-format-datetime/             
             */

            ts = new TimeSpan();

            try
            {
                DateTime dt;
                bool ok = DateTime.TryParseExact(
                                                txt,
                                                HarrisTriggerDateTimeFormatProvider,
                                                null,
                                                System.Globalization.DateTimeStyles.NoCurrentDateDefault,
                                                out dt);
                ts = new TimeSpan(ok ? dt.Ticks : 0);
                if (ok == false)
                {
                    //----.LogHelper.logger.Error(string.Format(@"Wrong InputStringTimeSpan = [{0}] for VIDEO_ITEM = [{1}] : Either VIDEO_INTIME, DURATION cannot be parsed to a valid TIMESPAN based on the EBS stored format setting (AR_Harris_Trigger_DateTimeFormatProvider). Styles is not a valid DateTimeStyles value -or- styles contains an invalid combination of DateTimeStyles values.", 
                    //----.                       txt, 
                    //----.                      VIDEO_ITEM)                                           );
                }
                return ok;
            }
            catch (Exception ex)
            {
                //----.LogHelper.logger.Error(null, ex);                
                return false;
            }
        }

        /// <summary>
        ///  Used to parse UDP message "TIME" element to UTC DateTime.
        /// </summary>
        /// <param name="txt">string</param>
        /// <param name="VIDEO_ITEM">string</param>
        /// <param name="exactStart">out DateTime</param>
        /// <returns>bool</returns>
        public static bool TryParseTime(string txt, string VIDEO_ITEM, out DateTime exactStart)
        {
            exactStart = new DateTime();
            try
            {
                exactStart = DateTime.Parse(txt).ToUniversalTime();
                return true;
            }
            catch (Exception ex)
            {
                //----.LogHelper.logger.Error(string.Format(@"Wrong InputStringTimeSpan = [{0}] for VIDEO_ITEM = [{1}] : TIME cannot be parsed to a valid TIMESPAN. Styles is not a valid DateTimeStyles value -or- styles contains an invalid combination of DateTimeStyles values.",
                //----.                                       txt,
                //----.                                       VIDEO_ITEM),
                //----.                                       ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventExactStartTime"></param>
        /// <param name="receivedTiggerDateTime"></param>
        /// <param name="gracePeriod"></param>
        /// <returns></returns>
        public static DateTime NextTimeOfDayAfter(TimeSpan eventExactStartTime, DateTime receivedTiggerDateTime, TimeSpan gracePeriod)
        {
            //----.LogHelper.logger.Debug("eventExactStartTime = " + eventExactStartTime);
            //----.LogHelper.logger.Debug("receivedTiggerDateTime = " + receivedTiggerDateTime);
            //----.LogHelper.logger.Debug("gracePeriod = " + gracePeriod);
            

            DateTime result = receivedTiggerDateTime.Date + eventExactStartTime;
            if (result + gracePeriod < receivedTiggerDateTime)
            {
                result = result.AddDays(1);
                //----.LogHelper.logger.Debug("Eurika = " + result);
            }

            //----.LogHelper.logger.Debug("New Date = " + result);

            return result;            
        }

        /// <summary>
        /// If trigger messages are xml formatted, each message will specify the version of the schema
        /// to allow for futur expansion of the protocol.
        /// </summary>
        /// <param name="versionNumber"></param>
        /// <returns></returns>
        public static bool CheckIsValidSchemaVersion(decimal versionNumber)
        {
            bool isvalid = false;
            if (versionNumber == 1.0m)
            {
                isvalid = true;
                return isvalid;
            }
            return isvalid;
        }

        /// <summary>
        /// Create Directory.
        /// </summary>
        /// <param name="_sDirectory">string</param>
        /// <returns>string</returns>
        public static string CreateDirectory(string _sDirectory)
        {
            try
            {
                DirectoryInfo dirInf = Directory.CreateDirectory(_sDirectory);
                return dirInf.FullName;
            }
            catch (Exception ex)
            {
                //----.LogHelper.logger.Error(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// App Path Name.
        /// </summary>
        /// <returns>string</returns> 
        public static string GetPathName()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// Generat File name .
        /// </summary>
        /// <returns>string</returns> 
        public static string GenerateFileName(string context)
        {
            return context + "_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "_" + Guid.NewGuid().ToString() + ".xml";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stringLength"></param>
        /// <returns></returns>
        public static string GenerateRandomString(int stringLength)
        {
            Random rnd = new Random();
            Guid guid;
            String randomString = string.Empty;

            int numberOfGuidsRequired = (int)Math.Ceiling((double)stringLength / 32d);
            for (int i = 0; i < numberOfGuidsRequired; i++)
            {
                guid = Guid.NewGuid();
                randomString += guid.ToString().Replace("-", "");
            }

            return randomString.Substring(0, stringLength);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetLogDirectoryPath()
        {
            return CustomSettings.LogDirectoryPath;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetUDPMsgStatusDirectoryPath()
        {
            string _buildUDPMsgDirectory = GetPathName() + @"\" + "UDPMsgStatus";
            CreateIfMissing(_buildUDPMsgDirectory);
            return _buildUDPMsgDirectory;
            
          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetIngestaDirectoryPath()
        {
            //return useProdSystem ? @"D:\FtpRepository\Contents\IngestedMedia" : @"\\192.168.1.15\Contents\ingestedMedia";
            return @"\\127.0.0.1\Contents\ingestedMedia";
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetRedundancyDirectoryPath()
        {
            
            string _buildRedundancyDirectory = GetPathName() + @"\" + "RedundancyStatus";
            CreateIfMissing(_buildRedundancyDirectory);
            return _buildRedundancyDirectory;
        
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        private static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static long GetFileSize(string fileName)
        {
            FileInfo fInfo = new FileInfo(fileName);
            return fInfo.Length;
        }

      
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static int GetLogsTypeId()
        {
            return CustomSettings.LogsTypeId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        public static string TimeSpanToString(TimeSpan ts)
        {
            return new DateTime(ts.Ticks).ToString(HarrisTriggerDateTimeFormatProvider);
        }



        /// <summary>
        /// Check's if the file has the accessrights specified in the input parameters
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="fa">Read,Write,ReadWrite</param>
        /// <param name="fs">Read,ReadWrite...</param>
        /// <returns></returns>
        public static void CheckFileAccessRights(string fileName, FileMode fm, FileAccess fa, FileShare fs)
        {
            FileStream fileStream = null;
            StreamReader streamReader = null;
            try
            {
                Encoding fileEncoding = Encoding.Default;
                fileStream = File.Open(fileName, fm, fa, fs);
                streamReader = new StreamReader(fileStream, fileEncoding, true);
            }
            finally
            {
                try
                {
                    if (fileStream != null)
                    {
                        fileStream.Close();
                        fileStream.Dispose();

                    }
                    if (streamReader != null)
                    {
                        streamReader.Close();
                        streamReader.Dispose();
                    }
                }
                finally
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strFile"></param>
        /// <returns></returns>
        public static void FileDelete(string strFile)
        {
            if (strFile == null) return;
            
            if (strFile.Length == 0) return;
            
            try
            {
                string strFileFullPath = string.Format("{0}{1}{2}", MainForm.Conf.ReqDirectory, DefaultValues.IngestedMedia_Directory, strFile);
                if (!System.IO.File.Exists(strFileFullPath)) return;
                System.IO.File.Delete(strFileFullPath);
               
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
          
        }

        public static void ShutDownCMESchedulerService()
        {

            // Stop Vista & XP services
            bool ehListnerExist = false;
            bool success = true;
            // Check for existance of CMESchedulerService  without throwing/catching exceptions
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController srv in services)
            {
                if (srv.ServiceName == "CMEScheduler")
                {
                    ehListnerExist = true;
                    if (ehListnerExist) 
                        break;
                }
               
            }

            if (ehListnerExist && (_ehListnerService.Status != ServiceControllerStatus.Stopped) && (_ehListnerService.Status != ServiceControllerStatus.StopPending))
            {

                //Log.Info("  Stopping CMESchedulerService");
                try
                {
                    if ((_ehListnerService.Status != ServiceControllerStatus.Stopped) && (_ehListnerService.Status != ServiceControllerStatus.StopPending))
                    {
                        _ehListnerService.Stop();
                      
                    }
                }
                catch
                {
                    success = false;
                    //log error
                }

            }

            if (success)
            {
                // Log success
            }


        }

        public static double ConvertByteToMegaByte(long bytes)
        {
            try
            {
                return Math.Ceiling((bytes / 1024f) / 1024f);
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        #endregion
    }
}
