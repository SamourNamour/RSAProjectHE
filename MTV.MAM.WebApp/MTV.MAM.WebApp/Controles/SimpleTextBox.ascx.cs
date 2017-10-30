#region Name Space(s)
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
using BLC = MTV.Library.Common;
#endregion

namespace MTV.MAM.WebApp.Controles
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SimpleTextBox : BLC.BaseMEBSMAMUserControl
    {
        #region Property(ies)
        public string Text
        {
            get
            {
                return txtValue.Text;
            }
            set
            {
                txtValue.Text = value;
            }
        }

        public bool Enabled
        {
            get
            {
                return txtValue.Enabled;
            }
            set
            {
                txtValue.Enabled = value;
            }
        }

        public string ErrorMessage
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
            }
        }

        public Unit Height
        {
            get
            {
                return txtValue.Height;                
            }
            set
            {
                txtValue.Height = value;                
            }
        }

        public TextBoxMode TextMode
        {
            get
            {
                return txtValue.TextMode;
            }
            set
            {
                txtValue.TextMode = value;
            }
        }
        #endregion
    }
}