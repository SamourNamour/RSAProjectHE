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
    public partial class Feedback : Form
    {
        public Feedback()
        {
            InitializeComponent();
        }

        bool IsValidEmail(string Email)
        {
            if (Email.IndexOf("@") == -1 || Email.IndexOf(".") == -1 || Email.Length < 5)
                return false;
            return true;
        }

        private void btnFeedback_Click(object sender, EventArgs e)
        {
            bool success = false;
            string _Feedback = txtFeedback.Text.ToString().Trim();
            if (_Feedback == "")
            {
                MessageBox.Show("Feedback_PleaseEnter", "Error");
                return;
            }

            string FromEmail = txtEmail.Text.Trim();
            
            if (!IsValidEmail(FromEmail))
            {
                if (MessageBox.Show("Feedback_ValidateEmail", "AreYouSure", MessageBoxButtons.YesNo) == DialogResult.No)
                    return;
            }
          
            try
            {

                if (MainForm.SendAlert(FromEmail, "MTV.Scheduler.APP Feedback", _Feedback + "<br/><br/>Version: " + Application.ProductVersion))
                success = true;
                
                
            }
            catch (Exception ex)
            {
               
                MainForm.LogExceptionToFile(ex);
                MessageBox.Show("Feedback_NotSent", "Error");
            }
            if (success)
            {
                MessageBox.Show("Feedback_Sent", "Note");
                Close();
            }
            else
            {
                MessageBox.Show("Feedback_NotSent", "Error");
                Close();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
