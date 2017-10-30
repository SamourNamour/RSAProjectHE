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

namespace MTV.MAM.WebApp.Admin
{
    public partial class CategoryDetails : BLC.BaseMEBSMAMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Global().BlockBrowzerPageReview();;
        }
    }
}
