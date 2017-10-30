#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: PosterIntegrationUIExtension.cs
//
#endregion

#region -.-.-.-.-.-.-.-.-.-.- Class : Namespace (s) -.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MTV.Library.Core.Extensions;
using MTV.EventDispatcher.Service.Extensions.PosterExtension.UI;
using MTV.Scheduler.App.Properties;
#endregion

namespace MTV.EventDispatcher.Service.Extensions.PosterExtension
{
    public class PosterIntegrationUIExtension :  IUIExtension
    {

        #region -.-.-.-.-.-.-.-.-.-.- Class : IUIExtension Members -.-.-.-.-.-.-.-.-.-.-

        public Control[] CreateSettingsView()
        {
            return new Control[] { new PosterUI() };
        }

        public void PersistSettings(Control[] settingsView)
        {
            PosterUI options = (PosterUI)settingsView[0];
            Settings.Default.Height = options.ImgOutputHeight;
            Settings.Default.Width = options.ImgOutputWidth;
            Settings.Default.Save();
        }

        #endregion

    }
}
