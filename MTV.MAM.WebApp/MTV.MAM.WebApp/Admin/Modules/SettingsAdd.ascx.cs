#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Xml;
using System.Threading;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Data.Services.Client;

using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using AUTH = MTV.MAM.WebApp.Authentication;
using BLC = MTV.Library.Common;

#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SettingsAddControl : BLC.BaseMEBSMAMUserControl
    {
        #region Event(s)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("SettingsAddControl : Page_Load : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    mebs_settings settings = ctrlSystemSettingsInfo.SaveInfo();
                    Response.Redirect("SettingsDetails.aspx?SettingID=" + settings.IdSetting.ToString(),false);
                }

                catch (Exception ex)
                {
                    //DAL.Log.Write(string.Format("SettingsAdd : AddButton_Click : {0}", exc.ToString()));
                    LogHelper.logger.Error(string.Format("SettingsAddControl : AddButton_Click : {0}", ex.Message));
                }
            }
        }
        #endregion 
    }
}