
#region Copyright Motive Television 2012
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: LogHelper.cs
//
#endregion

#region Using Directive
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

// Custom Directive(s)
using log4net;
using log4net.Config;
#endregion 


[assembly: log4net.Config.XmlConfigurator(Watch = true)]
[assembly: log4net.Config.Repository()]

namespace MTV.Library.Core
{
    /// <summary>
    /// 
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 
        /// </summary>
        public static ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly log4net.ILog HarrisLogger = log4net.LogManager.GetLogger("LoggerHarris");
    }
}
