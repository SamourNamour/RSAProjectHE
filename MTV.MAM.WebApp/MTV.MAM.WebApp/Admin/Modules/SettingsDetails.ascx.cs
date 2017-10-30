#region -.-.-.-.-.-.-.-.-.-.-.- Class : Name Space(s) -.-.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using AUTH = MTV.MAM.WebApp.Authentication;
using System.Data.Services.Client;
using BLC = MTV.Library.Common;
using System.Xml;
using System.Threading;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class SettingsDetailsControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
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
                LogHelper.logger.Error(string.Format("SettingsDetailsControl : Page_Load : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
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
                    //DAL.Log.Write(string.Format("SettingsDetails : SaveButton_Click : {0}", exc.ToString()));
                    LogHelper.logger.Error(string.Format("SettingsDetailsControl : SaveButton_Click : {0}", ex.Message));
                }
            }
        }
        #endregion 

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        public int SettingID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("SettingID");
            }
        }
        #endregion 
    }
}