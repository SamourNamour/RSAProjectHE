using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTV.Scheduler.App.MTVControl;
using System.Net;

namespace MTV.Scheduler.App.UI
{
    public partial class WebAccessForm : Form
    {
        public static string NL = Environment.NewLine;
        public bool SupportsUpnp;
        
        public WebAccessForm()
        {
            InitializeComponent();
        }

        private bool SetupNetwork(out int port, out int localport, out string error)
        {
            port = Convert.ToInt32(ddlPort.Text);
            localport = (int)txtLANPort.Value;
            if (tcIPMode.SelectedIndex == 1)
            {
                localport = (int)txtPort.Value;
            }

            MainForm.Conf.ServerPort = port;
            MainForm.Conf.LANPort = localport;
            MainForm.MWS.NumErr = 0;

            switch (tcIPMode.SelectedIndex)
            {
                case 0:
                    MainForm.Conf.IPMode = "IPv4";
                    MainForm.Conf.IPv4Address = lbIPv4Address.SelectedItem.ToString();
                    MainForm.AddressIPv4 = MainForm.Conf.IPv4Address;
                    break;
                case 1:
                    MainForm.Conf.IPMode = "IPv6";
                    MainForm.Conf.IPv6Address = lbIPv6Address.SelectedItem.ToString();
                    MainForm.AddressIPv6 = MainForm.Conf.IPv6Address;
                    break;
            }
           

            error = MainForm.StopAndStartServer();
            Application.DoEvents();
            return error == "";
        }

        private void btnTroubleshooting_Click(object sender, EventArgs e)
        {
            ShowTroubleShooter();
        }


        private void ShowTroubleShooter()
        {
            int port, localPort;
            string error = "";
            if (!SetupNetwork(out port, out localPort, out error))
            {
                MessageBox.Show(error);
                return;
            }

            var nt = new NetworkTroubleshooter();
            nt.ShowDialog(this);
            nt.Dispose();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            bool bIPv6 = tcIPMode.SelectedIndex == 1;
            int port, localPort;
            string error = "";

            if (!SetupNetwork(out port, out localPort, out error))
            {
                MessageBox.Show(error + " - Try a different port.");
                return;
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
                        fw.AddApplication(strApplication, "MTVSchedulerApp");
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            Next.Enabled = false;
            Next.Text = "...";
            Application.DoEvents();

            MainForm.Conf.DHCPReroute = chkReroute.Checked;
            bool failed = false;



            if (!bIPv6)
            {
                //try setting port automatically
                if (chkuPNP.Checked)
                {
                    if (!NATControl.SetPorts(port, localPort))
                    {
                        MessageBox.Show(LocRm.GetString("ErrorPortMapping"), LocRm.GetString("Error"));
                        chkuPNP.Checked = false;
                    }
                    
                }

            }


            //MainForm.Conf.Loopback = false;
            Next.Enabled = true;
            Next.Text = LocRm.GetString("Finish");
            //MainForm.LoopBack = false;
            if (!bIPv6)
            {
                switch (
                    MessageBox.Show(
                        LocRm.GetString("ErrorLoopback").Replace("[PORT]", port.ToString()),
                        LocRm.GetString("Error"), MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        ShowTroubleShooter();
                        return;
                    case DialogResult.No:
                        //MainForm.Conf.Loopback = false;
                        //MainForm.LoopBack = false;
                        DialogResult = DialogResult.Yes;
                        Close();
                        return;
                    case DialogResult.Cancel:
                        return;
                }
            }
            else
            {
                switch (
                    MessageBox.Show(
                        LocRm.GetString("ErrorLoopbackIPv6").Replace("[PORT]", localPort.ToString()),
                        LocRm.GetString("Error"), MessageBoxButtons.YesNoCancel))
                {
                    case DialogResult.Yes:
                        ShowTroubleShooter();
                        return;
                    case DialogResult.No:
                        //MainForm.Conf.Loopback = false;
                        //MainForm.LoopBack = false;
                        DialogResult = DialogResult.Yes;
                        Close();
                        return;
                    case DialogResult.Cancel:
                        return;
                }
            }

            
            Next.Enabled = true;
            Next.Text = "Finish"; 
        }

        private void WebAccessForm_Load(object sender, EventArgs e)
        {
            var ports = new[] { "21", "25", "80", "110", "143", "443", "587", "8889", "1433", "3306", "8080", "11433" };
            foreach (string port in ports)
            {
                ddlPort.Items.Add(port);
            }
            for (int i = 23010; i <= 23110; i++)
            {
                ddlPort.Items.Add(i.ToString());
            }
            ddlPort.SelectedItem = MainForm.Conf.ServerPort.ToString();


            
            txtLANPort.Text = MainForm.Conf.LANPort.ToString();
            txtPort.Text = MainForm.Conf.LANPort.ToString();
            chkReroute.Checked = MainForm.Conf.DHCPReroute;

            chkuPNP.Checked = MainForm.Conf.UseUPNP;
            if (!chkuPNP.Checked)
                chkReroute.Checked = chkReroute.Enabled = false;

            lblIPAddresses.Text = LocRm.GetString("PublicIPAddress").Replace("[IP]", MainForm.IPAddressExternal);
            int i2 = 0;
            foreach (IPAddress ipadd in MainForm.AddressListIPv4)
            {
                lbIPv4Address.Items.Add(ipadd.ToString());
                if (ipadd.ToString() == MainForm.AddressIPv4)
                    lbIPv4Address.SelectedIndex = i2;
                i2++;
            }
            if (lbIPv4Address.Items.Count > 0 && lbIPv4Address.SelectedIndex == -1)
                lbIPv4Address.SelectedIndex = 0;

            int _i = 0;
            foreach (IPAddress _ipadd in MainForm.AddressListIPv6)
            {
                lbIPv6Address.Items.Add(_ipadd.ToString());
                if (_ipadd.ToString() == MainForm.AddressIPv6)
                    lbIPv6Address.SelectedIndex = _i;

                _i++;
            }

            if (_i == 0)
                tcIPMode.TabPages.RemoveAt(1);


            if (tcIPMode.TabPages.Count == 2)
            {
                switch (MainForm.Conf.IPMode)
                {
                    case "IPv4":
                        tcIPMode.SelectedIndex = 0;
                        break;
                    case "IPv6":
                        tcIPMode.SelectedIndex = 1;
                        break;
                }
            }
            else
            {
                tcIPMode.SelectedIndex = 0;
                MainForm.Conf.IPMode = "IPv4";
            }
            EnableNext();
        }

        private void EnableNext()
        {
            switch (tcIPMode.SelectedIndex)
            {
                case 0:
                    Next.Enabled = btnTroubleshooting.Enabled = lbIPv4Address.SelectedIndex != -1;
                    break;
                case 1:
                    Next.Enabled = btnTroubleshooting.Enabled = lbIPv6Address.SelectedIndex != -1;
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void tcIPMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableNext();
        }

        private void chkuPNP_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.Conf.UseUPNP = chkuPNP.Checked;
            chkReroute.Checked = chkReroute.Enabled = chkuPNP.Checked;
        }


    }
}
