using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.Helper;
using MTV.MAM.WebApp.MEBSCatalog;
using System.Data.Services.Client;
using System.Xml.Linq;
using System.Globalization;
using System.Web.UI.HtmlControls;

namespace MTV.MAM.WebApp.Modules
{
    public partial class UC_VodProgramingControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-.-
        public mebsEntities _context;
        static readonly object lockPublishDC = new object();
        public bool IsSelected = false;
        public bool IsContent = false;
        public bool IsToSchedule = false;

        public int ItemId = 0;
        public int startHourOfWeekPlanner = 14;	// Start hour of Calander
        public int endHourOfWeekPlanner = 23;	// End hour of Calander.
        public DateTime SelectedDateTime;
        public double date = 0;
        public string selected = "";
        public DateTime Timing;

        public int initilizeTopHours = 1;
        public int CalenderStartHour = 1;
        public DateTime SetdateStartOfWeek;
        public DateTime SetDateStopOfWeek;
        public int itemRowHeight = 179;
        public DateTime ExatStart;
        public DateTime ExactStop;
        public int nbrDays = 3;
        int nBitrate = 0;

        protected int m_nHoraIniTimeInterval = 0;
        protected int m_nHoraFiTimeInterval = 0;
        protected string m_strTimeInterval = ""; //format: horaIni(enter)Bloc1_horaFiBloc1_horaIniBloc2(si hi ha)_horaFiBloc2(si hi ha)
        protected bool m_BlocNoSchedulableSelecc = false;
        protected bool m_BlocNoSchedulableSeleccTV = false;
        protected bool CreatTableInfo = false;

