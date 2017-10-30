using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MTV.MAM.WebApp.Controles
{
    public partial class NumericTextBoxControl : System.Web.UI.UserControl
    {
        public int Value
        {
            get
            {
                return int.Parse(txtValue.Text);
            }
            set
            {
                txtValue.Text = value.ToString();
            }
        }

        public string RequiredErrorMessage
        {
            get
            {
                return rfvValue.ErrorMessage;
            }
            set
            {
                rfvValue.ErrorMessage = value;
            }
        }

        public string RangeErrorMessage
        {
            get
            {
                return rvValue.ErrorMessage;
            }
            set
            {
                rvValue.ErrorMessage = value;
            }
        }

        public string MinimumValue
        {
            get
            {
                return rvValue.MinimumValue;
            }
            set
            {
                rvValue.MinimumValue = value;
            }
        }

        public string MaximumValue
        {
            get
            {
                return rvValue.MaximumValue;
            }
            set
            {
                rvValue.MaximumValue = value;
            }
        }

        public string ValidationGroup
        {
            get
            {
                return rfvValue.ValidationGroup;
            }
            set
            {
                txtValue.ValidationGroup = value;
                rfvValue.ValidationGroup = value;
                rvValue.ValidationGroup = value;
            }
        }

        public Unit Width
        {
            get
            {
                return txtValue.Width;
            }
            set
            {
                txtValue.Width = value;
            }
        }

        public string CssClass
        {
            get
            {
                return txtValue.CssClass;
            }
            set
            {
                txtValue.CssClass = value;
            }
        }
        public int MaxLength
        {
            get { return txtValue.MaxLength; }
            set { txtValue.MaxLength = value; }
        }

        public bool Enabled
        {
            get { return txtValue.Enabled; }
            set { txtValue.Enabled = value; }
        }

    }
}