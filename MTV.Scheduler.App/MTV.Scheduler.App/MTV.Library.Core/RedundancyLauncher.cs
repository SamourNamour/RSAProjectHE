using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.IO;
using MTV.Scheduler.App.UI;
//using Logger.HELogger;

namespace MTV.Library.Core
{
    public class RedundancyLauncher
    {
        #region Constants
        static readonly bool   DefaultRunAtStrart = true;
        static readonly string DefaultProgram = "dc_redundancy_generator.exe";
        static readonly string DefaultParameters = "\"{0}\"";
        static readonly string DefaultRedundancyRate = "0";
        static readonly string DefaultBlockSize = "558";
        static readonly string DefaultRepetitionSpace = "250";
        #endregion Constants

        #region Members
        static bool   _runAtStart = DefaultRunAtStrart;
        static string _program = DefaultProgram;
        static string _parameters = DefaultParameters;
        static string _redundancyRate = DefaultRedundancyRate;
        static string _blockSize = DefaultBlockSize;
        static string _repetitionSpace = DefaultRepetitionSpace;

        #endregion Members
        
        #region Properties
        internal static bool RunAtStart
        {
            get { return _runAtStart; }
            set { _runAtStart = value; }
        }
        internal static string Program
        {
            get { return _program; }
            set { _program = value; }
        }
        internal static string Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        internal static string RedundancyRate
        {
            get { return _redundancyRate; }
            set { _redundancyRate = value; }
        }

        internal static string BlockSize
        {
            get { return _blockSize; }
            set { _blockSize = value; }
        }

        internal static string RepetitionSpace
        {
            get { return _repetitionSpace; }
            set { _repetitionSpace = value; }
        }

        #endregion Properties


        /// <summary>
        /// 
        /// </summary>
        /// <param name="_fullSourceFilePath"></param>
        /// <param name="_fullDestinationFilePath"></param>
        /// <param name="_fullStatusFileName"></param>
        /// <returns></returns>
         public static bool Run(string _fullSourceFilePath, string _fullDestinationFilePath, string _fullStatusFileName)
         {
             string parameters = RedundancyProcessParameters(_redundancyRate, _blockSize, _repetitionSpace, _fullSourceFilePath, _fullDestinationFilePath, _fullStatusFileName);
            
             return LaunchProcess(_program, parameters, Path.GetDirectoryName(_program), ProcessWindowStyle.Hidden,true);
            // AppPath + "/MTV.Catalog.Host/
             
          }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="redundancy_rate"></param>
        /// <param name="block_size"></param>
        /// <param name="repetition_space"></param>
        /// <param name="fullSourceFilePath"></param>
        /// <param name="fullDestinationFilePath"></param>
        /// <param name="fullStatusFileName"></param>
        /// <returns></returns>
        internal static string RedundancyProcessParameters(string redundancy_rate, string block_size, string repetition_space, string fullSourceFilePath, string fullDestinationFilePath, string fullStatusFileName)
        {
            string output = String.Empty;

            try
            {
                output = string.Format(@"{0} {1} {2} ""{3}"" ""{4}"" ""{5}""",
                 redundancy_rate,              // {0} = Redundancy Rate
                 block_size,                  //  {1} = Block Size
                 repetition_space,           //   {2} = Repetition Space
                 fullSourceFilePath,        // {3} = Input full source file Path.
                 fullDestinationFilePath,  //{4} = Redundanced Destination file Path.
                 fullStatusFileName       //{5} = status file Name.

                );

             
            }
            catch (Exception ex)
            {
               
                MainForm.LogErrorToFile(string.Format("RedundancyLauncher - ProcessParameters(): {0}", ex.Message));
            }

            return output;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameters"></param>
        /// <param name="workingFolder"></param>
        /// <param name="windowStyle"></param>
        internal static bool LaunchProcess(string program, string parameters, string workingFolder, ProcessWindowStyle windowStyle, bool lowPrio)
        {
            try
            {
                Process process = new Process();
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.Arguments = parameters;
                process.StartInfo.FileName = program;
                process.StartInfo.WindowStyle = windowStyle;
                process.StartInfo.WorkingDirectory = workingFolder;

                process.Start();

                if (lowPrio)
                    process.PriorityClass = ProcessPriorityClass.BelowNormal;

                int i = 0;
                while (!process.HasExited)
                {
                    process.Refresh();
                    i++;
                    Thread.Sleep(100);
                }
                process.Close();
                return true;

            }

            catch (InvalidOperationException ex)
            {
               
                MainForm.LogExceptionToFile(ex);
                return false;
            }
            catch (NotSupportedException ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }

            catch (Exception ex)
            {
                MainForm.LogExceptionToFile(ex);
                return false;
            }
        }



    }
}
