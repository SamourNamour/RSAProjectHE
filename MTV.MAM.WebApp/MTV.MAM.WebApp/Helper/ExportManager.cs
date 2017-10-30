using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;
using System.Collections.Specialized;
using System.IO;
using BLC = MTV.Library.Common;
using System.Threading;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;

namespace MTV.MAM.WebApp.Helper
{
    public class ExportManager
    {
        // static bool bUnclass = false;
        public static string CreateXMLCategory()
        {
            mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
            StringBuilder sb = new StringBuilder();
            BLC.StringWriterWithEncoding stringWriter = new BLC.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);

            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();

            NameValueCollection _Name = null;
            NameValueCollection _MediaSetName = null;

            //DAL.Languages lang = null;
            List<mebs_category> lMixedCategories = null;
            //IList lMultiLanguageCategoryDetails = null;
            List<mebs_category_language_mapping> lCatgeoriesLanguage = null;
            StringCollection IDCollection;
            string id = string.Empty;

            try
            {
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

                //---- Set the XmlWriterSettings to the XMLWriter
                xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

                //---- Get List of Root Categories
                lCatgeoriesLanguage = _context.Execute<mebs_category_language_mapping>(new Uri(Config.GetRootCategoryCollection, UriKind.Relative)).ToList();
                    //bLayer.GetRootCategoryCollection();

                xmlWriter.WriteStartDocument(false);

                #region Categorization 1.0
                //---- <Categorization version=`1.0`>
                xmlWriter.WriteStartElement("Categorization");
                xmlWriter.WriteAttributeString("version", "1.0");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/Category_schema.xsd");

                if (lCatgeoriesLanguage != null && lCatgeoriesLanguage.Count > 0)
                {
                    foreach (mebs_category_language_mapping root in lCatgeoriesLanguage)
                    {
                        if (root.mebs_category == null)
                            continue;

                        IDCollection = new StringCollection();
                        if (string.Compare( root.mebs_category.Value, BLC.DefaultValue.UnclassCategoryDataBaseValue.ToString())== 0 ) // Unclassified.
                            continue;
                        //---<Cat>
                        xmlWriter.WriteStartElement("Cat");
                        xmlWriter.WriteAttributeString("default", root.mebs_category.Default.ToString());
                        xmlWriter.WriteAttributeString("visible", root.mebs_category.Visibility);

                        // If Mixed : Add the CategoryValue of MixedCategories
                        //IsMixed = root.IsMixedCategory;
                        if (root.mebs_category.IsMixed.Value)
                        {
                            lMixedCategories = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetMixedCategoryElements, root.mebs_category.IdCategory), UriKind.Relative)).ToList();
                                //bLayer.GetMixedCategoryElements(root.CategoryValue);
                            if (lMixedCategories != null & lMixedCategories.Count >0)
                            {
                                foreach (mebs_category _mixed in lMixedCategories)
                                {
                                    id = (string.Compare(_mixed.Value, BLC.DefaultValue.AllCategoryDataBaseValue.ToString()) == 0 ? BLC.DefaultValue.AllCategoryXMLValue
                                        : string.Compare(_mixed.Value, BLC.DefaultValue.UnclassCategoryDataBaseValue.ToString()) == 0 ? BLC.DefaultValue.UnclassCategoryXMLValue
                                        : _mixed.Value);

                                    if (!IDCollection.Contains(id))
                                        IDCollection.Add(id);
                                }
                            }
                        }

                        //IDCollection = GetSubCategoriesTree(root.CategoryID, IDCollection);

                        if (IDCollection != null && IDCollection.Count > 0)
                        {
                            foreach (string MixedId in IDCollection)
                            {
                                xmlWriter.WriteStartElement("Id");
                                xmlWriter.WriteString(MixedId);
                                xmlWriter.WriteEndElement();
                            }
                        }

