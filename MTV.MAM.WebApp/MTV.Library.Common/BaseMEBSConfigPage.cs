using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Diagnostics;

namespace MTV.Library.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseMEBSConfigPage : Page
    {
        /// <summary>
        /// 
        /// </summary>
        protected Stopwatch executionTimer;

        /// <summary>
        /// 
        /// </summary>
        protected bool showExecutionTimer = false;

        /// <summary>
        /// Defalut construtor for the class.
        /// </summary>
        public BaseMEBSConfigPage()
        {
            showExecutionTimer = true; //Convert.ToBoolean(SettingsProvider.GetSettings("ShowExecutionTimer").Value);
            if (showExecutionTimer)
            {
                executionTimer = new Stopwatch();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            if (showExecutionTimer)
            {
                executionTimer.Start();
            }

            base.OnInit(e);

            if (showExecutionTimer)
            {
                executionTimer.Stop();
            }
        }

        /// <summary>
        /// Execute In the Load of Page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            //if (showExecutionTimer)
            //{
            //    executionTimer.Start();
            //}

            //base.OnLoad(e);

            //if (showExecutionTimer)
            //{
            //    executionTimer.Stop();
            //}
        }


        /// <summary>
        /// Execute on the PreInit of Page
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            SetPageTitle();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void CreateChildControls()
        {
            if (showExecutionTimer)
            {
                executionTimer.Start();
            }
            base.CreateChildControls();

            if (showExecutionTimer)
            {
                executionTimer.Stop();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected override void Render(HtmlTextWriter writer)
        {

            if (showExecutionTimer)
            {
                executionTimer.Start();
            }
            base.Render(writer);

            if (showExecutionTimer)
            {
                executionTimer.Stop();
                RenderExecutionTimerValue(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        protected virtual void RenderExecutionTimerValue(HtmlTextWriter writer)
        {
            if (showExecutionTimer)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<div style=""color:#ffffff;background:#000000;font-weight:bold,padding:5px"">");
                sb.Append(String.Format("Page execution time is {0:F10}.<br />", executionTimer.Elapsed.TotalSeconds));
                sb.Append(@"</div>");
                writer.Write(sb.ToString());
            }
        }


        /// <summary>
        /// Set the Page Title
        /// </summary>
        protected void SetPageTitle()
        {
            Page.Title = "MEBS Configuration & Administration";
        }


    }
}
