using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Library.Common
{
    public class DefaultValue
    {
        public const string DateTimeFormat = "dd/MM/yyyy";

        #region AR XML
        public const string AR_Description = "Abstract";
        public const string AR_Actors = "Actors";
        public const string AR_Directors = "Directors";
        public const string AR_Genre = "Genre";
        public const string AR_Form = "Form";
        public const string AR_Country = "Country";
        public const string AR_Year = "Year";
        public const string AR_Language = "Language";
        #endregion

        #region DC XML
        public const string DC_Description = "Abstract";
        public const string DC_Actors = "Actors";
        public const string DC_Directors = "Directors";
        public const string DC_Genre = "Genre";
        public const string DC_Form = "Form";
        public const string DC_Country = "Country";
        public const string DC_Year = "Year";
        public const string DC_Language = "languages";
        public const string DC_ScreenFormat = "Screen_Format";
        public const string DC_FileSize = "contentFileSize";
        public const string DC_Duration = "Duration";
        public const string DC_CHANNEL_SHORTNAME = "PVOD";

        #endregion

        #region Video Duration
        public const int TS_PACKET_SIZE = 188;
        public const double SYSTEM_CLOCK_FREQUENCY = 27000000.0;
        public const double DIVISION_PCR = 300.0;
        public const double TIME_UNIT_BASE_PCR = DIVISION_PCR / SYSTEM_CLOCK_FREQUENCY;
        public const double TIME_UNIT_EXT_PCR = 1 / SYSTEM_CLOCK_FREQUENCY;
        #endregion

        #region Prefix
        public const string RG_REQUEST_PREFIX = "RG_REQUEST";
        #endregion

        #region Encapsulator Status
        public const string WS_APP_RESULT_KO = "App ko";
        public const string WS_APP_RESULT_OK = "App ok";
        #endregion

        #region MEBSMAM Design Constant

        public const string Common_Path = "Common";
        //public const string Logo_Default = "~/Common/transparent.gif";
        public const string Cover_Default = "MEBS_Default_Poster.jpg";
        public const string Status_Prepared = "~/Common/prapared.png";
        public const string Status_Started = "~/Common/started.png";
        public const string Status_Stoped = "~/Common/stoped.png";
        //public const string Status_UnknownError = "~/Common/unknownerror.png";
        public const string Status_Locked = "~/Common/locked.png";
        public const string Status_expired = "~/Common/expired.png";
        public const string Status_Missing_Start = "~/Common/missingstart.png";
        public const string Status_Missing_Stop = "~/Common/missingstop.png";
        public const string Status_Failed_Start = "~/Common/failedstart.png";
        public const string Status_Failed_Stop = "~/Common/failedstop.png";

        public const string BTN_Remove = "~/Common/btnRemove.png";
        public const string BTN_Add = "~/Common/btnAdd.png";

        public const string PNG_Point = "~/Common/point.png";
        public const string PNG_Point_Red = "~/Common/pointRouge.png";
        public const string Status_Color_Prepared = "#604029";
        public const string Status_Color_Started = "#920000";
        public const string Status_Color_Stoped = "#0c3d1a";
        //public const string Status_Color_UnknownError = "#49206f";
        public const string Status_Color_Locked = "#1e355c";
        public const string Status_Color_expired = "#5c1e4c";
        public const string Status_Color_Missing_Start = "#0ba0a0";
        public const string Status_Color_Missing_Stop = "#49206f";
        public const string Status_Color_Failed_Start = "#49206f";
        public const string Status_Color_Failed_Stop = "#49206f";

        public const int Cover_Width = 81;
        public const int Cover_Height = 181;
        
        #endregion

        #region MEBSMAM Messages Constant:
        public const string MSG_EVENT_NOT_FOUND = "EVENT_NOT_FOUND";
        public const string MSG_OUTDATED_EVENT = "OUTDATED_EVENT";
        public const string MSG_INTERNAL_ERROR = "INTERNAL_ERROR";
        public const string MSG_CHANNEL_NOT_POPULATED = "CHANNEL_NOT_POPULATED";
        public const string MSG_DATA_NOT_FOUND = "DATA_NOT_FOUND";
        public const string MSG_CATEGORY_NOT_POPULATED = "CATEGORY_NOT_POPULATED";
        public const string MSG_SAVE_OK = "SAVE_OK - The operation was successful";
        public const string MSG_PUBLISH_OK = "PUBLISH_OK - The operation was successful";
        public const string MSG_EVENT_EXPIRED = "EVENT_EXPIRED";
        public const string MSG_EVENT_ERROR = "EVENT_ERROR";
        public const string MSG_TRIGGER_NOT_RECEIVED = "TRIGGER_NOT_RECEIVED";
        public const string MSG_EVENT_LOCKED = "EVENT_LOCKED";
        public const string MSG_EVENT_RECORDING = "EVENT_RECORDING";
        public const string MSG_LIFEMODE_VALIDATION_ERROR = "INPUT_VALIDATION_ERROR - The Immortality value cannot be greater than Expiration.";
        public const string MSG_EXPIRATION_VALIDATION_ERROR = "EXPIRATION_INPUT_VALIDATION_ERROR";
        public const string MSG_IMMORTALITY_VALIDATION_ERROR = "IMMORTALITY_INPUT_VALIDATION_ERROR";
        #endregion

        #region MEBSMAM - Settings
        //public const string DC_IntervalPackage = "DC_IntervalPackage";
        public const string DC_User_Bitrate = "DC_User_Bitrate";
        public const string DC_TimeInterval = "DC_TimeInterval";
        public const string DC_Time_Frame = "DC_Time_Frame";
        public const string DC_Inter_Package_Time_Gap = "DC_Inter_Package_Time_Gap";
        public const string SchedulerTimeInterval = "SchedulerTimeInterval";
        public const string IngestedMedia_NLB_FTP_Relative_URI = "IngestedMedia_NLB_FTP_Relative_URI";
        public const string NLB_FTP_UserName = "NLB_FTP_UserName";
        public const string NLB_FTP_UserPassword = "NLB_FTP_UserPassword";
        public const string NLB_FTP_IPAddress = "NLB_FTP_IPAddress";
        public const string Categorization_Channel_Sending = "Categorization_Channel_Sending";

        public const string MSG_NO_SELECTED_TIMEZONE = "NO_SELECTED_TIMEZONE";
        public const string MSG_NO_SELECTED_PRODUCT = "NO_SELECTED_PRODUCT";
        public const string MSG_PRODUCT_NOT_FOUND = "PRODUCT_NOT_FOUND";
        public const string MSG_PRODUCT_SIZE_ERROR = "PRODUCT_SIZE_ERROR";
        public const string MSG_OVERLAPPING_DETECTED = "OVERLAPPING_DETECTED";
        public const string MSG_TIME_NOT_ENOUGH = "TIME_NOT_ENOUGH";
        #endregion

        #region MESMAM - Settings - DefaultValue
        public const int Default_DC_User_Bitrate = 1111111;
        public const int Default_DC_TimeInterval = 16777215;
        public const int Default_DC_IntervalPackage = 4;
        public const int Default_DC_Time_Frame = 5;
        public const int Default_DC_Inter_Package_Time_Gap = 4;
        #endregion

        #region MEBSMAM - Advertisement
        /// <summary>
        /// StatTime with This Value : Indicate that Advertisement is In Previous list
        /// </summary>
        public const string AdvInPrevious = "00:00:00.0";

        /// <summary>
        /// StatTime with This Value : Indicate that Advertisement is In Rear list
        /// </summary>
        public const string AdvInRear = "99:99:99.9";
        #endregion

        #region Default USERS
        public const string Default_Admin_Name = "Administrator";
        public const string Default_Admin_Psw = "MEBSAdmin";
        public const string Default_User_Name = "User";
        public const string Default_User_Psw = "MEBSUser";
        #endregion

        #region MEBSConfig
        public const int IdDefaultLanguage = 1;
        public const string ContentCategoriesDirectoryName = "ContentCategories";
        public const string ContentCategoriesFileName = "content.categories.xml";
        public const string AllCategoryXMLValue = "all";
        public const string UnclassCategoryXMLValue = "unclass";
        public const string Logo_Default = "MEBS_Default_Logo.jpg";

        public const int AllCategoryDataBaseValue = 1;
        public const int UnclassCategoryDataBaseValue = 2;

        public const string Visibility_True = "true";
        public const string Visibility_False = "false";
        public const string Visibility_Always = "always";

        // I'am not put 0 to avoid troubles with (512,0) separator
        // The Value '1' is reserved for 'ALL' category    
        // The value '2'    
        public const int MinCategoryValue = 3;

        // Every category shall be identified by a unique number in the range 1-65534 (Cf. Content Categorization.pdf page 1)
        public const int MaxCategoryValue = 65534;
        #endregion
    }
}
