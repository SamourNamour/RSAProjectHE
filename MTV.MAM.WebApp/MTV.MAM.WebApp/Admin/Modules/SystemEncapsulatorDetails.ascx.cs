#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SystemEncapsulatorDetailsControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public int IdEncapsulador
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("IdEncapsulador");
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
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
                    mebs_encapsulator encapsulator = ctrlEncapsulatorInfo.SaveInfo();
                    Response.Redirect("SystemEncapsulatorDetails.aspx?IdEncapsulador=" + encapsulator.IdEncapsulator.ToString(),false);
                }
                catch (Exception ex)
                {
                    ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                    LogHelper.logger.Error(string.Format("SystemEncapsulatorControl : Bind : {0}", ex.Message));
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
                _context = new mebsEntities(Config.MTVCatalogLocation);
                mebs_encapsulator _encToRemove = _context.Execute<mebs_encapsulator>(new Uri(string.Format(Config.GetEncapsulatorById, this.IdEncapsulador), UriKind.Relative)).FirstOrDefault();
                _context.DeleteObject(_encToRemove);
                _context.SaveChanges(SaveChangesOptions.Batch);
                Response.Redirect("SystemEncapsulator.aspx",false);
            }
            catch (Exception ex)
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                if (ex.InnerException is DataServiceClientException)
                {
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);

                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorControl : Bind : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorControl : Bind : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("SystemEncapsulatorControl : Bind : {0}", ex.Message));
                }
            }
        }
        #endregion
    }
}