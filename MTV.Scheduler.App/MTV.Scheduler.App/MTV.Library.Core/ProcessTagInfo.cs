using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO;
using System.Security;
using System.Xml;
using System.Collections;
using System.Windows.Forms;
using System.Collections.Specialized;
using System.Web;
using System.Security.Cryptography;
using System.Globalization;
using System.ComponentModel;
using MTV.Library.Core.Common.WebDownload;
using MTV.Library.Core.Common;
using MTV.Library.Core.Services;
using MTV.Scheduler.App.UI;
using MTV.Library.Core.Data;
using MTV.Scheduler.App;
namespace MTV.Library.Core
{    
    public enum ProcessStatus
    {
     Unstarted = 0,
     Waiting = 1,
     Ended = 2,
     Failed = 3,
     WaitingForReconnect = 4,
     DataSavedSuccess = 5,
     PosterDownloadSuccess = 6,
     PosterDownloadError = 7,
     DataSavedError = 8
    };

    public enum RedundancyStatus
    {
        DEFAULT = 0,
        FINISHED = 1,
        RUNNING = 2,
        ERROR = 3,
       

    }

    public enum ProductStatus
     {
        [Description("Default.")]
        DEFAULT = 0,
        [Description("Success.")]
        SUCCESS = 1,
        [Description("Redundancy Error.")]
        REDUNDANCYERROR = 2,
        [Description("Unknown Error.")]
        UNKNOWNERROR = 3,
        [Description("MD5 Hash Error.")]
        MD5HASHERROR = 4,
        [Description("Stream Error.")]
        STREAMERROR = 5,
        [Description("Redundancy File Access Right Error.")]
        REDUNDANCYFILEACCESSRIGHTERROR = 6,
        [Description("Redundancy Launcher Error.")]
        REDUNDANCYLAUNCHERERROR = 7,
        [Description("Redundancy File Not Exist.")]
        REDUNDANCEDFILENOTEXIST = 8,
        [Description("Redundancy File Status Not Exist.")]
        REDUNDANCYFILESTATUSACCESSRIGHTERROR = 9,
       
        
    }


    /// <summary>
    /// 
    /// </summary>
    public class RGRequestTagInfo
    {
        public string id_Hexa;
        public string Input_Path;

    }

    /// <summary>
    /// 
    /// </summary>
    public class JADERequestTagInfo
    {
        public string _fileName;
        public string _channelIndexCount;
        public string _programIndexCount;
        public string _createdOn;
        public string _successChannelsCount;
        public string _failedChannelsCount;
        public string _successEPGCount;
        public string _failedEPGCount;
        public string _status;

    }

