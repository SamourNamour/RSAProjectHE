#region Copyright Motive Television 2013
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
using System.Reflection;

// Custom Directive(s)
using log4net;
using log4net.Config;
#endregion 


// [assembly: log4net.Config.XmlConfigurator(Watch = true)]
// [assembly: log4net.Config.Repository()]

namespace MTV.MAM.WebApp.Helper
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
    }
}