                        //---- If Not Mixed : Add the CategoryValue of Real Category.
                        if (!root.mebs_category.IsMixed.Value) //---- Demander a Brahim
                        {
                            //if (root.mebs_category.IsUnclass.Value)//root.IsUnclass == 1
                            //{
                            //    xmlWriter.WriteStartElement("Id");
                            //    xmlWriter.WriteString(("unclass"));
                            //    xmlWriter.WriteEndElement();
                            //}
                            xmlWriter.WriteStartElement("Id");
                            xmlWriter.WriteString((string.Compare(root.mebs_category.Value, BLC.DefaultValue.AllCategoryDataBaseValue.ToString()) == 0 ? BLC.DefaultValue.AllCategoryXMLValue
                                                    : string.Compare(root.mebs_category.Value, BLC.DefaultValue.UnclassCategoryDataBaseValue.ToString()) == 0 ? BLC.DefaultValue.UnclassCategoryXMLValue
                                                    : root.mebs_category.Value));
                            xmlWriter.WriteEndElement();
                        }


                        //lMultiLanguageCategoryDetails = root.ReferringMultiLanguageCategoryDetails();
                        _Name = new NameValueCollection();
                        _MediaSetName = new NameValueCollection();
                        if (root.mebs_language != null)//lMultiLanguageCategoryDetails != null
                        {
                            //foreach (DAL.MultiLanguageCategoryDetails _details in lMultiLanguageCategoryDetails)
                            //{
                                //lang = new DAL.Languages().GetLanguage(_details.IDLanguage);
                                //if (lang != null)
                                //{
                                    _Name.Add(root.mebs_language.ISOCode, root.Title);
                                    _MediaSetName.Add(root.mebs_language.ISOCode, root.VirtualChannelDisignation);
                                //}
                           // }
                        }

                        foreach (string _Key in _Name.AllKeys)
                        {
                            //---<Name>
                            xmlWriter.WriteStartElement("Name");
                            xmlWriter.WriteAttributeString("lang", _Key);
                            xmlWriter.WriteString(_Name[_Key]);
                            //---</Name>
                            xmlWriter.WriteEndElement();
                        }

                        foreach (string _Key in _MediaSetName.AllKeys)
                        {
                            //---<MediasetName>
                            xmlWriter.WriteStartElement("MediasetName");
                            xmlWriter.WriteAttributeString("lang", _Key);
                            xmlWriter.WriteString(_MediaSetName[_Key]);
                            //---</MediasetName>
                            xmlWriter.WriteEndElement();
                        }

                        //---<MediasetLCN>
                        xmlWriter.WriteStartElement("MediasetLCN");
                        xmlWriter.WriteString(root.mebs_category.MediasetLCN.ToString());
                        //---</MediasetLCN>
                        xmlWriter.WriteEndElement();

                        //---<StandardLCN>
                        xmlWriter.WriteStartElement("StandardLCN");
                        xmlWriter.WriteString(root.mebs_category.StandardLCN.ToString());
                        //---</StandardLCN>
                        xmlWriter.WriteEndElement();

