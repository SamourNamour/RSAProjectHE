#region Name Sapce(s)
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
    public partial class SystemEncapsulatorControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);

            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion 
 
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        void BindGrid()
        {
            try
            {
                List<mebs_encapsulator> _listEncapsulator = _context.Execute<mebs_encapsulator>(new Uri(Config.GetListOfEncapsulator, UriKind.Relative)).ToList();
                gvEncapsulators.DataSource = _listEncapsulator;
                gvEncapsulators.DataBind();
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