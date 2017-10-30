#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: PosterIntegrationExtension.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.-.- Class : Namespace (s) -.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

using MTV.Library.Core.Extensions;
using MTV.Library.Core;
using MTV.Scheduler.App.UI;
using MTV.Library.Core.Services;
using MTV.Library.Core.PlayoutCommandManager;
using MTV.Library.Core.Data;
using MTV.Scheduler.App.Properties;
#endregion

namespace MTV.EventDispatcher.Service.Extensions.PosterExtension
{
    public class PosterIntegrationExtension : IExtension, IDisposable
    {

        #region -.-.-.-.-.-.-.-.-.-.- Class : Variable (s) -.-.-.-.-.-.-.-.-.-.-
        private PosterParameters parameters;
        private System.Threading.Timer timer;
        private const int posterCheckIntervalInsecond = 60;
        private const int posterlockIntervalInmin = 1;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Constructor -.-.-.-.-.-.-.-.-.-.-

        public PosterIntegrationExtension() :
            this(new PosterParametersSettingsProxy()) {
        }

        public PosterIntegrationExtension(PosterParameters parameters) {
            if (parameters == null) {
                throw new ArgumentNullException("parameters");
            }

            this.parameters = parameters;


            TimerCallback refreshCallBack = new TimerCallback(checkV2);
            TimeSpan refreshInterval = TimeSpan.FromSeconds(posterCheckIntervalInsecond);
            timer = new Timer(refreshCallBack, null, new TimeSpan(-1), refreshInterval);

        }


        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : IExtension Members -.-.-.-.-.-.-.-.-.-.-

        public string Name {
            get { return "Poster Management"; }
        }

