using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MTV.Library.Common
{
    public class PictureManager
    {
        /// <summary>
        /// Convert Image to Byte 
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static byte[] GetPictureBits(Stream fs, int size)
        {
            byte[] img = new byte[size];
            fs.Read(img, 0, size);
            return img;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="img"></param>
        /// <param name="DefaultImg"></param>
        /// <returns></returns>
        public static string GetPicture(object img, string DefaultImg)
        {
            if (img == null)
                return DefaultImg;

            return "data:image/jpg;base64," + Convert.ToBase64String((byte[])img);
        }
    }
}
