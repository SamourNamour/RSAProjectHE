
#region - Copyright Motive Television 2014 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: Program.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data.Linq;

// Custom Directive(s)
using MTV.Scheduler.App.UI;
using System.IO;
using Microsoft.Win32;
using System.ServiceProcess;
using MTV.Scheduler.App.MTVControl;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.SqlServerCe;
#endregion 

namespace MTV.Scheduler.App
{
    enum ProviderType
    {
        
        MySql
    }
    
    /// <summary>
    /// 
    /// </summary>
    class  Program {


        public static Mutex Mutex;
        private static ProviderType _provider = ProviderType.MySql;
        private static string _apppath = "", _appdatapath = "";
        static ServiceController _eventDequeuersrv = new ServiceController("MTV.EventDequeuer.Host");
        static ServiceController _catalogsrv = new ServiceController("MTV.Catalog.Host");
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
                _appdatapath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\MTV.Scheduler.App\";
                return _appdatapath;
            }
        }


        public static string ExecutableDirectory = "";
        public static Mutex WriterMutex;
        private static int _reportedExceptionCount;
       



        #region - Entry Point(s) - 
        [STAThread]
        private static void Main(string[] args)
        {
           

            //uninstall?
            string[] arguments = Environment.GetCommandLineArgs();

            foreach (string argument in arguments)
            {
                if (argument.Split('=')[0].ToLower() == "/u")
                {
                    string guid = argument.Split('=')[1];
                    string path = Environment.GetFolderPath(Environment.SpecialFolder.System);
                    UnInstallMTVServices();
                    var si = new ProcessStartInfo(path + "/msiexec.exe", "/x " + guid);
                    Process.Start(si);
                    Application.Exit();
                    return;
                }
            }


            //Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            // AppDomain.CurrentDomain.ProcessExit += new EventHandler(CurrentDomain_ProcessExit);

            string executableName = Application.ExecutablePath;
            var executableFileInfo = new FileInfo(executableName);
            ExecutableDirectory = executableFileInfo.DirectoryName;

            bool ei = (!Directory.Exists(Program.AppDataPath) || !Directory.Exists(Program.AppDataPath + @"XML\") ||
           !File.Exists(Program.AppDataPath + @"XML\config.xml"));

            if (ei)
            {
                EnsureInstall(true);
            }
            else
            {
                if (!Directory.Exists(AppDataPath + @"Templates"))
                {
                    Directory.CreateDirectory(AppDataPath + @"Templates");

                    var didest = new DirectoryInfo(AppDataPath + @"Templates");
                    var disource = new DirectoryInfo(AppPath + @"Templates");
                    CopyAll(disource, didest);

                }

               
            }

            string command = "";
            if (args.Length > 0)
            {
               
                if (args[0].ToLower().Trim() == "-reset" && !ei)
                {
                    if (MessageBox.Show("Reset MTV.Scheduler.App ? This will overwrite all your settings.", "Confirm", MessageBoxButtons.OKCancel) == DialogResult.OK)
                        EnsureInstall(true);
                }

            }


            

            
            //Check if mysql is installed / Running.
            var w = Process.GetProcessesByName("mysqld");
            if (w.Length == 0)
            {
                MessageBox.Show("Mysql Engine is not Running OR Installed On your system. you will need to manually Start OR Install it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //---- check if database needs to be created ----         
            int currentSchemaVersion = GetCurrentShemaVersion();          
            if (currentSchemaVersion <= 514) //  handle -1
            {
                if (currentSchemaVersion < 0)
                {

                    if (!ExecuteSQLScript("create"))
                    {
                        MessageBox.Show("Failed to create the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                        
                    }
                }
                
                
            }

           
            
            // BEGIN CHECK : Microsoft Visual C++ 2008 SP1 Redistributable Package (x86). 

            //Checking for the availability of VC++ runtimes..

            //if (!VCRuntimeDetector.IsVC2008SP1Installed_x86())
            //{
            //    string warn = string.Format("The component Microsoft Visual C++ 2008 SP1 Redistributable Package (x86) Not available on your system, you will need to manually download and install. Please Visit http://www.microsoft.com/en-us/download/confirmation.aspx?id=5582");
            //    MessageBox.Show(warn, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    return;
            //}
          

            // END.

            // BEGIN CHECK : "dc_redundancy_generator.exe" .   
            
            if (!File.Exists(Program.AppPath + @"dc_redundancy_generator.exe"))
            {
                string m = string.Format("Could not load dc_redundancy_generator.exe - you will need to manually copy the file to : {0}", Program.AppPath);
                MessageBox.Show(m, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;           
            }

            // END.

            //to add in future //Check if MTV.Scheduler.APP.sdf need to be created.
            //if (!File.Exists(Program.AppDataPath + @"MTV.Scheduler.APP.sdf"))
            //{
            //    CreateMTVSchedulerAPPSdf();
            //}


            

#if (!DEBUG)
           
            WindowsServiceManager SM = new WindowsServiceManager();

          
            if (!SM.IsServiceInstalled("MTV.EventDequeuer.Host"))
            {
                if (!SM.InstallService(AppPath + "/MTV.EventDequeuer.Host/MTV.EventDequeuer.Host.exe -service",
                "MTV.EventDequeuer.Host", "MTV.EventDequeuer.Host Service"))
                {
                    MainForm.LogErrorToFile("MTV.EventDequeuer.Host Service install failed.");
                }
            }

            if (!SM.IsServiceInstalled("MTV.Catalog.Host"))
            {
                if (!SM.InstallService(AppPath + "/MTV.Catalog.Host/MTV.Catalog.Host.exe -service",
                "MTV.Catalog.Host", "MTV.Catalog.Host Service"))
                {
                    MainForm.LogErrorToFile("MTV.Catalog.Host Service install failed.");
                }
            }
            
#else



            try
            {
                ShutDownMTVServices();
            }
            catch (Exception ex)
            {

                MainForm.LogExceptionToFile(ex);


            }
#endif

            
            App.Instance.Start(args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void CurrentDomain_ProcessExit(object sender, EventArgs e) {
            EventLog.WriteEntry("CurrentDomain_ProcessExit",
                                "MTV.Scheduler.App AppDomain Process has been exited.",
                                EventLogEntryType.Error);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        static void Application_ApplicationExit(object sender, EventArgs e) {
            EventLog.WriteEntry("Application_ApplicationExit",
                                "MTV.Scheduler.App Application has been exited.",
                                EventLogEntryType.Error);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="reset"></param>
        public static void EnsureInstall(bool reset)
        {

            if (!Directory.Exists(AppDataPath))
            {
                Directory.CreateDirectory(AppDataPath);
            }
            if (!Directory.Exists(AppDataPath + @"XML"))
            {
                Directory.CreateDirectory(AppDataPath + @"XML");
            }



            var didest = new DirectoryInfo(AppDataPath + @"XML\");
            var disource = new DirectoryInfo(AppPath + @"XML\");


            TryCopy(disource + @"Translations.xml", didest + @"Translations.xml", true);



            if (reset || !File.Exists(didest + @"config.xml"))
            {
                TryCopy(disource + @"config.xml", didest + @"config.xml", reset);
            }


            if (!Directory.Exists(AppDataPath + @"Templates"))
            {
                Directory.CreateDirectory(AppDataPath + @"Templates");
            }

            didest = new DirectoryInfo(AppDataPath + @"Templates");
            disource = new DirectoryInfo(AppPath + @"Templates");
            CopyAll(disource, didest);


            if (!Directory.Exists(AppDataPath + @"WebServerRoot"))
            {
                Directory.CreateDirectory(AppDataPath + @"WebServerRoot");
            }
            didest = new DirectoryInfo(AppDataPath + @"WebServerRoot");
            disource = new DirectoryInfo(AppPath + @"WebServerRoot");
            CopyAll(disource, didest);

            if (!Directory.Exists(AppDataPath + @"WebServerRoot\Media\Audio"))
                Directory.CreateDirectory(AppDataPath + @"WebServerRoot\Media\Audio");
            if (!Directory.Exists(AppDataPath + @"WebServerRoot\Media\Video"))
                Directory.CreateDirectory(AppDataPath + @"WebServerRoot\Media\Video");

            Directory.SetCurrentDirectory(AppPath);

            
            Registry.CurrentUser.DeleteSubKey(@"Software\MTV.Scheduler.App\startup", false);

        }


        private static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            // Check if the target directory exists, if not, create it.
            if (Directory.Exists(target.FullName) == false)
            {
                Directory.CreateDirectory(target.FullName);
            }

            // Copy each file into it’s new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                
                try { fi.CopyTo(Path.Combine(target.ToString(), fi.Name), true); }
                catch
                {
                }
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }

        /// <summary>
        /// Try Copy 
        /// </summary>
        /// <param name="source">source files</param>
        /// <param name="target">target directory</param>
        /// <param name="overwrite">Overwrite True or false</param>
        private static void TryCopy(string source, string target, bool overwrite)
        {
            try
            {
                File.Copy(source, target, overwrite);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Gets the current schema version (-1= No database installed)
        /// </summary>
        /// <returns>the current schema version</returns>
        private static int GetCurrentShemaVersion()
        {
            int currentSchemaVersion = -1;
            string usingSettingName = "BroadcasterID";
            try
            {
                string connectionString = ComposeConnectionString("localhost", "root", "televisio", "sa_mebs", true, 300);
                
                using (MySqlConnection connect = new MySqlConnection(connectionString))
                {
                    connect.Open();
                    using (MySqlCommand cmd = connect.CreateCommand())
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "select * from mebs_settings";
                        using (IDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                               
                                if (usingSettingName == (string)reader["SettingName"])
                                {
                                    currentSchemaVersion = Convert.ToInt32(reader["SettingValue"].ToString());
                                    break; 
                                }                                   
                               
                            }

                            reader.Close();
                            connect.Close();
                        }
                    }
                }

                return currentSchemaVersion;

            }
            catch (Exception ex)
            {
               
                return -1;
            }

            finally
            {
                
                GC.Collect();
            }
        }

        
        /// <summary>
        ///  Execute sql script (db creation)
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static bool ExecuteSQLScript(string prefix)
        {
            bool succeeded = true;

            try
            {
                Assembly assm = Assembly.GetExecutingAssembly();
                string[] names = assm.GetManifestResourceNames();
                Stream stream = null;
                stream = assm.GetManifestResourceStream("MTV.Scheduler.App." + prefix + "_sa_mebs_database.sql");
             
                string sql = string.Empty;
                using (StreamReader reader = new StreamReader(stream))
                    sql = reader.ReadToEnd();
                                 
               
                string connectionString = ComposeConnectionString("localhost", "root", "televisio", "", true, 300);
                MySqlConnection myConn = new MySqlConnection(connectionString);
                MySqlScript script = new MySqlScript(myConn, sql);
                script.Execute();
               
            }
            catch (Exception gex)
            {
                MessageBox.Show("Unable to " + prefix + " database:" + gex.Message);
                MainForm.LogErrorToFile("Unable to " + prefix + " database:" + gex.Message);
                succeeded = false;
            }
            
            return succeeded;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        private static string[] CleanMySqlStatement(string sql)
        {
            sql = sql.Replace("\r\n", "\r");
            sql = sql.Replace("\t", " ");
            sql = sql.Replace('"', '`'); // allow usage of ANSI quoted identifiers
            
            string[] lines = sql.Split('\r');
            sql = "";
            for (int i = 0; i < lines.Length; ++i)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("/*")) continue;
                if (line.StartsWith("--")) continue;
                if (line.Length == 0) continue;
                sql += line;
            }
            return sql.Split('#');
        }

        private static string ComposeConnectionString(string server, string userid, string password, string database, bool pooling, int timeout)
        {
            
            switch (_provider)
            {
                
                case ProviderType.MySql:
                    if (database == "") database = "mysql";
                    return String.Format("Server={0};Database={3};User ID={1};Password={2};charset=utf8;Connection Timeout={4};", server, userid, password, database, timeout);
            }
            return "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool CreateMTVSchedulerAPPSdf()
        {
             string connectionString;
             string fileName = Program.AppDataPath + @"MTV.Scheduler.APP.sdf";
             string password = "televisio";           
             connectionString = string.Format("DataSource=\"{0}\"; Password='{1}'", fileName, password);

             try
             {
                 SqlCeEngine en = new SqlCeEngine(connectionString);
                 en.CreateDatabase();
                 return true;
             }
             catch (Exception ex)
             {
                 MainForm.LogExceptionToFile(ex);
                 return false;
             }
            
           
        }

        /// <summary>
        ///  UnInstall the MTV services
        /// </summary>
        public static void UnInstallMTVServices()
        {
            WindowsServiceManager WSM = new WindowsServiceManager();
            WSM.UnInstallService("MTV.EventDequeuer.Host");
            WSM.UnInstallService("MTV.Catalog.Host");
        }

        /// <summary>
        /// Shut down the MTV services
        /// </summary>
        public static void ShutDownMTVServices()
        {
            // Stop Vista & XP services
            bool eventDequeuerExist = false;
            bool catalogExist = false;
            bool success = true;

            // Check for existance of MTV services without throwing/catching exceptions
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController srv in services)
            {

                if (srv.ServiceName == "MTV.EventDequeuer.Host")
                {
                    eventDequeuerExist = true;
                    if (catalogExist) //Found both services
                        break;
                }
                if (srv.ServiceName == "MTV.Catalog.Host")
                {
                    catalogExist = true;
                    if (eventDequeuerExist) //Found both services
                        break;
                }

            }

            // Stop MTV.EventDequeuer.Host and MTV.Catalog.Host services


            if ((catalogExist && (_eventDequeuersrv.Status != ServiceControllerStatus.Stopped) && (_eventDequeuersrv.Status != ServiceControllerStatus.StopPending))
         || (catalogExist && (_catalogsrv.Status != ServiceControllerStatus.Stopped) && (_catalogsrv.Status != ServiceControllerStatus.StopPending)))
            {
                MainForm.LogMessageToFile("Stopping MTV services");
                try
                {
                    if ((_eventDequeuersrv.Status != ServiceControllerStatus.Stopped) && (_eventDequeuersrv.Status != ServiceControllerStatus.StopPending))
                    {
                        _eventDequeuersrv.Stop();
                        // _restartMCEehRecvr = true;
                    }
                }
                catch
                {
                    success = false;
                    MainForm.LogErrorToFile("Error stopping MTV services \"MTV.EventDequeuer.Host\"");
                }
                try
                {
                    if ((_catalogsrv.Status != ServiceControllerStatus.Stopped) && (_catalogsrv.Status != ServiceControllerStatus.StopPending))
                    {
                        _catalogsrv.Stop();
                        // _restartMCEehSched = true;
                    }
                }
                catch
                {
                    success = false;
                    MainForm.LogErrorToFile("Error stopping MTV service \"MTV.Catalog.Host services\"");
                }
            }

            if (success)
            {
            }
            else
                MainForm.LogErrorToFile("!!! MTV.Scheduler.APP needs to be run as Administrator to stop the MTV services !!!");


        }
        
        #endregion

       
    }
}
