using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Collections;
using ThreadState = System.Threading.ThreadState;
using System.Threading;
using System.IO;
using System.Net;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Xml;
using System.Globalization;
using Newtonsoft.Json;
using MTV.Library.Core.UI;
using MTV.Library.Core;
using MTV.Scheduler.App.UI;

namespace MTV.Scheduler.App.HttpServer
{

    public class RemoteCommandEventArgs : EventArgs
    {
        public string Command;
        public int ObjectId;
        public int ObjectTypeId;

        // Constructor
        public RemoteCommandEventArgs(string command, int objectid, int objecttypeid)
        {
            Command = command;
            ObjectId = objectid;
            ObjectTypeId = objecttypeid;
        }
    }

    public class LocalServer
    {
        private static readonly List<Socket> MySockets = new List<Socket>();
        private static List<String> _allowedIPs;
        private static int _socketindex;
        private readonly MainForm _parent;
        public string ServerRoot;
        private Hashtable _mimetypes;
        private TcpListener _myListener;
        public int NumErr;
        private Thread _th;

        //The constructor which make the TcpListener start listening on the
        //given port. It also calls a Thread on the method StartListen(). 
        public LocalServer(MainForm parent)
        {
            _parent = parent;
        }

        public Hashtable MimeTypes
        {
            get
            {
                if (_mimetypes == null)
                {
                    _mimetypes = new Hashtable();
                    using (var sr = new StreamReader(ServerRoot + @"data\mime.Dat"))
                    {
                        string sLine;
                        while ((sLine = sr.ReadLine()) != null)
                        {
                            sLine.Trim();

                            if (sLine.Length > 0)
                            {
                                //find the separator
                                int iStartPos = sLine.IndexOf(";");

                                // Convert to lower case
                                sLine = sLine.ToLower();

                                string sMimeExt = sLine.Substring(0, iStartPos);
                                string sMimeType = sLine.Substring(iStartPos + 1);
                                _mimetypes.Add(sMimeExt, sMimeType);
                            }
                        }
                        sr.Close();
                    }
                }
                return _mimetypes;
            }
        }

        public bool Running
        {
            get
            {
                if (_th == null)
                    return false;
                return _th.IsAlive;
            }
        }

        public string StartServer()
        {
            string message = "";
            try
            {
                if (MainForm.Conf.IPMode == "IPv6")
                {
                    _myListener = new TcpListener(IPAddress.IPv6Any, MainForm.Conf.LANPort) { ExclusiveAddressUse = false };
                    _myListener.AllowNatTraversal(true);
                }
                else
                {
                    _myListener = new TcpListener(IPAddress.Any, MainForm.Conf.LANPort) { ExclusiveAddressUse = false };
                }
                _myListener.Start(200);
            }
            catch (Exception e)
            {
                // MainForm.LogExceptionToFile(e);
                if (_myListener != null)
                {
                    try
                    {
                        _myListener.Stop();
                    }
                    catch (SocketException)
                    {
                    }
                    _myListener = null;
                }
                message = "Could not start local MTV.Scheduler.APP http server - please select a different LAN port in settings. The port specified is in use. See the log file for more information.";
            }
            if (message != "")
            {
                MainForm.LogMessageToFile(message);
                return message;
            }
            try
            {
                //start the thread which calls the method 'StartListen'
                if (_th != null)
                {
                    while (_th.ThreadState == ThreadState.AbortRequested)
                    {
                        Application.DoEvents();
                    }
                }
                _th = new Thread(StartListen);
                _th.Start();
            }
            catch (Exception e)
            {
                message = e.Message;
                MainForm.LogExceptionToFile(e);
            }
            return message;
        }

        public void StopServer()
        {
            for (int i = 0; i < MySockets.Count; i++)
            {
                Socket mySocket = MySockets[i];
                if (mySocket != null)
                {
                    try
                    {
                        if (mySocket.Connected)
                            mySocket.Shutdown(SocketShutdown.Both);
                        mySocket.Close();
                        mySocket = null;
                    }
                    catch
                    {
                        try
                        {
                            mySocket.Close();
                        }
                        catch { }

                        mySocket = null;
                    }
                }
            }
            Application.DoEvents();
            if (_myListener != null)
            {
                try
                {
                    _myListener.Stop();
                    _myListener = null;
                }
                catch
                {
                    _myListener = null;
                }
            }
            Application.DoEvents();
            if (_th != null)
            {
                try
                {
                    if (_th.ThreadState == ThreadState.Running)
                    {
                        _th.Abort();
                        _th.Join(3000);
                    }

                }
                catch
                {
                }
                Application.DoEvents();
                _th = null;
            }
        }

        /// <summary>
        /// This function takes FileName as Input and returns the mime type..
        /// </summary>
        /// <param name="sRequestedFile">To indentify the Mime Type</param>
        /// <returns>Mime Type</returns>
        public string GetMimeType(string sRequestedFile)
        {
            if (sRequestedFile == "")
                return "";
            String sMimeType = "";

            // Convert to lowercase
            sRequestedFile = sRequestedFile.ToLower();

            int iStartPos = sRequestedFile.LastIndexOf(".");
            if (iStartPos == -1)
                return "text/javascript";
            string sFileExt = sRequestedFile.Substring(iStartPos);

            try
            {
                sMimeType = MimeTypes[sFileExt].ToString();
            }
            catch (Exception)
            {
                MainForm.LogErrorToFile("No mime type for request " + sRequestedFile);
            }


            return sMimeType;
        }


        public void SendHeader(string sHttpVersion, string sMimeHeader, int iTotBytes, string sStatusCode, int cacheDays,
                               ref Socket socket)
        {
            String sBuffer = "";

            // if Mime type is not provided set default to text/html
            if (sMimeHeader.Length == 0)
            {
                sMimeHeader = "text/html"; // Default Mime Type is text/html
            }

            sBuffer += sHttpVersion + sStatusCode + "\r\n";
            sBuffer += "Server: mebs\r\n";
            sBuffer += "Content-Type: " + sMimeHeader + "\r\n";
            //sBuffer += "X-Content-Type-Options: nosniff\r\n";
            sBuffer += "Accept-Ranges: bytes\r\n";
            sBuffer += "Access-Control-Allow-Origin: *\r\n";
            if (iTotBytes > -1)
                sBuffer += "Content-Length: " + iTotBytes + "\r\n";
            //sBuffer += "Cache-Control:Date: Tue, 25 Jan 2011 08:18:53 GMT\r\nExpires: Tue, 08 Feb 2011 05:06:38 GMT\r\nConnection: keep-alive\r\n";
            if (cacheDays > 0)
            {
                //this is needed for video content to work in chrome/android
                DateTime d = DateTime.UtcNow;
                sBuffer += "Cache-Control: Date: " + d.ToUniversalTime().ToString("r") +
                           "\r\nLast-Modified: Tue, 01 Jan 2011 12:00:00 GMT\r\nExpires: " +
                           d.AddDays(cacheDays).ToUniversalTime().ToString("r") + "\r\nConnection: keep-alive\r\n";
            }

            sBuffer += "\r\n";

            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);

            SendToBrowser(bSendData, socket);
            //Console.WriteLine("Total Bytes : " + iTotBytes);
        }

