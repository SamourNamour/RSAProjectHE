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
using System.Globalization;

namespace MTV.MAM.WebApp.Modules
{
    public partial class UC_VodSystemOutputControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        private mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
            if (!IsPostBack)
            {
                SetDefaultValues();
                if (!string.IsNullOrEmpty(this._SelectedDateStart))
                    Bind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            this._SelectedDateStart = txtStartDate.Text;
            this._SelectedDateStop = txtStopDate.Text;
            this._Title = txtTitle.Text.Trim();

            Bind();
        }

        protected void gvwdatacast_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvwdatacast.PageIndex = e.NewPageIndex;
            Bind();
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method (s) -.-.-.-.-.-.-.-.-.-.-.-
        //---- Step # 03 : Not OK
        /// <summary>
        /// Set The Controls by Default Values
        /// </summary>
        protected void SetDefaultValues()
        {
            //----- Set The Calendar and the Mask Informations
            MaskedEditValidatorFrom.InvalidValueMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorFrom.InvalidValueBlurredMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorTo.InvalidValueMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorTo.InvalidValueBlurredMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);

            if (!string.IsNullOrEmpty(this._SelectedDateStart))
                txtStartDate.Text = this._SelectedDateStart;
            else
                txtStartDate.Text = DateTime.Today.ToString("d");

            if (!string.IsNullOrEmpty(this._SelectedDateStop))
                txtStopDate.Text = this._SelectedDateStop;
            else
                txtStopDate.Text = DateTime.Today.ToString("d");

            txtTitle.Text = this._Title;
        }

        //---- Step # 05 : OK
        /// <summary>
        /// Cherche La Liste des Ingesta qui correspant au critère de l'utilisateur.
        /// </summary>
        void Bind()
        {
            if (string.IsNullOrEmpty(this._SelectedDateStart))
            {
                this.gvwdatacast.Visible = false;
                return;
            }

            try
            {
                string strFrom = BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(Convert.ToDateTime(this._SelectedDateStart));
                string strTo = BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(Convert.ToDateTime(this._SelectedDateStop).AddDays(1));
                _context = new mebsEntities(Config.MTVCatalogLocation);

                string queryString = string.Format(Config.GetPackagesByDateCreation, strFrom, strTo.ToString());
                if (!string.IsNullOrEmpty(this._Title))
                    queryString = string.Format(Config.GetPackagesByTitle, strFrom, strTo, this._Title.Trim());

                List<mebs_ingesta> _listIngesta = _context.Execute<mebs_ingesta>(new Uri(queryString, UriKind.Relative)).ToList();

                if (_listIngesta == null || _listIngesta.Count <= 0)
                {
                    ShowMessage(BLC.DefaultValue.MSG_DATA_NOT_FOUND);
                }
                this.gvwdatacast.DataSource = _listIngesta;
                this.gvwdatacast.DataBind();
            }
            catch (Exception ex) //---- Log in log4Net
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("TrafficSystem_Control : Bind : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("TrafficSystem_Control : Bind : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("TrafficSystem_Control : Bind : {0}", ex.Message));
                }
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Parameter (s) -.-.-.-.-.-.-.-.-.-.-.-
        public string _SelectedDateStart
        {
            get
            {
                if (Session["SelectedDatadtStart"] != null)
                    return Session["SelectedDatadtStart"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                Session["SelectedDatadtStart"] = txtStartDate.Text;
            }
        }
        public string _SelectedDateStop
        {
            get
            {
                if (Session["SelectedDatadtStop"] != null)
                    return Session["SelectedDatadtStop"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                Session["SelectedDatadtStop"] = txtStopDate.Text;
            }
        }
        public string _Title
        {
            get
            {
                if (Session["SelectedDataTitle"] != null)
                    return Session["SelectedDataTitle"].ToString();
                else
                    return string.Empty;
            }
            set
            {
                Session["SelectedDataTitle"] = txtTitle.Text;
            }
        }
        #endregion
    }
}