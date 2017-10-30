using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using log4net;


namespace MTV.EventDequeuer.Common
{
    public static class LogManager
    {
        private static ILog log = null;

        public static ILog Log
        {
            get { return log ?? (log = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)); }
        }

    }
}
