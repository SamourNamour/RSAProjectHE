using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace MTV.MAM.WebApp.Controles
{
    public partial class ToolTipLabelControl : UserControl
    {
        public string Text
        {
            get
            {
                return lblValue.Text;
            }
            set
            {
                lblValue.Text = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return lblValue.Enabled;
            }
            set
            {
                lblValue.Enabled = value;
            }
        }

        public string ToolTip
        {
            get
            {
                return lblValue.ToolTip;
            }
            set
            {
                lblValue.ToolTip = value;
                imgToolTip.ToolTip = value;
            }
        }

        public string ToolTipImage
        {
            get
            {
                return imgToolTip.ImageUrl;
            }
            set
            {
                imgToolTip.ImageUrl = value;
            }
        }


        public Unit Width
        {
            get
            {
                return lblValue.Width;
            }
            set
            {
                lblValue.Width = value;
            }
        }

        public string CssClass
        {
            get
            {
                return lblValue.CssClass;
            }
            set
            {
                lblValue.CssClass = value;
            }
        }
    }
}