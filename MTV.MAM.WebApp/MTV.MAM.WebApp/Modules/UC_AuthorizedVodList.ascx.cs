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

namespace MTV.MAM.WebApp.Modules
{
    public partial class UC_AuthorizedVodListControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.- 
        mebsEntities _context;
        static readonly object LockTheSelection = new object();
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
            if (!IsPostBack)
            {
                BindGridView();
                InitialisePageControl();
            }
        }

        protected void gvwdatacast_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "SelectItem")
            {
                if (Page.IsValid)
                {
                    lock (LockTheSelection)
                    {
                        int idIngesta = Convert.ToInt32(e.CommandArgument);
                        CPintaTaula SelectedIngesta = new CPintaTaula();
                        //----Get the Selected Ingeta Infos (Title + CodePackage)
                        
                        Control ctrl = e.CommandSource as Control;
                        if (ctrl != null)
                        {
                            GridViewRow _currenrtrow = ctrl.Parent.NamingContainer as GridViewRow;
                            HiddenField HFCodePackage = (HiddenField)_currenrtrow.FindControl("HFCodePackage");
                            if (HFCodePackage != null)
                            {
                                SelectedIngesta.Code_Package = HFCodePackage.Value;
                            }

                            HiddenField HFEventID = (HiddenField)_currenrtrow.FindControl("HFEventID");
                            if (HFEventID != null)
                            {
                                SelectedIngesta.EventID = HFEventID.Value;
                            }
                        }


                        //---- The Time start of Selection
                        int nHora = int.Parse(lblStartTimeResultat.Text.Substring(0, lblStartTimeResultat.Text.IndexOf(":")));
                        int nMinut = int.Parse(lblStartTimeResultat.Text.Substring(lblStartTimeResultat.Text.IndexOf(":") + 1));
                        DateTime selectedDateTime = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, nHora, nMinut, 0);

                        //---- The Time 
                        nHora = int.Parse(txtNewStartTime.Text.Substring(0, txtNewStartTime.Text.IndexOf(":")));
                        nMinut = int.Parse(txtNewStartTime.Text.Substring(txtNewStartTime.Text.IndexOf(":") + 1));

                        DateTime NewDateTime = Convert.ToDateTime(lblStartDateResult.Text); //DateTime.Parse(lblStartDateResult.Text);
                        NewDateTime = new DateTime(NewDateTime.Year, NewDateTime.Month, NewDateTime.Day, nHora, nMinut, 0);

                        if (NewDateTime >= selectedDateTime)
                        {
                            SelectedIngesta.IdIngesta = idIngesta; 
                            SelectedIngesta.StartTime = NewDateTime;

                            Session["SelectedIngesta"] = SelectedIngesta;

                            //----After the Save , Redirect the User to Page : TimeTable to continue the process Scheduling
                            Response.Redirect("~/VodPrograming.aspx", false);
                        }
                        else
                        {
                            ShowError(string.Format("MAM_ListPushVodOverDVBS_TimeError {0}", selectedDateTime.ToString()));
                        }
                    }
                }
            }
        }

        protected void gvwdatacast_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gvwdatacast_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvwdatacast.PageIndex = e.NewPageIndex;
            BindGridView();
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method (s) -.-.-.-.-.-.-.-.-.-.-.-

        protected void BindGridView()
        {
            try
            {
                List<mebs_ingesta> _listIngesta = _context.Execute<mebs_ingesta>(new Uri(string.Format(Config.GetPackagesToSchedule,Convert.ToInt32(BLC.MediaType.DC_CHANNEL)), UriKind.Relative)).ToList();

                if (_listIngesta == null || _listIngesta.Count <= 0)
                {
                    ShowMessage(BLC.DefaultValue.MSG_DATA_NOT_FOUND);
                    TableTiming.Visible = false;
                    this.gvwdatacast.DataSource = null;
                    this.gvwdatacast.DataBind();
                }
                else
                {
                    this.gvwdatacast.DataSource = _listIngesta;
                    this.gvwdatacast.DataBind();
                }
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
                        LogHelper.logger.Error(string.Format("UC_AuthorizedVodListControl : Bind : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("UC_AuthorizedVodListControl : Bind : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("UC_AuthorizedVodListControl : Bind : {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void InitialisePageControl()
        {
            //---- Get the Selected Cellule
            CPintaTaula SelectedCellule = (CPintaTaula)Session["SelectedCellule"];
            if (SelectedCellule == null)
            {
                ShowError("NO_SELECTED_CELULLE");
                return;
            }


            lblStartTime.Text = "Time";
            if (SelectedCellule.SecondStart != -1)
            {
                //Session["NewStartTime"] = BLC.DateTimeHelper.ConvertSecondsToHours(SelectedCellule.SecondStart);  //Session["StartHmax"].ToString();
                string StartHmax = BLC.DateTimeHelper.ConvertSecondsToHours(SelectedCellule.SecondStart).ToString();//Session["NewStartTime"].ToString();
                if (!string.IsNullOrEmpty(StartHmax))
                {
                    int nHora = int.Parse(StartHmax.Substring(0, StartHmax.IndexOf(":")));
                    int nMinut = int.Parse(StartHmax.Substring(StartHmax.IndexOf(":") + 1));

                    lblStartTimeResultat.Text = string.Format("{0}:{1}", BLC.DateTimeHelper.DisplayValueInDateFormat(nHora), BLC.DateTimeHelper.DisplayValueInDateFormat(nMinut));
                    txtNewStartTime.Text = string.Format("{0}:{1}", BLC.DateTimeHelper.DisplayValueInDateFormat(nHora), BLC.DateTimeHelper.DisplayValueInDateFormat(nMinut));


                    //---- End Time
                    if (SelectedCellule.SecondEndHmax != -1)
                    {
                        string EndHmax = BLC.DateTimeHelper.ConvertSecondsToHours(SelectedCellule.SecondEndHmax); //Session["EndHmax"].ToString();
                        nHora = int.Parse(EndHmax.Substring(0, EndHmax.IndexOf(":")));
                        nMinut = int.Parse(EndHmax.Substring(EndHmax.IndexOf(":") + 1));
                        lblEndTimeResult.Text = string.Format("{0}:{1}", BLC.DateTimeHelper.DisplayValueInDateFormat(nHora), BLC.DateTimeHelper.DisplayValueInDateFormat(nMinut));
                    }
                   // string.Format("{0:00}/{1:00}/{2:0000}", Item.StartTime.Day, Item.StartTime.Month, Item.StartTime.Year)
                    if (SelectedCellule.StartTime.Date != DateTime.MinValue.Date)
                    {
                        lblStartDateResult.Text = string.Format("{0:00}/{1:00}/{2:0000}", SelectedCellule.StartTime.Day, SelectedCellule.StartTime.Month, SelectedCellule.StartTime.Year);
                        lblEndDateResult.Text = string.Format("{0:00}/{1:00}/{2:0000}", SelectedCellule.StartTime.Day, SelectedCellule.StartTime.Month, SelectedCellule.StartTime.Year);
                    }
               }
            }



            //if (Session["StartHmax"] != null)
            //{
            //    Session["NewStartTime"] = Session["StartHmax"].ToString();

            //    string StartHmax = Session["NewStartTime"].ToString();
            //    if (!string.IsNullOrEmpty(StartHmax))
            //    {
            //        int nHora = int.Parse(StartHmax.Substring(0, StartHmax.IndexOf(":")));
            //        int nMinut = int.Parse(StartHmax.Substring(StartHmax.IndexOf(":") + 1));

            //        lblStartTimeResultat.Text = string.Format("{0}:{1}", CTools.DisplayValueInDateFormat(nHora), CTools.DisplayValueInDateFormat(nMinut));
            //        txtNewStartTime.Text = string.Format("{0}:{1}", CTools.DisplayValueInDateFormat(nHora), CTools.DisplayValueInDateFormat(nMinut));


            //        //---- End Time
            //        if (Session["EndHmax"] != null)
            //        {
            //            string EndHmax = Session["EndHmax"].ToString();
            //            nHora = int.Parse(EndHmax.Substring(0, EndHmax.IndexOf(":")));
            //            nMinut = int.Parse(EndHmax.Substring(EndHmax.IndexOf(":") + 1));
            //            lblEndTimeResult.Text = string.Format("{0}:{1}", CTools.DisplayValueInDateFormat(nHora), CTools.DisplayValueInDateFormat(nMinut));
            //        }

            //        if (Session["DataIni"] != null)
            //        {
            //            lblStartDateResult.Text = Session["DataIni"].ToString();
            //            lblEndDateResult.Text = Session["DataIni"].ToString();
            //        }
            //    }
            //}
        }

        #endregion

    }
}