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

using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SettingsInfoControl : BLC.BaseMEBSMAMUserControl
    {
        mebsEntities _context;
        mebs_settings _currentsetting;

        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public int SettingID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("SettingID",0);
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
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                _currentsetting = _context.Execute<mebs_settings>(new Uri(string.Format(Config.GetSettingByID, this.SettingID), UriKind.Relative)).FirstOrDefault();
                // Forbid user to changes already stored Setting Name :
                txtName.Enabled = (SettingID > 0 ? false : true);
                if (!IsPostBack)
                {
                    BindData();
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
                        LogHelper.logger.Error(string.Format("SettingsInfoControl : Page_Load : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SettingsInfoControl : Page_Load : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("SettingsInfoControl : Page_Load : {0}", ex.Message));
                }  
            }
        }
        #endregion 

        #region Method(s)
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            try
            {
                if (_currentsetting != null)
                {
                    txtName.Text = _currentsetting.SettingName;
                    txtValue.Text = _currentsetting.SettingValue;
                    txtDescription.Text = _currentsetting.Description;
                    chkVisibility.Checked = (_currentsetting.Visibility == "Y" ? true : false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("SettingsInfoControl : Page_Load : {0}", ex.Message));
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public mebs_settings SaveInfo()
        {
            try
            {
                if (this.SettingID != 0)
                {
                    _currentsetting.SettingName = txtName.Text;
                    _currentsetting.SettingValue = txtValue.Text;
                    _currentsetting.Description = txtDescription.Text;
                    _currentsetting.Visibility = (chkVisibility.Checked ? "Y" : "N");

                    _context.UpdateObject(_currentsetting);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                    return _currentsetting;
                }
                else
                {
                    mebs_settings _newSetting = new mebs_settings();
                    _newSetting.SettingName = txtName.Text;
                    _newSetting.SettingValue = txtValue.Text;
                    _newSetting.Description = txtDescription.Text;
                    _newSetting.Visibility = "Y";

                    _context.AddTomebs_settings(_newSetting);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                    return _newSetting;
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
                return null;
            }            
        }
        #endregion 
    }
}