using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI.HtmlControls;
using System.Web;

namespace MTV.Library.Common
{
    /// <summary>
    /// Represents a personalized MEBSConfig Master page.
    /// </summary>
    public class BaseMEBSConfigMasterPage : MasterPage
    {

        /// <summary>
        /// Default Constrctor based on Master page parent constructor.
        /// </summary>
        public BaseMEBSConfigMasterPage()
            : base()
        { 
        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }


        /// <summary>
        /// Messages to be shown inot the page.
        /// </summary>
        public virtual void ShowMessage(string Message, string CompleteMessage)
        {
            Literal lErrorTitle = (Literal)this.Page.Master.FindControl("lMessageConfirmation");
            lErrorTitle.Text = Message;
        }

        /// <summary>
        /// Messages to be shown inot the page.
        /// </summary>
        public virtual void ShowError(string Message, string CompleteMessage)
        {
            Literal lErrorTitle = (Literal)this.Page.Master.FindControl("lErrorTitle");
            //Literal lErrorComplete = (Literal)this.Page.Master.FindControl("lErrorComplete");
            lErrorTitle.Text = Message;
            //lErrorComplete.Text = CompleteMessage;
        }

        /// <summary>
        /// Display BesTv Icon in the Navigator
        /// </summary>
        protected void SetFavIcon()
        {
            string favIconPath = HttpContext.Current.Request.PhysicalApplicationPath + "BesTV.ico";
            if (File.Exists(favIconPath))
            {
                string favIconUrl = "BesTV.ico"; // +  Location
                HtmlLink htmlLink = new HtmlLink();
                htmlLink.Attributes["rel"] = "SHORTCUT ICON";
                htmlLink.Attributes["href"] = favIconUrl;
                Page.Header.Controls.Add(htmlLink);
            }
        }
    }
}
