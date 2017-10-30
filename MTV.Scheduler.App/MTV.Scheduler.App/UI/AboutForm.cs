
#region - Copyright Motive Television 2012 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: AboutForm.cs
//
#endregion

#region - Using Directive(s) -
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

// Custom Directive(s)
#endregion 

namespace MTV.Scheduler.App.UI
{
    /// <summary>
    /// 
    /// </summary>
    public partial class AboutForm : Form {
           
        /// <summary>
        /// 
        /// </summary>
        public AboutForm() {
            InitializeComponent();
            RenderResources();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MainForm.StartBrowser("http://www.motivetelevision.co.uk/");
        }

        private void RenderResources()
        {
            lblVersion.Text = "MTV.Scheduler.APP v" + Application.ProductVersion + " " + "[Open Beta]";
            Text = "About MTV.Scheduler.APP";
            label1.Text = "Homepage";
            lblCopyright.Text = "Copyright " + DateTime.Now.Year;
        }



     

      

     

       

       
    }
}
