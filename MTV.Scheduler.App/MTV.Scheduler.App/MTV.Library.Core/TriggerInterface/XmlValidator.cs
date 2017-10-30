#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.CodeDom.Compiler;
//using CMEScheduler;
//using CMEScheduler.NS_LogManager;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// 
    /// </summary>
    public class XmlValidator
    {
        #region public static members
        public static List<SchemaValidationError> mListSchemaErrors;
        #endregion

        #region private methods
        /// <summary>
        /// Reset the list of errors
        /// </summary>
        private static void ResetListSchemaErrors()
        {
            if (mListSchemaErrors == null)
                mListSchemaErrors = new List<SchemaValidationError>();
            mListSchemaErrors.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sXmlDoc"></param>
        /// <param name="sXsdDoc"></param>
        /// <returns></returns>
        private static bool PerformValidation(
            string sXmlDoc,
            string sXsdDoc)
        {
            bool bValidationResult = false;

            if (string.IsNullOrEmpty(CustomSettings.ConfigurationFileName))
            {
                return false;
            }

            try
            {
                if (!string.IsNullOrEmpty(sXmlDoc) || !string.IsNullOrEmpty(sXsdDoc))
                {
                    sXmlDoc = sXmlDoc.Replace(Convert.ToChar(0x0).ToString(), "");
                    String SearchString = new string((char)26, 1);
                    sXmlDoc = sXmlDoc.Replace(SearchString, "");

                    ResetListSchemaErrors();

                    // Collection of temp files to store generated xml and xsd:
                    TempFileCollection tempFileCollection = new TempFileCollection();
                    tempFileCollection.KeepFiles = false;

                    string sXmlFileName = Path.GetTempFileName();
                    tempFileCollection.AddFile(sXmlFileName, false);
                    string sSchemaFileName = Path.GetTempFileName();
                    tempFileCollection.AddFile(sSchemaFileName, false);

                    // Fill XSD and XML temp files with parameters values:
                    File.WriteAllText(
                        sXmlFileName,
                        sXmlDoc);

                    File.WriteAllText(
                        sSchemaFileName,
                        sXsdDoc);

                    //Load xml
                    FileStream fs = new FileStream(sXmlFileName, FileMode.Open);
                    XmlReaderSettings xrs = null;

                    try
                    {
                        //Get the import list
                        XmlDocument xsdDoc = new XmlDocument();
                        xsdDoc.Load(sSchemaFileName);
                        XmlNodeList importList = xsdDoc.GetElementsByTagName("xs:import");
                        xsdDoc = null;


                        xrs = new XmlReaderSettings();
                        xrs.Schemas.Add(null, "file://" + sSchemaFileName);
                        List<string> lstTargetsNamespaces = new List<string>();

                        #region Code To Analyse

                        if (importList.Count > 0)
                        {
                            //Add the import list to the XmlSchemaSet
                            foreach (XmlNode importNode in importList)
                            {
                                string sImportSchema = importNode.Attributes["schemaLocation"].Value;
                                lstTargetsNamespaces.Add(importNode.Attributes["namespace"].Value);
                                StringBuilder sb = new StringBuilder();
                                sb.Append("file://");
                                //if (!Path.IsPathRooted(sImportSchema))
                                //{
                                //    //If the "schemaLocation" attribute contains a relative path, it completes the path
                                //    if (string.IsNullOrEmpty(sLibraryPath))
                                //    {
                                //        FolderBrowserDialog fbd = new FolderBrowserDialog();
                                //        if (fbd.ShowDialog() != DialogResult.Cancel)
                                //        {
                                //            sb.Append(Path.Combine(fbd.SelectedPath, sImportSchema));
                                //        }
                                //        else
                                //            throw new Exception("No library's directory selected. You need to select a directory for the library schema.");

                                //        fbd = null;
                                //    }
                                //    else
                                //    {
                                //        sb.Append(Path.Combine(sLibraryPath, sImportSchema));
                                //    }
                                //}                            
                                xrs.Schemas.Add(null, sb.ToString());
                                sb = null;
                            }
                        }

                        #endregion

                        xrs.ValidationType = ValidationType.Schema;

                        xrs.ValidationFlags = XmlSchemaValidationFlags.None;
                        if (CustomSettings.XsdReportValidationWarnings)
                            xrs.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
                        if (CustomSettings.XsdProcessIdentityConstraints)
                            xrs.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;
                        if (CustomSettings.XsdProcessSchemaLocation)
                            xrs.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;

                        xrs.ValidationEventHandler += new ValidationEventHandler(ValidationCallBack);

                        xrs.Schemas.Compile();

                        if (importList.Count > 0)
                        {
                            XmlSchema schema = null;
                            XmlSchema libSchema = null;
                            // Retrieve the compiled XmlSchema objects for the customer and
                            // address schema from the XmlSchemaSet by iterating over 
                            // the Schemas property.
                            foreach (string sNamespace in lstTargetsNamespaces)
                            {
                                foreach (XmlSchema sch in xrs.Schemas.Schemas())
                                {
                                    if (sch.TargetNamespace == sNamespace)
                                        libSchema = sch;
                                    else
                                        schema = sch;
                                }

                                // Create an XmlSchemaImport object, set the Namespace property
                                // to the namespace of the address schema, the Schema property 
                                // to the address schema, and add it to the Includes property
                                // of the customer schema.
                                XmlSchemaImport import = new XmlSchemaImport();
                                import.Namespace = sNamespace;

                                import.Schema = libSchema;
                                schema.TargetNamespace = null;  //take the default targetNamespace

                                schema.Includes.Add(import);
                            }

                            // Reprocess and compile the modified XmlSchema object 
                            // of the customer schema and write it to the console.    
                            xrs.Schemas.Reprocess(schema);
                            xrs.Schemas.Compile();
                        }

                    }
                    catch (Exception ex)
                    {
                        //Exception loading xsd schema.
                        mListSchemaErrors.Add(new SchemaValidationError(XmlSeverityType.Error, Convert.ToString(ex)));
                    }

                    try
                    {
                        XmlReader xr = XmlReader.Create(fs, xrs);
                        while (xr.Read()) ;
                        bValidationResult = (mListSchemaErrors.Count == 0);
                        xr = null;
                    }
                    catch (Exception ex1)
                    {
                        //Exception loading xml document
                        mListSchemaErrors.Add(new SchemaValidationError(XmlSeverityType.Error, Convert.ToString(ex1)));
                    }

                    fs.Close();
                    fs = null;
                    xrs = null;

                    //Delete temporary files
                    tempFileCollection.Delete();
                    tempFileCollection = null;
                }
                else
                {
                    mListSchemaErrors.Add(new SchemaValidationError(XmlSeverityType.Error, "Missing xml or xsd document."));
                }


            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);                
            }
            return bValidationResult;
        }

        /// <summary>
        /// Get HarrisXMLTrigger.xsd path
        /// </summary>
        /// <returns></returns>
        private static string GetPathName()
        {
            string Path = AppDomain.CurrentDomain.BaseDirectory;
            return String.Format(@"{0}\XSD\HarrisXMLTrigger.xsd", Path);
        }

        /// <summary>
        /// Convert HarrisXMLTrigger.xsd  to string.
        /// </summary>
        /// <returns>string</returns>
        private static string FromXsdToString()
        {
            try
            {
                string sXsdDoc = string.Empty;
                using (TextReader txtReader = new StreamReader(GetPathName()))
                {
                    sXsdDoc = txtReader.ReadToEnd();
                }
                return sXsdDoc;
            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);
                return string.Empty;
            }
        }
        #endregion

        #region callback methods
        /// <summary>
        /// Add giving error to the errors collection:       
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void ValidationCallBack(object sender, ValidationEventArgs args)
        {               
            mListSchemaErrors.Add(new SchemaValidationError(args.Severity, args.Message));            
        }
        #endregion

        #region public methods
        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static bool IsXmlStringIsWellFormated(string xmlString)
        {
            XmlTextReader xmlTxtReader = null;
            try
            {
                using (xmlTxtReader = new XmlTextReader(new StringReader(xmlString)))
                {
                    while (xmlTxtReader.Read()) { };
                    return true;
                }
            }
            catch (Exception ex)
            {
               // Connection.WriteErrorToRepository(ex);
                return false;
            }
            finally
            {
                if (xmlTxtReader != null)
                {
                    xmlTxtReader.Close();
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bAutoIndentDocuments"></param>
        /// <param name="bHighlightSintax"></param>
        /// <param name="bXsdProcessIdentityConstraints"></param>
        /// <param name="bXsdProcessSchemaLocation"></param>
        /// <param name="bXsdReportValidationWarnings"></param>
        public static void UpdateXmlValidatorSettings(
            bool bAutoIndentDocuments,
            bool bHighlightSintax,
            bool bXsdProcessIdentityConstraints,
            bool bXsdProcessSchemaLocation,
            bool bXsdReportValidationWarnings)
        {
            try
            {
                CustomSettings.AutoIndentDocuments = bAutoIndentDocuments;
                CustomSettings.HighlightSintax = bHighlightSintax;
                CustomSettings.XsdProcessIdentityConstraints = bXsdProcessIdentityConstraints;
                CustomSettings.XsdProcessSchemaLocation = bXsdProcessSchemaLocation;
                CustomSettings.XsdReportValidationWarnings = bXsdReportValidationWarnings;
                CustomSettings.IndentationCharsNumber = CustomSettings.IndentationCharsNumber - 1;
            }
            catch (Exception ex)
            {
                //Connection.WriteErrorToRepository(ex);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sXmlDoc"></param>
        /// <param name="sXsdDoc"></param>
        /// <returns></returns>
        public static bool IsValidTriggerOrReplyXmlString(string sXmlDoc)
        {                                    
            return PerformValidation(
                sXmlDoc, 
                FromXsdToString()
                );
        }
        #endregion 
    }
}
