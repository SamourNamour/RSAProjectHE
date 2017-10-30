// ACK :

#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;

//using CMEScheduler;
using MTV.Library.Core.Common;
//using Logger.HELogger;
//using CMEScheduler.UtilityAPI;
#endregion 

namespace MTV.Library.Core.TriggerInterface
{
    /// <summary>
    /// Provide methods to manage Harris UDP message Acknowledgmenet and retransmission.
    /// </summary>
    public class UDPmsg_t_Provider
    {
        #region Variables
        /// <summary>
        /// To avoid fragmentation, the size of the messages will be limited to 1472 bytes so as to fit in one Ethernet packet. 
        /// </summary>
        public static int MaxUdpPacketSize
        {
            get
            {
                return 1472;
            }
        }
        #endregion 

        #region Main Method(s)

        /// <summary>
        /// Convert UDPmsg_t to xml file.
        /// </summary>
        /// <param name="UDPmsg">UDPmsg_t to be serialized</param>
        /// <param name="xmlPath">Xml file location.</param>
        public static void SerializeUDPmsg(UDPmsg_t UDPmsg, string xmlPath)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(UDPmsg_t));
                using (StreamWriter sw = new StreamWriter(xmlPath))
                {
                    xmlSer.Serialize(sw, UDPmsg);
                }
            }
            catch (Exception ex)
            {
                //-----.DefaultLogger.STARLogger.Error("Exception", ex);
            }
        }

        /// <summary>
        /// Return UDPmsg_t Object by XML Deserializing the Harris XML Body.
        /// </summary>
        /// <param name="UDPmsgTriggerXmlBody">UDP Trigger message XML Body.</param>
        /// <returns>UDPmsg_t : result of the XML Deserialization process of the UDP Trigger message XML Body</returns>
        public static UDPmsg_t GetUDPmsg(string UDPmsgTriggerXmlBody)
        {
            UDPmsg_t UDPmsg = null;
            try
            {
                if (!string.IsNullOrEmpty(UDPmsgTriggerXmlBody))
                {
                    UDPmsg = new UDPmsg_t();
                    XmlSerializer serializer = new XmlSerializer(typeof(UDPmsg_t));
                    using (StringReader sr = new StringReader(UDPmsgTriggerXmlBody))
                    {
                        UDPmsg = (UDPmsg_t)serializer.Deserialize(sr);
                    }                    
                }
                return UDPmsg;
            }
            catch(Exception ex)
            {
                //-----.DefaultLogger.STARLogger.Error(string.Format("Cannot parse UDP message to a valid UDPmsg_t object [{0}]", UDPmsgTriggerXmlBody), ex);
                return null;
            }
        }

        /// <summary>
        /// Get Harris UDPmsg reply object either ACK or NAK for a specific UDPmsg number.
        /// </summary>
        /// <param name="msgNumber">UDPmsg number to reply to it.</param>
        /// <param name="itemsChoiceTypeEnum">UDPmsg reply type ACK or NAK.</param>
        /// <returns>UDPmsg_t</returns>
        public static UDPmsg_t GetUDPmsgObjectReply(
            int msgNumber,
            ItemsChoiceType itemsChoiceTypeEnum)
        {
            UDPmsg_t UDPmsg = new UDPmsg_t();

            // Set reply msgNumber:
            UDPmsg.msgNumber = Convert.ToString(msgNumber);

            // Set whether schema Version Specified:
            UDPmsg.schemaVersionSpecified = false;

            UDPmsg.Items =
                new object[] { new empty_t() };

            UDPmsg.ItemsElementName =
                new ItemsChoiceType[1] { itemsChoiceTypeEnum };

            return UDPmsg;
        }

        /// <summary>
        /// Get Harris UDPmsg reply string either ACK or NAK for a specific UDPmsg number.
        /// </summary>
        /// <param name="msgNumber">UDPmsg number to reply to it.</param>
        /// <param name="itemsChoiceTypeEnum">UDPmsg reply type ACK or NAK.</param>
        /// <returns>string</returns>
        public static string GetUDPmsgStringReply(
           int msgNumber,
           ItemsChoiceType itemsChoiceTypeEnum)
        {
            UDPmsg_t UDPmsg = GetUDPmsgObjectReply(msgNumber, itemsChoiceTypeEnum);
            string xmlBody = Encoding.UTF8.GetString(GetXMLBodyFromUDPmsgObject(UDPmsg));
            return (UDPmsg != null
                    ? xmlBody
                    : string.Empty);
        }

        /// <summary>
        /// Get Harris UDPmsg reply byte array either ACK or NAK for a specific UDPmsg number.
        /// </summary>
        /// <param name="msgNumber">UDPmsg number to reply to it.</param>
        /// <param name="itemsChoiceTypeEnum">UDPmsg reply type ACK or NAK.</param>
        /// <returns>byte array</returns>
        public static byte[] GetUDPmsgByteArrayReply(
           int msgNumber,
           ItemsChoiceType itemsChoiceTypeEnum)
        {
            UDPmsg_t UDPmsg = GetUDPmsgObjectReply(msgNumber, itemsChoiceTypeEnum);
            byte[] xmlBody = GetXMLBodyFromUDPmsgObject(UDPmsg);
            return (UDPmsg != null
                    ? xmlBody
                    : null);
        }

        /// <summary>
        /// Get Harris UDPmsg reply byte array either ACK or NAK for a specific UDPmsg number (possibility to include End of byte array null carater).
        /// </summary>
        /// <param name="msgNumber">UDPmsg number to reply to it.</param>
        /// <param name="itemsChoiceTypeEnum">UDPmsg reply type ACK or NAK.</param>
        /// <param name="includeNullEndChar">include End of byte array null carater or not</param>
        /// <returns>byte array</returns>
        public static byte[] GetUDPmsgByteArrayReply(
            int msgNumber,
            ItemsChoiceType itemsChoiceTypeEnum,
            bool includeNullEndChar)
        {
            byte[] reply = GetUDPmsgByteArrayReply(msgNumber, itemsChoiceTypeEnum);
            byte[] replyWithNullEndChar = new byte[reply.Length+1];
            reply.CopyTo(replyWithNullEndChar, 0);

            return (includeNullEndChar ?
                replyWithNullEndChar
                :
                reply);
        }

        /// <summary>
        /// Get UDPmsg_t event_t collection from UDPmsg_t object.
        /// </summary>
        /// <param name="UDPmsg">UDPmsg_t</param>
        /// <returns>event_t Collection</returns>
        public static List<event_t> GetEventsByUDPmsg(UDPmsg_t UDPmsg)
        {            
            List<event_t> eventCollection = null;

            if (UDPmsg != null)
            {
                eventCollection = new List<event_t>();
                if (UDPmsg.Items != null
                    && UDPmsg.ItemsElementName != null
                    && (UDPmsg.Items.Length == UDPmsg.ItemsElementName.Length)
                    )
                {
                    for (int i = 0; i < UDPmsg.Items.Length; i++)
                    {
                        // test if UDPmsg.Items[i] object is an event_t instance :                        
                        if ((UDPmsg.Items[i].GetType() == typeof(event_t)) &&
                            UDPmsg.ItemsElementName[i] == ItemsChoiceType.@event)
                        {
                            eventCollection.Add(UDPmsg.Items[i] as event_t);
                        }
                    }
                }
            }
            return eventCollection;
        }

        /// <summary>
        /// Get TITLE field value from event_t object.
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetTITLE(event_t objEvent)
        {
            string strTITLE = string.Empty;
            if (objEvent != null && objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.TITLE)
                                      ) == 0
                        )
                    {
                        strTITLE = var.Value;
                        break;
                    }
                }
            }
            return strTITLE;
        }

        /// <summary>
        /// Get VIDEO_ITEM(Lysis Code) field value from event_t object.
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetVIDEO_ITEM(event_t objEvent)
        {
            string strVIDEO_ITEM = string.Empty;
            if (objEvent != null && objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.VIDEO_ITEM)
                                      ) == 0
                        )
                    {
                        strVIDEO_ITEM = var.Value;
                        break;
                    }
                }
            }
            return strVIDEO_ITEM;
        }

        /// <summary>
        /// Get VIDEO_INTIME field value from event_t object(default format hh:mm:ss:ff).
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetVIDEO_INTIME(event_t objEvent)
        {
            string strVIDEO_INTIME = string.Empty;
            if (objEvent != null && objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.VIDEO_INTIME)
                                      ) == 0
                        )
                    {
                        strVIDEO_INTIME = var.Value;
                        break;
                    }
                }
            }
            return strVIDEO_INTIME;
        }

        /// <summary>
        /// Get Duration field value from event_t object(default format hh:mm:ss:ff).
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetDURATION(event_t objEvent)
        {
            string strDURATION = string.Empty;
            if (objEvent != null && objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.DURATION)
                                      ) == 0
                        )
                    {
                        strDURATION = var.Value;
                        break;
                    }
                }
            }
            return strDURATION;
        }

        /// <summary>
        /// Get BUS(Harris Interface Name) field value from event_t object.
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetBUS(event_t objEvent)
        {
            string strBUS = string.Empty;
            if (objEvent != null && 
                objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.BUS)
                                      ) == 0
                        )
                    {
                        strBUS = var.Value;
                        break;
                    }
                }
            }
            return strBUS;
        }

        /// <summary>
        /// Get Start Time(Harris Interface Name) field value from event_t object.
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetTIME(event_t objEvent)
        {
            string strTime = string.Empty;
            if (objEvent != null &&
                objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.TIME)
                                      ) == 0
                        )
                    {
                        strTime = var.Value;
                        break;
                    }
                }
            }
            return strTime;
        }

        /// <summary>
        /// Get TYPE_MATERIAL (Movie.. M / Series... S / Commercial.. C / Trailer..T) field value from event_t object.
        /// </summary>
        /// <param name="objEvent">event_t</param>
        /// <returns>string</returns>
        public static string GetTYPE_MATERIAL(event_t objEvent)
        {
            string strTYPE_MATERIAL = string.Empty;
            if (objEvent != null &&
                objEvent.field.Length > 0)
            {
                foreach (field_t var in objEvent.field)
                {
                    if (string.Compare(
                                      var.name,
                                      Convert.ToString(UDPmsgTagAttributs.TYPE_MATERIAL)
                                      ) == 0
                        )
                    {
                        strTYPE_MATERIAL = var.Value;
                        break;
                    }
                }
            }
            return strTYPE_MATERIAL;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objEvent"></param>
        /// <returns></returns>
        public static MaterialType GetTYPE_MATERIAL_Enum(event_t objEvent)
        {            
            string strType = GetTYPE_MATERIAL(objEvent);
            MaterialType _metarial = MaterialType.U;
            foreach (MaterialType item in Enum.GetValues(typeof(MaterialType)))
            {
                if (string.Compare(Convert.ToString(item), strType) == 0)
                {
                    _metarial = item;
                }
            }
            return _metarial;   
        }
        #endregion         

        #region Auxiliary Method(s)
        /// <summary>
        /// To convert a Byte Array of Unicode values (UTF-8 encoded) to a complete String.
        /// </summary>
        /// <param name="characters">Unicode Byte Array to be converted to String</param>
        /// <returns>String converted from Unicode Byte Array</returns>
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            try
            {
                UTF8Encoding encoding = new UTF8Encoding();
                String constructedString = encoding.GetString(characters);
                return constructedString;
            }
            catch (Exception ex)
            {
                //-----.DefaultLogger.STARLogger.Error("Exception", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Get XML body string from XML physical path.
        /// </summary>
        /// <param name="_sUDPmsgTriggerXMLPath">XML path</param>
        /// <returns>return XML body string from XML physical path</returns>
        public static string GetXMLBodyFromXMLFile(string _sUDPmsgTriggerXMLPath)
        {
            try
            {
                using (TextReader TR = new StreamReader(_sUDPmsgTriggerXMLPath, UTF8Encoding.Default))
                {
                    return TR.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                //-----.DefaultLogger.STARLogger.Error("Exception", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Method to convert a custom Object to XML string
        /// </summary>
        /// <param name="UDPmsg">ARMetadata Object that is to be serialized to XML</param>
        /// <returns>XML string</returns>
        public static byte[] GetXMLBodyFromUDPmsgObject(UDPmsg_t UDPmsg)
        {
            if (UDPmsg != null)
            {
                try
                {
                    MemoryStream memoryStream = new MemoryStream();
                    XmlSerializer xs = new XmlSerializer(typeof(UDPmsg_t));
                    using (XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8))
                    {
                        XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                        ns.Add(string.Empty, string.Empty);
                        xs.Serialize(xmlTextWriter, UDPmsg, ns);
                        memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                        StreamReader reader = new StreamReader(memoryStream, UTF8Encoding.Default, true);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                        string xmlString = reader.ReadToEnd();
                        byte[] xmlBytes = UTF8Encoding.Default.GetBytes(xmlString);
                        return xmlBytes;
                    }
                }
                catch (Exception ex)
                {
                    //-----.DefaultLogger.STARLogger.Error("Exception", ex);
                    return null;
                }
            }
            return null;
        }
        #endregion
    }
}