        public const int SEGONS_5_MINUTS = 300;
        public const int SEGONS_DIA_MENYS_1 = 86399;
        int nBitsTimeInterval = BLC.DefaultValue.Default_DC_TimeInterval;

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
            LoadJSandCSS();
            if (!IsPostBack)
            {
                SetDefaultValues();
                nBitsTimeInterval = BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_TimeInterval), BLC.DefaultValue.Default_DC_TimeInterval);
            }

            if (!string.IsNullOrEmpty(this.SelectedDate))
            {
                if (this.SelectedCellule == null)
                {
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                    ShowError(BLC.DefaultValue.MSG_NO_SELECTED_TIMEZONE);
                    TableCreation();
                    return;
                }

                if (this.SelectedIngesta == null) // && this.ScheduleToModify == null
                {
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                    ShowError(BLC.DefaultValue.MSG_NO_SELECTED_PRODUCT);
                    TableCreation();
                    return;
                }

                //------ Importer tout les éléments du même paquet.
                List<mebs_ingesta> lPackageElements = _context.Execute<mebs_ingesta>(new Uri(string.Format(Config.GetPackageByCode, SelectedIngesta.Code_Package),UriKind.Relative)).ToList();
                if (lPackageElements == null || lPackageElements.Count <= 0)
                {
                    ShowError(BLC.DefaultValue.MSG_PRODUCT_NOT_FOUND);
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                    TableCreation();
                    return;
                }

                //----- Get the Bitrate frm Database
                nBitrate = BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_User_Bitrate), BLC.DefaultValue.Default_DC_User_Bitrate);

                if (this.SelectedIngesta.StartTime.Date != DateTime.MinValue.Date)
                {
                    if (this.SelectedIngesta.StartTime <= DateTime.UtcNow)
                    {
                        //ShowError(string.Format("MAM_TimeTable_Instructions_Time_Frame {0}", BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Time_Frame), BLC.DefaultValue.Default_DC_Time_Frame)));
                        IsContent = false;
                        IsSelected = true;
                        IsToSchedule = false;
                        TableCreation();
                        return;
                    }


                    //----- Get the Size of the Movie (After Redandancy)  - Movie
                    DateTime dtInitial_StartTime;
                    int nTempsEnviamentSeg;
                    long lContentFileSize = 0;
                    DateTime dIni, dEnd = new DateTime();
                    lContentFileSize = GetFileSize(lPackageElements[0]);

                    if (lContentFileSize == 0)
                    {
                        this.SelectedIngesta = null;
                        this.ScheduleToModify = null;
                        ShowError(BLC.DefaultValue.MSG_PRODUCT_SIZE_ERROR);
                        //------------------------------------------------------------------------------------------------------------IOUtil.FreeLockedTimeTableOfUser();
                        TableCreation();
                        return;
                    }
                    this.SelectedIngesta.RedundancyFileSize = lContentFileSize;
                    this.SelectedIngesta.Title = lPackageElements[0].Title;
                    this.SelectedIngesta.Duration = GetDuration(lPackageElements[0]);

                    // return;
                    //string StartHmax = Session["NewStartTime"].ToString();
                    //int nHora = int.Parse(StartHmax.Substring(0, StartHmax.IndexOf(":")));
                    //int nMinut = int.Parse(StartHmax.Substring(StartHmax.IndexOf(":") + 1));
                    dIni = this.SelectedIngesta.StartTime;  //new DateTime(this.SelectedCellule.StartTime.Year, this.SelectedCellule.StartTime.Month, this.SelectedCellule.StartTime.Day, nHora, nMinut, 0);
                    dEnd = dIni; //Initialise the End of the Package
                    //int iInterval_between_schedule = BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Inter_PackageElements_Time_Gap), BLC.DefaultValue.Default_DC_IntervalPackage);  //Convert.ToInt32(Bestv.Library.DataAccess.SettingsProvider.GetSettings("DC_IntervalPackage").Value); //60; //-- Put in table setting in configuration
                    //int nElement = 0;
                    /********************************************************************/
                    /*--- Get the Advertisement Linked to the Selected Ingesta (If Exist)/*
                    /********************************************************************/
                    #region Advertisement
                    //ViewState["ADV_Timing"] = null;
                    //Session["IngestaADV"] = null;
                    //List<PublicitatTiming> ListPublicitatTiming = new List<PublicitatTiming>();

                    //if (SelectedIngesta.Self_commercial == Convert.ToInt32(SelfCommercial.Linked)) //----- The Content Is Lincked to ADV
                    //{
                    //    TstDictionary TstIngestaPublicitat = new TstDictionary();
                    //    List<IngestaPublicitat> ListIngestaPublicitat;
                    //    nElement = IngestaPublicitatProvider.GetIngestaPubLinkedByIngestaV2(SelectedIngesta.PackageId, Convert.ToInt32(MediaType.DATA_CHANNEL), out ListIngestaPublicitat);
                    //    if (nElement > 0)
                    //    {
                    //        foreach (IngestaPublicitat PubItem in ListIngestaPublicitat)
                    //        {
                    //            if (!TstIngestaPublicitat.Contains(PubItem.GuidPublicitat))
                    //                TstIngestaPublicitat.Add(PubItem.GuidPublicitat, PubItem);

                    //            nTempsEnviamentSeg = TempsEnviamentSeg(PubItem.Tamany, nBitrate);
                    //            dEnd = dIni.AddSeconds((double)nTempsEnviamentSeg);
                    //            if (dEnd.Second != 0)
                    //            {
                    //                dEnd = dEnd.AddSeconds(-dEnd.Second);
                    //                dEnd = dEnd.AddMinutes(1);
                    //            }
                    //            ListPublicitatTiming.Add(new PublicitatTiming(PubItem.GuidPublicitat, dIni, dEnd));
                    //            //---- Add the Difference Time between the End of the Content and the Start of the Triler
                    //            dEnd = dEnd.AddSeconds(iInterval_between_schedule);
                    //            dIni = dEnd;
                    //        }
                    //        Session["IngestaADV"] = TstIngestaPublicitat;
                    //        ViewState["ADV_Timing"] = ListPublicitatTiming;
                    //    }
                    //}
                    #endregion

                    /********************************************************************/
                    /*--- Get the Triller Linked to the Selected Ingesta (If Exist)     /*
                    /********************************************************************/
                    #region Trailer
                    //mebs_ingesta TrailerItem = MEBSMAMHelper.GetIngestaByType(lPackageElements, Convert.ToInt32(BLC.MediaType.TRAILER_CHANNEL));
                    //if (TrailerItem != null)//If Exist
                    //{
                    //    lTrailerFileSize = GetRedundancyFileSize(TrailerItem);
                    //    if (lTrailerFileSize > 0)
                    //    {
                    //        //Session["IngestaTrailer"] = TrailerItem;

                    //        nTempsEnviamentSeg = TempsEnviamentSeg(lTrailerFileSize, nBitrate);
                    //        dEnd = dIni.AddSeconds((double)nTempsEnviamentSeg);
                    //        if (dEnd.Second != 0)
                    //        {
                    //            dEnd = dEnd.AddSeconds(-dEnd.Second);
                    //            dEnd = dEnd.AddMinutes(1);
                    //        }
                    //        //ViewState["Trailer_StartTime"] = dIni;
                    //        //ViewState["Trailer_EndTime"] = dEnd;
                    //        SelectedIngesta.Trailer_StartTime = dIni;
                    //        SelectedIngesta.Trailer_EndTime = dEnd;

                    //        //---- Add the Difference Time between the End of the Content and the Start of the Triler
                    //        dEnd = dEnd.AddSeconds(iInterval_between_schedule);
                    //    }
                    //}
                    //else
                    //{
                    //    SelectedIngesta.Trailer_StartTime = DateTime.MinValue;
                    //    SelectedIngesta.Trailer_EndTime = DateTime.MinValue;
                    //    //ViewState["Trailer_StartTime"] = null;
                    //    //ViewState["Trailer_EndTime"] = null;
                    //    //Session["IngestaTrailer"] = null;
                    //}
                    #endregion

                    /********************************************************************/
                    /*--- Get The Bonus Linked to the Selected Ingesa (if Exist)        /*
                    /********************************************************************/
                    #region Bonus
                    //mebs_ingesta BonusItem = MEBSMAMHelper.GetIngestaByType(lPackageElements, Convert.ToInt32(BLC.MediaType.BONUS_CHANNEL));
                    //if (BonusItem != null)//If Exist
                    //{
                    //    lBonusFileSize = GetRedundancyFileSize(BonusItem);
                    //    if (lBonusFileSize > 0)
                    //    {
                    //        //Session["IngestaBonus"] = BonusItem;

                    //        nTempsEnviamentSeg = TempsEnviamentSeg(lBonusFileSize, nBitrate);
                    //        dIni = dEnd;
                    //        dEnd = dIni.AddSeconds((double)nTempsEnviamentSeg);
                    //        if (dEnd.Second != 0)
                    //        {
                    //            dEnd = dEnd.AddSeconds(-dEnd.Second);
                    //            dEnd = dEnd.AddMinutes(1);
                    //        }

                    //        //ViewState["Bonus_StartTime"] = dIni;
                    //        //ViewState["Bonus_EndTime"] = dEnd;
                    //        SelectedIngesta.Bonus_StartTime = dIni;
                    //        SelectedIngesta.Bonus_EndTime = dEnd;

                    //        dEnd = dEnd.AddSeconds(iInterval_between_schedule);
                    //    }
                    //}
                    //else
                    //{
                    //    ViewState["Bonus_StartTime"] = null;
                    //    ViewState["Bonus_EndTime"] = null;
                    //    Session["IngestaBonus"] = null;
                    //}
                    #endregion

                    nTempsEnviamentSeg = TempsEnviamentSeg(lContentFileSize, nBitrate);
                    dIni = dEnd;
                    dEnd = dIni.AddSeconds((double)nTempsEnviamentSeg);
                    if (dEnd.Second != 0)
                    {
                        dEnd = dEnd.AddSeconds(-dEnd.Second);
                        dEnd = dEnd.AddMinutes(1);
                    }
                    //----- save the EndTime od the Content in a Session
                    //ViewState["Content_StartTime"] = dIni;
                    //ViewState["Content_EndTime"] = dEnd;
                    this.SelectedIngesta.Content_StartTime = dIni;
                    this.SelectedIngesta.Content_EndTime = dEnd;


                    //if (ListPublicitatTiming != null && ListPublicitatTiming.Count > 0)
                    //    dtInitial_StartTime = ListPublicitatTiming[0].StartTime;
                    //if (ViewState["Trailer_StartTime"] != null)
                    //    dtInitial_StartTime = (DateTime)ViewState["Trailer_StartTime"];
                    //else if (ViewState["Bonus_StartTime"] != null)
                    //    dtInitial_StartTime = (DateTime)ViewState["Bonus_StartTime"];
                    //else
                    //    dtInitial_StartTime = (DateTime)ViewState["Content_StartTime"];


                    //if (SelectedIngesta.Trailer_StartTime != DateTime.MinValue)
                    //    dtInitial_StartTime = SelectedIngesta.Trailer_StartTime;
                    //else if (SelectedIngesta.Bonus_StartTime != DateTime.MinValue)
                    //    dtInitial_StartTime = SelectedIngesta.Bonus_StartTime;
                    //else
                        dtInitial_StartTime = SelectedIngesta.Content_StartTime;


                    //--- Check If the Time Is enought to send the All package (Ingesta + Triller and Bonus if Exist and the Advertisement)
                    int resultat = ComprovarSiCab(dtInitial_StartTime, dEnd);

                    //--- Session Used In the Page IngestaInformation
                    //Session["EstimatedStartPackage"] = dtInitial_StartTime;
                    //Session["EstimatedStopPackage"] = dEnd;

                    if (resultat == 0)
                    {
                        //Session["SelectedIngesta"] = SelectedIngesta;
                        IsContent = true;
                        IsSelected = true;
                        IsToSchedule = true;
                    }
                    else if (resultat == 1)
                    {
                        //no hi cab
                        ShowError(BLC.DefaultValue.MSG_OVERLAPPING_DETECTED);//GetLocaleStringResource("MAM_TimeTable_Error_No_Time")
                        IsContent = false;
                        IsSelected = true;
                        IsToSchedule = false;
                    }
                    else
                    {
                        //no hi cab
                        ShowError(BLC.DefaultValue.MSG_TIME_NOT_ENOUGH); 
                        IsContent = false;
                        IsSelected = true;
                        IsToSchedule = false;
                    }
                }
                TableCreation();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ShowError(string.Empty);
            ShowMessage(string.Empty);
            this.SelectedDate = txtSelectedDate.Text;
            this.SelectedIngesta = null;
            this.ScheduleToModify = null;
            this.CellId = string.Empty;

            if (!string.IsNullOrEmpty(this.SelectedDate))
            {
                TableCreation();
            }
        }

        protected void btnParcourir_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/AuthorizedVodList.aspx", false);
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //-- Reload The Page
            this.SelectedDate = string.Empty;
            BLC.CommonHelper.ReloadCurrentPage();
            
        }

        protected void ContentPanel_OnClick(object sender, EventArgs e)
        {
            ShowError("");

            // Get the Selected Button
            ClickablePanel ThisPanel = (ClickablePanel)sender;

            //-- Get the ID of The Selected Button (Exp Col_Cell : 9_0)
            this.CellId = ThisPanel.ID.ToString();

            //-- Clear the Selected Ingesta (If change the selecte Cellule) - avoir avec Imad
            this.SelectedIngesta = null;
            IsToSchedule = false;

            //-- Reload The Page
            if (!string.IsNullOrEmpty(this.SelectedDate))
            {
                PlaceTimeTable.Controls.Clear();
                CreateTimeTable();
                //Enabled the button selon the Selected Cellule
                EnabledButton();
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (this.ScheduleToModify != null)
                {
                    Response.Redirect("~/DCScheduleDetails.aspx", false);
                }
            }

        }

        protected void btnSchedule_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (this.SelectedIngesta == null)
                    return;

                if (this.SelectedCellule == null)
                    return;

                DateTime dtInitial_StartTime = this.SelectedIngesta.Content_StartTime;
                DateTime dEnd = this.SelectedIngesta.Content_EndTime;

                int resultat = ComprovarSiCab(dtInitial_StartTime, dEnd);
                if (resultat == 0)
                {
                    PublishItem();
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                }
                else if (resultat == 1)
                {
                    //no hi cab
                    ShowError(BLC.DefaultValue.MSG_OVERLAPPING_DETECTED);
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                }
                else
                {
                    //no hi cab
                    ShowError(BLC.DefaultValue.MSG_TIME_NOT_ENOUGH);
                    IsContent = false;
                    IsSelected = false;
                    IsToSchedule = false;
                }

                if (!string.IsNullOrEmpty(this.SelectedDate))
                {
                    this.CellId = null;
                    PlaceTimeTable.Controls.Clear();
                    CreateTimeTable();
                    //Enabled the button selon the Selected Cellule
                    EnabledButton();
                }

            }
        }

        private void Page_PreRender(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.SelectedDate))
            {
                string obj = "text_charts";
                string evenement = "javascript:LoadIngestaInformation(document.getElementById('" + obj + "'));";

                PHLegend.Visible = true;
                PH_IngestaInformation.Visible = true;
                Page.ClientScript.RegisterStartupScript(this.GetType(), "myscript", evenement, true);
            }
            else
            {
                PH_IngestaInformation.Visible = false;
                PHLegend.Visible = false;
            }
        }

        protected void btnPVODReport_Click(object sender, EventArgs e)
        {
            //if (Page.IsValid)
            //{
            //    if (this.ScheduleToModify != null)
            //    {
            //        Response.Redirect("~/PVODReport.aspx", false);
            //    }
            //}
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : TimeTable Design -.-.-.-.-.-.-.-.-.-.-.-
        protected void TableCreation()
        {
            PlaceTimeTable.Controls.Clear();
            CreateTimeTable();
            //Enabled the button selon the Selected Cellule
            EnabledButton();
        }
        //----- Step 01 : OK
        /// <summary>
        /// Create the TimeTable
        /// </summary>
        protected void CreateTimeTable()
        {
            DateTime UsedDate = new DateTime();
            SelectedDateTime = GenerateDateFromString(this.SelectedDate);
            //Number of days to Display
            nbrDays = 5;

            //int nBitsTimeInterval = BLC.CommonHelper.ConvertStringToInt(GetSettingValue(BLC.DefaultValue.DC_TimeInterval), BLC.DefaultValue.Default_DC_TimeInterval);
            BLC.DateTimeHelper.CalculaHoraIniHoraFiTimeInterval(nBitsTimeInterval, out m_strTimeInterval, out m_nHoraIniTimeInterval, out m_nHoraFiTimeInterval);

            startHourOfWeekPlanner = m_nHoraIniTimeInterval; // to Get from Database, bessing 
            endHourOfWeekPlanner = m_nHoraFiTimeInterval - 1;
            Timing = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, startHourOfWeekPlanner, 0, 0);

            initCalendar();

            Panel Container = new Panel();
            Container.ID = "container_TimeTable";
            Container.CssClass = "container";
            string TotalWidth = string.Format("{0}px", (61 + 20) + (nbrDays * 150));
            Container.Style.Add(HtmlTextWriterStyle.Width, TotalWidth);


            Panel Panel_container = new Panel();
            Panel_container.ID = "Panel_container";
            Panel_container.CssClass = "Panel_container";
            TotalWidth = string.Format("{0}px", (61 + 20) + (nbrDays * 150) + nbrDays);
            Panel_container.Style.Add(HtmlTextWriterStyle.Width, TotalWidth);

            Panel Panel_top = new Panel();
            Panel_top.ID = "Panel_top";
            Panel_top.CssClass = "Panel_top";
            Panel_container.Controls.Add(Panel_top);

            Panel Panel_spacer = new Panel();
            Panel_spacer.CssClass = "spacer";
            Label lblspacer = new Label();
            Panel_spacer.Controls.Add(lblspacer);
            Panel_top.Controls.Add(Panel_spacer);

            Panel Scheduler_dayRow = new Panel();
            Scheduler_dayRow.ID = "weekScheduler_dayRow";
            Scheduler_dayRow.CssClass = "days";

            CultureInfo _CulturInfos = new CultureInfo("en-US");

            for (int i = 0; i < nbrDays; i++)
            {
                Panel Sub_dayRow = new Panel();
                Label lblSub = new Label();
                lblSub.Text = SelectedDateTime.AddDays(i).ToString("dddd, dd MMM", _CulturInfos);
                lblSub.CssClass = "DaysTitle";
                Sub_dayRow.Controls.Add(lblSub);
                Scheduler_dayRow.Controls.Add(Sub_dayRow);
            }
            Panel_top.Controls.Add(Scheduler_dayRow);

            Panel Panel_cal_content = new Panel();

            int Panel_cal_content_Height = 0;
            Panel_cal_content_Height = 182 * (endHourOfWeekPlanner + 1);
            if (Panel_cal_content_Height > 470)
            {
                Panel_cal_content_Height = 600;
            }

            Panel_cal_content.ID = "Panel_cal_content";
            Panel_cal_content.CssClass = "Panel_cal_content";
            Panel_cal_content.Height = Panel_cal_content_Height;
            Panel_cal_content.ScrollBars = ScrollBars.Auto;
            Panel_cal_content.Attributes.Add("onscroll", "SetDivPosition()");

            /*************************************************************************/

            Panel Panel_cal_hours = new Panel();
            Panel_cal_hours.ID = "Panel_cal_hours";
            Panel_cal_hours.CssClass = "Panel_cal_hours";
            for (int no = startHourOfWeekPlanner; no <= endHourOfWeekPlanner; no++)
            {
                Panel PanelContentTime = new Panel();
                PanelContentTime.CssClass = "PanelContentTime";


                Panel DivHoure = new Panel();
                DivHoure.CssClass = "DivHoureStyle";

                Label lblTime = new Label();
                lblTime.Text = Timing.ToString("HH:mm");
                lblTime.CssClass = "hours_style";
                DivHoure.Controls.Add(lblTime);

                Panel DivBlockIndicator = new Panel();
                DivBlockIndicator.CssClass = "DivBlockIndicatorStyle";

                int Block = 0;
                for (int a = 0; a < 12; a++)
                {
                    Panel DivIndicator = new Panel();

                    if (Block == 0 || Block == 60)
                    {
                        DivIndicator.CssClass = "Block60Minutes";
                    }
                    else if (Block != 15 && Block != 30 && Block != 45)
                    {
                        DivIndicator.CssClass = "Block5Minutes";
                    }
                    else
                    {
                        DivIndicator.CssClass = "Block15Minutes";
                    }

                    Block = Block + 5;
                    DivBlockIndicator.Controls.Add(DivIndicator);
                }


                PanelContentTime.Controls.Add(DivBlockIndicator);
                PanelContentTime.Controls.Add(DivHoure);
                Panel_cal_hours.Controls.Add(PanelContentTime);

                Timing = Timing.AddMinutes(60);
            }

            /*************************************************************************/

            Panel Panel_cal_appointments = new Panel();
            Panel_cal_appointments.ID = "Panel_cal_appointments";
            Panel_cal_appointments.CssClass = "Panel_cal_appointments";
            string cal_appointments_Width = string.Format("{0}px", (150 * nbrDays) + nbrDays);
            Panel_cal_appointments.Style.Add(HtmlTextWriterStyle.Width, cal_appointments_Width);


            for (int no = 0; no < nbrDays; no++)
            {
                Panel Panel_appoint_day = new Panel();
                if (no % 2 == 0)
                {
                    Panel_appoint_day.CssClass = "Panel_appoint_day";
                }
                else
                {
                    Panel_appoint_day.CssClass = "Panel_appoint_day_Alternative";
                }

                Panel Scheduler_appointment_hour = new Panel();
                Scheduler_appointment_hour.ID = string.Format("Scheduler_appointment_hour{0}", no);
                Scheduler_appointment_hour.CssClass = "Panel_appointHour";
                Scheduler_appointment_hour.Style.Add(HtmlTextWriterStyle.Height, string.Format("{0}px", (itemRowHeight + 1) * ((endHourOfWeekPlanner - startHourOfWeekPlanner) + 1)));

                ItemId = 0;
                UsedDate = SelectedDateTime.AddDays(no);

                List<CPintaTaula> ItemFromDB = getItemsFromDBServer(UsedDate);

                foreach (CPintaTaula item in ItemFromDB)
                {
                    ExatStart = item.StartTime;
                    ExactStop = item.EndTime;

                    if (ExatStart >= SetdateStartOfWeek)
                    {
                        Int64 dayDiff = GetTime(ExatStart) - GetTime(SetdateStartOfWeek);
                        dayDiff = (Int64)Math.Floor((double)dayDiff / (1000 * 60 * 60 * 24));
                        if (no == dayDiff)
                        {
                            Scheduler_appointment_hour.Controls.Add(CreateNewContentPanel(item));
                        }
                    }
                }

                Panel_appoint_day.Controls.Add(Scheduler_appointment_hour);

                Panel_cal_appointments.Controls.Add(Panel_appoint_day);
            }


            Panel_cal_content.Controls.Add(Panel_cal_hours);
            Panel_cal_content.Controls.Add(Panel_cal_appointments);
            Panel_container.Controls.Add(Panel_cal_content);

            Container.Controls.Add(Panel_container);
            PlaceTimeTable.Controls.Add(Panel_container);
        }

        //----- Step 02 : OK
        /// <summary>
        /// Initial the paramaters used by the TimaTable
        /// </summary>
        protected void initCalendar()
        {
            //try
            //{
            HttpBrowserCapabilities browser = Request.Browser;
            if (browser.Type == "IE6" && browser.Browser == "IE" && browser.Version == "6.0")
            {
                itemRowHeight = 180;
            }

            CalenderStartHour = startHourOfWeekPlanner;
            initilizeTopHours = startHourOfWeekPlanner;


            SetdateStartOfWeek = SelectedDateTime;
            SetDateStopOfWeek = SelectedDateTime.AddDays(2);
        }
        
        //---- Step 03 : OK
        /// <summary>
        /// Create a New Clickable Button depond conditions
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        protected ClickablePanel CreateNewContentPanel(CPintaTaula Item)
        {
            ClickablePanel ContentPanel = new ClickablePanel();
            //try
            //{
            int topPos = (int)Math.Round(getYPositionFromTime(ExatStart.Hour, ExatStart.Minute));

            decimal TotalTime = (decimal)(GetTime(ExactStop) - GetTime(ExatStart));
            decimal Diviseur = (decimal)(60 * 60 * 1000);
            decimal elHeight = TotalTime / Diviseur;

            elHeight = Math.Round((elHeight * (itemRowHeight + 1)) - 2);

            HttpBrowserCapabilities browser = Request.Browser;
            if (browser.Type == "IE6" && browser.Browser == "IE" && browser.Version == "6.0")
            {
                elHeight = elHeight + 1;
                topPos = topPos - 1;
            }
            if (topPos < 0)
            {
                topPos = 0;
            }

            ContentPanel.ID = string.Format("{0}_{1}", Item.StartTime.Day, ItemId);
            ItemId++;
            ContentPanel.Click += new EventHandler(ContentPanel_OnClick);

            ContentPanel.Height = (Unit)elHeight;

            //Check If The Time if Free

            if (Item.FreeSpace == true)
            {
                //this.ScheduleToModify = null;
                if (Item.EnableSchedule)
                {
                    if (Item.SelectedSpace)
                    {
                        ContentPanel.CssClass = "EnableScheduleSelected";
                        this.SelectedCellule = Item;
                        IsSelected = true;
                        IsContent = false;
                        this.ScheduleToModify = null;
                    }
                    else
                    {
                        ContentPanel.CssClass = "EnableSchedule";
                    }
                }
                else
                {
                    ContentPanel.CssClass = "NotEnableSchedule";
                    if (Item.SelectedSpace)
                    {

                        IsSelected = false;
                        IsContent = false;
                        IsToSchedule = false;

                        if (m_BlocNoSchedulableSelecc)
                        {
                            // m_labelError.Text = ResourceManager.GetString("ERROR_NOT_SCHEDULABLE");
                            ShowError("MAM_TimeTable_Error_Not_Schedulable");

                        }
                        else if (Item.Type == Convert.ToInt32(BLC.MediaType.DUMMY_SCHEDULE))
                        {
                            //string.Format(GetLocaleStringResource("MAM_TimeTable_Instructions_Time_Frame"), Convert.ToInt32(Bestv.Library.DataAccess.SettingsProvider.GetSettings("DC_Time_Frame").Value))
                            ShowError(string.Format("MAM_TimeTable_Instructions_Time_InterPackage {0}", BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Inter_Package_Time_Gap), BLC.DefaultValue.Default_DC_Inter_Package_Time_Gap)));
                        }
                        else
                        {
                            //m_labelError.Text = string.Format(ResourceManager.GetString("INSTRUCTIONS_TIME_FRAME"), m_MySQLMMPSDataBase.GetHoursTimeFrame());
                            ShowError(string.Format("MAM_TimeTable_Instructions_Time_Frame {0}", BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Time_Frame), BLC.DefaultValue.Default_DC_Time_Frame)));
                        }
                    }
                }
            }
            else
            {
                //espai ocupat
                //Label lblTransmissionType;
                string strStatus = string.Empty, strImgName = string.Empty;
                HtmlTable Mytable = new HtmlTable();
                HtmlTableCell tc = new HtmlTableCell();
                HtmlTableRow tr = new HtmlTableRow();
                Mytable.Height = string.Format("{0}px", elHeight);
                Mytable.Width = "100%";
                Mytable.Attributes.Add("style", "border-left:solid 4px #b52e37;	padding:0 0 0 0px;margin: 0 0 0 0px;");
                //Schedule ObjSchedule = IOUtil.GetSchedulePackageV2(Item.ItemSchedule);

                //int _PackageStatus = IOUtil.GetSchedulePackageStatusV2(ObjSchedule);

                if (elHeight > 14)
                {
                    //----- 
                    LinkButton lbltxt = new LinkButton();
                    lbltxt.Text = string.Format("{0}", Item.Title);
                    lbltxt.CssClass = "LabelStyle";
                    lbltxt.Attributes.Add("title", DisplayScheduleDetails(Item));

                    tc.Align = "left";
                    tc.VAlign = "top";
                    //strImgName = DisplayStatusNameV2(ObjSchedule, _PackageStatus);

                    Image imgStatus = new Image();
                    imgStatus.ImageUrl = MEBSMAMHelper.GetEventStatusImage(Item.PackageStatus, Item.IsExpired);
                    tc.Controls.Add(imgStatus);
                    //----Display Image of Locked

                    //if (ObjSchedule.DC_IsLockedToDelete && _PackageStatus != Convert.ToInt32(EventProgressStatus.ERROR))
                    //{
                    //    Image imgLock = new Image();
                    //    imgLock.ImageUrl = string.Format("~/{0}/lock.png", Convert.ToString(Properties.EBSMAMDefaultValues.Default.Properties["Common_Path"].DefaultValue));
                    //    tc.Controls.Add(imgLock);
                    //}

                    ////--- Display the Expiration Icon
                    //if (Item.ItemSchedule.IsExpired && _PackageStatus != Convert.ToInt32(EventProgressStatus.ERROR))
                    //{
                    //    Image imgExpired = new Image();
                    //    imgExpired.ImageUrl = string.Format("~/{0}/Expired.png", Convert.ToString(Properties.EBSMAMDefaultValues.Default.Properties["Common_Path"].DefaultValue));
                    //    tc.Controls.Add(imgExpired);
                    //}

                    tc.Controls.Add(lbltxt);
                    tr.Cells.Add(tc);
                    Mytable.Rows.Add(tr);
                    ContentPanel.Controls.Add(Mytable);
                }
                else
                {
                    Image Imgpoint = new Image();
                    Imgpoint.Attributes.Add("title", DisplayScheduleDetails(Item));
                    if (elHeight > 1)
                    {
                        Imgpoint.ImageUrl = BLC.DefaultValue.PNG_Point;
                        tc.VAlign = "middle";
                        tc.Align = "left";
                        tc.Controls.Add(Imgpoint);
                        tr.Cells.Add(tc);
                        Mytable.Rows.Add(tr);
                        ContentPanel.Controls.Add(Mytable);
                    }
                    else
                    {
                        Imgpoint.ImageUrl = BLC.DefaultValue.PNG_Point_Red;
                        ContentPanel.Attributes.Add("style", "text-align:left;vertical-align:top");
                        ContentPanel.Height = 1;
                        ContentPanel.Controls.Add(Imgpoint);
                    }
                }

                if (!Item.SelectedSpace)
                {
                    ContentPanel.CssClass = "ContaintScheduleItem";
                }
                else
                {
                    this.SelectedCellule = Item;
                    ContentPanel.CssClass = "ContaintScheduleItemSelected";
                    this.SelectedIngesta = Item;
                    IsSelected = true;
                    IsContent = true;
                    IsToSchedule = false;
                }
            }

            ContentPanel.Width = 149;
            string strTop = string.Format("{0}px", topPos);
            ContentPanel.Style.Add(HtmlTextWriterStyle.Top, strTop);
            ContentPanel.Style.Add(HtmlTextWriterStyle.Position, "absolute");
            return ContentPanel;
        }

        /// <summary>
        /// Total Time in Milliseconds
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        private Int64 GetTime(DateTime dt)
        {
            Int64 retval = 0;

            DateTime st = new DateTime(1970, 1, 1);
            TimeSpan t = (dt - st);
            retval = (Int64)(t.TotalMilliseconds + 0.5);
            return retval;
        }

        /// <summary>
        /// Get the Position of button
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <returns></returns>
        protected decimal getYPositionFromTime(int hour, int minute)
        {
            decimal res = 0;
            decimal min = minute;
            decimal diviseur = 60;
            decimal res1 = min / diviseur;
            res = Math.Floor((decimal)hour - CalenderStartHour) * (itemRowHeight + 1) + (res1 * (itemRowHeight + 1));
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strDate"></param>
        /// <returns></returns>
        public DateTime GenerateDateFromString(string strDate)
        {
            DateTime NewDate = new DateTime();

            if (!string.IsNullOrEmpty(strDate))
            {
                //string[] _date = strDate.Split('/');
                //NewDate = new DateTime(Convert.ToInt32(_date[2]), Convert.ToInt32(_date[1]), Convert.ToInt32(_date[0]));
                DateTime.TryParse(strDate, out NewDate);
            }
            else
            {
                NewDate = DateTime.UtcNow.ToUniversalTime();
            }
            return NewDate;
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : TimeTable Method (s) -.-.-.-.-.-.-.-.-.-.-.-
        //----- Step 23 : OK
        /// <summary>
        /// Prepare the list of Cellule to be designaded in the TimeTable if there is a Data from dataBase
        /// </summary>
        /// <param name="aItemsPintaTaula"></param>
        /// <param name="data"></param>
        private void PreparaDadesPintaTaula(out List<CPintaTaula> aItemsPintaTaula, DateTime data)
        {
            #region Declarations
            int nCurrSegon = 0;
            int nElements = 0;
            int nItemAfegit = 0;
            string strItemAfegit = string.Empty;
            string nHighlightedCell = string.Empty;
            int nSegonsLimit;
            int SegonsIniElement = 0;
            int SegonsFiElement = 0;
            int i;
            int SegonsFiTimeInterval;
            int SegonsIniTimeInterval;
            int SegonsIniElementAmbMarge;
            List<mebs_schedule> arrayScheduledAux;
            List<mebs_schedule> arrayScheduled = new List<mebs_schedule>();
            CPintaTaula cela;
            mebs_schedule it;
            //mebs_schedule itBuit;
            mebs_schedule itAux;
            DateTime dataHoraLimit;
            int nHoraIniBloc1 = 0;
            int nHoraIniBloc2 = 0;
            int nHoraFiBloc1 = 0;
            int nHoraFiBloc2 = 0;
            bool bTimeInterval2Blocs = false;
            bool bCelaEnableSchedule = false;
            bool bHiHaItemAlBloc2 = false;
            bool bSegonsLimit = false;
            int nElementsAux = 0;

            m_BlocNoSchedulableSelecc = false;

            aItemsPintaTaula = new List<CPintaTaula>();
            it = new mebs_schedule();
            //itBuit = new mebs_schedule();
            _context = new mebsEntities(Config.MTVCatalogLocation);
            #endregion

            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
            nHighlightedCell = this.CellId;

            //--- The Time Bettwen the end of Content and start of Trailer (in Second)

            //data i hora limit permesos per schedular
            dataHoraLimit = DateTime.UtcNow.AddMinutes(BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Time_Frame), BLC.DefaultValue.Default_DC_Time_Frame));  //DateTime.Now.AddHours((double)CMySQLMSPDataBase.GetHoursTimeFrame());
            string strTime = string.Format("{0}:{1}:00", dataHoraLimit.Hour, dataHoraLimit.Minute);
            nSegonsLimit = (int)BLC.DateTimeHelper.HMS2Seconds(strTime);   //BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);

            if (data.Date == dataHoraLimit.Date)//dia Actual (amb temps límit schedulable)
                bSegonsLimit = true;//variable nSegonsLímit vàlida

            //---- Get list of schedule Between Selected date and Number of Days , just the Content Scheduled for Encapsulator
            try
            {
                arrayScheduled = _context.Execute<mebs_schedule>(new Uri(string.Format(Config.GetListScheduleByDate, BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(data)), UriKind.Relative)).ToList();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Warn(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0}", ex.Message));
                }
            }

            if (arrayScheduled != null && arrayScheduled.Count > 0)
            {
                arrayScheduled = CreateDummySchedules(arrayScheduled);
                nElements = arrayScheduled.Count;
            }
            //nElements = GetListScheduleByDateV2(data, out arrayScheduled); //CMySQLMSPDataBase.GetScheduleItemByDate(data, out arrayScheduled,Convert.ToInt32(MediaType.DATA_CHANNEL) );
            if (nElements > 0)
            {
                SegonsIniTimeInterval = BLC.DateTimeHelper.ConvertHoursToSeconds(m_nHoraIniTimeInterval, 0);
                nCurrSegon = SegonsIniTimeInterval;
                SegonsFiTimeInterval = BLC.DateTimeHelper.ConvertHoursToSeconds(m_nHoraFiTimeInterval, 0);

                bTimeInterval2Blocs = BLC.DateTimeHelper.GetSiTimeInterval2Blocs(out nHoraIniBloc1, out nHoraIniBloc2, out nHoraFiBloc1, out nHoraFiBloc2, m_strTimeInterval);

                //prepara dades dels elements i espais entre mig
                for (i = 0; i < nElements; i++)
                {
                    DateTime dIni = new DateTime(), dEnd;

                    it = (mebs_schedule)arrayScheduled[i];
                    dIni = it.Estimated_Start.Value;
                    dEnd = it.Estimated_Stop.Value;

                    if (dIni < data.AddDays(1))
                    {
                        //preparem SegonsIniElement i SegonsFiElement del item
                        SegonsIniElement = BLC.DateTimeHelper.ConvertHoursToSeconds(dIni.Hour, dIni.Minute) + it.Estimated_Start.Value.Second;
                        SegonsFiElement = BLC.DateTimeHelper.ConvertHoursToSeconds(dEnd.Hour, dEnd.Minute); //+ it.Estimated_Stop.Value.Second

                        if (it.Estimated_Stop.Value.Date > data.Date)//item acaba el dia/dies següents
                            SegonsFiElement = SegonsFiTimeInterval;

                        if (it.Estimated_Start.Value.Date < data.Date)//item comença dia/dies anteriors
                            SegonsIniElement = SegonsIniTimeInterval;

                        if (SegonsFiElement == SEGONS_DIA_MENYS_1)
                            SegonsFiElement++;//= Constants.SEGONS_DIA

                        //tallem items si TimeInterval es fa més petit
                        if (SegonsIniElement < SegonsIniTimeInterval)
                            SegonsIniElement = SegonsIniTimeInterval;

                        if (SegonsFiElement > SegonsFiTimeInterval)
                            SegonsFiElement = SegonsFiTimeInterval;

                        if ((SegonsFiElement > SegonsIniTimeInterval) && (SegonsIniElement < SegonsFiTimeInterval))//afegim item si està dins del TimeInterval
                        {
                            if (SegonsIniElement > nCurrSegon)
                            {
                                //afegir espai buit fins al següent item
                                bCelaEnableSchedule = false;
                                if ((data > dataHoraLimit.Date) || ((data == dataHoraLimit.Date) && (SegonsIniElement > nSegonsLimit)))
                                    bCelaEnableSchedule = true;

                                if (bCelaEnableSchedule == false)
                                {
                                    //un sol item per tot l'espai buit
                                    cela = new CPintaTaula();
                                    cela.EnableSchedule = bCelaEnableSchedule;
                                    cela.FreeSpace = true;
                                    cela.SecondStart = nCurrSegon;
                                    cela.SecondEnd = SegonsIniElement;
                                    cela.SelectedSpace = false;
                                    strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                    if (strItemAfegit == nHighlightedCell)
                                    {
                                        //sel·leccionat espai lliure
                                        cela.SelectedSpace = true;
                                        cela.SecondStartHmax = cela.SecondStart;
                                        cela.SecondEndHmax = cela.SecondEnd;
                                    }

                                    nItemAfegit++;
                                    strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);

                                    //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 1, cela.SecondStart, cela.SecondEnd));
                                    cela.StartTime = data.AddSeconds(cela.SecondStart);
                                    cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                    aItemsPintaTaula.Add(cela);
                                    nCurrSegon = SegonsIniElement;
                                }
                                else
                                {
                                    if ((data == dataHoraLimit.Date) && ((nCurrSegon < nSegonsLimit) || (bTimeInterval2Blocs && (nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)))))
                                    {
                                        //separem espai en dos items: 
                                        //part no schedulable
                                        int nSegonsBloc2Aux = 0;
                                        int nSegonsBloc1Aux = 0;
                                        int nSegonsLimitAux = 0;
                                        int SegonsFiAux = 0;

                                        if (bTimeInterval2Blocs)
                                        {
                                            if (nCurrSegon > BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0))
                                                nSegonsLimitAux = nCurrSegon;

                                            if (nCurrSegon < nSegonsLimit)
                                                nSegonsLimitAux = nSegonsLimit;

                                            if ((nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)) && (nCurrSegon > BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)))
                                                nSegonsBloc2Aux = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);

                                            if ((nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)) && (nCurrSegon <= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)))
                                            {
                                                nSegonsBloc2Aux = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                                nSegonsBloc1Aux = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                                            }

                                            if (SegonsIniElement >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0))
                                                bHiHaItemAlBloc2 = true;

                                            if ((nSegonsBloc2Aux == 0) && (nSegonsBloc1Aux == 0))//controlem segonsLimit vs. segonsBloc2
                                            {
                                                if (nSegonsLimitAux > nSegonsBloc2Aux)
                                                {
                                                    SegonsFiAux = nSegonsLimitAux;

                                                    if (nCurrSegon < SegonsFiAux)
                                                    {
                                                        SegonsFiAux = nSegonsLimitAux;
                                                        //part no schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = false;
                                                        cela.FreeSpace = true;
                                                        cela.EnableSchedule = false;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)//sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;

                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 2000, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);
                                                    }
                                                }
                                                else
                                                {
                                                    if (nCurrSegon < nSegonsBloc2Aux)
                                                        SegonsFiAux = nCurrSegon;
                                                    else
                                                        SegonsFiAux = nSegonsBloc2Aux;
                                                }
                                            }
                                            else if ((nSegonsBloc2Aux != 0) && (nSegonsBloc1Aux != 0))//controlem segonsLimit vs. segonsBloc1
                                            {
                                                if ((SegonsIniElement > nSegonsBloc1Aux) && bHiHaItemAlBloc2)//el item es troba al bloc2 --> calen vàries parts lliures
                                                {
                                                    if (nCurrSegon < nSegonsLimit)//encara estem dins segonsLimit
                                                    {
                                                        SegonsFiAux = nSegonsLimitAux;

                                                        //part no schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = false;
                                                        cela.FreeSpace = true;
                                                        cela.EnableSchedule = false;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)//sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;

                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 3, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);
                                                        nCurrSegon = SegonsFiAux;
                                                    }

                                                    if (nCurrSegon < nSegonsBloc1Aux)
                                                    {
                                                        SegonsFiAux = nSegonsBloc1Aux;

                                                        //part schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = true;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)
                                                        {
                                                            //sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;
                                                            cela.SecondStartHmax = cela.SecondStart;
                                                            cela.SecondEndHmax = cela.SecondEnd;
                                                        }

                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 4, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);

                                                        nCurrSegon = SegonsFiAux;
                                                    }

                                                    if (nSegonsBloc2Aux > nSegonsLimitAux)
                                                        SegonsFiAux = nSegonsBloc2Aux;
                                                    else
                                                        SegonsFiAux = nSegonsLimitAux;
                                                }
                                                else//item es troba dins bloc1
                                                {
                                                    bHiHaItemAlBloc2 = true;

                                                    if (nCurrSegon < nSegonsLimit)//encara estem dins segonsLimit
                                                    {
                                                        SegonsFiAux = nSegonsLimitAux;

                                                        //part no schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = false;
                                                        cela.FreeSpace = true;
                                                        cela.EnableSchedule = false;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)//sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;

                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 5, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);

                                                        nCurrSegon = SegonsFiAux;
                                                    }
                                                    int nsegHoraFiBloc1 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                                                    if ((nSegonsLimit < nsegHoraFiBloc1) && (nCurrSegon != nsegHoraFiBloc1))
                                                    {
                                                        //part no schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = true;
                                                        cela.FreeSpace = true;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)
                                                        {
                                                            //sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;
                                                            cela.SecondStartHmax = cela.SecondStart;
                                                            cela.SecondEndHmax = cela.SecondEnd;
                                                        }

                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 1000, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);

                                                        nCurrSegon = SegonsFiAux;
                                                    }

                                                    if (nCurrSegon < SegonsIniElement)
                                                    {
                                                        SegonsFiAux = SegonsIniElement;

                                                        //part schedulable
                                                        cela = new CPintaTaula();
                                                        cela.EnableSchedule = true;
                                                        cela.SecondStart = nCurrSegon;
                                                        cela.SecondEnd = SegonsFiAux;
                                                        cela.SelectedSpace = false;
                                                        if (strItemAfegit == nHighlightedCell)
                                                        {
                                                            //sel·leccionat espai lliure
                                                            cela.SelectedSpace = true;
                                                            cela.SecondStartHmax = cela.SecondStart;
                                                            cela.SecondEndHmax = cela.SecondEnd;
                                                        }
                                                        nItemAfegit++;
                                                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 6, cela.SecondStart, cela.SecondEnd));
                                                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                        aItemsPintaTaula.Add(cela);

                                                        nCurrSegon = SegonsFiAux;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                            }
                                        }
                                        else
                                        {
                                            SegonsFiAux = nSegonsLimit;
                                        }

                                        if (nCurrSegon < SegonsFiAux)//només pinta cela si SecondStart i SecondEnd són diferents
                                        {
                                            cela = new CPintaTaula();
                                            cela.FreeSpace = true;
                                            cela.EnableSchedule = false;
                                            cela.SecondStart = nCurrSegon;
                                            cela.SecondEnd = SegonsFiAux;
                                            cela.SelectedSpace = false;
                                            if (strItemAfegit == nHighlightedCell)
                                            {
                                                //sel·leccionat espai lliure
                                                cela.SelectedSpace = true;
                                                cela.SecondStartHmax = cela.SecondStart;
                                                cela.SecondEndHmax = cela.SecondEnd;
                                                if (SegonsFiAux == nSegonsBloc2Aux)
                                                {
                                                    m_BlocNoSchedulableSelecc = true;
                                                }
                                            }

                                            nItemAfegit++;
                                            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                            nCurrSegon = SegonsFiAux;
                                            //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 7, cela.SecondStart, cela.SecondEnd));
                                            cela.StartTime = data.AddSeconds(cela.SecondStart);
                                            cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                            aItemsPintaTaula.Add(cela);
                                        }

                                        //part schedulable
                                        if (SegonsFiAux < SegonsIniElement)//només entra si temps inici i temps final de l'espai no són iguals!!
                                        {
                                            cela = new CPintaTaula();
                                            cela.EnableSchedule = true;
                                            cela.SecondStart = nCurrSegon;
                                            cela.SecondEnd = SegonsIniElement;
                                            cela.SelectedSpace = false;
                                            if (strItemAfegit == nHighlightedCell)
                                            {
                                                //sel·leccionat espai lliure
                                                cela.SelectedSpace = true;
                                                cela.SecondStartHmax = cela.SecondStart;
                                                cela.SecondEndHmax = cela.SecondEnd;
                                                //--- The Selected Scheduled Item
                                                //this.ScheduleToModify = itBuit;
                                            }

                                            nItemAfegit++;
                                            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                            //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 8, cela.SecondStart, cela.SecondEnd));
                                            cela.StartTime = data.AddSeconds(cela.SecondStart);
                                            cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                            aItemsPintaTaula.Add(cela);
                                        }
                                    }
                                    else
                                    {
                                        if (bTimeInterval2Blocs && (SegonsIniElement > BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)))//el item es troba al bloc2 --> cal pintar el interval no schedulable
                                        {
                                            if (nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0))
                                            {//NO hem pintat fins al bloc2
                                                //part schedulable
                                                cela = new CPintaTaula();
                                                cela.EnableSchedule = true;
                                                cela.FreeSpace = true;
                                                cela.SecondStart = nCurrSegon;
                                                cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                                                cela.SelectedSpace = false;
                                                if (strItemAfegit == nHighlightedCell)
                                                {
                                                    //sel·leccionat espai lliure
                                                    cela.SelectedSpace = true;
                                                    //cela.SecondStartHmax = cela.SecondStart;
                                                    cela.SecondStartHmax = BLC.DateTimeHelper.CalculaSegonsStartHmax_EspaiLliure(bSegonsLimit, cela.SecondStart, nSegonsLimit, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                                                    cela.SecondEndHmax = cela.SecondEnd;
                                                }

                                                nItemAfegit++;
                                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 9, cela.SecondStart, cela.SecondEnd));
                                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                aItemsPintaTaula.Add(cela);

                                                nCurrSegon = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);

                                                //part no schedulable
                                                cela = new CPintaTaula();
                                                cela.FreeSpace = true;
                                                cela.EnableSchedule = false;
                                                cela.SecondStart = nCurrSegon;
                                                cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                                cela.SelectedSpace = false;
                                                if (strItemAfegit == nHighlightedCell)
                                                {
                                                    //sel·leccionat espai lliure
                                                    cela.SelectedSpace = true;
                                                    m_BlocNoSchedulableSelecc = true;
                                                }
                                                nItemAfegit++;
                                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 10, cela.SecondStart, cela.SecondEnd));
                                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                                aItemsPintaTaula.Add(cela);

                                                nCurrSegon = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                            }

                                            //part schedulable
                                            cela = new CPintaTaula();
                                            cela.EnableSchedule = true;
                                            cela.FreeSpace = true;
                                            cela.SecondStart = nCurrSegon;
                                            cela.SecondEnd = SegonsIniElement;
                                            cela.SelectedSpace = false;
                                            if (strItemAfegit == nHighlightedCell)
                                            {
                                                //sel·leccionat espai lliure
                                                cela.SelectedSpace = true;
                                                cela.SecondStartHmax = cela.SecondStart;
                                                cela.SecondEndHmax = cela.SecondEnd;
                                            }
                                            nItemAfegit++;
                                            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                            //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 11, cela.SecondStart, cela.SecondEnd));
                                            cela.StartTime = data.AddSeconds(cela.SecondStart);
                                            cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                            aItemsPintaTaula.Add(cela);
                                        }
                                        else
                                        {
                                            //Tot schedulable: un sol item per tot l'espai buit
                                            cela = new CPintaTaula();
                                            cela.EnableSchedule = true;
                                            cela.SecondStart = nCurrSegon;
                                            cela.SecondEnd = SegonsIniElement;
                                            cela.SelectedSpace = false;
                                            if (strItemAfegit == nHighlightedCell)
                                            {
                                                //sel·leccionat espai lliure
                                                cela.SelectedSpace = true;
                                                //cela.SecondStartHmax = cela.SecondStart;
                                                cela.SecondStartHmax = BLC.DateTimeHelper.CalculaSegonsStartHmax_EspaiLliure(bSegonsLimit, cela.SecondStart, nSegonsLimit, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                                                cela.SecondEndHmax = cela.SecondEnd;
                                                //this.ScheduleToModify = itBuit;
                                            }

                                            nItemAfegit++;
                                            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                            //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 12, cela.SecondStart, cela.SecondEnd));
                                            cela.StartTime = data.AddSeconds(cela.SecondStart);
                                            cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                            aItemsPintaTaula.Add(cela);
                                        }
                                    }

                                    nCurrSegon = SegonsIniElement;

                                }
                            }
                            //afegir espai ocupat
                            cela = new CPintaTaula();
                            SegonsIniElementAmbMarge = SegonsIniElement + SEGONS_5_MINUTS;//marge de 5 minuts per poder modificar item 
                            if (data > dataHoraLimit.Date)
                            {
                                if ((it.Estimated_Start.Value.Date < dataHoraLimit.Date) || ((it.Estimated_Start.Value.Date == dataHoraLimit.Date) && (BLC.DateTimeHelper.ConvertHoursToSeconds(it.Estimated_Start.Value.Hour, it.Estimated_Start.Value.Minute) < nSegonsLimit)))
                                    cela.EnableSchedule = false;
                                else
                                    cela.EnableSchedule = true;
                            }
                            else if ((data == dataHoraLimit.Date) && (SegonsIniElementAmbMarge >= nSegonsLimit))
                                cela.EnableSchedule = true;
                            else
                                cela.EnableSchedule = false;

                            cela.FreeSpace = false;
                            cela.SecondStart = SegonsIniElement;
                            cela.SecondEnd = SegonsFiElement;

                            cela.IdSchedule = it.IdSchedule;
                            cela.ContentID = (it.ContentID != null ? it.ContentID.Value : -1);
                            cela.Type = it.mebs_ingesta.Type.Value;
                            cela.Code_Package = it.mebs_ingesta.Code_Package;
                            cela.EventID = it.mebs_ingesta.EventId;
                            cela.Title = it.mebs_ingesta.Title;
                            cela.IdIngesta = it.IdIngesta;
                            cela.PackageStatus = it.Status.Value;
                            cela.IsExpired = it.mebs_ingesta.IsExpired;
                            cela.Content_StartTime = it.Estimated_Start.Value;
                            cela.Content_EndTime = it.Estimated_Stop.Value;
                            cela.RedundancyFileSize = GetFileSize(it.mebs_ingesta);
                            cela.Duration = GetDuration(it.mebs_ingesta);
                            cela.Immortality_time = (it.mebs_ingesta.Immortality_time != null ? it.mebs_ingesta.Immortality_time.Value : DateTime.MinValue);
                            cela.Expiration_time = (it.mebs_ingesta.Expiration_time != null ? it.mebs_ingesta.Expiration_time.Value : DateTime.MinValue);

                            if (strItemAfegit == nHighlightedCell)
                            {
                                //sel·leccionat element
                                cela.SelectedSpace = true;
                                cela.ContentID = it.ContentID.Value;
                                this.ScheduleToModify = cela;
                                this.SelectedIngesta = null;

                                if (cela.EnableSchedule == true)
                                {
                                    if (i <= 0)
                                    {
                                        if ((dataHoraLimit.Date == data) && (nSegonsLimit > SegonsIniTimeInterval))
                                        {
                                            if (bTimeInterval2Blocs)
                                            {
                                                if ((cela.SecondStart >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0)) && (cela.SecondStart < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)))//estem al bloc 1
                                                {
                                                    if (nSegonsLimit < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0))
                                                    {
                                                        cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0);
                                                    }
                                                    else
                                                    {
                                                        cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);
                                                    }
                                                }
                                                else if ((cela.SecondStart >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)) && (cela.SecondStart <= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc2, 0)))//estem al bloc 2
                                                {
                                                    if (nSegonsLimit < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0))
                                                    {
                                                        cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                                    }
                                                    else
                                                    {
                                                        cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);
                                            }
                                        }
                                        else
                                        {
                                            //cela.SecondStartHmax = SegonsIniTimeInterval; Voir le Type PVOD Or Advertisement
                                            arrayScheduledAux = new List<mebs_schedule>();
                                            nElementsAux = 0;
                                            try
                                            {
                                                arrayScheduledAux = _context.Execute<mebs_schedule>(new Uri(string.Format(Config.GetListScheduleByDate, BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(data)), UriKind.Relative)).ToList();
                                                if (arrayScheduledAux != null)
                                                    nElementsAux = arrayScheduledAux.Count;
                                            }
                                            catch (Exception ex)
                                            {
                                                if (ex.InnerException is DataServiceClientException)
                                                {
                                                    // Parse the DataServieClientException
                                                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                                                    // Display the DataServiceClientException message
                                                    if (innerException != null)
                                                    {
                                                        LogHelper.logger.Warn(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0} - {1}", innerException.Code, innerException.Message));
                                                    }
                                                    else
                                                    {
                                                        LogHelper.logger.Error(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0}", ex.InnerException.Message));
                                                    }
                                                }
                                                else
                                                {
                                                    LogHelper.logger.Error(string.Format("TimeTableControl : PreparaDadesPintaTaula : {0}", ex.Message));
                                                }
                                            }
                                            if (nElementsAux > 1)
                                                itAux = (mebs_schedule)arrayScheduledAux[nElementsAux - 2];//agafem l'últim item del dia anterior (pel cas si un item està schedulat entre 2 dies)
                                            else
                                                itAux = new mebs_schedule();

                                            cela.SecondStartHmax = CalculaSegonsStartHmax_EspaiOcupat(bSegonsLimit, cela.SecondStart, nSegonsLimit, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2, it, itAux);
                                        }
                                    }
                                    else
                                    {
                                        itAux = (mebs_schedule)arrayScheduled[i - 1];
                                        if (bTimeInterval2Blocs && (cela.SecondStart >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)) && (cela.SecondStart <= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc2, 0)))//TimeInterval de 2 blocs i estem al bloc2
                                        {
                                            if (BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Stop.Value.Hour, itAux.Estimated_Stop.Value.Minute) < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0))
                                                cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                            else
                                                cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Stop.Value.Hour, itAux.Estimated_Stop.Value.Minute);
                                        }
                                        else//resta de casos
                                        {
                                            if ((dataHoraLimit.Date == data) && (itAux.Estimated_Stop.Value < dataHoraLimit))
                                                cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);
                                            else
                                                cela.SecondStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Stop.Value.Hour, itAux.Estimated_Stop.Value.Minute);
                                        }
                                    }

                                    if (i >= nElements - 1)
                                    {
                                        //cela.SecondEndHmax = SegonsFiTimeInterval; Voir le Type PVOD Or Advertisement
                                        arrayScheduledAux = new List<mebs_schedule>();
                                        try
                                        {
                                            arrayScheduledAux = _context.Execute<mebs_schedule>(new Uri(string.Format(Config.GetListScheduleByDate, BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(data)), UriKind.Relative)).ToList();
                                            if (arrayScheduledAux != null)
                                                nElementsAux = arrayScheduledAux.Count;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (ex.InnerException is DataServiceClientException)
                                            {
                                                // Parse the DataServieClientException
                                                BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                                                // Display the DataServiceClientException message
                                                if (innerException != null)
                                                {
                                                    LogHelper.logger.Warn(string.Format("MyEventDetailsControl : PreparaDadesPintaTaula : {0} - {1}", innerException.Code, innerException.Message));
                                                }
                                                else
                                                {
                                                    LogHelper.logger.Error(string.Format("MyEventDetailsControl : PreparaDadesPintaTaula : {0}", ex.InnerException.Message));
                                                }
                                            }
                                            else
                                            {
                                                LogHelper.logger.Error(string.Format("MyEventDetailsControl : PreparaDadesPintaTaula : {0}", ex.Message));
                                            }
                                        }
                                        if (nElementsAux > 0)
                                            itAux = (mebs_schedule)arrayScheduledAux[nElementsAux - 1];//agafem l'últim item del dia anterior
                                        else
                                            itAux = new mebs_schedule();

                                        cela.SecondEndHmax = CalculaSegonsEndHmax_EspaiOcupat(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2, it, itAux);
                                    }
                                    else
                                    {
                                        itAux = (mebs_schedule)arrayScheduled[i + 1];
                                        if (bTimeInterval2Blocs && (cela.SecondStart >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0)) && (cela.SecondStart < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)))//TimeInterval de 2 blocs i estem al bloc1
                                        {
                                            if (BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Start.Value.Hour, itAux.Estimated_Start.Value.Minute) > BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0))
                                                cela.SecondEndHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                                            else
                                                cela.SecondEndHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Start.Value.Hour, itAux.Estimated_Start.Value.Minute);
                                        }
                                        else//resta de casos
                                        {
                                            cela.SecondEndHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Start.Value.Hour, itAux.Estimated_Start.Value.Minute);
                                        }
                                    }
                                }
                                else
                                {
                                    cela.SecondStartHmax = cela.SecondStart;
                                    cela.SecondEndHmax = cela.SecondEnd;
                                }
                            }
                            else
                            {
                                cela.SelectedSpace = false;
                            }
                            nItemAfegit++;
                            strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                            //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 13, cela.SecondStart, cela.SecondEnd));
                            cela.StartTime = data.AddSeconds(cela.SecondStart);
                            cela.EndTime = data.AddSeconds(cela.SecondEnd);
                            aItemsPintaTaula.Add(cela);

                            nCurrSegon = SegonsFiElement;

                            //--- if the schedule is dummy, desabled the schedule:
                            if (cela.Type == Convert.ToInt32(BLC.MediaType.DUMMY_SCHEDULE))
                            {
                                cela.EnableSchedule = false;
                                cela.FreeSpace = true;
                            }

                        }
                    }


                }//---- END For
                //afegir espai buit fins al final
                SegonsFiTimeInterval = BLC.DateTimeHelper.ConvertHoursToSeconds(m_nHoraFiTimeInterval, 0);
                if (nCurrSegon < SegonsFiTimeInterval)
                {
                    cela = new CPintaTaula();
                    cela.FreeSpace = true;
                    bTimeInterval2Blocs = BLC.DateTimeHelper.GetSiTimeInterval2Blocs(out nHoraIniBloc1, out nHoraIniBloc2, out nHoraFiBloc1, out nHoraFiBloc2, m_strTimeInterval);
                    cela.EnableSchedule = false;
                    if ((data > dataHoraLimit.Date) || ((data == dataHoraLimit.Date) && ((SegonsFiTimeInterval > nSegonsLimit)) || (bTimeInterval2Blocs)))
                    {
                        cela.EnableSchedule = true;
                    }

                    if (cela.EnableSchedule == false)
                    {
                        //un sol item per tot l'espai buit
                        cela.SecondStart = nCurrSegon;
                        cela.SecondEnd = SegonsFiTimeInterval;
                        cela.SelectedSpace = false;
                        if (strItemAfegit == nHighlightedCell)
                        {
                            //sel·leccionat espai lliure
                            cela.SelectedSpace = true;
                            //Verifier ces valeurs
                            cela.SecondStartHmax = cela.SecondStart;
                            cela.SecondEndHmax = cela.SecondEnd;
                        }

                        nItemAfegit++;
                        strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                        //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 14, cela.SecondStart, cela.SecondEnd));
                        cela.StartTime = data.AddSeconds(cela.SecondStart);
                        cela.EndTime = data.AddSeconds(cela.SecondEnd);
                        aItemsPintaTaula.Add(cela);
                        nCurrSegon = SegonsFiTimeInterval;
                    }
                    else
                    {
                        if (bTimeInterval2Blocs)
                        {
                            if ((data == dataHoraLimit.Date) && (nCurrSegon < nSegonsLimit))
                            {
                                //part no schedulable
                                cela.EnableSchedule = false;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = nSegonsLimit;
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)//sel·leccionat espai lliure
                                {
                                    cela.SelectedSpace = true;
                                }
                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 15, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);

                                nCurrSegon = nSegonsLimit;
                            }

                            if ((nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)) && (nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)))//nCurrentSegons es troba dins el bloc1 del TimeInterval
                            {
                                //part schedulable
                                cela = new CPintaTaula();
                                cela.FreeSpace = true;
                                cela.EnableSchedule = true;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)
                                {
                                    //sel·leccionat espai lliure
                                    cela.SelectedSpace = true;
                                    cela.SecondStartHmax = cela.SecondStart;
                                    cela.SecondEndHmax = cela.SecondEnd;
                                    //this.ScheduleToModify = itBuit;
                                }

                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 16, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);

                                nCurrSegon = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
                            }
                            if ((nCurrSegon >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)) && (nCurrSegon < BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)))//nCurrentSegons es troba entre Fibloc1 i IniBloc2 del TimeInterval
                            {
                                //part no schedulable
                                cela = new CPintaTaula();
                                cela.EnableSchedule = false;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell) //sel·leccionat espai lliure
                                {
                                    m_BlocNoSchedulableSelecc = true;
                                    cela.SelectedSpace = true;
                                }

                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 17, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);

                                nCurrSegon = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
                            }
                            if ((nCurrSegon > BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0)) && (nCurrSegon >= BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0)))//nCurrentSegons es troba dins el Bloc2 del TimeInterval
                            {
                                //part schedulable: un sol item per tot l'espai buit
                                cela = new CPintaTaula();
                                cela.FreeSpace = true;
                                cela.EnableSchedule = true;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = SegonsFiTimeInterval;
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)
                                {
                                    //sel·leccionat espai lliure
                                    cela.SelectedSpace = true;
                                    cela.SecondStartHmax = cela.SecondStart;
                                    cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                                    //this.ScheduleToModify = itBuit;
                                }
                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 18, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);
                            }
                        }
                        else//TimeInterval de 1 bloc
                        {
                            if ((data == dataHoraLimit.Date) && (nCurrSegon < nSegonsLimit))
                            {
                                //separem espai en dos items: 
                                //part no schedulable
                                cela.EnableSchedule = false;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = nSegonsLimit;
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)  //sel·leccionat espai lliure
                                {
                                    cela.SelectedSpace = true;
                                }
                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 19, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);

                                //part schedulable
                                cela = new CPintaTaula();
                                cela.FreeSpace = true;
                                cela.EnableSchedule = true;
                                cela.SecondStart = nSegonsLimit;
                                cela.SecondEnd = SegonsFiTimeInterval;
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)
                                {
                                    //sel·leccionat espai lliure
                                    cela.SelectedSpace = true;
                                    cela.SecondStartHmax = cela.SecondStart;
                                    cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                                    //this.ScheduleToModify = itBuit;
                                }

                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 20, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);
                            }
                            else
                            {
                                //Tot schedulable: un sol item per tot l'espai buit
                                cela = new CPintaTaula();
                                cela.EnableSchedule = true;
                                cela.SecondStart = nCurrSegon;
                                cela.SecondEnd = SegonsFiTimeInterval;
                                cela.SelectedSpace = false;
                                if (strItemAfegit == nHighlightedCell)
                                {
                                    //sel·leccionat espai lliure
                                    cela.SelectedSpace = true;
                                    cela.SecondStartHmax = cela.SecondStart;
                                    //cela.SecondEndHmax = cela.SecondEnd;
                                    cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                                    //this.ScheduleToModify = itBuit;
                                }
                                nItemAfegit++;
                                strItemAfegit = string.Format("{0}_{1}", data.Day, nItemAfegit);
                                //Log.Write(string.Format("Item : {0} -> Ini : {1}- Fi : {2}", 21, cela.SecondStart, cela.SecondEnd));
                                cela.StartTime = data.AddSeconds(cela.SecondStart);
                                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                                aItemsPintaTaula.Add(cela);
                            }
                        }

                        nCurrSegon = SegonsFiTimeInterval;
                    }
                }
            }
            else
            {
                //no hi ha elements a la taula --> prepara taula buida
                PreparaDadesPintaTaulaBuida(out aItemsPintaTaula, data);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected List<CPintaTaula> getItemsFromDBServer(DateTime data)
        {
            List<CPintaTaula> aItemsPintaTaula = new List<CPintaTaula>();
            PreparaDadesPintaTaula(out aItemsPintaTaula, data);
            return aItemsPintaTaula;
        }
        //----- Step 24 : OK
        /// <summary>
        /// Prepare the list of Cellule to be designaded in the TimeTable if there is no Data in dataBase
        /// </summary>
        /// <param name="aItemsPintaTaula"></param>
        /// <param name="data"></param>
        private void PreparaDadesPintaTaulaBuida(out List<CPintaTaula> aItemsPintaTaula, DateTime data)
        {
            string nHighlightedCell = string.Empty;
            DateTime dataHoraLimit;
            int nSegonsLimit;
            int SegonsFiTimeInterval;
            int SegonsIniTimeInterval;
            CPintaTaula cela;
            int nCurrentHora = 0;
            int nCelaId = 0;
            string strCelaId = string.Empty;
            bool bTimeInterval2Blocs = false;
            int nHoraIniBloc1 = 0;
            int nHoraIniBloc2 = 0;
            int nHoraFiBloc1 = 0;
            int nHoraFiBloc2 = 0;

            strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);
            nHighlightedCell = this.CellId;
            aItemsPintaTaula = new List<CPintaTaula>();

            //try
            //{
            //dia i hora limit permesos per schedular
            dataHoraLimit = DateTime.Now.ToUniversalTime().AddMinutes(BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetSettingValue(BLC.DefaultValue.DC_Time_Frame), BLC.DefaultValue.Default_DC_Time_Frame));  //DateTime.Now.AddHours((double)CMySQLMSPDataBase.GetHoursTimeFrame());
            string strTime = string.Format("{0}:{1}:00", dataHoraLimit.Hour, dataHoraLimit.Minute);
            nSegonsLimit = (int)BLC.DateTimeHelper.HMS2Seconds(strTime);   //BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, dataHoraLimit.Minute);

            //Check if there is two blocks
            bTimeInterval2Blocs = BLC.DateTimeHelper.GetSiTimeInterval2Blocs(out nHoraIniBloc1, out nHoraIniBloc2, out nHoraFiBloc1, out nHoraFiBloc2, m_strTimeInterval);

            SegonsFiTimeInterval = BLC.DateTimeHelper.ConvertHoursToSeconds(m_nHoraFiTimeInterval, 0);
            SegonsIniTimeInterval = BLC.DateTimeHelper.ConvertHoursToSeconds(m_nHoraIniTimeInterval, 0);
            if ((data < dataHoraLimit.Date) || ((data == dataHoraLimit.Date) && (nSegonsLimit > SegonsFiTimeInterval)))//únic item per tot el dia (no schedulable)
            {
                cela = new CPintaTaula();
                cela.FreeSpace = true;
                cela.EnableSchedule = false;

                cela.SecondStart = SegonsIniTimeInterval;
                cela.SecondEnd = SegonsFiTimeInterval;

                strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);
                if (nHighlightedCell == strCelaId)
                {
                    cela.SelectedSpace = true;
                    cela.SecondStartHmax = cela.SecondStart;
                    cela.SecondEndHmax = cela.SecondEnd;
                    //Session.Remove("CellId");
                }
                else
                {
                    cela.SelectedSpace = false;
                }
                cela.StartTime = data.AddSeconds(cela.SecondStart);
                cela.EndTime = data.AddSeconds(cela.SecondEnd);
                aItemsPintaTaula.Add(cela);
            }
            else if (data == dataHoraLimit.Date)//varis items per tot el dia(schedulable i no schedulable)
            {
                nCurrentHora = m_nHoraIniTimeInterval;
                if (nSegonsLimit > SegonsIniTimeInterval)
                {
                    //item no schedulable
                    cela = new CPintaTaula();
                    cela.FreeSpace = true;

                    cela.SecondStart = SegonsIniTimeInterval;
                    cela.SecondEnd = nSegonsLimit;
                    cela.EnableSchedule = false;

                    if (nHighlightedCell == strCelaId)
                    {
                        cela.SelectedSpace = true;
                        cela.SecondStartHmax = cela.SecondStart;
                        cela.SecondEndHmax = cela.SecondEnd;
                        //Session.Remove("CellId");
                    }
                    else
                    {
                        cela.SelectedSpace = false;
                    }

                    cela.StartTime = data.AddSeconds(cela.SecondStart);
                    cela.EndTime = data.AddSeconds(cela.SecondEnd);
                    aItemsPintaTaula.Add(cela);
                    nCelaId++;
                    strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);

                    //item schedulable (fracció d'una hora)
                    cela = new CPintaTaula();
                    cela.FreeSpace = true;

                    cela.SecondStart = nSegonsLimit;
                    if (dataHoraLimit.Hour == 23)
                    {
                        cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.Hour, 59) + 60;//per arribar a les 24h == 0h del datetime 
                    }
                    else
                    {
                        cela.SecondEnd = BLC.DateTimeHelper.ConvertHoursToSeconds(dataHoraLimit.AddHours(1).Hour, 0);
                    }
                    cela.EnableSchedule = true;

                    if (nHighlightedCell == strCelaId)
                    {
                        cela.SelectedSpace = true;
                        cela.SecondStartHmax = cela.SecondStart;
                        //cela.SecondEndHmax = SegonsFiTimeInterval;
                        cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                        //Session.Remove("CellId");
                    }
                    else
                    {
                        cela.SelectedSpace = false;
                    }
                    cela.StartTime = data.AddSeconds(cela.SecondStart);
                    cela.EndTime = data.AddSeconds(cela.SecondEnd);
                    aItemsPintaTaula.Add(cela);
                    nCelaId++;
                    strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);

                    if (dataHoraLimit.Hour == 23)
                    {
                        nCurrentHora = 24;//== 0h del datetime
                    }
                    else
                    {
                        nCurrentHora = dataHoraLimit.AddHours(1).Hour;
                    }
                }

                for (int i = nCurrentHora; i < m_nHoraFiTimeInterval; i++)
                {
                    //la resta d'items schedulables
                    cela = new CPintaTaula();
                    cela.FreeSpace = true;

                    //hora seleccionada a la taula
                    cela.SecondStart = (60 * 60 * (i));
                    cela.SecondEnd = (60 * 60 * (i + 1));

                    cela.EnableSchedule = true;

                    if (nHighlightedCell == strCelaId)
                    {
                        cela.SelectedSpace = true;
                        cela.SecondStartHmax = cela.SecondStart;
                        //cela.SecondEndHmax = SegonsFiTimeInterval;
                        cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                        //Session.Remove("CellId");
                    }
                    else
                    {
                        cela.SelectedSpace = false;
                    }
                    cela.StartTime = data.AddSeconds(cela.SecondStart);
                    cela.EndTime = data.AddSeconds(cela.SecondEnd);
                    aItemsPintaTaula.Add(cela);
                    nCelaId++;
                    strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);
                }
            }
            else if (data > dataHoraLimit.Date)//dies futurs schedulables
            {
                cela = new CPintaTaula();
                for (int i = m_nHoraIniTimeInterval; i < m_nHoraFiTimeInterval; i++)
                {
                    //tots items schedulables
                    if (i > nHoraFiBloc1 && i < nHoraIniBloc2)
                    {
                        cela.EnableSchedule = false;
                    }
                    else
                    {
                        cela = new CPintaTaula();
                        //hora seleccionada a la taula
                        cela.SecondStart = (60 * 60 * (i));
                        cela.EnableSchedule = true;
                    }

                    cela.FreeSpace = true;
                    cela.SecondEnd = (60 * 60 * (i + 1));



                    if (nHighlightedCell == strCelaId)
                    {
                        cela.SelectedSpace = true;
                        cela.SecondStartHmax = cela.SecondStart;
                        //cela.SecondEndHmax = SegonsFiTimeInterval;
                        cela.SecondEndHmax = BLC.DateTimeHelper.CalculaSegonsEndHmax_EspaiLLiure(cela.SecondStart, bTimeInterval2Blocs, nHoraIniBloc1, nHoraIniBloc2, nHoraFiBloc1, nHoraFiBloc2);
                        //Session.Remove("CellId");
                    }
                    else
                    {
                        cela.SelectedSpace = false;
                    }
                    cela.StartTime = data.AddSeconds(cela.SecondStart);
                    cela.EndTime = data.AddSeconds(cela.SecondEnd);
                    aItemsPintaTaula.Add(cela);
                    nCelaId++;
                    strCelaId = string.Format("{0}_{1}", data.Day, nCelaId);
                }
            }
        }

        /// <summary>
        /// Check if the selected Content have a Enoght Time to be scheduled
        /// </summary>
        /// <param name="dIni"></param>
        /// <param name="dEnd"></param>
        /// <returns></returns>
        private int ComprovarSiCab(DateTime dIni, DateTime dEnd)
        {
            List<mebs_schedule> ListOfSchedule = new List<mebs_schedule>();
            int nHoraIniBloc1 = 0;
            int nHoraIniBloc2 = 0;
            int nHoraFiBloc1 = 0;
            int nHoraFiBloc2 = 0;
            DateTime dDataIniBloc1;
            DateTime dDataFiBloc1;
            DateTime dDataIniBloc2;
            DateTime dDataFiBloc2;

            int Resultat = 0;
            
            BLC.DateTimeHelper.CalculaHoraIniHoraFiTimeInterval(nBitsTimeInterval, out m_strTimeInterval, out m_nHoraIniTimeInterval, out m_nHoraFiTimeInterval);

            string[] strHores = m_strTimeInterval.Split('_');
            if (strHores.Length == 2)//hi ha un bloc d'hores
            {
                nHoraIniBloc1 = int.Parse(strHores[0]);
                nHoraFiBloc1 = int.Parse(strHores[1]);
            }
            else if (strHores.Length == 4)//hi ha 2 blocs d'hores
            {
                nHoraIniBloc1 = int.Parse(strHores[0]);
                nHoraFiBloc1 = int.Parse(strHores[1]);
                nHoraIniBloc2 = int.Parse(strHores[2]);
                nHoraFiBloc2 = int.Parse(strHores[3]);
            }

            dDataIniBloc1 = new DateTime(dIni.Year, dIni.Month, dIni.Day, nHoraIniBloc1, 0, 0);
            if (nHoraFiBloc1 == 24)
            {
                dDataFiBloc1 = new DateTime(dIni.AddDays(1).Year, dIni.AddDays(1).Month, dIni.AddDays(1).Day, 0, 0, 0);
            }
            else
            {
                dDataFiBloc1 = new DateTime(dIni.Year, dIni.Month, dIni.Day, nHoraFiBloc1, 0, 0);
            }
            if (strHores.Length == 4)//hi ha 2 blocs d'hores
            {
                dDataIniBloc2 = new DateTime(dIni.Year, dIni.Month, dIni.Day, nHoraIniBloc2, 0, 0);
                dDataFiBloc2 = new DateTime(dIni.AddDays(1).Year, dIni.AddDays(1).Month, dIni.AddDays(1).Day, nHoraFiBloc1, 0, 0);
            }
            else
            {
                dDataIniBloc2 = new DateTime();
                dDataFiBloc2 = new DateTime();
            }

            //si només hi ha un bloc (bloc1)
            if (nHoraIniBloc2 == nHoraFiBloc2)
            {
                if (!((nHoraIniBloc1 == 0) && (nHoraFiBloc1 == 24)))//entra sempre excepte el cas de poder schedular les 24 hores d'un dia.
                {
                    if (dIni < dDataIniBloc1)
                    {
                        //return false;
                        Resultat = 3;
                    }

                    if (dEnd.AddMinutes(1) > dDataFiBloc1)
                    {
                        //return false;
                        Resultat = 3;
                    }
                }
            }
            else//hi ha 2 blocs (bloc1 i bloc2)
            {
                if (dIni < dDataIniBloc1)//horaIni del item més petit que l'inici del bloc1
                {
                    //return false;
                    Resultat = 3;
                }
                if ((dIni > dDataIniBloc1) && (dIni < dDataFiBloc1))//si horaIni del item es troba dins el bloc1 d'hores
                {
                    if (dEnd.AddMinutes(1) > dDataFiBloc1)
                    {
                        //return false;
                        Resultat = 3;
                    }
                }
                else if ((dIni > dDataFiBloc1) && (dIni < dDataIniBloc2))//horaIni del item es troba fora de l'interval schedulable
                {
                    //return false;
                    Resultat = 3;
                }
                else if ((dIni > dDataIniBloc2) && (dIni < dDataFiBloc2))//horaIni del item es troba dins del bloc2 d'hores 
                {
                    if (dEnd.AddMinutes(1) > dDataFiBloc2)
                    {
                        //return false;
                        Resultat = 3;
                    }
                }
            }

            //cas solapament amb altres items de la taula (+ 1 minut de pico          
            try
            {
                ListOfSchedule = _context.Execute<mebs_schedule>(new Uri(string.Format(Config.GetListScheduleByDate, BLC.DateTimeHelper.ConvertDateTimeToEDMFormat(dIni)), UriKind.Relative)).ToList();
            }
            catch (Exception ex)
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Warn(string.Format("TimeTable : ComprovarSiCab : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("TimeTable : ComprovarSiCab : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("TimeTable : ComprovarSiCab : {0}", ex.Message));
                }
            }


            DateTime dtHora_Ini, dtHora_Fi;
            mebs_schedule DCItem;
            if (ListOfSchedule == null || ListOfSchedule.Count <= 0)
                return Resultat;

            foreach (mebs_schedule item in ListOfSchedule)
            {
                dtHora_Ini = item.Estimated_Start.Value;
                dtHora_Fi = item.Estimated_Stop.Value;
                DCItem = MEBSMAMHelper.GetSchedulePackageV2(item);
                if ((dIni < DCItem.Estimated_Start.Value) && (dEnd.AddMinutes(1) > DCItem.Estimated_Start.Value))
                {
                    Resultat = 1;
                    break;
                }
            }
            return Resultat;
        }

        //----- Step 26 : OK
        /// <summary>
        /// Calculate the Time Neded fro a Size compared the UserBitrate set in Configuration
        /// </summary>
        /// <param name="lTamany"></param>
        /// <param name="nBitrate"></param>
        /// <returns></returns>
        private int TempsEnviamentSeg(long lTamany, int nBitrate)
        {
            int nTemps = 0;

            double dTemps = 0;
            dTemps = (lTamany * 8 * 1.15) / nBitrate;
            nTemps = (int)dTemps;
            if (dTemps > (double)nTemps)
            {
                nTemps = nTemps + 1;
            }

            return nTemps;
        }

        //----- Step 27 : OK
        /// <summary>
        /// Description : Calculate the end of the occupied space
        /// </summary>
        /// <param name="nSegonsIni"></param>
        /// <param name="bTimeInterval2Blocs"></param>
        /// <param name="nHoraIniBloc1"></param>
        /// <param name="nHoraIniBloc2"></param>
        /// <param name="nHoraFiBloc1"></param>
        /// <param name="nHoraFiBloc2"></param>
        /// <param name="it"></param>
        /// <param name="itAux"></param>
        /// <returns></returns>
        private int CalculaSegonsEndHmax_EspaiOcupat(int nSegonsIni, bool bTimeInterval2Blocs, int nHoraIniBloc1, int nHoraIniBloc2, int nHoraFiBloc1, int nHoraFiBloc2, mebs_schedule it, mebs_schedule itAux)
        {
            //retorna nSegonsEndHmax de la cela
            int nSegonsEndHmax = 0;

            int nSegonsIniBloc1 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0);
            int nSegonsIniBloc2 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
            int nSegonsFiBloc1 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
            int nSegonsFiBloc2 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc2, 0);

            if (!bTimeInterval2Blocs)//Només hi ha un bloc de TimeInterval
            {
                return nSegonsFiBloc1;
            }
            else//hi ha 2 blocs de TimeInterval
            {
                if ((nSegonsIni >= nSegonsIniBloc2) && (nSegonsIni <= nSegonsFiBloc2))//estem a una cela del bloc2
                {
                    if (it.Estimated_Start.Value.Date != it.Estimated_Stop.Value.Date)
                    {
                        if (BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Start.Value.Hour, 0) < nSegonsFiBloc1)
                            nSegonsEndHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Start.Value.Hour, 0);
                        else
                            nSegonsEndHmax = nSegonsFiBloc1;
                    }
                    else
                    {
                        nSegonsEndHmax = nSegonsFiBloc2;
                    }
                }
                else if ((nSegonsIni >= nSegonsIniBloc1) && (nSegonsIni < nSegonsFiBloc1))//estem a una cela del bloc1
                    nSegonsEndHmax = nSegonsFiBloc1;
                else//altre cas: bloc no schedulable
                    nSegonsEndHmax = nSegonsFiBloc2;
            }

            return nSegonsEndHmax;
        }

        //----- Step 28 : OK
        /// <summary>
        /// Description :Calculate the start of the space occupied
        /// </summary>
        /// <param name="bSegonsLimit"></param>
        /// <param name="nSegonsIni"></param>
        /// <param name="nSegonsLimit"></param>
        /// <param name="bTimeInterval2Blocs"></param>
        /// <param name="nHoraIniBloc1"></param>
        /// <param name="nHoraIniBloc2"></param>
        /// <param name="nHoraFiBloc1"></param>
        /// <param name="nHoraFiBloc2"></param>
        /// <param name="it"></param>
        /// <param name="itAux"></param>
        /// <returns></returns>
        private int CalculaSegonsStartHmax_EspaiOcupat(bool bSegonsLimit, int nSegonsIni, int nSegonsLimit, bool bTimeInterval2Blocs, int nHoraIniBloc1, int nHoraIniBloc2, int nHoraFiBloc1, int nHoraFiBloc2, mebs_schedule it, mebs_schedule itAux)
        {
            //retorna nSegonsStartHmax de la cela
            int nSegonsStartHmax = 0;

            int nSegonsIniBloc1 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc1, 0);
            int nSegonsIniBloc2 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraIniBloc2, 0);
            int nSegonsFiBloc1 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc1, 0);
            int nSegonsFiBloc2 = BLC.DateTimeHelper.ConvertHoursToSeconds(nHoraFiBloc2, 0);

            if (!bTimeInterval2Blocs)//Només hi ha un bloc de TimeInterval
            {
                nSegonsStartHmax = nSegonsIniBloc1;
            }
            else//hi ha 2 blocs de TimeInterval
            {
                if ((nSegonsIni >= nSegonsIniBloc1) && (nSegonsIni < nSegonsFiBloc1))//estem a una cela del bloc1
                {
                    if (bSegonsLimit)
                    {
                        nSegonsStartHmax = nSegonsLimit;
                    }
                    else
                    {
                        if (it.Estimated_Start.Value.Date != it.Estimated_Stop.Value.Date)
                        {
                            if (BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Stop.Value.Hour, 0) > nSegonsIniBloc2)
                            {
                                nSegonsStartHmax = BLC.DateTimeHelper.ConvertHoursToSeconds(itAux.Estimated_Stop.Value.Hour, 0);
                            }
                            else
                            {
                                nSegonsStartHmax = nSegonsIniBloc2;
                            }
                        }
                        else
                        {
                            nSegonsStartHmax = nSegonsIniBloc1;
                        }
                    }
                }
                else if ((nSegonsIni >= nSegonsIniBloc2) && (nSegonsIni <= nSegonsFiBloc2))//estem a una cela del bloc2
                {
                    if (bSegonsLimit)
                    {
                        if (nSegonsLimit < nSegonsIniBloc2)
                        {
                            nSegonsStartHmax = nSegonsIniBloc2;
                        }
                        else
                        {
                            nSegonsStartHmax = nSegonsLimit;
                        }
                    }
                    else
                    {
                        nSegonsStartHmax = nSegonsIniBloc2;
                    }
                }
                else//altre cas: bloc no schedulable
                {
                    nSegonsStartHmax = nSegonsIniBloc1;
                }
            }

            return nSegonsStartHmax;
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Method (s) -.-.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// Replace the special Character '[' by '[[' and ']' by ']]'
        /// </summary>
        /// <param name="strInput"></param>
        /// <returns></returns>
        protected string ReplaceSpecialCharacter(string strInput)
        {
            return strInput.Replace("[", "[[").Replace("]", "]]");
        }

        /// <summary>
        /// Load the DivScrollBarPosition Files
        /// </summary>
        private void LoadJSandCSS()
        {
            string DivScrollBarPosition = "<script src='" + Page.ResolveUrl("Scripts/DivScrollBarPosition.js") + "' type='text/javascript'></script>";
            Page.ClientScript.RegisterClientScriptBlock(this.Page.GetType(), "js12643", DivScrollBarPosition, false);
        }

        /// <summary>
        /// Get the File Size after Redundancy
        /// </summary>
        /// <param name="_PackageId"></param>
        /// <param name="_PackageDetailsID"></param>
        /// <returns></returns>
        protected long GetFileSize(mebs_ingesta IngestaItem)
        {
            return (long)IngestaItem.MediaFileSizeAfterRedundancy;
        }
        
        /// <summary>
        /// Get Package Duration
        /// </summary>
        /// <param name="IngestaItem"></param>
        /// <returns></returns>
        protected int GetDuration(mebs_ingesta IngestaItem)
        {
            return BLC.CommonHelper.ConvertStringToInt(MEBSMAMHelper.GetIngestaDetails(IngestaItem, BLC.DefaultValue.DC_Duration).DetailsValue, 0);
        }

        /// <summary>
        /// Method to Enabled or Desabled manipulation Button
        /// </summary>
        protected void EnabledButton()
        {

            if (IsSelected)
            {
                if (IsContent)
                {
                    btnSchedule.Enabled = IsToSchedule;
                    btnEdit.Enabled = true;
                    //btnModify.Enabled = true;
                    btnParcourir.Enabled = false;
                }
                else
                {
                    btnSchedule.Enabled = IsToSchedule;
                    btnEdit.Enabled = false;
                    //btnModify.Enabled = false;
                    btnParcourir.Enabled = true;
                }
            }
            else
            {
                btnSchedule.Enabled = IsToSchedule;
                btnEdit.Enabled = false;
                //btnModify.Enabled = false;
                btnParcourir.Enabled = false;
            }
        }

        /// <summary>
        /// Add the Published Items to Database
        /// </summary>
        public bool PublishItem()
        {
            try
            {
                lock (lockPublishDC)
                {
                    //bool PackageScheduled = false;

                    //string strScheduleGUID = string.Empty;
                    //string strBonusGUID = string.Empty;
                    //string strTrailerGUID = string.Empty;
                    //string strGenreNotFound = string.Empty;

                    //List<Channel> ListDCChannel;
                    //mebs_schedule NewScheduleContent;
                    //mebs_schedule NewScheduleTrailer;
                    //mebs_schedule NewScheduleBonus;
                    //List<mebs_schedule> ListScheduleAdv;

                    //mebs_ingesta ObjIngesta;
                    //IngestaPackage ObjIngestaPackage;
                    //DateTime dtSchedule;

                    //---- Check If DC is Associated to a Cover (using the setting )

                    //Check If the DC Channel Is Configured (Has a DVBLocator)
                    //ListDCChannel = ChannelProvider.GetListOfChannel(MediaType.DATA_CHANNEL);
                    //if (ListDCChannel == null || ListDCChannel.Count < 1)
                    //{
                    //    IsContent = false;
                    //    IsSelected = false;
                    //    IsToSchedule = false;
                    //    Session["SelectedIngesta"] = null;
                    //    Session["IngestaBonus"] = null;
                    //    Session["IngestaTrailer"] = null;
                    //    ViewState["ADV_Timing"] = null;
                    //    Session["IngestaADV"] = null;
                    //    this.CellId = null;
                    //    ShowError(GetLocaleStringResource("MAM_TimeTable_NoDCChannel"));
                    //    return false;
                    //}
                    //if (ListDCChannel[0].DVBLocators == null || ListDCChannel[0].DVBLocators.Count < 1)
                    //{
                    //    IsContent = false;
                    //    IsSelected = false;
                    //    IsToSchedule = false;
                    //    Session["SelectedIngesta"] = null;
                    //    Session["IngestaBonus"] = null;
                    //    Session["IngestaTrailer"] = null;
                    //    ViewState["ADV_Timing"] = null;
                    //    Session["IngestaADV"] = null;
                    //    this.CellId = null;
                    //    ShowError(string.Format(GetLocaleStringResource("MAM_TimeTable_NoDVBTriplet"), ListDCChannel[0].LongName));
                    //    return false;
                    //}



                    //SelectedPintatula = (CPintaTaula)Session["SelectedCellule"];


                    //ObjIngesta = (mebs_ingesta)Session["SelectedIngesta"];
                    //if (ObjIngesta.IsEmptyCategory)
                    //{
                    //    IsContent = false;
                    //    IsSelected = false;
                    //    IsToSchedule = false;
                    //    Session["SelectedIngesta"] = null;
                    //    Session["IngestaBonus"] = null;
                    //    Session["IngestaTrailer"] = null;
                    //    ViewState["ADV_Timing"] = null;
                    //    Session["IngestaADV"] = null;
                    //    this.CellId = null;
                    //    ShowError(string.Format(GetLocaleStringResource("MAM_Events_NoGenre"), ObjIngesta.Turkish_Titol));
                    //    return false;
                    //}
                    //if (!ObjIngesta.IsPosterSelected)
                    //{
                    //    IsContent = false;
                    //    IsSelected = false;
                    //    IsToSchedule = false;
                    //    Session["SelectedIngesta"] = null;
                    //    Session["IngestaBonus"] = null;
                    //    Session["IngestaTrailer"] = null;
                    //    ViewState["ADV_Timing"] = null;
                    //    Session["IngestaADV"] = null;
                    //    this.CellId = null;
                    //    ShowError(string.Format(GetLocaleStringResource("MAM_DC_NoPosterAssociated"), ObjIngesta.Turkish_Titol));
                    //    return false;
                    //}

                    //-------------- Prepare the Schedule Item for Content TS 
                    DateTime dtSchedule = DateTime.Now.ToUniversalTime();
                    #region TS File
                    //string _inputData = string.Empty;
                    //_inputData = string.Format("{0}{1}{2}{3}", DateTime.Now.ToUniversalTime().ToString("MM/dd/yyyy hh:mm:ss"), CommonHelper.GenerateRandomDigitCode(20), ObjIngesta.AssetId_Title, Convert.ToInt32(MediaType.DATA_CHANNEL));
                    //string _code_Schedule = IOUtil.NewToken(_inputData);

                    //------- Get EventId using the AssetId_Title : Get the last 16 Numbers
                    //string _EventId = CommonHelper.GetAssetCode(ObjIngesta.AssetId_Title);

                    //DateTime Content_EstimatedStart = (DateTime)ViewState["Content_StartTime"];
                    //DateTime Content_EstimatedStop = (DateTime)ViewState["Content_EndTime"];

                    //--- Get the MovieDetails
                    //ObjIngestaMovie = IngestaMovieProvider.GetMovieByIdIngesta(ObjIngesta.IdIngesta);
                    //ObjIngestaPackage = IngestaPackageProvider.GetPackageByPackageIDV2(ObjIngesta.PackageId);


                    mebs_schedule NewScheduleContent = new mebs_schedule();
                    NewScheduleContent.IdIngesta = this.SelectedIngesta.IdIngesta;
                    NewScheduleContent.EventId = this.SelectedIngesta.EventID;
                    NewScheduleContent.Date_Schedule = dtSchedule;
                    NewScheduleContent.Exact_Start = DateTime.MinValue;
                    NewScheduleContent.Exact_Stop = DateTime.MinValue;
                    NewScheduleContent.Estimated_Start = this.SelectedIngesta.Content_StartTime;
                    NewScheduleContent.Estimated_Stop = this.SelectedIngesta.Content_EndTime;
                    NewScheduleContent.IsActive = true;
                    NewScheduleContent.Status = Convert.ToInt32(BLC.ScheduleStatus.PREPARED);
                    NewScheduleContent.Poster_Status = Convert.ToInt32(BLC.PosterStatus.PREPARED);
                    NewScheduleContent.Poster_DateSent = DateTime.MinValue;
                    NewScheduleContent.Poster_SentTries = 0;
                    NewScheduleContent.IsDeleted = Convert.ToInt32(BLC.ScheduleDeleteStatus.Default);
                    NewScheduleContent.Trigger_Type = Convert.ToSByte(BLC.TriggerType.Default);
                    NewScheduleContent.Dummy_Status = Convert.ToInt32(BLC.EpgDummyCommandStatus.PREPARED);
                    #endregion

                    this.SelectedCellule = null;
                    this.SelectedIngesta = null;
                    this.CellId = null;
                    //Session["NewStartTime"] = null;

                    _context.AddTomebs_schedule(NewScheduleContent);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                    //BLC.CommonHelper.ReloadCurrentPage();
                    ShowMessage("MAM_TimeTable_Pusblished_Message");

                    return true;
                }
            }
            catch (Exception ex)
            {
                ShowError("MAM_TimeTable_Pusblished_Error");
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        ShowError(innerException.Message);
                        LogHelper.logger.Warn(string.Format("TimeTable : PublishItem : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("TimeTable : PublishItem : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("TimeTable : PublishItem : {0}", ex.Message));
                }

                return false;
            }
        }

        private List<mebs_schedule> CreateDummySchedules(List<mebs_schedule> arrayScheduled)
        {
            List<mebs_schedule> TempScheduleList = new List<mebs_schedule>();
            if (arrayScheduled == null || arrayScheduled.Count <= 0)
                return TempScheduleList;

            foreach (mebs_schedule scheduleItem in arrayScheduled)
            {
                TempScheduleList.Add(scheduleItem);
                TempScheduleList.Add(MEBSMAMHelper.CreateDummySchedule(scheduleItem.Estimated_Stop.Value));
            }
            return TempScheduleList;
        }

        /// <summary>
        /// 
        /// </summary>
        private void SetDefaultValues()
        {
            MaskedEditValidatorSelectedDate.InvalidValueMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);
            MaskedEditValidatorSelectedDate.InvalidValueBlurredMessage = string.Format("Date is invalid : {0}", BLC.DefaultValue.DateTimeFormat);

            if (!string.IsNullOrEmpty(this.SelectedDate))
            {
                txtSelectedDate.Text = this.SelectedDate;
            }
            else
            {
                txtSelectedDate.Text = DateTime.Today.ToString("d");
            }
        }

        //----- Step 16 : OK
        /// <summary>
        /// Get and display the Scheduled Ingesta in ToolTip
        /// </summary>
        /// <param name="IdSchedule"></param>
        /// <returns></returns>
        protected string DisplayScheduleDetails(CPintaTaula Item)
        {
            string strDetails = string.Empty;

            CultureInfo ui = new CultureInfo("en-GB");
            if (Item != null)
            {
                strDetails = string.Format(@"header=[{0}]
                                           body=[<b>{1} </b> {2}<br><br>
                                                 <b>{3} </b> {4}<br><br>
                                                 <b>{5} </b> {6}<br><br> 
                                                 <b>{7} </b> {8}<br><br> 
                                                 <b>{9} </b> {10}<br><br>
                                                 <b>{11} </b> {12}<br>
                                                ] 
                                             windowlock=[on]", 
                                             ReplaceSpecialCharacter(Item.Title),
                                             "Content Status : ",
                                             Item.StatusName,
                                             "Content ID :",
                                             Item.ContentID,
                                             "Start Time : ",
                                             Item.StartTime.ToString(),
                                             "Stop Time : ",
                                             Item.EndTime.ToString(),
                                             "Expiration Time : ",
                                             Item.Expiration_time.Date.ToString("dddd, dd MMMM yyyy", ui),
                                             "Immortality Time : ",
                                             Item.Immortality_time.Date.ToString("dddd, dd MMMM yyyy", ui)
                                             );
            }
            return strDetails;
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Proprity (ies) -.-.-.-.-.-.-.-.-.-.-.-

        public string SelectedDate
        {

            get
            {
                if (Session["SelectedDate"] != null)
                {
                    return Session["SelectedDate"].ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Session["SelectedDate"] = txtSelectedDate.Text;
            }
        }

        public string CellId
        {
            get
            {
                if (Session["CellId"] != null)
                {
                    return Session["CellId"].ToString();//Convert.ToInt32()
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                Session["CellId"] = value;
            }
        }

        public CPintaTaula SelectedIngesta
        {
            get
            {
                if (Session["SelectedIngesta"] != null)
                {
                    return (CPintaTaula)Session["SelectedIngesta"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["SelectedIngesta"] = value;
            }
        }

        public CPintaTaula SelectedCellule
        {
            get
            {
                if (Session["SelectedCellule"] != null)
                {
                    return (CPintaTaula)Session["SelectedCellule"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["SelectedCellule"] = value;
            }
        }

        public CPintaTaula ScheduleToModify
        {
            get
            {
                if (Session["ScheduleToModify"] != null)
                {
                    return (CPintaTaula)Session["ScheduleToModify"];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Session["ScheduleToModify"] = value;
            }
        }

        #endregion
    }
}
