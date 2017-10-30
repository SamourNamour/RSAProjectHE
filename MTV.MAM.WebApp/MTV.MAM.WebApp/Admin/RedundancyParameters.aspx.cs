#region -.-.-.-.-.-.-.-.-.-.-.- NameSpaces -.-.-.-.-.-.-.-.-.-.-.-
using System;
using BLC = MTV.Library.Common;
#endregion

namespace MTV.MAM.WebApp.Admin
{
    public partial class RedundancyParameters_aspx : BLC.BaseMEBSMAMPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            new Global().BlockBrowzerPageReview();;
        }
    }
}
