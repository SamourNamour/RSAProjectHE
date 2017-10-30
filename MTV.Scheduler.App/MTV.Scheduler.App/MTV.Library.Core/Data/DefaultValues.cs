using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Library.Core.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class DefaultValues {

        public const string AR_CHANNEL_VIDEO_ITEM_SUFFIX = "CHNL_";
        public const string WS_APP_RESULT_KO = "App ko";
        public const string WS_APP_RESULT_OK = "App ok";
        public const string WS_COMMAND_SENT_OK = "CmdSent_ok";
        public const string WS_RESULT_TEST_OK = "WS ok";
        public const int AUTO_RECORDING_AUTOMATIC_OFFSET = 1;
        public const string TV_CHANNEL_TRIGGER_AUTOMATIC = "auto";
        public const string TV_CHANNEL_TRIGGER_MANUAL = "manual";
        public const string AR_ADV_XML_FILE_CMD_REPOSITORY = "CMD_ADV_BCK";
        public const string XML_DATACASTING_NAME = "dccommand{0}_{1}.xml";
        public static readonly TimeSpan SEND_DC_XML_COMMAND_OFFSET = TimeSpan.FromMinutes(-45);
        public static readonly TimeSpan DC_Inter_Command_Time_Gap = TimeSpan.FromMinutes(5);        
        public static readonly TimeSpan SEND_MPEG7_XML_COMMAND_OFFSET = TimeSpan.FromMinutes(-180);
        public static readonly TimeSpan DC_XML_COMMAND_SEND_GAP = TimeSpan.FromMinutes(5);
        public static readonly TimeSpan DC_XML_COMMAND_STARTTIME_OFFSET = TimeSpan.FromMinutes(-1);
        public static readonly string CurrentPath = AppDomain.CurrentDomain.BaseDirectory;
        public const string Setting_IngestedMedia_NLB_FTP_Relative_URI= "IngestedMedia_NLB_FTP_Relative_URI";
        public const string Setting_NLB_FTP_UserName = "NLB_FTP_UserName";
        public const string Setting_NLB_FTP_UserPassword = "NLB_FTP_UserPassword";
        public const string Setting_NLB_FTP_IPAddress = "NLB_FTP_IPAddress";
        public const string Setting_BroadcasterID = "BroadcasterID";
        public const string MPEG7_XML_FILENAME_PATTERN = "{0}_Mpeg7.xml";
        public const string EXTENDED_ADV_XML_FILENAME_PATTERN = "{0}_extended.xml";
        public static readonly TimeSpan EXTENDED_ADV_SENDING_OFFSET = TimeSpan.FromMinutes(-30);
        public const string futureDCListFileName = "futureDCList";
        public const string CategorizationFileName = "content.categories";
        public static string XML_EXTENSION = ".xml";
        public static string BESTVINFO_VERSION = "4.0";


        public const string IngestedMedia_Directory = @"IngestedMedia\";
        public const string ClearMedia_Directory = @"ClearMedia\";

        public DefaultValues() {
        }
    }
}
