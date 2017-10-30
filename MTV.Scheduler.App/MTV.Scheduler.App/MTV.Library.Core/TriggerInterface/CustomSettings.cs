// ACK :

#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
//using CMEScheduler;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomSettings
    {
        #region Variable(s)
        /// <summary>
        /// 
        /// </summary>
        public const string xmlValidatorSettingsFileName = "CMEScheduler.xml";
        #endregion 

        #region private static fields
        private static bool mbIsMonoFramework;
        private static string msExecutablePath;
        #endregion

        #region private properties
        /// <summary>
        /// 
        /// </summary>
        public static string ConfigurationFileName
        {            
            get 
            { 
                string xmlValidatorSettingsFilePath = Path.Combine(ExecutablePath, xmlValidatorSettingsFileName);
                if (!File.Exists(xmlValidatorSettingsFilePath))
                {
                    //Connection.WriteCustomErrorToRepository(
                        //new IOException(string.Format("Cannot validate Harris UDP packet message since \"{0}\" file not found in the specified location.", xmlValidatorSettingsFilePath))
                       // );
                }
                return xmlValidatorSettingsFilePath;
            }            
        }
        #endregion

        #region public properties
        /// <summary>
        /// 
        /// </summary>
        public static bool IsMonoFramework
        {
            get 
            { 
                return mbIsMonoFramework; 
            }
            set
            {
                //Check for Framework type: it needs to perform conditional execution for Mono/MSDotNet differences
                string sFrameworkDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
                mbIsMonoFramework = sFrameworkDir.Contains("mono");  
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AutoIndentDocuments
        {
            get
            {
                string sValue = LoadSetting("Editor/AutoIndentDocuments");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }

            set
            {
                SaveSetting("Editor/AutoIndentDocuments", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int IndentationCharsNumber
        {
            get
            {
                return Int32.Parse(LoadSetting("Editor/IndentationCharsNumber"));
            }
            set
            {
                SaveSetting("Editor/IndentationCharsNumber", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool HighlightSintax
        {
            get
            {
                string sValue = LoadSetting("Editor/HighlightSintax");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }
            set
            {
                SaveSetting("Editor/HighlightSintax", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool XsdProcessIdentityConstraints
        {
            get
            {
                string sValue = LoadSetting("SchemaXsd/XsdProcessIdentityConstraints");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }
            set
            {
                SaveSetting("SchemaXsd/XsdProcessIdentityConstraints", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool XsdProcessSchemaLocation
        {
            get
            {
                string sValue = LoadSetting("SchemaXsd/XsdProcessSchemaLocation");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }
            set
            {
                SaveSetting("SchemaXsd/XsdProcessSchemaLocation", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool XsdReportValidationWarnings
        {
            get
            {
                string sValue = LoadSetting("SchemaXsd/XsdReportValidationWarnings");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }
            set
            {
                SaveSetting("SchemaXsd/XsdReportValidationWarnings", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static int LogsTypeId
        {
            get
            {
                string sValue = LoadSetting("LogManager/LogsTypeId");
                return Int32.Parse(sValue);
            }
            set
            {
                SaveSetting("LogManager/LogsTypeId", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string LogDirectoryPath
        {
            get
            {
                string sValue = LoadSetting("LogManager/LogDirectoryPath");
                return sValue;
            }
            set
            {
                SaveSetting("LogManager/LogDirectoryPath", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static string UDPMsgStatusDirectoryPath
        {
            get
            {
                string sValue = LoadSetting("LogManager/UDPMsgStatusDirectoryPath");
                return sValue;
            }
            set
            {
                SaveSetting("LogManager/UDPMsgStatusDirectoryPath", value.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static bool AutoGenerateXMLFile
        {
            get
            {
                string sValue = LoadSetting("LogManager/AutoGenerateXMLFile");

                if (!string.IsNullOrEmpty(sValue))
                    return Convert.ToBoolean(sValue);

                else
                    return true;
            }

            set
            {
                SaveSetting("LogManager/AutoGenerateXMLFile", value.ToString());
            }
        }

        /// <summary>
        /// BaseDirectory.
        /// </summary>
        public static string ExecutablePath
        {
            get
            {
                if (string.IsNullOrEmpty(msExecutablePath))
                {
                    FileInfo fi = new FileInfo(AppDomain.CurrentDomain.BaseDirectory);
                    msExecutablePath = fi.DirectoryName;
                    fi = null;
                }
                return msExecutablePath;
            }
        }
        #endregion

        #region private methods
        /// <summary>
        /// Load the stored setting in input.
        /// </summary>
        /// <param name="sSettingKey"></param>
        /// <returns></returns>
        private static string LoadSetting(string sSettingKey)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigurationFileName))
                {
                    return string.Empty;
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(ConfigurationFileName);
                return GetRoot(ref xmlDoc).SelectSingleNode(sSettingKey).InnerText;
            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Save the setting value in input.
        /// </summary>
        /// <param name="sSettingKey"></param>
        /// <param name="sValue"></param>
        public static void SaveSetting(string sSettingKey, string sValue)
        {
            try
            {
                if (string.IsNullOrEmpty(ConfigurationFileName))
                {
                    return;
                }

                XmlDocument xmlDoc = new XmlDocument();
                string sConfigFileName = ConfigurationFileName;

                xmlDoc.Load(sConfigFileName);
                XmlNode xmlRoot = GetRoot(ref xmlDoc);

                xmlRoot.SelectSingleNode(sSettingKey).InnerText = sValue;
                //Fix for Mono 2.0 for Windows: "FileMode.Create" doesn't clean the file before saving it
                File.Delete(sConfigFileName);

                FileStream fs = new FileStream(sConfigFileName, FileMode.Create);
                xmlDoc.Save(fs);
                fs.Close();
                fs = null;
                xmlDoc = null;
            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);
            }
        }

        /// <summary>
        /// Return the root of the configiration file.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        private static XmlNode GetRoot(ref XmlDocument xmlDoc)
        {
            try
            {
                if (xmlDoc.SelectSingleNode("/Settings") == null)
                {
                    XmlElement xmlElem = xmlDoc.CreateElement("Settings");
                    xmlDoc.AppendChild(xmlElem);
                }
                return xmlDoc.SelectSingleNode("/Settings");
            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);
                return null;
            }
        }
        #endregion
    }
}
