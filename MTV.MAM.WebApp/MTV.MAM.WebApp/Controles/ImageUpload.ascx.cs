using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using System.IO;
using MTV.MAM.WebApp.Helper;

namespace MTV.MAM.WebApp.Controles
{
    public partial class ImageUploadControl : System.Web.UI.UserControl
    {
        #region Method (s)
        public void BindImageUpload()
        {
            try
            {
                this.ImgCover.ImageUrl = string.Format("~/GetImage.ashx?EventID={0}&width={1}&height={2}", this.EventIdentifier, BLC.DefaultValue.Cover_Width, BLC.DefaultValue.Cover_Height);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("ImageUploadControl : BindImageUpload : {0}", ex.Message));
            }
        }

        public byte[] SaveSelectedCover()
        {
            byte[] PosterPictureBinary = new byte[0];
            HttpPostedFile PosterPictureFile = IdFileUpload.PostedFile;

            try
            {
                //----- Check if the selected Image name is different to the Defaut poster name ''
                if ((PosterPictureFile == null) || (String.IsNullOrEmpty(PosterPictureFile.FileName))) // || PosterPictureFile.FileName == BLC.DefaultValue.Cover_Default
                    return PosterPictureBinary;

                //------ Convert the Poster to byte Array
                PosterPictureBinary = BLC.PictureManager.GetPictureBits(PosterPictureFile.InputStream, PosterPictureFile.ContentLength);

            }
            catch(Exception ex)
            {
                LogHelper.logger.Error(string.Format("ImageUploadControl : SaveSelectedCover : {0}", ex.Message));
            }
            return PosterPictureBinary;
        }

        #endregion

        #region Parameter (s)
        public string SelectedCoverExtension
        {
            get
            {
                HttpPostedFile PosterPictureFile = IdFileUpload.PostedFile;
                
                if (PosterPictureFile != null && !string.IsNullOrEmpty(PosterPictureFile.FileName))
                {
                    FileInfo posterInfos = null;
                    try
                    {
                        posterInfos = new FileInfo(PosterPictureFile.FileName);
                    }
                    catch { }

                    if(posterInfos== null || string.IsNullOrEmpty(posterInfos.Name))
                        return string.Empty;

                    return posterInfos.Extension;
                }
                else
                    return string.Empty;
            }
        }
        public bool Enabled
        {
            set
            {
                IdFileUpload.Enabled = value;
            }
        }
        public int EventIdentifier { get; set; }
        #endregion
    }
}