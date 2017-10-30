using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.ComponentModel;
using System.Xml.Linq;
using System.Collections.ObjectModel;
using BLC = MTV.Library.Common;
using System.IO;
using System.Text;
using System.Xml;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;

namespace MTV.MAM.WebApp.Helper
{
    public class MEBSConfigHelper
    {
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

        public static string GetSettingValue(string SettingName)
        {
            mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
            mebs_settings setting = _context.Execute<mebs_settings>(new Uri(string.Format(Config.GetSettingsByName, SettingName), UriKind.Relative)).FirstOrDefault();
            if (setting != null)
                return setting.SettingValue;

            return string.Empty;
        }

        #region -.-.-.-.-.-.-.-.-.-.-.- Categorization -.-.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// Export category list to xml.
        /// </summary>
        /// <returns>Result in XML format</returns>
        //public static string ExportCategoriesToXML()
        //{
        //    CreateContentClassificationXML();

        //    return GetXMLBodyFromXMLFile();

        //}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns></returns>
        //public static void CreateContentClassificationXML()
        //{
        //    mebsEntities _context = new mebsEntities(Config.MEBSCatalogLocation);
        //    //List<mebs_category> MEBSCategories = null;

        //    // Get list of categories to be shown in STBs menu.
        //    List<mebs_category_language_mapping> lCategoryMapping = _context.Execute<mebs_category_language_mapping>(new Uri(Config.GetCategories, UriKind.Relative)).ToList();
        //    if (lCategoryMapping == null || lCategoryMapping.Count <= 0)
        //        throw new Exception("Cannot create the Content Category Xml file : list Categories null or empty.");


        //    while (string.IsNullOrEmpty(CreateCategorizationFolder()))
        //    {
        //        CreateCategorizationFolder();
        //    }

        //    String categoryXml = CenerateXmlCategories(lCategoryMapping);
        //    if (string.IsNullOrEmpty(categoryXml))
        //        throw new Exception("Cannot create the Content Category Xml file : output Xml string is empty.");

        //    bool isCategoryXmlCreated = XmlHelper.XMLStringToFile(categoryXml, ContentCategoriesXMLLocalPath);
        //    if (!isCategoryXmlCreated)
        //        throw new Exception("Cannot create the Content Category Xml file : unable to write the categories string to the Xml file document.");
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string CenerateXmlCategories(List<mebs_category_language_mapping> Languagecategories)
        {
            StringBuilder sb = new StringBuilder();
            BLC.StringWriterWithEncoding stringWriter = new BLC.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            xmlWriterSettings.Indent = true;
            xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

            //---- Set the XmlWriterSettings to the XMLWriter
            xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);


            xmlWriter.WriteStartDocument(false);
            xmlWriter.WriteStartElement("Categorization");
            xmlWriter.WriteAttributeString("version", "1.0");

            xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
            xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/Category_schema.xsd");

            foreach (mebs_category_language_mapping Item in Languagecategories)
            {
                //<Cat>
                xmlWriter.WriteStartElement("Cat");
                xmlWriter.WriteAttributeString("default", "-1");
                xmlWriter.WriteAttributeString("visible", "true");

                //<Id>
                //xmlWriter.WriteStartElement("Id");
                //int nDVBContent = Convert.ToInt32(cat.DVBContentID, 16);
                //xmlWriter.WriteString(nDVBContent.ToString());
                //xmlWriter.WriteEndElement();
                //</Id>
                if (Item.mebs_language != null)
                {
                    xmlWriter.WriteStartElement("Name");
                    xmlWriter.WriteAttributeString("lang", Item.mebs_language.ISOCode.ToLower());
                    xmlWriter.WriteString(Item.Title);
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();

            }



            //foreach (Category cat in categories)
            //{
            //    //<Cat>
            //    xmlWriter.WriteStartElement("Cat");
            //    xmlWriter.WriteAttributeString("default", "-1");
            //    xmlWriter.WriteAttributeString("visible", "true");

            //    //<Id>
            //    xmlWriter.WriteStartElement("Id");
            //    int nDVBContent = Convert.ToInt32(cat.DVBContentID, 16);
            //    xmlWriter.WriteString(nDVBContent.ToString());
            //    xmlWriter.WriteEndElement();
            //    //</Id>

            //    // get the Language for this category
            //    List<LanguageCategoryDetails> categoryLanguages
            //        = LanguageCategoryDetailsProvider.GetLanguageCategoryDetailsCollectionByCategory(cat.IdCategory);
            //    if (categoryLanguages != null && categoryLanguages.Count > 0)
            //    {
            //        foreach (LanguageCategoryDetails lang in categoryLanguages)
            //        {
            //            LanguageCategory catLang = LanguageCategoryProvider.GetCategoryLanguageByID(lang.IDLanguage);
            //            if (catLang != null)
            //            {
            //                xmlWriter.WriteStartElement("Name");
            //                xmlWriter.WriteAttributeString("lang", catLang.IsoCode.ToLower());
            //                xmlWriter.WriteString(lang.Title);
            //                xmlWriter.WriteEndElement();
            //            }
            //        }
            //    }

            //    xmlWriter.WriteEndElement();

            //}

            //</RecCmd>
            xmlWriter.WriteEndElement();


            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
            return stringWriter.ToString();
        }

        /// <summary>
        /// Reads and returns as a string the contents of a file . 
        /// </summary>
        /// <returns>the returned string is encoded to UTF-8 format</returns>
        public static string GetXMLBodyFromXMLFile()
        {
            string strXMLBody = string.Empty;

            // Create Content.Categories XML file in application local Path (since MMPXML cannot create remote file) :
            if (File.Exists(ContentCategoriesXMLLocalPath))
            {
                using (TextReader TR = new StreamReader(ContentCategoriesXMLLocalPath, Encoding.UTF8))
                {
                    strXMLBody = TR.ReadToEnd();
                }
            }
            return strXMLBody;
        }

        /// <summary>
        /// Create ContentCategories.xml file folder
        /// </summary>    
        /// <returns></returns>
        public static string CreateCategorizationFolder()
        {
            string strXMLDir = string.Format(@"{0}{1}",
                                               AppDomain.CurrentDomain.BaseDirectory,
                                               BLC.DefaultValue.ContentCategoriesDirectoryName);
            if (!Directory.Exists(strXMLDir))
            {
                return BLC.CommonHelper.CreateDirectory(strXMLDir);
            }
            return strXMLDir;
        }


        /// <summary>
        /// 
        /// </summary>
        public static void DeleteGeneratedClassificationXMlFile()
        {
            if (File.Exists(ContentCategoriesXMLLocalPath))
            {
                File.Delete(ContentCategoriesXMLLocalPath);
            }
        }

        #region Property(ies)
        /// <summary>
        /// 
        /// </summary>
        public static string ContentCategoriesXMLLocalPath
        {
            get
            {
                return string.Format(@"{0}{1}{2}{3}",
                                   AppDomain.CurrentDomain.BaseDirectory,
                                  BLC.DefaultValue.ContentCategoriesDirectoryName,
                                   Path.DirectorySeparatorChar,
                                   BLC.DefaultValue.ContentCategoriesFileName);
            }
        }

        #endregion

        #endregion 


    }
}