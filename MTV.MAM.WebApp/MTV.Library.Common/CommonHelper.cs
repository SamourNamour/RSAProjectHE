using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BLC = MTV.Library.Common;
using System.Web;
using System.Xml;
using System.IO;

namespace MTV.Library.Common
{
    public class CommonHelper
    {
        public static int ConvertStringToInt(string value, int defaultvalue)
        {
            try
            {
                return Convert.ToInt32(value);
            }
            catch
            {
                return defaultvalue;
            }
        }

        public static long ConvertStringToLong(string value, long defaultvalue)
        {
            try
            {
                return (long)Convert.ToDouble(value);
            }
            catch
            {
                return defaultvalue;
            }
        }

        /// <summary>
        /// Gets query string value by name
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static string QueryString(string Name)
        {
            string result = string.Empty;
            if (HttpContext.Current != null && HttpContext.Current.Request.QueryString[Name] != null)
                result = HttpContext.Current.Request.QueryString[Name].ToString();
            return result;
        }

        /// <summary>
        /// Gets boolean value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static bool QueryStringBool(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            return (resultStr == "YES" || resultStr == "TRUE" || resultStr == "1");
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string Name)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            int result;
            Int32.TryParse(resultStr, out result);
            return result;
        }

        /// <summary>
        /// Gets integer value from query string 
        /// </summary>
        /// <param name="Name">Parameter name</param>
        /// <param name="DefaultValue">Default value</param>
        /// <returns>Query string value</returns>
        public static int QueryStringInt(string Name, int DefaultValue)
        {
            string resultStr = QueryString(Name).ToUpperInvariant();
            if (resultStr.Length > 0)
            {
                return Int32.Parse(resultStr);
            }
            return DefaultValue;
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        public static void ReloadCurrentPage()
        {
            ReloadCurrentPage(false);
        }

        /// <summary>
        /// Reloads current page
        /// </summary>
        /// <param name="UseSSL">Use SSL</param>
        public static void ReloadCurrentPage(bool UseSSL)
        {
            string AppHost = GetHost(UseSSL);
            if (AppHost.EndsWith("/"))
                AppHost = AppHost.Substring(0, AppHost.Length - 1);
            string URL = AppHost + HttpContext.Current.Request.RawUrl;
            HttpContext.Current.Response.Redirect(URL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="UseSSL"></param>
        /// <returns></returns>
        public static string GetHost(bool UseSSL)
        {
            string result = "http://" + ServerVariables("HTTP_HOST");
            if (!result.EndsWith("/"))
                result += "/";

            if (UseSSL)
            {
                if (!String.IsNullOrEmpty(""))
                {
                    result = "";
                }
                else
                {
                    result = result.Replace("http:/", "https:/");
                    result = result.Replace("www.www", "www");
                }
            }

            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Gets server variable by name
        /// </summary>
        /// <param name="Name">Name</param>
        /// <returns>Server variable</returns>
        public static string ServerVariables(string Name)
        {
            string tmpS = String.Empty;
            try
            {
                if (HttpContext.Current.Request.ServerVariables[Name] != null)
                {

                    tmpS = HttpContext.Current.Request.ServerVariables[Name].ToString();

                }
            }
            catch
            {
                tmpS = String.Empty;
            }
            return tmpS;
        }

        /// <summary>
        /// Convert a size to a Formate Text
        /// </summary>
        /// <param name="dwFileSize"></param>
        /// <returns></returns>
        static public string GetSize(long dwFileSize)
        {
            if (dwFileSize < 0) return "0";
            string szTemp;
            // file < 1 kbyte?
            if (dwFileSize < 1024)
            {
                //  substract the integer part of the float value
                float fRemainder = (((float)dwFileSize) / 1024.0f) - (((float)dwFileSize) / 1024.0f);
                float fToAdd = 0.0f;
                if (fRemainder < 0.01f)
                    fToAdd = 0.1f;
                szTemp = String.Format("{0:f} KB", (((float)dwFileSize) / 1024.0f) + fToAdd);
                return szTemp;
            }
            long iOneMeg = 1024 * 1024;

            // file < 1 megabyte?
            if (dwFileSize < iOneMeg)
            {
                szTemp = String.Format("{0:f} KB", ((float)dwFileSize) / 1024.0f);
                return szTemp;
            }

            // file < 1 GByte?
            long iOneGigabyte = iOneMeg;
            iOneGigabyte *= (long)1000;
            if (dwFileSize < iOneGigabyte)
            {
                szTemp = String.Format("{0:f} MB", ((float)dwFileSize) / ((float)iOneMeg));
                return szTemp;
            }
            //file > 1 GByte
            int iGigs = 0;
            while (dwFileSize >= iOneGigabyte)
            {
                dwFileSize -= iOneGigabyte;
                iGigs++;
            }
            float fMegs = ((float)dwFileSize) / ((float)iOneMeg);
            fMegs /= 1000.0f;
            fMegs += iGigs;
            szTemp = String.Format("{0:f} GB", fMegs);
            return szTemp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="includeQueryString"></param>
        /// <returns></returns>
        public static string GetThisPageURL(bool includeQueryString)
        {
            string URL = string.Empty;
            if (HttpContext.Current == null)
                return URL;

            if (includeQueryString)
            {
                string Host = GetHost(false);
                if (Host.EndsWith("/"))
                    Host = Host.Substring(0, Host.Length - 1);
                URL = Host + HttpContext.Current.Request.RawUrl;
            }
            else
            {
                URL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Path);
            }
            return URL;
        }

        /// <summary>
        /// Gets location
        /// </summary>
        /// <returns>location</returns>
        public static string GetLocation()
        {
            string result = GetHost(false);
            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            result = result + HttpContext.Current.Request.ApplicationPath;
            if (!result.EndsWith("/"))
                result += "/";

            return result;
        }

        /// <summary>
        /// Write XML to response
        /// </summary>
        /// <param name="xml">XML</param>
        /// <param name="Filename">Filename</param>
        public static void WriteResponseXML(string xml, string Filename)
        {
            if (!String.IsNullOrEmpty(xml))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(xml);
                ((XmlDeclaration)document.FirstChild).Encoding = "utf-8";
                HttpResponse response = HttpContext.Current.Response;
                response.Clear();
                response.Charset = "utf-8";
                response.ContentType = "text/xml";
                response.AddHeader("content-disposition", string.Format("attachment; filename={0}", Filename));
                response.BinaryWrite(Encoding.UTF8.GetBytes(document.InnerXml));

                // To avoid ThreadAbortException Occurs If You Use Response.End, Response.Redirect, or Server.Transfer
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                response.End();
            }
        }

        /// <summary>
        /// Create directory.
        /// </summary>
        /// <param name="strDirectory">string</param>
        /// <returns>string</returns>
        public static string CreateDirectory(string strDirectory)
        {
            DirectoryInfo dirInf = Directory.CreateDirectory(strDirectory);
            return dirInf.FullName;
        }
    }
}