                        //---</Cat>
                        xmlWriter.WriteEndElement();
                    }
                }
                //---- </Categorization>
                xmlWriter.WriteEndElement();
                #endregion

                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            return stringWriter.ToString();
        }

        /// <summary>
        /// Generate a Empty Content.Category XML File to hise all Content in the STBs
        /// </summary>
        /// <returns></returns>
        public static string HideAllContent()
        {
            StringBuilder sb = new StringBuilder();
            BLC.StringWriterWithEncoding stringWriter = new BLC.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            try
            {
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

                //---- Set the XmlWriterSettings to the XMLWriter
                xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

                //---- <Categorization version=`1.0`>
                xmlWriter.WriteStartDocument(false);
                xmlWriter.WriteStartElement("Categorization");
                xmlWriter.WriteAttributeString("version", "1.0");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/Category_schema.xsd");

                //---- <Categorization version=`1.1`>
                xmlWriter.WriteStartElement("Categorization");
                xmlWriter.WriteAttributeString("version", "1.1");
                xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/Category_schema.xsd");
                //---- </Categorization>
                xmlWriter.WriteEndElement();


                //---- </Categorization>
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndDocument();
                xmlWriter.Close();
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("ExportManager : HideAllContent : {0}", ex.Message));
                return string.Empty;
            }
            return stringWriter.ToString();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool CreateContentClassificationXML(string xmlBodyString)
        {
            int maxTries = 5;
            bool res = false;
            try
            {
                while (maxTries-- > 0 &&
                       string.IsNullOrEmpty(MEBSConfigHelper.CreateCategorizationFolder()))
                {
                    MEBSConfigHelper.CreateCategorizationFolder();
                }

                XmlDocument xdoc = new XmlDocument();
                xdoc.LoadXml(xmlBodyString);
                xdoc.Save(MEBSConfigHelper.ContentCategoriesXMLLocalPath);
                res = true;
            }
            catch (IOException ioExc)
            {
                LogHelper.logger.Error(string.Format("ExportManager : CreateContentClassificationXML : {0}", ioExc.Message));
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("ExportManager : CreateContentClassificationXML : {0}", ex.Message));
            }
            return res;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="ftpIP"></param>
        /// <param name="ftpUri"></param>
        /// <returns></returns>
        public static bool UploadCategorizationXMLFile(
            string ftpUser,
            string ftpPassword,
            string ftpIP,
            string ftpUri)
        {

            BLC.FTP FTPRef = new BLC.FTP(ftpIP, ftpUser, ftpPassword);
            return FTPRef.UploadFile(ftpUri, MEBSConfigHelper.ContentCategoriesXMLLocalPath);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="ftpIP"></param>
        /// <param name="ftpUri"></param>        
        /// <returns></returns>
        public static bool IsUploadedCategorizationFile(
            string ftpUser,
            string ftpPassword,
            string ftpIP,
            string ftpUri)
        {
            BLC.FTP FTPRef = new BLC.FTP(ftpIP, ftpUser, ftpPassword);
            //return FTPRef.CheckFileExist(
            //    ftpUri,
            //    string.Format("{0}", BLC.DefaultValue.ContentCategoriesFileName ));

            return FTPRef.CheckFileExist(string.Format("{0}{1}", ftpUri, BLC.DefaultValue.ContentCategoriesFileName));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encIP"></param>
        /// <param name="ftpUser"></param>
        /// <param name="ftpPassword"></param>
        /// <param name="ftpIP"></param>
        /// <param name="ftpUri"></param>
        /// <returns></returns>
        public static string SendCategorizationToEncapsulator(
            string encIP,
            string ftpUser,
            string ftpPassword,
            string ftpIP,
            string ftpUri)
        {
            EBSEncapsulator.Service1 _Service1 = new EBSEncapsulator.Service1(encIP);
            int mainSentTries = 0;

            do
            {
                mainSentTries++;
                try
                {
                    return _Service1.EnviaXML(
                                    Convert.ToString(BLC.DefaultValue.ContentCategoriesFileName),
                                    Convert.ToInt32(Helper.MEBSConfigHelper.GetSettingValue(BLC.DefaultValue.Categorization_Channel_Sending)),
                                    ftpIP,
                                    ftpUri,
                                    ftpUser,
                                    ftpPassword);
                }
                catch (Exception e)
                {

                    if (mainSentTries < 3)
                    {
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                    }
                    else
                    {
                        return BLC.DefaultValue.WS_APP_RESULT_KO;
                    }
                }

            } while (true);
        }

        /// <summary>
        /// 
        /// </summary>
        public static void DeleteGeneratedClassificationXMlFile()
        {
            if (File.Exists(MEBSConfigHelper.ContentCategoriesXMLLocalPath))
            {
                File.Delete(MEBSConfigHelper.ContentCategoriesXMLLocalPath);
            }
        }
    }
}