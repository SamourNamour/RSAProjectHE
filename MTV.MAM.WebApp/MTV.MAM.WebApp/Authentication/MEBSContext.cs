using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
using System.Globalization;
using System.Threading;
using BLC = MTV.Library.Common;

namespace MTV.MAM.WebApp.Authentication
{
    /// <summary>
    /// Manage the User session into the system.
    /// </summary>

    public class MEBSContext
    {

        #region Constants
        private const string CONST_USERSESSION = "MEBS.UserSession";
        private const string CONST_USERSESSIONCOOKIE = "MEBS.UserSessionGUIDCookie";
        mebsEntities _context;
        #endregion

        #region Fields
        private mebs_login currentUser;
        private bool isAdmin;
        private HttpContext context = HttpContext.Current;
        #endregion

        #region Ctor
        /// <summary>
        /// Creates a new instance of the EBSContext class
        /// </summary>
        private MEBSContext()
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
        }
        #endregion


        /// <summary>
        /// Save User session to DataBase
        /// </summary>
        /// <returns>Saved user ssion</returns>
        private mebs_session SaveSessionToDatabase()
        {
            mebs_session session = new mebs_session();
            try
            {
                Guid sessionId = Guid.NewGuid();
                bool guidExist = true;
                List<mebs_session> listsession = null;

                //---- Generate new and Unique GUID for the session ID.
                while (guidExist)         //-----> Karima Todo
                {
                    sessionId = Guid.NewGuid();
                    listsession = _context.Execute<mebs_session>(new Uri(string.Format(Config.GetSessionByGuid, sessionId.ToString()), UriKind.Relative)).ToList();
                    if (listsession == null || listsession.Count() == 0)
                        guidExist = false;
                }
                
                string UserGuid = string.Empty;
                int UserID = 0;

                if (this.CurrentUser != null)
                {
                    UserGuid = this.CurrentUser.UserGUID;
                    UserID = this.CurrentUser.UserId;
                    //this.CurrentUser.
                }

                //----- Selectionner la session du User : Si Exist Modifier sinon Ajouter la session.
                //UserSessionProvider.AddUserSession(session);
                listsession = null;
                listsession = _context.Execute<mebs_session>(new Uri(string.Format(Config.GetUserSession, UserGuid), UriKind.Relative)).ToList();
                if (listsession == null || listsession.Count() <= 0)
                {
                    session.UserId = UserID;
                    session.UserGuid = UserGuid;
                    session.LastAccess = DateTime.UtcNow;
                    session.IsExpired = false;
                    session.SessionId = sessionId.ToString();
                    _context.AddTomebs_session(session);
                }
                else
                {
                    session = listsession[0];
                    session.LastAccess = DateTime.UtcNow;
                    session.IsExpired = false;
                    _context.UpdateObject(session);
                }
                _context.SaveChanges(SaveChangesOptions.Batch);
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
                        LogHelper.logger.Error(string.Format("MEBSContext : SaveSessionToDatabase : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MEBSContext : SaveSessionToDatabase : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MEBSContext : SaveSessionToDatabase : {0}", ex.Message));
                }
            }

