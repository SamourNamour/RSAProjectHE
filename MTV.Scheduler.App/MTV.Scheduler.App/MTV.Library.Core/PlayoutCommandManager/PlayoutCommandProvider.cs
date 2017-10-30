#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: PlayoutCommandProvider.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.- Class : Namespace (s) -.-.-.-.-.-.-.-.-.-
using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;

// Custom Directive(s)
using MTV.Library.Core.Tools;
using MTV.Library.Core.Data;
using MTV.Library.Core.Proxy;
using MTV.Library.Core.Services;
using MTV.Scheduler.App.MEBSCatalogServiceRef;
using MTV.Scheduler.App.UI;
using MTV.Library.Core.Common;
#endregion 

namespace MTV.Library.Core.PlayoutCommandManager
{
    /// <summary>
    /// 
    /// </summary>
    public class PlayoutCommandProvider
    {

        #region -.-.-.-.-.-.-.-.-.-.- Nested Class(es) -.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        public class ReceiverAction {

            /// <summary>
            /// 
            /// </summary>
            public string ActionName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public int ContentId { get; set; }
        }

        /// <summary>
        /// 
        /// </summary>
        public class StringWriterWithEncoding : StringWriter {
            /// <summary>
            /// 
            /// </summary>
            Encoding encoding;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="builder"></param>
            /// <param name="encoding"></param>
            public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
                : base(builder) {
                this.encoding = encoding;
            }

