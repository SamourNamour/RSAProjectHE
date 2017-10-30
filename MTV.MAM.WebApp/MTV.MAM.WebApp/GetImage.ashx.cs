using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Net;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using MTV.MAM.WebApp.MEBSCatalog;
using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.Helper;
using System.Data.Services.Client;
using System.Globalization;
namespace MTV.MAM.WebApp
{
    /// <summary>
    /// Summary description for $codebehindclassname$
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GetImage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            byte[] imgPoster = GetPosterFromDatabase();
            if (imgPoster == null || imgPoster.Length == 0)
            {
                StreamingDefaultPicture(context, BLC.DefaultValue.Cover_Default);
                return;
            }
            try
            {
                MemoryStream mStream = new MemoryStream(imgPoster);
                MemoryStream NewMStream;
                ResizeImageFile(mStream, out NewMStream);
                context.Response.Clear();
                context.Response.ClearHeaders();
                context.Response.ClearContent();
                context.Response.ContentType = "image/jpeg";
                context.Response.BinaryWrite(NewMStream.GetBuffer());

                //stream.Close();
                //response.Close();
                mStream.Dispose();
                NewMStream.Close();
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("GetImage : ProcessRequest : {0}", ex.Message));

                StreamingDefaultPicture(context, BLC.DefaultValue.Cover_Default);
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public byte[] GetPosterFromDatabase()
        {
            byte[] _poster = null;
            try
            {
                mebsEntities _context = new mebsEntities(Config.MTVCatalogLocation);
                mebs_ingesta _CurrentIngesta = _context.Execute<mebs_ingesta>(new Uri(string.Format(Config.GetIngestaById, this.EventIdentifier), UriKind.Relative)).FirstOrDefault();
                if (_CurrentIngesta != null)
                    _poster = _CurrentIngesta.Poster;
            }
            catch (Exception ex) //---- Log in log4Net
            {
                if (ex.InnerException is DataServiceClientException)
                {
                    // Parse the DataServieClientException
                    BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                    // Display the DataServiceClientException message
                    if (innerException != null)
                    {
                        LogHelper.logger.Error(string.Format("GetImage : GetPosterFromDatabase : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("GetImage : GetPosterFromDatabase : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("GetImage : GetPosterFromDatabase : {0}", ex.Message));
                }
            }

            return _poster;
        }

        public void ResizeImageFile(MemoryStream StartMemoryStream, out MemoryStream NewMemoryStream)
        {
            Bitmap startBitmap = new Bitmap(StartMemoryStream);
            // create a new Bitmap with dimensions for the thumbnail.  
            Bitmap newBitmap = new Bitmap(this.width, this.height);

            // Copy the image from the START Bitmap into the NEW Bitmap.  
            // This will create a thumnail size of the same image.  
            newBitmap = ResizeImage(startBitmap, this.width, this.height);

            // Save this image to the specified stream in the specified format.  
            NewMemoryStream = new MemoryStream();
            newBitmap.Save(NewMemoryStream, System.Drawing.Imaging.ImageFormat.Jpeg);
            newBitmap.Dispose();
            startBitmap.Dispose();
        }

        /// <summary>
        /// Resize a Bitmap  
        /// </summary>
        /// <param name="image"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private static Bitmap ResizeImage(Bitmap image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics gfx = Graphics.FromImage(resizedImage))
            {
                gfx.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                gfx.DrawImage(image, 0, 0, width, height);

            }
            return resizedImage;
        }

        public void StreamingDefaultPicture(HttpContext context , string strFileName)
        {
            string ImageName = null;
            string serverDirectory = context.Server.MapPath(BLC.DefaultValue.Common_Path);
            ImageName = Path.Combine(serverDirectory, strFileName);
            
            EncoderParameters encodingParameters = new EncoderParameters(1);
            encodingParameters.Param[0] = new EncoderParameter(Encoder.Quality, 100L); // Set the JPG Quality percentage to 90%.

            ImageCodecInfo jpgEncoder = GetEncoderInfo("image/jpeg");

            // Incoming! This is the original image. 
            System.Drawing.Image image = System.Drawing.Image.FromFile(ImageName);

            // Creating two blank canvas. One that the original image is placed into, the other for the resized version.
            Bitmap originalImage = new Bitmap(image);
            Bitmap newImage = new Bitmap(originalImage, 300, (image.Height * 300 / image.Width));  // Width of 300 & maintain aspect ratio (let it be as high as it needs to be).

            //Changing the newImage to a graphic to allow us to set the HighQualityBilinear property and resize nicely.
            Graphics g = Graphics.FromImage(newImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            g.DrawImage(originalImage, 0, 0, newImage.Width, newImage.Height);

            MemoryStream imageStream = new MemoryStream();
            newImage.Save(imageStream, jpgEncoder, encodingParameters);


            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.ContentType = "image/jpeg";
            context.Response.BinaryWrite(imageStream.GetBuffer());

            // Good boy's tidy-up after themselves! :O
            originalImage.Dispose();
            newImage.Dispose();
            g.Dispose();
            imageStream.Dispose();

        }

        private ImageCodecInfo GetEncoderInfo(string strCodec)
        {
            //Get the list of available encoders

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            //find the encoder with the image/jpeg mime-type
            ImageCodecInfo ImgCodec = null;

            foreach (ImageCodecInfo codec in codecs)
            {

                if (codec.MimeType == strCodec)

                    ImgCodec = codec;
            }
            return ImgCodec;

        }

        ///// <summary>
        ///// 
        ///// </summary>
        public int EventIdentifier
        {
            get
            {
                int _Id = -1;
                _Id = BLC.CommonHelper.QueryStringInt("EventID", -1);
                return _Id;
            }
        }

        public int width
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("width", 86);
            }
        }

        public int height
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("height", 181);
            }
        }
    }
}
