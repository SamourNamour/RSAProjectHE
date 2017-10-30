using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace MTV.Library.Core.Services
{
    /// <summary>
    /// Interface to parse DataService Client Exception
    /// </summary>
    public class DataServiceClientExceptionHelper
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static DataServiceErrorInfo ParseDataServiceClientException(string exception)
        {
            try
            {
                // namespace XML de DataServiceClientException
                XNamespace ns = "http://schemas.microsoft.com/ado/2007/08/dataservices/metadata";

                XDocument doc = XDocument.Parse(exception);

                return new DataServiceErrorInfo
                {
                    Code = String.IsNullOrEmpty(
                        doc.Root.Element(ns + "code").Value) ? 400 :
                        int.Parse(doc.Root.Element(ns + "code").Value),
                    Message = doc.Root.Element(ns + "message").Value
                };
            }
            catch
            {
                return null;
            }
        }

    }
}
