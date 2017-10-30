#region -.-.-.-.-.-.-.-.-.-.-.- Class : NameSpace(s) -.-.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data.Services.Client;
using System.ComponentModel;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using System.Data;

using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.MEBSCatalog;
#endregion

namespace MTV.MAM.WebApp.Helper
{
    public class MEBSMAMHelper
    {
        #region MEBSMAMHelper : Convert List des Schedule en DataTable
        /// <summary>
        /// this is the method I have been using
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable ConvertTo(List<mebs_schedule> list)
        {
            DataTable table = CreateTable();
            foreach (var item in list)
            {
                DataRow row = table.NewRow();

                row["IdSchedule"] = item.IdSchedule;
                row["IdIngesta"] = item.mebs_ingesta.IdIngesta;
                row["Estimated_Start"] = item.Estimated_Start;
                row["Estimated_stop"] = item.Estimated_Stop;
                row["Exact_Start"] = item.Exact_Start;
                row["Exact_Stop"] = item.Exact_Stop;
                row["ChannelName"] = item.mebs_ingesta.mebs_channel.LongName;
                row["Status"] = item.Status;
                row["Title"] = item.mebs_ingesta.Title;
                row["IsExpired"] = item.mebs_ingesta.IsExpired;

                table.Rows.Add(row);
            }

            return table;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateTable()
        {
            Type entityType = typeof(ScheduleInfoView);
            DataTable table = new DataTable(entityType.Name);
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(entityType);

            foreach (PropertyDescriptor prop in properties)
            {
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            }

            return table;
        }
        #endregion

        /// <summary>
        /// Apporter un object IngestaDetails par nom , sinon retourner un objet vide.
        /// </summary>
        /// <param name="Ingesta"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static mebs_ingestadetails GetIngestaDetails(mebs_ingesta Ingesta, string name)
        {
            IEnumerable<mebs_ingestadetails> lIngestaDetails = Ingesta.mebs_ingestadetails.Where(c => c.DetailsName == name);
            if (lIngestaDetails != null && lIngestaDetails.Count() > 0)
                return lIngestaDetails.First();

            return new mebs_ingestadetails();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="lPackageElements"></param>
        /// <param name="_type"></param>
        /// <returns></returns>
        public static mebs_ingesta GetIngestaByType(List<mebs_ingesta> lPackageElements, int _type)
        {
            return lPackageElements.Where(i => i.Type.Value == _type).FirstOrDefault();
        }

        /// <summary>
        /// Apporter la list des Item selectionner dans une ListBox.
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static List<ListItem> GetSelectedItems(ListBox lst)
        {
            return lst.Items.OfType<ListItem>().Where(i => i.Selected).ToList();
        }

        /// <summary>
        /// Vérifier si un event peut être publier.
        /// </summary>
        /// <param name="_startTime"> Estimated Start Time d'un Event</param>
        /// <returns>True si le event ne peut plus être publier || false.</returns>
        public static bool CheckIfIsOldEvent(DateTime? _startTime)
        {
            if (_startTime.Value <= DateTime.UtcNow)
                return true; // The Event is Old
            else
                return false;
        }

        /// <summary>
        /// Get The StartTime Of a Event
        /// </summary>
        /// <param name="ScheduleItem"></param>
        /// <returns></returns>
        public static DateTime GetStartTimeEvent(int EventStatus, DateTime Estimated_Start, DateTime Exact_Start)
        {
            if (EventStatus == Convert.ToInt32(BLC.ScheduleStatus.STARTED) ||
                EventStatus == Convert.ToInt32(BLC.ScheduleStatus.STOPPED) ||
                EventStatus == Convert.ToInt32(BLC.ScheduleStatus.MISSING_STOP))
                return Exact_Start;
            else
                return Estimated_Start;
        }

        /// <summary>
        /// Get The StopTime Of a Event
        /// </summary>
        /// <param name="ScheduleItem"></param>
        /// <returns></returns>
        public static DateTime GetStopTimeEvent(int EventStatus, DateTime Estimated_Stop, DateTime Exact_Stop)
        {
            if (EventStatus == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
                return Exact_Stop;
            else
                return Estimated_Stop;
        }

        /// <summary>
        /// Retourne le nom de l'image a afficher dans le controle scheduler dependemment du Status du Event.
        /// </summary>
        /// <param name="_startus"></param>
        /// <param name="_isExpired"></param>
        /// <returns></returns>
        public static string GetEventStatusImage(int _startus, bool _isExpired) 
        { 
            if (_isExpired)
                return BLC.DefaultValue.Status_expired;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.MISSING_START))
                return BLC.DefaultValue.Status_Missing_Start;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.MISSING_STOP))
                return BLC.DefaultValue.Status_Missing_Stop;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.FAILED_START))
                return BLC.DefaultValue.Status_Failed_Start;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.FAILED_STOP))
                return BLC.DefaultValue.Status_Failed_Stop;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.STARTED))
                return BLC.DefaultValue.Status_Started;

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
                return BLC.DefaultValue.Status_Stoped;

            if (_startus >= Convert.ToInt32(BLC.ScheduleStatus.LOCKED))
                return BLC.DefaultValue.Status_Locked;

            return BLC.DefaultValue.Status_Prepared;
        }

        /// <summary>
        /// Retourne le nom de l'image a afficher dans le controle scheduler dependemment du Status du Event.
        /// </summary>
        /// <param name="_startus"></param>
        /// <param name="_isExpired"></param>
        /// <returns></returns>
        public static void DisplayEventStatus(int _startus, bool _isExpired, out string StatusName , out System.Drawing.Color StatusColor)
        {
            StatusName = "PREPARED.";
            StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Prepared);

            if (_isExpired)
            {
                StatusName = "EXPIRED.";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_expired);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.MISSING_START))
            {
                StatusName = "TRIGGER_NOT_RECEIVED";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Missing_Start);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.MISSING_STOP))
            {
                StatusName = "MUSSING_STOP";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Missing_Stop);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.FAILED_START))
            {
                StatusName = "FAILED_START";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Failed_Start);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.FAILED_STOP))
            {
                StatusName = "FAILED_STOP";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Failed_Stop);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.STARTED))
            {
                StatusName = "STARTED";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Started);
            }

            if (_startus == Convert.ToInt32(BLC.ScheduleStatus.STOPPED))
            {
                StatusName = "ENDED";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Stoped);
            }

            if (_startus >= Convert.ToInt32(BLC.ScheduleStatus.LOCKED))
            {
                StatusName = "LOCKED";
                StatusColor = System.Drawing.ColorTranslator.FromHtml(BLC.DefaultValue.Status_Color_Locked);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static BLC.DataServiceErrorInfo ParseDataServiceClientException(string exception)
        {
            try
            {
                // namespace XML de DataServiceClientException
                XNamespace ns = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                XDocument doc = XDocument.Parse(exception);

                return new BLC.DataServiceErrorInfo
                {
                    Code = String.IsNullOrEmpty(
                        doc.Root.Element(ns + "code").Value) ? 400 :
                        int.Parse(doc.Root.Element(ns + "code").Value),
                    Message = doc.Root.Element(ns + "message").Value
                };
            }
            catch
            {
                //---- Log in log4Net
                //ShowError("Exceptions when parsing the DataServiceClientException: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ObjPackageSchedule"></param>
        /// <returns></returns>
        public static mebs_schedule GetSchedulePackageV2(mebs_schedule ObjPackageSchedule)
        {
            return ObjPackageSchedule;
        }

        /// <summary>
        /// Get Setting Value
        /// </summary>
        /// <param name="SettingName"></param>
        /// <returns></returns>
        public static string GetSettingValue(string SettingName)
        {
            try
            {
                mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
                mebs_settings setting = _context.Execute<mebs_settings>(new Uri(string.Format(Config.GetSettingsByName, SettingName), UriKind.Relative)).FirstOrDefault();
                if (setting != null)
                    return setting.SettingValue;

                return string.Empty;
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
                        LogHelper.logger.Error(string.Format("MEBSMAMHelper : GetSettingValue : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("MEBSMAMHelper : GetSettingValue : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("MEBSMAMHelper : GetSettingValue : {0}", ex.Message));
                }

                return string.Empty;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EstimatedStart"></param>
        /// <returns></returns>
        public static mebs_schedule CreateDummySchedule(DateTime EstimatedStart)
        {
            mebs_schedule _dummySchedule;
            int _dc_InterPackage = BLC.CommonHelper.ConvertStringToInt (GetSettingValue(BLC.DefaultValue.DC_Inter_Package_Time_Gap),BLC.DefaultValue.Default_DC_Inter_Package_Time_Gap);

            _dummySchedule = new mebs_schedule();
            _dummySchedule.ContentID = -1;
            _dummySchedule.Date_Schedule = DateTime.UtcNow;
            _dummySchedule.Estimated_Start = EstimatedStart;
            _dummySchedule.Estimated_Stop = EstimatedStart.AddMinutes(_dc_InterPackage);
            _dummySchedule.EventId = string.Empty;
            _dummySchedule.Exact_Start = DateTime.MinValue;
            _dummySchedule.Exact_Stop = DateTime.MinValue;
            _dummySchedule.IdIngesta = -1;
            _dummySchedule.IdSchedule = -1;
            _dummySchedule.mebs_ingesta = new mebs_ingesta();
            _dummySchedule.mebs_ingesta.Type = Convert.ToSByte(BLC.MediaType.DUMMY_SCHEDULE);
            _dummySchedule.mebs_ingesta.mebs_ingestadetails = new Collection<mebs_ingestadetails>();
            _dummySchedule.Status = Convert.ToInt32(BLC.ScheduleStatus.PREPARED);
            return _dummySchedule;
        }
    }
}