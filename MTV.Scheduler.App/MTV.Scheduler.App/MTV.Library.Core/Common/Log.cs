using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using System.Data;
using System.Configuration;

namespace MTV.Library.Core
{
    /// <summary>
    /// Descripción breve de ErrHandler
    /// </summary>
    public class Log
    {
        #region Static Member(s)
        static int _maxRepetitions = 10;
        static int _maxLogSizeMb = 200;
        static List<string> _lastLogLines = new List<string>(_maxRepetitions);
        #endregion

        #region Constructor(s)

        /// <summary>
        /// 
        /// </summary>
        public Log()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        static Log()
        {
            Directory.CreateDirectory(string.Format(@"{0}\Logs\", GetPathName()));
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetPathName()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// Handles error by accepting the error message 
        /// Displays the page on which the error occured
        public static void Write(string errorMessage)
        {
            lock (typeof(Log))
            {
                try
                {
                    string path = GetPathName() + @"\Logs\events_" + DateTime.Today.ToString("dd-MM-yy", DateTimeFormatInfo.InvariantInfo) + ".log";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }

                    if (IsRepetition(errorMessage))
                        return;

                    CacheLogLine(errorMessage);

                    if (CheckLogPrepared(path))
                    {
                        using (StreamWriter w = File.AppendText(path))
                        {
                            string err = string.Format("{0}{1}", DateTime.Now.ToString(CultureInfo.InvariantCulture).PadRight(25), errorMessage);
                            w.WriteLine(err);
                            w.Flush();
                            w.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Write(ex.Message);
                }
            }
        }


        /// <summary>
        /// /
        /// </summary>
        /// <param name="channelBus"></param>
        /// <param name="trigegrMessage"></param>
        public static void ChannelLoger(
            string channelBus, 
            string trigegrMessage)
        {
            lock (typeof(Log))
            {
                try
                {
                    StreamWriter SW = null;
                    string channelFolder = string.Format(@"{0}{1}Logs\{2}",
                             GetPathName(),
                             Path.DirectorySeparatorChar,
                             channelBus
                             );
                    if (!Directory.Exists(channelFolder))
                    {
                        Directory.CreateDirectory(channelFolder);
                    }

                    string path = string.Format(@"{0}{1}Logs\{2}{3}{4}{5}",
                                                 GetPathName(),
                                                 Path.DirectorySeparatorChar,
                                                 channelBus,
                                                 Path.DirectorySeparatorChar,
                                                 DateTime.Today.ToString("dd-MM-yy", DateTimeFormatInfo.InvariantInfo),
                                                 ".log"
                                                 );
                    if (!File.Exists(path))
                    {
                        string headLine = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}",
                                                     "Date and Time".PadRight(25),
                                                     "TYPE_MATERIAL".PadRight(15),
                                                     "VIDEO_ITEM".PadRight(25),
                                                     "TIME".PadRight(25),
                                                     "DURATION".PadRight(20),
                                                     "BUS".PadRight(15),
                                                     "VIDEO_INTIME".PadRight(15),
                                                     "TITLE".PadRight(50));

                        File.Create(path).Close();
                        using (SW = File.AppendText(path))
                        {
                            string err = string.Format("{0}{1}", DateTime.Now.ToString(CultureInfo.InvariantCulture).PadRight(25), trigegrMessage);
                            SW.WriteLine(headLine);
                            SW.Flush();
                            SW.Close();
                        }
                    }
                    using (SW = File.AppendText(path))
                    {
                        string err = string.Format("{0}{1}", DateTime.Now.ToString(CultureInfo.InvariantCulture).PadRight(25), trigegrMessage);
                        SW.WriteLine(err);
                        SW.Flush();
                        SW.Close();
                    }
                }
                catch (Exception ex)
                {
                    Write(ex.Message);
                }
            }
        }

        /// Handles error by accepting the error message 
        /// Displays the page on which the error occured
        public static void LogEventAction(string eventActionMessage)
        {
            lock (typeof(Log))
            {
                try
                {
                    string path = GetPathName() + @"\Logs\eventsAction_" + DateTime.Today.ToString("dd-MM-yy", DateTimeFormatInfo.InvariantInfo) + ".log";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }

                    if (IsRepetition(eventActionMessage))
                        return;

                    CacheLogLine(eventActionMessage);

                    if (CheckLogPrepared(path))
                    {
                        using (StreamWriter w = File.AppendText(path))
                        {
                            string err = string.Format("{0}{1}", DateTime.Now.ToString(CultureInfo.InvariantCulture).PadRight(25), eventActionMessage);
                            w.WriteLine(err);
                            w.Flush();
                            w.Close();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Write(ex.Message);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aLogLine"></param>
        /// <returns></returns>
        private static bool IsRepetition(string aLogLine)
        {
            bool result = true;
            // as long as the cache is not full we have no repetitions
            if (_lastLogLines.Count == _maxRepetitions)
            {
                foreach (string singleLine in _lastLogLines)
                {
                    if (aLogLine.CompareTo(singleLine) != 0)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else
                result = false;

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aLogLine"></param>
        private static void CacheLogLine(string aLogLine)
        {
            if (!string.IsNullOrEmpty(aLogLine))
            {
                if (_lastLogLines.Count == _maxRepetitions)
                    _lastLogLines.RemoveAt(0);

                _lastLogLines.Add(aLogLine);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="aLogFileName"></param>
        /// <returns></returns>
        private static bool CheckLogPrepared(string aLogFileName)
        {
            bool result = true;

            try
            {
                //If the user or some other event deleted the dir make sure to recreate it.
                // Directory.CreateDirectory(string.Format(@"{0}\Logs\", GetPathName()));
                if (File.Exists(aLogFileName))
                {


                    DateTime fileDate = DateTime.Now;
                    try
                    {
                        FileInfo logFi = new FileInfo(aLogFileName);

                        logFi.Refresh();
                        fileDate = logFi.CreationTime;


                        if (logFi.Length > _maxLogSizeMb * 1000 * 1000)
                        {
                            result = false;
                        }
                    }
                    catch (Exception) { }

                }



            }
            catch (Exception) { }

            return result;


        }
    }
}
