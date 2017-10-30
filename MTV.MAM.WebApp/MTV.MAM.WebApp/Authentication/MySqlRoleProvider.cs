using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using BLC = MTV.Library.Common;
using System.Data.Services.Client;
using MTV.MAM.WebApp.Helper;
using MTV.MAM.WebApp.MEBSCatalog;

namespace MTV.MAM.WebApp.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public class MySqlRoleProvider : RoleProvider
    {
        #region Variable (s)
        mebsEntities _context;
        #endregion

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        public MySqlRoleProvider()
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                mebs_usersinroles ObjUserInRole = null;
                foreach (var roleName in roleNames)
                {
                    ObjUserInRole = new mebs_usersinroles();
                    ObjUserInRole.RoleName = roleName;
                    foreach (var username in usernames)
                    {
                        ObjUserInRole.UserName = username;
                        _context.AddObject("mebs_usersinroles", ObjUserInRole);
                        _context.SaveChanges(SaveChangesOptions.Batch);
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUsersToRoles : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUsersToRoles : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUsersToRoles : {0}", ex.Message));
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        public override void CreateRole(string roleName)
        {
            try
            {
                mebs_roles ObjRole = new mebs_roles();
                ObjRole.name = roleName;
                _context.AddObject("mebs_roles", ObjRole);
                _context.SaveChanges(SaveChangesOptions.Batch);
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : CreateRole : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : CreateRole : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : CreateRole : {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="throwOnPopulatedRole"></param>
        /// <returns></returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            //if (DAL.CMySQLMSPDataBase.DeleteRole(roleName, throwOnPopulatedRole) != -1)
            //{
            //    return true;
            //}
            throw new Exception("DeleteRole : The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string[] GetAllRoles()
        {
            try
            {
                List<mebs_roles> listRoles = _context.Execute<mebs_roles>(new Uri(Config.GetAllRoles, UriKind.Relative)).ToList();
                if (listRoles == null || listRoles.Count <= 0)
                    return null;

                List<string> rolesArray = new List<string>();
                foreach (mebs_roles item in listRoles)
                {
                    if (rolesArray.Contains(item.name)) continue;
                    rolesArray.Add(item.name);
                }
                return rolesArray.ToArray();
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0}", ex.Message));
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public override string[] GetRolesForUser(string username)
        {

            try
            {
                List<mebs_usersinroles> listRolesforUser = _context.Execute<mebs_usersinroles>(new Uri(string.Format(Config.GetRolesForUser, username), UriKind.Relative)).ToList();
                if (listRolesforUser == null || listRolesforUser.Count <= 0)
                    return null;

                List<string> rolesArray = new List<string>();
                foreach (mebs_usersinroles item in listRolesforUser)
                {
                    if (rolesArray.Contains(item.RoleName)) continue;
                    rolesArray.Add(item.RoleName);
                }
                return rolesArray.ToArray();
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : GetAllRoles : {0}", ex.Message));
                }
                return null;
            }
            //List<string> userRoles = DAL.CMySQLMSPDataBase.GetRolesForUser(username);
            //if (userRoles != null)
            //{
            //    return userRoles.ToArray();
            //}

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            try
            {
                //mebsEntities _context = new mebsEntities(Config.MEBSCatalogLocation);
                List<mebs_usersinroles> lUserRoles = _context.Execute<mebs_usersinroles>(new Uri(string.Format(Config.IsUserInRole, username, roleName), UriKind.Relative)).ToList();
                if (lUserRoles != null && lUserRoles.Count > 0)
                    return true;
                else
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : IsUserInRole : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : IsUserInRole : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : IsUserInRole : {0}", ex.Message));
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override string[] GetUsersInRole(string roleName)
        {
            //return DAL.CMySQLMSPDataBase.GetUsersInRole(roleName).ToArray();
            throw new Exception("GetUsersInRole : The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public override bool RoleExists(string roleName)
        {
            try
            {
                mebs_roles ObjRole = _context.Execute<mebs_roles>(new Uri(string.Format(Config.GetRoleByName, roleName), UriKind.Relative)).FirstOrDefault();
                if (ObjRole == null)
                    return false;

                return true;
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : RoleExists : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : RoleExists : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : RoleExists : {0}", ex.Message));
                }
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        public void AddUserToRole(string username, string roleName)
        {
            try
            {
                mebs_usersinroles ObjUserInRole = new mebs_usersinroles();

                ObjUserInRole.RoleName = roleName;
                ObjUserInRole.UserName = username;
                _context.AddObject("mebs_usersinroles", ObjUserInRole);
                _context.SaveChanges(SaveChangesOptions.Batch);
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
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUserToRole : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUserToRole : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MySqlRoleProvider : AddUserToRole : {0}", ex.Message));
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="roleName"></param>
        public void RemoveUserFromRole(string username, string roleName)
        {
            // DAL.CMySQLMSPDataBase.RemoveUserFromRole(username, roleName);
            throw new Exception("RemoveUserFromRole : The method or operation is not implemented.");
        }

        /// <summary>
        /// 
        /// </summary>
        public override string ApplicationName
        {
            get
            {
                throw new Exception("The method or operation is not implemented.");
            }
            set
            {
                throw new Exception("The method or operation is not implemented.");
            }
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
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleName"></param>
        /// <param name="usernameToMatch"></param>
        /// <returns></returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
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
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernames"></param>
        /// <param name="roleNames"></param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
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
    }
}