   /// <summary>
   /// 
   /// </summary>
    public class RGRequestTagHandler
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static RGRequestTagInfo ReadTag(string filename)
        {
            if (!File.Exists(filename))
                return null;
            RGRequestTagInfo info = new RGRequestTagInfo();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList simpleTags = doc.SelectNodes("/tags/tag/SimpleTag");
            foreach (XmlNode simpleTag in simpleTags)
            {
                string tagName = simpleTag.ChildNodes[0].InnerText;
                switch (tagName)
                {
                    case "IdIngesta":
                        info.id_Hexa = simpleTag.ChildNodes[1].InnerText;
                        break;
                    case "InputPath":
                        info.Input_Path = simpleTag.ChildNodes[1].InnerText;
                        break;


                }
            }
            return info;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class JADERequestTagHandler
    {
        /// <summary>
        /// Read the given JADERequestTagInfo from an XML file
        /// </summary>
        /// <param name="filename">Filename of the XML file</param>
        /// <returns></returns>
        public static JADERequestTagInfo ReadTag(string filename)
        {
            if (!File.Exists(filename))
                return null;
            JADERequestTagInfo info = new JADERequestTagInfo();
            XmlDocument doc = new XmlDocument();
            doc.Load(filename);
            XmlNodeList simpleTags = doc.SelectNodes("/tags/tag/SimpleTag");
            foreach (XmlNode simpleTag in simpleTags)
            {
                string tagName = simpleTag.ChildNodes[0].InnerText;
                switch (tagName)
                {
                    case "JADEFileName":
                        info._fileName = simpleTag.ChildNodes[1].InnerText;
                        break;
                    case "ChannelIndexCount":
                        info._channelIndexCount = simpleTag.ChildNodes[1].InnerText;
                        break;

                    case "ProgramIndexCount":
                        info._programIndexCount = simpleTag.ChildNodes[1].InnerText;
                        break;

                    case "CreatedOn":
                        info._createdOn = simpleTag.ChildNodes[1].InnerText;
                        break;

                    case "Status":
                        info._status = simpleTag.ChildNodes[1].InnerText;
                        break;

                }
            }
            return info;
        }


        /// <summary>
        /// Saves the given JADERequestTagInfo into an XML file
        /// </summary>
        /// <param name="filename">Filename of the XML file</param>
        /// <param name="taginfo">the file process information the xml file should contain</param>
        public static void WriteTag(string filename, JADERequestTagInfo taginfo)
        {
            if (!Directory.Exists(Path.GetDirectoryName(filename)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filename));
            }
            XmlDocument doc = new XmlDocument();
            XmlDeclaration xmldecl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            XmlNode tagsNode = doc.CreateElement("tags");
            XmlNode tagNode = doc.CreateElement("tag");
            tagNode.AppendChild(AddSimpleTag("JADEFileName", taginfo._fileName, doc));
            tagNode.AppendChild(AddSimpleTag("ChannelIndexCount", taginfo._channelIndexCount, doc));
            tagNode.AppendChild(AddSimpleTag("ProgramIndexCount", taginfo._programIndexCount, doc));
            tagNode.AppendChild(AddSimpleTag("CreatedOn", taginfo._createdOn, doc));
            tagNode.AppendChild(AddSimpleTag("SuccessChannelsCount", taginfo._successChannelsCount, doc));
            tagNode.AppendChild(AddSimpleTag("FailedChannelsCount", taginfo._failedChannelsCount, doc));
            tagNode.AppendChild(AddSimpleTag("SuccessEpgCount", taginfo._successEPGCount, doc));
            tagNode.AppendChild(AddSimpleTag("FailedEpgCount", taginfo._failedEPGCount, doc));
            tagNode.AppendChild(AddSimpleTag("Status", taginfo._status, doc));
            tagsNode.AppendChild(tagNode);
            doc.AppendChild(tagsNode);
            doc.InsertBefore(xmldecl, tagsNode);
            doc.Save(filename);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tagName"></param>
        /// <param name="value"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static XmlNode AddSimpleTag(string tagName, string value, XmlDocument doc)
        {
            XmlNode rootNode = doc.CreateElement("SimpleTag");
            XmlNode nameNode = doc.CreateElement("name");
            nameNode.InnerText = tagName;
            XmlNode valueNode = doc.CreateElement("value");
            valueNode.InnerText = value;
            rootNode.AppendChild(nameNode);
            rootNode.AppendChild(valueNode);
            return rootNode;
        }


    }
    
    
    /// <summary>
    /// 
    /// </summary>
    public class ProcessTagInfo
    {
        #region Field(s)
        private object syncRoot = new object();
        Thread mainThread;
        private string _id;
        public string ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string fileName;
        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }
        private string fullFileName;
        public string FullFileName
        {
            get { return fullFileName; }
            set { fullFileName = value; }
        }
        private DateTime createdDateTime;
        public DateTime CreatedDateTime
        {
            get { return createdDateTime; }
            set { createdDateTime = value; }
        }
                private string daemonName;
        public string DaemonName
        {
            get { return daemonName; }
            set { daemonName = value; }
        }    
        private Exception lastError;
        public Exception LastError
        {
            get { return lastError; }
            set { lastError = value; }
        }
        private ProcessStatus stateField;
        public ProcessStatus StateField
        {
            get { return stateField; }
            set { stateField = value; }
        }
        private string scheduleID;
        public string ScheduleID
        {
            get { return scheduleID; }
            set { scheduleID = value; }
        }
        #endregion 

        public ProcessTagInfo()
        {
            this._id = Guid.NewGuid().ToString();
            this.createdDateTime = DateTime.UtcNow;
            this.stateField = ProcessStatus.Unstarted;
            this.scheduleID = string.Empty;
        }

        #region Event(s)

        /// <summary>
        /// 
        /// </summary>
        //public event EventHandler Ending;

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler StateChanged;
       
       
		
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void SetState(ProcessStatus value)
        {
            stateField = value;

            OnStateChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void OnStateChanged()
        {
            if (StateChanged != null)
            {
                StateChanged(this, EventArgs.Empty);
            }
        }

       


        private void StartToWatch()
        {
            Thread mainThread = new Thread(new ParameterizedThreadStart(DoWork));
            mainThread.Start();
        }

        private void DoWork(object sender)
        {
            FileStream fileStream;
           
            
                do
                {

                    lastError = null;
                    SetState(ProcessStatus.Waiting);

                    try
                    {
                        fileStream = File.Open(this.FullFileName,
                                                     FileMode.Open,
                                                     FileAccess.Read,
                                                     FileShare.None);
                        fileStream.Close();
                        SetState(ProcessStatus.Waiting);
                        break; 
                    }
                    catch (SecurityException e1)
                    {
                        lastError = e1;
                        SetState(ProcessStatus.Failed);
                        return;
                    }
                    catch (FileNotFoundException e2)
                    {
                        lastError = e2;
                        SetState(ProcessStatus.Failed);
                        return;
                    }
                    
                    catch (DirectoryNotFoundException e3)
                    {
                        lastError = e3;
                        SetState(ProcessStatus.Failed);
                        return;
                    }
                    catch (IOException ex)
                    {
                        lastError = ex;
                        SetState(ProcessStatus.WaitingForReconnect);
                        Thread.Sleep(TimeSpan.FromSeconds(100));

                    }
                    
                    
                }
                while (true);

                try
                {

                    if (this.daemonName == "ENQUEUE")
                    {

                        // Check if the input file is an xml File
                        String _extension = Path.GetExtension(this.FullFileName);
                        switch (_extension.ToLower())
                        {
                            case ".xml":
                               
                                    Dictionary<string, ChannelPrograms> dChannelPrograms = new Dictionary<string, ChannelPrograms>();
                                    Dictionary<string, Channel> guideChannels = new Dictionary<string, Channel>();
                                    ChannelPrograms newProgChan = new ChannelPrograms();
                                    newProgChan._tsID = string.Empty; newProgChan._onid = string.Empty; newProgChan._id = string.Empty; newProgChan._name = "PVOD";
                                    Channel chan = new Channel(); chan._tsID = "21000"; chan._onid = "126"; chan._id = "2358"; chan._name = "PVOD"; chan._longName = "PUSH VOD CHANNEL"; chan._type = Convert.ToSByte(MediaType.DC_CHANNEL).ToString();
                                    chan._dvb_type = string.Empty; chan._key = "CHNL_VOD"; string _channel_key = "CHNL_VOD";
                                    if (!guideChannels.ContainsKey(_channel_key))
                                    {
                                        guideChannels.Add(_channel_key, chan);
                                        dChannelPrograms.Add(_channel_key, newProgChan);
                                    }
                                    // Parse Products file.
                                    XmlTextReader xmlReader = null;
                                    xmlReader = new XmlTextReader(this.FullFileName);
                                    if (xmlReader.ReadToDescendant("enqueue"))
                                    {
                                        // get the first product
                                        if (xmlReader.ReadToDescendant("request"))
                                        {

                                            do
                                            {
                                                String _node_channelRef = "CHNL_VOD";
                                                String _node_Title = null;
                                                String _node_OriginalTitle = null;
                                                String _node_ProductId = null;
                                                String _node_Genre = null;
                                                String _node_Abstract = null;
                                                String _node_Actors = null;
                                                String _node_Directors = null;
                                                String _node_Year = null;
                                                String _node_country = null;
                                                String _node_StudioName = null;
                                                String _node_AvailabilityDate = null;
                                                String _node_ExpirationDate = null;
                                                String _node_ValidityDate = null;
                                                String _node_ParentalControl = null;
                                                String _node_formatScreen = null;
                                                String _node_HD = null;
                                                String _node_mainImage = null;
                                                String _node_mediaFileName = null;

                                                ChannelPrograms channelPrograms = new ChannelPrograms();

                                                XmlReader _productDetails = xmlReader.ReadSubtree();
                                                _productDetails.ReadStartElement();


                                                while (!_productDetails.EOF)
                                                {

                                                    if (_productDetails.NodeType == XmlNodeType.Element)
                                                    {

                                                        switch (_productDetails.Name)
                                                        {


                                                            case "title":

                                                                _node_Title = _productDetails.ReadString();

                                                                if (_node_Title == null || _node_Title.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product {0} doesnt contain an _node_Title"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;

                                                            case "originalTitle":

                                                                _node_OriginalTitle = _productDetails.ReadString();

                                                                if (_node_OriginalTitle == null || _node_OriginalTitle.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_OriginalTitle"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "productId":

                                                                _node_ProductId = _productDetails.ReadString();

                                                                if (_node_ProductId == null || _node_ProductId.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_ProductId"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "genre":

                                                                _node_Genre = _productDetails.ReadString();

                                                                if (_node_Genre == null || _node_Genre.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_Genre"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;



                                                            case "abstract":

                                                                _node_Abstract = _productDetails.ReadString();

                                                                if (_node_Abstract == null || _node_Abstract.Length == 0)
                                                                {
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_Abstract"));
                                                                    
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "actors":

                                                                _node_Actors = _productDetails.ReadString();

                                                                if (_node_Actors == null || _node_Actors.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_Abstract"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "directors":

                                                                _node_Directors = _productDetails.ReadString();

                                                                if (_node_Directors == null || _node_Directors.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_Directors"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "year":

                                                                _node_Year = _productDetails.ReadString();

                                                                if (_node_Year == null || _node_Year.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_Year"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "country":

                                                                _node_country = _productDetails.ReadString();

                                                                if (_node_country == null || _node_country.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_country"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "studioName":

                                                                _node_StudioName = _productDetails.ReadString();

                                                                if (_node_StudioName == null || _node_StudioName.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_StudioName"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "availabilityDate":

                                                                _node_AvailabilityDate = _productDetails.ReadString();

                                                                if (_node_AvailabilityDate == null || _node_AvailabilityDate.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_AvailabilityDate"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "expirationDate":

                                                                _node_ExpirationDate = _productDetails.ReadString();

                                                                if (_node_ExpirationDate == null || _node_ExpirationDate.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_ExpirationDate"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "validityDate":

                                                                _node_ValidityDate = _productDetails.ReadString();

                                                                if (_node_ValidityDate == null || _node_ValidityDate.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_ValidityDate"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "parentalControl":

                                                                _node_ParentalControl = _productDetails.ReadString();

                                                                if (_node_ParentalControl == null || _node_ParentalControl.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_ParentalControl"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "formatScreen":

                                                                _node_formatScreen = _productDetails.ReadString();

                                                                if (_node_formatScreen == null || _node_formatScreen.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_formatScreen"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "HD":

                                                                _node_HD = _productDetails.ReadString();

                                                                if (_node_HD == null || _node_HD.Length == 0)
                                                                {
                                                                   
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_HD"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;


                                                            case "mainImage":

                                                                _node_mainImage = _productDetails.ReadString();

                                                                if (_node_mainImage == null || _node_mainImage.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an Node_mainImage"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;



                                                            case "mediaFileName":

                                                                _node_mediaFileName = _productDetails.ReadString();

                                                                if (_node_mediaFileName == null || _node_mediaFileName.Length == 0)
                                                                {
                                                                    
                                                                    MainForm.LogWarningToFile(string.Format("Product doesnt contain an _node_mediaFileName"));
                                                                }

                                                                _productDetails.Skip();
                                                                break;

                                                                

                                                            default:
                                                                // unknown, skip entire node
                                                                _productDetails.Skip();
                                                                break;

                                                        }

                                                    }
                                                    else
                                                        _productDetails.Read();


                                                }

                                                if (_productDetails != null)
                                                {
                                                    _productDetails.Close();
                                                    _productDetails = null;
                                                }


                                                if (_node_Title != null && _node_Title.Length > 0 && _node_ProductId != null && _node_ProductId.Length > 0)
                                                {

                                                    if (guideChannels.ContainsKey(_node_channelRef))
                                                    {

                                                        channelPrograms = dChannelPrograms[_node_channelRef];

                                                        Programme _prog = new Programme();
                                                        _prog.Name = ConvertHTMLToAnsi(_node_Title.Trim('"'));
                                                        _prog.Duration = System.Xml.XmlConvert.ToTimeSpan("PT00M00S").ToString();//default(TimeSpan).ToString();
                                                        _prog.StartTime = DateTime.MinValue.ToString();//new DateTime(1971, 11, 6, 23, 59, 59).ToString();
                                                        _prog.ContentRef = _node_ProductId.Trim('"');
                                                        _prog.XmlFileName = this.fileName;
                                                        _prog.ParentalRating = ConvertHTMLToAnsi(_node_ParentalControl.Trim('"'));
                                                        //_prog.AvailabilityDate = (DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6)).ConvertFrom(_node_AvailabilityDate));
                                                        _prog.PublishAfter = CorrectIllegalPublishAfter(_node_AvailabilityDate.Trim('"'));
                                                        _prog.ExpirationDate = (DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6)).ConvertFrom(_node_ExpirationDate.Trim('"')));
                                                        _prog.ValidityDate = CorrectIllegalValidityDate(_node_ValidityDate.Trim('"'));//0; //(DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6)).ConvertFrom(_node_ValidityDate));
                                                        _prog.EventMediaType = MediaType.DC_CHANNEL;
                                                        _prog.MainImage = _node_mainImage.Trim('"');
                                                        _prog.PosterImage = null;
                                                        _prog.PosterFileName = DecodePosterFileName(_node_mainImage.Trim('"'));
                                                        _prog.MediaFileName = ConvertHTMLToAnsi(_node_mediaFileName.Trim('"'));
                                                        //_node_mediaFileName

                                                        _prog.extendInfo.Add("Title", ConvertHTMLToAnsi(_node_OriginalTitle.Trim('"'))); // original Title
                                                        _prog.extendInfo.Add("Abstract", ConvertHTMLToAnsi(_node_Abstract.Trim('"')));
                                                        _prog.extendInfo.Add("Directors", ConvertHTMLToAnsi(_node_Directors.Trim('"')));
                                                        _prog.extendInfo.Add("Actors", ConvertHTMLToAnsi(_node_Actors.Trim('"')));
                                                        _prog.extendInfo.Add("Country", ConvertHTMLToAnsi(_node_country.Trim('"')));
                                                        _prog.extendInfo.Add("creationDate", string.Empty);
                                                        _prog.extendInfo.Add("Year", _node_Year.Trim('"'));
                                                        _prog.extendInfo.Add("Form", string.Empty);
                                                        _prog.extendInfo.Add("Genre", ConvertHTMLToAnsi(_node_Genre.Trim('"')));
                                                        _prog.extendInfo.Add("Duration", string.Empty);                                                    
                                                        //_prog.extendInfo.Add("assetId", _node_ProductId.Trim('"'));
                                                        _prog.extendInfo.Add("assetId_Content", _node_ProductId.Trim('"'));
                                                        _prog.extendInfo.Add("validityDate", string.Empty);
                                                        _prog.extendInfo.Add("lastChanceDuration", string.Empty);
                                                        _prog.extendInfo.Add("hdContent", string.Empty); //  _prog.extendInfo.Add("%HD%", _node_HD);
                                                        _prog.extendInfo.Add("hqContent", string.Empty);
                                                        _prog.extendInfo.Add("Content3D", string.Empty);    
                                                        _prog.extendInfo.Add("businessModels", string.Empty);
                                                        _prog.extendInfo.Add("contentFileSize", "0");
                                                        _prog.extendInfo.Add("forced", string.Empty);
                                                        _prog.extendInfo.Add("briefTitle", ConvertHTMLToAnsi(_node_OriginalTitle.Trim('"'))); // original Title
                                                        _prog.extendInfo.Add("billingId", string.Empty);
                                                        _prog.extendInfo.Add("licensingWindowStart", string.Empty);
                                                        _prog.extendInfo.Add("licensingWindowEnd", string.Empty);                                                        
                                                        _prog.extendInfo.Add("studioName", ConvertHTMLToAnsi(_node_StudioName.Trim('"')));
                                                        _prog.extendInfo.Add("discountRepurchase", "");
                                                        _prog.extendInfo.Add("premiumDiscount", "");
                                                        _prog.extendInfo.Add("bundleDiscount", "");
                                                        _prog.extendInfo.Add("ppvCost", "");
                                                        _prog.extendInfo.Add("audioType", "");
                                                        _prog.extendInfo.Add("languages", "");
                                                        _prog.extendInfo.Add("subtitleLanguages", "");
                                                        _prog.extendInfo.Add("bonusIncluded", "");
                                                        _prog.extendInfo.Add("costInTokens", "");
                                                        _prog.extendInfo.Add("rentalPeriod", "");
                                                        _prog.extendInfo.Add("bookmark", "");
                                                        _prog.extendInfo.Add("CoverFileName", "");

                                                        // To check 
                                                        //_prog.extendInfo.Add("%ScreenFormat%", ConvertHTMLToAnsi(_node_formatScreen));
                                                        //_prog.extendInfo.Add("%MainImage%", ConvertHTMLToAnsi(_node_mainImage));



                                                        //-----.DefaultLogger.JADELogger.Debug(string.Format("New Product received {0} / {1}", _prog.XmlFileName, _prog.StartTime));


                                                        channelPrograms.programs.Add(_prog);
                                                    }

                                                }



                                            } while (xmlReader.ReadToNextSibling("request"));



                                        }
                                    }



                                    int aCounter = 0;

                                    foreach (KeyValuePair<string, Channel> kvp in guideChannels)
                                    {

                                        Channel _chnl = guideChannels[kvp.Key];

                                        //-----.DefaultLogger.JADELogger.Info(string.Format("New channel detected : {0}", _chnl._name));

                                        ChannelCreateStatus _chnlAddStatus = MEBSCatalogProvider.AddNewChannelEntity(_chnl);

                                        if (_chnlAddStatus == ChannelCreateStatus.Success)
                                        {
                                           
                                        }

                                        else
                                        {

                                            //-----.DefaultLogger.JADELogger.Error(string.Format("an error occurred while processing channel {0} / {1}", _chnl._name, _chnlAddStatus));
                                        }


                                        aCounter++;


                                        if (aCounter % 3 == 0)
                                            Thread.Sleep(100);

                                    }


                                    foreach (KeyValuePair<string, ChannelPrograms> kvp in dChannelPrograms)
                                    {
                                      
                                        ChannelPrograms _progChan = kvp.Value;

                                        // empty, skip it
                                        if (_progChan.programs.Count == 0) continue;

                                        // _progChan.programs.Sort();

                                        for (int i = 0; i < _progChan.programs.Count; ++i)
                                        {

                                            Programme prog = (Programme)_progChan.programs[i];
                                            
                                            // Check if ts file exist before adding prog to SA_MEBS
                                            this.lastError = null;
                                            string mediaFileName = MainForm.Conf.ReqDirectory + DefaultValues.ClearMedia_Directory + prog.MediaFileName;

                                            if (!System.IO.File.Exists(mediaFileName))
                                            {
                                                this.lastError = new Exception(string.Format("An error occurred while processing product {0} , Transport stream file not found in | {1}", prog.Name, MainForm.Conf.ReqDirectory));
                                                SetState(ProcessStatus.Failed);
                                                MainForm.buildErrorResponseXmlFile(this.fileName, prog.ContentRef, "ERROR", "REQUEST", string.Format("Transport stream file not found | {0} (System cannot find the file specified)", MainForm.Conf.ReqDirectory + DefaultValues.ClearMedia_Directory + prog.MediaFileName));
                                                continue; // skip it
                                            }

                                             string __buildFullDestinationRedundancyFilePath = MainForm.Conf.ReqDirectory + DefaultValues.IngestedMedia_Directory + @"\" + Utils.GenerateRandomString(10) + Path.GetExtension(prog.MediaFileName);
                                             string __buildFullDestinationRedundancySatutsPath = Program.AppDataPath + @"WebServerRoot\Media\" + @"\" + Path.GetFileNameWithoutExtension(this.FullFileName) + ".txt";
                                             if (RedundancyLauncher.Run(mediaFileName,
                                                                                    __buildFullDestinationRedundancyFilePath,
                                                                                    __buildFullDestinationRedundancySatutsPath))
                                             {

                                                 if (!System.IO.File.Exists(__buildFullDestinationRedundancyFilePath))
                                                 {
                                                     this.lastError = new Exception(string.Format("Redundancy Generator | The output transport stream file not found  | {0} (System cannot find the file specified)", __buildFullDestinationRedundancyFilePath));
                                                     SetState(ProcessStatus.Failed);
                                                     MainForm.buildErrorResponseXmlFile(this.fileName, prog.ContentRef, "ERROR", "SERVER", string.Format("Redundancy Generator | The output transport stream file not found | {0} (System cannot find the file specified)", __buildFullDestinationRedundancyFilePath));
                                                     continue;
                                                 }

                                               
                                                 try
                                                 {
                                                     //check if file can be opened for reading....
                                                     Utils.CheckFileAccessRights(__buildFullDestinationRedundancySatutsPath, FileMode.Open, FileAccess.Read, FileShare.Read);
                                                 }
                                                 catch (Exception e)
                                                 {
                                                     string strErr = string.Format("File {0} doesn't have read access : {1} ", Path.GetFileName(__buildFullDestinationRedundancySatutsPath), e.Message);

                                                     //MEBSCatalogProvider.AddNewMediaFileEntity(this.FileName, default(long), default(long), Convert.ToInt32(ProductStatus.REDUNDANCYFILESTATUSACCESSRIGHTERROR), default(string));
                                                     this.lastError = new Exception(strErr);
                                                     SetState(ProcessStatus.Failed);
                                                     MainForm.buildErrorResponseXmlFile(this.fileName, prog.ContentRef, "ERROR", "SERVER", string.Format("File {0} doesn't have read access : {1} ", Path.GetFileName(__buildFullDestinationRedundancySatutsPath), e.Message));
                                                     continue;
                                                 }

                                                                                                  

                                             }

                                             string strStatus = string.Empty;
                                             
                                             using (TextReader tr = new StreamReader(__buildFullDestinationRedundancySatutsPath))
                                             {
                                                 strStatus = tr.ReadToEnd();
                                             }

                                             if (string.Compare(strStatus, Convert.ToString(RedundancyStatus.FINISHED)) == 0)
                                             {

                                                 MediaFileCreateStatus _mediaFileAddStatus = MEBSCatalogProvider.AddNewMediaFileEntity(prog.MediaFileName, Utils.GetFileSize(mediaFileName), Utils.GetFileSize(__buildFullDestinationRedundancyFilePath), Convert.ToInt32(ProductStatus.SUCCESS), Path.GetFileName(__buildFullDestinationRedundancyFilePath));

                                                 EpgCreateStatus _epgAddStatus = MEBSCatalogProvider.AddNewEpgEntityCascade(kvp.Key, prog);

                                                 if (_mediaFileAddStatus == MediaFileCreateStatus.Success && _epgAddStatus == EpgCreateStatus.Success)
                                                 {
                                                     SetState(ProcessStatus.Ended);

                                                     MainForm.buildSuccessResponseXmlFile(this.fileName, prog.ContentRef, "Success");
                                                 }

                                                 else
                                                 {
                                                     this.lastError = new Exception(string.Format("an error occurred while saving Metadata for product {0} - See log file.", this.fileName));

                                                     SetState(ProcessStatus.Failed);

                                                     MainForm.buildErrorResponseXmlFile(this.fileName, prog.ContentRef, "ERROR", "SERVER", string.Format("An error occurred while saving Metadata for product | {0}", this.fileName));
                                                 }

                                             }
                                             else
                                             {
                                                 SetState(ProcessStatus.Failed);
                                                 
                                                 MainForm.buildErrorResponseXmlFile(this.fileName, prog.ContentRef, "ERROR", "SERVER", string.Format(" RG unkown error product | {0}", this.fileName));
                                             }
                                            
                                           
                                           if (i % 3 == 0)
                                                Thread.Sleep(100);


                                        }


                                    }


                                   // SetState(ProcessStatus.Ended);


                                    if (xmlReader != null)
                                    {
                                        xmlReader.Close();
                                        xmlReader = null;
                                    }
    
                                break;
                            case ".mpg" :
                            case ".mpeg":
                            case ".ts"  :
                                   
                                  
                                   
                                
                            default:
                                     
                                   
                                   break;
                        }
                     
                     



                    }
                    else
                    {

                        // Call JADE Parser.

                        if (this.daemonName == "JADE_REQUEST")
                        {

                            SetState(ProcessStatus.Waiting);
                            lastError = null;

                            XmlTextReader xmlReader = null;
                            //int TransportStreamIndex = 0;
                            int ChannelIndex = 0;
                            int programIndex = 0;
                            //ArrayList Programs = new ArrayList();
                            Dictionary<string, ChannelPrograms> dChannelPrograms = new Dictionary<string, ChannelPrograms>();
                           
                           try
                            {
                                Dictionary<string, Channel> guideChannels = new Dictionary<string, Channel>();
                                //LogHelper.logger.Error(new Exception(string.Format("JADE XML import {0}", this.FullFileName)));
                                
                                                             
                                xmlReader = new XmlTextReader(this.FullFileName);


                                //#region PSI Root


                                //if (xmlReader.ReadToDescendant("PSI"))
                                //{
                                //    // get the first Transport stream
                                //    if (xmlReader.ReadToDescendant("TRANSPORT_STREAM"))
                                //    {
                                                                                                                   
                                //            do
                                //            {
                                //                LogHelper.logger.Error(new Exception("TRANSPORT_STREAM begin read"));

                                //                String _id = xmlReader.GetAttribute("id");

                                //                LogHelper.logger.Error(new Exception(_id));

                                //                if (_id == null || _id.Length == 0)
                                //                {
                                //                   // LogHelper.logger.Error(new Exception(string.Format("TRANSPORT_STREAM #{0} doesnt contain an id")));         
                                //                }

                                //                String _on_id = xmlReader.GetAttribute("on_id");

                                //                LogHelper.logger.Error(new Exception(_on_id));

                                //                if (_on_id == null || _on_id.Length == 0)
                                //                {
                                //                   // LogHelper.logger.Error(new Exception(string.Format("TRANSPORT_STREAM #{0} doesnt contain an on_id", _iTRANSPORT_STREAM)));           
                                //                }

                                //                String _node_Name = null;
                                //                XmlReader _xmlService = xmlReader.ReadSubtree();
                                //                _xmlService.ReadStartElement();

                                //                 // now, xmlProg is positioned on the first sub-element of <SERVICE>

                                //                 while (!_xmlService.EOF)
                                //                 {


                                //                     if (_xmlService.NodeType == XmlNodeType.Element)
                                //                     {

                                //                         switch (_xmlService.Name)
                                //                         {

                                //                             case "SERVICE":

                                //                                      String _node_Id = _xmlService.GetAttribute("id");
                                //                                      LogHelper.logger.Error(new Exception(_node_Id));

                                //                                      String _node_dvb_type = _xmlService.GetAttribute("dvb_type");
                                //                                      LogHelper.logger.Error(new Exception(_node_dvb_type));


                                //                                      XmlReader _xmlprog = xmlReader.ReadSubtree();
                                //                                      _xmlprog.ReadStartElement();

                                //                                      while (!_xmlprog.EOF)
                                //                                      {
                                //                                          if (_xmlprog.NodeType == XmlNodeType.Element)
                                //                                          {
                                //                                              switch (_xmlprog.Name)
                                //                                              {

                                                                                  
                                //                                                  case "SERVICENAME": _node_Name = _xmlprog.ReadString();
                                //                                                                       LogHelper.logger.Error(new Exception(_node_Name));
                                //                                                                      _xmlprog.Skip();
                                //                                                                      break;

                                //                                                  case "EVENT":       String _node_duration = _xmlprog.GetAttribute("duration");
                                //                                                                      LogHelper.logger.Error(new Exception( System.Xml.XmlConvert.ToTimeSpan(_node_duration).ToString())); 
                                //                                                                      String _node_time = _xmlprog.GetAttribute("time");
                                //                                                                      LogHelper.logger.Error(new Exception(_node_time));
                                //                                                                      String _node_id = _xmlprog.GetAttribute("id");
                                //                                                                      LogHelper.logger.Error(new Exception(_node_id));

                                //                                                                      String _node_EventName = null;
                                //                                                                      String _node_ShortDescription = null;
                                //                                                                      String _node_Description = null;
                                //                                                                      String _node_VIDEO_component_tag = null;
                                //                                                                      String _node_VIDEO_description = null;
                                //                                                                      String _node_VIDEO_dvb_type = null;
                                //                                                                      String _node_AUDIO_component_tag = null;
                                //                                                                      String _node_AUDIO_description = null;
                                //                                                                      String _node_AUDIO_dvb_type = null;
                                //                                                                      String _node_KIND_dvb_content = null;
                                //                                                                      String _node_KIND_USER_NIBBLE = null;
                                //                                                                      String _node_PARENTAL_RATING_dvb_rating = null;
                                                                                               
                                                                                                       
                                //                                                                       XmlReader _xmlprogdetails = xmlReader.ReadSubtree();
                                //                                                                       _xmlprogdetails.ReadStartElement();

                                //                                                                       while (!_xmlprogdetails.EOF)
                                //                                                                       {
                                //                                                                           if (_xmlprogdetails.NodeType == XmlNodeType.Element)
                                //                                                                           {
                                //                                                                               switch (_xmlprogdetails.Name)
                                //                                                                               {

                                //                                                                                   case "NAME":
                                //                                                                                        _node_EventName = _xmlprogdetails.ReadString();
                                //                                                                                       LogHelper.logger.Error(new Exception(_node_EventName));
                                //                                                                                       _xmlprogdetails.Skip();
                                //                                                                                        break;

                                //                                                                                   case "SHORT_DESCRIPTION":
                                //                                                                                        _node_ShortDescription = _xmlprogdetails.ReadString();
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_ShortDescription));
                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;

                                //                                                                                   case "DESCRIPTION":
                                //                                                                                        _node_Description = _xmlprogdetails.ReadString();
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_Description));
                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;

                                //                                                                                   case "VIDEO":
                                //                                                                                        _node_VIDEO_component_tag = _xmlprogdetails.GetAttribute("component_tag");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_VIDEO_component_tag));
                                //                                                                                        _node_VIDEO_description = _xmlprogdetails.GetAttribute("description");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_VIDEO_description));
                                //                                                                                        _node_VIDEO_dvb_type = _xmlprogdetails.GetAttribute("dvb_type");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_VIDEO_dvb_type));
                                                                                                                        
                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;


                                //                                                                                   case "AUDIO":
                                //                                                                                        _node_AUDIO_component_tag = _xmlprogdetails.GetAttribute("component_tag");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_AUDIO_component_tag));
                                //                                                                                        _node_AUDIO_description = _xmlprogdetails.GetAttribute("description");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_AUDIO_description));
                                //                                                                                        _node_AUDIO_dvb_type = _xmlprogdetails.GetAttribute("dvb_type");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_AUDIO_dvb_type));

                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;

                                //                                                                                   case "KIND":
                                //                                                                                        _node_KIND_dvb_content = _xmlprogdetails.GetAttribute("dvb_content");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_KIND_dvb_content));
                                //                                                                                        _node_KIND_USER_NIBBLE = _xmlprogdetails.GetAttribute("USER_NIBBLE");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_KIND_USER_NIBBLE));

                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;

                                //                                                                                   case "PARENTAL_RATING":
                                //                                                                                        _node_PARENTAL_RATING_dvb_rating = _xmlprogdetails.GetAttribute("dvb_rating");
                                //                                                                                        LogHelper.logger.Error(new Exception(_node_PARENTAL_RATING_dvb_rating));
                                                                                                                                            
                                //                                                                                        _xmlprogdetails.Skip();
                                //                                                                                        break;


                                //                                                                                   default:
                                //                                                                                       // unknown, skip entire node
                                //                                                                                       _xmlprogdetails.Skip();
                                //                                                                                       break;

                                //                                                                               }


                                //                                                                           }
                                //                                                                           else
                                //                                                                               _xmlprogdetails.Read();



                                //                                                                       }

                                //                                                                       if (_xmlprogdetails != null)
                                //                                                                       {
                                //                                                                           _xmlprogdetails.Close();
                                //                                                                           _xmlprogdetails = null;
                                //                                                                       }


                                //                                                                       ChannelPrograms newProgChan = new ChannelPrograms();
                                //                                                                       newProgChan._tsID = _id;
                                //                                                                       newProgChan._onid = _on_id;
                                //                                                                       newProgChan._id = _node_Id;
                                //                                                                       newProgChan._name = _node_Name;
                                                                                                         
                                //                                                                       Channel chan = new Channel();
                                //                                                                       chan._tsID = _id;
                                //                                                                       chan._onid = _on_id;
                                //                                                                       chan._id = _node_Id;
                                //                                                                       chan._name = _node_Name;
                                //                                                                       chan._dvb_type = _node_dvb_type;

                                //                                                                       string _channel_key = _id + "_" + _on_id + "_" + _node_Id; /* transpot stream _ ONID _ Service ID*/


                                //                                                                       if (!guideChannels.ContainsKey(_channel_key))
                                //                                                                       {
                                //                                                                           guideChannels.Add(_channel_key, chan);
                                //                                                                           dChannelPrograms.Add(_channel_key, newProgChan);

                                //                                                                       }
                                                                                                                          
                                //                                                                       //if (!dChannelPrograms.ContainsKey(_channel_key))
                                //                                                                       //{

                                //                                                                       //    dChannelPrograms.Add(_channel_key, newProgChan);

                                //                                                                       //}


                                //                                                                        Programme _prog = new Programme();
                                //                                                                       _prog.StartTime = _node_time;
                                //                                                                       _prog.Duration = System.Xml.XmlConvert.ToTimeSpan(_node_duration).ToString();   //  According to ISO 8601 standart.
                                //                                                                       _prog.Id = _node_id;
                                //                                                                       _prog.Name = ConvertHTMLToAnsi(_node_EventName);
                                //                                                                       _prog.ShortDescription = ConvertHTMLToAnsi(_node_ShortDescription);
                                //                                                                       _prog.Description = ConvertHTMLToAnsi(_node_Description);
                                //                                                                       _prog.Video_component_tag = _node_VIDEO_component_tag;
                                //                                                                       _prog.Video_description = _node_VIDEO_description;
                                //                                                                       _prog.Video_dvb_type = _node_VIDEO_dvb_type;
                                //                                                                       _prog.Audio_component_tag = _node_AUDIO_component_tag;
                                //                                                                       _prog.Audio_description = _node_AUDIO_description;
                                //                                                                       _prog.Audio_dvb_type = _node_AUDIO_dvb_type;
                                //                                                                       _prog.KIND_dvb_content = _node_KIND_dvb_content;
                                //                                                                       _prog.KIND_USER_NIBBLE = _node_KIND_USER_NIBBLE;
                                //                                                                       _prog.ParentalRating = _node_PARENTAL_RATING_dvb_rating;
                                //                                                                       _prog.Actors = "";
                                //                                                                       _prog.Directors = "";
                                //                                                                       _prog.Year = "";
                                //                                                                       _prog.Country = "";


                                //                                                                       newProgChan = dChannelPrograms[_channel_key];

                                //                                                                       newProgChan.programs.Add(_prog);

                                                                                                                                                                                                            
                                //                                                                      programIndex++; //  


                                //                                                                      _xmlprog.Skip();
                                //                                                                      break;


                                //                                                  default:
                                //                                                      // unknown, skip entire node
                                //                                                      _xmlprog.Skip();
                                //                                                      break;

                                //                                              }

                                //                                          }
                                //                                          else
                                //                                              _xmlprog.Read();



                                //                                      }


                                //                                      if (_xmlprog != null)
                                //                                      {
                                //                                          _xmlprog.Close();
                                //                                          _xmlprog = null;
                                //                                      }

                                                                      

                                //                                      ChannelIndex++ ;

                                //                                     _xmlService.Skip();
                                //                                      break;




                                //                               default:
                                //                               // unknown, skip entire node
                                //                               _xmlService.Skip();
                                //                               break;

                                //                         }





                                //                     }
                                //                      else
                                //                      _xmlService.Read();


                                //                 }

                                //                 if (_xmlService != null)
                                //                 {
                                //                  _xmlService.Close();
                                //                  _xmlService = null;
                                //                 }




                                //            } while (xmlReader.ReadToNextSibling("TRANSPORT_STREAM"));

                                //            LogHelper.logger.Error(new Exception("TRANSPORT_STREAM End read"));


                                //    }  // Element TRANSPORT_STREAM
                                        



                                //    }  // Element PSI


                                //#endregion 

                                #region SCHEDULE Root

                                //-----.DefaultLogger.JADELogger.Debug("Jade Schema V2 ");

                                if (xmlReader.ReadToDescendant("SCHEDULE"))
                                {

                                    //-----.DefaultLogger.JADELogger.Debug("Loading channel list");

                                    String _scheduleDate = xmlReader.GetAttribute("scheduleDate");

                                   // DefaultLogger.JADELogger.Debug(_scheduleDate);

                                    // get the first channel
                                    if (xmlReader.ReadToDescendant("channel"))
                                    {

                                        do
                                        {
                                            String _id = xmlReader.GetAttribute("id");
                                            
                                            if (_id == null || _id.Length == 0)
                                            {
                                                //-----.DefaultLogger.JADELogger.Debug(string.Format("channel#{0} doesnt contain an id", ChannelIndex));
                                            }

                                            String _node_LongName = null;
                                            String _node_ShortName = null;
                                            XmlReader _xmlChannel = xmlReader.ReadSubtree();
                                            _xmlChannel.ReadStartElement();

                                            while (!_xmlChannel.EOF)
                                            {

                                                if (_xmlChannel.NodeType == XmlNodeType.Element)
                                                {

                                                    switch (_xmlChannel.Name)
                                                    {

                                                        case "longName":
               
                                                            _node_LongName = _xmlChannel.ReadString();

                                                            if (_node_LongName == null || _node_LongName.Length == 0)
                                                            {
                                                                //-----.DefaultLogger.JADELogger.Debug(string.Format("channel#{0} doesnt contain an LongName", _node_LongName));
                                                            }
                                                            
                                                            _xmlChannel.Skip();
                                                             break;

                                                        case "shortName":
                                                             
                                                             _node_ShortName = _xmlChannel.ReadString();
                                                             if (_node_ShortName == null || _node_ShortName.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Debug(string.Format("channel#{0} doesnt contain an ShortName", _node_ShortName));
                                                             }
                                                            
                                                             _xmlChannel.Skip();
                                                             break;
                                                        
                                                        default:
                                                            // unknown, skip entire node
                                                            _xmlChannel.Skip();
                                                            break;

                                                    }


                                                }
                                                else
                                                    _xmlChannel.Read();

                                            }

                                            if (_xmlChannel != null)
                                            {
                                                _xmlChannel.Close();
                                                _xmlChannel = null;
                                            }

                                            
                                            ChannelPrograms newProgChan = new ChannelPrograms();
                                            newProgChan._tsID = string.Empty;
                                            newProgChan._onid = string.Empty;
                                            newProgChan._id =   string.Empty;
                                            newProgChan._name = _node_ShortName;

                                            Channel chan = new Channel();
                                            chan._tsID = "0";
                                            chan._onid = "0";
                                            chan._id =   "0";
                                            chan._name = _node_ShortName;
                                            chan._longName = _node_LongName;
                                            chan._dvb_type = string.Empty;
                                            chan._key = _id;
                                            
                                            string _channel_key = _id ; //Guid.NewGuid().ToString();  //_id + "_" + _on_id + "_" + _node_Id; /* transpot stream _ ONID _ Service ID*/

                                            if (!guideChannels.ContainsKey(_channel_key))
                                            {
                                                guideChannels.Add(_channel_key, chan);
                                                dChannelPrograms.Add(_channel_key, newProgChan);
                                            }
                                                             
                                                                                      
                                            ChannelIndex++;
                                           

                                        } while (xmlReader.ReadToNextSibling("channel")); // get the next channel

                                    }



                                }

                                if (xmlReader != null)
                                {
                                    xmlReader.Close();
                                    xmlReader = null;
                                }

                                //-----.DefaultLogger.JADELogger.Debug("Reading TV events");
                                //StringCollection myCol = new StringCollection();
                                List<PROGRAMME_ID> programIdCollection = new List<PROGRAMME_ID>();
                                xmlReader = new XmlTextReader(this.FullFileName);
                                
                                if (xmlReader.ReadToDescendant("SCHEDULE"))
                                {
                                    // get the first EVENT
                                    if (xmlReader.ReadToDescendant("EVENT"))
                                    {

                                         do
                                         {

                                             programIdCollection.Clear();
                                             
                                             ChannelPrograms channelPrograms = new ChannelPrograms();
                                             String _node_channelRef = null;
                                             String _node_contentRef = null;
                                             String _node_duration = null;
                                             String _node_time = null;
                                             String _node_NAME = null;
                                             String _node_SHORT_DESCRIPTION = null;
                                             String _node_DESCRIPTION = null;
                                             String _node_ParentalRating = null;
                                             String _node_ScreenFormat = null;


                                             _node_channelRef = xmlReader.GetAttribute("channelRef");

                                             if (_node_channelRef == null || _node_channelRef.Length == 0)
                                             {
                                                 //-----.DefaultLogger.JADELogger.Error(string.Format("The event#{0} doesnt contain channelRef", programIndex));
                                             }

                                             _node_contentRef = xmlReader.GetAttribute("contentRef");

                                             if (_node_contentRef == null || _node_contentRef.Length == 0)
                                             {
                                                 //-----.DefaultLogger.JADELogger.Error(string.Format("The event#{0} doesnt contain contentRef", programIndex));
                                             }

                                             _node_duration = xmlReader.GetAttribute("duration");

                                             if (_node_duration == null || _node_duration.Length == 0)
                                             {
                                                 //-----.DefaultLogger.JADELogger.Error(string.Format("The event#{0} doesnt contain duration", programIndex));
                                             }

                                             _node_time = xmlReader.GetAttribute("time");

                                             if (_node_time == null || _node_time.Length == 0)
                                             {
                                                 //-----.DefaultLogger.JADELogger.Error(string.Format("The event#{0} doesnt contain time", programIndex));
                                             }

                                             
                                             XmlReader _xmlEventDetails = xmlReader.ReadSubtree();
                                             _xmlEventDetails.ReadStartElement();

                                             while (!_xmlEventDetails.EOF)
                                             {

                                                 if (_xmlEventDetails.NodeType == XmlNodeType.Element)
                                                 {

                                                     switch (_xmlEventDetails.Name)
                                                     {


                                                         case "NAME":

                                                             _node_NAME = _xmlEventDetails.ReadString();

                                                             if (_node_NAME == null || _node_NAME.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an NAME", programIndex));
                                                             }

                                                             _xmlEventDetails.Skip();
                                                             break;



                                                         case "SHORT_DESCRIPTION":

                                                             _node_SHORT_DESCRIPTION = _xmlEventDetails.ReadString();

                                                             if (_node_SHORT_DESCRIPTION == null || _node_SHORT_DESCRIPTION.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an SHORT_DESCRIPTION", programIndex));
                                                             }

                                                             _xmlEventDetails.Skip();
                                                             break;


                                                         case "DESCRIPTION":

                                                             _node_DESCRIPTION = _xmlEventDetails.ReadString();

                                                             if (_node_DESCRIPTION == null || _node_DESCRIPTION.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an DESCRIPTION", programIndex));
                                                             }

                                                             _xmlEventDetails.Skip();
                                                             break;


                                                         case "ParentalRating":

                                                             _node_ParentalRating = _xmlEventDetails.ReadString();

                                                             if (_node_ParentalRating == null || _node_ParentalRating.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an ParentalRating", programIndex));
                                                             }

                                                             _xmlEventDetails.Skip();
                                                             break;



                                                         case "ScreenFormat":

                                                             _node_ScreenFormat = _xmlEventDetails.ReadString();

                                                             if (_node_ScreenFormat == null || _node_ScreenFormat.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an ScreenFormat", programIndex));
                                                             }

                                                             _xmlEventDetails.Skip();
                                                             break;


                                                         case "PROGRAMME_ID":


                                                             String _node_start_tc = _xmlEventDetails.GetAttribute("start_tc");

                                                             if (_node_start_tc == null || _node_start_tc.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an start_tc", programIndex));
                                                             }


                                                             String _node_PROGRAMME_ID = _xmlEventDetails.ReadString();

                                                             if (_node_PROGRAMME_ID == null || _node_PROGRAMME_ID.Length == 0)
                                                             {
                                                                 //-----.DefaultLogger.JADELogger.Warn(string.Format("The event#{0} doesnt contain an PROGRAMME_ID", programIndex));
                                                             }
                                                             else
                                                             {
                                                                 PROGRAMME_ID _PID = new PROGRAMME_ID();
                                                                 _PID.Start_tc = _node_start_tc;
                                                                 _PID.Value = _node_PROGRAMME_ID;
                                                                 programIdCollection.Add(_PID);
                                                             }
                                                             
                                                             _xmlEventDetails.Skip();
                                                             break;




                                                         default:
                                                             // unknown, skip entire node
                                                             _xmlEventDetails.Skip();
                                                             break;

                                                     }

                                                 }
                                                 else
                                                     _xmlEventDetails.Read();

                                             }

                                             if (_xmlEventDetails != null)
                                             {
                                                 _xmlEventDetails.Close();
                                                 _xmlEventDetails = null;
                                             }


                                             if (_node_time != null && _node_time.Length > 0 && _node_duration != null && _node_duration.Length > 0 && programIdCollection != null && programIdCollection.Count > 0)
                                             {

                                                 if (guideChannels.ContainsKey(_node_channelRef))
                                                 {
                                                     channelPrograms = dChannelPrograms[_node_channelRef];

                                                      Programme _prog = new Programme();
                                                     _prog.StartTime = _node_time;
                                                     _prog.Duration = System.Xml.XmlConvert.ToTimeSpan(_node_duration).ToString();   //  According to ISO 8601 standart.
                                                     _prog.Name = ConvertHTMLToAnsi(_node_NAME);
                                                     _prog.ContentRef = _node_contentRef;
                                                     _prog.XmlFileName = this.fileName;
                                                     _prog.EventMediaType = MediaType.TV_CHANNEL;
                                                     _prog.ParentalRating = _node_ParentalRating;
                                                     _prog.PublishAfter = 0; // Default value 
                                                     _prog.ExpirationDate = DateTime.UtcNow.AddDays(7);  //new DateTime(1971, 11, 6, 23, 59, 59); // (DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6 ,23,59,59)).ConvertFrom(DateTime.MinValue.ToString())); // never expired
                                                     _prog.ValidityDate = 0;
                                                     _prog.PosterImage = null;// Default value  //(DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1971, 11, 6)).ConvertFrom(DateTime.MinValue.ToString()));                                            
                                                     
                                                     ///////////////////////////////////////////////////////////////////////////
                                                     /*  DESCRIPTION & SHORT DESCRIPTION LOGIC:
                                                      * 1.  Check if Description value is null or empty 
                                                      * 2.  if Desciption value is null or empty then replace it with  SHORT_Descption value
                                                        3.  Copy Description value into abstract tag
                                                      */
                                                     ///////////////////////////////////////////////////////////////////////////
                                                     _prog.extendInfo.Add("Title", ConvertHTMLToAnsi(_node_NAME));
                                                     if (_node_DESCRIPTION != null && _node_DESCRIPTION.Length > 0) { _prog.extendInfo.Add("Abstract", ConvertHTMLToAnsi(_node_DESCRIPTION)); }
                                                     else { _prog.extendInfo.Add("Abstract", ConvertHTMLToAnsi(_node_SHORT_DESCRIPTION)); }
                                                     _prog.extendInfo.Add("Directors", string.Empty);
                                                     _prog.extendInfo.Add("Actors", string.Empty);
                                                     _prog.extendInfo.Add("Country", string.Empty);
                                                     //_prog.extendInfo.Add("creationDate", string.Empty);
                                                     _prog.extendInfo.Add("Year", string.Empty);
                                                     _prog.extendInfo.Add("Form", string.Empty);
                                                     _prog.extendInfo.Add("Genre", string.Empty);
                                                     _prog.extendInfo.Add("Duration", System.Xml.XmlConvert.ToTimeSpan(_node_duration).TotalSeconds.ToString());
                                                     _prog.extendInfo.Add("hdContent", "false");
                                                     _prog.extendInfo.Add("hqContent", "true");
                                                     _prog.extendInfo.Add("Content3D", "false");
                                                     _prog.extendInfo.Add("CoverFileName", string.Empty);
                                                                                                           
                                                      // To check
                                                     //_prog.extendInfo.Add("%ParentalRating%", _node_ParentalRating);
                                                    // _prog.extendInfo.Add("%ScreenFormat%", _node_ScreenFormat);
                                                     _prog.programIDs.AddRange(programIdCollection);
                                                     channelPrograms.programs.Add(_prog);


                                                 }





                                             }
                                             
                                             
                                             
                                             //for (int i = 0; i < programIdCollection.Count; i++)
                                             //{
                                             //    DefaultLogger.JADELogger.Debug(string.Format("{0}/{1}/{2}", programIdCollection[i].Start_tc, programIdCollection[i].Value, programIndex));
                                             //}

                                                                                                                                                                                    
                                             programIndex++;

                                         } while (xmlReader.ReadToNextSibling("EVENT"));



                                    }

                                }

                                


                                #endregion

                                int _totalSuccessChannels = 0;
                                int _totalFailedChannels = 0;
                                int _totalSuccessEPG = 0;
                                int _totalFailedEPG = 0;
                                int  aCounter = 0 ;

                                foreach (KeyValuePair<string, Channel> kvp in guideChannels)
                                {

                                    Channel _chnl = guideChannels[kvp.Key];

                                    //-----.DefaultLogger.JADELogger.Info(string.Format("New channel detected : {0}", _chnl._name));

                                    ChannelCreateStatus _chnlAddStatus = MEBSCatalogProvider.AddNewChannelEntity(_chnl);

                                    if (_chnlAddStatus == ChannelCreateStatus.Success)
                                    {
                                        _totalSuccessChannels++;
                                    }

                                    else
                                    {
                                        _totalFailedChannels++;
                                        //-----.DefaultLogger.JADELogger.Error(string.Format("an error occurred while processing channel {0} / {1}", _chnl._name, _chnlAddStatus));
                                    }


                                    aCounter++;


                                    if (aCounter % 3 == 0)
                                        Thread.Sleep(100);

                                }



                                foreach (KeyValuePair<string, ChannelPrograms> kvp in dChannelPrograms)
                                {
                                    //-----.LogHelper.logger.Error(new Exception(string.Format("Key :{0} , Value : {1}", kvp.Key, kvp.Value.programs.Count.ToString())));

                                    ChannelPrograms _progChan = kvp.Value;

                                    // empty, skip it
                                    if (_progChan.programs.Count == 0) continue;

                                    // _progChan.programs.Sort();

                                    for (int i = 0; i < _progChan.programs.Count; ++i)
                                    {

                                        Programme prog = (Programme)_progChan.programs[i];
                                        
                                        // Adding prog to CATALOG.

                                        EpgCreateStatus _epgAddStatus = MEBSCatalogProvider.AddNewEpgEntityCascade(kvp.Key, prog);

                                        if (_epgAddStatus == EpgCreateStatus.Success)
                                        {

                                            _totalSuccessEPG++;

                                        }
                                        else
                                        {
                                            _totalFailedEPG++;
                                            //-----.DefaultLogger.JADELogger.Error(string.Format("an error occurred while processing EPG {0} / {1}", prog.Name, _epgAddStatus));
                                        }


                                        if (i % 3 == 0)
                                            Thread.Sleep(100);


                                    }


                                }

                                //-----.DefaultLogger.JADELogger.Info(string.Format("Total Program : {0}, Total service : {1}", programIndex.ToString(), ChannelIndex.ToString()));

                                JADERequestTagInfo _tagInfo = new JADERequestTagInfo();
                                _tagInfo._fileName = this.FileName;
                                _tagInfo._channelIndexCount = ChannelIndex.ToString();
                                _tagInfo._programIndexCount = programIndex.ToString();
                                _tagInfo._successChannelsCount = _totalSuccessChannels.ToString();
                                _tagInfo._failedChannelsCount = _totalFailedChannels.ToString();
                                _tagInfo._successEPGCount = _totalSuccessEPG.ToString();
                                _tagInfo._failedEPGCount = _totalFailedEPG.ToString();
                                _tagInfo._createdOn = this.CreatedDateTime.ToString();
                                _tagInfo._status = "Ended";

                                string _historyFileName = Path.GetDirectoryName(Application.ExecutablePath) + @"\JadeHistory\" + Path.GetFileNameWithoutExtension(this.FileName) + "_R_" + Guid.NewGuid().ToString() + ".xml";
                               
                                JADERequestTagHandler.WriteTag(_historyFileName, _tagInfo);

                                SetState(ProcessStatus.Ended);

                             }

                           
                            catch (Exception ex)
                            {
                                lastError = new Exception(string.Format("INVALID_JADE_XML_FILE {0} , Error : {1} ", this.fileName,ex.Message));
                                SetState(ProcessStatus.Failed);
                                
                                
                            }

                            if (xmlReader != null)
                            {
                                xmlReader.Close();
                                xmlReader = null;
                            }

                           

                        }


                    }



                }

                catch (ThreadAbortException e)
                {
                    lastError = e;
                    SetState(ProcessStatus.Failed);
                }
                catch (Exception ex)
                {
                    lastError = ex;
                    SetState(ProcessStatus.Failed);
                }





        }

              

        public void Start()
        {
            StartToWatch();
        }


        public void WaitForConclusion()
        {
            if (!IsWorking())
            {
                if (mainThread != null && mainThread.IsAlive)
                {
                    mainThread.Join(TimeSpan.FromSeconds(1));
                }
            }

           
        }


        public void ForceEnd()
        {

            if (stateField == ProcessStatus.Waiting || stateField == ProcessStatus.Unstarted || stateField == ProcessStatus.WaitingForReconnect)
            {
                mainThread.Abort();
                mainThread = null;
                SetState(ProcessStatus.Failed);
                return;

            }

            if (stateField == ProcessStatus.Ended || stateField == ProcessStatus.Failed)
            {
                if (mainThread != null && mainThread.IsAlive)
                {
                    mainThread.Join(TimeSpan.FromSeconds(1));
                }
            }

            SetState(ProcessStatus.Ended);
            
        }


        public void End()
        {
            if (stateField == ProcessStatus.Ended || stateField == ProcessStatus.Failed)
            {

                if (mainThread != null && mainThread.IsAlive)
                {
                    mainThread.Join(TimeSpan.FromSeconds(1));

                }
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsWorking()
        {
            ProcessStatus state = this.StateField;
            return (state == ProcessStatus.Unstarted ||
                state == ProcessStatus.WaitingForReconnect ||
                state == ProcessStatus.Waiting);
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string ConvertHTMLToAnsi(string html)
        {
            string strippedHtml = String.Empty;
            ConvertHTMLToAnsi(html, out strippedHtml);
            return strippedHtml;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="html"></param>
        /// <param name="strippedHtml"></param>
        private void ConvertHTMLToAnsi(string html, out string strippedHtml)
        {
            strippedHtml = "";
            //	    int i=0; 
            if (html.Length == 0)
            {
                strippedHtml = "";
                return;
            }
            //int iAnsiPos=0;
            StringWriter writer = new StringWriter();

            System.Web.HttpUtility.HtmlDecode(html, writer);

            String DecodedString = writer.ToString();
            strippedHtml = DecodedString.Replace("<br>", "\n");
            if (true)
                return;
        }


        // <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

       /// <summary>
       ///  Correct Illegal date on wich the product can be viewed.
       /// </summary>
       /// <param name="dt"> Availability Date </param>
        /// <returns> Positive integer number specifying the amount of time (in minutes) </returns>
        private int CorrectIllegalPublishAfter(string publishAfterTimeStr)
        {

            DateTime currentTime = DateTime.Now.ToUniversalTime();
            DateTime publishAfterTime = (DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1990, 5, 6)).ConvertFrom(publishAfterTimeStr));                                
            if (publishAfterTime.CompareTo(currentTime) > 0)
            {
                TimeSpan ts = publishAfterTime.Subtract(currentTime);
                return (int)ts.TotalMinutes;
            }

            // If zero the content is published when it is created.
            return 0;
            
            
        }

        /// <summary>
        /// Correct Illegal date on wich user can purchase product.
        /// </summary>
        /// <param name="validityDatestr"> Validity Date</param>
        /// <returns>Positive integer number specifying the amount of time (in minutes)</returns>
        private int  CorrectIllegalValidityDate(string validityDatestr)
        {
            DateTime currentTime = DateTime.Now.ToUniversalTime();

            DateTime validityDateTime = (DateTime)(System.ComponentModel.TypeDescriptor.GetConverter(new DateTime(1990, 5, 6)).ConvertFrom(validityDatestr));

            if (validityDateTime.CompareTo(currentTime) > 0)
            {
                TimeSpan ts = validityDateTime.Subtract(currentTime);
                return (int)(ts.TotalMinutes);
            }
            
            return 0;
        }

        /// <summary>
        ///  Get file name from URI
        /// </summary>
        /// <param name="HttpURL">http URL</param>
        /// <returns> File Name </returns>
        private string DecodePosterFileName(string HttpURL)
        {
            if (string.IsNullOrEmpty(HttpURL)) return "";
            
            Uri tmp = null;
            try
            {
                tmp = new Uri(HttpURL);
                return HttpUtility.UrlDecode(System.IO.Path.GetFileName(tmp.AbsolutePath));
            }
            catch
            {
                return "";
            }

            
         }

        /// <summary>
        /// Save byte to file on disk.
        /// </summary>
        /// <param name="indata">byte data to save</param>
        /// <param name="targetDirectory">directory where it should be written</param>
        /// <param name="filename">name of file to create</param>
        private void SaveFile(byte[] indata, string targetDirectory, string filename)
        {
            try
            {
                //creates directoris if they dont exists.
                try
                {
                    Directory.CreateDirectory(targetDirectory);
                }
                catch (Exception) { }
                //write byte to disk.
                FileStream fs = new FileStream(@targetDirectory + "/" + filename, FileMode.CreateNew);
                for (int i = 0; i < indata.Length; i++)
                    fs.WriteByte(indata[i]);
                fs.Close();

            }
            catch (Exception e)
            {
                if (!e.Message.EndsWith("already exists."))
                    throw e;
            }
        }

        /// <summary>
        ///  Check if the input file is Media File
        /// </summary>
        /// <param name="strLocalFile">fileName</param>
        /// <returns>True - False</returns>
        private  bool IsMediaFile(string strLocalFile)
        {
            FileInfo fInfo = new FileInfo(strLocalFile);
            if (string.Compare(fInfo.Extension, ".ts") == 0)  
            {
                return true;
            }
            if (string.Compare(fInfo.Extension, ".mpg") == 0) 
            {
                return true;
            }
            if (string.Compare(fInfo.Extension, ".mpeg") == 0) 
            {
                return true;
            }
            if (string.Compare(fInfo.Extension, ".mpg") == 0) 
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Calculates MD5sum.
        /// </summary>
        /// <param name="fullPath">Path of the file.</param>
        /// <returns>MD5sum.</returns>
        private string CalculateMD5Sum(string fullPath)
        {
            using (FileStream fileStream = File.OpenRead(fullPath))
            {
                byte[] output;

                using (MD5 md5 = MD5.Create())
                {
                    output = md5.ComputeHash(fileStream);
                }

                StringBuilder stringBuilder = new StringBuilder();

                for (int i = 0; i < output.Length; i++)
                {
                    stringBuilder.Append(output[i].ToString("x2", CultureInfo.InvariantCulture));
                }

                return stringBuilder.ToString();
            }
        }

        #endregion 

      

        public class ChannelPrograms
        {
            public string _id;    // ServiceID
            public string  _tsID; // TSID
            public string _name; // Name
            public string _onid; // ONID
            
            public ArrayList programs = new ArrayList();
        };


        public class PROGRAMME_ID
        {
            string start_tc;

            public string Start_tc
            {
                get { return start_tc; }
                set { start_tc = value; }
            }
            string value;

            public string Value
            {
                get { return this.value; }
                set { this.value = value; }
            }
        };

        public class Channel
        {
            public string _id;
            public string _type;
            public string _dvb_type;
            public string _tsID;
            public string _name;
            public string _longName;
            public string _onid;
            public string _key; 
         
        };


        public class Programme
        {

            #region Members
            string _startTime;
            string _duration;
            string _name;
            string _parentalRating;
            string  contentRef;
            int _publishAfter;
            DateTime  _expirationDate;
            int _validityDate;     
            MediaType eventMediaType;
            string mainImage;
            byte[] _posterImage;
            string _posterFileName;
            string _mediaFileName;
            #endregion

             #region Public Properties
           
             public string StartTime
            {
                get { return _startTime; }
                set { _startTime = value; }
            }         
            public string Duration
            {
                get { return _duration; }
                set { _duration = value; }
            }
            /// <summary>
            /// Media Name.
            /// </summary>
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
            public string ContentRef
            {
                get { return contentRef; }
                set { contentRef = value; }
            }
            
            /// <summary>
            ///  Media Type autorecording or pushVOD.
            /// </summary>
            public MediaType EventMediaType
            {
                get { return eventMediaType; }
                set { eventMediaType = value; }
            }
            
            /// <summary>
            /// Country regulatory parental rating levels shall be used.  
            /// </summary>
            public string ParentalRating
            {
                get { return _parentalRating; }
                set { _parentalRating = value; }
            }

            /// <summary>
            /// the amount of time (in minutes) after which the content will be published (starting the countdown at the time of content’s creation).
            /// </summary>
            public int PublishAfter
            {
                get { return _publishAfter; }
                set { _publishAfter = value; }
            }

            /// <summary>
            /// Last date on wich the product can be wiewed.
            /// </summary>
            public DateTime ExpirationDate
            {
                get { return _expirationDate; }
                set { _expirationDate = value; }
            }

            /// <summary>
            /// It defines until when user can purchase product.It can be equal toexpiration date.
            /// </summary>
            public int ValidityDate
            {
                get { return _validityDate; }
                set { _validityDate = value; }
            }

            /// <summary>
            /// Download URL
            /// </summary>
            public string MainImage
            {
                get { return mainImage; }
                set { mainImage = value; }
            }

            /// <summary>
            ///  Poster image in byte
            /// </summary>
            public byte[] PosterImage
            {
                get { return _posterImage; }
                set { _posterImage = value; }
            }

            /// <summary>
            /// Poster fileName
            /// </summary>
            public string PosterFileName
            {
                get { return _posterFileName; }
                set { _posterFileName = value; }
            }

            /// <summary>
            /// Media FileName
            /// </summary>
            public string MediaFileName
            {
                get { return _mediaFileName; }
                set { _mediaFileName = value; }
            }

            
            
           /// <summary>
           ///  Metadata XML file name.
           /// </summary>
            public string XmlFileName { get; set; }
            
            /// <summary>
            ///  Extend information Collection.
            /// </summary>
            public NameValueCollection extendInfo = new NameValueCollection();
            
            /// <summary>
            ///  Episode(s) Collection. 
            /// </summary>
            public List<PROGRAMME_ID> programIDs = new List<PROGRAMME_ID>();
            #endregion
        };

    }
}
