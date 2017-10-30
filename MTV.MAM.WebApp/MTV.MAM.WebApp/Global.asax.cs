using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using AUTH = MTV.MAM.WebApp.Authentication;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
using BLC = MTV.Library.Common;

namespace MTV.MAM.WebApp
{
    public class Global : System.Web.HttpApplication
    {
        mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
             string strThisPageUrl = BLC.CommonHelper.GetThisPageURL(false).ToLower();
             if (strThisPageUrl.Contains("/getting_started.aspx"))
                    return;

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    string cookie = Request.Headers["Cookie"];
                    // Code that runs when a new session is started
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        if (Request.QueryString["timeout"] == null || !Request.QueryString["timeout"].ToString().Equals("yes"))
                            Session.Abandon();
                        Session.Clear();
                        FormsAuthentication.SignOut();
                        Response.Redirect("~/Login.aspx?timeout=yes");
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_Start(object sender, EventArgs e)
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            try
            {
                logout();
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("Global.asax : Application_End : {0} ", ex.ToString()));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            try
            {
                bool authenticated = false;
                if (HttpContext.Current.User != null && HttpContext.Current.User.Identity != null)
                    authenticated = HttpContext.Current.User.Identity.IsAuthenticated;
                if (authenticated)
                {
                    string username = HttpContext.Current.User.Identity.Name;
                    mebs_login objCurrentUser = null;
                    List<mebs_login> lCurrentUser = _context.Execute<mebs_login>(new Uri(string.Format(Config.GetUserByName, username), UriKind.Relative)).ToList();

                    if (lCurrentUser != null && lCurrentUser.Count > 0)
                        objCurrentUser = lCurrentUser[0];


                    if (objCurrentUser != null)
                    {
                        mebs_session objUserSession = null;

                        //objCurrentUser.LastActivityDate = DateTime.Now.ToUniversalTime();
                        //_context.UpdateObject(objCurrentUser);
                        //_context.SaveChanges(); 

                        if (objCurrentUser.mebs_session != null && objCurrentUser.mebs_session.Count > 0)
                            objUserSession = objCurrentUser.mebs_session[0];

                        if (objUserSession == null)
                        {
                            AUTH.MEBSContext.Current.CurrentUser = objCurrentUser;
                            objUserSession = AUTH.MEBSContext.Current.GetSession(true);
                            objUserSession.UserGuid = objCurrentUser.UserGUID;
                            objUserSession.LastAccess = DateTime.Now.ToUniversalTime();
                            objUserSession.IsExpired = false;
                        }
                        else
                        {
                            objUserSession.LastAccess = DateTime.Now.ToUniversalTime();
                            _context.AddTomebs_session(objUserSession);
                            _context.SaveChanges(SaveChangesOptions.Batch);
                        }

                        if (!String.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                        {
                            AUTH.MEBSContext.Current.CurrentUser = objCurrentUser;
                            AUTH.MEBSContext.Current.IsAdmin = _context.Execute<mebs_usersinroles>(new Uri(string.Format(Config.IsUserInRole, objCurrentUser.Login, BLC.UsersRoles.MEBSAdmin.ToString()), UriKind.Relative)).Any();
                            AUTH.MEBSContext.Current.Session = objUserSession;
                        }
                        else
                        {
                            logout();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("Global.asax : Application_AuthenticateRequest : {0} ", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            try
            {
                if (AUTH.MEBSContext.Current.Session != null)
                {
                    DateTime dtNow = DateTime.UtcNow;
                    if (AUTH.MEBSContext.Current.Session.LastAccess.Value.AddMinutes(1.0) < dtNow)
                    {
                        AUTH.MEBSContext.Current.Session.LastAccess = dtNow;
                        //BLC.MEBSContext.Current.Session = DAL.UserSessionProvider.AddUserSession(BLC.MEBSContext.Current.Session);
                        _context.AddTomebs_session(AUTH.MEBSContext.Current.Session);
                        _context.SaveChanges(SaveChangesOptions.Batch);
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("Global.asax : Application_BeginRequest : {0} ", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                bool sessionReseted = false;
                if (AUTH.MEBSContext.Current["MEBS.SessionReseted"] != null)
                {
                    sessionReseted = Convert.ToBoolean(AUTH.MEBSContext.Current["MEBS.SessionReseted"]);
                }
                if (!sessionReseted)
                {
                    //BLC.EBSContext.Current.SessionSaveToClient(); // ToDo
                }
            }
            catch (HttpException ex)
            {
                LogHelper.logger.Error(string.Format("Global.asax : Application_EndRequest : {0} ", ex.Message));
            }
        }

        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            ClearDCSchedulerSessions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            if (ex != null)
            {
                LogHelper.logger.Error(string.Format("Application Erros : Requested url : {0} ", Request.RawUrl));
                LogHelper.logger.Error(string.Format("Application Erros : Application_Error : {0} ", ex.ToString()));
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Auxiliary Method(s)-.-.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// Logout customer
        /// </summary>
        public static void logout()
        {
            if (AUTH.MEBSContext.Current != null)
            {
                AUTH.MEBSContext.Current.ResetSession();
            }
            if (HttpContext.Current != null && 
                HttpContext.Current.Request !=  null && 
                HttpContext.Current.Request.Cookies["ASP.Net_SessionId"] != null)
            {
                HttpContext.Current.Request.Cookies["ASP.Net_SessionId"].Expires = DateTime.Now.AddDays(-1d);
            }
             FormsAuthentication.SignOut();
        }

        /// <summary>
        /// 
        /// </summary>
        public void BlockBrowzerPageReview()
        {
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddMinutes(-1));
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();

            if (AUTH.MEBSContext.Current.CurrentUser == null)
            {
                HttpContext.Current.Response.Redirect("Login.aspx");
            }
        }

        public void ClearDCSchedulerSessions()
        {
            try
            {
                string strThisPageUrl = BLC.CommonHelper.GetThisPageURL(false).ToLower();
                if (!strThisPageUrl.Contains("/dcscheduledetails.aspx") && !strThisPageUrl.Contains("/authorizedvodlist.aspx") &&
                    !strThisPageUrl.Contains("/vodprograming.aspx") && !strThisPageUrl.Contains("/ingestainformation.aspx")
                    && !strThisPageUrl.Contains("/pushvodhome.aspx")
                    )
                {
                    Session["SelectedDate"] = null;
                    Session["CellId"] = null;
                    Session["SelectedIngesta"] = null;
                    Session["SelectedCellule"] = null;
                    Session["ScheduleToModify"] = null;
                    //Session["NewStartTime"] = null;
                }
            }
            catch
            {
                
            }

        }

        #endregion
    }
}