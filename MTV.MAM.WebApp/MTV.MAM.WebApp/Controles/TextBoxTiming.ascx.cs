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
    public partial class TextBoxTimingControl : UserControl
    {
        public string Text
        {
            get { return txtTiming.Text; }
            set { txtTiming.Text = value; }
        }

        public string ValidationGroup
        {
            get
            {
                return rfvValue.ValidationGroup;
            }
            set
            {
                txtTiming.ValidationGroup = value;
                rfvValue.ValidationGroup = value;
                revValue.ValidationGroup = value;
            }
        }


        public string ControlToValidate
        {
            get
            { return rfvValue.ControlToValidate; }

            set
            {
                rfvValue.ControlToValidate = value;
                //revValue.ControlToValidate = value;
            }
        }


        public string CssClass
        {
            get
            {
                return txtTiming.CssClass;
            }
            set
            {
                txtTiming.CssClass = value;
            }
        }

        public Unit Width
        {
            get
            {
                return txtTiming.Width;
            }
            set
            {
                txtTiming.Width = value;
            }
        }


    }
}