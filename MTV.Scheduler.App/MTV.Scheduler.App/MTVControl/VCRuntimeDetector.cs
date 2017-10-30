using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;

namespace MTV.Scheduler.App.MTVControl
{
    //Installed states are:
    public enum INSTALLSTATE
    {
        INSTALLSTATE_NOTUSED = -7,
        INSTALLSTATE_BADCONFIG = -6,
        INSTALLSTATE_INCOMPLETE = -5,
        INSTALLSTATE_SOURCEABSENT = -4,
        INSTALLSTATE_MOREDATA = -3,
        INSTALLSTATE_INVALIDARG = -2,
        INSTALLSTATE_UNKNOWN = -1,
        INSTALLSTATE_BROKEN = 0,
        INSTALLSTATE_ADVERTISED = 1,
        INSTALLSTATE_ABSENT = 2,
        INSTALLSTATE_LOCAL = 3,
        INSTALLSTATE_SOURCE = 4,
        INSTALLSTATE_DEFAULT = 5
    }
    // Architecture enum - used when inspecting VC++ binaries in a local folder
    //public enum Architecture { X86 = IMAGE_FILE_MACHINE_I386, X64 = IMAGE_FILE_MACHINE_AMD64, Itanium = IMAGE_FILE_MACHINE_IA64 };

