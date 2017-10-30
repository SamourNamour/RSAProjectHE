using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTV.MAM.WebApp.Helper;
using BLC = MTV.Library.Common;
using System.Drawing;

namespace MTV.MAM.WebApp
{
    public partial class IngestaInformation_aspx : System.Web.UI.Page
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.SelectedIngesta == null)
                return;

            CPintaTaula _SelectedItem = (CPintaTaula)this.SelectedIngesta;
            lblMovieSize.InnerText = BLC.CommonHelper.GetSize(_SelectedItem.RedundancyFileSize);
            lblStartTime.InnerText = BLC.DateTimeHelper.ConvertDateTimeToString(_SelectedItem.Content_StartTime);
            lblStopTime.InnerText = BLC.DateTimeHelper.ConvertDateTimeToString(_SelectedItem.Content_EndTime);
            lblTitle.InnerText = _SelectedItem.Title.ToUpper();
            lblTitleCodePackage.InnerText = _SelectedItem.Code_Package;
            lblContentID.InnerText = _SelectedItem.ContentID.ToString();
            TimeSpan TsDuration = BLC.DateTimeHelper.GenerateTimeSpan(_SelectedItem.Duration / 60);
            lblDurationMovie.InnerText = string.Format("{0}:{1}:{2}", BLC.DateTimeHelper.DisplayValueInDateFormat(TsDuration.Hours),
                                                                      BLC.DateTimeHelper.DisplayValueInDateFormat(TsDuration.Minutes), 
                                                                      BLC.DateTimeHelper.DisplayValueInDateFormat(TsDuration.Seconds));
            string strStatus = string.Empty;
            Color strstatusColor;
            MEBSMAMHelper.DisplayEventStatus(_SelectedItem.PackageStatus, _SelectedItem.IsExpired, out strStatus, out strstatusColor);
            lblStatusResult.InnerText = strStatus;
            lblStatusResult.Style.Add("color",ColorTranslator.ToHtml(strstatusColor));
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Proprity (ies) -.-.-.-.-.-.-.-.-.-.-.-
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
        }
        #endregion
    }
}