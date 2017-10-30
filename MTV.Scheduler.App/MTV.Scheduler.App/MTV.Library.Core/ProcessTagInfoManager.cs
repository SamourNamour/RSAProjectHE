using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.Library.Core.Concurrency;
using System.Collections.ObjectModel;

namespace MTV.Library.Core
{
    public class ProcessTagInfoManager
    {
        #region Singleton

        private static ProcessTagInfoManager instance = new ProcessTagInfoManager();

        public static ProcessTagInfoManager Instance
        {
            get
            {
                return instance;
            }
        }

        #endregion


        private List<ProcessTagInfo> processList = new List<ProcessTagInfo>();
        private ReaderWriterObjectLocker processListSync = new ReaderWriterObjectLocker();


        public event EventHandler<ProcessEventArgs> ProcessEnded;
        public event EventHandler<ProcessEventArgs> ProcessAdded;
        public event EventHandler<ProcessEventArgs> ProcessRemoved;
        public event EventHandler<ProcessEventArgs> ProcessFailed;
        public event EventHandler<ProcessEventArgs> ProcessPosterDownloadError;
        public event EventHandler<ProcessEventArgs> ProcessPosterDownloadEnded;


        public ReadOnlyCollection<ProcessTagInfo> AllProcess
        {
            get
            {
                return processList.AsReadOnly();
            }
        }

        protected virtual void OnProcessPosterDownloadEnded(ProcessTagInfo p)
        {
            if (ProcessPosterDownloadEnded != null)
            {
                ProcessPosterDownloadEnded(this, new ProcessEventArgs(p));
            }
        }

        protected virtual void OnProcessPosterDownloadError(ProcessTagInfo p)
        {
            if (ProcessPosterDownloadError != null)
            {
                ProcessPosterDownloadError(this, new ProcessEventArgs(p));
            }
        }

        void processTagInfo_StateChanged(object sender, EventArgs e)
        {
            ProcessTagInfo p = (ProcessTagInfo)sender;

            if (p.StateField == ProcessStatus.Ended ) 
                
            {
                OnProcessTagInfoEnded((ProcessTagInfo)sender);
            }

            if (p.StateField == ProcessStatus.Failed)
            {
                OnProcessTagInfoFailed((ProcessTagInfo)sender);
            }

            if (p.StateField == ProcessStatus.PosterDownloadError)
            {
                OnProcessPosterDownloadError((ProcessTagInfo)sender);
            }

            if (p.StateField == ProcessStatus.PosterDownloadSuccess)
            {
                OnProcessPosterDownloadEnded((ProcessTagInfo)sender);
            }

        }

        protected virtual void OnProcessTagInfoRemoved(ProcessTagInfo p)
        {
            if (ProcessRemoved != null)
            {
                ProcessRemoved(this, new ProcessEventArgs(p));
            }
        }

        protected virtual void OnProcessTagInfoAdded(ProcessTagInfo p, bool willStart)
        {
            if (ProcessAdded != null)
            {
                ProcessAdded(this, new ProcessEventArgs(p, willStart));
            }
        }

        protected virtual void OnProcessTagInfoEnded(ProcessTagInfo d)
        {
            if (ProcessEnded != null)
            {
                ProcessEnded(this, new ProcessEventArgs(d));
            }
        }
        
        protected virtual void OnProcessTagInfoFailed(ProcessTagInfo d)
        {
            if (ProcessFailed != null)
            {
                ProcessFailed(this, new ProcessEventArgs(d));
            }
        }


        public IDisposable LockProcessTagInfoList(bool lockForWrite)
        {
            if (lockForWrite)
            {
                return processListSync.LockForWrite();
            }
            else
            {
                return processListSync.LockForRead();
            }
        }

        public void Add(ProcessTagInfo p, bool autoStart)
        {
            p.StateChanged += new EventHandler(processTagInfo_StateChanged);

            using (LockProcessTagInfoList(true))
            {
                processList.Add(p);
            }

            OnProcessTagInfoAdded(p, autoStart);

            if (autoStart)
            {
                p.Start();
            }
        }

        public void ClearEnded()
        {
            using (LockProcessTagInfoList(true))
            {
                for (int i = processList.Count - 1; i >= 0; i--)
                {
                    if (processList[i].StateField == ProcessStatus.Ended || processList[i].StateField != ProcessStatus.Failed)
                    {
                        ProcessTagInfo p = processList[i];
                        processList.RemoveAt(i);
                        OnProcessTagInfoRemoved(p);
                    }
                }
            }
        }


        public void PauseAll()
        {
            using (LockProcessTagInfoList(false))
            {
                for (int i = 0; i < this.AllProcess.Count; i++)
                {
                    this.AllProcess[i].ForceEnd();
                }
            }
        }

        public void RemoveProcess(int index)
        {
            RemoveProcess(processList[index]);
        }

        public void RemoveProcess(ProcessTagInfo p)
        {
            if (p.StateField != ProcessStatus.Ended || p.StateField != ProcessStatus.Failed)
            {
                p.End();
            }

            using (LockProcessTagInfoList(true))
            {
                processList.Remove(p);
            }

            OnProcessTagInfoRemoved(p);
        }

       





    }
}