            return session;

        }

        /// <summary>
        /// Gets User session
        /// </summary>
        /// <param name="createInDatabase">Create session in database if no one exists</param>
        /// <returns>Customer session</returns>
        public mebs_session GetSession(bool createInDatabase)
        {
            return this.GetSession(createInDatabase, null);
        }


        /// <summary>
        /// Gets User session
        /// </summary>
        /// <param name="createInDatabase">Create session in database if no one exists</param>
        /// <param name="sessionId">Session identifier</param>
        /// <returns>User session</returns>
        public mebs_session GetSession(bool createInDatabase, Guid? sessionId)
        {
            mebs_session byId = null;
            object obj2 = Current[CONST_USERSESSION];
            if (obj2 != null)
                byId = (mebs_session)obj2;
            if ((byId == null) && (sessionId.HasValue))
            {
                //byId = UserSessionProvider.GetUser(sessionId.Value);
                List<mebs_session> lSession = _context.Execute<mebs_session>(new Uri(string.Format(Config.GetSessionByGuid, sessionId.Value.ToString()), UriKind.Relative)).ToList();
                if (lSession != null && lSession.Count > 0)
                    byId = lSession[0];
                return byId;
            }
            if (byId == null && createInDatabase)
            {
                byId = SaveSessionToDatabase();
            }
            string userSessionCookieValue = string.Empty;
            if ((HttpContext.Current.Request.Cookies[CONST_USERSESSIONCOOKIE] != null) && (HttpContext.Current.Request.Cookies[CONST_USERSESSIONCOOKIE].Value != null))
                userSessionCookieValue = HttpContext.Current.Request.Cookies[CONST_USERSESSIONCOOKIE].Value;
            if ((byId) == null && (!string.IsNullOrEmpty(userSessionCookieValue)))
            {
                mebs_session dbUserSession = null; //UserSessionProvider.GetUser(new Guid(userSessionCookieValue));
                List<mebs_session> lSession = _context.Execute<mebs_session>(new Uri(string.Format(Config.GetSessionByGuid, sessionId.Value.ToString()), UriKind.Relative)).ToList();
                if (lSession != null && lSession.Count > 0)
                    dbUserSession = lSession[0];

                byId = dbUserSession;
            }
            Current[CONST_USERSESSION] = byId;
            return byId;

        }

        /// <summary>
        /// Saves current session to client
        /// </summary>
        public void SessionSaveToClient()
        {
            if (HttpContext.Current != null && this.Session != null)
                SetCookie(HttpContext.Current.ApplicationInstance, CONST_USERSESSIONCOOKIE, this.Session.SessionId.ToString());

        }

        /// <summary>
        /// Sets cookie
        /// </summary>
        /// <param name="application">Application</param>
        /// <param name="key">Key</param>
        /// <param name="val">Value</param>
        private static void SetCookie(HttpApplication application, string key, string val)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = val;
            if (string.IsNullOrEmpty(val))
            {
                cookie.Expires = DateTime.Now.AddMonths(-1);
            }
            else
            {
                cookie.Expires = DateTime.Now.AddHours(1); // can be configured 
            }
            application.Response.Cookies.Remove(key);
            application.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// Reset User session
        /// </summary>
        public void ResetSession()
        {
            if (HttpContext.Current != null)
                SetCookie(HttpContext.Current.ApplicationInstance, CONST_USERSESSIONCOOKIE, string.Empty);
            this.Session = null;
            this.CurrentUser = null;
            this["MEBS.SessionReseted"] = true;
        }

        /// <summary>
        /// Gets an instance of the EBSContext, which can be used to retrieve information about current context.
        /// </summary>
        public static MEBSContext Current
        {
            get
            {
                if (HttpContext.Current == null)
                    return null;

                if (HttpContext.Current.Items["MEBSContext"] == null)
                {
                    MEBSContext context2 = new MEBSContext();
                    HttpContext.Current.Items.Add("MEBSContext", context2);
                    return context2;
                }
                return (MEBSContext)HttpContext.Current.Items["MEBSContext"];
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether the context is running in admin-mode
        /// </summary>
        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }
            set
            {
                isAdmin = value;
            }
        }


        /// <summary>
        /// Gets or sets an object item in the context by the specified key.
        /// </summary>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        public object this[string key]
        {
            get
            {
                if (this.context == null)
                {
                    return null;
                }

                if (this.context.Items[key] != null)
                {
                    return this.context.Items[key];
                }
                return null;
            }
            set
            {
                if (this.context != null)
                {
                    this.context.Items.Remove(key);
                    this.context.Items.Add(key, value);

                }
            }
        }


        /// <summary>
        /// Sets the CultureInfo 
        /// </summary>
        /// <param name="culture">Culture</param>
        public void SetCulture(CultureInfo culture)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }


        /// <summary>
        /// Gets or sets the current user
        /// </summary>
        public mebs_login CurrentUser
        {
            get
            {
                return this.currentUser;
            }
            set
            {
                this.currentUser = value;
            }
        }


        /// <summary>
        /// Gets or sets the current session
        /// </summary>
        public mebs_session Session
        {
            get
            {
                return this.GetSession(false);
            }
            set
            {
                Current[CONST_USERSESSION] = value;
            }
        }

    }
}