    //Struct layout required for FindFirstFile
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
    public struct WIN32_FIND_DATA
    {
        public uint dwFileAttributes;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftCreationTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastAccessTime;
        public System.Runtime.InteropServices.ComTypes.FILETIME ftLastWriteTime;
        public uint nFileSizeHigh;
        public uint nFileSizeLow;
        public uint dwReserved0;
        public uint dwReserved1;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
        public string cFileName;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 14)]
        public string cAlternateFileName;
    }
    
    public class VCRuntimeDetector
    {
        // translated from c++ by Cherki imad (https://github.com/markedup-mobi/crt-detector/blob/master/VCRuntimeDetector/VCDetector.cpp)
        
        // Constants used to represent the MSI product CODES for all of the runtimes
        public const string _vc2008x86Code = "{FF66E9F6-83E7-3A3E-AF14-8DE9A809A6A4}";
        public const string _vc2008x64Code = "{350AA351-21FA-3270-8B7A-835434E766AD}";
        public const string _vc2008SP1x86Code = "{9A25302D-30C0-39D9-BD6F-21E6EC160475}";
        public const string _vc2008SP1x64Code = "{8220EEFE-38CD-377E-8595-13398D740ACE}";

               
        // Constants used to represent the MSI product FILE NAMES for all of the runtimes pre-VC++10
        public const string _vc2008x86FolderName = "x86_microsoft.vc90.crt_1fc8b3b9a1e18e3b_9.0.21022*";
        public const string _vc2008x64FolderName = "amd64_microsoft.vc90.crt_1fc8b3b9a1e18e3b_9.0.21022*";
        public const string _vc2008SP1x86FolderName = "x86_microsoft.vc90.crt_1fc8b3b9a1e18e3b_9.0.30729*";
        public const string _vc2008SP1x64FolderName = "amd64_microsoft.vc90.crt_1fc8b3b9a1e18e3b_9.0.30729*";
      
        // Constants used to represent the names of VC++ runtime binaries, when checking for local folder support
        public const string _vc2008FileName1 = "msvcr90.dll";
        public const string _vc2008FileName2 = "msvcm90.dll";
        public const string _vc2008FileName3 = "msvcp90.dll";


        // Global constants and variables used in file-lookup queries
        public const string _winSxSFolderName = "WinSXS";
       
        
        [DllImport("msi.dll")]
        public static extern INSTALLSTATE MsiQueryProductState(string szProduct);
        
        [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint GetWindowsDirectory(StringBuilder lpBuffer,
           uint uSize);

        [DllImport("kernel32", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FindClose(IntPtr hFindFile);


        /// <summary>
        /// Uses Microsoft Installer (MSI) instrumentation to check for the availability of Microsoft products using its product codes.
        /// </summary>
        /// <param name="pszProductToCheck">pszProductToCheck - product code to look up</param>
        /// <returns>true if the requested product is installed</returns>
        public static bool CheckProductUsingMsiQueryProductState(string pszProductToCheck)
        {

        	bool bFoundRequestedProduct = false;
            INSTALLSTATE ir = INSTALLSTATE.INSTALLSTATE_UNKNOWN;
            // Check input parameter
            if (pszProductToCheck == null)
            return false;

            ir = MsiQueryProductState(pszProductToCheck);
            if (ir == INSTALLSTATE.INSTALLSTATE_DEFAULT)
            {
                bFoundRequestedProduct = true;
            }
            return bFoundRequestedProduct;
        }

        /// <summary>
        /// Checks if the VC++9 runtime for x86 is installed on this machine.
        /// </summary>
        /// <returns>true if the VC++9 runtime for x86 is installed</returns>
        public static bool IsVC2008Installed_x86()
        {
            return CheckProductUsingMsiQueryProductState(_vc2008x86Code) || CheckProductUsingWinSxSFolder(_vc2008x86FolderName);
        }

        /// <summary>
        ///  Checks if the VC++9 runtime for x64 is installed on this machine.
        /// </summary>
        /// <returns>true if the VC++9 runtime for x64 is installed</returns>
        public static bool IsVC2008Installed_x64()
        {
            return CheckProductUsingMsiQueryProductState(_vc2008x64Code) || CheckProductUsingWinSxSFolder(_vc2008x64FolderName); ;
        }

        /// <summary>
        ///  Checks if the VC++9 runtime (SP1) for x86 is installed on this machine.
        /// </summary>
        /// <returns>true if the VC++9 runtime (SP1) for x86 is installed</returns>
        public static bool IsVC2008SP1Installed_x86()
        {
            return CheckProductUsingMsiQueryProductState(_vc2008SP1x86Code) || CheckProductUsingWinSxSFolder(_vc2008SP1x86FolderName); ;
        }

        /// <summary>
        /// Checks if the VC++9 runtime (SP1) for x64 is installed on this machine.
        /// </summary>
        /// <returns> true if the VC++9 runtime (SP1) for x64 is installed</returns>
        public static bool IsVC2008SP1Installed_x64()
        {
            return CheckProductUsingMsiQueryProductState(_vc2008SP1x64Code) || CheckProductUsingWinSxSFolder(_vc2008SP1x64FolderName); ;
        }


        /// <summary>
        /// Gets the path of the $WINDIR/WinSxS folder.
        /// </summary>
        /// <returns>a string containing the path to the folder if found,</returns>
        public static string GetWinSXSDirectory()
        {
            string strDirectory = null;
        
            const int MaxPathLength = 255;
            StringBuilder sb = new StringBuilder(MaxPathLength);
            int len = (int)GetWindowsDirectory(sb, MaxPathLength);
            
            string windowsDirectory = null;
            windowsDirectory = sb.ToString(0, len);

            string combinedPath = System.IO.Path.Combine(windowsDirectory, _winSxSFolderName);
            
            if (combinedPath != null)
            {
                strDirectory = combinedPath ;
            }

            return strDirectory;
        }

        /// <summary>
        /// Queries the $WINDIR/WinSxS folder for the appropriate install path for the requested product.
        /// </summary>
        /// <param name="pszProductFolderToCheck"> pszProductFolderToCheck - the product name to look up.</param>
        /// <returns>true if the requested product is installed</returns>
        public static bool CheckProductUsingWinSxSFolder(string pszProductFolderToCheck){

            // Thanks to http://www.pinvoke.net/default.aspx/kernel32.findfirstfile
            bool bFoundRequestedProduct = false;
            IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

            string strWinSxSDir = GetWinSXSDirectory();

            if (strWinSxSDir != null)
            {

                string searchPath = System.IO.Path.Combine(strWinSxSDir, pszProductFolderToCheck);

                if (!string.IsNullOrEmpty(searchPath))
                {
                    WIN32_FIND_DATA FindFileData;
                    IntPtr findHandle;
                    
                     findHandle = FindFirstFile(searchPath, out FindFileData);
                    
                    if (findHandle != INVALID_HANDLE_VALUE)
                    { //found it!
                        bFoundRequestedProduct = true;
                        FindClose(findHandle);
                    }
                }



            }
                

            return bFoundRequestedProduct;

        }

        /// <summary>
        /// Searches for the first file matching to searchPattern in the sepcified path.
        /// </summary>
        /// <param name="path">The path from where to start the search.</param>
        /// <param name="searchPattern">The pattern for which files to search for.</param>
        /// <returns>Either the complete path including filename of the first file found
        /// or string.Empty if no matching file could be found.</returns>
        public static string FindFirstFileV2(string path, string searchPattern)
        {
            string[] files;

            try
            {
                // Exception could occur due to insufficient permission.
                files = Directory.GetFiles(path, searchPattern, SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                return string.Empty;
            }

            // If matching files have been found, return the first one.
            if (files.Length > 0)
            {
                return files[0];
            }
            else
            {
                // Otherwise find all directories.
                string[] directories;

                try
                {
                    // Exception could occur due to insufficient permission.
                    directories = Directory.GetDirectories(path);
                }
                catch (Exception)
                {
                    return string.Empty;
                }

                // Iterate through each directory and call the method recursivly.
                foreach (string directory in directories)
                {
                    string file = FindFirstFileV2(directory, searchPattern);

                    // If we found a file, return it (and break the recursion).
                    if (file != string.Empty)
                    {
                        return file;
                    }
                }
            }

            // If no file was found (neither in this directory nor in the child directories)
            // simply return string.Empty.
            return string.Empty;
        }

    }
}
