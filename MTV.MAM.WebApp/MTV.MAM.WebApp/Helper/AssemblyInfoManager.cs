using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.IO;

namespace MTV.MAM.WebApp.Helper
{
    /// <summary>
    /// Summary description for AssemblyInfoManager
    /// </summary>
    public class AssemblyInfoManager
    {
        private static System.Reflection.Assembly _EntryAssembly = null;
        private static NameValueCollection _EntryAssemblyAttribCollection = null;

        static AssemblyInfoManager()
        {

        }

        private static void LoadResources()
        {
            if (_EntryAssembly == null)
            {
                try
                {
                    _EntryAssembly = default(System.Reflection.Assembly);
                    _EntryAssembly = Assembly.GetExecutingAssembly();
                    _EntryAssemblyAttribCollection = AssemblyAttribs(_EntryAssembly);
                }
                catch { }

            }

        }

        /// <summary>
        /// retrieves a AssemblyTitle from the assembly 
        /// </summary>
        public static string getAssemblyTitle()
        {
            LoadResources();
            return EntryAssemblyAttrib("Title");
        }

        /// <summary>
        /// retrieves a Copyright from the assembly 
        /// </summary>
        public static string getCopyright()
        {
            LoadResources();
            return EntryAssemblyAttrib("copyright");
        }

        /// <summary>
        /// retrieves a Description from the assembly 
        /// </summary>
        public static string getDescription()
        {
            LoadResources();
            return EntryAssemblyAttrib("description");
        }

        /// <summary>
        /// retrieves a Company from the assembly 
        /// </summary>
        public static string getCompany()
        {
            LoadResources();
            return EntryAssemblyAttrib("company");
        }

        /// <summary>
        /// retrieves a Company tel from the assembly AssemblyCompanyTelAttribute
        /// </summary>

        public static string getCompanyTel()
        {
            LoadResources();
            return EntryAssemblyAttrib("CompanyTel");
        }

        /// <summary>
        /// retrieves a Company Email from the assembly AssemblyCompanyTelAttribute
        /// </summary>

        public static string getCompanyEmail()
        {
            LoadResources();
            return EntryAssemblyAttrib("CompanyEmail");
        }


        /// <summary>
        /// retrieves a Company Email from the assembly AssemblyCompanyTelAttribute
        /// </summary>

        public static string getCompanyURL()
        {
            LoadResources();
            return EntryAssemblyAttrib("CompanyUrl");
        }


        /// <summary>
        /// retrieves a Company Email from the assembly AssemblyCompanyTelAttribute
        /// </summary>

        public static string getCompanyFax()
        {
            LoadResources();
            return EntryAssemblyAttrib("CompanyFax");
        }

        /// <summary>
        /// retrieves a Company Email from the assembly AssemblyCompanyTelAttribute
        /// </summary>

        public static string getCompanyAdress()
        {
            LoadResources();
            return EntryAssemblyAttrib("CompanyAdress");
        }

        /// <summary>
        /// retrieves a Product from the assembly 
        /// </summary>
        public static string getProduct()
        {
            LoadResources();
            return EntryAssemblyAttrib("product");
        }

        /// <summary>
        /// retrieves a Trademark from the assembly 
        /// </summary>
        public static string getTrademark()
        {
            LoadResources();
            return EntryAssemblyAttrib("trademark");
        }

        /// <summary>
        /// retrieves a Version from the assembly 
        /// </summary>
        public static string getVersion()
        {
            LoadResources();
            return EntryAssemblyAttrib("version");
        }

        /// <summary>
        /// retrieves a BuildDate from the assembly 
        /// </summary>
        public static string getBuildDate()
        {
            LoadResources();
            return EntryAssemblyAttrib("builddate");
        }


        /// <summary>
        /// retrieves a Location from the assembly 
        /// </summary>
        public static string getLocation()
        {
            LoadResources();
            return EntryAssemblyAttrib("Location");
        }

        /// <summary>
        /// retrieves a RuntimeVersion from the assembly 
        /// </summary>
        public static string getRuntimeVersion()
        {
            //LoadResources();
            return Regex.Match(Environment.Version.ToString(), @"\d+.\d+.\d+").ToString();
        }

        public static string getEntryAttrib(String key)
        {
            LoadResources();
            return EntryAssemblyAttrib(key);
        }

