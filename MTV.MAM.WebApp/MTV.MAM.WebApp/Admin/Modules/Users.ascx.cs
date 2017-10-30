using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using System.Data.Services.Client;
using System.Web.Security;
using AUTH = MTV.MAM.WebApp.Authentication;
using MTV.MAM.WebApp.Helper;
namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class UsersControl : BLC.BaseMEBSMAMUserControl
    {
        bool bAllUsers = false;
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        //---- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                SetDefaultValues();
            }
        }

        //---- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                AUTH.MySqlRoleProvider ebsRoleProvider = new AUTH.MySqlRoleProvider();
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    string strUserName = e.Row.Cells[0].Text;
                    if (!string.IsNullOrEmpty(strUserName))
                    {
                        GridView gvUserRoles = (GridView)e.Row.FindControl("gvUserRoles");
                        if (gvUserRoles != null)
                        {
                            string[] arrayUserRoles = ebsRoleProvider.GetRolesForUser(strUserName);
                            if (arrayUserRoles != null &&
                                arrayUserRoles.Length > 0)
                            {
                                gvUserRoles.DataSource = arrayUserRoles;
                                gvUserRoles.DataBind();
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                LogHelper.logger.Error(string.Format("UsersControl : gvUsers_RowDataBound : {0}", exc.Message));
            }
        }

        //---- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvUsers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvUsers.PageIndex = e.NewPageIndex;
                BindGrid();
            }
            catch (Exception exc)
            {
                LogHelper.logger.Error(string.Format("UsersControl : gvUsers_PageIndexChanging : {0}", exc.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SearchButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {

                    bAllUsers = false;
                    BindGrid();
                }
                catch (Exception exc)
                {
                    LogHelper.logger.Error(string.Format("UsersControl : SearchButton_Click : {0}", exc.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAllusers_Click(object sender, EventArgs e)
        {
            try
            {
                bAllUsers = true;
                BindGrid();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSConfigHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : btnAllusers_Click : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : btnAllusers_Click : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UsersControl : btnAllusers_Click : {0}", ex.Message));
                }
            }

        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// Set search users time interval boundaries (1 year).
        /// </summary>
        private void SetDefaultValues()
        {
            try
            {
                txtStartDate.Text = DateTime.Now.AddYears(-1).ToString(BLC.DefaultValue.DateTimeFormat);
                txtEndDate.Text = DateTime.Now.AddMonths(1).ToString(BLC.DefaultValue.DateTimeFormat);
            }
            catch (Exception exc)
            {
                LogHelper.logger.Error(string.Format("UsersControl : SetDefaultValues : {0}", exc.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void BindGrid()
        {
            try
            {
                MembershipUserCollection listOfAllUsers = new MembershipUserCollection();
                if (bAllUsers)
                {
                    AUTH.MySqlMembershipProvider ebsMembershipProvider = new AUTH.MySqlMembershipProvider();
                    listOfAllUsers = ebsMembershipProvider.GetAllUsers();
                }
                else
                {
                    listOfAllUsers = GetMembershipUserCollection(
                        Convert.ToDateTime(txtStartDate.Text),
                        Convert.ToDateTime(txtEndDate.Text),
                        txtUsername.Text.Replace(" ", ""),
                        txtEmail.Text.Replace(" ", "")
                        );
                }

                gvUsers.DataSource = listOfAllUsers;
                gvUsers.DataBind();

            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSConfigHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : BindGrid : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : BindGrid : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UsersControl : BindGrid : {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="login"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        private MembershipUserCollection GetMembershipUserCollection(
            DateTime start,
            DateTime end,
            string login,
            string email)
        {
            MembershipUserCollection MembershipUsersFind = null;
            try
            {
                AUTH.MySqlMembershipProvider ebsMembershipProvider = new AUTH.MySqlMembershipProvider();
                MembershipUserCollection listOfAllUsers = ebsMembershipProvider.GetAllUsers();
                if (listOfAllUsers != null &&
                    listOfAllUsers.Count > 0)
                {
                    MembershipUsersFind = new MembershipUserCollection(); ;
                    foreach (MembershipUser var in listOfAllUsers)
                    {
                        if ((var.CreationDate >= start) &&
                            (var.CreationDate <= end) &&
                            var.UserName.Contains(login) &&
                            var.Email.Contains(email))
                        {
                            MembershipUsersFind.Add(var);
                        }
                    }
                }
                return MembershipUsersFind;
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSConfigHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : GetMembershipUserCollection : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UsersControl : GetMembershipUserCollection : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UsersControl : GetMembershipUserCollection : {0}", ex.Message));
                }
                return MembershipUsersFind;
            }
        }
        #endregion         
    }
}