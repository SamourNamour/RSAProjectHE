#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: DataCastCommandScheduleIntegrationExtension.cs
//
#endregion

#region
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.Library.Core.Extensions;
using MTV.Library.Core;
using System.Threading;
using System.IO;
using MTV.Library.Core.Instrumentation;
using MTV.Scheduler.App.UI;
#endregion

namespace MTV.Scheduler.App.MTV.EventDispatcher.Service.Extensions.MemoryManagement
{
    public class MemoryManagementExtension : IExtension , IDisposable
    {
        #region -.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-
        private const int ClearOutDatedEventIntervalInMinutes = 5;
        private System.Threading.Timer timer;
        private object SaveFromDispose = new object();
        private string strlastRemove = Properties.Settings.Default.lastRemove;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : IExtension Member(s) -.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get { return "Extension for killing associated threads and removing Ended triggers object from memory"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public IUIExtension UIExtension
        {
            get { return null; }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Constructor(s) / Finalizer(s) -.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        public MemoryManagementExtension()
        {
            EPGManager.Instance.AddEpgEvent += new EventHandler<EPGInfoEventArgs>(manager_EPGAdd);

            TimerCallback refreshCallBack = new TimerCallback(Clean);
            TimeSpan refreshInterval = TimeSpan.FromMinutes(ClearOutDatedEventIntervalInMinutes);
            timer = new Timer(refreshCallBack, null, new TimeSpan(-1), refreshInterval);

        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Public Method(s) -.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        ///  Clean outdated epg events.
        /// </summary>
        /// <param name="_object"></param>
        void Clean(object _object)
        {
            DateTime lastRemove = DateTime.MinValue;
            DateTime.TryParse(strlastRemove, out lastRemove);
            TimeSpan ts = DateTime.Now.ToUniversalTime().Subtract(lastRemove);
            if (ts.TotalMinutes > TimeSpan.FromDays(1).TotalMinutes) //1440 mins = 1 day. - we only want to remove once per day.
            {
                ClearOutdatedEvents();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearOutdatedEvents()
        {
            List<EpgEntry> EPGsToRemove = new List<EpgEntry>();
            using (EPGManager.Instance.LockDownloadList(false))
            {
                IList<EpgEntry> ars = EPGManager.Instance.EpgCollection;
                if (ars != null &&
                    ars.Count > 0)
                {
                    for (int i = 0; i < ars.Count; i++)
                    {
                        TimeSpan ts = DateTime.Now.ToUniversalTime().Subtract(ars[i].CreatedDateTime);
                        if (ts.TotalMinutes > TimeSpan.FromDays(30).TotalMinutes) // 1 Month
                        {
                            EpgEntry E = ars[i];
                            EPGsToRemove.Add(E);
                        }
                    }
                }
            }

            RemoveItems(EPGsToRemove);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EventsToClear"></param>
        private void RemoveItems(List<EpgEntry> EventsToRemove)
        {
            try
            {
                if (EventsToRemove != null &&
                    EventsToRemove.Count > 0)
                {
                    for (int i = 0; i < EventsToRemove.Count; i++)
                    {
                        try
                        {
                            EPGManager.Instance.RemoveEpg(EventsToRemove[i]);

                            MainForm.LogMessageToFile(string.Format("{0} / {1} EPG entry has been successfully deleted.",
                                                                            EventsToRemove[i].OriginalTitle,
                                                                            EventsToRemove[i].ContentID));

                          
                        }
                        catch (Exception ex)
                        {
                            MainForm.LogExceptionToFile(ex);
                            continue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            finally
            {
                Properties.Settings.Default.lastRemove = DateTime.UtcNow.ToString();
                Properties.Settings.Default.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetXmlFile(string id)
        {
            string file = Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath) + "\\" + "PersistedList" + "\\" + id + ".xml";
            return file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void manager_EPGAdd(object sender, EPGInfoEventArgs e)
        {

            e.EpgItem.StateChanged += delegate(object s, EventArgs ea)
            {
                if (((EpgEntry)s).State == EpgStatus.Stopped ||
                    ((EpgEntry)s).State == EpgStatus.Failed ||
                    ((EpgEntry)s).State == EpgStatus.Aborted ||
                    ((EpgEntry)s).State == EpgStatus.Unknowned)
                {

                   
                        //MainForm.LogMessageToFile(string.Format("{0} killing associated Thread.",
                                                                         // e.EpgItem.ID.ToString() ));
                                                                         
                        e.EpgItem.Ended();
                   
                }
            };

        }


        #endregion
    }
}
