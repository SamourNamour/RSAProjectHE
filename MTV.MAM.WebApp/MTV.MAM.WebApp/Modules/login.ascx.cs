using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using AUTH = MTV.MAM.WebApp.Authentication;
using System.Web.Security;
using MTV.MAM.WebApp.Helper;
using System.ServiceModel;

namespace MTV.MAM.WebApp.Modules
{
    public partial class loginControl : BLC.BaseMEBSMAMUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
                
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LoginForm_LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                AUTH.MySqlRoleProvider ebsRoleProvider = new AUTH.MySqlRoleProvider();
                AUTH.MySqlMembershipProvider ebsMembershipProvider = new AUTH.MySqlMembershipProvider();

                MembershipUser loggedUser = ebsMembershipProvider.GetUser(LoginForm_UserName.Text, false);
                if (loggedUser != null)
                {
                    if (loggedUser.IsApproved)
                    {
                        // Check whether the current logged-on membership user is in the specified role.
                        bool IsUser = ebsRoleProvider.IsUserInRole(loggedUser.UserName, BLC.UsersRoles.MEBSMAM.ToString());
                        bool IsAdmin = ebsRoleProvider.IsUserInRole(loggedUser.UserName, BLC.UsersRoles.MEBSAdmin.ToString());
                        if ( IsAdmin|| IsUser)
                        {
                            if (ebsMembershipProvider.ValidateUser(
                                LoginForm_UserName.Text,
                                LoginForm_Password.Text))
                            {
                                FormsAuthentication.RedirectFromLoginPage(loggedUser.UserName, true);
                                loggedUser.LastActivityDate = loggedUser.LastLoginDate = DateTime.Now.ToUniversalTime();
                                ebsMembershipProvider.UpdateUser(loggedUser);
                                if (IsUser)
                                    Response.Redirect(@"VodSystemOutput.aspx", false);
                                else if(IsAdmin)
                                    Response.Redirect(@"Admin/CatalogHome.aspx", false);
                                else
                                    Response.Redirect(@"Login.aspx", false);
                            }
                            else
                            {
                                lblError.Text = "The password or Login you entered is incorrect";
                                lblError.Visible = true;
                            }
                        }
                        else
                        {
                            lblError.Text = "Only Users with MEBSMAM or Administrator roles can get access.";
                            lblError.Visible = true;
                        }
                    }
                    else
                    {
                        lblError.Text = "Your account has been locked check with your Administrator";
                        lblError.Visible = true;
                    }
                }
                else
                {
                    lblError.Text = "The username you entered is incorrect";
                    lblError.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("loginControl : LoginForm_LoginButton_Click : {0}", ex.ToString()));
            }
        }
    }
}