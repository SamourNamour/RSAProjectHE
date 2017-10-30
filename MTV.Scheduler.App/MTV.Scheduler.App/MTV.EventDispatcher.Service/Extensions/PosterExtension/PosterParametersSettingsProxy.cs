#region -.-.-.-.-.-.-.-.-.-.- Copyright Motive Television SARL 2014 -.-.-.-.-.-.-.-.-.-.-
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: PosterIntegrationExtension.cs
//
#endregion

using MTV.Scheduler.App.Properties;
namespace MTV.EventDispatcher.Service.Extensions.PosterExtension
{
    public class PosterParametersSettingsProxy : PosterParameters
    {
        #region -.-.-.-.-.-.-.-.-.-.- Class : Proprity(ies) -.-.-.-.-.-.-.-.-.-.-

        public int ImgOutputHeight
        {
            get
            {
                return Settings.Default.Height;
            }
            set
            {
                Settings.Default.Height = value;
            }
        }

        public int ImgOutputWidth
        {
            get
            {
                return Settings.Default.Width;
            }
            set
            {
                Settings.Default.Width = value;
            }
        }

        public string PosterAutomaticRecordLastSent
        {
            get
            {
                return Settings.Default.PosterAutomaticRecordLastSent;
            }
            set
            {
                Settings.Default.PosterAutomaticRecordLastSent = value;
            }
        }

        #endregion
    }
}
