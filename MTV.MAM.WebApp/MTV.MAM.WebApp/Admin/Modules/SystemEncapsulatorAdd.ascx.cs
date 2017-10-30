#region Name Space(s)
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
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
    public partial class SystemEncapsulatorAddControl : BLC.BaseMEBSMAMUserControl
    {
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
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    mebs_encapsulator encapsulator = SystemEncapsulatorInfo.SaveInfo();
                    Response.Redirect("SystemEncapsulatorDetails.aspx?IdEncapsulador=" + encapsulator.IdEncapsulator.ToString(),false);
                }

                catch (Exception ex)
                {
                    ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                    if (ex.InnerException is DataServiceClientException)
                    {
                        // Parse the DataServieClientException
                        BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                        // Display the DataServiceClientException message
                        if (innerException != null)
                        {
                            LogHelper.logger.Error(string.Format("SystemEncapsulatorAddControl : Bind : {0} - {1}", innerException.Code, innerException.Message));
                        }
                        else
                        {
                            LogHelper.logger.Error(string.Format("SystemEncapsulatorAddControl : Bind : {0}", ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorAddControl : Bind : {0}", ex.Message));
                    }
                }
            }
        }
        #endregion
    }
}