using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using AUTH = MTV.MAM.WebApp.Authentication;
using System.Web.Security;
using System.Data.Services.Client;
using System.Globalization;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;

namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class UserInfoControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class :  Field(s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        mebs_login _CurrentUser;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        //------ OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                if(!string.IsNullOrEmpty(this.UserName))
                    _CurrentUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByName, this.UserName), UriKind.Relative)).FirstOrDefault();

                if (!Page.IsPostBack)
                {
                    this.FillCountryList();
                    this.SetUserRoles();
                    this.BindData();
                }
            }
            catch(Exception ex)
            {
                LogHelper.logger.Error(string.Format("UserInfoControl : Page_Load : {0}", ex.Message));
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        //---- OK
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            try
            {
                if (string.IsNullOrEmpty(this.UserName))
                    return;
                AUTH.MySqlMembershipProvider ebsMembershipProvider = new AUTH.MySqlMembershipProvider();
                AUTH.MySqlRoleProvider ebsRoleProvider = new AUTH.MySqlRoleProvider();

                MembershipUser objMembershipUser = ebsMembershipProvider.GetUser(this.UserName, false);

                if (objMembershipUser != null)
                {
                    if (objMembershipUser.UserName.ToLower() == "user" || objMembershipUser.UserName.ToLower() == "administrator")
                    {
                        this.txtLogin.Enabled = false;
                        UserDetailsControl _parentControl = (UserDetailsControl)this.Parent;
                        if (_parentControl != null)
                        {
                            Button btnDelete = (Button)_parentControl.FindControl("DeleteButton");
                            if (btnDelete != null) btnDelete.Enabled = false;

                            cbIsActive.Enabled = false;
                        }
                    }

                    this.txtLogin.Text = objMembershipUser.UserName;
                    this.txtEmail.Text = objMembershipUser.Email;
                    cbIsActive.Checked = objMembershipUser.IsApproved;

                    string[] arrayUserRoles = ebsRoleProvider.GetRolesForUser(objMembershipUser.UserName);

                    if (arrayUserRoles != null &&
                        arrayUserRoles.Length > 0)
                    {
                        foreach (ListItem cb in rblRoles.Items)
                        {
                            foreach (string var in arrayUserRoles)
                            {
                                if (string.Compare(var, cb.Text) == 0)
                                {
                                    cb.Selected = true;
                                    break;
                                }
                            }
                        }
                    }

                   
                        //DAL.UserProvider.GetUser(objMembershipUser.UserName);
                    if (_CurrentUser != null
                        && _CurrentUser.mebs_userdetails != null
                        && _CurrentUser.mebs_userdetails.Count > 0)
                    {
                        this.txtPassword.Text = _CurrentUser.Password;
                        this.txtPasswordAnswer.Text = _CurrentUser.PasswordAnswer;
                        this.txtPasswordQuestion.Text = _CurrentUser.PasswordQuestion;
                        this.txtFirstName.Text = _CurrentUser.mebs_userdetails[0].FirstName;
                        this.txtLastName.Text = _CurrentUser.mebs_userdetails[0].LastName;
                        this.txtStreetAddress.Text = _CurrentUser.mebs_userdetails[0].StreetAddress;
                        this.txtStreetAddress2.Text = _CurrentUser.mebs_userdetails[0].StreetAddress2;
                        this.txtPhone.Text = _CurrentUser.mebs_userdetails[0].Phone;
                        this.txtMobile.Text = _CurrentUser.mebs_userdetails[0].Mobile;
                        this.txtPostalCode.Text = _CurrentUser.mebs_userdetails[0].PostalCode;
                        this.txtPostalCode.Text = _CurrentUser.mebs_userdetails[0].PostalCode;
                        this.txtDescription.Text = _CurrentUser.mebs_userdetails[0].Comment;
                        this.cbIsActive.Checked = _CurrentUser.IsActive.Value;
                        this.rblGender.Items.FindByText(_CurrentUser.mebs_userdetails[0].Gender).Selected = true;
                        if (this.ddlCity.Items.FindByText(_CurrentUser.mebs_userdetails[0].City) != null)
                        {
                            this.ddlCity.Items.FindByText(_CurrentUser.mebs_userdetails[0].City).Selected = true;
                        }
                        if (this.ddlCountry.Items.FindByText(_CurrentUser.mebs_userdetails[0].Country) != null)
                        {
                            this.ddlCountry.Items.FindByText(_CurrentUser.mebs_userdetails[0].Country).Selected = true;
                        }
                    }
                }
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
                        LogHelper.logger.Error(string.Format("UserInfoControl : BindData : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UserInfoControl : BindData : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UserInfoControl : BindData : {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MembershipUser SaveInfo()
        {
            MembershipUser objMembershipUser = null;
            try
            {
                AUTH.MySqlMembershipProvider ebsMembershipProvider = new AUTH.MySqlMembershipProvider();
                AUTH.MySqlRoleProvider ebsRoleProvider = new AUTH.MySqlRoleProvider();
                if (_CurrentUser != null)
                {
                    _CurrentUser.Login = txtLogin.Text;
                    _CurrentUser.Email = txtEmail.Text;
                    _CurrentUser.IsActive = cbIsActive.Checked;
                    _CurrentUser.Password = txtPassword.Text;
                    _CurrentUser.PasswordAnswer = txtPasswordAnswer.Text;
                    _CurrentUser.PasswordQuestion = txtPasswordQuestion.Text;
                    _context.UpdateObject(_CurrentUser);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                    if (_CurrentUser.mebs_userdetails != null && _CurrentUser.mebs_userdetails.Count > 0)
                    {
                        _CurrentUser.mebs_userdetails[0].City = string.Empty;
                        _CurrentUser.mebs_userdetails[0].Country = (ddlCountry.SelectedItem != null ? ddlCountry.SelectedItem.Text : string.Empty);
                        _CurrentUser.mebs_userdetails[0].FirstName = txtFirstName.Text; ;
                        _CurrentUser.mebs_userdetails[0].Gender = rblGender.SelectedItem.Text;
                        _CurrentUser.mebs_userdetails[0].LastName = txtLastName.Text;
                        _CurrentUser.mebs_userdetails[0].Mobile = txtMobile.Text;
                        _CurrentUser.mebs_userdetails[0].Phone = txtPhone.Text;
                        _CurrentUser.mebs_userdetails[0].PostalCode = txtPostalCode.Text;
                        _CurrentUser.mebs_userdetails[0].StreetAddress = txtStreetAddress.Text;
                        _CurrentUser.mebs_userdetails[0].StreetAddress2 = txtStreetAddress2.Text;
                        //_CurrentUser.mebs_userdetails[0].RegistrationDate = (_CurrentUser.mebs_userdetails[0].RegistrationDate > DateTime.MinValue );
                        _CurrentUser.mebs_userdetails[0].Comment = txtDescription.Text;
                        _context.UpdateObject(_CurrentUser.mebs_userdetails[0]);
                    }
                    else
                    {
                        mebs_userdetails objNewUserDetails = new mebs_userdetails();
                        objNewUserDetails.City = string.Empty;
                        objNewUserDetails.Country = (ddlCountry.SelectedItem != null ? ddlCountry.SelectedItem.Text : string.Empty);
                        objNewUserDetails.FirstName = txtFirstName.Text; ;
                        objNewUserDetails.Gender = rblGender.SelectedItem.Text;
                        objNewUserDetails.LastName = txtLastName.Text;
                        objNewUserDetails.Mobile = txtMobile.Text;
                        objNewUserDetails.Phone = txtPhone.Text;
                        objNewUserDetails.PostalCode = txtPostalCode.Text;
                        objNewUserDetails.RegistrationDate = DateTime.Now.ToUniversalTime();
                        objNewUserDetails.StreetAddress = txtStreetAddress.Text;
                        objNewUserDetails.StreetAddress2 = txtStreetAddress2.Text;
                        objNewUserDetails.Comment = txtDescription.Text;

                        _context.AddTomebs_userdetails(objNewUserDetails);
                        _context.AddLink(_CurrentUser, "mebs_userdetails", objNewUserDetails);
                    }

                    foreach (ListItem roleItem in rblRoles.Items)
                    {
                        if (roleItem.Selected)
                        {
                            if (!ebsRoleProvider.IsUserInRole(
                                _CurrentUser.Login,
                                roleItem.Text))
                            {
                                // Add current role to user roles :
                                //ebsRoleProvider.AddUserToRole(objMembershipUser.UserName, roleItem.Text);
                                mebs_usersinroles ObjUserInRole = new mebs_usersinroles();

                                ObjUserInRole.RoleName = roleItem.Text;
                                ObjUserInRole.UserName = _CurrentUser.Login;
                                _context.AddTomebs_usersinroles(ObjUserInRole);
                            }
                        }
                        else
                        {
                            if (ebsRoleProvider.IsUserInRole(
                                _CurrentUser.Login,
                                roleItem.Text))
                            {

                                //mebsEntities _context = new mebsEntities(Config.MEBSCatalogLocation);
                                mebs_usersinroles ObjUserInRole = _context.Execute<mebs_usersinroles>(new Uri(string.Format(Config.IsUserInRole, _CurrentUser.Login, roleItem.Text), UriKind.Relative)).FirstOrDefault();
                                if (ObjUserInRole != null)
                                    _context.DeleteObject(ObjUserInRole);
                                // Remove current role from user roles :
                                //ebsRoleProvider.RemoveUserFromRole(objMembershipUser.UserName, roleItem.Text);
                            }
                        }
                    }
                }
                else
                {
                    _CurrentUser = new mebs_login();
                    _CurrentUser.Email = txtEmail.Text;
                    _CurrentUser.IsActive = cbIsActive.Checked;
                    _CurrentUser.Password = txtPassword.Text;
                    _CurrentUser.PasswordAnswer = txtPasswordAnswer.Text;
                    _CurrentUser.PasswordQuestion = txtPasswordQuestion.Text;
                    _CurrentUser.LastActivityDate = DateTime.MinValue;
                    _CurrentUser.LastLoginDate = DateTime.MinValue;
                    _CurrentUser.Login = txtLogin.Text;
                    _CurrentUser.UserGUID = Guid.NewGuid().ToString();
                    _context.AddTomebs_login(_CurrentUser);

                    mebs_userdetails objNewUserDetails = new mebs_userdetails();
                    objNewUserDetails.City = string.Empty;
                    objNewUserDetails.Country = (ddlCountry.SelectedItem != null ? ddlCountry.SelectedItem.Text : string.Empty);
                    objNewUserDetails.FirstName = txtFirstName.Text; ;
                    objNewUserDetails.Gender = rblGender.SelectedItem.Text;
                    objNewUserDetails.LastName = txtLastName.Text;
                    objNewUserDetails.Mobile = txtMobile.Text;
                    objNewUserDetails.Phone = txtPhone.Text;
                    objNewUserDetails.PostalCode = txtPostalCode.Text;
                    objNewUserDetails.RegistrationDate = DateTime.Now.ToUniversalTime();
                    objNewUserDetails.StreetAddress = txtStreetAddress.Text;
                    objNewUserDetails.StreetAddress2 = txtStreetAddress2.Text;
                    objNewUserDetails.Comment = txtDescription.Text;

                    _context.AddTomebs_userdetails(objNewUserDetails);
                    _context.AddLink(_CurrentUser, "mebs_userdetails", objNewUserDetails);


                    foreach (ListItem roleItem in rblRoles.Items)
                    {
                        if (roleItem.Selected)
                        {
                            if (!ebsRoleProvider.IsUserInRole(
                                _CurrentUser.Login,
                                roleItem.Text))
                            {
                                // Add current role to user roles :
                                //ebsRoleProvider.AddUserToRole(objMembershipUser.UserName, roleItem.Text);
                                mebs_usersinroles ObjUserInRole = new mebs_usersinroles();

                                ObjUserInRole.RoleName = roleItem.Text;
                                ObjUserInRole.UserName = _CurrentUser.Login;
                                _context.AddTomebs_usersinroles(ObjUserInRole);
                            }
                        }
                    }

                }
                _context.SaveChanges(SaveChangesOptions.Batch);
                objMembershipUser = ebsMembershipProvider.GetUser(_CurrentUser.Login, false);


                return objMembershipUser;

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
                        LogHelper.logger.Error(string.Format("UserInfoControl : SaveInfo : {0} - {1}", innerException.Code, innerException.Message));
                        ShowError(innerException.Message);
                        return null;
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UserInfoControl : SaveInfo : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UserInfoControl : SaveInfo : {0}", ex.Message));
                }

                return objMembershipUser; ;
            }
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<string> GetCountryList()
        {
            List<string> cultureList = null;
            try
            {
                //create a new Generic list to hold the country names returned
                cultureList = new List<string>();

                //create an array of CultureInfo to hold all the cultures found, these include the users local cluture, and all the
                //cultures installed with the .Net Framework
                CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);

                //loop through all the cultures found
                foreach (CultureInfo culture in cultures)
                {
                    
                    //pass the current culture's Locale ID (http://msdn.microsoft.com/en-us/library/0h88fahh.aspx)
                    //to the RegionInfo contructor to gain access to the information for that culture
                    RegionInfo region = new RegionInfo(culture.LCID);

                    //make sure out generic list doesnt already
                    //contain this country
                    if (!(cultureList.Contains(region.EnglishName)))
                        //not there so add the EnglishName (http://msdn.microsoft.com/en-us/library/system.globalization.regioninfo.englishname.aspx)
                        //value to our generic list
                        cultureList.Add(region.EnglishName);

                }
                cultureList.Sort();
                return cultureList;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("UserInfoControl : GetCountryList : {0}", ex.Message));
                return cultureList;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void FillCountryList()
        {
            try
            {
                ddlCountry.Items.Clear();
                ddlCountry.ClearSelection();

                if (GetCountryList() != null && GetCountryList().Count > 0)
                {
                    foreach (string var in GetCountryList())
                    {
                        ddlCountry.Items.Add(
                            new ListItem(var, var)
                            );
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("UserInfoControl : FillCountryList : {0}", ex.Message));
            }
        }

        private void FillUserRolesRadioBtn()
        {
            rblRoles.Items.Add(new ListItem(BLC.UsersRoles.MEBSAdmin.ToString(), BLC.UsersRoles.MEBSAdmin.ToString()));
            rblRoles.Items.Add(new ListItem(BLC.UsersRoles.MEBSMAM.ToString(), BLC.UsersRoles.MEBSMAM.ToString()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        public string GetErrorMessage(MembershipCreateStatus status)
        {
            switch (status)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "Username already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A username for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetUserRoles()
        {
            try
            {
                AUTH.MySqlRoleProvider ebsRoleProvider = new AUTH.MySqlRoleProvider();

                string[] arrayUserRoles = ebsRoleProvider.GetAllRoles();
                if (arrayUserRoles != null &&
                    arrayUserRoles.Length > 0)
                {
                    rblRoles.DataSource = arrayUserRoles;
                    rblRoles.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("UserInfoControl : SetUserRoles : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool DeleteUserInfo()
        {
            try
            {
                if (_CurrentUser != null)
                {
                    if (_CurrentUser.mebs_userdetails != null && _CurrentUser.mebs_userdetails.Count > 0)
                    {
                        foreach (mebs_userdetails item in _CurrentUser.mebs_userdetails)
                            _context.DeleteObject(item);
                    }
                    if (_CurrentUser.mebs_session != null && _CurrentUser.mebs_session.Count > 0)
                    {
                        foreach (mebs_session item in _CurrentUser.mebs_session)
                            _context.DeleteObject(item);
                    }

                    if (_CurrentUser.mebs_useractivity != null && _CurrentUser.mebs_useractivity.Count > 0)
                    {
                        foreach (mebs_useractivity item in _CurrentUser.mebs_useractivity)
                            _context.DeleteObject(item);
                    }
                    
                    string[] arrayUserRoles = new AUTH.MySqlRoleProvider().GetRolesForUser(_CurrentUser.Login);

                    if (arrayUserRoles != null &&
                        arrayUserRoles.Length > 0)
                    {
                        foreach (string item in arrayUserRoles)
                        {
                            mebs_usersinroles ObjUserInRole = _context.Execute<mebs_usersinroles>(new Uri(string.Format(Config.IsUserInRole, _CurrentUser.Login, item), UriKind.Relative)).FirstOrDefault();
                            if (ObjUserInRole != null)
                                _context.DeleteObject(ObjUserInRole);
                        }
                    }

                    _context.DeleteObject(_CurrentUser);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                    return true;
                }
                return false;
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
                        LogHelper.logger.Error(string.Format("SettingsInfoControl : DeleteButton_Click : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SettingsInfoControl : DeleteButton_Click : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("SettingsInfoControl : DeleteButton_Click : {0}", ex.Message));
                }
            }

            return false;
        }

        #endregion 

        #region -.-.-.-.-.-.-.-.-.-.-.- Class :  Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
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
    }
}