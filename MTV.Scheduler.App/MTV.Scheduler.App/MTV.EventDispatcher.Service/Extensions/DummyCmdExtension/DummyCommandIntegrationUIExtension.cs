using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTV.Library.Core.Extensions;

namespace MTV.EventDispatcher.Service.Extensions.DummyCmdExtension
{
    public class DummyCommandIntegrationUIExtension : IUIExtension
    {

        #region IUIExtension Members

        public Control[] CreateSettingsView()
        {
            //return new Control[] { new DummyCmdUI() };
            return null;
        }

        public void PersistSettings(Control[] settingsView)
        {
            //DummyCmdUI options = (DummyCmdUI)settingsView[0];
            //Settings.Default.EnableDummyCmd = options.EnableDummyCmd;
            //Settings.Default.Save();
        }

        #endregion

    }
}
