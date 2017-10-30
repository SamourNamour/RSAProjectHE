using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace MTV.Library.Common
{
    /// <summary>
    /// Represents a personalized EBSMAM Use Control
    /// </summary>
    public class BaseMEBSMAMUserControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public BaseMEBSMAMUserControl()
        {

        }


        /// <summary>
        /// Shows the Exception message
        /// </summary>
        /// <param name="exc">The handled exception</param>
        protected void ProcessException(Exception exc)
        {

            bool ShowErrors = true; //Convert.ToBoolean(SettingsProvider.GetSettings("Show_Error").Value);
            if (ShowErrors) // if param true
            {
                ShowError(exc.Message, exc.ToString());
            }
            else
            {
                ShowError(exc.Message, string.Empty);
            }
        }

        /// <summary>
        /// Shows a message into the user control.
        /// </summary>
        /// <param name="Message">string of the message to be shown</param>
        protected void ShowMessage(string Message)
        {
            if (this.Page == null)
                return;

            MasterPage masterPage = this.Page.Master;
            if (masterPage == null)
                return;

            BaseMEBSMAMMasterPage BaseMEBSMAMMasterPage = masterPage as BaseMEBSMAMMasterPage;
            if (BaseMEBSMAMMasterPage != null)
                BaseMEBSMAMMasterPage.ShowMessage(Message, string.Empty);
        }

        /// <summary>
        /// Shows an error message into the User Control.
        /// </summary>
        /// <param name="Message">string of the error to be shown</param>
        protected void ShowError(string Message)
        {
            ShowError(Message, string.Empty);
        }

        /// <summary>
        /// Shows an error message into the User Control.
        /// </summary>
        protected void ShowError(string Message, string CompleteMessage)
        {
            if (this.Page == null)
                return;

            MasterPage masterPage = this.Page.Master;
            if (masterPage == null)
                return;

            BaseMEBSMAMMasterPage BaseMEBSMAMMasterPage = masterPage as BaseMEBSMAMMasterPage;
            if (BaseMEBSMAMMasterPage != null)
                BaseMEBSMAMMasterPage.ShowError(Message, CompleteMessage);
        }
    }
}
