using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
using System.Configuration.Provider;
using BLC = MTV.Library.Common;

namespace MTV.MAM.WebApp.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlMembershipProvider : MembershipProvider
    {
        #region Variable (s)
        mebsEntities _context;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        public MySqlMembershipProvider()
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
        }
        #endregion

        #region System.Web.Security.MembershipProvider properties.

        private string pApplicationName = "";
        private bool pEnablePasswordReset = false;
        private bool pEnablePasswordRetrieval = false;
        private bool pRequiresQuestionAndAnswer = false;
        private bool pRequiresUniqueEmail = false;
        private int pMaxInvalidPasswordAttempts = int.MinValue;
        private int pPasswordAttemptWindow = int.MinValue;
        private int pMinRequiredNonAlphanumericCharacters = int.MinValue;
        private int pMinRequiredPasswordLength = int.MinValue;
        private string pPasswordStrengthRegularExpression = "";
        private MembershipPasswordFormat pPasswordFormat = MembershipPasswordFormat.Clear;

        /// <summary>
        /// 
        /// </summary>
        public override string ApplicationName
        {
            get { return pApplicationName; }
            set { pApplicationName = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool EnablePasswordReset
        {
            get { return pEnablePasswordReset; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool EnablePasswordRetrieval
        {
            get { return pEnablePasswordRetrieval; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool RequiresQuestionAndAnswer
        {
            get { return pRequiresQuestionAndAnswer; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool RequiresUniqueEmail
        {
            get { return pRequiresUniqueEmail; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MaxInvalidPasswordAttempts
        {
            get { return pMaxInvalidPasswordAttempts; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int PasswordAttemptWindow
        {
            get { return pPasswordAttemptWindow; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override MembershipPasswordFormat PasswordFormat
        {
            get { return pPasswordFormat; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return pMinRequiredNonAlphanumericCharacters; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override int MinRequiredPasswordLength
        {
            get { return pMinRequiredPasswordLength; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string PasswordStrengthRegularExpression
        {
            get { return pPasswordStrengthRegularExpression; }
        }

        #endregion

        #region System.Web.Security.MembershipProvider methods.

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="oldPassword"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
           try
            {
                List<mebs_login> listUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByName, username), UriKind.Relative)).ToList(); //DAL.UserProvider.GetUser(username);

                if (listUser == null || listUser.Count <= 0)
                    return false;

                mebs_login objUser = listUser[0];
                if (objUser != null &&  string.Compare(objUser.Password, oldPassword) == 0)
                {
                    objUser.Password = newPassword;

                    _context.UpdateObject(objUser);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                    return true;
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("MySqlMembershipProvider : ChangePassword : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlMembershipProvider : ChangePassword : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlMembershipProvider : ChangePassword : {0}", ex.Message));
                }
            }

            return false;
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="email"></param>
        /// <param name="passwordQuestion"></param>
        /// <param name="passwordAnswer"></param>
        /// <param name="isApproved"></param>
        /// <param name="providerUserKey"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public override MembershipUser CreateUser(
            string username,
            string password,
            string email,
            string passwordQuestion,
            string passwordAnswer,
            bool isApproved,
            object providerUserKey,
            out MembershipCreateStatus status)
        {
            mebs_login objUser = new mebs_login();
            objUser.Login = username;
            objUser.Password = password;
            objUser.Email = email;
            objUser.PasswordQuestion = passwordQuestion;
            objUser.PasswordAnswer = passwordAnswer;
            objUser.IsActive = isApproved;
            objUser.UserGUID = Convert.ToString(providerUserKey);

            ValidatePasswordEventArgs Args = new ValidatePasswordEventArgs(username, password, true);

            OnValidatingPassword(Args);

            if (Args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                if ((Args.FailureInformation != null))
                {
                    throw Args.FailureInformation;
                }
                else
                {
                    throw new ProviderException("Change password canceled due to New password validation failure.");
                }
            }

            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser u = GetUser(username, false);

            if (u == null)
            {
                DateTime createDate = DateTime.Now;

                if (providerUserKey == null)
                {
                    providerUserKey = Guid.NewGuid();
                }
                else
                {
                    if (!(providerUserKey is Guid))
                    {
                        status = MembershipCreateStatus.InvalidProviderUserKey;
                        return null;
                    }
                }

                try
                {
                    _context.UpdateObject(objUser);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                    status = MembershipCreateStatus.Success;
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is DataServiceClientException)
                    {
                        // Parse the DataServieClientException
                        BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                        // Display the DataServiceClientException message
                        if (innerException != null)
                        {
                            LogHelper.logger.Error(string.Format("MySqlMembershipProvider : CreateUser : {0} - {1}", innerException.Code, innerException.Message));
                        }
                        else
                        {
                            LogHelper.logger.Error(string.Format("MySqlMembershipProvider : CreateUser : {0}", ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlMembershipProvider : CreateUser : {0}", ex.Message));
                    }
                    status = MembershipCreateStatus.UserRejected;
                }

                //if (DAL.UserProvider.AddUser(objUser) != -1)
                //{
                //    status = MembershipCreateStatus.Success;
                //}
                //else
                //{
                //    status = MembershipCreateStatus.UserRejected;
                //}
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="deleteAllRelatedData"></param>
        /// <returns></returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            //if (DAL.UserProvider.DeleteUser(username) != -1)
            //{
            //    return true;
            //}
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="newPasswordQuestion"></param>
        /// <param name="newPasswordAnswer"></param>
        /// <returns></returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encodedPassword"></param>
        /// <returns></returns>
        protected override byte[] DecryptPassword(byte[] encodedPassword)
        {
            return base.DecryptPassword(encodedPassword);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        protected override byte[] EncryptPassword(byte[] password)
        {
            return base.EncryptPassword(password);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="emailToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameToMatch"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection();
            totalRecords = 0;
            _context = new mebsEntities(Config.MTVCatalogLocation);
            List<mebs_login> UserCollection = _context.Execute<mebs_login>(new Uri(Config.GetAllUsers, UriKind.Relative)).ToList();

            if (UserCollection != null &&
                UserCollection.Count > 0)
            {
                foreach (mebs_login var in UserCollection)
                {
                    string comment = (var.mebs_userdetails != null ? var.mebs_userdetails[0].Comment : string.Empty);
                    DateTime registrationDate = (var.mebs_userdetails != null ? var.mebs_userdetails[0].RegistrationDate : DateTime.MinValue);

                    MembershipUser u = new MembershipUser(
                                           "MySqlMembershipProvider",
                                           var.Login,
                                           (object)var.UserGUID,
                                           var.Email,
                                           var.PasswordQuestion,
                                           comment,
                                           var.IsActive.Value,
                                           false,
                                           registrationDate,
                                           var.LastLoginDate.Value,
                                           var.LastActivityDate.Value,
                                           DateTime.MinValue,
                                           DateTime.MinValue);

                    users.Add(u);
                }
            }
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MembershipUserCollection GetAllUsers()
        {
            MembershipUserCollection users = new MembershipUserCollection();

            List<mebs_login> UserCollection = _context.Execute<mebs_login>(new Uri(Config.GetAllUsers, UriKind.Relative)).ToList();
            if (UserCollection != null &&
                UserCollection.Count > 0)
            {
                foreach (mebs_login var in UserCollection)
                {
                    string comment = (var.mebs_userdetails != null && var.mebs_userdetails.Count >0 ? var.mebs_userdetails[0].Comment : string.Empty);
                    DateTime registrationDate = (var.mebs_userdetails != null && var.mebs_userdetails.Count >0 ? var.mebs_userdetails[0].RegistrationDate : DateTime.MinValue);

                    MembershipUser u = new MembershipUser(
                                           "MySqlMembershipProvider",
                                           var.Login,
                                           (object)var.UserGUID,
                                           var.Email,
                                           var.PasswordQuestion,
                                           comment,
                                           var.IsActive.Value,
                                           false,
                                           registrationDate,
                                           var.LastLoginDate.Value,
                                           var.LastActivityDate.Value,
                                           DateTime.MinValue,
                                           DateTime.MinValue);

                    users.Add(u);
                }
            }
            return users;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetNumberOfUsersOnline()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string GetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerUserKey"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="userIsOnline"></param>
        /// <returns></returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser u = null;

            List<mebs_login> listUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByName, username), UriKind.Relative)).ToList(); //DAL.UserProvider.GetUser(username);
            if (listUser == null || listUser.Count <= 0)
                return null;

            mebs_login objUser = listUser[0];

            if (objUser != null)
            {
                u = new MembershipUser(
                    "MySqlMembershipProvider",
                    objUser.Login,
                    (object)objUser.UserId,
                    objUser.Email,
                    string.Empty,
                    string.Empty,
                    objUser.IsActive.Value,
                    false,
                    DateTime.MinValue,
                    objUser.LastLoginDate.Value,
                    DateTime.MinValue,
                    DateTime.MinValue,
                    DateTime.MinValue);
            }

            return u;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public override string GetUserNameByEmail(string email)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnValidatingPassword(ValidatePasswordEventArgs e)
        {
            base.OnValidatingPassword(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="answer"></param>
        /// <returns></returns>
        public override string ResetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public override bool UnlockUser(string userName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        public override void UpdateUser(MembershipUser user)
        {
            List<mebs_login> listUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByName, user.UserName), UriKind.Relative)).ToList(); //DAL.UserProvider.GetUser(username);

            if (listUser != null && listUser.Count > 0)
            {
                mebs_login loggeduser = listUser[0];
                if (loggeduser != null)
                {
                    loggeduser.Email = user.Email;
                    loggeduser.IsActive = user.IsApproved;
                    loggeduser.LastActivityDate = user.LastActivityDate;
                    loggeduser.LastLoginDate = user.LastLoginDate;
                    //DAL.UserProvider.UpdateUser(loggeduser);
                    _context.UpdateObject(loggeduser);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public override bool ValidateUser(string username, string password)
        {
            List<mebs_login> listUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByNameAndPassword, username,password), UriKind.Relative)).ToList(); //DAL.UserProvider.GetUser(username);

            if (listUser != null && listUser.Count > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Description
        {
            get
            {
                return base.Description;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name
        {
            get
            {
                return base.Name;
            }
        }
        #endregion
    }
}