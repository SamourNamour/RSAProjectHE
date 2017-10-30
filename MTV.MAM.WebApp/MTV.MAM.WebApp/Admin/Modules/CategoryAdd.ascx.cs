#region Name Space(s)
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using BLC = MTV.Library.Common;
#endregion 

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CategoryAddControl : BLC.BaseMEBSMAMUserControl
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
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                MEBSCatalog.mebs_category  item = ctrlCategorizationInfo.SaveInfo();
                if (item != null)
                    Response.Redirect("CategoryDetails.aspx?CategoryID=" + item.IdCategory.ToString(), false);
            }
        }
        #endregion
    }
}