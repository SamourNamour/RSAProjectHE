using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MTV.EventDispatcher.Service.Extensions.PosterExtension.UI
{
    public partial class PosterUI : UserControl
    {
        public PosterUI()
        {
            InitializeComponent();
            this.Text = "Parameters";
            //EnableAdvertisementCmd = Settings.Default.EnableAdvertisementCmd;
            ImgOutputHeight = MTV.Scheduler.App.Properties.Settings.Default.Height;
            ImgOutputWidth = MTV.Scheduler.App.Properties.Settings.Default.Width;
        }

        public int ImgOutputHeight
        {
            get
            {
                return (int)numHeight.Value;
            }
            set
            {
                numHeight.Value = value;
            }
        }


        public int ImgOutputWidth
        {
            get
            {
                return (int)numWidth.Value; ;
            }
            set
            {
                numWidth.Value = value;
            }
        } 


        
       

    }
}