        public void SendHeaderWithRange(string sHttpVersion, string sMimeHeader, int iStartBytes, int iEndBytes,
                                        int iTotBytes, string sStatusCode, int cacheDays, Socket socket)
        {
            String sBuffer = "";

            // if Mime type is not provided set default to text/html
            if (sMimeHeader.Length == 0)
            {
                sMimeHeader = "text/html"; // Default Mime Type is text/html
            }

            sBuffer += sHttpVersion + sStatusCode + "\r\n";
            sBuffer += "Server: mebs\r\n";
            sBuffer += "Content-Type: " + sMimeHeader + "\r\n";
            //sBuffer += "X-Content-Type-Options: nosniff\r\n";
            sBuffer += "Accept-Ranges: bytes\r\n";
            sBuffer += "Content-Range: bytes " + iStartBytes + "-" + iEndBytes + "/" + (iTotBytes) + "\r\n";
            sBuffer += "Content-Length: " + (iEndBytes - iStartBytes + 1) + "\r\n";
            if (cacheDays > 0)
            {
                //this is needed for video content to work in chrome/android
                DateTime d = DateTime.UtcNow;
                sBuffer += "Cache-Control: Date: " + d.ToUniversalTime().ToString("r") +
                           "\r\nLast-Modified: Tue, 01 Jan 2011 12:00:00 GMT\r\nExpires: " +
                           d.AddDays(cacheDays).ToUniversalTime().ToString("r") + "\r\nConnection: keep-alive\r\n";
            }

            sBuffer += "\r\n";
            Byte[] bSendData = Encoding.ASCII.GetBytes(sBuffer);

            SendToBrowser(bSendData, socket);
            //Console.WriteLine("Total Bytes : " + iTotBytes);
        }


        /// <summary>
        /// Overloaded Function, takes string, convert to bytes and calls 
        /// overloaded sendToBrowserFunction.
        /// </summary>
        /// <param name="sData">The data to be sent to the browser(client)</param>
        /// <param name="socket">Socket reference</param>
        public void SendToBrowser(String sData, Socket socket)
        {
            SendToBrowser(Encoding.ASCII.GetBytes(sData), socket);
        }


        /// <summary>
        /// Sends data to the browser (client)
        /// </summary>
        /// <param name="bSendData">Byte Array</param>
        /// <param name="socket">Socket reference</param>
        public void SendToBrowser(Byte[] bSendData, Socket socket)
        {
            try
            {
                if (socket.Connected)
                {
                    int sent = socket.Send(bSendData);
                    if (sent < bSendData.Length)
                    {
                        //Debug.WriteLine("Only sent " + sent + " of " + bSendData.Length);
                    }
                    if (sent == -1)
                        MainForm.LogExceptionToFile(new Exception("Socket Error cannot Send Packet"));
                }
            }
            catch (Exception e)
            {
                //Debug.WriteLine("Send To Browser Error: " + e.Message);
                MainForm.LogExceptionToFile(e);
            }
        }

        public bool ThumbnailCallback()
        {
            return false;
        }


