using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTV.MAM.WebApp.MEBSCatalog;
using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
using System.Globalization;
using System.Drawing;

namespace MTV.MAM.WebApp.Modules
{
    public partial class UC_DCScheduleDetails : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        mebs_schedule _currentSchedule;
        //DateTimeFormatInfo dtfi;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                _currentSchedule = _context.Execute<mebs_schedule>(new Uri(string.Format(Config.GetScheduleById, this.ScheduleIdentifier), UriKind.Relative)).FirstOrDefault();
                if (_currentSchedule != null && _currentSchedule.mebs_ingesta != null)
                    IdImageUpload.EventIdentifier = _currentSchedule.mebs_ingesta.IdIngesta;
                
            }
            catch (Exception ex) //---- Log in log4Net
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                //---- Disabled All Page Control
                EnableAllForm(false);
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : Page_Load : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : Page_Load : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : Page_Load : {0}", ex.Message));
                }
            }


            if (!IsPostBack)
            {
                //---- Check if the Event Exist : If true Display Message, Disabled All Page controls
                if (_currentSchedule == null || _currentSchedule.IdSchedule <= 0 || _currentSchedule.mebs_ingesta == null || _currentSchedule.mebs_ingesta.IdIngesta <= 0)
                {
                    ShowMessage(BLC.DefaultValue.MSG_EVENT_NOT_FOUND);
                    EnableAllForm(false);
                    return;
                }

                EnableControls();
                FillCategoryDropDowns();
                SetDefaultValues();
                DisplayEventDetails();
            }
        }

        protected void BtnSave_Click(object sender, EventArgs e)
        {
            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
            {
                UpdateExpiration();
                return;
            }

            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.PREPARED))
            {
                UpdateAllEventDetails();
                return;
            }
        }

        protected void BtnDelete_Click(object sender, EventArgs e)
        {
            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.FAILED_START) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.FAILED_STOP) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.MISSING_START) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.MISSING_STOP) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.PREPARED) ||
                _currentSchedule.mebs_ingesta.IsExpired)
            {
                DeleteEventFromRepositoty();
                return;
            }

            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
            {
                DeleteEventFromSTB();
                return;
            }
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
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : FillCategoryDropDowns : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : FillCategoryDropDowns : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : FillCategoryDropDowns : {0}", ex.Message));
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
                txtActors.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Actors).DetailsValue;
                txtScreenFormat.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_ScreenFormat).DetailsValue;
                txtCountry.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Country).DetailsValue;
                txtDescription.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Description).DetailsValue;
                txtDirectors.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Directors).DetailsValue;
                txtGenre.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Genre).DetailsValue;
                //txtForm.Text = MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Form).DetailsValue;
                txtYear.Value = BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Year).DetailsValue, 0);
                txtExpiration.Text = _currentSchedule.mebs_ingesta.Expiration_time.Value.ToString("d");
                txtImmortality.Text = _currentSchedule.mebs_ingesta.Immortality_time.Value.ToString("d");
                txtTitle.Text = _currentSchedule.mebs_ingesta.Title;
                txtParentalRating.Text = _currentSchedule.mebs_ingesta.ParentalRating;
                txtPriority.Value = _currentSchedule.mebs_ingesta.PreservationPriority.Value;
                IdImageUpload.BindImageUpload();

                if (_currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping != null && _currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping.Count > 0)
                {
                    foreach (mebs_ingesta_category_mapping item in _currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping)
                    {
                        ListItem itmCategory = listCategory.Items.FindByValue(item.IdCategory.ToString());
                        if (itmCategory != null)
                            itmCategory.Selected = true;
                    }
                }

                //---------------- Afficher les information dans le OverView
                lblContentIDResult.Text = _currentSchedule.mebs_ingesta.EventId;
                lblTitleResult.Text = _currentSchedule.mebs_ingesta.Title;
                lblStartTimeResult.Text = BLC.DateTimeHelper.ConvertDateTimeToString(MEBSMAMHelper.GetStartTimeEvent(_currentSchedule.Status.Value, _currentSchedule.Estimated_Start.Value, _currentSchedule.Exact_Start.Value));
                lblStopTimeResult.Text = BLC.DateTimeHelper.ConvertDateTimeToString(MEBSMAMHelper.GetStopTimeEvent(_currentSchedule.Status.Value, _currentSchedule.Estimated_Stop.Value, _currentSchedule.Exact_Stop.Value));
                lblChannelTimeResult.Text = _currentSchedule.mebs_ingesta.mebs_channel.LongName;
                lblCodiIDResult.Text = _currentSchedule.ContentID.ToString();
                string strStatus;
                Color strStatuscolor;
                MEBSMAMHelper.DisplayEventStatus(_currentSchedule.Status.Value, _currentSchedule.mebs_ingesta.IsExpired, out strStatus, out strStatuscolor);
                lblStatusResult.Text = strStatus;
                lblStatusResult.ForeColor = strStatuscolor;
            }
            catch (Exception ex)
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DisplayEventDetails : {0}", ex.Message));
            }
        }

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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        protected void EnableAllForm(bool value)
        {
            BtnSave.Visible = value;
            BtnDelete.Visible = value;
            txtActors.Enabled = value;
            txtCountry.Enabled = value;
            txtDescription.Enabled = value;
            txtDirectors.Enabled = value;
            txtGenre.Enabled = value;
            txtParentalRating.Enabled = value;
            txtScreenFormat.Enabled = value;
            txtShortDescription.Enabled = value;
            txtTitle.Enabled = value;
            txtYear.Enabled = value;
            txtPriority.Enabled = value;
            txtImmortality.Enabled = value;
            iImmortality.Enabled = value;

            txtExpiration.Enabled = value;
            iExpiration.Enabled = value;

            IdImageUpload.Enabled = value;
            listCategory.Enabled = value;
        }

        //---- Step #05 : OK
        /// <summary>
        /// 
        /// </summary>
        protected void EnableControls()
        {
            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.FAILED_START) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.FAILED_STOP) ||
                _currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.MISSING_STOP)
                )
            {
                EnableAllForm(false);
                //BtnDelete.Enabled = true;
                BtnDelete.Visible = true;
                //----- Afficher Message : The Event Is Expired or heve an Error , You can only delete-it from Database
                ShowError(BLC.DefaultValue.MSG_EVENT_ERROR);
                return;
            }

            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.MISSING_START))
            {
                EnableAllForm(false);
                //BtnDelete.Enabled = true;
                BtnDelete.Visible = true;
                //-----  You can only delete-it from Database
                ShowError(BLC.DefaultValue.MSG_TRIGGER_NOT_RECEIVED);
                return;
            }

            if (_currentSchedule.mebs_ingesta.IsExpired)
            {
                EnableAllForm(false);
                //BtnDelete.Enabled = true;
                BtnDelete.Visible = true;
                //----- You can only delete-it from Database
                ShowError(BLC.DefaultValue.MSG_EVENT_EXPIRED);
                return;
            }

            if (_currentSchedule.Status.Value >= Convert.ToInt32(BLC.ScheduleStatus.LOCKED))
            {
                EnableAllForm(false);
                //----- You cannot Delete or Update the event.
                ShowError(BLC.DefaultValue.MSG_EVENT_LOCKED);
                return;
            }

            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.STARTED))
            {
                EnableAllForm(false);
                //----- You cannot Delete or Update the event.
                ShowError(BLC.DefaultValue.MSG_EVENT_RECORDING);
                return;
            }
            if (_currentSchedule.Status.Value == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
            {
                EnableAllForm(false);
                BtnDelete.Visible = true;
                BtnSave.Visible = true;
                txtImmortality.Enabled = true;
                txtExpiration.Enabled = true;
                iExpiration.Enabled = true;
                iImmortality.Enabled = true;

                //---- L'utilisateur peut juste modifier l'expiration ou le poster sinon envoyer une commade de suppression au Encapsulateur.
                return;
            }

            //---- The Content Is Prepared
            EnableAllForm(true);

        }

        //---- Step 06 : OK
        /// <summary>
        /// Delete The Schedule from the Repository.
        /// </summary>
        /// <returns></returns>
        protected void DeleteEventFromRepositoty()
        {
            try
            {
                _context.DeleteObject(_currentSchedule);
                _context.SaveChanges(SaveChangesOptions.Batch);
                Response.Redirect("~/VodPrograming.aspx", false);
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
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromRepositoty : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromRepositoty : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromRepositoty : {0}", ex.Message));
                }

            }
        }

        //---- Step 07 : OK
        /// <summary>
        /// Modifier le champs "IsDeleted" dans la base de donnée par 0;
        /// </summary>
        protected void DeleteEventFromSTB()
        {
            try
            {
                _currentSchedule.IsDeleted = Convert.ToInt32(BLC.ScheduleDeleteStatus.Deleting);
                _context.UpdateObject(_currentSchedule);
                _context.SaveChanges(SaveChangesOptions.Batch);
                Response.Redirect("~/VodPrograming.aspx", false);
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
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromSTB : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromSTB : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : DeleteEventFromSTB : {0}", ex.Message));
                }
            }
        }

        //---- Step 08 : OK
        /// <summary>
        /// Update only the Expiration or the Poster and send the Command to the Encapsulator.
        /// </summary>
        protected void UpdateExpiration()
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
                _currentSchedule.mebs_ingesta.Expiration_time = Expiration_Time;
                _currentSchedule.mebs_ingesta.Immortality_time = Immortality_Time;
                _currentSchedule.mebs_ingesta.Last_Update = DateTime.UtcNow;
                _context.UpdateObject(_currentSchedule.mebs_ingesta);
                _context.SaveChanges(SaveChangesOptions.Batch);
                ShowMessage(BLC.DefaultValue.MSG_SAVE_OK);
                DisplayEventDetails();
            }
            catch (Exception ex)//---- Log in log4Net
            {
                ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateExpiration : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateExpiration : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateExpiration : {0}", ex.Message));
                }
            }
        }

        //---- Step 09 : OK
        /// <summary>
        /// Update all Event Details.
        /// </summary>
        protected void UpdateAllEventDetails()
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

                _currentSchedule.mebs_ingesta.Expiration_time = Expiration_Time;
                _currentSchedule.mebs_ingesta.Immortality_time = Immortality_Time;
                _currentSchedule.mebs_ingesta.Title = txtTitle.Text;
                _currentSchedule.mebs_ingesta.Last_Update = DateTime.UtcNow;
                _currentSchedule.mebs_ingesta.ParentalRating = txtParentalRating.Text;
                _currentSchedule.mebs_ingesta.PreservationPriority = txtPriority.Value;
                if (!string.IsNullOrEmpty(IdImageUpload.SelectedCoverExtension))
                {
                    _currentSchedule.mebs_ingesta.Poster = IdImageUpload.SaveSelectedCover();
                    _currentSchedule.mebs_ingesta.PosterFileExtension = IdImageUpload.SelectedCoverExtension;
                }


                /************************ Removing the old values : Clear the ingesta_category_mapping List ************************/
                foreach (var item in _currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping)
                {
                    _context.DeleteObject(item);
                }
                _currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping.Clear();
                mebs_ingesta_category_mapping newMapping;

                List<ListItem> _selectedItems = MEBSMAMHelper.GetSelectedItems(listCategory);

                if (_selectedItems != null && _selectedItems.Count > 0)
                {
                    foreach (var item in _selectedItems)
                    {
                        newMapping = new mebs_ingesta_category_mapping();
                        newMapping.IdCategory = BLC.CommonHelper.ConvertStringToInt(item.Value, 0);
                        newMapping.IdIngesta = _currentSchedule.mebs_ingesta.IdIngesta;
                        _context.AddTomebs_ingesta_category_mapping(newMapping);
                        _currentSchedule.mebs_ingesta.mebs_ingesta_category_mapping.Add(newMapping);
                        _context.AddLink(_currentSchedule.mebs_ingesta, "mebs_ingesta_category_mapping", newMapping);
                    }
                }

                //---- Enregistrer les modifications apportée aux ExtendedInfos
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Actors).DetailsValue = txtActors.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Directors).DetailsValue = txtDirectors.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Country).DetailsValue = txtCountry.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Description).DetailsValue = txtDescription.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Genre).DetailsValue = txtGenre.Text;
                //MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Form).DetailsValue = txtForm.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_ScreenFormat).DetailsValue = txtScreenFormat.Text;
                MEBSMAMHelper.GetIngestaDetails(_currentSchedule.mebs_ingesta, BLC.DefaultValue.DC_Year).DetailsValue = txtYear.Value.ToString();

                foreach (var item in _currentSchedule.mebs_ingesta.mebs_ingestadetails)
                {
                    _context.UpdateObject(item);
                }

                _context.UpdateObject(_currentSchedule.mebs_ingesta);
                _context.SaveChanges(System.Data.Services.Client.SaveChangesOptions.Batch);
                ShowMessage(BLC.DefaultValue.MSG_SAVE_OK);
                DisplayEventDetails();
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
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateAllEventDetails : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateAllEventDetails : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_DCScheduleDetails : UpdateAllEventDetails : {0}", ex.Message));
                }
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Proprity (ies) -.-.-.-.-.-.-.-.-.-.-.-
        public int ScheduleIdentifier
        {
            get
            {
                if (Session["ScheduleToModify"] == null)
                    return -1;

                CPintaTaula ScheduleToModify = (CPintaTaula)Session["ScheduleToModify"];
                return ScheduleToModify.IdSchedule;
            }
        }
        #endregion
    }
}