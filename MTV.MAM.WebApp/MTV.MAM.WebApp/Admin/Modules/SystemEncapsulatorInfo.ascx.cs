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
using MTV.Library.Common;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SystemEncapsulatorInfoControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable(s) -.-.-.-.-.-.-.-.-.-.-.-
        public mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public int IdEncapsulador
        {
            get
            {
                return CommonHelper.QueryStringInt("IdEncapsulador");
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
            _context = new mebsEntities(Config.MTVCatalogLocation);
            if (!Page.IsPostBack)
            {
                this.FillDropDownType();
                //this.FillDropDownStatus();
                BindData();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void AddButton_Click(object sender, EventArgs e)
        {
            SaveInfo();
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        private void FillDropDownType()
        {
            try
            {
                ddlEncapsulatorType.Items.Add(new ListItem(Convert.ToString(BLC.EncapsulatorType.Metadata), "0"));
                ddlEncapsulatorType.Items.Add(new ListItem(Convert.ToString(BLC.EncapsulatorType.Asset), "1"));
                ddlEncapsulatorType.Items.Add(new ListItem(Convert.ToString(BLC.EncapsulatorType.Metadata_Asset), "2"));
            }
            catch (Exception ex)
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.Message));
            }           
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private void FillDropDownStatus()
        //{
        //    try
        //    {
        //        ddlEncapsulatorStatus.Items.Add(new ListItem(Convert.ToString(BLC.EncapsulatorStatus.Master), "1"));
        //        ddlEncapsulatorStatus.Items.Add(new ListItem(Convert.ToString(BLC.EncapsulatorStatus.Slave), "0"));
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
        //        LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.Message));
        //    }            
        //}

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            try
            {
                mebs_encapsulator encapsulator = _context.Execute<mebs_encapsulator>(new Uri(string.Format(Config.GetEncapsulatorById, this.IdEncapsulador), UriKind.Relative)).FirstOrDefault();
                if (encapsulator != null)
                {
                    this.txtName.Text = encapsulator.Name;

                    ListItem ciItemType = ddlEncapsulatorType.Items.FindByText(encapsulator.Type);
                    if (ciItemType != null)
                        ciItemType.Selected = true;

                    //ListItem ciItemStatus = ddlEncapsulatorStatus.Items.FindByValue(encapsulator.Status.ToString());
                    //if (ciItemStatus != null)
                    //    ciItemStatus.Selected = true;



                    this.cbPublished.Checked = encapsulator.IsPublished.Value;
                    this.txtIpAddress.Text = encapsulator.IpAddress;

                    //this.txtMultiInstancesNum.Value = encapsulator.MultiInstancesNum;
                }
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
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.Message));
                }
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public mebs_encapsulator SaveInfo()
        {
            mebs_encapsulator encapsulator = null;
            try
            {
                encapsulator = _context.Execute<mebs_encapsulator>(new Uri(string.Format(Config.GetEncapsulatorById, this.IdEncapsulador), UriKind.Relative)).FirstOrDefault();
                string name = txtName.Text;
                string type = ddlEncapsulatorType.SelectedItem.Text;
                //int status = Convert.ToInt32(ddlEncapsulatorStatus.SelectedItem.Value);
                bool published = cbPublished.Checked;
                string ipaddress = txtIpAddress.Text;
                if (encapsulator != null)
                {
                    encapsulator.Name = name;
                    encapsulator.Type = type;
                    //encapsulator.Status = status;
                    encapsulator.IsPublished = published;
                    encapsulator.IpAddress = ipaddress;
                    //encapsulator.MultiInstancesNum = (int)this.txtMultiInstancesNum.Value;
                    //DAL.EncapsulatorProvider.UpdateEncapsulatorById(encapsulator);
                    _context.UpdateObject(encapsulator);
                }
                else
                {
                    encapsulator = new mebs_encapsulator();
                    encapsulator.Name = name;
                    encapsulator.Type = type;
                    //encapsulator.Status = status;
                    encapsulator.IsPublished = published;
                    encapsulator.IpAddress = ipaddress;
                    //encapsulator.MultiInstancesNum = (int)this.txtMultiInstancesNum.Value;
                    _context.AddTomebs_encapsulator(encapsulator);
                }

                
                _context.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                ShowMessage(BLC.DefaultValue.MSG_SAVE_OK);
                return encapsulator;
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
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("SystemEncapsulatorInfoControl : Bind : {0}", ex.Message));
                }
                return encapsulator;
            }            
        }
        #endregion
    }
}