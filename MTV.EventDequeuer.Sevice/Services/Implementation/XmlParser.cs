using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MTV.EventDequeuer.Service.Services.Contracts;
using MTV.EventDequeuer.Contracts.Data;
using System.Net;
using MTV.EventDequeuer.Common;
using System.Xml.Linq;

namespace MTV.EventDequeuer.Service.Services.Implementation
{
    public class XmlParser : IXmlParser
    {

        public XmlParser()
        {
            
        }
        public RealTimeEventMsg ParseXmlMsg(string msg)
        {
            //<realtimeEvent>
            //  <eventName>OnNewContentsScheduled</eventName>
            //  <date>2012-01-16T15:31:31</date>
            //</realtimeEvent>

            try
            {
                RealTimeEventMsg newInfo = new RealTimeEventMsg();
                XElement xelement = XElement.Parse(msg);
                newInfo.EventName = GetSafeString(xelement, "eventName");
                newInfo.Date = GetSafeDate(xelement, "date");
                LogManager.Log.Error(newInfo.EventName.ToString());
                
                return newInfo;

                

            }
            catch (Exception ex)
            {
                LogManager.Log.Error(ex);
                return null;
            }

        }


        public static Int32 GetSafeInt32(XElement root, string elementName)
        {
            try
            {
                XElement element = root.Elements().Where(node => node.Name.LocalName == elementName).Single();
                return Convert.ToInt32(element.Value);
            }
            catch
            {
                return 0;
            }
        }

        private static DateTime? GetSafeDate(XElement root, string elementName)
        {
            try
            {
                XElement element = root.Elements().Where(node => node.Name.LocalName == elementName).Single();
                return DateTime.Parse(element.Value);
            }
            catch
            {
                return null;
            }
        }


        public static String GetSafeString(XElement root, string elementName)
        {
            try
            {
                XElement element = root.Elements().Where(node => node.Name.LocalName == elementName).Single();
                return element.Value;
            }
            catch
            {
                return String.Empty;
            }
        }       


    }
}
