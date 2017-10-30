#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.Services.Client;

using System.Xml;
using System.Threading;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using BLC = MTV.Library.Common;
using AUTH = MTV.MAM.WebApp.Authentication;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SettingsControl : BLC.BaseMEBSMAMUserControl
    {
        #region
        mebsEntities _context;
        #endregion
        #region Event(s)
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
                if (!Page.IsPostBack)
                {
                    BindGrid(txtFilterKey.Text);
                }
            }
            catch (Exception ex)
            {
                //BLC.LoggerProvider.MSPLogger.IEBSAdminConfigLogger.Error(exc);
                LogHelper.logger.Error(string.Format("SettingsControl : Page_Load : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvSettings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvSettings.PageIndex = e.NewPageIndex;

            BindGrid(txtFilterKey.Text);          
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid(txtFilterKey.Text.Replace(" ",""));
        }
        #endregion 

        #region Method(s)
        /// <summary>
        /// 
        /// </summary>
        void BindGrid()
        {
            try
            {
               List<mebs_settings> _listAllSetings = _context.Execute<mebs_settings>(new Uri(Config.GetAllSettings, UriKind.Relative)).ToList();
               gvSettings.DataSource = _listAllSetings;
                    //DAL.Settings.ListAll(); //DAL.SettingsProvider.GetListOfSettings();
                gvSettings.DataBind();
            }
            catch (Exception ex)
            {
                //DAL.Log.Write(string.Format("Settings : BindGrid : {0}", exc.ToString()));
                //BLC.LoggerProvider.MSPLogger.IEBSAdminConfigLogger.Error(exc);
                LogHelper.logger.Error(string.Format("SettingsControl : BindGrid : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        void BindGrid(string key)
        {
            try
            {
                if (string.IsNullOrEmpty(key))
                {
                    BindGrid();
                }
                else
                {
                    List<mebs_settings> _listAllSetings = _context.Execute<mebs_settings>(new Uri(string.Format(Config.SearchSettingsByName,key), UriKind.Relative)).ToList();
                    gvSettings.DataSource = _listAllSetings;  //DAL.SettingsProvider.GetListOfSettings(key);
                    gvSettings.DataBind();
                }
            }
            catch (Exception ex)
            {
                //DAL.Log.Write(string.Format("Settings : BindGrid (..): {0}", exc.ToString()));
                //BLC.LoggerProvider.MSPLogger.IEBSAdminConfigLogger.Error(exc);
                LogHelper.logger.Error(string.Format("SettingsControl : BindGrid : {0}", ex.Message));
            }
        }
        #endregion 
    }
}