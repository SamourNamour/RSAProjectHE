#region -.-.-.-.-.-.-.-.-.-.-.- NameSpaces -.-.-.-.-.-.-.-.-.-.-.-
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
    public partial class RedundancyParametersControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGrid();
            }
        }
        #endregion 

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        void BindGrid()
        {
            try
            {
                mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
                List<mebs_settings> lSettings = _context.Execute<mebs_settings>(new Uri(Config.GetRedundancyParameters, UriKind.Relative)).ToList();

                gvRedundancyParameters.DataSource = lSettings; 
                gvRedundancyParameters.DataBind();
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
                        LogHelper.logger.Error(string.Format("RedundancyParametersControl : BindGrid : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("RedundancyParametersControl : BindGrid : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("RedundancyParametersControl : BindGrid : {0}", ex.Message));
                }
            }
        }
        #endregion 
    }
}