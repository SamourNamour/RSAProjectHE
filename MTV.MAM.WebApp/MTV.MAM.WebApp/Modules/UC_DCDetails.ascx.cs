#region -.-.-.-.-.-.-.-.-.-.-.- Class : NameSpace(s) -.-.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Services.Client;

using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.Helper;
using MTV.MAM.WebApp.MEBSCatalog;
#endregion

namespace MTV.MAM.WebApp.Modules
{
    public partial class UC_DCDetailsControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        mebs_ingesta _CurrentIngesta;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                _CurrentIngesta = _context.Execute<mebs_ingesta>(new Uri(string.Format(Config.GetIngestaById, this.EventIdentifier), UriKind.Relative)).FirstOrDefault();
                if (_CurrentIngesta != null)
                    IdImageUpload.EventIdentifier = _CurrentIngesta.IdIngesta;
            }
            catch (Exception ex) //---- Log in log4Net
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                //---- Disabled All Page Control
                DisableAllForm(false);
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UC_DCDetailsControl : Page_Load : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCDetailsControl : Page_Load : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCDetailsControl : Page_Load : {0}", ex.Message));
                }
            }


            if (!IsPostBack)
            {
                //---- Check if the Event Exist : If true Display Message, Disabled All Page controls
                if (_CurrentIngesta == null || _CurrentIngesta.IdIngesta <= 0)
                {
                    ShowMessage(BLC.DefaultValue.MSG_EVENT_NOT_FOUND);
                    DisableAllForm(false);
                    return;
                }

                FillCategoryDropDowns();
                SetDefaultValues();
                DisplayEventDetails();
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (_CurrentIngesta == null || _CurrentIngesta.IdIngesta <= 0)
            {
                ShowMessage(BLC.DefaultValue.MSG_EVENT_NOT_FOUND);
                DisableAllForm(false);
                return;
            }
            SaveEventChanges();
            DisplayEventDetails();
        }

        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (listCategory.Items.Count == 0)
                args.IsValid = false;
            else
            {
                if (listCategory.SelectedIndex >= 0)
                    args.IsValid = true;
                else
                    args.IsValid = false;
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method (s) -.-.-.-.-.-.-.-.-.-.-.-
        //---- Step 01 # OK
        /// <summary>
        /// Remplir la list des Categories.
        /// </summary>
        protected void FillCategoryDropDowns()
        {
            this.listCategory.Items.Clear();
            try
            {
                List<mebs_category_language_mapping> lCategoryMapping = _context.Execute<mebs_category_language_mapping>(new Uri(Config.GetCategories, UriKind.Relative)).ToList();
                listCategory.DataSource = lCategoryMapping;
                listCategory.DataValueField = "IdCategory";
                listCategory.DataTextField = "Title";
                listCategory.DataBind();
            }
            catch (Exception ex) //---- Log in log4Net
            {
                ShowError(BLC.DefaultValue.MSG_CATEGORY_NOT_POPULATED);
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UC_DCDetailsControl : FillCategoryDropDowns : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCDetailsControl : FillCategoryDropDowns : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCDetailsControl : FillCategoryDropDowns : {0}", ex.Message));
                }
            }
        }

        //---- Step 02 # OK
        /// <summary>
        /// Afficher les détails du Event selectionné.
        /// </summary>
        protected void DisplayEventDetails()
        {
            try
            {
                //---- Afficher les informations a modifier.
                txtActors.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Actors).DetailsValue;
                txtScreenFormat.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_ScreenFormat).DetailsValue;
                txtCountry.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Country).DetailsValue;
                txtDescription.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Description).DetailsValue;
                txtDirectors.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Directors).DetailsValue;
                txtGenre.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Genre).DetailsValue;
                //txtForm.Text = MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Form).DetailsValue;
                txtYear.Value = BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Year).DetailsValue, 0);
                //chkSelfCommercial.Checked = (_CurrentIngesta.SelfCommercial == Convert.ToInt32(BLC.SelfCommercial.Prepared) || _CurrentIngesta.SelfCommercial == Convert.ToInt32(BLC.SelfCommercial.Linked));
                txtExpiration.Text = _CurrentIngesta.Expiration_time.Value.ToString("d");
                txtImmortality.Text = _CurrentIngesta.Immortality_time.Value.ToString("d");
                txtPriority.Value = _CurrentIngesta.PreservationPriority.Value;
                txtTitle.Text = _CurrentIngesta.Title;
                txtParentalRating.Text = _CurrentIngesta.ParentalRating;
                IdImageUpload.BindImageUpload();

                if (_CurrentIngesta.mebs_ingesta_category_mapping != null && _CurrentIngesta.mebs_ingesta_category_mapping.Count > 0)
                {
                    foreach (mebs_ingesta_category_mapping item in _CurrentIngesta.mebs_ingesta_category_mapping)
                    {
                        ListItem itmCategory = listCategory.Items.FindByValue(item.IdCategory.ToString());
                        if (itmCategory != null)
                            itmCategory.Selected = true;
                    }
                }

                //---------------- Afficher les information dans le OverView
                lblContentIDResult.Text = _CurrentIngesta.EventId;
                lblTitleResult.Text = _CurrentIngesta.Title;
            }
            catch (Exception ex)
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                LogHelper.logger.Error(string.Format("UC_DCDetailsControl : DisplayEventDetails : {0}", ex.Message));
            }
        }

        //---- Step 03 # OK
        /// <summary>
        /// 
        /// </summary>
        protected void SaveEventChanges()
        {
            try
            {
                DateTime Expiration_Time = BLC.DateTimeHelper.ConvertStringToDateTime(txtExpiration.Text);
                DateTime Immortality_Time = BLC.DateTimeHelper.ConvertStringToDateTime(txtImmortality.Text);

                /************************ Check the METADATA before Saving In DATABASE ************************/
                if (Expiration_Time.CompareTo(Immortality_Time) < 0)
                {
                    //Display Message Error (Expiration must be Great than Immortality)
                    ShowError(BLC.DefaultValue.MSG_LIFEMODE_VALIDATION_ERROR);
                    return;
                }

                _CurrentIngesta.Title = txtTitle.Text;
                _CurrentIngesta.Last_Update = DateTime.UtcNow;
                _CurrentIngesta.Expiration_time = Expiration_Time;
                _CurrentIngesta.Immortality_time = Immortality_Time;
                _CurrentIngesta.ParentalRating = txtParentalRating.Text;
                if (!string.IsNullOrEmpty(IdImageUpload.SelectedCoverExtension))
                {
                    _CurrentIngesta.Poster = IdImageUpload.SaveSelectedCover();
                    _CurrentIngesta.PosterFileExtension = IdImageUpload.SelectedCoverExtension;
                }
                _CurrentIngesta.PreservationPriority = txtPriority.Value;
                //if (chkSelfCommercial.Checked)
                //    _CurrentIngesta.SelfCommercial = Convert.ToInt32(BLC.SelfCommercial.Prepared);
                //else
                //    _CurrentIngesta.SelfCommercial = Convert.ToInt32(BLC.SelfCommercial.NotLinked);

                /************************ Removing the old values : Clear the ingesta_category_mapping List ************************/
                foreach (var item in _CurrentIngesta.mebs_ingesta_category_mapping)
                {
                    _context.DeleteObject(item);
                }
                _CurrentIngesta.mebs_ingesta_category_mapping.Clear();
                mebs_ingesta_category_mapping newMapping;

                List<ListItem> _selectedItems = MEBSMAMHelper.GetSelectedItems(listCategory);

                if (_selectedItems != null && _selectedItems.Count > 0)
                {
                    foreach (var item in _selectedItems)
                    {
                        newMapping = new mebs_ingesta_category_mapping();
                        newMapping.IdCategory = BLC.CommonHelper.ConvertStringToInt(item.Value, 0);
                        newMapping.IdIngesta = _CurrentIngesta.IdIngesta;
                        _context.AddTomebs_ingesta_category_mapping(newMapping);
                        _CurrentIngesta.mebs_ingesta_category_mapping.Add(newMapping);
                        _context.AddLink(_CurrentIngesta, "mebs_ingesta_category_mapping", newMapping);
                    }
                }

                //---- Enregistrer les modifications apportée aux ExtendedInfos
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Actors).DetailsValue = txtActors.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Directors).DetailsValue = txtDirectors.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Country).DetailsValue = txtCountry.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Description).DetailsValue = txtDescription.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Genre).DetailsValue = txtGenre.Text;
                //MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Form).DetailsValue = txtForm.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_ScreenFormat).DetailsValue = txtScreenFormat.Text;
                MEBSMAMHelper.GetIngestaDetails(_CurrentIngesta, BLC.DefaultValue.DC_Year).DetailsValue = txtYear.Value.ToString();
                foreach (var item in _CurrentIngesta.mebs_ingestadetails)
                {
                    _context.UpdateObject(item);
                }

                _context.UpdateObject(_CurrentIngesta);
                _context.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                ShowMessage(BLC.DefaultValue.MSG_SAVE_OK);
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
                        ShowError(innerException.Message);
                        LogHelper.logger.Warn(string.Format("UC_DCDetailsControl : SaveEventChanges : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCDetailsControl : SaveEventChanges : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCDetailsControl : SaveEventChanges : {0}", ex.Message));
                }
            }
        }

        //---- Step 04 # OK
        /// <summary>
        /// Set The Controls by Default Values
        /// </summary>
        protected void SetDefaultValues()
        {
            //----- Set The Calendar and the Mask Informations
            MaskedEditValidatorExpiration.InvalidValueMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorExpiration.InvalidValueBlurredMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorImmortality.InvalidValueMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorImmortality.InvalidValueBlurredMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
        }

        //---- Step 05 # OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void DisableAllForm(bool value)
        {
            BtnSave.Visible = value;
            txtActors.Enabled = value;
            txtCountry.Enabled = value;
            txtDescription.Enabled = value;
            txtDirectors.Enabled = value;
            txtGenre.Enabled = value;
            txtParentalRating.Enabled = value;
            txtScreenFormat.Enabled = value;
            txtTitle.Enabled = value;
            txtYear.Enabled = value;
            txtImmortality.Enabled = value;
            iImmortality.Enabled = value;
            txtExpiration.Enabled = value;
            iExpiration.Enabled = value;
            IdImageUpload.Enabled = value;
            listCategory.Enabled = value;
            //chkSelfCommercial.Enabled = value;
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Proprity (ies) -.-.-.-.-.-.-.-.-.-.-.-
        public int EventIdentifier
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("EventID");
            }
        }
        #endregion

    }
}