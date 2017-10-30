using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTV.Scheduler.App.MTVControl;
using Microsoft.Win32;
using System.IO;
using MTV.Scheduler.App.HttpServer;

namespace MTV.Scheduler.App.UI
{
    public partial class SettingsForm : Form
    {

        public int InitialTab;
        private RegistryKey _rkApp;
        
        #region Nested type: ListItem

        private struct ListItem
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

        private class UISync
        {
            private static ISynchronizeInvoke _sync;

            public static void Init(ISynchronizeInvoke sync)
            {
                _sync = sync;
            }

            public static void Execute(Action action)
            {
                try { _sync.BeginInvoke(action, null); }
                catch { }
            }
        }
        
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void ReloadLanguages()
        {
            ddlLanguage.Items.Clear();
            
            int i = 0, selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {
                ddlLanguage.Items.Add(new ListItem(set.Name, new[] { set.CultureCode }));
                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }
            ddlLanguage.SelectedIndex = selind;
            
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            UISync.Init(this);
            tcTabs.SelectedIndex = InitialTab;
            chkErrorReporting.Checked = MainForm.Conf.Enable_Error_Reporting;
            _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", false);
            chkStartup.Checked = (_rkApp != null && _rkApp.GetValue("MTV.Scheduler.App") != null);
            txtMediaDirectory.Text = MainForm.Conf.ReqDirectory;//MainForm.Conf.MediaDirectory;
            txtDaysDelete.Text = MainForm.Conf.DeleteFilesOlderThanDays.ToString();
            
            txtMaxMediaSize.Value = MainForm.Conf.MaxMediaFolderSizeMB;
            //numMaxCPU.Value = MainForm.Conf.CPUMax;
            gbStorage.Enabled = chkStorage.Checked = MainForm.Conf.Enable_Storage_Management;
            
            txtServerName.Text = MainForm.Conf.ServerName;
            rtbAccessList.Text = MainForm.Conf.AllowedIPList;

            int i = 0, selind = 0;
            foreach (TranslationsTranslationSet set in LocRm.TranslationSets.OrderBy(p => p.Name))
            {
                ddlLanguage.Items.Add(new ListItem(set.Name, new[] { set.CultureCode }));
                if (set.CultureCode == MainForm.Conf.Language)
                    selind = i;
                i++;
            }
            ddlLanguage.SelectedIndex = selind;

            ddlPriority.SelectedIndex = MainForm.Conf.Priority - 1;

            txtSMTPFromAddress.Text = MainForm.Conf.SMTPFromAddress;
            txtSMTPUsername.Text = MainForm.Conf.SMTPUsername;
            txtSMTPPassword.Text = MainForm.Conf.SMTPPassword;
            txtSMTPServer.Text = MainForm.Conf.SMTPServer;
            chkSMTPUseSSL.Checked = MainForm.Conf.SMTPSSL;
            numSMTPPort.Value = MainForm.Conf.SMTPPort;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string err = "";

            if (!Directory.Exists(txtMediaDirectory.Text))
            {
                err += LocRm.GetString("Validate_FTP_Root_Directory") + "\n";
            }

            if (err != "")
            {
                MessageBox.Show(err, LocRm.GetString("Error"));
                return;
            }

            MainForm.Conf.Enable_Error_Reporting = chkErrorReporting.Checked;

            string dir = txtMediaDirectory.Text.Trim();
            if (!dir.EndsWith("\\"))
                dir += "\\";

            if (MainForm.Conf.ReqDirectory != dir)
            {
                MainForm.Conf.ReqDirectory = dir;
             
            }

            MainForm.Conf.MaxMediaFolderSizeMB = Convert.ToInt32(txtMaxMediaSize.Value);
            MainForm.Conf.DeleteFilesOlderThanDays = Convert.ToInt32(txtDaysDelete.Value);
           
            MainForm.Conf.Enable_Storage_Management = chkStorage.Checked;

            MainForm.Conf.ServerName = txtServerName.Text;
            //MainForm.Conf.CPUMax = Convert.ToInt32(numMaxCPU.Value);

            MainForm.Conf.Priority = ddlPriority.SelectedIndex + 1;

            MainForm.SetPriority();

            var ips = rtbAccessList.Text.Trim().Split(',');
            var t = ips.Select(ip => ip.Trim()).Where(ip2 => ip2 != "").Aggregate("", (current, ip2) => current + (ip2 + ","));
            MainForm.Conf.AllowedIPList = t.Trim(',');
            LocalServer.AllowedIPs = null;

            string lang = ((ListItem)ddlLanguage.SelectedItem).Value[0];
            if (lang != MainForm.Conf.Language)
            {
                
                LocRm.CurrentSet = null;
            }
            MainForm.Conf.Language = lang;

            if (chkStartup.Checked)
            {
                try
                {
                    _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    if (_rkApp != null)
                        _rkApp.SetValue("MTV.Scheduler.App", "\"" + Application.ExecutablePath + "\" -silent", RegistryValueKind.String);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainForm.LogExceptionToFile(ex);
                }
            }
            else
            {
                try
                {
                    _rkApp = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                    if (_rkApp != null) _rkApp.DeleteValue("MTV.Scheduler.App", false);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    MainForm.LogExceptionToFile(ex);
                }
            }

            SaveSMTPSettings();

            DialogResult = DialogResult.OK;
            Close();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnBrowseVideo_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(txtMediaDirectory.Text))
                fbdSaveLocation.SelectedPath = txtMediaDirectory.Text;
            fbdSaveLocation.ShowDialog(this);
            if (fbdSaveLocation.SelectedPath != "")
            {
                bool success = false;
                try
                {
                    string path = fbdSaveLocation.SelectedPath;
                    if (!path.EndsWith("\\"))
                        path += "\\";
                    Directory.CreateDirectory(path + "IngestedMedia");
                    Directory.CreateDirectory(path + "ClearMedia");
                    Directory.CreateDirectory(path + "ClearMediaRedundancy");
                    success = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                if (success)
                {
                    txtMediaDirectory.Text = fbdSaveLocation.SelectedPath;
                    if (!txtMediaDirectory.Text.EndsWith(@"\"))
                        txtMediaDirectory.Text += @"\";
                }
            }
        }

        private void btnTestSMTP_Click(object sender, EventArgs e)
        {
            SaveSMTPSettings();
            var p = new Prompt("TestMailTo", MainForm.Conf.SMTPFromAddress);
            if (p.ShowDialog(this) == DialogResult.OK)
            {
                if (MailProvider.Send(p.Val, "test",
                    "MTV.Scheduler.App.Message.Test"))
                {
                    MessageBox.Show(this, "MessageSent");
                }
                else
                {
                    MessageBox.Show(this, "FailedCheckLog");
                }
            }
        }

        private void SaveSMTPSettings()
        {
            
            MainForm.Conf.SMTPFromAddress = txtSMTPFromAddress.Text;
            MainForm.Conf.SMTPUsername = txtSMTPUsername.Text;
            MainForm.Conf.SMTPPassword = txtSMTPPassword.Text;
            MainForm.Conf.SMTPServer = txtSMTPServer.Text;
            MainForm.Conf.SMTPSSL = chkSMTPUseSSL.Checked;
            MainForm.Conf.SMTPPort = (int)numSMTPPort.Value;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ((MainForm)Owner).RunStorageManagement();
        }

    }
}
