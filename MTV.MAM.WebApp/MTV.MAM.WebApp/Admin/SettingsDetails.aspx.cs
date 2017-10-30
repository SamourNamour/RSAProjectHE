#region Name Space(s)
using System;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using BLC = MTV.Library.Common;
using System.Web.UI.WebControls;
#endregion


namespace MTV.MAM.WebApp.Admin
{
    public partial class SettingsDetails : BLC.BaseMEBSMAMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Global().BlockBrowzerPageReview();
        }
    }
}
