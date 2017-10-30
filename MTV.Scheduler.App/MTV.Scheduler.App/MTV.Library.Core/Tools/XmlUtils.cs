using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTV.Library.Core.Tools
{
    public class XmlUtils {
        /// <summary>
        /// Convert string To UTF8 Encoding
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public static string ConvertToUTF8(string strXml) {
            byte[] byteArray = Encoding.UTF8.GetBytes(strXml);
            using (MemoryStream stream = new MemoryStream(byteArray)) {
                using (TextReader TR = new StreamReader(stream, Encoding.UTF8)) {
                    return TR.ReadToEnd();
                }
            }
        }
    }
}
