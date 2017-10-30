
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MTV.EventDispatcher.Service.Extensions.DummyCmdExtension
{
    /// <summary>
    /// 
    /// </summary>
    public class DummyCmdParametersSettingsProxy : DummyCmdParameters {
        /// <summary>
        /// 
        /// </summary>
        public bool EnableDummyCmd {
            get {
                return Settings.Default.EnableDummyCmd;
            }
            set {
                Settings.Default.EnableDummyCmd = value;
            }
        }
    }
}