            public override Encoding Encoding {
                get { return encoding; }
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        public static string CommandBackupFullDirectoryPath {
            get {
                return string.Format("{0}{1}{2}",
                                       DefaultValues.CurrentPath,
                                       DefaultValues.AR_ADV_XML_FILE_CMD_REPOSITORY,
                                       Path.DirectorySeparatorChar);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int XML_CANAL_ENVIAMENT {
            get {
                try {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DatacastCommand_Channel_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string IngestedMedia_NLB_FTP_Relative_URI
        {
            get
            {
                try
                {
                    return MEBSCatalogProvider.GetSettingByKeyName("IngestedMedia_NLB_FTP_Relative_URI");
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NLB_FTP_UserName {
            get {
                try {
                    return MEBSCatalogProvider.GetSettingByKeyName("NLB_FTP_UserName");
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NLB_FTP_UserPassword {
            get {
                try {
                    return MEBSCatalogProvider.GetSettingByKeyName("NLB_FTP_UserPassword");
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string NLB_FTP_IPAddress {
            get {
                try {
                    return MEBSCatalogProvider.GetSettingByKeyName("NLB_FTP_IPAddress");
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int DatacastCommand_Channel_Sending {
            get {
                try {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DatacastCommand_Channel_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int AR_Cover_Channel_Sending {
            get {
                try {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("AR_Cover_Channel_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int DC_Cover_Channel_Sending {
            get {
                try {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DC_Cover_Channel_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int Interval_Cover_Sending {
            get {
                try {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("Interval_Cover_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return -1;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan DCCommand_Sending_Offset {
            get {
                try {
                    return TimeSpan.FromMinutes(int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DCCommand_Sending_Offset")));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return DefaultValues.SEND_DC_XML_COMMAND_OFFSET;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan DC_Inter_Command_Time_Gap {
            get {
                try {
                    return TimeSpan.FromMinutes(int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DC_Inter_Command_Time_Gap")));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return DefaultValues.DC_Inter_Command_Time_Gap;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan ExtendedAdv_Sending_Offset {
            get {
                try {
                    return TimeSpan.FromMinutes(int.Parse(MEBSCatalogProvider.GetSettingByKeyName("ExtendedAdv_Sending_Offset")));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return DefaultValues.EXTENDED_ADV_SENDING_OFFSET;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int AR_EXTENDED_XML_CANAL_ENVIAMENT {
            get {
                try {
                    return 4; // int.Parse(MEBSCatalogProvider.GetSettingByKeyName("DatacastCommand_Channel_Sending"));
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                    return 4;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int FUTUR_DC_LIST_XML_CANAL_ENVIAMENT
        {
            get
            {
                try
                {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("FuturDCList_Channel_Sending"));
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return 7;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TimeSpan AR_Extended_Command_Time_Gap {
            get {
                return TimeSpan.FromMinutes(5);
            }
        }

                /// <summary>
        /// 
        /// </summary>
        public static int CATEGORIZATION_XML_CANAL_ENVIAMENT
        {
            get
            {
                try
                {
                    return int.Parse(MEBSCatalogProvider.GetSettingByKeyName("Categorization_Channel_Sending"));
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return 0;
                }
            }
        }

        public static string BroadcasterID
        {
            get
            {
                try
                {
                    return MEBSCatalogProvider.GetSettingByKeyName("BroadcasterID");
                }
                catch (Exception ex)
                {
                    MainForm.LogExceptionToFile(ex);
                    return "513";
                }
            }
        }


        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-.-
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <param name="triggerEntryItem"></param>
        /// <returns></returns>
        private static string SendAutomaticRecordingStartCommand(mebs_schedule mebsScheduleParam,
                                                                 TriggerEntry triggerEntryItem,
                                                                 CatchupTVCommandType commandType)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            string cmdXmlBodyString = string.Empty;
            byte iMaxTry = 5;

            XDocument xDocumentItem = null;
            try
            {
                switch (commandType)
                {
                    case CatchupTVCommandType.DUMMY:
                        xDocumentItem = FromMebsScheduleToDummyCommandXDocument(mebsScheduleParam);
                        break;

                    case CatchupTVCommandType.ACTUAL:
                    case CatchupTVCommandType.ADVERTISEMENT:
                        break;

                    default:
                        break;
                }

            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }

            cmdXmlBodyString = (xDocumentItem != null ?
                                FromXDocumentToString(xDocumentItem) :
                                null);
            if (string.IsNullOrEmpty(cmdXmlBodyString))
            {
                return null;
            }

            string folderPath = string.Format("{0}{1}_{2}",
                                                CommandBackupFullDirectoryPath,
                                                mebsScheduleParam.IdSchedule.ToString(),
                                                mebsScheduleParam.ContentID.ToString());

            string fileName = string.Format("{0}_{1}_{2}_{3}_{4}.xml",
                                             mebsScheduleParam.ContentID.ToString(),
                                             mebsScheduleParam.IdSchedule.ToString(),
                                             commandType.ToString(),
                                             commandType == CatchupTVCommandType.DUMMY ?
                                             MaterialType.U.ToString() :
                                             triggerEntryItem.TypeOfMaterial.ToString(),
                                             commandType == CatchupTVCommandType.DUMMY ?
                                             mebsScheduleParam.Estimated_Start.Value.ToString("yyyy-MM-dd_hh-mm-ss-ff") :
                                             triggerEntryItem.TIME.ToString("yyyy-MM-dd_hh-mm-ss-ff")
                                             );

            string fileFullPath = string.Format("{0}{1}{2}",
                                                 folderPath,
                                                 Path.DirectorySeparatorChar,
                                                 fileName);
            try
            {
                XDocumentToXmlFile(xDocumentItem, folderPath, fileFullPath);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }

            List<mebs_encapsulator> metadataEncapsulatorCollection;
            try
            {
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                 metadataEncapsulatorCollection = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
                if (metadataEncapsulatorCollection == null || metadataEncapsulatorCollection.Count <= 0)
                {

                    msg = string.Format(@"Send automatic-recording START command to encapsulator failed (No Metadata encapsulator found)).
                                        IdSchedule = {0},
                                        Code_Package = {1}.",
                                          mebsScheduleParam.IdSchedule,
                                          mebsScheduleParam.mebs_ingesta.Code_Package);
                    switch (commandType)
                    {
                        case CatchupTVCommandType.DUMMY:
                            msg = string.Format(@"Sending Dummy-command to encapsulator failed (No Metadata encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                        mebsScheduleParam.IdSchedule,
                                        mebsScheduleParam.mebs_ingesta.Code_Package);
                            break;

                        case CatchupTVCommandType.ACTUAL:
                        case CatchupTVCommandType.ADVERTISEMENT:                         
                            break;

                        default:
                            break;
                    }
                    
                       

                    MainForm.LogErrorToFile(msg);
                    return null;
                }
            }

            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }

            
            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {

                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.CreaTVComandaGravacioV2(cmdXmlBodyString);
                        break;
                    }
                    catch (Exception ex)
                    {
                        
                        msg = string.Format(@"Tentative {0} To send invalid automatic-recording command to encapsulator (either null or empty).
                                              IdSchedule = {1},
                                              Code_Package = {2}.
                                              ",
                                              iMaxTry,
                                              mebsScheduleParam.IdSchedule,
                                              mebsScheduleParam.mebs_ingesta.Code_Package);

                        switch (commandType)
                        {
                            case CatchupTVCommandType.DUMMY:
                                msg = string.Format(@"Tentative {0} To send invalid Dummy-command to encapsulator (either null or empty).
                                              IdSchedule = {1},
                                              Code_Package = {2}.
                                              ",
                                               iMaxTry,
                                               mebsScheduleParam.IdSchedule,
                                               mebsScheduleParam.mebs_ingesta.Code_Package);
                                break;

                            case CatchupTVCommandType.ACTUAL:
                            case CatchupTVCommandType.ADVERTISEMENT:
                                break;

                            default:
                                break;
                        }

                        iMaxTry--;
                        MainForm.LogWarningToFile(msg);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(300).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
               
            }

            return reply;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <returns></returns>
        private static XDocument FromMebsScheduleToDummyCommandXDocument(mebs_schedule mebsScheduleParam) {
            XDocument xDocumentItem = null;
            try {
                string startTriggerTypeElement = DefaultValues.TV_CHANNEL_TRIGGER_AUTOMATIC;
                string stopTriggerTypeElement = DefaultValues.TV_CHANNEL_TRIGGER_AUTOMATIC;
                string autorecXmlCommandStartDate = DateTimeUtils.GenerateStringDate(mebsScheduleParam.Estimated_Start.Value);
                string autorecXmlCommandStopDate = DateTimeUtils.GenerateStringDate(mebsScheduleParam.Estimated_Stop.Value);
                string autorecXmlCommandStartTime = DateTimeUtils.GenerateStringTime(mebsScheduleParam.Estimated_Start.Value);
                string autorecXmlCommandStopTime = DateTimeUtils.GenerateStringTime(mebsScheduleParam.Estimated_Stop.Value);
                int? contentID = mebsScheduleParam.ContentID * -1;

                xDocumentItem =
                    new XDocument
                    (
                    new XDeclaration("1.0", "utf-8", "no"),
                    new XElement("RecCmd",
                                  new XAttribute("version", "2.2"),
                                  new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                  new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/RecCmd_schema.xsd"),
                                  new XElement("Priority", "1"),
                                  new XElement("Service",
                                                new XAttribute("type", "dvb"),
                                                new XElement("SID", default(int)),
                                                new XElement("ONID", default(int)),
                                                new XElement("TSID", default(int))
                                              ),
                                  new XElement("Source",
                                               new XElement("Type", "tuner"),
                                               new XElement("Path", "0")
                                              ),
                                  new XElement("Start",
                                               new XElement("Trigger", startTriggerTypeElement),
                                               new XElement("Date", autorecXmlCommandStartDate),
                                               new XElement("Time", autorecXmlCommandStartTime),
                                               new XElement("PreliminaryActions", null),
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null)
                                              ),
                                new XElement("Stop",
                                               new XElement("Trigger", stopTriggerTypeElement),
                                               new XElement("Date", autorecXmlCommandStopDate),
                                               new XElement("Time", autorecXmlCommandStopTime),
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null),
                                               new XElement("OnCancelActions", null)
                                              ),
                                 new XElement("BesTVInfo",
                                               new XAttribute("version", DefaultValues.BESTVINFO_VERSION),
                                               new XElement("ContentId", contentID.Value.ToString()),
                                               new XElement("BroadcasterId", "513"),
                                               new XElement("Template", null),
                                               new XElement("Type", Convert.ToInt32(MediaType.TV_CHANNEL).ToString()),
                                               new XElement("Queue", "1"),
                                               new XElement("Name", null),
                                               new XElement("ParentalRating", mebsScheduleParam.mebs_ingesta.ParentalRating),
                                               new XElement("Category", default(int)),
                                               new XElement("Hidden", "false"),
                                               new XElement("PublishAfter", default(int)),
                                               new XElement("ExpiresAfter", default(int)),
                                               new XElement("ImmortalDuring", default(int)),
                                               new XElement("MinLifeAfterFirstAccess", default(int)),
                                               new XElement("LifeAfterFirstAccess", default(int)),
                                               new XElement("MinLifeAfterActivation", default(int)),
                                               new XElement("LifeAfterActivation", default(int)),
                                               new XElement("DisableAccess", "false"),
                                               new XElement("ActiveSince", null),
                                               new XElement("ActiveDuring", default(int)),
                                               new XElement("ActiveTimeAfterFirstAccess", default(int)),
                                               new XElement("MinActiveTimeAfterFirstAccess", default(int)),
                                               new XElement("MaxAccesses", default(int)),
                                               new XElement("DrmProtected", "false"),
                                               new XElement("CopyControl", null),
                                               new XElement("PreservationPriority", "0"),
                                               new XElement("ContentFileSizeInMB", "0"),
                                               new XElement("ExtendedInfo")
                                             )
                                )
                    );
            }
            catch (Exception ex) {
                MainForm.LogExceptionToFile(ex);
            }
            return xDocumentItem ?? null;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc">XDocument</param>
        /// <param name="folderFullPath">string</param>
        /// <param name="fileFullPath">string</param>
        private static void XDocumentToXmlFile(XDocument doc,
                                               string folderFullPath,
                                               string fileFullPath)
        {
            if (!Directory.Exists(folderFullPath))
            {
                Directory.CreateDirectory(folderFullPath);
            }

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = new UTF8Encoding(false); // The false means, do not emit the BOM.
            using (XmlWriter w = XmlWriter.Create(fileFullPath, settings))
            {
                doc.Save(w);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xDocumentItem"></param>
        /// <returns></returns>
        private static string FromXDocumentToString(XDocument xDocumentItem)
        {
            return ConvertToUTF7(string.Format("{0}{1}{2}",
                                                xDocumentItem.Declaration,
                                                Environment.NewLine,
                                                xDocumentItem));
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlBodyString"></param>
        /// <returns></returns>
        private static string ConvertToUTF7(string xmlBodyString)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(xmlBodyString);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                using (TextReader TR = new StreamReader(stream, Encoding.UTF7))
                {
                    return TR.ReadToEnd();
                }
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static string SendARExtendedCommand(mebs_schedule mebsScheduleParam,
                                                    int nDataChannel)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            string cmdXmlBodyString = string.Empty;
            byte iMaxTry = 5;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
            var exists = default(Boolean);
            XDocument xDocumentItem = null;

            try
            {
                xDocumentItem = FromMebsScheduleToDataCastCommandXDocument(mebsScheduleParam);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            cmdXmlBodyString = (xDocumentItem != null ?
                                FromXDocumentToString(xDocumentItem) :
                                null);

            if (string.IsNullOrEmpty(cmdXmlBodyString))
            {
                MainForm.LogErrorToFile(string.Format(@"Attempt to send invalid AR Extended command to Metadata encapsulator (either null or empty).
                                                        IdSchedule = {0},
                                                        Code_Package = {1}.",
                                                        mebsScheduleParam.IdSchedule,
                                                        mebsScheduleParam.mebs_ingesta.Code_Package));
                return null;
            }
            string folderPath = string.Format("{0}{1}",
                                               CommandBackupFullDirectoryPath,
                                               mebsScheduleParam.IdSchedule.ToString());

            string fileName = string.Format(DefaultValues.EXTENDED_ADV_XML_FILENAME_PATTERN,
                                            mebsScheduleParam.ContentID.Value
                                            );

            string fileFullPath = string.Format("{0}{1}{2}",
                                                 folderPath,
                                                 Path.DirectorySeparatorChar,
                                                 fileName);

            XDocumentToXmlFile(xDocumentItem, folderPath, fileFullPath);

            exists = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).Any<mebs_encapsulator>();
            if (!exists)
            {
                msg = string.Format(@"Send AR Extended xml command to Metadata encapsulator failed (No Metadata encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);

                MainForm.LogWarningToFile(msg);
                return null;
            }
            List<mebs_encapsulator> metadataEncapsulatorCollection =
                                    proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {
                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.EnviaXML(fileName,
                                                             nDataChannel,
                                                             NLB_FTP_IPAddress,
                                                             IngestedMedia_NLB_FTP_Relative_URI,
                                                             NLB_FTP_UserName,
                                                             NLB_FTP_UserPassword);
                        break;
                    }
                    catch (Exception ex)
                    {
                        msg = string.Format(@"Tentative {0} To re-send AR Extended xml command to Metadata encapsulator.
                                              IdSchedule = {1},
                                              Code_Package = {2}.
                                              ",
                                              iMaxTry,
                                              mebsScheduleParam.IdSchedule,
                                              mebsScheduleParam.mebs_ingesta.Code_Package);
                        iMaxTry--;
                        
                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }

            return reply;
        }


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <returns></returns>
        private static XDocument FromMebsScheduleToDataCastCommandXDocument(mebs_schedule mebsScheduleParam)
        {
            XDocument xDocumentItem = null;
            try
            {
                DateTime datacastingXmlCommandStartDateTime = mebsScheduleParam.Estimated_Start.Value.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET);
                DateTime datacastingXmlCommandStopDateTime = mebsScheduleParam.Estimated_Stop.Value;
                string startTriggerTypeElement,
                       stopTriggerTypeElement;
                startTriggerTypeElement =
                stopTriggerTypeElement = DefaultValues.TV_CHANNEL_TRIGGER_AUTOMATIC;
                int? contentID = mebsScheduleParam.ContentID.Value;
                TimeSpan expiresAfter = (mebsScheduleParam.mebs_ingesta.Expiration_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                 default(TimeSpan) :
                                 mebsScheduleParam.mebs_ingesta.Expiration_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                 );
                TimeSpan immortalDuring = (mebsScheduleParam.mebs_ingesta.Immortality_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                           default(TimeSpan) :
                                           mebsScheduleParam.mebs_ingesta.Immortality_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                           );
                xDocumentItem =
                  new XDocument
                  (
                  new XDeclaration("1.0", "utf-8", "no"),
                  new XElement("RecCmd",
                                new XAttribute("version", "2.2"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/PVODCmd_schema.xsd"),
                                new XElement("Priority", "1"),
                                from mebs_channeltuning mebsChannelTuningOccurrence
                                in MEBSCatalogProvider.ChannelDVBTripletCollection(mebsScheduleParam.mebs_ingesta.mebs_channel.IdChannel)
                                select new XElement("Service",
                                                     new XAttribute("type", "dvb"),
                                                     new XElement("SID", mebsChannelTuningOccurrence.ServiceID),
                                                     new XElement("ONID", mebsChannelTuningOccurrence.OriginalNetworkID),
                                                     new XElement("TSID", mebsChannelTuningOccurrence.TransportStreamID)
                                                    ),
                                new XElement("Source",
                                               new XElement("Type", "tuner"),
                                               new XElement("Path", "2")
                                              ),
                                new XElement("Start",
                                               new XElement("Trigger", startTriggerTypeElement),
                                               new XElement("Date", DateTimeUtils.GenerateStringDate(datacastingXmlCommandStartDateTime)),
                                               new XElement("Time", DateTimeUtils.GenerateStringTime(datacastingXmlCommandStartDateTime)),
                                               new XElement("PreliminaryActions", null),
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null)
                                              ),
                                new XElement("Stop",
                                               new XElement("Trigger", stopTriggerTypeElement),
                                               new XElement("Date", DateTimeUtils.GenerateStringDate(datacastingXmlCommandStopDateTime)),
                                               new XElement("Time", DateTimeUtils.GenerateStringTime(datacastingXmlCommandStopDateTime)),
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null),
                                               new XElement("OnCancelActions", null)
                                              ),
                                new XElement("BesTVInfo",
                                               new XAttribute("version", DefaultValues.BESTVINFO_VERSION),
                                               new XElement("ContentId", contentID.Value.ToString()),
                                               new XElement("BroadcasterId", BroadcasterID),
                                               new XElement("Template", null),
                                               new XElement("Type", Convert.ToInt32(MediaType.DC_CHANNEL).ToString()),
                                               new XElement("Queue", "2"),
                                               new XElement("Name", mebsScheduleParam.mebs_ingesta.Title),
                                               new XElement("ParentalRating", mebsScheduleParam.mebs_ingesta.ParentalRating),
                                               from mebs_ingesta_category_mapping mebsCategoryMappingOccurrence
                                               in mebsScheduleParam.mebs_ingesta.mebs_ingesta_category_mapping.ToList()
                                               select new XElement("Category", MEBSCatalogProvider.GetCategoryByID(mebsCategoryMappingOccurrence.IdCategory).Value),
                                               new XElement("Hidden", mebsScheduleParam.mebs_ingesta.Hidden.Value.ToString()),
                                               new XElement("PublishAfter", mebsScheduleParam.mebs_ingesta.PublishAfter.Value.ToString()),
                                               new XElement("ExpiresAfter", ((int)expiresAfter.TotalMinutes).ToString()),
                                               new XElement("ImmortalDuring", ((int)immortalDuring.TotalMinutes).ToString()),
                                               new XElement("MinLifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinLifeAfterFirstAccess.Value.ToString()),
                                               new XElement("LifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.LifeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinLifeAfterActivation", mebsScheduleParam.mebs_ingesta.MinLifeAfterActivation.Value.ToString()),
                                               new XElement("LifeAfterActivation", mebsScheduleParam.mebs_ingesta.LifeAfterActivation.Value.ToString()),
                                               new XElement("DisableAccess", mebsScheduleParam.mebs_ingesta.DisableAccess.Value.ToString()),
                                               new XElement("ActiveSince", FormatActiveSinceInputValue(mebsScheduleParam.mebs_ingesta.ActiveSince)),
                                               new XElement("ActiveDuring", mebsScheduleParam.mebs_ingesta.ActiveDuring.Value.ToString()),
                                               new XElement("ActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.ActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MaxAccesses", mebsScheduleParam.mebs_ingesta.MaxAccesses.Value.ToString()),
                                               new XElement("DrmProtected", mebsScheduleParam.mebs_ingesta.DrmProtected.Value.ToString()),
                                               new XElement("CopyControl", mebsScheduleParam.mebs_ingesta.CopyControl),
                                               new XElement("PreservationPriority", mebsScheduleParam.mebs_ingesta.PreservationPriority),
                                               new XElement("ContentFileSizeInMB", Utils.ConvertByteToMegaByte((long)mebsScheduleParam.mebs_ingesta.MediaFileSizeAfterRedundancy).ToString()),
                                               new XElement("ExtendedInfo", null)
                                             )
                               )
                  );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            return xDocumentItem ?? null;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static bool UploadARExtendedFile(mebs_schedule mebsScheduleParam)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            string cmdXmlBodyString = string.Empty;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
            var exists = default(Boolean);
            XDocument xDocumentItem = null;

            try
            {
                xDocumentItem = FromMebsScheduleToARExtendedXDocument(mebsScheduleParam);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            if (xDocumentItem == null)
            {
                MainForm.LogErrorToFile(string.Format(@"Attempt to send invalid AR Extend file to Metadata encapsulator (either null or empty).
                                                        IdSchedule = {0},
                                                        Code_Package = {1}.",
                                                        mebsScheduleParam.IdSchedule,
                                                        mebsScheduleParam.mebs_ingesta.Code_Package));
                return exists;
            }

            string arExtendedFileName = string.Format(DefaultValues.EXTENDED_ADV_XML_FILENAME_PATTERN,
                                                      mebsScheduleParam.ContentID
                                                      );


            CreateFileIntoFtpServerRepository(xDocumentItem,
                                              arExtendedFileName);

            exists = CheckFileCreation(arExtendedFileName);

            return exists;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <param name="triggerEntryItem"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private static XDocument FromMebsScheduleToARExtendedXDocument(mebs_schedule mebsScheduleParam)
        {
            XDocument xDocumentItem = null;
            try
            {
                int? contentID = mebsScheduleParam.ContentID;

                XElement extendedInfo = new XElement("ExtendedInfo",
                                                      new XElement("AdvertisementDescription",
                                                                    new XAttribute("version", "1.0"),
                                                                    new XElement("AdPlaylist",
                                                                                  new XAttribute("name", "OnPlay"),
                                                                                  from mebs_ingesta_advertisement_mapping advOccurrence
                                                                                  in mebsScheduleParam.mebs_ingesta.mebs_ingesta_advertisement_mapping.ToList()
                                                                                  select new XElement("AdInsertion",
                                                                                                       new XElement("StartTime", advOccurrence.StartTimePoint),
                                                                                                       new XElement("ContentId", contentID.ToString()),
                                                                                                       new XElement("MaxFwdSpeed", advOccurrence.MaxFwdSpeed.Value.ToString()),
                                                                                                       new XElement("MaxRwdSpeed", advOccurrence.MaxRwdSpeed.Value.ToString()),
                                                                                                       new XElement("CanSkip", advOccurrence.CanSkip.Value ?
                                                                                                                               "yes" :
                                                                                                                               "no")
                                                                                                       )
                                                                                  )
                                                                      )
                                                      );

                xDocumentItem =
                    new XDocument
                    (
                    new XDeclaration("1.0", "utf-8", "no"),
                    new XElement("ExtendedRecCmd",
                                  new XAttribute("version", "1.0"),
                                  new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                  new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/RecCmd_schema.xsd"),
                                 new XElement("BesTVInfo",
                                               new XAttribute("version", "3.0"),
                                               new XElement("ContentId", contentID.Value.ToString()),
                                               new XElement("BroadcasterId", BroadcasterID),
                                               extendedInfo
                                             )
                                )
                    );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            return xDocumentItem ?? null;
        }
        
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="mpeg7FullPath"></param>
        private static void CreateFileIntoFtpServerRepository(XDocument doc,
                                                              string fileName)
        {
            try
            {
                string mpeg7FullPath = string.Format("{0}{1}{2}",
                                                      MainForm.Conf.ReqDirectory,
                                                      DefaultValues.IngestedMedia_Directory,
                                                      fileName);
                doc.Save(mpeg7FullPath);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mpeg7FileName"></param>
        /// <returns></returns>
        private static bool CheckFileCreation(string fileName)
        {
            try
            {
                string mpeg7FullPath = string.Format("{0}{1}{2}", MainForm.Conf.ReqDirectory, DefaultValues.IngestedMedia_Directory, fileName);
                return File.Exists(mpeg7FullPath);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <returns></returns>
        private static string SendAutomaticRecordingStopCommand(mebs_schedule mebsScheduleParam)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            byte iMaxTry = 5;

            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

            List<mebs_encapsulator> metadataEncapsulatorCollection = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
            if (metadataEncapsulatorCollection == null || metadataEncapsulatorCollection.Count <= 0)
            {
                msg = string.Format(@"Send automatic-recording STOP command to encapsulator failed (No Metadata encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);

                MainForm.LogWarningToFile(msg);
                return null;
            }

            
            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {
                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);
                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.AturaTVComandaGravacio(mebsScheduleParam.ContentID.Value);
                        break;
                    }
                    catch (Exception ex)
                    {
                        msg = string.Format(@"Tentative {0} To send automatic-recording STOP command to encapsulator.
                                              IdSchedule = {1},
                                              Code_Package = {2}.",
                                              iMaxTry,
                                              mebsScheduleParam.IdSchedule,
                                              mebsScheduleParam.mebs_ingesta.Code_Package);
                        iMaxTry--;

                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }
            return reply;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <returns></returns>
        private static XDocument FromMebsScheduleToMPEG7CommandXDocument(mebs_schedule mebsScheduleParam)
        {
            XDocument xDocumentItem = null;
            try
            {
                DateTime datacastingXmlCommandStartDateTime = mebsScheduleParam.Estimated_Start.Value; //.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET);
                DateTime datacastingXmlCommandStopDateTime = mebsScheduleParam.Estimated_Stop.Value;
                int? contentID = mebsScheduleParam.ContentID.Value;
                TimeSpan expiresAfter = (mebsScheduleParam.mebs_ingesta.Expiration_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                 default(TimeSpan) :
                                 mebsScheduleParam.mebs_ingesta.Expiration_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                 );
                TimeSpan immortalDuring = (mebsScheduleParam.mebs_ingesta.Immortality_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                           default(TimeSpan) :
                                           mebsScheduleParam.mebs_ingesta.Immortality_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                           );

                XElement extendedInfo = (mebsScheduleParam.mebs_ingesta_advertisement_mapping.Count > 0 ?
                                                      new XElement("AdvertisementDescription",
                                                                    new XAttribute("version", "1.0"),
                                                                    new XElement("AdPlaylist",
                                                                                  new XAttribute("name", "OnPlay"),
                                                                                  from mebs_ingesta_advertisement_mapping advOccurrence
                                                                                  in mebsScheduleParam.mebs_ingesta.mebs_ingesta_advertisement_mapping.ToList()
                                                                                  select new XElement("AdInsertion",
                                                                                                       new XElement("StartTime", advOccurrence.StartTimePoint),
                                                                                                       new XElement("ContentId", contentID.ToString()),
                                                                                                       new XElement("MaxFwdSpeed", advOccurrence.MaxFwdSpeed.Value.ToString()),
                                                                                                       new XElement("MaxRwdSpeed", advOccurrence.MaxRwdSpeed.Value.ToString()),
                                                                                                       new XElement("CanSkip", advOccurrence.CanSkip.Value ?
                                                                                                                               "yes" :
                                                                                                                               "no")
                                                                                                       )
                                                                                  )
                                                                      )

                                           :
                                           null);

                xDocumentItem =
                  new XDocument
                  (
                  new XDeclaration("1.0", "utf-8", "no"),
                  new XElement("DCInfo",
                                new XAttribute("version", "1.0"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/PVODCmd_schema.xsd"),
                                new XElement("Start",
                                               new XElement("OnAssociatedInfoReceptionActions", null)
                                              ),
                                new XElement("Stop",
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null)
                                              ),
                                new XElement("BesTVInfo",
                                               new XAttribute("version", DefaultValues.BESTVINFO_VERSION),
                                               new XElement("ContentId", contentID.Value.ToString()),
                                               new XElement("BroadcasterId", BroadcasterID),
                                               new XElement("Template", null),
                                               new XElement("Type", Convert.ToInt32(MediaType.DC_CHANNEL).ToString()),
                                               new XElement("Queue", "2"),
                                               new XElement("Name", mebsScheduleParam.mebs_ingesta.Title),
                                               new XElement("ParentalRating", mebsScheduleParam.mebs_ingesta.ParentalRating),
                                               from mebs_ingesta_category_mapping mebsCategoryMappingOccurrence
                                               in mebsScheduleParam.mebs_ingesta.mebs_ingesta_category_mapping.ToList()
                                               select new XElement("Category", MEBSCatalogProvider.GetCategoryByID(mebsCategoryMappingOccurrence.IdCategory).Value),
                                               new XElement("Hidden", mebsScheduleParam.mebs_ingesta.Hidden.Value.ToString()),
                                               new XElement("PublishAfter", mebsScheduleParam.mebs_ingesta.PublishAfter.Value.ToString()),
                                               new XElement("ExpiresAfter", ((int)expiresAfter.TotalMinutes).ToString()),
                                               new XElement("ImmortalDuring", ((int)immortalDuring.TotalMinutes).ToString()),
                                               new XElement("MinLifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinLifeAfterFirstAccess.Value.ToString()),
                                               new XElement("LifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.LifeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinLifeAfterActivation", mebsScheduleParam.mebs_ingesta.MinLifeAfterActivation.Value.ToString()),
                                               new XElement("LifeAfterActivation", mebsScheduleParam.mebs_ingesta.LifeAfterActivation.Value.ToString()),
                                               new XElement("DisableAccess", mebsScheduleParam.mebs_ingesta.DisableAccess.Value.ToString()),
                                               new XElement("ActiveSince", FormatActiveSinceInputValue(mebsScheduleParam.mebs_ingesta.ActiveSince)),
                                               new XElement("ActiveDuring", mebsScheduleParam.mebs_ingesta.ActiveDuring.Value.ToString()),
                                               new XElement("ActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.ActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MaxAccesses", mebsScheduleParam.mebs_ingesta.MaxAccesses.Value.ToString()),
                                               new XElement("DrmProtected", mebsScheduleParam.mebs_ingesta.DrmProtected.Value.ToString()),
                                               new XElement("CopyControl", mebsScheduleParam.mebs_ingesta.CopyControl),
                                               new XElement("PreservationPriority", mebsScheduleParam.mebs_ingesta.PreservationPriority),
                                               new XElement("ContentFileSizeInMB", Utils.ConvertByteToMegaByte((long)mebsScheduleParam.mebs_ingesta.MediaFileSizeAfterRedundancy).ToString()),
                                               new XElement("ExtendedInfo",
                                                             from mebs_ingestadetails mebsingestadetailOccurrence
                                                             in mebsScheduleParam.mebs_ingesta.mebs_ingestadetails
                                                             select new XElement(mebsingestadetailOccurrence.DetailsName,
                                                                                 new XAttribute("target", "nlscore"),
                                                                                 new XAttribute("visibility", string.IsNullOrEmpty(mebsingestadetailOccurrence.DetailsValue) ?
                                                                                                                                   false.ToString() :
                                                                                                                                   true.ToString()
                                                                                               ),
                                                                                 mebsingestadetailOccurrence.DetailsValue
                                                                                 ),
                                                             extendedInfo
                                                            )
                                               ),
                                                new XElement("Cover",
                                                                    new XAttribute("version", "1.0")
                                                             )
                               )
                  );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            return xDocumentItem ?? null;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static bool UploadMPEG7File(mebs_schedule mebsScheduleParam)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            string cmdXmlBodyString = string.Empty;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
            var exists = default(Boolean);
            XDocument xDocumentItem = null;

            try
            {
                xDocumentItem = FromMebsScheduleToMPEG7CommandXDocument(mebsScheduleParam);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            if (xDocumentItem == null)
            {
                msg = string.Format(@"Attempt to send invalid MPEG7 file to Asset encapsulator (either null or empty).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);
                MainForm.LogWarningToFile(msg);
                return exists;
            }

            string mpeg7FileName = string.Format(DefaultValues.MPEG7_XML_FILENAME_PATTERN,
                                                 Path.GetFileNameWithoutExtension(mebsScheduleParam.mebs_ingesta.MediaFileNameAfterRedundancy)
                                                 );


            CreateFileIntoFtpServerRepository(xDocumentItem,
                                              mpeg7FileName);

            exists = CheckFileCreation(mpeg7FileName);

            return exists;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static bool UploadDCCommandFile(mebs_schedule mebsScheduleParam)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            string cmdXmlBodyString = string.Empty;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
            var exists = default(Boolean);

            string strCommandBody = string.Empty;
            try
            {
                strCommandBody = FromMebsScheduleToDataCastCommandString(mebsScheduleParam);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            if (string.IsNullOrEmpty(strCommandBody))
            {
                //xDocumentItem == null
                msg = string.Format(@"Attempt to send invalid DCCommand file to Asset encapsulator (either null or empty).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);
                MainForm.LogWarningToFile(msg);
                return exists;
            }

            string dcCommandfileName = string.Format(DefaultValues.XML_DATACASTING_NAME,
                                                     mebsScheduleParam.ContentID.Value.ToString("X"),
                                                     mebsScheduleParam.IdSchedule.ToString("X")
                                                     );

            CreateFileIntoFtpServerRepository(strCommandBody, dcCommandfileName);

            exists = CheckFileCreation(dcCommandfileName);

            return exists;
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static string SendDatacastCommand(mebs_schedule mebsScheduleParam,
                                                  int nDataChannel)
        {
            string msg = string.Empty;
            string reply = string.Empty;

            byte iMaxTry = 5;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

            string fileName = string.Format(DefaultValues.XML_DATACASTING_NAME,
                                            mebsScheduleParam.ContentID.Value.ToString("X"),
                                            mebsScheduleParam.IdSchedule.ToString("X"));

            List<mebs_encapsulator> metadataEncapsulatorCollection =
                        proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
            if (metadataEncapsulatorCollection == null || metadataEncapsulatorCollection.Count <= 0)
            {
                msg = string.Format(@"Send DataCast xml command to Metadata encapsulator failed (No Metadata encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);

                MainForm.LogWarningToFile(msg);
                return null;
            }

            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {
                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.EnviaXML(fileName,
                                                             nDataChannel,
                                                             NLB_FTP_IPAddress,
                                                             IngestedMedia_NLB_FTP_Relative_URI,
                                                             NLB_FTP_UserName,
                                                             NLB_FTP_UserPassword);
                        break;
                    }
                    catch (Exception ex)
                    {
                        msg = string.Format(@"Tentative {0} To re-send DataCast xml command to Metadata encapsulator.
                                              IdSchedule = {1},
                                              Code_Package = {2}.
                                              ",
                                              iMaxTry,
                                              mebsScheduleParam.IdSchedule,
                                              mebsScheduleParam.mebs_ingesta.Code_Package);

                        iMaxTry--;
                        
                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(300).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }

            return reply;
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static string CreateEncapsulatorMPEG7XmlFile(mebs_schedule mebsScheduleParam)
        {
            string msg = string.Empty;
            string reply = string.Empty;

            try
            {
                DateTime datacastingXmlCommandStartDateTime = mebsScheduleParam.Estimated_Start.Value; //.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET);
                DateTime datacastingXmlCommandStopDateTime = mebsScheduleParam.Estimated_Stop.Value;

                TimeSpan expiresAfter = (mebsScheduleParam.mebs_ingesta.Expiration_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ? // DateTime.MinValue
                                         default(TimeSpan) :
                                         mebsScheduleParam.mebs_ingesta.Expiration_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                         );

                TimeSpan immortalDuring = (mebsScheduleParam.mebs_ingesta.Immortality_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ? // DateTime.MinValue
                                           default(TimeSpan) :
                                           mebsScheduleParam.mebs_ingesta.Immortality_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                           );


                string strArxiu = mebsScheduleParam.mebs_ingesta.MediaFileNameAfterRedundancy;
                long lTamany = (long)mebsScheduleParam.mebs_ingesta.MediaFileSizeAfterRedundancy;
                int nPreviewTime = default(int);
                int nPrioritat = (int)expiresAfter.TotalMinutes;
                int nImmortalityTime = (int)immortalDuring.TotalMinutes;
                int nActiveTime = default(int);
                int nLifeExtension = default(int);
                string strDiaIni = DateTimeUtils.GenerateStringDate(datacastingXmlCommandStartDateTime);
                string strHoraIni = DateTimeUtils.GenerateMPEG7StringTime(datacastingXmlCommandStartDateTime);
                string strDiaFi = DateTimeUtils.GenerateStringDate(datacastingXmlCommandStopDateTime);
                string strHoraFi = DateTimeUtils.GenerateMPEG7StringTime(datacastingXmlCommandStopDateTime);
                int nCodiID = mebsScheduleParam.ContentID.Value;
                int nCodiUnlock = default(int);
                double dPreu = default(double);
                string strSMS = "";
                double dPVP = default(double);
                string strAnuncisIni = "";
                string strTamanysIni = "";
                string strDuracionsIni = "";
                string strAnuncisFi = "";
                string strTamanysFi = "";
                string strDuracionsFi = "";
                string strServidorFTP = NLB_FTP_IPAddress;
                string strPathOrigenFTP = IngestedMedia_NLB_FTP_Relative_URI;
                string strUsuariFTP = NLB_FTP_UserName;
                string strPasswordFTP = NLB_FTP_UserPassword;

                byte iMaxTry = 5;
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

                List<mebs_encapsulator> assetEncapsulatorCollection =
                        proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Asset.ToString()) == 0).ToList();
                if (assetEncapsulatorCollection == null || assetEncapsulatorCollection.Count <=0)
                {
                    msg = string.Format(@"Send MPEG7 xml command to Asset encapsulator failed! (No Asset encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                          mebsScheduleParam.IdSchedule,
                                          mebsScheduleParam.mebs_ingesta.Code_Package);

                    MainForm.LogWarningToFile(msg);
                    return null;
                }

                foreach (mebs_encapsulator item in assetEncapsulatorCollection)
                {
                    MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                    do
                    {
                        try
                        {
                            reply = encapsulatorWSProxy.CreaScheduleItem(strArxiu,
                                                                         lTamany,
                                                                         nPreviewTime,
                                                                         nPrioritat,
                                                                         nImmortalityTime,
                                                                         nActiveTime,
                                                                         nLifeExtension,
                                                                         strDiaIni,
                                                                         strHoraIni,
                                                                         strDiaFi,
                                                                         strHoraFi,
                                                                         nCodiID,
                                                                         nCodiUnlock,
                                                                         dPreu,
                                                                         strSMS,
                                                                         dPVP,
                                                                         strAnuncisIni,
                                                                         strTamanysIni,
                                                                         strDuracionsIni,
                                                                         strAnuncisFi,
                                                                         strTamanysFi,
                                                                         strDuracionsFi,
                                                                         strServidorFTP,
                                                                         strPathOrigenFTP,
                                                                         strUsuariFTP,
                                                                         strPasswordFTP
                                                                         );
                            break;
                        }
                        catch (Exception ex)
                        {
                            msg = string.Format(@"Tentative {0} To re-send MPEG7 xml command to Asset encapsulator.
                                                  IdSchedule = {1},
                                                  Code_Package = {2}.",
                                                  iMaxTry,
                                                  mebsScheduleParam.IdSchedule,
                                                  mebsScheduleParam.mebs_ingesta.Code_Package);

                            iMaxTry--;
                            MainForm.LogExceptionToFile(ex);
                            Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                            continue;
                        }
                    } while (iMaxTry > 0 &&
                             string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
                }
                return reply;
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        private static string DeleteMPEG7XmlFile(mebs_schedule mebsScheduleParam)
        {
            try
            {
                string msg = string.Empty;
                string reply = string.Empty;
                byte iMaxTry = 5;
                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);
                List<mebs_encapsulator> assetEncapsulatorCollection =
                                                        proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Asset.ToString()) == 0).ToList();
                if (assetEncapsulatorCollection == null || assetEncapsulatorCollection.Count <= 0)
                {
                    msg = string.Format(@"Send EliminaScheduleItem command to Asset encapsulator failed! (No Asset encapsulator found)).
                                          IdSchedule = {0},
                                          Code_Package = {1}.",
                                          mebsScheduleParam.IdSchedule,
                                          mebsScheduleParam.mebs_ingesta.Code_Package);

                    MainForm.LogWarningToFile(msg);
                    return null;
                }
                
                foreach (mebs_encapsulator item in assetEncapsulatorCollection)
                {
                    MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                    do
                    {
                        try
                        {
                            reply = encapsulatorWSProxy.EliminaScheduleItem(mebsScheduleParam.mebs_ingesta.MediaFileNameAfterRedundancy);
                            break;
                        }
                        catch (Exception ex)
                        {
                            msg = string.Format(@"Tentative {0} To re-send EliminaScheduleItem command to Asset encapsulator.
                                                  IdSchedule = {1},
                                                  Code_Package = {2}.
                                                  ",
                                                  iMaxTry,
                                                  mebsScheduleParam.IdSchedule,
                                                  mebsScheduleParam.mebs_ingesta.Code_Package);

                            iMaxTry--;
                            MainForm.LogExceptionToFile(ex);
                            Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                            continue;
                        }
                    } while (iMaxTry > 0 &&
                             string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
                }

                return reply;
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <param name="nDataChannel"></param>
        /// <returns></returns>
        private static string DeleteDatacastCommand(mebs_schedule mebsScheduleParam,
                                                    int nDataChannel)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            byte iMaxTry = 5;
            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

            List<mebs_encapsulator> assetEncapsulatorCollection =
                                    proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Asset.ToString()) == 0).ToList();
            if (assetEncapsulatorCollection == null || assetEncapsulatorCollection.Count <= 0)
            {
                msg = string.Format(@"Send AturaXML command to Asset encapsulator failed (No Asset encapsulator found)).
                                      IdSchedule = {0},
                                      Code_Package = {1}.",
                                      mebsScheduleParam.IdSchedule,
                                      mebsScheduleParam.mebs_ingesta.Code_Package);

                MainForm.LogWarningToFile(msg);
                return null;
            }
            
            foreach (mebs_encapsulator item in assetEncapsulatorCollection)
            {
                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.AturaXML(nDataChannel);
                        break;
                    }
                    catch (Exception ex)
                    {
                        msg = string.Format(@"Tentative {0} To re-send AturaXML command to Asset encapsulator.
                                              IdSchedule = {1},
                                              Code_Package = {2}.
                                              ",
                                              iMaxTry,
                                              mebsScheduleParam.IdSchedule,
                                              mebsScheduleParam.mebs_ingesta.Code_Package);

                        iMaxTry--;
                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }

            return reply;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mebsScheduleParam"></param>
        /// <returns></returns>
        private static XDocument FromMebsScheduleToLightMPEG7CommandXDocument(mebs_schedule mebsScheduleParam)
        {
            XDocument xDocumentItem = null;
            try
            {
                DateTime datacastingXmlCommandStartDateTime = mebsScheduleParam.Estimated_Start.Value; //.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET);
                DateTime datacastingXmlCommandStopDateTime = mebsScheduleParam.Estimated_Stop.Value;
                int? contentID = mebsScheduleParam.ContentID.Value;
                TimeSpan expiresAfter = (mebsScheduleParam.mebs_ingesta.Expiration_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                 default(TimeSpan) :
                                 mebsScheduleParam.mebs_ingesta.Expiration_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                 );
                TimeSpan immortalDuring = (mebsScheduleParam.mebs_ingesta.Immortality_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                           default(TimeSpan) :
                                           mebsScheduleParam.mebs_ingesta.Immortality_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                           );

                XElement extendedInfo = (mebsScheduleParam.mebs_ingesta_advertisement_mapping.Count > 0 ?
                                                      new XElement("AdvertisementDescription",
                                                                    new XAttribute("version", "1.0"),
                                                                    new XElement("AdPlaylist",
                                                                                  new XAttribute("name", "OnPlay"),
                                                                                  from mebs_ingesta_advertisement_mapping advOccurrence
                                                                                  in mebsScheduleParam.mebs_ingesta.mebs_ingesta_advertisement_mapping.ToList()
                                                                                  select new XElement("AdInsertion",
                                                                                                       new XElement("StartTime", advOccurrence.StartTimePoint),
                                                                                                       new XElement("ContentId", contentID.ToString()),
                                                                                                       new XElement("MaxFwdSpeed", advOccurrence.MaxFwdSpeed.Value.ToString()),
                                                                                                       new XElement("MaxRwdSpeed", advOccurrence.MaxRwdSpeed.Value.ToString()),
                                                                                                       new XElement("CanSkip", advOccurrence.CanSkip.Value ?
                                                                                                                               "yes" :
                                                                                                                               "no")
                                                                                                       )
                                                                                  )
                                                                      )

                                           :
                                           new XElement("AdvertisementDescription", new XAttribute("version", "1.0"), string.Empty));

                xDocumentItem =
                  new XDocument
                  (
                  new XDeclaration("1.0", "utf-8", "no"),
                  new XElement("DCInfo",
                                new XAttribute("version", "1.0"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/PVODCmd_schema.xsd"),
                                new XElement("Start",
                                               new XElement("OnAssociatedInfoReceptionActions", null)
                                              ),
                                new XElement("Stop",
                                               new XElement("OnSuccessActions", null),
                                               new XElement("OnErrorActions", null)
                                              ),
                                new XElement("BesTVInfo",
                                               new XAttribute("version", DefaultValues.BESTVINFO_VERSION),
                                               new XElement("ContentId", contentID.Value.ToString()),
                                               new XElement("BroadcasterId", BroadcasterID),
                                               new XElement("Template", null),
                                               new XElement("Type", Convert.ToInt32(MediaType.DC_CHANNEL).ToString()),
                                               new XElement("Queue", "2"),
                                               new XElement("Name", mebsScheduleParam.mebs_ingesta.Title),
                                               new XElement("ParentalRating", mebsScheduleParam.mebs_ingesta.ParentalRating),
                                               from mebs_ingesta_category_mapping mebsCategoryMappingOccurrence
                                               in mebsScheduleParam.mebs_ingesta.mebs_ingesta_category_mapping.ToList()
                                               select new XElement("Category", MEBSCatalogProvider.GetCategoryByID(mebsCategoryMappingOccurrence.IdCategory).Value),
                                               new XElement("Hidden", mebsScheduleParam.mebs_ingesta.Hidden.Value.ToString()),
                                               new XElement("PublishAfter", mebsScheduleParam.mebs_ingesta.PublishAfter.Value.ToString()),
                                               new XElement("ExpiresAfter", ((int)expiresAfter.TotalMinutes).ToString()),
                                               new XElement("ImmortalDuring", ((int)immortalDuring.TotalMinutes).ToString()),
                                               new XElement("MinLifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinLifeAfterFirstAccess.Value.ToString()),
                                               new XElement("LifeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.LifeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinLifeAfterActivation", mebsScheduleParam.mebs_ingesta.MinLifeAfterActivation.Value.ToString()),
                                               new XElement("LifeAfterActivation", mebsScheduleParam.mebs_ingesta.LifeAfterActivation.Value.ToString()),
                                               new XElement("DisableAccess", mebsScheduleParam.mebs_ingesta.DisableAccess.Value.ToString()),
                                               new XElement("ActiveSince", FormatActiveSinceInputValue(mebsScheduleParam.mebs_ingesta.ActiveSince)),
                                               new XElement("ActiveDuring", mebsScheduleParam.mebs_ingesta.ActiveDuring.Value.ToString()),
                                               new XElement("ActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.ActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MinActiveTimeAfterFirstAccess", mebsScheduleParam.mebs_ingesta.MinActiveTimeAfterFirstAccess.Value.ToString()),
                                               new XElement("MaxAccesses", mebsScheduleParam.mebs_ingesta.MaxAccesses.Value.ToString()),
                                               new XElement("DrmProtected", mebsScheduleParam.mebs_ingesta.DrmProtected.Value.ToString()),
                                               new XElement("CopyControl", mebsScheduleParam.mebs_ingesta.CopyControl),
                                               new XElement("PreservationPriority", mebsScheduleParam.mebs_ingesta.PreservationPriority),
                                               new XElement("ContentFileSizeInMB", Utils.ConvertByteToMegaByte((long)mebsScheduleParam.mebs_ingesta.MediaFileSizeAfterRedundancy).ToString()),
                                               new XElement("ExtendedInfo",
                                                             from mebs_ingestadetails mebsingestadetailOccurrence
                                                             in mebsScheduleParam.mebs_ingesta.mebs_ingestadetails
                                                             select new XElement(mebsingestadetailOccurrence.DetailsName,
                                                                                 new XAttribute("target", "nlscore"),
                                                                                 new XAttribute("visibility", string.IsNullOrEmpty(mebsingestadetailOccurrence.DetailsValue) ?
                                                                                                                                   false.ToString() :
                                                                                                                                   true.ToString()
                                                                                               ),
                                                                                 mebsingestadetailOccurrence.DetailsValue
                                                                                 ),
                                                             extendedInfo,
                                                             new XElement("Cover", (mebsScheduleParam.mebs_ingesta != null && mebsScheduleParam.mebs_ingesta.Poster != null ? Convert.ToBase64String(mebsScheduleParam.mebs_ingesta.Poster, 0, mebsScheduleParam.mebs_ingesta.Poster.Length) : string.Empty))
                                                            )
                                               )
                               )
                  );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }
            return xDocumentItem ?? null;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_listAvailableCategories">All available Categories</param>
        /// <returns>XDocument</returns>
        private static XDocument FromMebsCategoryToXDocument(List<mebs_category> _listAvailableCategories)
        {
            XDocument xDocumentItem = null;
            try
            {
                xDocumentItem =
                  new XDocument
                  (
                  new XDeclaration("1.0", "utf-8", "no"),
                  new XElement("Categorization",
                                new XAttribute("version", "1.1"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/Category_schema.xsd"),

                                /* Based on Robert request : adding the duplication of the Categorization tag.
                                 useless information but still needed for latest latest version of lib (CLIENT SIDE) */

                                new XElement("Categorization",
                                new XAttribute("version", "1.1"),
                                new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
                                new XAttribute(XNamespace.Get("http://www.w3.org/2001/XMLSchema-instance") + "noNamespaceSchemaLocation", "../xsd/Category_schema.xsd"),


                                /* the duplication of the Categorization tag End. */
                                
                                from mebs_category _cat
                                in _listAvailableCategories
                                select new XElement("Cat",
                                                     new XAttribute("default", _cat.Default.Value.ToString()),
                                                     new XAttribute("visible", _cat.Visibility),
                                                     new XElement("Id", _cat.Value),
                                                     from mebs_category_language_mapping _catMapping in
                                                     _cat.mebs_category_language_mapping
                                                     select
                                                     new XElement("Name",
                                                         new XAttribute("lang", "ENG"),
                                                         _catMapping.Title
                                                         )
                                                    )

                               )
                               )
                  );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            return xDocumentItem ?? null;
        }

        private static string SendCategorizationFile(List<mebs_category> _listAvailableCategories, DateTime? _dtLastSendCategory)
        {
            string reply = string.Empty;
            byte iMaxTry = 5;
            var exists = default(Boolean);


            DateTime dt = DateTimeUtils.ConvertValueToDateTime(_dtLastSendCategory);
            string _categorization_FileName = string.Format("{0}_{1}{2}",
                                              DefaultValues.CategorizationFileName,
                                              DateTimeUtils.DateTime_UnixTimestamp(dt).ToString(),
                                              DefaultValues.XML_EXTENSION);


            XDocument xDocumentItem = FromMebsCategoryToXDocument(_listAvailableCategories);
            if (xDocumentItem == null)
            {
                MainForm.LogErrorToFile(@"Attempt to send invalid Categorization file to Metadata encapsulator (either null or empty).");
                return SendCategoryStatus.NAK.ToString(); ;
            }

            CreateFileIntoFtpServerRepository(xDocumentItem,
                                         _categorization_FileName);

            exists = CheckFileCreation(_categorization_FileName);

            if (!exists)
            {
                MainForm.LogWarningToFile(string.Format("The File '{0}' Cannot be found", _categorization_FileName));
                return SendCategoryStatus.NAK.ToString();
            }

            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

            List<mebs_encapsulator> metadataEncapsulatorCollection = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
            if (metadataEncapsulatorCollection == null
                || metadataEncapsulatorCollection.Count <= 0)
            {
                MainForm.LogWarningToFile("Send Categorization command to encapsulator failed (No Metadata encapsulator found).");
                return FuturDCListStatus.NAK.ToString();
            }

            MMPCProxyProvider encapsulatorWSProxy = null;
            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {
                encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.EnviaXML(_categorization_FileName,
                            CATEGORIZATION_XML_CANAL_ENVIAMENT,
                            NLB_FTP_IPAddress,
                            IngestedMedia_NLB_FTP_Relative_URI,
                            NLB_FTP_UserName,
                            NLB_FTP_UserPassword);
                        break;
                    }
                    catch (Exception ex)
                    {
                        iMaxTry--;

                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }

            return (reply == DefaultValues.WS_APP_RESULT_OK ? SendCategoryStatus.ACK.ToString() : SendCategoryStatus.NAK.ToString());
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Public Method(s) -.-.-.-.-.-.-.-.-.-.-

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string SendAutomaticRecordingStartCommand(EpgEntry epgEntryParam,
                                                                CatchupTVCommandType commandType) {
            try {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        SendAutomaticRecordingStartCommand(mebsScheduleParam,
                                                           epgEntryParam.LastHarrisTrigger,
                                                           commandType) :
                        null);
            }
            catch (Exception ex) {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exactEpgTriggerStartTime"></param>
        /// <returns></returns>
        public static bool IsAutomaticTrigger(DateTime exactEpgTriggerStartTime)
        {
            return (exactEpgTriggerStartTime.CompareTo(DateTime.UtcNow.AddMinutes(DefaultValues.AUTO_RECORDING_AUTOMATIC_OFFSET)) > 0 ?
                    true :
                    false);
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string SendARExtendedCommand(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        SendARExtendedCommand(mebsScheduleParam, AR_EXTENDED_XML_CANAL_ENVIAMENT) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static bool UploadARExtendedFile(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        UploadARExtendedFile(mebsScheduleParam) :
                        false);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string SendAutomaticRecordingStopCommand(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        SendAutomaticRecordingStopCommand(mebsScheduleParam) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string SendFuturSchedulesEntitiesCommand(List<mebs_schedule> _FuturSchedulesParam, DateTime? _dtLastSendFuturList)
        {
            XDocument xDocumentItem = null;
            try
            {
                //--- Create a XDocument <DCContentList>
                xDocumentItem =
                   new XDocument
                   (
                       new XDeclaration("1.0", "utf-8", "no"),
                       new XElement
                       ("DCContentList", //------ Create a new XDocument <DCContent>
                         from s in _FuturSchedulesParam
                         select
                             new XElement
                             (
                                new XElement
                                 ("DCContent", 
                                     new XElement
                                     (
                                     FromMebsScheduleToDataCastCommandXDocument(s).Root
                                     ),
                                     new XElement
                                     (
                                     FromMebsScheduleToLightMPEG7CommandXDocument(s).Root
                                     )

                                 )
                             )
                     )
                 );
                if (xDocumentItem == null) return FuturDCListStatus.NAK.ToString();
                return SendFuturSchedulesEntitiesCommand(xDocumentItem, _dtLastSendFuturList);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return FuturDCListStatus.NAK.ToString();
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_XDocFuturList"></param>
        /// <returns></returns>
        public static string SendFuturSchedulesEntitiesCommand(XDocument _XDocFuturList, DateTime? _dtLastSendFuturList)
        {
            string reply = string.Empty;
            string _futurDCList_FileName = string.Empty;
          
            try
            {
                byte iMaxTry = 5;
                var exists = default(Boolean);

                DateTime dt = DateTimeUtils.ConvertValueToDateTime(_dtLastSendFuturList);

                _futurDCList_FileName = string.Format("{0}_{1}{2}",
                                                              DefaultValues.futureDCListFileName,
                                                              DateTimeUtils.DateTime_UnixTimestamp(dt).ToString(),
                                                              DefaultValues.XML_EXTENSION);

                CreateFileIntoFtpServerRepository(_XDocFuturList,
                                             _futurDCList_FileName);

               
                exists = CheckFileCreation(_futurDCList_FileName);

               

                if (!exists)
                {
                    MainForm.LogWarningToFile(string.Format("The File '{0}' Cannot be found", _futurDCList_FileName));
                    return FuturDCListStatus.NAK.ToString();
                }

                var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

                List<mebs_encapsulator> metadataEncapsulatorCollection = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
                if (metadataEncapsulatorCollection == null
                    || metadataEncapsulatorCollection.Count <= 0)
                {
                    MainForm.LogWarningToFile("Send FuturDCList command to encapsulator failed (No Metadata encapsulator found).");
                    Utils.FileDelete(_futurDCList_FileName); // Remove created file.
                    return FuturDCListStatus.NAK.ToString();
                }

                MMPCProxyProvider encapsulatorWSProxy = null;
                foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
                {
                    encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);

                    do
                    {
                        try
                        {
                            reply = encapsulatorWSProxy.EnviaXML(_futurDCList_FileName,
                                FUTUR_DC_LIST_XML_CANAL_ENVIAMENT,
                                NLB_FTP_IPAddress,
                                IngestedMedia_NLB_FTP_Relative_URI,
                                NLB_FTP_UserName,
                                NLB_FTP_UserPassword);
                            break;
                        }
                        catch (Exception ex)
                        {
                            iMaxTry--;

                           //MainForm.LogExceptionToFile(ex);
                           Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                           continue;
                        }
                    } 
                    //while (iMaxTry > 0 &&
                    //         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
                    while (iMaxTry > 0) ;
                }
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            if (string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0) { Utils.FileDelete(_futurDCList_FileName); }
           
            return (reply == DefaultValues.WS_APP_RESULT_OK ? FuturDCListStatus.ACK.ToString() : FuturDCListStatus.NAK.ToString());
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static bool UploadMPEG7File(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        UploadMPEG7File(mebsScheduleParam) :
                        false);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }


        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static bool UploadDCCommandFile(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        UploadDCCommandFile(mebsScheduleParam) :
                        false);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }


       
        /// <summary>
        ///  Generate DataCasting XML Command
        /// </summary>
        /// <returns>In XML Format</returns>
        public static string FromMebsScheduleToDataCastCommandString(mebs_schedule mebsScheduleParam)
        {
            StringBuilder sb = new StringBuilder();
            StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            try
            {

                DateTime datacastingXmlCommandStartDateTime = mebsScheduleParam.Estimated_Start.Value.Add(DefaultValues.DC_XML_COMMAND_STARTTIME_OFFSET);
                DateTime datacastingXmlCommandStopDateTime = mebsScheduleParam.Estimated_Stop.Value;
                string startTriggerTypeElement,
                       stopTriggerTypeElement;
                startTriggerTypeElement =
                stopTriggerTypeElement = DefaultValues.TV_CHANNEL_TRIGGER_AUTOMATIC;
                int? contentID = mebsScheduleParam.ContentID.Value;
                TimeSpan expiresAfter = (mebsScheduleParam.mebs_ingesta.Expiration_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                 default(TimeSpan) :
                                 mebsScheduleParam.mebs_ingesta.Expiration_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                 );
                TimeSpan immortalDuring = (mebsScheduleParam.mebs_ingesta.Immortality_time.Value.CompareTo(new DateTime(1971, 11, 6, 23, 59, 59)) == 0 ?
                                           default(TimeSpan) :
                                           mebsScheduleParam.mebs_ingesta.Immortality_time.Value.Subtract(datacastingXmlCommandStartDateTime)
                                           );


                XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

                //---- Set the XmlWriterSettings to the XMLWriter
                xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);


                xmlWriter.WriteStartDocument(false);
                xmlWriter.WriteStartElement("RecCmd");
                xmlWriter.WriteAttributeString("version", "2.2");

                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/PVODCmd_schema.xsd");


                // Priority 
                xmlWriter.WriteStartElement("Priority");
                xmlWriter.WriteString("1");
                xmlWriter.WriteEndElement();
                var VODChannel = from mebs_channeltuning mebsChannelTuningOccurrence
                                                in MEBSCatalogProvider.ChannelDVBTripletCollection(mebsScheduleParam.mebs_ingesta.mebs_channel.IdChannel)
                                 select mebsChannelTuningOccurrence;
                if (VODChannel != null && VODChannel.Count() > 0)
                {
                    foreach (var item in VODChannel)
                    {
                        //Service 
                        xmlWriter.WriteStartElement("Service");
                        xmlWriter.WriteAttributeString("type", "dvb");

                        //<SID></SID>
                        xmlWriter.WriteStartElement("SID");
                        xmlWriter.WriteString(item.ServiceID.ToString());
                        xmlWriter.WriteEndElement();

                        //<ONID></ONID>
                        xmlWriter.WriteStartElement("ONID");
                        xmlWriter.WriteString(item.OriginalNetworkID.ToString());
                        xmlWriter.WriteEndElement();

                        //<TSID></TSID>
                        xmlWriter.WriteStartElement("TSID");
                        xmlWriter.WriteString(item.TransportStreamID.ToString());
                        xmlWriter.WriteEndElement();
                        //</Service>
                        xmlWriter.WriteEndElement();
                    }
                }

                //<Source> 
                xmlWriter.WriteStartElement("Source");
                //<Type></Type>
                xmlWriter.WriteStartElement("Type");
                xmlWriter.WriteString("tuner");
                xmlWriter.WriteEndElement();

                //<Path></Path>
                xmlWriter.WriteStartElement("Path");
                xmlWriter.WriteString("2");
                xmlWriter.WriteEndElement();
                //</Source> 
                xmlWriter.WriteEndElement();

                //<Start> 
                xmlWriter.WriteStartElement("Start");
                //<Trigger></Trigger>
                xmlWriter.WriteStartElement("Trigger");
                xmlWriter.WriteString(startTriggerTypeElement);
                xmlWriter.WriteEndElement();

                //<Date></Date>
                xmlWriter.WriteStartElement("Date");
                xmlWriter.WriteString(DateTimeUtils.GenerateStringDate(datacastingXmlCommandStartDateTime));
                xmlWriter.WriteEndElement();

                //<Time></Time>
                xmlWriter.WriteStartElement("Time");
                xmlWriter.WriteString(DateTimeUtils.GenerateStringTime(datacastingXmlCommandStartDateTime));
                xmlWriter.WriteEndElement();

                //<PreliminaryActions>
                xmlWriter.WriteStartElement("PreliminaryActions");

                //</PreliminaryActions> 
                xmlWriter.WriteEndElement();

                //<OnSuccessActions>
                xmlWriter.WriteStartElement("OnSuccessActions");

                //</OnSuccessActions> 
                xmlWriter.WriteEndElement();

                //<OnErrorActions>
                xmlWriter.WriteStartElement("OnErrorActions");

                //</OnErrorActions> 
                xmlWriter.WriteEndElement();
                //</Start> 
                xmlWriter.WriteEndElement();

                //<Stop> 
                xmlWriter.WriteStartElement("Stop");
                //<Trigger></Trigger> 
                xmlWriter.WriteStartElement("Trigger");
                xmlWriter.WriteString(stopTriggerTypeElement);
                xmlWriter.WriteEndElement();

                //<Date></Date> 
                xmlWriter.WriteStartElement("Date");
                xmlWriter.WriteString(DateTimeUtils.GenerateStringDate(datacastingXmlCommandStopDateTime));
                xmlWriter.WriteEndElement();

                //<Time></Time>
                xmlWriter.WriteStartElement("Time");
                xmlWriter.WriteString(DateTimeUtils.GenerateStringTime(datacastingXmlCommandStopDateTime));
                xmlWriter.WriteEndElement();

                //<OnSuccessActions>
                xmlWriter.WriteStartElement("OnSuccessActions");

                //</OnSuccessActions> 
                xmlWriter.WriteEndElement();

                //<OnErrorActions> 
                xmlWriter.WriteStartElement("OnErrorActions");

                //</OnErrorActions>
                xmlWriter.WriteEndElement();

                //<OnCancelActions>
                xmlWriter.WriteStartElement("OnCancelActions");

                //</OnCancelActions>
                xmlWriter.WriteEndElement();
                //</Stop> 
                xmlWriter.WriteEndElement();

                //<BesTVInfo>
                xmlWriter.WriteStartElement("BesTVInfo");
                xmlWriter.WriteAttributeString("version",  DefaultValues.BESTVINFO_VERSION);

                //<ContentId>
                xmlWriter.WriteStartElement("ContentId");
                xmlWriter.WriteString(contentID.Value.ToString());
                //</ContentId>
                xmlWriter.WriteEndElement();

                //<BroadcasterId>
                xmlWriter.WriteStartElement("BroadcasterId");
                xmlWriter.WriteString(BroadcasterID);
                //</BroadcasterId>
                xmlWriter.WriteEndElement();


                //<Template>
                xmlWriter.WriteStartElement("Template");
                xmlWriter.WriteString(string.Empty);
                //</Template>
                xmlWriter.WriteEndElement();


                //<Type>
                xmlWriter.WriteStartElement("Type");
                xmlWriter.WriteString(Convert.ToInt32(MediaType.DC_CHANNEL).ToString());
                //</Type>
                xmlWriter.WriteEndElement();


                //<Queue>
                xmlWriter.WriteStartElement("Queue");
                xmlWriter.WriteString("2");
                //</Queue>
                xmlWriter.WriteEndElement();


                //<Name>
                xmlWriter.WriteStartElement("Name");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.Title);
                //</Name>
                xmlWriter.WriteEndElement();


                //<ParentalRating>
                xmlWriter.WriteStartElement("ParentalRating");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.ParentalRating);
                //</ParentalRating>
                xmlWriter.WriteEndElement();

                var listCategories = from mebs_ingesta_category_mapping mebsCategoryMappingOccurrence
                                               in mebsScheduleParam.mebs_ingesta.mebs_ingesta_category_mapping.ToList()
                                     select mebsCategoryMappingOccurrence;

                if (listCategories != null && listCategories.Count() > 0)
                {
                    //<Category>
                    foreach (mebs_ingesta_category_mapping catMapping in listCategories)
                    {
                        xmlWriter.WriteStartElement("Category");
                        xmlWriter.WriteString(MEBSCatalogProvider.GetCategoryByID(catMapping.IdCategory).Value);
                        xmlWriter.WriteEndElement();
                    }
                    //</Category>
                }

                //<Hidden>
                xmlWriter.WriteStartElement("Hidden");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.Hidden.Value.ToString());
                //</Hidden>
                xmlWriter.WriteEndElement();


                //<PublishAfter>
                xmlWriter.WriteStartElement("PublishAfter");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.PublishAfter.Value.ToString());
                //</PublishAfter>
                xmlWriter.WriteEndElement();


                //<ExpiresAfter>
                xmlWriter.WriteStartElement("ExpiresAfter");
                xmlWriter.WriteString(((int)expiresAfter.TotalMinutes).ToString());
                //</ExpiresAfter>
                xmlWriter.WriteEndElement();


                //<ImmortalDuring>
                xmlWriter.WriteStartElement("ImmortalDuring");
                xmlWriter.WriteString(((int)immortalDuring.TotalMinutes).ToString());
                //</ImmortalDuring>
                xmlWriter.WriteEndElement();


                //<MinLifeAfterFirstAccess>
                xmlWriter.WriteStartElement("MinLifeAfterFirstAccess");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.MinLifeAfterFirstAccess.Value.ToString());
                //</MinLifeAfterFirstAccess>
                xmlWriter.WriteEndElement();


                //<LifeAfterFirstAccess>
                xmlWriter.WriteStartElement("LifeAfterFirstAccess");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.LifeAfterFirstAccess.Value.ToString());
                //</LifeAfterFirstAccess>
                xmlWriter.WriteEndElement();


                //<MinLifeAfterActivation>
                xmlWriter.WriteStartElement("MinLifeAfterActivation");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.MinLifeAfterActivation.Value.ToString());
                //</MinLifeAfterActivation>
                xmlWriter.WriteEndElement();

                //<LifeAfterActivation>
                xmlWriter.WriteStartElement("LifeAfterActivation");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.LifeAfterActivation.Value.ToString());
                //</LifeAfterActivation>
                xmlWriter.WriteEndElement();

                //<DisableAccess>
                xmlWriter.WriteStartElement("DisableAccess");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.DisableAccess.Value.ToString());
                //</DisableAccess>
                xmlWriter.WriteEndElement();


                //<ActiveSince>
                xmlWriter.WriteStartElement("ActiveSince");
                xmlWriter.WriteString(FormatActiveSinceInputValue(mebsScheduleParam.mebs_ingesta.ActiveSince));
                //</ActiveSince>
                xmlWriter.WriteEndElement();


                //<ActiveDuring>
                xmlWriter.WriteStartElement("ActiveDuring");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.ActiveDuring.Value.ToString());
                //</ActiveDuring>
                xmlWriter.WriteEndElement();


                //<ActiveTimeAfterFirstAccess>
                xmlWriter.WriteStartElement("ActiveTimeAfterFirstAccess");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.ActiveTimeAfterFirstAccess.Value.ToString());
                //</ActiveTimeAfterFirstAccess>
                xmlWriter.WriteEndElement();

                //<MinActiveTimeAfterFirstAccess>
                xmlWriter.WriteStartElement("MinActiveTimeAfterFirstAccess");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.MinActiveTimeAfterFirstAccess.Value.ToString());
                //</MinActiveTimeAfterFirstAccess>
                xmlWriter.WriteEndElement();


                //<MaxAccesses>
                xmlWriter.WriteStartElement("MaxAccesses");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.MaxAccesses.Value.ToString());
                //</MaxAccesses>
                xmlWriter.WriteEndElement();


                //<DrmProtected>
                xmlWriter.WriteStartElement("DrmProtected");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.DrmProtected.Value.ToString());
                //</DrmProtected>
                xmlWriter.WriteEndElement();


                //<CopyControl>
                xmlWriter.WriteStartElement("CopyControl");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.CopyControl);
                //</CopyControl>
                xmlWriter.WriteEndElement();

                //<PreservationPriority>
                xmlWriter.WriteStartElement("PreservationPriority");
                xmlWriter.WriteString(mebsScheduleParam.mebs_ingesta.PreservationPriority.ToString());
                //</PreservationPriority>
                xmlWriter.WriteEndElement();

                //<ContentFileSizeInMB>
                xmlWriter.WriteStartElement("ContentFileSizeInMB");
                xmlWriter.WriteString(Utils.ConvertByteToMegaByte((long)mebsScheduleParam.mebs_ingesta.MediaFileSizeAfterRedundancy).ToString());
                //</ContentFileSizeInMB>
                xmlWriter.WriteEndElement();

                //<ExtendedInfo></ExtendedInfo>
                xmlWriter.WriteStartElement("ExtendedInfo");
                xmlWriter.WriteEndElement();

                //</BesTVInfo>
                xmlWriter.WriteEndElement();

                //</RecCmd>
                xmlWriter.WriteEndElement();


                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
            }

            return stringWriter.ToString();
        }

        
        /// <summary>
        /// Save a XML String to a File
        /// </summary>
        /// <param name="xml">String XML Format</param>
        /// <param name="path">Path of the File</param>
        public static bool CreateFileIntoFtpServerRepository(string xml, string fileName)
        {
            try
            {
                string mpeg7FullPath = string.Format("{0}{1}{2}", MainForm.Conf.ReqDirectory, DefaultValues.IngestedMedia_Directory, fileName);

                using (StreamWriter sw = new StreamWriter(mpeg7FullPath, false, UTF8Encoding.UTF8))
                {
                    sw.Write(xml);
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string SendDatacastCommand(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        SendDatacastCommand(mebsScheduleParam, XML_CANAL_ENVIAMENT) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// Save a XML String ot a File
        /// </summary>
        /// <param name="xml">String XML Format</param>
        /// <param name="folderFullPath">Path of the Folder</param>
        /// <param name="fileFullPath">Path of the File</param>
        public static bool XMLStringToXmlFile(string xml, string folderFullPath, string fileFullPath)
        {
            try
            {
                if (!Directory.Exists(folderFullPath))
                    Directory.CreateDirectory(folderFullPath);

                using (StreamWriter sw = new StreamWriter(fileFullPath, false, UTF8Encoding.UTF8))
                {
                    sw.Write(xml);
                    sw.Close();
                }
                return true;
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string CreateEncapsulatorMPEG7XmlFile(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        CreateEncapsulatorMPEG7XmlFile(mebsScheduleParam) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string DeleteMPEG7XmlFile(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        DeleteMPEG7XmlFile(mebsScheduleParam) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epgEntryParam"></param>
        /// <returns></returns>
        public static string DeleteDatacastCommand(EpgEntry epgEntryParam)
        {
            try
            {
                mebs_schedule mebsScheduleParam = MEBSCatalogProvider.GetScheduleEntityTreeByScheduleID(int.Parse(epgEntryParam.ID));
                return (mebsScheduleParam != null ?
                        DeleteDatacastCommand(mebsScheduleParam, XML_CANAL_ENVIAMENT) :
                        null);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string TryToAttachDataThroughFistXMLChannel(string data)
        {
            string msg = string.Empty;
            string reply = string.Empty;
            byte iMaxTry = 5;

            if (string.IsNullOrEmpty(data))
            {
                MainForm.LogErrorToFile("Try To Attach Data Through Fist XML Channel : No data attached.");
                return null;
            }

            var proxy = new mebsEntities(Configuration.MTVCatalogLocation);

            List<mebs_encapsulator> metadataEncapsulatorCollection = proxy.mebs_encapsulator.ToList().Where(mebsEncapsulatorParam => string.Compare(mebsEncapsulatorParam.Type, EncapsulatorType.Metadata.ToString()) == 0).ToList();
            if (metadataEncapsulatorCollection == null || metadataEncapsulatorCollection.Count <= 0)
            {
                msg = string.Format(@"Sending {0} Through Fist XML Channel failed (No Metadata encapsulator found)).", data);
                MainForm.LogWarningToFile(msg);
                return null;
            }

            
            foreach (mebs_encapsulator item in metadataEncapsulatorCollection)
            {
                MMPCProxyProvider encapsulatorWSProxy = new MMPCProxyProvider(item.IpAddress);
                do
                {
                    try
                    {
                        reply = encapsulatorWSProxy.EnviaXML(data,
                                                             AR_Cover_Channel_Sending,
                                                             NLB_FTP_IPAddress,
                                                             IngestedMedia_NLB_FTP_Relative_URI,
                                                             NLB_FTP_UserName,
                                                             NLB_FTP_UserPassword);
                        break;
                    }
                    catch (Exception ex)
                    {
                        //-----.msg = string.Format(@"Tentative {0} To send File Through Fist XML Channel.", iMaxTry);
                        iMaxTry--;
                        MainForm.LogExceptionToFile(ex);
                        Thread.Sleep((int)TimeSpan.FromMilliseconds(500).TotalMilliseconds);
                        continue;
                    }
                } while (iMaxTry > 0 &&
                         string.Compare(reply, DefaultValues.WS_APP_RESULT_OK) != 0);
            }

            return reply;
        }

        public static string SendCategorizationEntitiesCommand(List<mebs_category> _categoryCollection, DateTime? _dtLastSendCategory)
        {
            try
            {
                return SendCategorizationFile(_categoryCollection, _dtLastSendCategory);
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return SendCategoryStatus.NAK.ToString();
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Auxiliary Method(s) -.-.-.-.-.-.-.-.-.-.-
        private static string FormatActiveSinceInputValue(string activeSinceValue) {
            try
            {
                return (string.IsNullOrEmpty(activeSinceValue) ?
                        null :
                        DateTime.Parse(activeSinceValue).ToString("dd/mm/yyyy hh:mm:ss")
                       );
            }
            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return null;
            }
        }
        #endregion 
    

    }
}
