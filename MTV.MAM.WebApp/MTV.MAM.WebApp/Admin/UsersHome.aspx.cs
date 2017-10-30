using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;

namespace MTV.MAM.WebApp.Admin
{
    public partial class UsersHome_aspx : BLC.BaseMEBSMAMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Global().BlockBrowzerPageReview();
        }
    }
}