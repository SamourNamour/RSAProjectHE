using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MTV.MAM.WebApp.Helper
{
    public class ClickablePanel : Panel, IPostBackEventHandler, INamingContainer
    {
        public event EventHandler Click;
        public delegate void ClickEventHandler(object sender, EventArgs e);

        protected void OnClick(EventArgs e)
        {
            if (Click != null)
            {
                Click(this, e);
            }
        }

        public void RaisePostBackEvent(string eventArgument)
        {
            OnClick(new EventArgs());

        }

        protected override void AddAttributesToRender(HtmlTextWriter writer)
        {
            base.AddAttributesToRender(writer);
            writer.AddAttribute(HtmlTextWriterAttribute.Onclick, Page.ClientScript.GetPostBackEventReference(this, this.ID));
        }

    }
}