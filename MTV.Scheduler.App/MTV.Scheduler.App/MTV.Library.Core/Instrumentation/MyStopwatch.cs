using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
//using CMEScheduler.Core.Common;
//using Logger.HELogger;

namespace MTV.Library.Core.Instrumentation
{
    /// <summary>
    /// 
    /// </summary>
    public class MyStopwatch : IDisposable
    {
        #region Fields
        private Stopwatch internalStopwatch;
        private string name;
        #endregion

        #region Constructor

        /// <summary>
        /// Initializes the MyStopwatch
        /// </summary>
        /// <param name="name">The name of MyStopwatch</param>
        public MyStopwatch(string name)
        {
#if DEBUG
            this.name = name;
            internalStopwatch = new Stopwatch();
            internalStopwatch.Start();
#endif
        }

        #endregion

        #region Methods

        /// <summary>
        /// Disposes the MyStopwatch and writes into debug the amount of time used on the operation.
        /// </summary>
        public void Dispose()
        {
#if DEBUG
            internalStopwatch.Stop();
            //----.DefaultLogger.GARBAGELogger.Debug(name + ": " + internalStopwatch.Elapsed);
#endif
        }

        #endregion
    }
}
