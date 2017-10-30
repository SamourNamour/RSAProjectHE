
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: EpgEntry.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

// Custom Directive(s)
using MTV.Scheduler.App;
using MTV.Scheduler.App.UI;
using MTV.Scheduler.App.SingleInstancing;
//using CMEScheduler.Extension.WindowsIntegration;
//using CMEScheduler.Extension.SocketServerExtension;
//using CMEScheduler.Extension.LogCleanerExtension;
//using CMEScheduler.Extension.EventCleanerExtension;
//using CMEScheduler.Extension.WatcherServiceExtension;
//using CMEScheduler.Extension.DummyCmdExtension;
//using CMEScheduler.Extension.PosterExtension;
//using CMEScheduler.Extension.AdvManagementExtension;
//using CMEScheduler.Extension.DuplexCallBackNotificationExtension;
//using CMEScheduler.Extension.MemoryManagement;
//using CMEScheduler.Extension.DataCastExtension.DataCastCommandScheduleExtension;
//using CMEScheduler.Extension.ExtendedAdvertismentExtension;
using MTV.Library.Core.UI;
using MTV.Library.Core.Extensions;
using MTV.Library.Core.Core.Extensions;
using MTV.EventDispatcher.Service.Extensions.DummyCmdExtension;
using MTV.Scheduler.App.MTV.EventDispatcher.Service.Extensions.DataCastCommandScheduleExtension;
using MTV.EventDispatcher.Service.Extensions.PosterExtension;
using MTV.Scheduler.App.MTV.EventDispatcher.Service.Extensions.MemoryManagement;
using System.Threading;

#endregion 

namespace MTV.Scheduler.App
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class App : IApp {
        #region - Singleton -
        private static App objApp = new App();
        
        private App() {
            AppManager.Instance.Initialize(this);
            extensions = new List<IExtension>();
            extensions.Add(new DummyCommandIntegrationExtension());
            extensions.Add(new DataCastCommandScheduleIntegrationExtension());
            extensions.Add(new PosterIntegrationExtension());
            extensions.Add(new MemoryManagementExtension());
                      
        }

        /// <summary>
        /// 
        /// </summary>
        public static App Instance {
            get {
                return objApp;
            }
        }
        #endregion

        #region - Field(s) -
        private bool disposed = false;
        private List<IExtension> extensions;
        private SingleInstanceTracker tracker = null;
        #endregion

        #region - Property(ies) -
        /// <summary>
        /// 
        /// </summary>
        public Form MainForm {
            get {
                return (MainForm)tracker.Enforcer;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<IExtension> Extensions {
            get {
                return extensions;
            }
        } 
        #endregion

        #region - Private Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        ISingleInstanceEnforcer GetSingleInstanceEnforcer() {
            return new MainForm();
        }
        #endregion

        #region - Public Method(s) -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IExtension GetExtensionByType(Type type) {
            for (int i = 0; i < this.extensions.Count; i++) {
                if (this.extensions[i].GetType() == type) {
                    return this.extensions[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitExtensions() {
            for (int i = 0; i < Extensions.Count; i++) {
                if (Extensions[i] is IInitializable) {
                    ((IInitializable)Extensions[i]).Init();
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            if (!disposed) {
                disposed = true;
                for (int i = 0; i < Extensions.Count; i++) {
                    if (Extensions[i] is IDisposable) {
                        try {
                            ((IDisposable)Extensions[i]).Dispose();
                        }
                        catch (Exception ex) {
                            Debug.WriteLine(ex.ToString());
                        }
                    }
                }

               
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public void Start(string[] args) {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);

            try {
                // Attempt to create a tracker
                tracker = new SingleInstanceTracker("SingleInstanceSample", new SingleInstanceEnforcerRetriever(GetSingleInstanceEnforcer));

                // If this is the first instance of the application, run the main form
                if (tracker.IsFirstInstance) {
                    try {
                        MainForm form = (MainForm)tracker.Enforcer;



                        if (Array.IndexOf<string>(args, "/as") >= 0) {
                            form.WindowState = FormWindowState.Minimized;
                        }

                        form.Load += delegate(object sender, EventArgs e) {
                            InitExtensions();

                            if (form.WindowState == FormWindowState.Minimized) {
                                form.HideForm();
                            }

                            if (args.Length > 0) {
                                form.OnMessageReceived(new MessageEventArgs(args));
                            }
                        };

                        form.FormClosing += delegate(object sender, FormClosingEventArgs e) {
                            Dispose();
                            
                        };

                        Application.Run(form);
                    }
                    finally {
                        Dispose();
                    }
                }
                else {
                    // This is not the first instance of the application, so do nothing but send a message to the first instance
                    if (args.Length > 0) {
                        tracker.SendMessageToFirstInstance(args);
                    }
                }
            }
            catch (SingleInstancingException ex) {
                MessageBox.Show("Could not create a Instance object:\n"  + "\nApplication will now terminate.\n");

                return;
            }
            finally {
                if (tracker != null)
                    tracker.Dispose();
            }
        }

        
        private static int _ReportedExceptionCount = 0;
        private static ErrorReporting _ER = null;
        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
              
                
                    if (_ReportedExceptionCount == 0 && e.Exception != null && e.Exception.Message != null && e.Exception.Message.ToString().Trim() != "")
                    {
                        if (_ER == null)
                        {
                            
                            _ER = new ErrorReporting();
                            _ER.UnhandledException = e.Exception;
                            _ER.ShowDialog();
                            _ER.Dispose();
                            _ER = null;
                            _ReportedExceptionCount++;
                        }

                    }
                
               // MainForm.LogExceptionToFile(e.Exception);
            }
            catch (Exception ex2)
            {
                // MainForm.LogExceptionToFile(ex2);
            }
        }


        #endregion

    }
}
