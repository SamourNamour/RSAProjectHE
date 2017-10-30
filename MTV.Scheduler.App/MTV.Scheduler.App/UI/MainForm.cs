
#region - Copyright Motive Television 2014 -

// Filename: MainForm.cs

#endregion

#region - Using Directive(s) -
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.Web;
using System.Diagnostics;
using Timer = System.Timers.Timer;
// Custom Directive(s)
using MTV.Scheduler.App.SingleInstancing;
//using Logger.HELogger;
using MTV.Scheduler.App.Properties;
using System.Threading;
using System.Net;
using System.Xml.Serialization;
using System.Net.NetworkInformation;
using System.Timers;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using MTV.Scheduler.App.MTVControl;
using System.Linq;
using Microsoft.Win32;
using MTV.Library.Core;
using MTV.Scheduler.App.HttpServer;
using MTV.EventDispatcher.Service.Extensions.DummyCmdExtension;
using MTV.Scheduler.App.MTV.EventDispatcher.Service.Extensions.DataCastCommandScheduleExtension;
using MTVPRO = MTV.Scheduler.App.Properties;
using MTV.Library.Core.Data;
using System.Xml;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Scheduler.App.MTV.Library.Core;
#endregion 

namespace MTV.Scheduler.App.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class MainForm : Form, ISingleInstanceEnforcer {


        public static string NextLog = "";
        private static bool _logging;
        private static readonly StringBuilder LogFile = new StringBuilder();
        private static readonly string LogTemplate =
           "<html><head><title>MTV.Scheduler.App Log File</title><style type=\"text/css\">body,td,th,div {font-family:Verdana;font-size:10px}</style></head><body><h1>Log Start: " +
           DateTime.Now.ToLongDateString() +
           "</h1><p><table cellpadding=\"2px\"><!--CONTENT--></table></p></body></html>";
        private PerformanceCounter _cpuCounter, _cputotalCounter, _pcMem;
        private static bool _pcMemAvailable;
        private Timer _houseKeepingTimer;
        private Timer _pollTimer;
        private Timer _enqueuingVodTimer;
        public static int CpuUsage, CpuTotal;
        private static string _counters = "";
        private static int _pingCounter;
        private static int _storageCounter;
        public static double ThrottleFramerate = 40;
        public bool reallyclose = false;
        private static configuration _conf;
        public static string Identifier;
        internal static LocalServer MWS;
        private bool _shuttingDown;
        private bool _closing;
        private static Timer RescanIPTimer;
        private static IPAddress[] _ipv4Addresses, _ipv6Addresses;
        private static string _lastlog = "";
        private static string _browser = String.Empty; // Default browser to use .
        public static string Website = "http://www.motivetelevision.co.uk/";
        public static string MAMWebApp = "http://localhost/MEBSMAM/";
        private Thread StorageThread;
        internal static WindowsServiceManager SM;
        private static EventDequeuerWatchManager obj;
        bool _mysqServerStat = false;
        bool _eventDequeuerStat = false;
        bool _catalogHostStat = false;
        bool _smtpServerStat = false;

        public event EventHandler<MysqlServerEventArgs> RaiseMysqlServerEvent;               //declare Mysql event
        public event EventHandler<DequeuerServiceEventArgs> RaiseDequeuerServiceEvent;      //declare Dequeuer event
        public event EventHandler<CatalogServiceEventArgs> RaiseCatalogServiceEvent;       // declare Catalog event
        //public event EventHandler<SmtpServerEventArgs> RaiseSmtpServerEvent;  
        
        #region - ISingleInstanceEnforcer Members -
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnMessageReceived(MessageEventArgs e) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void OnNewInstanceCreated(EventArgs e) {
            this.Focus();
        }

        #endregion
        