        /// <summary>
        /// returns string name / string value pair of all 
        /// for specified assembly
        /// </summary>
        /// <remarks>
        /// Trademark = AssemblyTrademark string
        /// Debuggable = True
        /// GUID = 7FDF68D5-8C6F-44C9-B391-117B5AFB5467
        /// CLSCompliant = True
        /// Product = AssemblyProduct string
        /// Copyright = AssemblyCopyright string
        /// Company = AssemblyCompany string
        /// Description = AssemblyDescription string
        /// Title = AssemblyTitle string
        /// </remarks>

        private static NameValueCollection AssemblyAttribs(System.Reflection.Assembly a)
        {
            String TypeName;
            String Name;
            String Value;

            NameValueCollection nvc = new NameValueCollection();
            Regex r = new Regex("(\\.Assembly|\\.)(?<Name>[^.]*)Attribute$", RegexOptions.IgnoreCase);
            // Iterate for all the attributes for the assembly. 
            foreach (Object attrib in a.GetCustomAttributes(false))
            {
                TypeName = attrib.GetType().ToString();
                Name = r.Match(TypeName).Groups["Name"].ToString();
                Value = "";

                switch (TypeName)
                {
                    case "System.CLSCompliantAttribute":
                        Value = ((CLSCompliantAttribute)attrib).IsCompliant.ToString();
                        break;
                    case "System.Diagnostics.DebuggableAttribute":
                        Value = ((DebuggableAttribute)attrib).IsJITTrackingEnabled.ToString();
                        break;
                    case "System.Reflection.AssemblyCompanyAttribute":
                        Value = ((AssemblyCompanyAttribute)attrib).Company.ToString();
                        break;

                    //case "AssemblyUtils.AssemblyCompanyEmailAttribute":
                    //    Value = ((AssemblyUtils.AssemblyCompanyEmailAttribute)attrib).CompanyEmail.ToString();
                    //    break;

                    //case "AssemblyUtils.AssemblyCompanyUrlAttribute":
                    //    Value = ((AssemblyUtils.AssemblyCompanyUrlAttribute)attrib).CompanyUrl.ToString();
                    //    break;

                    //case "AssemblyUtils.AssemblyCompanyFaxAttribute":
                    //    Value = ((AssemblyUtils.AssemblyCompanyFaxAttribute)attrib).CompanyFax.ToString();
                    //    break;

                    //case "AssemblyUtils.AssemblyCompanyTelAttribute":
                    //    Value = ((AssemblyUtils.AssemblyCompanyTelAttribute)attrib).CompanyTel.ToString();

                    //    break;

                    //case "AssemblyUtils.AssemblyCompanyAdressAttribute":
                    //    Value = ((AssemblyUtils.AssemblyCompanyAdressAttribute)attrib).CompanyAdress.ToString();
                    //    break;

                    case "System.Reflection.AssemblyConfigurationAttribute":
                        Value = ((AssemblyConfigurationAttribute)attrib).Configuration.ToString();
                        break;

                    case "System.Reflection.AssemblyCopyrightAttribute":
                        Value = ((AssemblyCopyrightAttribute)attrib).Copyright.ToString();
                        break;

                    case "System.Reflection.AssemblyDefaultAliasAttribute":
                        Value = ((AssemblyDefaultAliasAttribute)attrib).DefaultAlias.ToString();
                        break;

                    case "System.Reflection.AssemblyDelaySignAttribute":
                        Value = ((AssemblyDelaySignAttribute)attrib).DelaySign.ToString();
                        break;

                    case "System.Reflection.AssemblyDescriptionAttribute":
                        Value = ((AssemblyDescriptionAttribute)attrib).Description.ToString();
                        break;

                    case "System.Reflection.AssemblyInformationalVersionAttribute":
                        Value = ((AssemblyInformationalVersionAttribute)attrib).InformationalVersion.ToString();
                        break;

                    case "System.Reflection.AssemblyKeyFileAttribute":
                        Value = ((AssemblyKeyFileAttribute)attrib).KeyFile.ToString();
                        break;

                    case "System.Reflection.AssemblyProductAttribute":
                        Value = ((AssemblyProductAttribute)attrib).Product.ToString();
                        break;

                    case "System.Reflection.AssemblyTrademarkAttribute":
                        Value = ((AssemblyTrademarkAttribute)attrib).Trademark.ToString();
                        break;

                    case "System.Reflection.AssemblyTitleAttribute":
                        Value = ((AssemblyTitleAttribute)attrib).Title.ToString();
                        break;

                    case "System.Resources.NeutralResourcesLanguageAttribute":
                        Value = ((System.Resources.NeutralResourcesLanguageAttribute)attrib).CultureName.ToString();
                        break;

                    case "System.Resources.SatelliteContractVersionAttribute":
                        Value = ((System.Resources.SatelliteContractVersionAttribute)attrib).Version.ToString();
                        break;

                    case "System.Runtime.InteropServices.ComCompatibleVersionAttribute":
                        System.Runtime.InteropServices.ComCompatibleVersionAttribute x;
                        x = attrib as System.Runtime.InteropServices.ComCompatibleVersionAttribute;
                        Value = string.Format("{0}.{1}.{2}.{3}", x.MajorVersion, x.MinorVersion, x.RevisionNumber, x.BuildNumber);
                        break;

                    case "System.Runtime.InteropServices.ComVisibleAttribute":
                        Value = ((System.Runtime.InteropServices.ComVisibleAttribute)attrib).Value.ToString();
                        break;

                    case "System.Runtime.InteropServices.GuidAttribute":
                        Value = ((System.Runtime.InteropServices.GuidAttribute)attrib).Value.ToString();
                        break;

                    case "System.Runtime.InteropServices.TypeLibVersionAttribute":
                        System.Runtime.InteropServices.TypeLibVersionAttribute tlva = attrib as System.Runtime.InteropServices.TypeLibVersionAttribute;
                        Value = tlva.MajorVersion + "." + tlva.MinorVersion;
                        break;

                    case "System.Security.AllowPartiallyTrustedCallersAttribute":
                        Value = "(Present)";
                        break;


                    default:

                        Value = TypeName;
                        break;


                }

                if (string.IsNullOrEmpty(nvc[Name]))
                    nvc.Add(Name, Value);

            }

            // build date
            DateTime dt = AssemblyBuildDate(a);
            if (dt == DateTime.MaxValue)
                nvc.Add("BuildDate", "(unknown)");

            else
                nvc.Add("BuildDate", dt.ToString("yyyy-MM-dd hh:mm tt"));

            // location
            try { nvc.Add("Location", a.Location); }
            catch { nvc.Add("Location", "(not supported)"); }

            // version
            try
            {
                if (a.GetName().Version.Major == 0 && a.GetName().Version.Minor == 0)
                    nvc.Add("Version", "(unknown)");
                else
                    nvc.Add("Version", a.GetName().Version.ToString());
            }
            catch { nvc.Add("Version", "(unknown)"); }

            nvc.Add("FullName", a.FullName);

            return nvc;

        }




