
#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.Collections;
using System.Threading;
using MTV.Library.Core.Concurrency;
using System.IO;
using MTV.Library.Core.Common;
//using Logger.HELogger;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// 
    /// </summary>
    public class TriggerManager
    {
        #region Variables(s)
        private static TriggerManager instance = new TriggerManager();
        public static TriggerManager Instance
        {
            get
            {
                return instance;
            }
        }
        private List<event_t> triggers = new List<event_t>();
        private ReaderWriterObjectLocker TriggerListSync = new ReaderWriterObjectLocker();
        #endregion 
        
        #region Event(s)
        public event EventHandler<TriggerInfoEventArgs> TriggerAdded;
        public event EventHandler<TriggerInfoEventArgs> TriggerEnded;
        public event EventHandler<TriggerInfoEventArgs> TriggerRemoved;
        #endregion 

        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public ReadOnlyCollection<event_t> Triggers
        {
            get
            {
                return triggers.AsReadOnly();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalFailedTrigger
        {
            get
            {
                double total = 0;
                return total;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public double TotalSuccessTrigger
        {
            get
            {
                double total = 0;

                using (LockDownloadList(false))
                {
                    for (int i = 0; i < this.Triggers.Count; i++)
                    {
                        if (this.Triggers[i].EventStateProp == EventState.succes)
                        {
                            total++;
                        }
                    }
                }

                return total;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public double TotalTriggers
        {
            get
            {
                double total = 0;

                using (LockDownloadList(false))
                {
                    for (int i = 0; i < this.Triggers.Count; i++)
                    {
                        total++;
                        //-----.DefaultLogger.GARBAGELogger.Warn(this.Triggers[i].EventStateProp.ToString());
                    }
                }

                return total;
            }
        }
        #endregion 

        #region Method(s)
        /// <summary>
        /// 
        /// </summary>
        public void ClearEnded()
        {
            using (LockDownloadList(true))
            {
               // int Total_Succes_Or_Failed_Triggers = 0;
               // int Total_Wrong_UDPMessage_Input_Params = 0;
                //Log.Write("Total Triggers Into The System Now (Before Cleaning) = " + triggers.Count);                
                for (int i = triggers.Count - 1; i >= 0; i--)
                {
                    if (triggers[i].EventStateProp == EventState.succes || triggers[i].EventStateProp == EventState.Failed ||
                        triggers[i].EventStateProp == EventState.Error || triggers[i].EventStateProp == EventState.Wrong)
                    {
                        triggers.RemoveAt(i);
                    }                   
                }                            
            }
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lockForWrite"></param>
        /// <returns></returns>
    
        public IDisposable LockDownloadList(bool lockForWrite)
        {
            if (lockForWrite)
            {
                return TriggerListSync.LockForWrite();
            }
            else
            {
                return TriggerListSync.LockForRead();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void trigger_StateChanged(object sender, EventArgs e)
        {
            event_t eventInfo = (event_t)sender;

            if (eventInfo.EventStateProp == EventState.Ended ||
                eventInfo.EventStateProp == EventState.Error)
            {
                OnTriggerEnded((event_t)sender);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTriggerEnded(event_t e)
        {
            if (TriggerEnded != null)
            {
                TriggerEnded(this, new TriggerInfoEventArgs(e));
            }
        }
            
        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
 
        public void RemoveEvent(int index)
        {
            RemoveTrigger(triggers[index]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void RemoveTrigger(event_t e)
        {
            if (e.EventStateProp != EventState.Ended)
            {
                // To make
            }

            using (LockDownloadList(true))
            {
                triggers.Remove(e);
            }

            OnTriggerRemoved(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnTriggerRemoved(event_t e)
        {
            if (TriggerRemoved != null)
            {
                TriggerRemoved(this, new TriggerInfoEventArgs(e));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="autostart"></param>
        /// <returns></returns>
        public UDPmsg_t Add(UDPmsg_t msg , bool autostart)
        {
            List<event_t> eventCollection =
                UDPmsg_t_Provider.GetEventsByUDPmsg(msg);

            foreach (event_t var in eventCollection)
            {
                // Add all event one by one 
                Add(var, autostart);
            }
             
             //return object 

            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="trigger"></param>
        /// <param name="autoStart"></param>
        public void Add(event_t trigger, bool autoStart)
        {
            trigger.StateChanged += new EventHandler(trigger_StateChanged);

            using (LockDownloadList(true))
            {
                triggers.Add(trigger);
            }

            OnTriggerAdded(trigger, autoStart);

            if (autoStart)
            {
                trigger.Start();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="d"></param>
        /// <param name="willStart"></param>
        protected virtual void OnTriggerAdded(event_t d, bool willStart)
        {
            if (TriggerAdded != null)
            {
                TriggerAdded(this, new TriggerInfoEventArgs(d, willStart));
            }
        }


        public void PauseAll()
        {
            using (LockDownloadList(false))
            {
                for (int i = 0; i < this.triggers.Count; i++)
                {
                    this.triggers[i].Ended();
                }
            }
        }


        #endregion 
    }
}