        public IUIExtension UIExtension {
            get {
                return new PosterIntegrationUIExtension();
            }
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : Private Method (s) -.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        private void checkV2(object state) {
            
            
            try {
                
                EpgEntry item = null;
                
                using (EPGManager.Instance.LockDownloadList(false)) {
                    IList<EpgEntry> ars = EPGManager.Instance.EpgCollection;
                    if (ars != null &&
                        ars.Count > 0) {
                        for (int i = 0; i < ars.Count; i++) {
                            
                                if (ars[i].PosterTransferStatus == PosterTransferStatus.PREPARED) {
                                
                                item = ars[i];
                                
                                break;
                            }
                        }
                    }
                }


               

                if (item != null && IsOwned(item)) {

                    AttachDataThroughXMLDataChannelV2(item);

                }
            }
            catch (Exception ex) {
                MainForm.LogExceptionToFile(ex);
            }
        }

        private bool IsOwned(EpgEntry item)
        {
            bool _isOwned = MEBSCatalogProvider.AttemptAcquirePosterLock(item);
            //MainForm.LogWarningToFile(string.Format("Poster - {0} - Is Owned : {1}", item.ID, _isOwned));
            return _isOwned;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="EPG"></param>
        private void AttachDataThroughXMLDataChannelV2(EpgEntry EPG) {
            
            if (EPG == null) {
                MainForm.LogWarningToFile("Attach Data Through XML DataChannelV2 : Empty item.");
                return;
            }

            TryToSendPosterForAutomaticRecordV2(EPG);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epg"></param>
        private void TryToSendPosterForAutomaticRecordV2(EpgEntry epg) {

            //DateTime posterAutomaticRecordlastSent = DateTime.MinValue;
            //DateTime.TryParse(this.parameters.PosterAutomaticRecordLastSent, out posterAutomaticRecordlastSent);
            //TimeSpan ts = DateTime.Now.ToUniversalTime().Subtract(posterAutomaticRecordlastSent);
            //if (ts.TotalMinutes > TimeSpan.FromMinutes(posterlockIntervalInmin).TotalMinutes) {
                
            //}

            PrepareDataFile(epg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="epg"></param>
        private void PrepareDataFile(EpgEntry epg) {
            string fileName = string.Format("{0}{1}", epg.ContentID, epg.PosterFileExtension); //Path.GetExtension(epg.PosterFileName)
            if (PosterFileCreated(epg.Poster, GetPosterFilePath(fileName)) && File.Exists(GetPosterFilePath(fileName))) {
                // success.
                DataTransmissionOverPlayout(epg);

            }
            else {

                EpgEntry e = null;

                using (EPGManager.Instance.LockDownloadList(true)) {
                    IList<EpgEntry> ars = EPGManager.Instance.EpgCollection;
                    if (ars != null &&
                        ars.Count > 0) {
                        for (int i = 0; i < ars.Count; i++) {
                            if (ars[i].ID == epg.ID) {


                                ars[i].PosterTransferStatus = PosterTransferStatus.ERROR;
                                ars[i].PosterTransferDateTime = DateTime.UtcNow;
                                ars[i].PosterTransferTries = 0;

                                 e = ars[i];

                                break;

                            }
                        }
                    }
                }


                if (e != null) {

                    try {
                        MEBSCatalogProvider.SetSchedulePosterTransferMetadata(e);
                    }
                    catch (Exception ex) {
                        MainForm.LogExceptionToFile(ex);
                    }
                }


            }


        }

        private void DataTransmissionOverPlayout(EpgEntry epg) {

            EpgEntry e = null;

            bool _saveSetting = false;

            string data = string.Format("{0}{1}", epg.ContentID, epg.PosterFileExtension);

            string _result = PlayoutCommandProvider.TryToAttachDataThroughFistXMLChannel(data);

            using (EPGManager.Instance.LockDownloadList(true)) {
                IList<EpgEntry> ars = EPGManager.Instance.EpgCollection;
                if (ars != null &&
                    ars.Count > 0) {
                    for (int i = 0; i < ars.Count; i++) {
                        if (ars[i].ID == epg.ID) {

                            if (string.IsNullOrEmpty(_result)) {

                                ars[i].PosterTransferStatus = PosterTransferStatus.ERROR;
                                ars[i].PosterTransferDateTime = DateTime.UtcNow;
                                ars[i].PosterTransferTries = 0;

                            }

                            if (string.Compare(_result, DefaultValues.WS_APP_RESULT_OK) == 0) {

                                ars[i].PosterTransferStatus = PosterTransferStatus.SENT;
                                ars[i].PosterTransferDateTime = DateTime.UtcNow;
                                ars[i].PosterTransferTries = 0;
                                _saveSetting = true;

                            }

                            if (string.Compare(_result, DefaultValues.WS_APP_RESULT_KO) == 0) {

                                ars[i].PosterTransferStatus = PosterTransferStatus.FAILED;
                                ars[i].PosterTransferDateTime = DateTime.UtcNow;
                                ars[i].PosterTransferTries = 0;

                            }

                            e = ars[i];

                            break;

                        }
                    }
                }
            }

            if (e != null) {

                try {
                    MEBSCatalogProvider.SetSchedulePosterTransferMetadata(e);
                }
                catch (Exception ex) {
                    MainForm.LogExceptionToFile(ex);
                }
            }
            if (_saveSetting) {
                this.parameters.PosterAutomaticRecordLastSent = DateTime.UtcNow.ToString();
                Settings.Default.Save();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private string GetPosterFilePath(string fileName) {
           
            string fullPosterPath = string.Format(@"{0}{1}{2}",
                                                    MainForm.Conf.ReqDirectory,
                                                    DefaultValues.IngestedMedia_Directory,
                                                    fileName);

            return fullPosterPath;

        }

        /// <summary>
        /// Create a Poster File in a specific path from a table of byte.
        /// </summary>
        /// <param name="_poster"></param>
        /// <param name="_path"></param>
        /// <returns></returns>
        private bool PosterFileCreated(byte[] _poster, string _path) {
            try {
                int _OutputHeight = this.parameters.ImgOutputHeight;
                int _OutputWidth = this.parameters.ImgOutputWidth;

                if (_OutputWidth == 0) {
                    MainForm.LogWarningToFile("With  param is not configured.");
                    return false;
                }

                if (_OutputHeight == 0) {
                    MainForm.LogWarningToFile("Height param is not configured.");
                    return false;
                }

                if (!Directory.Exists(Path.GetDirectoryName(_path)))
                    Directory.CreateDirectory(Path.GetDirectoryName(_path));

                using (MemoryStream ms = new MemoryStream(_poster)) {
                    Image _returnImage = Image.FromStream(ms);
                    Image _returnImageResized = ResizeImage(_returnImage, new Size(_OutputWidth, _OutputHeight));
                    _returnImageResized.Save(_path);
                    _returnImageResized.Dispose();
                    ms.Close();
                }
                return true;
            }
            catch (Exception ex) {

                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <param name="size"></param>
        /// <param name="preserveAspectRatio"></param>
        /// <returns></returns>
        private static Image ResizeImage(Image image, Size size, bool preserveAspectRatio = false) {
            int newWidth;
            int newHeight;
            if (preserveAspectRatio) {
                int originalWidth = image.Width;
                int originalHeight = image.Height;
                float percentWidth = (float)size.Width / (float)originalWidth;
                float percentHeight = (float)size.Height / (float)originalHeight;
                float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
                newWidth = (int)(originalWidth * percent);
                newHeight = (int)(originalHeight * percent);
            }
            else {
                newWidth = size.Width;
                newHeight = size.Height;
            }
            Image newImage = new Bitmap(newWidth, newHeight);
            using (Graphics graphicsHandle = Graphics.FromImage(newImage)) {
                graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
                graphicsHandle.Dispose();
            }
            return newImage;
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.- Class : IDisposable Members -.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public void Dispose() {
            if (timer != null) {
                timer.Dispose();
                timer = null;
            }
        }
        #endregion

    }
}
