using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.Library.Core.Tools
{
    public class StringUtils {
        /// <summary>
        /// Lower the first character for a TagName
        /// </summary>
        /// <param name="_TagName"></param>
        /// <returns></returns>
        public static string LowerCaseFirst(string _TagName) {
            if (string.IsNullOrEmpty(_TagName))
                return string.Empty;

            char[] a = _TagName.ToCharArray();
            a[0] = char.ToLower(a[0]);
            return new string(a);
        }
    }
}
