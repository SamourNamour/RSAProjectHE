using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Net;
using System.IO;
using EnterpriseDT.Net.Ftp;
using System.Data;

namespace MTV.Library.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class FTP
    {
        #region Private Attribut(s)
        FTPConnection ftpConnection;
        private string ftpServerIP;
        private string ftpUser;
        private string ftpPassword;
        #endregion

        #region Public Attribut Property(ies)

        /// <summary>
        /// 
        /// </summary>
        public string FTPServerIP
        {
            set
            {
                ftpServerIP = value;
            }
            get
            {
                return ftpServerIP;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FTPUser
        {
            get
            {
                return ftpUser;
            }
            set
            {
                ftpUser = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string FTPPassword
        {
            get
            {
                return ftpPassword;
            }
            set
            {
                ftpPassword = value;
            }
        }

        #endregion

        #region Constructor(s)
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_ftpServerIP"></param>
        /// <param name="_ftpUser"></param>
        /// <param name="_ftpPassword"></param>
        public FTP(string _ftpServerIP, string _ftpUser, string _ftpPassword)
        {
            this.ftpConnection = new FTPConnection();
            this.ftpConnection.ServerAddress = _ftpServerIP;
            this.ftpConnection.UserName = _ftpUser;
            this.ftpConnection.Password = _ftpPassword;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsServerUp()
        {
            bool res = false;
            try
            {
                this.Connect();
                res = this.ftpConnection.IsConnected;
                this.Close();

                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Return FTP URI.
        /// </summary>
        /// <returns>string</returns>
        private string ReturnURI()
        {
            string uri = "ftp://" + ftpServerIP + "/";
            return uri;
        }

        /// <summary>
        /// Connect to FTP server.
        /// </summary>
        private bool Connect()
        {
            bool IsConnected = false;
            try
            {
                this.ftpConnection.Connect();
                IsConnected = true;
            }
            catch (Exception ex)
            {
                //MSPLogger.IEBSAdminConfigLogger.Error(ex);
                IsConnected = false;
            }
            return IsConnected;
        }

        /// <summary>
        /// Disconnect from the FTP server.
        /// </summary>
        private void Close()
        {
            if (this.ftpConnection.IsConnected) this.ftpConnection.Close();
        }

        /// <summary>
        /// Check if a specific file exists.
        /// </summary>
        /// <param name="_ftpLocalPath">string : complete file local path</param>
        /// <param name="ftpFileName"></param>
        /// <returns></returns>
        public bool CheckFileExist(string _ftpLocalPath, string ftpFileName)
        {
            bool IsExist = false;
            if (this.Connect())
            {
                try
                {
                    string[] listFiles = ftpConnection.GetFiles(_ftpLocalPath);
                    foreach (string var in listFiles)
                    {
                        if (string.Compare(var, ftpFileName) == 0)
                        {
                            this.Close();
                            IsExist = true;
                        }
                    }
                    this.Close();
                }
                catch (Exception ex)
                {
                    //MSPLogger.IEBSAdminConfigLogger.Error(ex);
                    this.Close();
                    IsExist = false;
                }
            }
            else
            {
                IsExist = false;
            }
            return IsExist;
        }

        /// <summary>
        /// Check if a specific file exists.
        /// </summary>
        /// <param name="ftpFileName"></param>
        /// <returns></returns>
        public bool CheckFileExist(string ftpFileName)
        {
            //bool IsExist = 0;
            if (this.Connect())
            {
                try
                {
                    if (this.ftpConnection.Exists(ftpFileName))
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    //MSPLogger.IEBSAdminConfigLogger.Error(ex);
                    return false;
                }
                finally
                {
                    this.Close();
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        /// <summary>
        /// Upload a specific file into FTP server.
        /// </summary>
        /// <param name="_ftpLocalPath">string : complete file local path</param>
        /// <param name="_completeLocalPath"></param>
        /// <returns>bool</returns>    
        public bool UploadFile(string _ftpLocalPath, string _completeLocalPath)
        {
            bool IsUploaded = false;
            if (this.Connect())
            {
                try
                {
                    FileInfo fileInf = new FileInfo(_completeLocalPath);
                    this.ftpConnection.ServerDirectory = _ftpLocalPath;
                    this.ftpConnection.UploadFile(_completeLocalPath, fileInf.Name);
                    this.Close();
                    IsUploaded = true;
                }
                catch (Exception ex)
                {
                    this.Close();
                    //MSPLogger.IEBSAdminConfigLogger.Error(ex);
                    IsUploaded = false;
                }
            }
            else
            {
                IsUploaded = false;
            }
            return IsUploaded;
        }

        /// <summary>
        /// Remove a specific file from FTP Server.
        /// </summary>
        /// <param name="strServerDirectory"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        public bool RemoveFile(string strServerDirectory, string strFileName)
        {
            bool res = false;
            this.ftpConnection.Connect();
            this.ftpConnection.ServerDirectory = strServerDirectory;
            res = this.ftpConnection.DeleteFile(strFileName);
            this.ftpConnection.Close();

            return res;

        }

        /// <summary>
        /// Get Directory List files
        /// </summary>
        /// <param name="strServerDirectory"></param>
        /// <returns></returns>
        public string[] GetFiles(string strServerDirectory)
        {
            if (Connect())
            {
                try
                {
                    return this.ftpConnection.GetFiles(strServerDirectory);
                }
                catch (Exception ex)
                {
                    //MSPLogger.IEBSAdminConfigLogger.Error(ex);
                    return null;
                }
            }
            return null;
        }

        /// <summary>
        /// Get File Size (In FTP Repisitory)
        /// </summary>
        /// <param name="FilePath"></param>
        /// <returns></returns>
        public long GetFileSize(string FilePath)
        {
            if (Connect())
            {
                try
                {
                    return this.ftpConnection.GetSize(FilePath);
                }
                catch (Exception ex)
                {
                   // MSPLogger.IEBSAdminConfigLogger.Error(ex);
                    return 0;
                }
            }
            return 0;
        }
    }
}
