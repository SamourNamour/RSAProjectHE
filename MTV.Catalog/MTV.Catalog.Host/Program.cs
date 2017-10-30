using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using log4net.Config;
using System.Reflection;
using MTV.Catalog.Common;
using MTV.Catalog.Service.IOC;
using System.Windows.Forms;
using System.IO;

namespace MTV.Catalog.Host
{
    static class Program
    {

        private static string _apppath = "", _appdatapath = "";
        public static string AppPath
        {
            get
            {
                if (_apppath != "")
                    return _apppath;
                _apppath = (Application.StartupPath.ToLower());
                _apppath = _apppath.Replace(@"\bin\debug", @"\").Replace(@"\bin\release", @"\");
                _apppath = _apppath.Replace(@"\bin\x86\debug", @"\").Replace(@"\bin\x86\release", @"\");

                _apppath = _apppath.Replace(@"\\", @"\");

                if (!_apppath.EndsWith(@"\"))
                    _apppath += @"\";
                Directory.SetCurrentDirectory(_apppath);
                return _apppath;
            }
        }

        public static string AppDataPath
        {
            get
            {
                if (_appdatapath != "")
                    return _appdatapath;
                _appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MTV.Catalog.Host\";
                return _appdatapath;
            }
        }
        
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {


            XmlConfigurator.Configure();
            IOCatalogManager.Instance.Container.Install(new StandardIOCatalogInstaller());



#if (!DEBUG)
            try
            {

                LogManager.Log.InfoFormat("Initialising MTV.Catalog.Service service in assembly {0} RELEASE windows service mode.", Assembly.GetExecutingAssembly().FullName);
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { new Service() };
                ServiceBase.Run(ServicesToRun);
            }
            catch (Exception e)
            {
                LogManager.Log.Error(e);
            }
#else
            try
            {


                bool ei = (!Directory.Exists(Program.AppDataPath));
                if (ei)
                {
                    LogManager.Log.InfoFormat("Ensure Install...");
                    EnsureInstall(true);
                }
                LogManager.Log.InfoFormat("Initialising mebs.Gateway service in assembly {0} DEBUG console mode.", Assembly.GetExecutingAssembly().FullName);
                LogManager.Log.Info("Starting Service");
                Service.StartService();
                LogManager.Log.Info("Service Started");


                System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);



            }
            catch (Exception e)
            {
                LogManager.Log.Error(e);
            }
#endif


        }


        // Insure Install
        public static void EnsureInstall(bool reset)
        {

            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }

            Directory.SetCurrentDirectory(AppPath);

            //reset layout position
            //Registry.CurrentUser.DeleteSubKey(@"Software\mebsScheduler\startup", false);

        }


      

    }
}
