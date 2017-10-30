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
using MTV.MAM.WebApp.Helper;
#endregion 

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class UserDetailsControl : BLC.BaseMEBSMAMUserControl
    {
        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            get
            {
                return BLC.CommonHelper.QueryString("UserName");
            }
        }
        #endregion 

        #region Event(s)
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
                try
                {
                    MembershipUser user = ctrlUserInfo.SaveInfo();
                    Response.Redirect("UserDetails.aspx?UserName=" + user.UserName,false);
                }
                catch (Exception ex)
                {
                    LogHelper.logger.Error(string.Format("UserDetailsControl : SaveButton_Click : {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                //DAL.UserProvider.DeleteUser(this.UserName);
                if(ctrlUserInfo.DeleteUserInfo())
                    Response.Redirect("Users.aspx",false);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("UserDetailsControl : DeleteButton_Click : {0}", ex.Message));
            }
        }
        #endregion 
    }
}