        /// <summary>
        /// 
        /// </summary>
        public static configuration Conf
        {
            get
            {
                if (_conf != null)
                    return _conf;
                var s = new XmlSerializer(typeof(configuration));
                bool loaded = false;

                using (var fs = new FileStream(Program.AppDataPath + @"XML\config.xml", FileMode.Open))
                {
                    try
                    {
                        using (TextReader reader = new StreamReader(fs))
                        {
                            fs.Position = 0;
                            _conf = (configuration)s.Deserialize(reader);
                            reader.Close();
                            loaded = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        LogExceptionToFile(ex);
                    }
                    fs.Close();
                }

                if (!loaded)
                {
                   
                    try
                    {
                        var didest = new DirectoryInfo(Program.AppDataPath + @"XML\");
                        var disource = new DirectoryInfo(Program.AppPath + @"XML\");
                        File.Copy(disource + @"config.xml", didest + @"config.xml", true);


                        using (var fs = new FileStream(Program.AppDataPath + @"XML\config.xml", FileMode.Open))
                        {
                            fs.Position = 0;
                            using (TextReader reader = new StreamReader(fs))
                            {
                                _conf = (configuration)s.Deserialize(reader);
                                reader.Close();
                            }
                            fs.Close();
                        }
                    }
                    catch (Exception ex2)
                    {
                        string m =
                            "Could not load or restore configuration - you will need to manually copy the file /program files/MTV.Scheduler.App/XML/config.xml to  /users/<username>/appdata/roaming/MTV.Scheduler.App/xml: " +
                            ex2.Message;
                        MessageBox.Show(m);
                        LogMessageToFile(m);
                        throw;
                    }
                }


                return _conf;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private static string Zeropad(int i)
        {
            if (i > 9)
                return i.ToString();
            return "0" + i;
        }
       

        #region - Constructor(s) / Finalizer(s) -
        /// <summary>
        ///  Constructor
        /// </summary>
        public MainForm() {
            
            InitializeComponent();
            
            RaiseMysqlServerEvent       += new EventHandler<MysqlServerEventArgs>(MainForm_RaiseMysqlServerEvent);
            RaiseDequeuerServiceEvent   += new EventHandler<DequeuerServiceEventArgs>(MainForm_RaiseDequeuerServiceEvent);
            RaiseCatalogServiceEvent    += new EventHandler<CatalogServiceEventArgs>(MainForm_RaiseCatalogServiceEvent);
            //RaiseSmtpServerEvent        += new EventHandler<SmtpServerEventArgs>(MainForm_RaiseSmtpServerEvent); 
            
            SetPriority();
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailMessage"></param>
        public void SendEmailInBackgroundThread(string mailMessage)
        {
            Thread bgThread = new Thread(new ParameterizedThreadStart(SendEmail));
            bgThread.IsBackground = true;
            bgThread.Start(mailMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mailMsg"></param>
        public void SendEmail(Object mailMsg)
        {
            string mailMessage = (string)mailMsg;

            //if (TestSmtpConnection(MainForm.Conf.SMTPServer, MainForm.Conf.SMTPPort))
            //{

                // Build Alert body
                string msgBody = mailMessage;

                // Send Alert
                if (SendAlertV2("icherki@motivetelevision.co.uk,kgoubar@motivetelevision.co.uk", "Alert System", msgBody))
                {
                    //Alert sent success
                     UISync.Execute(() =>mainGuiUC.MonitroingSystemBoxAddMsg("Alert Sent ..... [OK]", LogMode.Information));
                }
                else
                {
                    // Failed to Send Alert
                     UISync.Execute(() =>mainGuiUC.MonitroingSystemBoxAddMsg("Alert Failed ..... [SeeLogFile]", LogMode.Error));
                }


            //}
            //else
            //{
            //    //smtp server is not responding
            //    UISync.Execute(() =>mainGuiUC.MonitroingSystemBoxAddMsg("SMTP Server is not responding, Alert process Aborted ..... [SeeLogFile]", LogMode.Error));
            //}

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_RaiseSmtpServerEvent(object sender, SmtpServerEventArgs e)
        {
            if (e.SmtpServerStatus)
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("smtp server OK, Checked on [{0}]", DateTime.UtcNow.ToString()), LogMode.Information);
            }
            else
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("smtp server is not responding, Checked on [{0}]", DateTime.UtcNow.ToString()), LogMode.Error);
               
            }

        }
        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_RaiseMysqlServerEvent(object sender, MysqlServerEventArgs e)
        {
            if (e.MysqlServerStatus)
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MySQL engine is Up & Running, Checked on [{0}]",DateTime.UtcNow.ToString()), LogMode.Information);
            }
            else
            {
                 mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MySQL server is not running Checked on [{0}], Sending an Alert to Motive televison Support Team...", DateTime.UtcNow.ToString()), LogMode.Error);

                 SendEmailInBackgroundThread("MySQL server is not running");
         
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_RaiseDequeuerServiceEvent(object sender, DequeuerServiceEventArgs e)
        {
            if (e.DequeuerServiceStatus)
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MTV.EventDequeuer.Host is Up & Running, Checked on [{0}]", DateTime.UtcNow.ToString()), LogMode.Information);
            }
            else
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MTV.EventDequeuer.Host is not running, Checked on [{0}], Sending an Alert to Motive televison Support Team...", DateTime.UtcNow.ToString()), LogMode.Error);
                // Sending an Alert

                SendEmailInBackgroundThread("MTV.EventDequeuer.Host is not running");
                
                
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_RaiseCatalogServiceEvent(object sender, CatalogServiceEventArgs e)
        {
            if (e.CatalogServiceStatus)
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MTV.Catalog.Host is Up & Running, Checked on [{0}]", DateTime.UtcNow.ToString()), LogMode.Information);
            }
            else
            {
                mainGuiUC.MonitroingSystemBoxAddMsg(string.Format("MTV.Catalog.Host is not running, Checked on [{0}], Sending an Alert to Motive televison...", DateTime.UtcNow.ToString()), LogMode.Error);

                SendEmailInBackgroundThread("MTV.Catalog.Host is not running");
            }

        }
        
        /// <summary>
        /// 
        /// </summary>
        public static void SetPriority()
        {
            switch (Conf.Priority)
            {
                case 1:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.Normal;
                    break;
                case 2:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.AboveNormal;
                    break;
                case 3:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.High;
                    break;
                case 4:
                    Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
                    break;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void InitLogging()
        {
            DateTime logdate = DateTime.Now;

            FileInfo fi;
            foreach (string s in Directory.GetFiles(Program.AppDataPath, "log_*", SearchOption.TopDirectoryOnly))
            {
                fi = new FileInfo(s);
                if (fi.CreationTime < DateTime.Now.AddDays(-20))
                    File.Delete(s);
            }
            NextLog = Zeropad(logdate.Day) + Zeropad(logdate.Month) + logdate.Year;
            int i = 1;
            if (File.Exists(Program.AppDataPath + "log_" + NextLog + ".htm"))
            {
                while (File.Exists(Program.AppDataPath + "log_" + NextLog + "_" + i + ".htm"))
                    i++;
                NextLog += "_" + i;
            }
            try
            {
                File.WriteAllText(Program.AppDataPath + "log_" + NextLog + ".htm", DateTime.Now + Environment.NewLine);
                _logging = true;
            }


            catch (Exception ex)
            {
                if (
                    MessageBox.Show(LocRm.GetString("LogStartError").Replace("[MESSAGE]", ex.Message),
                                    LocRm.GetString("Warning"), MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    reallyclose = true;
                    Close();
                    return;
                }
            }
        }




        #region - Private Method(s) -

       /// <summary>
       /// 
       /// </summary>
        private void WriteLog()
        {
            
            if (_logging)
            {
                try
                {
                    if (LogFile.Length > Conf.LogFileSizeKB * 1024)
                    {
                        LogFile.Append(
                            "<tr><td style=\"color:red\" valign=\"top\">Logging Exiting</td><td valign=\"top\">" +
                            DateTime.Now.ToLongTimeString() +
                            "</td><td valign=\"top\">Logging is being disabled as it has reached the maximum size (" +
                            Conf.LogFileSizeKB + "kb).</td></tr>");
                        _logging = false;
                    }
                    if (_lastlog.Length != LogFile.Length)
                    {
                        string fc = LogTemplate.Replace("<!--CONTENT-->", LogFile.ToString()).Replace("<!--VERSION-->",
                                                                                                      Application.
                                                                                                          ProductVersion);
                        File.WriteAllText(Program.AppDataPath + @"log_" + NextLog + ".htm", fc);
                        _lastlog = LogFile.ToString();
                    }
                }
                catch (Exception)
                {
                    _logging = false;
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>     
        private void ShowInfo()
        {

            OsDetection.OSVersionInfo os = new OsDetection.OperatingSystemVersion();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(Application.ExecutablePath);
            string ServicePack = string.Empty;

            if (!String.IsNullOrEmpty(os.OSCSDVersion))
                ServicePack = " (" + os.OSCSDVersion + ")";

            UISync.Execute(() => Text = string.Format("Motive Enhanced Broadcast System (MEBS)"));
            UISync.Execute(() => oslbl.Text = string.Format("Operation System : {0}", os.OSVersionString + ServicePack));
            UISync.Execute(() => productVersionlbl.Text = string.Format("Product Version : [ Phase1 ] - [ {0} ] - [ Open Beta ]", Application.ProductVersion));
            UISync.Execute(() => lblSupport.Text = string.Format("Submit a Ticket\n[Report a Problem , get technical help]"));
            
            if (Conf.UseUPNP)
            {
                if (!NATControl.SetPorts(Conf.ServerPort, Conf.LANPort))
                {
                    MessageBox.Show(LocRm.GetString("ErrorPortMapping"), LocRm.GetString("Error"));
                    
                }
            }

            try
            {
                var fw = new WinXPSP2FireWall();
                fw.Initialize();

                bool bOn = false;
                fw.IsWindowsFirewallOn(ref bOn);
                if (bOn)
                {
                    string strApplication = Application.StartupPath + "\\MTV.Scheduler.App.exe";
                    bool bEnabled = false;
                    fw.IsAppEnabled(strApplication, ref bEnabled);
                    if (!bEnabled)
                    {
                        fw.AddApplication(strApplication, "MTV.Scheduler.App");
                    }
                }
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void RunServices()
        {

#if (!DEBUG)


             //SM = new WindowsServiceManager();
       
            if (!SM.IsServiceRunning("MTV.EventDequeuer.Host"))
            {
                if (!SM.StartService("MTV.EventDequeuer.Host"))
                {
                    LogMessageToFile("MTV.EventDequeuer.Host Service starting failed.");
                    
                }
                else
                {
                   
                    LogMessageToFile("MTV.EventDequeuer.Host Service is started successfully.");
                }
               
            }
            else
                
                
                LogMessageToFile("MTV.EventDequeuer.Host Service is Online.");


            // MTV.Catalog.Host.exe Region 

            if (!SM.IsServiceRunning("MTV.Catalog.Host"))
            {
                if (!SM.StartService("MTV.Catalog.Host"))
                {
                    LogMessageToFile("MTV.Catalog.Host Service starting failed.");
                    //UISync.Execute(() => CatalogStatslbl.Text = "MTV.Catalog.Host Service is Offline");
                }
                else
                {
                    //UISync.Execute(() => CatalogStatslbl.Text = "MTV.Catalog.Host Service is Online");
                    LogMessageToFile("MTV.Catalog.Host Service is started successfully.");
                }

            }
            else

                //UISync.Execute(() => CatalogStatslbl.Text = "MTV.Catalog.Host Service is Online");
                LogMessageToFile("MTV.Catalog.Host Service is Online.");

#else


            var _wEventDequeuerHost = Process.GetProcessesByName("MTV.EventDequeuer.Host");
            if (_wEventDequeuerHost.Length == 0)
            {
                try
                {
                    var si = new ProcessStartInfo(Program.AppPath + "/MTV.EventDequeuer.Host/MTV.EventDequeuer.Host.exe", "MTV.Scheduler.app");
                    Process.Start(si);
                                        
                }
                catch (Exception _wEventDequeuerHostEx)
                {
                    LogExceptionToFile(_wEventDequeuerHostEx);
                    
                }
            }


            var _wCatalogHost = Process.GetProcessesByName("MTV.Catalog.Host");
            if (_wCatalogHost.Length == 0)
            {
                try
                {
                    var si = new ProcessStartInfo(Program.AppPath + "/MTV.Catalog.Host/MTV.Catalog.Host.exe", "MTV.MebsScheduler");
                    Process.Start(si);
                   
                }
                catch (Exception _wCatalogHostEx)
                {
                    LogExceptionToFile(_wCatalogHostEx);
                }
            }

#endif






        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void NetworkChange_NetworkAddressChanged(object sender, EventArgs e)
        {
            // check for a few seconds as a network change 
            if (RescanIPTimer == null)
            {
                RescanIPTimer = new Timer(5000);
                RescanIPTimer.Elapsed += RescanIPTimer_Elapsed;
                RescanIPTimer.Start();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void RescanIPTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            RescanIPTimer.Stop();
            RescanIPTimer.Dispose();
            //RescanIPTimer = null;

            if (Conf.IPMode == "IPv4")
            {
                _ipv4Addresses = null;
                bool iplisted = false;
                foreach (IPAddress ip in AddressListIPv4)
                {
                    if (Conf.IPv4Address == ip.ToString())
                        iplisted = true;
                }
                if (!iplisted)
                {

                    _ipv4Address = "";
                    Conf.IPv4Address = AddressIPv4;
                }
                if (iplisted)
                    return;
            }

            if (true)
            {
                switch (Conf.IPMode)
                {
                    case "IPv4":
                        string msg = "Your IP address has changed. Please set a static IP address for your local computer to ensure uninterrupted connectivity.";
                        LogErrorToFile(msg
                        );
                       

                        if (Conf.DHCPReroute && Conf.IPMode == "IPv4")
                        {
                            //check if IP address has changed
                            if (Conf.UseUPNP)
                            {
                                //change router ports
                                if (NATControl.SetPorts(Conf.ServerPort, Conf.LANPort))
                                    LogMessageToFile("Router port forwarding has been updated. (" +
                                                     Conf.IPv4Address + ")");
                            }
                            else
                            {
                                LogMessageToFile("Please check Use UPNP in web settings to handle this automatically");
                            }
                        }
                        else
                        {
                            LogMessageToFile("Enable DHCP Reroute in Web Settings to handle this automatically");
                        }
                        
                        break;
                    case "IPv6":
                        _ipv6Addresses = null;
                        bool iplisted = false;
                        foreach (IPAddress ip in AddressListIPv6)
                        {
                            if (Conf.IPv6Address == ip.ToString())
                                iplisted = true;
                        }
                        if (!iplisted)
                        {
                            LogErrorToFile(
                                "Your IP address has changed. Please set a static IP address for your local PC to ensure uninterrupted connectivity.");
                            _ipv6Address = "";
                            Conf.IPv6Address = AddressIPv6;
                        }
                        break;
                }
            }



        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EnqueuingVodTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _enqueuingVodTimer.Stop();

            try
            {
                if (Conf.ReqDirectory == null || Conf.ReqDirectory == "NotSet")
                {
                    Conf.ReqDirectory = Program.AppDataPath + @"WebServerRoot\Media\";
                }

                if (!Directory.Exists(Conf.ReqDirectory))
                {
                    string notfound = Conf.ReqDirectory;
                    LogErrorToFile("Request directory could not be found (" + notfound + ") - reset it to " +
                                     Program.AppDataPath + @"WebServerRoot\Media\" + " in settings.");
                }

                else
                {
                    FileInfo fi;
                    foreach (string s in Directory.GetFiles(Conf.ReqDirectory + DefaultValues.ClearMedia_Directory, "*.xml", SearchOption.TopDirectoryOnly))
                    {

                        fi = new FileInfo(s);
                        //string fextension = fi.Extension;
                        string sLocalDir = Program.AppDataPath + @"WebServerRoot\Media\" + "video\\";

                        if (!File.Exists(sLocalDir + fi.Name))
                        {
                            fi.CopyTo(Path.Combine(sLocalDir, fi.Name), true);

                            ProcessTagInfo ProcessInfo = new ProcessTagInfo();
                            ProcessInfo.DaemonName = "ENQUEUE";
                            ProcessInfo.FileName = fi.Name;
                            ProcessInfo.FullFileName = fi.FullName;
                            ProcessTagInfoManager.Instance.Add(ProcessInfo, true);
                            

                        }

                    }
                }
            }
            catch(Exception ex)
            {
                LogExceptionToFile(ex);
            }


            if (!_shuttingDown)
                _enqueuingVodTimer.Start();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HouseKeepingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            _houseKeepingTimer.Stop();

            

            if (_cputotalCounter != null)
            {
                try
                {
                    CpuUsage = Convert.ToInt32(_cpuCounter.NextValue()) / Environment.ProcessorCount;
                    CpuTotal = Convert.ToInt32(_cputotalCounter.NextValue());
                    _counters = "CPU: " + CpuUsage + "%";

                    if (_pcMem != null)
                    {
                        if (_pcMemAvailable)
                            _counters += " RAM Available: " + Convert.ToInt32(_pcMem.NextValue()) + "Mb";
                        else
                            _counters += " RAM Usage: " + Convert.ToInt32(_pcMem.RawValue / 1048576) + "Mb";
                    }
                    
                    PerfMonitoringlbl.Text = _counters;
                }
                catch (Exception ex)
                {
                    _cputotalCounter = null;
                    LogExceptionToFile(ex);
                }
                if (CpuTotal > 90) // adding to config file
                {
                    if (ThrottleFramerate > 1)
                        ThrottleFramerate--;
                }
                else
                {
                    if (ThrottleFramerate < 40)
                        ThrottleFramerate++;
                }
            }
            else
            {
                _counters = "Stats Unavailable - See Log File";
            }

            
            try
            {
                if (!MWS.Running && MWS.NumErr >= 5)
                {
                    LogMessageToFile("Server not running - Restarting");
                   _tsslStats.Text = "Server not running - Restarting...";
                    StopAndStartServer();
                }

                // Save File Data.

            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }

            if (MWS.Running)
                _tsslStats.Text = "Server is Online";
            else
                _tsslStats.Text = "Server is Offline";


            _pingCounter++;

            
            if (_pingCounter == 180)
            {
                string res = string.Empty;
                if (!loadurl("http://localhost:8085/MEBSCatalog/mebs_settings", out res))
                {
                    string msg = string.Format("While attempting to load MTV.Catalog.Host the following exception was thrown : {0}", res);
                    mainGuiUC.MonitroingSystemBoxAddMsg(msg, LogMode.Error);
                    msg = null;

                    //SendEmailInBackgroundThread(msg);

                }
               
               _pingCounter = 0;
            }

            if (Conf.Enable_Storage_Management)
            {
                _storageCounter++;

                if (_storageCounter == 3600) // every hour // 3600
                {
                    RunStorageManagement();
                    _storageCounter = 0;
                }
            }

          
            //Check if Mysql engine is up and running
            var w = Process.GetProcessesByName("mysqld");
            if (w.Length != 0)
            {
                bool mysqlstat = true;
                if (_mysqServerStat != mysqlstat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseMysqlServerEvent(new MysqlServerEventArgs(true)); //raise server event
                    _mysqServerStat = mysqlstat;
                }
            }
            else
            {
                bool mysqlstat = false;
                if (_mysqServerStat != mysqlstat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseMysqlServerEvent(new MysqlServerEventArgs(false)); //raise server event
                    _mysqServerStat = mysqlstat; //update global status
                }

            }


            WriteLog();

            if (!_shuttingDown)
                _houseKeepingTimer.Start();


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRaiseMysqlServerEvent(MysqlServerEventArgs e)
        {
            EventHandler<MysqlServerEventArgs> handler = RaiseMysqlServerEvent;
            //publish Publish Events that Conform to .NET Framework Guidelines (C# Programming Guide)
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnRaiseDequeuerServiceEvent(DequeuerServiceEventArgs e)
        {
            EventHandler<DequeuerServiceEventArgs> handler = RaiseDequeuerServiceEvent;
            //publish Publish Events that Conform to .NET Framework Guidelines (C# Programming Guide)
            if (handler != null)
            {
                handler(this, e);
            }
        }


        protected virtual void OnRaiseCatalogServiceEvent(CatalogServiceEventArgs e)
        {
            EventHandler<CatalogServiceEventArgs> handler = RaiseCatalogServiceEvent;
            //publish Publish Events that Conform to .NET Framework Guidelines (C# Programming Guide)
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //protected virtual void OnRaiseSmtpServerEvent(SmtpServerEventArgs e)
        //{
        //    EventHandler<SmtpServerEventArgs> handler = RaiseSmtpServerEvent;
        //    //publish Publish Events that Conform to .NET Framework Guidelines (C# Programming Guide)
        //    if (handler != null)
        //    {
        //        handler(this, e);
        //    }
        //}

        private bool loadurl(string url, out string result)
        {
            result = "";
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(url);
                
                httpWReq.Timeout = 5000;
                httpWReq.KeepAlive = false;
                httpWReq.ProtocolVersion = HttpVersion.Version10;
                httpWReq.ServicePoint.ConnectionLimit = 1;
                
                httpWReq.Method = "GET";

                var myResponse = (HttpWebResponse)httpWReq.GetResponse();
                var s = myResponse.GetResponseStream();
                if (s != null)
                {
                    var read = new StreamReader(s);
                    result = read.ReadToEnd();
                }
                
                myResponse.Close();
                
                
                return true;
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            return false;
        }


        /// <summary>
        /// test the smtp connection by sending a HELO command
        /// </summary>
        /// <param name="smtpServerAddress">Smtp Server</param>
        /// <param name="port">port</param>
        internal static bool TestSmtpConnection(string smtpServerAddress, int port)
        {
            try
            {
                IPHostEntry hostEntry = Dns.GetHostEntry(smtpServerAddress);
                IPEndPoint endPoint = new IPEndPoint(hostEntry.AddressList[0], port);
                using (Socket tcpSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp))
                {
                    //try to connect and test the rsponse for code 220 = success
                    tcpSocket.Connect(endPoint);
                    if (!CheckResponse(tcpSocket, 220))
                    {
                        return false;
                    }

                    // send HELO and test the response for code 250 = proper response
                    SendData(tcpSocket, string.Format("HELO {0}\r\n", Dns.GetHostName()));
                    if (!CheckResponse(tcpSocket, 250))
                    {
                        return false;
                    }

                    // if we got here it's that we can connect to the smtp server
                   
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
                return false;
            }
        }

        private static void SendData(Socket socket, string data)
        {
            byte[] dataArray = Encoding.ASCII.GetBytes(data);
            socket.Send(dataArray, 0, dataArray.Length, SocketFlags.None);
        }

        private static bool CheckResponse(Socket socket, int expectedCode)
        {
            while (socket.Available == 0)
            {
                System.Threading.Thread.Sleep(100);
            }
            byte[] responseArray = new byte[1024];
            socket.Receive(responseArray, 0, socket.Available, SocketFlags.None);
            string responseData = Encoding.ASCII.GetString(responseArray);
            int responseCode = Convert.ToInt32(responseData.Substring(0, 3));
            if (responseCode == expectedCode)
            {
                return true;
            }
            return false;
        }
    


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrPoll_Tick(object sender, ElapsedEventArgs e)
        {

            _pollTimer.Stop();


#if (!DEBUG)

            // MTV.EventDequeuer.Host is RUNNING ???

            if (SM.IsServiceRunning("MTV.EventDequeuer.Host"))
            {
                bool DequeuerStat = true;
                if (_eventDequeuerStat != DequeuerStat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseDequeuerServiceEvent(new DequeuerServiceEventArgs(true)); //raise server event
                    _eventDequeuerStat = DequeuerStat;
                }
                
                
            }
            else
            {
               
                bool DequeuerStat = false;
                if (_eventDequeuerStat != DequeuerStat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseDequeuerServiceEvent(new DequeuerServiceEventArgs(false)); //raise server event
                    _eventDequeuerStat = DequeuerStat; //update global status
                }

            }

            if (SM.IsServiceRunning("MTV.Catalog.Host"))
            {
                bool CatalogStat = true;
                if (_catalogHostStat != CatalogStat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseCatalogServiceEvent(new CatalogServiceEventArgs(true)); //raise server event
                    _catalogHostStat = CatalogStat;
                }


            }

            else
            {

                bool CatalogStat = false;
                if (_catalogHostStat != CatalogStat) //check if current status same as previous. this prevent notification always shown
                {
                    OnRaiseCatalogServiceEvent(new CatalogServiceEventArgs(false)); //raise server event
                    _catalogHostStat = CatalogStat; //update global status
                }

            }


            //if (TestSmtpConnection(MainForm.Conf.SMTPServer, MainForm.Conf.SMTPPort))
            //{
            //    bool SmtpStat = true;

            //    if (_smtpServerStat != SmtpStat) //check if current status same as previous. this prevent notification always shown
            //    {
            //        OnRaiseSmtpServerEvent(new SmtpServerEventArgs(true)); //raise server event
            //        _smtpServerStat = SmtpStat;
            //    }
            //}
            //else
            //{
            //    bool SmtpStat = false;
            //    if (_smtpServerStat != SmtpStat) //check if current status same as previous. this prevent notification always shown
            //    {
            //        OnRaiseSmtpServerEvent(new SmtpServerEventArgs(false)); //raise server event
            //        _smtpServerStat = SmtpStat; //update global status
            //    }

            //}
            
           

#else
            DateTime _now = DateTime.UtcNow;
            var _wEventDequeuerHost = Process.GetProcessesByName("MTV.EventDequeuer.Host");
            if (_wEventDequeuerHost.Length == 0)
            {
                try
                {
                   mainGuiUC.MonitroingSystemBoxAddMsg(String.Format("[ MTV.EventDequeuer.Host Service ] is [ OFFLINE ], checked on [{0}]", _now),LogMode.Error);
                }
                catch
                {
                }
               

            }
            else
            {
                try
                {
                   mainGuiUC.MonitroingSystemBoxAddMsg(String.Format("[ MTV.EventDequeuer.Host Service ] is [ ONLINE ], checked on [ {0} ]", _now), LogMode.Information);
                
                }
                catch
                {
                }
            }

            //var _wCatalogHost = Process.GetProcessesByName("MTV.Catalog.Host");
            //if (_wCatalogHost.Length == 0)
            //{
            //    try
            //    {
                  
            //       // mainGuiUC.MonitroingSystemBoxAddMsg(String.Format("[ MTV.Catalog.Host Service ] is [ OFFLINE ], checked on [ {0} ]", _now), LogMode.Error);
            //    }
            //    catch
            //    {
            //    }


            //}
            //else
            //{
            //    try
            //    {
            //       //mainGuiUC.MonitroingSystemBoxAddMsg(String.Format("[ MTV.Catalog.Host Service ] is [ ONLINE ], checked on [ {0} ]", _now), LogMode.Information);
            //    }
            //    catch
            //    {
            //    }
            //}




#endif
          
            
            if (!_shuttingDown)
                _pollTimer.Start();

        }


        private delegate void RunStorageManagementDelegate();
        public void RunStorageManagement()
        {
            if (InvokeRequired)
            {
                Invoke(new RunStorageManagementDelegate(RunStorageManagement));
                return;
            }


            if (StorageThread == null || !StorageThread.IsAlive)
            {
                LogMessageToFile("Running Storage Management");
                StorageThread = new Thread(DeleteOldFiles) { IsBackground = true };
                StorageThread.Start();
            }
            else
                LogMessageToFile("Storage Management is already running");
        }



        private void DeleteOldFiles()
        {
            if (Conf.DeleteFilesOlderThanDays <= 0)
                return;

            var a = new List<string>(); 
            try
            {
                string dir = Conf.ReqDirectory.Trim('\\');
                a.AddRange(Directory.GetFiles(dir + "\\" + DefaultValues.IngestedMedia_Directory , "*.*", SearchOption.AllDirectories).ToList());
                
            }
            catch (Exception ex)
            {
                //can fail permissions reasons
                LogExceptionToFile(ex);
                return;
            }

            DateTime dtref = DateTime.Now.AddDays(0 - Conf.DeleteFilesOlderThanDays);

            foreach (string name in a)
            {
                try
                {
                    var fi = new FileInfo(name);
                    if (fi.CreationTime < dtref && fi.Extension.ToLower() != ".ts")
                    {
                        try
                        {
                            MainForm.LogMessageToFile(name);
                            File.Delete(name);
                           

                        }
                        catch (Exception ex)
                        {
                            LogExceptionToFile(ex);
                        }
                    }
                }
                catch
                {
                    //file may have been deleted
                }
            }



        }





        /// <summary>
        /// 
        /// </summary>
        void LoadViewSettings() {
            mainGuiUC.LoadSettingsView();
            gridToolStripMenuItem.Checked = MTVPRO.Settings.Default.ViewGrid;
            EpisodeToolStripMenuItem.Checked = MTVPRO.Settings.Default.ViewScheduleDetails;
            NotificationsToolStripMenuItem.Checked = MTVPRO.Settings.Default.ViewNotifications;
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void faTabStripItem1_Changed(object sender, EventArgs e) {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void segmentsToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem menu = ((ToolStripMenuItem)sender);
            menu.Checked = !menu.Checked;
            MTVPRO.Settings.Default.ViewGrid = gridToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewNotifications = NotificationsToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewScheduleDetails = EpisodeToolStripMenuItem.Checked;
            LoadViewSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tmrRefresh_Tick(object sender, EventArgs e) {
            // Refresh Timer.
            try
            {
                mainGuiUC.UpdateList();
                
               

            }
            catch (Exception ex)
            {
              
                LogExceptionToFile(ex);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void toolbarToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem menu = ((ToolStripMenuItem)sender);
            menu.Checked = !menu.Checked;
            MTVPRO.Settings.Default.ViewGrid = gridToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewNotifications = NotificationsToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewScheduleDetails = EpisodeToolStripMenuItem.Checked;
            LoadViewSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void gridToolStripMenuItem_Click(object sender, EventArgs e) {
            ToolStripMenuItem menu = ((ToolStripMenuItem)sender);
            menu.Checked = !menu.Checked;
            MTVPRO.Settings.Default.ViewGrid = gridToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewNotifications = NotificationsToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewScheduleDetails = EpisodeToolStripMenuItem.Checked;
            LoadViewSettings();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            e.Cancel = !ShowClose();
            if (e.Cancel == false) {
                LogMessageToFile("MTV.Scheduler.APP has been Stopped by User.");
                Exit();
                
                
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool ShowClose() {
            //Yes or no message box to exit the application
            DialogResult Response;
            Response = MessageBox.Show("Are you sure you want to Exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return Response == DialogResult.Yes;
        }

        void MainForm_Load(object sender, EventArgs e) {

            
            
            UISync.Init(this);

            SetDoubleBuffering();
           
            try { File.WriteAllText(Program.AppDataPath + "exit.txt", "RUNNING"); }
            catch (Exception ex) { LogExceptionToFile(ex); }

            InitLogging();

            //this initializes the port mapping collection
            NATUPNPLib.IStaticPortMappingCollection map = NATControl.Mappings;

            if (Conf.MediaDirectory == null || Conf.MediaDirectory == "NotSet")
            {
                Conf.MediaDirectory = Program.AppDataPath + @"WebServerRoot\Media\";
            }
            if (!Directory.Exists(Conf.MediaDirectory))
            {
                string notfound = Conf.MediaDirectory;
                //Conf.MediaDirectory = Program.AppDataPath + @"WebServerRoot\Media\";
                LogErrorToFile("Media directory could not be found (" + notfound + ") - reset it to " +
                                 Program.AppDataPath + @"WebServerRoot\Media\" + " in settings if it doesn't attach.");
            }


            Identifier = Guid.NewGuid().ToString();

            SM = new WindowsServiceManager();

            MWS = new LocalServer(this)
            {
                ServerRoot = Program.AppDataPath + @"WebServerRoot\",
            };


            GC.KeepAlive(Program.Mutex);
            GC.KeepAlive(MWS);


            if (string.IsNullOrEmpty(Conf.ServerName) || Conf.ServerName == "NotSet")
            {
                Conf.ServerName = SystemInformation.ComputerName;
            }

            
            StopAndStartServer();


            var t = new Thread(ShowInfo) { IsBackground = false };
            t.Start();


            var t1 = new Thread(RunServices) { IsBackground = false };
            t1.Start();

            try
            {
                _cputotalCounter = new PerformanceCounter("Processor", "% Processor Time", "_total", true);
                _cpuCounter = new PerformanceCounter("Process", "% Processor Time",
                                                     Process.GetCurrentProcess().ProcessName, true);
                try
                {
                    _pcMem = new PerformanceCounter("Process", "Working Set - Private",
                                                    Process.GetCurrentProcess().ProcessName, true);
                }
                catch
                {
                    //no working set - only total available on windows xp
                    try
                    {
                        _pcMem = new PerformanceCounter("Memory", "Available MBytes");
                        _pcMemAvailable = true;
                    }
                    catch (Exception ex2)
                    {
                        LogExceptionToFile(ex2);
                        _pcMem = null;
                    }
                }

            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
                _cputotalCounter = null;
            }


          

            _houseKeepingTimer = new Timer(1000);
            _houseKeepingTimer.Elapsed += HouseKeepingTimerElapsed;
            _houseKeepingTimer.AutoReset = true;
            _houseKeepingTimer.SynchronizingObject = this;
            GC.KeepAlive(_houseKeepingTimer);


            //_pollTimer

            _pollTimer = new Timer(1000);
            _pollTimer.Elapsed += tmrPoll_Tick;
            _pollTimer.AutoReset = true;
            _pollTimer.SynchronizingObject = this;
            GC.KeepAlive(_pollTimer);


            // _enqueuingVodTimer
            _enqueuingVodTimer = new Timer(1000);
            _enqueuingVodTimer.Elapsed += EnqueuingVodTimerElapsed;
            _enqueuingVodTimer.AutoReset = true;
            _enqueuingVodTimer.SynchronizingObject = this;
            GC.KeepAlive(_enqueuingVodTimer);


            NetworkChange.NetworkAddressChanged += NetworkChange_NetworkAddressChanged;

            _houseKeepingTimer.Start();
            _pollTimer.Start();
            _enqueuingVodTimer.Start();

          //  EventDequeuerWatch.Instance.Subscribe();
            obj = new EventDequeuerWatchManager();
            
 
            LoadViewSettings();

            

            notifyIcon.Icon = this.Icon;
            notifyIcon.Text = "MTV.Scheduler.App";
            notifyIcon.Visible = true;

         


        }

        void SetDoubleBuffering() {
            // Use double buffering.
            this.SetStyle(ControlStyles.AllPaintingInWmPaint |
                          ControlStyles.OptimizedDoubleBuffer |
                          ControlStyles.UserPaint, true);
        }

        void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        void showHideToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowHideForm();
        }

        void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                ShowHideForm();
            }
        }

        void exitToolStripMenuItem1_Click(object sender, EventArgs e) {
            Close();
        }

        void optionsToolStripMenuItem1_Click(object sender, EventArgs e) {
            using (OptionsForm options = new OptionsForm()) {
                options.ShowDialog();
            }
        }
        #endregion

        #region - Internal Method(s) -

        internal static string StopAndStartServer()
        {
            string message = "";
            try
            {
                MWS.StopServer();
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }

            Application.DoEvents();
            try
            {
                message = MWS.StartServer();
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }
            return message;
        }


        internal static void LogExceptionToFile(Exception ex)
        {
            if (!_logging)
                return;
            try
            {
                string em = ex.HelpLink + "<br/>" + ex.Message + "<br/>" + ex.Source + "<br/>" + ex.StackTrace +
                            "<br/>" + ex.InnerException + "<br/>" + ex.Data;
                LogFile.Append("<tr><td style=\"color:red\" valign=\"top\">Exception:</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + em + "</td></tr>");
            }
            catch
            {

            }
        }


        internal static void LogMessageToFile(String message)
        {
            if (!_logging)
                return;

            try
            {
                LogFile.Append("<tr><td style=\"color:green\" valign=\"top\">Message</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }


        internal static void LogErrorToFile(String message)
        {
            if (!_logging)
                return;

            try
            {
                LogFile.Append("<tr><td style=\"color:red\" valign=\"top\">Error</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }



        internal static void LogWarningToFile(String message)
        {
            if (!_logging)
                return;

            try
            {
                LogFile.Append("<tr><td style=\"color:orange\" valign=\"top\">Warning</td><td valign=\"top\">" +
                               DateTime.Now.ToLongTimeString() + "</td><td valign=\"top\">" + message + "</td></tr>");
            }
            catch
            {
                //do nothing
            }
        }


         /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static bool SendAlert(string emailAddress, string subject, string message)
        {

                return MailProvider.Send(emailAddress, subject, message);
                
                            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static bool SendAlertV2(string emailAddress, string subject, string message)
        {

            string body = PopulateBody(message, subject);

            return MailProvider.Send(emailAddress, subject, body);


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static string PopulateBody(string message, string subject)
        {
            string body = string.Empty;
            string _TemplatePath = string.Format(@"{0}\{1}", System.Windows.Forms.Application.StartupPath, @"Templates\EmailTemplate.html");

            using (StreamReader reader = new StreamReader(_TemplatePath))
            {
                body = reader.ReadToEnd();
            }

            body = body.Replace("{MAIL_TITLE}", subject);
            body = body.Replace("{MAIN_TITLE}", subject);
            body = body.Replace("{NOTIF_DATE}", DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss"));
            body = body.Replace("{HOST}", MainForm.Conf.ServerName);

            body = body.Replace("{MAIL_BODY}", message);
            body = body.Replace("{SIGNATURE_NAME}", "Support Team");
            body = body.Replace("{MAIL_ADRESS}", MainForm.Conf.SMTPFromAddress);
            body = body.Replace("{COMPANY_NAME}", "Motive Television SARL");
            body = body.Replace("{ADRESS}", "Technopark - Bureau 251, Route de Nouaceur <br /> 20000 Casablanca, Maroc");
            body = body.Replace("{POST_CODE}", "20800");
            body = body.Replace("{PHONE}", "+212 661 607 120");
            body = body.Replace("{PHONE1}", "+212 661 339 844");
            body = body.Replace("{WEB_SITE}", Website);
            return body;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        internal static bool SendContent(string emailAddress, string subject, string message)
        {
               return MailProvider.Send(emailAddress, subject, message);
               
              
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="imageData"></param>
        internal static void SendAlertWithImage(string emailAddress, string subject, string message, byte[] imageData)
        {

            if (imageData.Length == 0)
            {
                //SendAlert(emailAddress, subject, message);
                SendAlertV2(emailAddress, subject, message);
                return;
            }
            
            MailProvider.Send(emailAddress, subject, message, imageData);
                return;
         
        }

        /// <summary>
        /// Build enqueueing-response file Errro.
        /// </summary>
        /// <param name="productFileName"></param>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="errortype"></param>
        /// <param name="message"></param>
        internal static void buildErrorResponseXmlFile(string productFileName, string id, string status, string errortype, string message)
        {
            //<?xml version="1.0" encoding="UTF-8"?>
            //<enqueueing-response >
            //    <id>16548</id>
            //    <status>ERROR</status>
            //    <errortype>REQUEST</errortype>
            //    <message>Transport stream file not found | c:\..\sample.ts (The system cannot find the file specified)</message>	
            //</enqueueing-response>

            string sResp = "";

            StringBuilder sb = new StringBuilder();
           PlayoutCommandProvider.StringWriterWithEncoding stringWriter = new PlayoutCommandProvider.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);


            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

            //---- Set the XmlWriterSettings to the XMLWriter
            xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

            /*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            * false = no
            * true = yes
            */
            xmlWriter.WriteStartDocument(false);

            //<enqueueing-response>
            xmlWriter.WriteStartElement("enqueueing-response");

            // <id>
            xmlWriter.WriteStartElement("id");
            xmlWriter.WriteString(id);
            xmlWriter.WriteEndElement();
            // </id>

            // <status>
            xmlWriter.WriteStartElement("status");
            xmlWriter.WriteString(status);
            xmlWriter.WriteEndElement();
            // </status>

            // <errortype>
            xmlWriter.WriteStartElement("errortype");
            xmlWriter.WriteString(errortype);
            xmlWriter.WriteEndElement();
            // </errortype>

            // <message>
            xmlWriter.WriteStartElement("message");
            xmlWriter.WriteString(message);
            xmlWriter.WriteEndElement();
            // </message>

            //</enqueueing-response>
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            sResp = stringWriter.ToString();

            string sLocalErrDir = Conf.ReqDirectory + "\\err";
            if (!Directory.Exists(sLocalErrDir))
            {
                Directory.CreateDirectory(sLocalErrDir);
            }


            XMLStringToFile(sResp, sLocalErrDir + @"\" + productFileName);

        }



        /// <summary>
        /// Save a XML String ot a File
        /// </summary>
        /// <param name="xml">String XML Format</param>
        /// <param name="path">Path of the File</param>
        public static bool XMLStringToFile(string xml, string path)
        {
            try
            {
                if (!Directory.Exists(Path.GetDirectoryName(path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(path));

                using (StreamWriter sw = new StreamWriter(path, false, UTF8Encoding.UTF8))
                {
                    sw.Write(xml);
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
                return false;
            }
        }

       /// <summary>
        /// Build enqueueing-response file Success.
       /// </summary>
       /// <param name="productFileName"></param>
       /// <param name="id"></param>
       /// <param name="status"></param>
        internal static void buildSuccessResponseXmlFile(string productFileName, string id, string status)
        {

            //<?xml version="1.0" encoding="UTF-8"?>
            //<enqueueing-response >
            //    <id>16548</id>
            //    <status>OK</status>
            //</enqueueing-response>

            string sResp = "";
            StringBuilder sb = new StringBuilder();
            PlayoutCommandProvider.StringWriterWithEncoding stringWriter = new PlayoutCommandProvider.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);


            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

            //---- Set the XmlWriterSettings to the XMLWriter
            xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

            /*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            * false = no
            * true = yes
            */
            xmlWriter.WriteStartDocument(false);

            //<enqueueing-response>
            xmlWriter.WriteStartElement("enqueueing-response");

            // <id>
            xmlWriter.WriteStartElement("id");
            xmlWriter.WriteString(id);
            xmlWriter.WriteEndElement();
            // </id>

            // <id>
            xmlWriter.WriteStartElement("status");
            xmlWriter.WriteString(status);
            xmlWriter.WriteEndElement();
            // </id>

            //</enqueueing-response>
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            sResp = stringWriter.ToString();

            string sLocalsuccessDir = MainForm.Conf.ReqDirectory + "\\ok";
            if (!Directory.Exists(sLocalsuccessDir))
            {
                Directory.CreateDirectory(sLocalsuccessDir);
            }
            XMLStringToFile(sResp, sLocalsuccessDir + @"\" + productFileName);

        }




        #endregion

        #region - Public Method(s) -
       
        
        
        
        /// <summary>
        /// 
        /// </summary>
        public void ShowHideForm() {
            if (this.Visible) {
                HideForm();
            }
            else {
                ShowForm();
                LoadViewSettings();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ShowForm() {
            this.ShowInTaskbar = true;
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
        }

        /// <summary>
        /// 
        /// </summary>
        public void HideForm() {
            this.ShowInTaskbar = false;
            this.Visible = false;
        }


        


        public static IPAddress[] AddressListIPv4
        {
            get
            {
                if (_ipv4Addresses != null)
                    return _ipv4Addresses;
                _ipv4Addresses =
                    Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(
                        p => p.AddressFamily == AddressFamily.InterNetwork).ToArray();
                return _ipv4Addresses;
            }
        }


        public static IPAddress[] AddressListIPv6
        {
            get
            {
                if (_ipv6Addresses != null)
                    return _ipv6Addresses;
                var ipv6Adds = new List<IPAddress>();
                if (Conf.IPv6Disabled)
                {
                    _ipv6Addresses = ipv6Adds.ToArray();
                    return _ipv6Addresses;
                }

                try
                {
                    var addressInfoCollection = IPGlobalProperties.GetIPGlobalProperties().GetUnicastAddresses();

                    foreach (var addressInfo in addressInfoCollection)
                    {
                        if (addressInfo.Address.IsIPv6Teredo ||
                            (addressInfo.Address.AddressFamily == AddressFamily.InterNetworkV6 &&
                            !addressInfo.Address.IsIPv6LinkLocal && !addressInfo.Address.IsIPv6SiteLocal))
                        {
                            if (!System.Net.IPAddress.IsLoopback(addressInfo.Address))
                            {
                                ipv6Adds.Add(addressInfo.Address);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //unsupported on win xp
                    LogExceptionToFile(ex);
                }
                _ipv6Addresses = ipv6Adds.ToArray();
                return _ipv6Addresses;
            }
        }

        private static string _ipv4Address = "";


        public static string AddressIPv4
        {
            get
            {
                if (_ipv4Address != "")
                    return _ipv4Address;

                string detectip = "";
                foreach (IPAddress ip in AddressListIPv4)
                {
                    if (detectip == "")
                        detectip = ip.ToString();

                    if (Conf.IPv4Address == ip.ToString())
                    {
                        _ipv4Address = ip.ToString();
                        break;
                    }

                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {

                        if (!System.Net.IPAddress.IsLoopback(ip))
                        {
                            if (detectip == "")
                                detectip = ip.ToString();
                        }
                    }
                }
                if (_ipv4Address == "")
                    _ipv4Address = detectip;
                Conf.IPv4Address = _ipv4Address;

                return _ipv4Address;
            }
            set { _ipv4Address = value; }
        }


        private static string _ipv6Address = "";
        public static string AddressIPv6
        {
            get
            {
                if (_ipv6Address != "")
                    return _ipv6Address;

                string detectip = "";
                foreach (IPAddress ip in AddressListIPv6)
                {
                    if (detectip == "")
                        detectip = ip.ToString();

                    if (Conf.IPv6Address == ip.ToString())
                    {
                        _ipv6Address = ip.ToString();
                        break;
                    }

                    if (ip.IsIPv6Teredo)
                    {
                        detectip = ip.ToString();
                    }
                }

                if (_ipv6Address == "")
                    _ipv6Address = detectip;
                Conf.IPv6Address = _ipv6Address;

                return _ipv6Address;

            }
            set { _ipv6Address = value; }
        }


        public static string IPAddress
        {
            get
            {
                if (Conf.IPMode == "IPv4")
                    return AddressIPv4;
                return MakeIPv6Url(AddressIPv6);
            }
        }

        public static string IPAddressExternal
        {
            get
            {
                if (Conf.IPMode == "IPv4")
                {

                    try
                    {
                        string externalIP;
                        externalIP = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                        externalIP = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                                     .Matches(externalIP)[0].ToString();
                        return externalIP;
                    }
                    catch
                    {
                        return "";

                    }

                    //return "";//WsWrapper.ExternalIPv4(false);
                }
                return MakeIPv6Url(AddressIPv6);
            }
        }

        private static string MakeIPv6Url(string ip)
        {
            //strip scope id
            if (ip.IndexOf("%") != -1)
                ip = ip.Substring(0, ip.IndexOf("%"));
            return "[" + ip + "]";
        }



        #endregion

        private void menuBarStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void EpisodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //ToolStripMenuItem menu = ((ToolStripMenuItem)sender);
            //menu.Checked = !menu.Checked;
            //Settings.Default.ViewGrid = gridToolStripMenuItem.Checked;
            //Settings.Default.ViewNotifications = NotificationsToolStripMenuItem.Checked;
            //Settings.Default.ViewScheduleDetails = EpisodeToolStripMenuItem.Checked;
            //LoadViewSettings();
        }

        private void NotificationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menu = ((ToolStripMenuItem)sender);
            menu.Checked = !menu.Checked;
            MTVPRO.Settings.Default.ViewGrid = gridToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewNotifications = NotificationsToolStripMenuItem.Checked;
            MTVPRO.Settings.Default.ViewScheduleDetails = EpisodeToolStripMenuItem.Checked;
            LoadViewSettings();
        }

        public static void LogRealTimeNotification(string msg)
        {
           //_eventDequeuerNotification = msg;
        }

        #region - IDisposable Member(s) -

        /// <summary>
        /// Dispose Resources.
        /// </summary>
        private void Exit()
        {

            if (_houseKeepingTimer != null)
                _houseKeepingTimer.Stop();


            if (_pollTimer != null)
                _pollTimer.Stop();

            if (_enqueuingVodTimer != null)
            {
                _enqueuingVodTimer.Dispose();
            }

            _shuttingDown = true;

            _closing = true;

            try
            {
                MWS.StopServer();

            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }

            try
            {
                obj.Dispose();
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }


#if (!DEBUG)
         
            // Stop MTV specific services
            try
            {
                Program.ShutDownMTVServices();
            }
            catch (Exception ex)
            {
                LogExceptionToFile(ex);
            }     
#else

            try
            {

                LogMessageToFile("MTV.EventDequeuer.Host service stopping...");

                if (Process.GetProcessesByName("MTV.EventDequeuer.Host").Length != 0)
                {
                    foreach (Process proc in Process.GetProcessesByName("MTV.EventDequeuer.Host"))
                        proc.Kill();
                }

                Thread.Sleep(200);

                if (Process.GetProcessesByName("MTV.EventDequeuer.Host").Length != 0)
                {
                    LogErrorToFile("StopMTVServices: Cannot terminate MTV.EventDequeuer.Host.exe");

                }
                else
                    LogMessageToFile("MTV.EventDequeuer.Host service stopped");
            }
            catch (System.ComponentModel.Win32Exception win32Ex)
            {
                LogExceptionToFile(win32Ex);
            }


            try
            {
                LogMessageToFile("MTV.Catalog.Host service stopping...");

                if (Process.GetProcessesByName("MTV.Catalog.Host").Length != 0)
                {
                    foreach (Process proc in Process.GetProcessesByName("MTV.Catalog.Host"))
                        proc.Kill();
                }

                Thread.Sleep(200);

                if (Process.GetProcessesByName("MTV.Catalog.Host").Length != 0)
                {
                    LogErrorToFile("StopMTVServices: Cannot terminate MTV.Catalog.Host.exe");

                }
                else
                    LogMessageToFile("MTV.Catalog.Host service stopped.");
            }
            catch (System.ComponentModel.Win32Exception win32Ex)
            {
                LogExceptionToFile(win32Ex);
            }


            
#endif

            File.WriteAllText(Program.AppDataPath + "exit.txt", "OK");

            WriteLog();
        }

        #endregion


        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try
                {
                    _sync.BeginInvoke(action, null);
                }
                catch
                {
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenUrl(Website);
        }

        /// <summary>
        /// Open Motive Web site. 
        /// </summary>
        /// <param name="url"></param>
        public static void OpenUrl(string url)
        {
            try
            {
                Process.Start(url);
            }
            catch (Exception)
            {
                try
                {
                    var p = new Process { StartInfo = { FileName = DefaultBrowser, Arguments = url } };
                    p.Start();
                }
                catch (Exception ex2)
                {
                    LogExceptionToFile(ex2);
                }
            }
        }

        /// <summary>
        ///  Get Default Web Browser from Registry.
        /// </summary>
        private static string DefaultBrowser
        {
            get
            {
                if (!String.IsNullOrEmpty(_browser))
                    return _browser;

                _browser = string.Empty;
                RegistryKey key = null;
                try
                {
                    key = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //trim off quotes
                    if (key != null) _browser = key.GetValue(null).ToString().ToLower().Replace("\"", "");
                    if (!_browser.EndsWith(".exe"))
                    {
                        _browser = _browser.Substring(0, _browser.LastIndexOf(".exe") + 4);
                    }
                }
                finally
                {
                    if (key != null) key.Close();
                }
                return _browser;
            }
        }

        private void lblHome_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenUrl(Website);
        }

        private void lblSupport_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "mailto:icherki@motivetelevision.co.uk;kgoubar@motivetelevision.co.uk";
                proc.Start();
            }
            catch(Exception gEx)
            {
                LogExceptionToFile(gEx);
            }
        }


        private void networkTroubleshooterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTroubleShooter();
        }

        private void ShowTroubleShooter()
        {
          
            var nt = new NetworkTroubleshooter();
            nt.ShowDialog(this);
            nt.Dispose();
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        public void ShowSettings(int tabindex)
        {
            var settings = new SettingsForm { Owner = this, InitialTab = tabindex };
            if (settings.ShowDialog(this) == DialogResult.OK)
            {

                //notifyIcon1.Text = Conf.TrayIconText;
            }


            AddressIPv4 = ""; //forces reload
            AddressIPv6 = "";
            settings.Dispose();
            SaveConfig();
        }

        private static void SaveConfig()
        {

            string fileName = Program.AppDataPath + @"XML\config.xml";
            //save configuration
            var s = new XmlSerializer(typeof(configuration));
            using (var fs = new FileStream(fileName, FileMode.Create))
            {
                using (var writer = new StreamWriter(fs))
                {
                    fs.Position = 0;
                    s.Serialize(writer, Conf);
                    writer.Close();
                }
                fs.Close();
            }
        }

        public void ShowSchedulesMsg(string msg, LogMode m)
        {
            mainGuiUC.ShowSchedulesNotifications(msg, m);
        }

        private void webAccessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void WebConnect()
        {
            var ws = new WebAccessForm();
            ws.ShowDialog(this);
            ws.Dispose();
        }

        #region Nested type: ListItem

        public struct ListItem
        {
            private readonly string _name;
            internal readonly string[] Value;

            public ListItem(string name, string[] value)
            {
                _name = name;
                Value = value;
            }

            public override string ToString()
            {
                return _name;
            }
        }

        #endregion


        /// <summary>
        /// Show Getting started Form.
        /// </summary>
        private void ShowGettingStarted()
        {
            var gs = new GettingStarted();
            gs.Closed += _gs_Closed;
            gs.Show(this);
            gs.Activate();
        }

        /// <summary>
        ///  On Getting started Closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _gs_Closed(object sender, EventArgs e)
        {
            if (((GettingStarted)sender).LangChanged)
            {
               
                Refresh();
            }
        }

        private void gettingStartedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        public static void StartBrowser(string url)
        {
            if (url != "")
                Help.ShowHelp(null, url);
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var form = new AboutForm();
            form.ShowDialog();
            form.Dispose();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {

        }

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Requested functionality not yet implemented");
        }

        private void reportBugFeedbackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fb = new Feedback();
            fb.ShowDialog(this);
            fb.Dispose();
        }

        private void goToWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartBrowser("http://www.motivetelevision.co.uk/");
        }

        private void gettingStartedToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowGettingStarted();
        }

        private void networkTroubleshooterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowTroubleShooter();
        }

        private void optionsToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            using (OptionsForm options = new OptionsForm())
            {
                options.ShowDialog();
            }
        }

        private void settingsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowSettings(0);
        }

        private void webAccessToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            WebConnect();
        }

        private void fullScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //fullScreenToolStripMenuItem.Checked =

            fullScreenToolStripMenuItem.Checked = !fullScreenToolStripMenuItem.Checked;
            if (fullScreenToolStripMenuItem.Checked)
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.None;
                WinApi.SetWinFullScreen(Handle);
            }
            else
            {
                WindowState = FormWindowState.Maximized;
                FormBorderStyle = FormBorderStyle.Sizable;
            }
        }

        private void logFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowLogFile();
        }

        private void ShowLogFile()
        {
            Process.Start(Program.AppDataPath + "log_" + NextLog + ".htm");
        }

        private void logFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                InitialDirectory = Program.AppDataPath,
                Filter = "MTV.Scheduler.APP Log Files (*.htm)|*.htm|XML Files (*.xml)|*.xml|All Files (*.*)|*.*"
            };

            if (ofd.ShowDialog(this) != DialogResult.OK) return;
            string fileName = ofd.FileName;

            if (fileName.Trim() != "")
            {
                Process.Start(ofd.FileName);
            }
        }

        
    }
}