        /// <summary>
        /// retrieves a cached value from the entry assembly attribute lookup collection
        /// </summary>
        private static String EntryAssemblyAttrib(String strName)
        {
            if (string.IsNullOrEmpty(_EntryAssemblyAttribCollection[strName]))
                return "[Assembly: Assembly" + strName + "(\"\")]";
            else
                return _EntryAssemblyAttribCollection[strName].ToString();
        }

        private static DateTime AssemblyBuildDate(System.Reflection.Assembly a)
        {
            return AssemblyBuildDate(a, false);
        }


        private static DateTime AssemblyBuildDate(System.Reflection.Assembly a, bool ForceFileDate)
        {
            System.Version AssemblyVersion = a.GetName().Version;
            DateTime dt;

            if (ForceFileDate)
                dt = AssemblyLastWriteTime(a);
            else
            {
                dt = new DateTime(2000, 1, 1).AddDays(AssemblyVersion.Build).AddSeconds(AssemblyVersion.Revision * 2);
                if (TimeZone.IsDaylightSavingTime(dt, TimeZone.CurrentTimeZone.GetDaylightChanges(dt.Year)))
                    dt = dt.AddHours(1);
                if (dt > DateTime.Now || AssemblyVersion.Build < 730 || AssemblyVersion.Revision == 0)
                    dt = AssemblyLastWriteTime(a);
            }

            return dt;
        }


        private static DateTime AssemblyLastWriteTime(System.Reflection.Assembly a)
        {
            try
            {
                return File.GetLastWriteTime(a.Location);
            }
            catch
            {
                return DateTime.MaxValue;
            }
        }

    }
}