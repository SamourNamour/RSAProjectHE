using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTV.Library.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class StringWriterWithEncoding : StringWriter
    {
        /// <summary>
        /// 
        /// </summary>
        Encoding encoding;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="encoding"></param>
        public StringWriterWithEncoding(StringBuilder builder, Encoding encoding)
            : base(builder)
        {
            this.encoding = encoding;
        }

        public override Encoding Encoding
        {
            get { return encoding; }
        }
    }
}
