using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTV.MAM.WebApp.MEBSCatalog;

namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class RedundancyDetailsControl : System.Web.UI.UserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

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
                mebs_settings settings = ctrlSystemSettingsInfo.SaveInfo();
                Response.Redirect("SettingsDetails.aspx?SettingID=" + settings.IdSetting.ToString(), false);
            }
        }
        #endregion 
    }
}