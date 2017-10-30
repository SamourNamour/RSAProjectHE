using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using System.Web.Security;
using MTV.MAM.WebApp.Helper;
namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class UserAddControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class :  Event(s) -.-.-.-.-.-.-.-.-.-.-.-
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
                LogHelper.logger.Error(string.Format("UserAddControl : Page_Load : {0}", ex.Message));
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
                MembershipUser user = ctrlCustomerInfo.SaveInfo();
                if (user != null)
                {
                    Response.Redirect("UserDetails.aspx?UserName=" + user.UserName, false);
                }
            }
        }
        #endregion        
    }
}