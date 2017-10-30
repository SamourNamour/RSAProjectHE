using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using MTV.Scheduler.App.MTVControl;

namespace MTV.Scheduler.App.UI
{
    public partial class NetworkTroubleshooter : Form
    {
        private string NL = Environment.NewLine;
        public NetworkTroubleshooter()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void NetworkTroubleshooter_Load(object sender, EventArgs e)
        {
            UISync.Init(this);


            var t = new Thread(Troubleshooter) { IsBackground = false };
            t.Start();
        }

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


        private bool loadurl(string url, out string result)
        {
            result = "";
            try
            {
                var httpWReq = (HttpWebRequest)WebRequest.Create(url);
                httpWReq.Timeout = 5000;
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


        private void Troubleshooter()
        {
            UISync.Execute(() => rtbOutput.Clear());
            try
            {
                MainForm.StopAndStartServer();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            bool portMapOk = false;
            //bool IPv6 = MainForm.Conf.IPMode == "IPv6";
            UISync.Execute(() => button2.Enabled = false);

            string localserver = "http://" + MainForm.IPAddress + ":" + MainForm.Conf.LANPort;

            UISync.Execute(() => rtbOutput.Text = "Local web Server: " + localserver + NL);
            if (MainForm.Conf.LANPort != 8080)
            {
                UISync.Execute(() => rtbOutput.Text +=
                    "--Warning, running a local server on a non-standard port (" + MainForm.Conf.LANPort + ") may cause web-browser security errors. Click the link above to test in your web browser." +
                    NL);
            }
            UISync.Execute(() => rtbOutput.Text += "Checking local server... ");
            Application.DoEvents();
            string res = "";
            if (!loadurl(localserver + "/ping", out res))
            {
                UISync.Execute(() => rtbOutput.Text += "Failed: " + res + NL);
                if (MainForm.MWS.Running)
                {
                    UISync.Execute(() => rtbOutput.Text += "Server reports it IS running" + NL);
                }
                else
                    UISync.Execute(() => rtbOutput.Text += "Server reports it IS NOT running - check the log file for errors (View-> Log File)" + NL);

                UISync.Execute(() => rtbOutput.Text += "Do you have a third party firewall or antivirus running (AVG/ zonealarm etc)?" + NL);
            }
            else
            {
                res = res.ToLower();
                if (res.IndexOf("access") != -1 || res.IndexOf("ok") != -1)
                {
                    UISync.Execute(() => rtbOutput.Text += "OK");
                }
                else
                {
                    UISync.Execute(() => rtbOutput.Text += "Unexpected output: " + res);
                }
            }


            string localcatalogserver = "http://localhost:8085/MEBSCatalog";

            UISync.Execute(() => rtbOutput.Text += NL);
            UISync.Execute(() => rtbOutput.Text += "Checking your local catalog... ");
            Application.DoEvents();

            if (!loadurl(localcatalogserver + "/Ping", out res))
            {
                UISync.Execute(() => rtbOutput.Text += "Failed: " + res + NL);
            }
            else {

                res = res.ToLower();
                if (res.IndexOf("Ping") == -1)
                {
                    UISync.Execute(() => rtbOutput.Text += "OK.");
                }
                else
                {
                    UISync.Execute(() => rtbOutput.Text += "Unexpected output: " + res);
                }
            }

            UISync.Execute(() => rtbOutput.Text += NL);
            UISync.Execute(() => rtbOutput.Text += "Checking your local Mysql Service... ");
            Application.DoEvents();

            if (!loadurl(localcatalogserver + "/mebs_settings", out res))
            {
                UISync.Execute(() => rtbOutput.Text += "Failed: " + res + NL);
            }
            else
            {

                UISync.Execute(() => rtbOutput.Text += "OK.");
              
            }

            UISync.Execute(() => rtbOutput.Text += NL);
            UISync.Execute(() => rtbOutput.Text += "Checking your firewall... ");
            Application.DoEvents();


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
                    UISync.Execute(() => rtbOutput.Text += "MTV.Scheduler.App is *not* enabled");
                }
                else
                {
                    UISync.Execute(() => rtbOutput.Text += "MTV.Scheduler.App is enabled");
                }
            }
            else
            {
                UISync.Execute(() => rtbOutput.Text += "Firewall is off");
            }

            UISync.Execute(() => rtbOutput.Text += NL);


            if (MainForm.Conf.IPMode == "IPv4")
            {

                UISync.Execute(() => rtbOutput.Text += "IPv4: Checking port mappings... " + NL);
                try
                {
                    if (NATControl.Mappings == null)
                    {
                        UISync.Execute(
                            () =>
                            rtbOutput.Text +=
                            "IPv4 Port mappings are unavailable - set up manually, instructions here: http://portforward.com/english/routers/port_forwarding/routerindex.htm" +
                            NL);
                    }
                    else
                    {
                        int j = 2;
                        while (!portMapOk && j > 0)
                        {
                            var enumerator = NATControl.Mappings.GetEnumerator();

                            while (enumerator.MoveNext())
                            {
                                var map = (NATUPNPLib.IStaticPortMapping)enumerator.Current;
                                UISync.Execute(
                                    () =>
                                    rtbOutput.Text +=
                                    map.ExternalPort + " -> " + map.InternalPort + " on " + map.InternalClient +
                                    " (" +
                                    map.Protocol + ")" + NL);
                                if (map.ExternalPort == MainForm.Conf.ServerPort)
                                {
                                    if (map.InternalPort != MainForm.Conf.LANPort)
                                    {
                                        UISync.Execute(
                                            () =>
                                            rtbOutput.Text +=
                                            "*** External port is routing to " + map.InternalPort +
                                            " instead of " +
                                            MainForm.Conf.LANPort + NL);
                                    }
                                    else
                                    {
                                        if (map.InternalClient != MainForm.AddressIPv4)
                                        {
                                            UISync.Execute(
                                                () =>
                                                rtbOutput.Text +=
                                                "*** Port is mapping to IP Address " + map.InternalClient +
                                                " - should be " +
                                                MainForm.AddressIPv4 +
                                                ". Set a static IP address for your computer and then update the port mapping." +
                                                NL);
                                        }
                                        else
                                        {
                                            portMapOk = true;
                                        }
                                    }
                                }
                            }
                            if (!portMapOk)
                            {
                                //add port mapping
                                UISync.Execute(() => rtbOutput.Text += "IPv4: Fixing port mapping... " + NL);
                                if (!NATControl.SetPorts(MainForm.Conf.ServerPort, MainForm.Conf.LANPort))
                                {
                                    UISync.Execute(
                                        () => rtbOutput.Text += LocRm.GetString("ErrorPortMapping") + NL);
                                }

                                j--;
                                if (j > 0)
                                    UISync.Execute(
                                        () => rtbOutput.Text += "IPv4: Checking port mappings... " + NL);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                }
            }


            Application.DoEvents();
            UISync.Execute(() => button2.Enabled = true);


        }

        private void button2_Click(object sender, EventArgs e)
        {
            var t = new Thread(Troubleshooter) { IsBackground = false };
            t.Start();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Clipboard.SetText(rtbOutput.Text);
        }

    }
}
