using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTV.Scheduler.App.UI
{
    public partial class ErrorReporting : Form
    {
        public Exception UnhandledException;
        public ErrorReporting()
        {
            InitializeComponent();
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            try
            {
                string g = Guid.NewGuid().ToString();
            }
            catch { }
            MainForm.Conf.Enable_Error_Reporting = chkErrorReporting.Checked;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            MainForm.Conf.Enable_Error_Reporting = chkErrorReporting.Checked;
            Close();
        }

        private void ErrorReport_Load(object sender, EventArgs e)
        {
            chkErrorReporting.Checked = MainForm.Conf.Enable_Error_Reporting;
            txtErrorReport.Text = UnhandledException.Message + Environment.NewLine + Environment.NewLine +
                                  UnhandledException.StackTrace;
            txtHumanDescription.Text = "";
        }


    }
}