        //This method Accepts new connection and
        //First it receives the welcome message from the client,
        //Then it sends the Current date time to the Client.
        public void StartListen()
        {
            String sRequest;
            String sMyWebServerRoot = ServerRoot;
            String sPhysicalFilePath;
            NumErr = 0;

            while (Running && NumErr < 5 && _myListener != null)
            {
                //Accept a new connection
                try
                {
                    Socket mySocket = _myListener.AcceptSocket();
                    if (MainForm.Conf.IPMode == "IPv6")
                        mySocket.SetIPProtectionLevel(IPProtectionLevel.Unrestricted);

                    if (MySockets.Count() < _socketindex + 1)
                    {
                        MySockets.Add(mySocket);
                    }
                    else
                        MySockets[_socketindex] = mySocket;

                    if (mySocket.Connected)
                    {
                        mySocket.NoDelay = true;
                        mySocket.ReceiveBufferSize = 8192;
                        mySocket.ReceiveTimeout = MainForm.Conf.ServerReceiveTimeout;
                        try
                        {
                            //make a byte array and receive data from the client 
                            string sHttpVersion;
                            string sFileName;
                            string resp;
                            String sMimeType;
                            bool bServe, bHasAuth;

                            var bReceive = new Byte[1024];
                            mySocket.Receive(bReceive);
                            string sBuffer = Encoding.ASCII.GetString(bReceive);



                            if (sBuffer.Substring(0, 3) != "GET")
                            {
                                goto Finish;
                            }

                            String sRequestedFile;
                            String sErrorMessage;
                            String sLocalDir;
                            String sDirName;

                            try
                            {
                                ParseRequest(sMyWebServerRoot, sBuffer, out sRequest, out sRequestedFile,
                                             out sErrorMessage,
                                             out sLocalDir, out sDirName, out sPhysicalFilePath, out sHttpVersion,
                                             out sFileName, out sMimeType, out bServe, out bHasAuth, ref mySocket);
                            }
                            catch (Exception ex)
                            {
                                goto Finish;
                            }

                            if (!bServe)
                            {
                                resp = "Access this server locally through MTV.Scheduler.APP";
                                SendHeader(sHttpVersion, "text/html", resp.Length, " 200 OK", 0, ref mySocket);
                                SendToBrowser(resp, mySocket);
                                goto Finish;
                            }

                            //MainForm.LogMessageToFile(sRequest);

                            resp = ProcessCommandInternal(sRequest, sHttpVersion, ref mySocket);

                            //if (resp != "")
                            //{
                            //    //SendHeader(sHttpVersion, "text/javascript", resp.Length, " 200 OK", 0, ref mySocket);
                            //    SendHeader(sHttpVersion, "text/html", resp.Length, " 200 OK", 0, ref mySocket);
                            //    SendToBrowser(resp, mySocket);
                            //}
                            //else //not a js request 



                            if (resp == "")
                            {
                                string cmd = sRequest.Trim('/').ToLower();
                                int i = cmd.IndexOf("?");
                                if (i > -1)
                                    cmd = cmd.Substring(0, i);
                                if (cmd.StartsWith("get /"))
                                    cmd = cmd.Substring(5);

                                int oid, otid;
                                int.TryParse(GetVar(sRequest, "oid"), out oid);
                                int.TryParse(GetVar(sRequest, "ot"), out otid);
                                switch (cmd)
                                {
                                    case "logfile":
                                        //File.WriteAllText(Program.AppPath + "logfileCommand.txt", "logfile"); 
                                        SendLogFile(sHttpVersion, ref mySocket);
                                        break;

                                    case "liveservices":
                                        //frmMain.LogMessageToFile("DONE.");
                                        SendChannelFeed(sHttpVersion, ref mySocket);
                                        break;

                                    case "liveservicesasjson":
                                        //frmMain.LogMessageToFile("DONE.");
                                        //SendChannelFeedAsJson(sHttpVersion, ref mySocket);
                                        break;

                                    case "liveschedules":
                                        //frmMain.LogMessageToFile("DONE.");
                                        SendScheduleFeed(sHttpVersion, ref mySocket);
                                        break;

                                    case "liveschedulesasjson":
                                        //frmMain.LogMessageToFile("DONE.");
                                        SendScheduleFeedasjson(sHttpVersion, ref mySocket);
                                        break;

                                    case "loadimage":
                                        SendImage(sPhysicalFilePath, sHttpVersion, ref mySocket);
                                        break;

                                    default:
                                        if (sPhysicalFilePath.IndexOf('?') != -1)
                                        {
                                            sPhysicalFilePath = sPhysicalFilePath.Substring(0, sPhysicalFilePath.IndexOf('?'));
                                        }

                                        if (!File.Exists(sPhysicalFilePath))
                                        {
                                            ServeNotFound(sHttpVersion, ref mySocket);
                                        }
                                        else
                                        {
                                            ServeFile(sHttpVersion, sPhysicalFilePath, sMimeType, ref mySocket);
                                        }
                                        break;
                                }
                            }

                        Finish:
                            NumErr = 0;
                        }
                        catch (SocketException ex)
                        {
                            //Debug.WriteLine("Server Error (socket): " + ex.Message);
                            MainForm.LogExceptionToFile(ex);
                            NumErr++;
                        }

                        if (MySockets.Count() == _socketindex + 1)
                        {
                            mySocket.Shutdown(SocketShutdown.Both);
                            mySocket.Close();
                            //mySocket = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    //Debug.WriteLine("Server Error (generic): " + ex.Message);
                    //MainForm.LogExceptionToFile(ex); //Server Error (generic):
                    NumErr++;
                }
            }
        }


        private void ServeNotFound(string sHttpVersion, ref Socket mySocket)
        {
            const string resp = "mebs server is running";
            SendHeader(sHttpVersion, "", resp.Length, " 200 OK", 0, ref mySocket);
            SendToBrowser(resp, mySocket);
        }

        public static List<String> AllowedIPs
        {
            get
            {
                if (_allowedIPs != null)
                    return _allowedIPs;

                _allowedIPs = MainForm.Conf.AllowedIPList.Split(',').ToList();
                _allowedIPs.Add("127.0.0.1");
                _allowedIPs.RemoveAll(p => p == "");
                return _allowedIPs;
            }
            set { _allowedIPs = value; }
        }

        private void ParseRequest(String sMyWebServerRoot, string sBuffer, out String sRequest,
                                  out String sRequestedFile, out String sErrorMessage, out String sLocalDir,
                                  out String sDirName, out String sPhysicalFilePath, out string sHttpVersion,
                                  out string sFileName, out String sMimeType, out bool bServe, out bool bHasAuth, ref Socket mySocket)
        {
            sErrorMessage = "";
            string sClientIP = mySocket.RemoteEndPoint.ToString();

            sClientIP = sClientIP.Substring(0, sClientIP.LastIndexOf(":")).Trim();
            sClientIP = sClientIP.Replace("[", "").Replace("]", "");

            bServe = false;
            foreach (var ip in AllowedIPs)
            {
                if (Regex.IsMatch(sClientIP, ip))
                {
                    bServe = true;
                    break;
                }
            }

            int iStartPos = sBuffer.IndexOf("HTTP", 1);

            sHttpVersion = sBuffer.Substring(iStartPos, 8);
            sRequest = sBuffer.Substring(0, iStartPos - 1);
            sRequest.Replace("\\", "/");

            if (sRequest.IndexOf("command.txt") != -1)
            {
                sRequest = sRequest.Replace("Video/", "Video|");
                sRequest = sRequest.Replace("Audio/", "Audio|");
            }
            iStartPos = sRequest.LastIndexOf("/") + 1;
            sRequestedFile = Uri.UnescapeDataString(sRequest.Substring(iStartPos));
            GetDirectoryPath(sRequest, sMyWebServerRoot, out sLocalDir, out sDirName);


            if (sLocalDir.Length == 0)
            {
                sErrorMessage = "<H2>Error!! Requested Directory does not exists</H2><Br>";
                SendHeader(sHttpVersion, "", sErrorMessage.Length, " 404 Not Found", 0, ref mySocket);
                SendToBrowser(sErrorMessage, mySocket);
                throw new Exception("Requested Directory does not exist (" + sLocalDir + ")");
            }

            ParseMimeType(sRequestedFile, out sFileName, out sMimeType);

            sPhysicalFilePath = (sLocalDir + sRequestedFile).Replace("%20", " ").ToLower();

            bHasAuth = sPhysicalFilePath.EndsWith("crossdomain.xml") || CheckAuth(sPhysicalFilePath);
            if (!bServe)
                bServe = bHasAuth;
        }

        private void ServeFile(string sHttpVersion, string sFileName, String sMimeType,
                               ref Socket mySocket)
        {
            var fi = new FileInfo(sFileName);
            int iTotBytes = Convert.ToInt32(fi.Length);

            byte[] bytes;
            using (var fs =
                new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                using (var reader = new BinaryReader(fs))
                {

                    bytes = new byte[fs.Length];
                    while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                    {
                    }
                    reader.Close();
                }
                fs.Close();
            }

            SendHeader(sHttpVersion, sMimeType, iTotBytes, " 200 OK", 20, ref mySocket);
            SendToBrowser(bytes, mySocket);
        }

        private static string GetVar(string url, string var)
        {
            url = url.ToLower();
            var = var.ToLower();

            int i = url.IndexOf("&" + var + "=");
            if (i == -1)
                i = url.IndexOf("?" + var + "=");
            if (i == -1)
            {
                i = url.IndexOf(var);
                if (i == -1)
                    return "";
                i--;
            }

            string txt = url.Substring(i + var.Length + 1).Trim('=');
            if (txt.IndexOf("&") != -1)
                txt = txt.Substring(0, txt.IndexOf("&"));

            return txt;
        }

        internal string ProcessCommandInternal(string sRequest, string sHttpVersion, ref Socket mySocket)
        {

            string cmd = sRequest.Trim('/').ToLower().Trim();
            string resp = "";
            int i = cmd.IndexOf("?");
            if (i != -1)
                cmd = cmd.Substring(0, i);
            if (cmd.StartsWith("get /"))
                cmd = cmd.Substring(5);


            string sd, ed, channelId;


            switch (cmd)
            {
                case "connect":
                    resp = MainForm.Identifier + ",OK";
                    SendHeader(sHttpVersion, "text/html", resp.Length, " 200 OK", 0, ref mySocket);
                    SendToBrowser(resp, mySocket);
                    break;

                case "ping":
                    resp = "PING,OK";
                    SendHeader(sHttpVersion, "text/html", resp.Length, " 200 OK", 0, ref mySocket);
                    SendToBrowser(resp, mySocket);
                    break;


                case "getprograms":

                    sd = GetVar(sRequest, "startdate");
                    ed = GetVar(sRequest, "enddate");

                    MainForm.LogMessageToFile(sd.ToUpper());

                    //DateTime sddt = DateTime.MinValue;  // = DateTime.Now;
                    //DateTime eddt = DateTime.MinValue;  //  =  DateTime.Now.AddDays(3);
                    //try
                    //{
                    //    //  sddt = DateTime.ParseExact(sd, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    //    // sddt = System.Xml.XmlConvert.ToDateTime(sd, XmlDateTimeSerializationMode.Local);
                    //    sddt = DateTime.ParseExact(sd.ToUpper(), "yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MainForm.LogExceptionToFile(ex);
                    //}

                    //try
                    //{
                    //    //eddt = DateTime.ParseExact(ed, "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
                    //    // eddt = System.Xml.XmlConvert.ToDateTime(ed, XmlDateTimeSerializationMode.Local);
                    //    eddt = DateTime.ParseExact(ed.ToUpper(), "yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MainForm.LogExceptionToFile(ex);
                    //}


                    //List<VideoContent> ProgramsToServe = new List<VideoContent>();
                    //using (VideoContentManager.Instance.LockVideoContentList(false))
                    //{

                    //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
                    //    for (int j = 0; j < VideoContents.Count; j++)
                    //    {
                    //        VideoContent vc = VideoContents[j];

                    //        if ((vc.StopDateTime > sddt && vc.StopDateTime < eddt) ||
                    //            (vc.StartDateTime >= sddt && vc.StartDateTime <= eddt) ||
                    //            (vc.StartDateTime <= sddt && vc.StopDateTime >= eddt))

                    //        //if ((vc.StartDateTime >= sddt && vc.StartDateTime <= eddt))
                    //        {
                    //            ProgramsToServe.Add(vc);
                    //        }
                    //    }
                    //}

                    //// genarate resp to send to the client 
                    //StringBuilder sb = new StringBuilder();
                    //CMEScheduler.Core.PlayoutCommandManager.PlayoutCommandProvider.StringWriterWithEncoding stringWriter = new CMEScheduler.Core.PlayoutCommandManager.PlayoutCommandProvider.StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
                    //XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
                    //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                    //xmlWriterSettings.Indent = true;
                    //xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;
                    ////---- Set the XmlWriterSettings to the XMLWriter
                    //xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
                    ///*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
                    //* false = no
                    //* true = yes
                    //*/
                    //xmlWriter.WriteStartDocument(false);
                    //// <VirtualChannel>
                    //xmlWriter.WriteStartElement("VirtualChannel");
                    //xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");
                    //// ADD LOGIC
                    //if (ProgramsToServe.Count > 0)
                    //{
                    //    ProgramsToServe.Sort(
                    //    delegate(VideoContent v1, VideoContent v2)
                    //    {
                    //        return v1.StartDateTime.CompareTo(v2.StartDateTime);
                    //    }
                    //    );

                    //    for (int j = 0; j < ProgramsToServe.Count; j++)
                    //    {

                    //        VideoContent chnl = ProgramsToServe[j];
                    //        // <channel>
                    //        xmlWriter.WriteStartElement("channel");
                    //        xmlWriter.WriteAttributeString("id", chnl.ChannelID);
                    //        // <display-name>
                    //        xmlWriter.WriteStartElement("display-name");
                    //        xmlWriter.WriteString(chnl.ChannelDisplayName.ToString());
                    //        // </display-name>
                    //        xmlWriter.WriteEndElement();
                    //        // </channel>
                    //        xmlWriter.WriteEndElement();

                    //    }

                    //    for (int j = 0; j < ProgramsToServe.Count; j++)
                    //    {

                    //        VideoContent prog = ProgramsToServe[j];
                    //        //<programme>
                    //        xmlWriter.WriteStartElement("programme");
                    //        // id
                    //        xmlWriter.WriteAttributeString("id", prog.ProgrammeID.ToString());
                    //        // start
                    //        xmlWriter.WriteAttributeString("start", prog.StartDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture)); // Convert to SortableDateTime ("2008-03-09T16:05:07") // ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'", DateTimeFormatInfo.InvariantInfo) // ToString("s")
                    //        // stop
                    //        xmlWriter.WriteAttributeString("stop", prog.StopDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture));  // Convert to SortableDateTime ("2008-03-09T16:05:07")

                    //        // episodeCode
                    //        xmlWriter.WriteAttributeString("episodeCode", prog.EpisodeCode.ToString());  // Convert to SortableDateTime ("2008-03-09T16:05:07")

                    //        // nodeCode
                    //        xmlWriter.WriteAttributeString("nodeCode", prog.NodeCode.ToString());

                    //        // channel
                    //        xmlWriter.WriteAttributeString("channel", prog.ChannelID.ToString());

                    //        //<title = ShowEpisode>
                    //        xmlWriter.WriteStartElement("showEpisode");
                    //        xmlWriter.WriteAttributeString("lang", "en");
                    //        xmlWriter.WriteString(prog.ProgrammeTitle.ToString());
                    //        //</title = ShowEpisode>
                    //        xmlWriter.WriteEndElement();

                    //        //<ShowNode>
                    //        xmlWriter.WriteStartElement("showNode");
                    //        xmlWriter.WriteAttributeString("lang", "en");
                    //        xmlWriter.WriteString(prog.Node.ToString());
                    //        //</ShowNode>
                    //        xmlWriter.WriteEndElement();


                    //        // <desc>
                    //        xmlWriter.WriteStartElement("desc");
                    //        xmlWriter.WriteAttributeString("lang", "en");
                    //        if (!string.IsNullOrEmpty(prog.ProgrammeDesc))
                    //            xmlWriter.WriteString(prog.ProgrammeDesc.ToString());
                    //        else
                    //            xmlWriter.WriteString(string.Empty);
                    //        //</desc>
                    //        xmlWriter.WriteEndElement();

                    //        //<posterFileName>
                    //        xmlWriter.WriteStartElement("posterFileName");

                    //        Uri uri = new Uri(prog.ImageURL);
                    //        string url1 = uri.GetLeftPart(UriPartial.Path);
                    //        string imageFileName = Path.GetFileName(url1);
                    //        string imageExtension = Path.GetExtension(imageFileName);

                    //        xmlWriter.WriteString(prog.ProgrammeID.ToString() + imageExtension);
                    //        //</posterFileName>
                    //        xmlWriter.WriteEndElement();

                    //        // <videolink>
                    //        xmlWriter.WriteStartElement("videolink");
                    //        if (!string.IsNullOrEmpty(prog.VideoLink))
                    //            xmlWriter.WriteString(prog.VideoLink.ToString());
                    //        else
                    //            xmlWriter.WriteString(string.Empty);
                    //        //</videolink>
                    //        xmlWriter.WriteEndElement();

                    //        //</programme>
                    //        xmlWriter.WriteEndElement();
                    //    }
                    //}

                    //// END LOGIC 

                    //// </VirtualChannel>
                    //xmlWriter.WriteEndElement();

                    //xmlWriter.WriteEndDocument();
                    //xmlWriter.Close();

                    //resp = stringWriter.ToString();

                    //SendHeader(sHttpVersion, "text/xml", resp.Length, " 200 OK", 20, ref mySocket);
                    //SendToBrowser(resp, mySocket);


                    //resp = "getcontentlist,OK";

                    break;

                case "getnewestprogramforchannel":

                    channelId = GetVar(sRequest, "chnl");
                    resp = Newestprogramforchannel(channelId);
                    SendHeader(sHttpVersion, "text/xml", resp.Length, " 200 OK", 0, ref mySocket);
                    SendToBrowser(resp, mySocket);
                    break;


                case "getnewestprogramforchannelasjson":

                    channelId = GetVar(sRequest, "chnl");

                    resp = Newestprogramforchannel(channelId);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(resp);
                    string jsonresp = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);

                    SendHeader(sHttpVersion, "application/json", jsonresp.Length, " 200 OK", 20, ref mySocket);
                    SendToBrowser(jsonresp, mySocket);
                    break;



                case "getonairnow":

                    //List<VideoContent> OnairNowlist = new List<VideoContent>();

                    //using (VideoContentManager.Instance.LockVideoContentList(false))
                    //{
                    //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
                    //    for (int j = 0; j < VideoContents.Count; j++)
                    //    {
                    //        VideoContent vc = VideoContents[j];

                    //        if (vc.StartDateTime <= DateTime.Now && vc.StopDateTime >= DateTime.Now)
                    //        {
                    //            OnairNowlist.Add(vc);
                    //        }

                    //    }
                    //}


                    //resp = ServeOnairNow(OnairNowlist);
                    ////resp = "GetOnairNow,OK";
                    //SendHeader(sHttpVersion, "text/xml", resp.Length, " 200 OK", 0, ref mySocket);
                    //SendToBrowser(resp, mySocket);
                    break;

                case "getonairnowasjson":

                    //List<VideoContent> OnairNowlistJ = new List<VideoContent>();

                    //using (VideoContentManager.Instance.LockVideoContentList(false))
                    //{
                    //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
                    //    for (int j = 0; j < VideoContents.Count; j++)
                    //    {
                    //        VideoContent vc = VideoContents[j];

                    //        if (vc.StartDateTime <= DateTime.Now && vc.StopDateTime >= DateTime.Now)
                    //        {
                    //            OnairNowlistJ.Add(vc);
                    //        }

                    //    }
                    //}


                    //resp = ServeOnairNow(OnairNowlistJ);

                    //XmlDocument docj = new XmlDocument();
                    //docj.LoadXml(resp);
                    //string jsonres = Newtonsoft.Json.JsonConvert.SerializeXmlNode(docj);

                    //SendHeader(sHttpVersion, "application/json", jsonres.Length, " 200 OK", 20, ref mySocket);
                    //SendToBrowser(jsonres, mySocket);
                    break;


            }

            return resp;
        }

        private static string GetDirectory(int objectTypeId, int objectId)
        {

            return "";
        }

        private static void GetDirectoryPath(String sRequest, String sMyWebServerRoot, out String sLocalDir,
                                             out String sDirName)
        {
            try
            {
                sDirName = sRequest.Substring(sRequest.IndexOf("/"));
                sDirName = sDirName.Substring(0, sDirName.LastIndexOf("/"));

                if (sDirName == "/")
                    sLocalDir = sMyWebServerRoot;
                else
                {
                    if (sDirName.ToLower().StartsWith(@"/video/"))
                    {
                        sLocalDir = Program.AppDataPath + @"WebServerRoot\Media\" + "video\\";
                        string sfile = sRequest.Substring(sRequest.LastIndexOf("/") + 1);
                        int iind = Convert.ToInt32(sfile.Substring(0, sfile.IndexOf("_")));
                        sLocalDir += GetDirectory(2, iind) + "\\";
                        if (sfile.Contains(".jpg"))
                            sLocalDir += "thumbs\\";
                    }
                    else
                    {
                        if (sDirName.ToLower().StartsWith(@"/audio/"))
                        {
                            sLocalDir = MainForm.Conf.MediaDirectory + "audio\\";
                            string sfile = sRequest.Substring(sRequest.LastIndexOf("/") + 1);
                            int iind = Convert.ToInt32(sfile.Substring(0, sfile.IndexOf("_")));
                            sLocalDir += GetDirectory(1, iind) + "\\";
                        }
                        else
                            sLocalDir = sMyWebServerRoot + sDirName.Replace("../", "").Replace("/", @"\");
                    }
                }
            }
            catch (Exception ex)
            {
                MainForm.LogErrorToFile("Failed to get path for request: " + sRequest + " (" + sMyWebServerRoot + ") - " + ex.Message);
                sLocalDir = "";
                sDirName = "";
            }
        }

        private void ParseMimeType(String sRequestedFile, out string sFileName, out String sMimeType)
        {
            sFileName = sRequestedFile;


            if (sFileName.IndexOf("?") != -1)
                sFileName = sFileName.Substring(0, sFileName.IndexOf("?"));
            if (sFileName.IndexOf("&") != -1)
                sFileName = sFileName.Substring(0, sFileName.IndexOf("&"));

            sMimeType = GetMimeType(sFileName);
            if (sMimeType == "")
                sMimeType = "text/javascript";
        }

        private static bool CheckAuth(String sPhysicalFilePath)
        {
            string auth = "";
            if (sPhysicalFilePath.IndexOf("auth=") != -1)
            {
                auth = sPhysicalFilePath.Substring(sPhysicalFilePath.IndexOf("auth=") + 5).Trim('\\');
            }

            if (auth.IndexOf("&") != -1)
                auth = auth.Substring(0, auth.IndexOf("&"));
            if (auth.IndexOf("?") != -1)
                auth = auth.Substring(0, auth.IndexOf("?"));
            if (auth.IndexOf("/") != -1)
                auth = auth.Substring(0, auth.IndexOf("/"));
            if (auth.IndexOf("\\") != -1)
                auth = auth.Substring(0, auth.IndexOf("\\"));

            return auth == MainForm.Identifier;
        }

        private void SendLogFile(string sHttpVersion, ref Socket mySocket)
        {
            var fi = new FileInfo(Program.AppDataPath + "log_" + MainForm.NextLog + ".htm");
            int iTotBytes = Convert.ToInt32(fi.Length);
            byte[] bytes;
            using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {

                using (var reader = new BinaryReader(fs))
                {
                    bytes = new byte[iTotBytes];
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    bytes = reader.ReadBytes(bytes.Length);
                    reader.Close();
                }
                fs.Close();
            }

            SendHeader(sHttpVersion, "text/html", iTotBytes, " 200 OK", 20, ref mySocket);
            SendToBrowser(bytes, mySocket);
        }


        private void SendImage(String sPhysicalFilePath, string sHttpVersion, ref Socket mySocket)
        {
            string fn = GetVar(sPhysicalFilePath, "fn");

            // frmMain.LogMessageToFile("sPhysicalFilePath" + sPhysicalFilePath);

            // frmMain.LogMessageToFile("fn"+ fn);



            string sFileName = Program.AppDataPath + @"WebServerRoot\Media\" + "Video/" + "thumbs/" +
                                       fn;

            MainForm.LogMessageToFile("sFileName" + sFileName);

            if (!File.Exists(sFileName))
            {
                sFileName = Program.AppPath + @"WebServerRoot\notfound.jpg";
            }

            string smimeType = GetMimeTypefromfile(sFileName);

            //frmMain.LogMessageToFile("smimeType" + smimeType);

            using (var fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                // Create a reader that can read bytes from the FileStream.

                using (var reader = new BinaryReader(fs))
                {
                    var bytes = new byte[fs.Length];
                    while ((reader.Read(bytes, 0, bytes.Length)) != 0)
                    {
                    }
                    SendHeader(sHttpVersion, smimeType, bytes.Length, " 200 OK", 30, ref mySocket);

                    SendToBrowser(bytes, mySocket);
                    reader.Close();
                }
                fs.Close();
            }

        }

        private string GetMimeTypefromfile(string fileName)
        {
            string mimeType = "application/unknown";
            string ext = System.IO.Path.GetExtension(fileName).ToLower();
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null)
                mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        private string Newestprogramforchannel(string chnl_id)
        {
            string resp = "";

            //// genarate resp to send to the client 
            //StringBuilder sb = new StringBuilder();
            //StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            //XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Indent = true;
            //xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;
            ////---- Set the XmlWriterSettings to the XMLWriter
            //xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
            ///*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            //* false = no
            //* true = yes
            //*/
            //xmlWriter.WriteStartDocument(false);
            //// <VirtualChannel>
            //xmlWriter.WriteStartElement("VirtualChannel");
            //xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");


            //VideoContent item = VideoContentManager.Instance.getnewestprogramforchannel(chnl_id);
            //if (item != null)
            //{
            //    //Build channel item 
            //    // <channel>
            //    xmlWriter.WriteStartElement("channel");
            //    xmlWriter.WriteAttributeString("id", item.ChannelID);
            //    // <display-name>
            //    xmlWriter.WriteStartElement("display-name");
            //    xmlWriter.WriteString(item.ChannelDisplayName.ToString());
            //    // </display-name>
            //    xmlWriter.WriteEndElement();
            //    // </channel>
            //    xmlWriter.WriteEndElement();


            //    // Build program item
            //    //<programme>
            //    xmlWriter.WriteStartElement("programme");
            //    // id
            //    xmlWriter.WriteAttributeString("id", item.ProgrammeID.ToString());
            //    // start
            //    xmlWriter.WriteAttributeString("start", item.StartDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture)); // Convert to SortableDateTime ("2008-03-09T16:05:07")
            //    // stop
            //    xmlWriter.WriteAttributeString("stop", item.StopDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture));  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //    // episodeCode
            //    xmlWriter.WriteAttributeString("episodeCode", item.EpisodeCode.ToString());  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //    // nodeCode
            //    xmlWriter.WriteAttributeString("nodeCode", item.NodeCode.ToString());

            //    // channel
            //    xmlWriter.WriteAttributeString("channel", item.ChannelID.ToString());

            //    //<title : showEpisode>
            //    xmlWriter.WriteStartElement("showEpisode");
            //    xmlWriter.WriteAttributeString("lang", "en");
            //    xmlWriter.WriteString(item.ProgrammeTitle.ToString());
            //    //</title : showEpisode>
            //    xmlWriter.WriteEndElement();


            //    //<ShowNode>
            //    xmlWriter.WriteStartElement("showNode");
            //    xmlWriter.WriteAttributeString("lang", "en");
            //    xmlWriter.WriteString(item.Node.ToString());
            //    //</ShowNode>
            //    xmlWriter.WriteEndElement();



            //    // <desc>
            //    xmlWriter.WriteStartElement("desc");
            //    xmlWriter.WriteAttributeString("lang", "en");
            //    if (!string.IsNullOrEmpty(item.ProgrammeDesc))
            //        xmlWriter.WriteString(item.ProgrammeDesc.ToString());
            //    else
            //        xmlWriter.WriteString(string.Empty);
            //    //</desc>
            //    xmlWriter.WriteEndElement();


            //    //<posterFileName>
            //    xmlWriter.WriteStartElement("posterFileName");

            //    Uri uri = new Uri(item.ImageURL);
            //    string url1 = uri.GetLeftPart(UriPartial.Path);
            //    string imageFileName = Path.GetFileName(url1);
            //    string imageExtension = Path.GetExtension(imageFileName);

            //    xmlWriter.WriteString(item.ProgrammeID.ToString() + imageExtension);
            //    //</posterFileName>
            //    xmlWriter.WriteEndElement();

            //    // <videolink>
            //    xmlWriter.WriteStartElement("videolink");
            //    if (!string.IsNullOrEmpty(item.VideoLink))
            //        xmlWriter.WriteString(item.VideoLink.ToString());
            //    else
            //        xmlWriter.WriteString(string.Empty);
            //    //</videolink>
            //    xmlWriter.WriteEndElement();

            //    //</programme>
            //    xmlWriter.WriteEndElement();



            //}

            //// </VirtualChannel>
            //xmlWriter.WriteEndElement();

            //xmlWriter.WriteEndDocument();
            //xmlWriter.Close();

            //resp = stringWriter.ToString();

            return resp;
        }


        //private string ServeOnairNow(List<VideoContent> L)
        //{
        //    string resp = string.Empty;

        //    // genarate resp to send to the client 
        //    StringBuilder sb = new StringBuilder();
        //    StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
        //    XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
        //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
        //    xmlWriterSettings.Indent = true;
        //    xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;
        //    //---- Set the XmlWriterSettings to the XMLWriter
        //    xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
        //    /*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
        //    * false = no
        //    * true = yes
        //    */
        //    xmlWriter.WriteStartDocument(false);
        //    // <VirtualChannel>
        //    xmlWriter.WriteStartElement("VirtualChannel");
        //    xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");

        //    if (L.Count > 0)
        //    {
        //        L.Sort(
        //        delegate(VideoContent v1, VideoContent v2)
        //        {
        //            return v1.StartDateTime.CompareTo(v2.StartDateTime);
        //        }
        //        );

        //        for (int j = 0; j < L.Count; j++)
        //        {

        //            VideoContent chnl = L[j];
        //            // <channel>
        //            xmlWriter.WriteStartElement("channel");
        //            xmlWriter.WriteAttributeString("id", chnl.ChannelID);
        //            // <display-name>
        //            xmlWriter.WriteStartElement("display-name");
        //            xmlWriter.WriteString(chnl.ChannelDisplayName.ToString());
        //            // </display-name>
        //            xmlWriter.WriteEndElement();
        //            // </channel>
        //            xmlWriter.WriteEndElement();

        //        }

        //        for (int j = 0; j < L.Count; j++)
        //        {

        //            VideoContent prog = L[j];
        //            //<programme>
        //            xmlWriter.WriteStartElement("programme");
        //            // id
        //            xmlWriter.WriteAttributeString("id", prog.ProgrammeID.ToString());
        //            // start
        //            xmlWriter.WriteAttributeString("start", prog.StartDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture)); // Convert to SortableDateTime ("2008-03-09T16:05:07")
        //            // stop
        //            xmlWriter.WriteAttributeString("stop", prog.StopDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture));  // Convert to SortableDateTime ("2008-03-09T16:05:07")

        //            // episodeCode
        //            xmlWriter.WriteAttributeString("episodeCode", prog.EpisodeCode.ToString());  // Convert to SortableDateTime ("2008-03-09T16:05:07")

        //            // nodeCode
        //            xmlWriter.WriteAttributeString("nodeCode", prog.NodeCode.ToString());

        //            // channel
        //            xmlWriter.WriteAttributeString("channel", prog.ChannelID.ToString());

        //            //<title : showEpisode>
        //            xmlWriter.WriteStartElement("showEpisode");
        //            xmlWriter.WriteAttributeString("lang", "en");
        //            xmlWriter.WriteString(prog.ProgrammeTitle.ToString());
        //            //</title : showEpisode>
        //            xmlWriter.WriteEndElement();

        //            //<ShowNode>
        //            xmlWriter.WriteStartElement("showNode");
        //            xmlWriter.WriteAttributeString("lang", "en");
        //            xmlWriter.WriteString(prog.Node.ToString());
        //            //</ShowNode>
        //            xmlWriter.WriteEndElement();


        //            // <desc>
        //            xmlWriter.WriteStartElement("desc");
        //            xmlWriter.WriteAttributeString("lang", "en");
        //            if (!string.IsNullOrEmpty(prog.ProgrammeDesc))
        //                xmlWriter.WriteString(prog.ProgrammeDesc.ToString());
        //            else
        //                xmlWriter.WriteString(string.Empty);
        //            //</desc>
        //            xmlWriter.WriteEndElement();


        //            //<posterFileName>
        //            xmlWriter.WriteStartElement("posterFileName");

        //            Uri uri = new Uri(prog.ImageURL);
        //            string url1 = uri.GetLeftPart(UriPartial.Path);
        //            string imageFileName = Path.GetFileName(url1);
        //            string imageExtension = Path.GetExtension(imageFileName);

        //            xmlWriter.WriteString(prog.ProgrammeID.ToString() + imageExtension);
        //            //</posterFileName>
        //            xmlWriter.WriteEndElement();


        //            // <videolink>
        //            xmlWriter.WriteStartElement("videolink");
        //            if (!string.IsNullOrEmpty(prog.VideoLink))
        //                xmlWriter.WriteString(prog.VideoLink.ToString());
        //            else
        //                xmlWriter.WriteString(string.Empty);
        //            //</videolink>
        //            xmlWriter.WriteEndElement();

        //            //</programme>
        //            xmlWriter.WriteEndElement();
        //        }
        //    }


        //    // </VirtualChannel>
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Close();

        //    resp = stringWriter.ToString();


        //    return resp;
        //}

        //private void SendChannelFeedAsJson(string sHttpVersion, ref Socket mySocket)
        //{

        //    string resp = "";
        //    List<Channel> ChannelFeedToServe = new List<Channel>();
        //    using (VideoContentManager.Instance.LockVideoContentList(false))
        //    {
        //        IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
        //        for (int i = 0; i < VideoContents.Count; i++)
        //        {
        //            VideoContent vc = VideoContents[i];
        //            Channel chnl = new Channel();
        //            chnl.ID = vc.ChannelID;
        //            chnl.DisplayName = vc.ChannelDisplayName;
        //            bool containsItem = ChannelFeedToServe.Any(item => item.ID == chnl.ID);
        //            if (!containsItem)
        //                ChannelFeedToServe.Add(chnl);

        //        }
        //    }

        //    // genarate resp to send 
        //    StringBuilder sb = new StringBuilder();
        //    StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
        //    XmlWriter xmlWriter = new XmlTextWriter(stringWriter);


        //    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
        //    xmlWriterSettings.Indent = true;
        //    xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

        //    //---- Set the XmlWriterSettings to the XMLWriter
        //    xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

        //    /*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
        //    * false = no
        //    * true = yes
        //    */
        //    xmlWriter.WriteStartDocument(false);
        //    // <VirtualChannel>
        //    xmlWriter.WriteStartElement("VirtualChannel");
        //    xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");

        //    //xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance );
        //    //xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/DCInfo_schema.xsd");

        //    if (ChannelFeedToServe.Count > 0)
        //    {

        //        for (int i = 0; i < ChannelFeedToServe.Count; i++)
        //        {

        //            Channel chnl = ChannelFeedToServe[i];
        //            // <channel>
        //            xmlWriter.WriteStartElement("channel");
        //            xmlWriter.WriteAttributeString("id", chnl.ID);
        //            // <display-name>
        //            xmlWriter.WriteStartElement("display-name");
        //            xmlWriter.WriteString(chnl.DisplayName.ToString());
        //            // </display-name>
        //            xmlWriter.WriteEndElement();

        //            // </channel>
        //            xmlWriter.WriteEndElement();

        //        }
        //    }

        //    // </VirtualChannel>
        //    xmlWriter.WriteEndElement();

        //    xmlWriter.WriteEndDocument();
        //    xmlWriter.Close();

        //    resp = stringWriter.ToString();

        //    // convert xml to json
        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml(resp);
        //    string jsonresp = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
        //    SendHeader(sHttpVersion, "application/json", jsonresp.Length, " 200 OK", 20, ref mySocket);
        //    SendToBrowser(jsonresp, mySocket);



        //}

        private void SendChannelFeed(string sHttpVersion, ref Socket mySocket)
        {
            //var fi = new FileInfo(Program.AppDataPath  + "TVGuide.xml");
            //int iTotBytes = Convert.ToInt32(fi.Length);
            //byte[] bytes;
            //using (var fs = new FileStream(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            //{

            //    using (var reader = new BinaryReader(fs))
            //    {
            //        bytes = new byte[iTotBytes];
            //        reader.BaseStream.Seek(0, SeekOrigin.Begin);
            //        bytes = reader.ReadBytes(bytes.Length);
            //        reader.Close();
            //    }
            //    fs.Close();
            //}

            //SendHeader(sHttpVersion, "text/xml", iTotBytes, " 200 OK", 20, ref mySocket);
            //SendToBrowser(bytes, mySocket);

            string resp = "";
            //List<Channel> ChannelFeedToServe = new List<Channel>();
            //using (VideoContentManager.Instance.LockVideoContentList(false))
            //{
            //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
            //    for (int i = 0; i < VideoContents.Count; i++)
            //    {
            //        VideoContent vc = VideoContents[i];
            //        Channel chnl = new Channel();
            //        chnl.ID = vc.ChannelID;
            //        chnl.DisplayName = vc.ChannelDisplayName;
            //        bool containsItem = ChannelFeedToServe.Any(item => item.ID == chnl.ID);
            //        if (!containsItem)
            //            ChannelFeedToServe.Add(chnl);

            //    }
            //}

            //// genarate resp to send 
            //StringBuilder sb = new StringBuilder();
            //StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            //XmlWriter xmlWriter = new XmlTextWriter(stringWriter);


            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Indent = true;
            //xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;

            ////---- Set the XmlWriterSettings to the XMLWriter
            //xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);

            ///*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            //* false = no
            //* true = yes
            //*/
            //xmlWriter.WriteStartDocument(false);
            //// <VirtualChannel>
            //xmlWriter.WriteStartElement("VirtualChannel");
            //xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");

            ////xmlWriter.WriteAttributeString("xmlns", "xsi", null, "http://www.w3.org/2001/XMLSchema-instance );
            ////xmlWriter.WriteAttributeString("xsi", "noNamespaceSchemaLocation", null, "../xsd/DCInfo_schema.xsd");

            //if (ChannelFeedToServe.Count > 0)
            //{

            //    for (int i = 0; i < ChannelFeedToServe.Count; i++)
            //    {

            //        Channel chnl = ChannelFeedToServe[i];
            //        // <channel>
            //        xmlWriter.WriteStartElement("channel");
            //        xmlWriter.WriteAttributeString("id", chnl.ID);
            //        // <display-name>
            //        xmlWriter.WriteStartElement("display-name");
            //        xmlWriter.WriteString(chnl.DisplayName.ToString());
            //        // </display-name>
            //        xmlWriter.WriteEndElement();

            //        // </channel>
            //        xmlWriter.WriteEndElement();

            //    }
            //}

            //// </VirtualChannel>
            //xmlWriter.WriteEndElement();

            //xmlWriter.WriteEndDocument();
            //xmlWriter.Close();

            //resp = stringWriter.ToString();

            //SendHeader(sHttpVersion, "text/xml", resp.Length, " 200 OK", 20, ref mySocket);
            //SendToBrowser(resp, mySocket);

        }


        private void SendScheduleFeedasjson(string sHttpVersion, ref Socket mySocket)
        {

            string resp = "";
            //List<VideoContent> ScheduleFeedToServe = new List<VideoContent>();
            //using (VideoContentManager.Instance.LockVideoContentList(false))
            //{
            //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
            //    for (int i = 0; i < VideoContents.Count; i++)
            //    {
            //        VideoContent vc = VideoContents[i];
            //        ScheduleFeedToServe.Add(vc);
            //    }
            //}

            //// genarate resp to send to the client 
            //StringBuilder sb = new StringBuilder();
            //StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            //XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Indent = true;
            //xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;
            ////---- Set the XmlWriterSettings to the XMLWriter
            //xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
            ///*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            //* false = no
            //* true = yes
            //*/
            //xmlWriter.WriteStartDocument(false);
            //// <VirtualChannel>
            //xmlWriter.WriteStartElement("VirtualChannel");
            //xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");
            //// List of channel to serve to the client.
            //List<Channel> ChannelFeedFeedToServe = new List<Channel>();
            //if (ScheduleFeedToServe.Count > 0) // if the list not empty. 
            //{
            //    for (int i = 0; i < ScheduleFeedToServe.Count; i++)
            //    {
            //        VideoContent vc = ScheduleFeedToServe[i];
            //        Channel chnl = new Channel();
            //        chnl.ID = vc.ChannelID;
            //        chnl.DisplayName = vc.ChannelDisplayName;
            //        bool containsItem = ChannelFeedFeedToServe.Any(item => item.ID == chnl.ID);
            //        if (!containsItem)
            //            ChannelFeedFeedToServe.Add(chnl);
            //    }

            //    if (ChannelFeedFeedToServe.Count > 0) // if the list not empty. 
            //    {
            //        for (int i = 0; i < ChannelFeedFeedToServe.Count; i++)
            //        {
            //            Channel _chnl = ChannelFeedFeedToServe[i];
            //            // <channel>
            //            xmlWriter.WriteStartElement("channel");
            //            xmlWriter.WriteAttributeString("id", _chnl.ID);
            //            // <display-name>
            //            xmlWriter.WriteStartElement("display-name");
            //            xmlWriter.WriteString(_chnl.DisplayName.ToString());
            //            // </display-name>
            //            xmlWriter.WriteEndElement();
            //            // </channel>
            //            xmlWriter.WriteEndElement();
            //        }
            //    }



            //    for (int i = 0; i < ScheduleFeedToServe.Count; i++)
            //    {
            //        VideoContent prog = ScheduleFeedToServe[i];
            //        //<programme>
            //        xmlWriter.WriteStartElement("programme");
            //        // id
            //        xmlWriter.WriteAttributeString("id", prog.ProgrammeID.ToString());
            //        // start
            //        xmlWriter.WriteAttributeString("start", prog.StartDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture)); // Convert to SortableDateTime ("2008-03-09T16:05:07")
            //        // stop
            //        xmlWriter.WriteAttributeString("stop", prog.StopDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture));  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //        // episodeCode
            //        xmlWriter.WriteAttributeString("episodeCode", prog.EpisodeCode.ToString());  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //        // nodeCode
            //        xmlWriter.WriteAttributeString("nodeCode", prog.NodeCode.ToString());

            //        // channel
            //        xmlWriter.WriteAttributeString("channel", prog.ChannelID.ToString());

            //        //<title = ShowEpisode>
            //        xmlWriter.WriteStartElement("showEpisode");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        xmlWriter.WriteString(prog.ProgrammeTitle.ToString());
            //        //</title = ShowEpisode>
            //        xmlWriter.WriteEndElement();

            //        //<ShowNode>
            //        xmlWriter.WriteStartElement("showNode");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        xmlWriter.WriteString(prog.Node.ToString());
            //        //</ShowNode>
            //        xmlWriter.WriteEndElement();

            //        // <desc>
            //        xmlWriter.WriteStartElement("desc");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        if (!string.IsNullOrEmpty(prog.ProgrammeDesc))
            //        {
            //            byte[] bytes = Encoding.Default.GetBytes(prog.ProgrammeDesc);
            //            xmlWriter.WriteString(Encoding.UTF8.GetString(bytes));
            //        }
            //        else
            //            xmlWriter.WriteString(string.Empty);
            //        //</desc>
            //        xmlWriter.WriteEndElement();

            //        //<posterFileName>
            //        xmlWriter.WriteStartElement("posterFileName");
            //        // xmlWriter.WriteAttributeString("lang", "en");

            //        Uri uri = new Uri(prog.ImageURL);
            //        string url1 = uri.GetLeftPart(UriPartial.Path);
            //        string imageFileName = Path.GetFileName(url1);
            //        string imageExtension = Path.GetExtension(imageFileName);

            //        xmlWriter.WriteString(prog.ProgrammeID.ToString() + imageExtension);
            //        //</posterFileName>
            //        xmlWriter.WriteEndElement();


            //        // <videolink>
            //        xmlWriter.WriteStartElement("videolink");
            //        if (!string.IsNullOrEmpty(prog.VideoLink))
            //            xmlWriter.WriteString(prog.VideoLink.ToString());
            //        else
            //            xmlWriter.WriteString(string.Empty);
            //        //</videolink>
            //        xmlWriter.WriteEndElement();

            //        //</programme>
            //        xmlWriter.WriteEndElement();
            //    }
            //}

            //// </VirtualChannel>
            //xmlWriter.WriteEndElement();

            //xmlWriter.WriteEndDocument();
            //xmlWriter.Close();

            //resp = stringWriter.ToString();

            //// convert xml to json
            //XmlDocument doc = new XmlDocument();
            //doc.LoadXml(resp);
            //string jsonresp = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            //SendHeader(sHttpVersion, "application/json", jsonresp.Length, " 200 OK", 20, ref mySocket);
            //SendToBrowser(jsonresp, mySocket);

        }

        private void SendScheduleFeed(string sHttpVersion, ref Socket mySocket)
        {
            string resp = "";
            //List<VideoContent> ScheduleFeedToServe = new List<VideoContent>();
            //using (VideoContentManager.Instance.LockVideoContentList(false))
            //{
            //    IList<VideoContent> VideoContents = VideoContentManager.Instance.VContents;
            //    for (int i = 0; i < VideoContents.Count; i++)
            //    {
            //        VideoContent vc = VideoContents[i];
            //        ScheduleFeedToServe.Add(vc);
            //    }
            //}

            //// genarate resp to send to the client 
            //StringBuilder sb = new StringBuilder();
            //StringWriterWithEncoding stringWriter = new StringWriterWithEncoding(sb, UTF8Encoding.UTF8);
            //XmlWriter xmlWriter = new XmlTextWriter(stringWriter);
            //XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
            //xmlWriterSettings.Indent = true;
            //xmlWriterSettings.NewLineChars = Environment.NewLine + Environment.NewLine;
            ////---- Set the XmlWriterSettings to the XMLWriter
            //xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings);
            ///*Add the Attribut standalone="no" to <?xml version="1.0" encoding="UTF-8" standalone="no" ?>
            //* false = no
            //* true = yes
            //*/
            //xmlWriter.WriteStartDocument(false);
            //// <VirtualChannel>
            //xmlWriter.WriteStartElement("VirtualChannel");
            //xmlWriter.WriteAttributeString("source-info-url", "http://www.foodnetwork.co.uk");
            //// List of channel to serve to the client.
            //List<Channel> ChannelFeedFeedToServe = new List<Channel>();
            //if (ScheduleFeedToServe.Count > 0) // if the list not empty. 
            //{
            //    for (int i = 0; i < ScheduleFeedToServe.Count; i++)
            //    {
            //        VideoContent vc = ScheduleFeedToServe[i];
            //        Channel chnl = new Channel();
            //        chnl.ID = vc.ChannelID;
            //        chnl.DisplayName = vc.ChannelDisplayName;
            //        bool containsItem = ChannelFeedFeedToServe.Any(item => item.ID == chnl.ID);
            //        if (!containsItem)
            //            ChannelFeedFeedToServe.Add(chnl);
            //    }

            //    if (ChannelFeedFeedToServe.Count > 0) // if the list not empty. 
            //    {
            //        for (int i = 0; i < ChannelFeedFeedToServe.Count; i++)
            //        {
            //            Channel _chnl = ChannelFeedFeedToServe[i];
            //            // <channel>
            //            xmlWriter.WriteStartElement("channel");
            //            xmlWriter.WriteAttributeString("id", _chnl.ID);
            //            // <display-name>
            //            xmlWriter.WriteStartElement("display-name");
            //            xmlWriter.WriteString(_chnl.DisplayName.ToString());
            //            // </display-name>
            //            xmlWriter.WriteEndElement();
            //            // </channel>
            //            xmlWriter.WriteEndElement();
            //        }
            //    }



            //    for (int i = 0; i < ScheduleFeedToServe.Count; i++)
            //    {
            //        VideoContent prog = ScheduleFeedToServe[i];
            //        //<programme>
            //        xmlWriter.WriteStartElement("programme");
            //        // id
            //        xmlWriter.WriteAttributeString("id", prog.ProgrammeID.ToString());
            //        // start
            //        xmlWriter.WriteAttributeString("start", prog.StartDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture)); // Convert to SortableDateTime ("2008-03-09T16:05:07")
            //        // stop
            //        xmlWriter.WriteAttributeString("stop", prog.StopDateTime.ToString("yyyyMMddTHHmm", System.Globalization.CultureInfo.InvariantCulture));  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //        // episodeCode
            //        xmlWriter.WriteAttributeString("episodeCode", prog.EpisodeCode.ToString());  // Convert to SortableDateTime ("2008-03-09T16:05:07")

            //        // nodeCode
            //        xmlWriter.WriteAttributeString("nodeCode", prog.NodeCode.ToString());

            //        // channel
            //        xmlWriter.WriteAttributeString("channel", prog.ChannelID.ToString());

            //        //<title = ShowEpisode>
            //        xmlWriter.WriteStartElement("showEpisode");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        xmlWriter.WriteString(prog.ProgrammeTitle.ToString());
            //        //</title = ShowEpisode>
            //        xmlWriter.WriteEndElement();

            //        //<ShowNode>
            //        xmlWriter.WriteStartElement("showNode");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        xmlWriter.WriteString(prog.Node.ToString());
            //        //</ShowNode>
            //        xmlWriter.WriteEndElement();

            //        // <desc>
            //        xmlWriter.WriteStartElement("desc");
            //        xmlWriter.WriteAttributeString("lang", "en");
            //        if (!string.IsNullOrEmpty(prog.ProgrammeDesc))
            //        {
            //            byte[] bytes = Encoding.Default.GetBytes(prog.ProgrammeDesc);
            //            xmlWriter.WriteString(Encoding.UTF8.GetString(bytes));
            //        }
            //        else
            //            xmlWriter.WriteString(string.Empty);
            //        //</desc>
            //        xmlWriter.WriteEndElement();

            //        //<posterFileName>
            //        xmlWriter.WriteStartElement("posterFileName");
            //        // xmlWriter.WriteAttributeString("lang", "en");

            //        Uri uri = new Uri(prog.ImageURL);
            //        string url1 = uri.GetLeftPart(UriPartial.Path);
            //        string imageFileName = Path.GetFileName(url1);
            //        string imageExtension = Path.GetExtension(imageFileName);

            //        xmlWriter.WriteString(prog.ProgrammeID.ToString() + imageExtension);
            //        //</posterFileName>
            //        xmlWriter.WriteEndElement();


            //        // <videolink>
            //        xmlWriter.WriteStartElement("videolink");
            //        if (!string.IsNullOrEmpty(prog.VideoLink))
            //            xmlWriter.WriteString(prog.VideoLink.ToString());
            //        else
            //            xmlWriter.WriteString(string.Empty);
            //        //</videolink>
            //        xmlWriter.WriteEndElement();

            //        //</programme>
            //        xmlWriter.WriteEndElement();
            //    }
            //}

            //// </VirtualChannel>
            //xmlWriter.WriteEndElement();

            //xmlWriter.WriteEndDocument();
            //xmlWriter.Close();

            //resp = stringWriter.ToString();

            //SendHeader(sHttpVersion, "text/xml", resp.Length, " 200 OK", 20, ref mySocket);
            //SendToBrowser(resp, mySocket);

        }

        private void GetPrograms(string sHttpVersion, ref Socket mySocket)
        {



        }

        public string FormatBytes(long bytes)
        {
            const int scale = 1024;
            var orders = new[] { "GB", "MB", "KB", "Bytes" };
            var max = (long)Math.Pow(scale, orders.Length - 1);

            foreach (string order in orders)
            {
                if (bytes > max)
                    return String.Format(System.Globalization.CultureInfo.InvariantCulture, "{0:##.##} {1}",
                                         decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }


        internal static string GetStatus(bool active)
        {
            string sts = "Online";
            if (!active)
            {
                sts = "Offline";
            }
            return sts;
        }





    }
}
