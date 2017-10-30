using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BLC = MTV.Library.Common;
using AUTH = MTV.MAM.WebApp.Authentication;

namespace MTV.MAM.WebApp
{
    /// <summary>
    /// Class Page : Main.Master
    /// </summary>
    public partial class Main : BLC.BaseMEBSMAMMasterPage
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(string.IsNullOrEmpty(this.UserName)) Response.Redirect("Login.aspx", false);

                lblLoghedUser.Text = string.Format("Logged is as : {0}", UserName);

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void mnuAdmin_MenuItemDataBound(object sender, MenuEventArgs e)
        {
            e.Item.ImageUrl = ((SiteMapNode)e.Item.DataItem)["IconUrl"];
            if (!AUTH.MEBSContext.Current.IsAdmin)
            {
                if (!e.Item.NavigateUrl.Contains("/Admin"))
                    return;
            }
            else
            {
                if (e.Item.NavigateUrl.Contains("/Admin"))
                    return;

                //if (e.Item.Parent != null)
                //{
                //    MenuItem menu = e.Item.Parent;
                //    menu.ChildItems.Remove(e.Item);
                //}
                //else
                //{
                //    Menu menu = (Menu)sender;
                //    menu.Items.Remove(e.Item);
                //}

            }
            if (e.Item.Parent != null)
            {
                MenuItem menu = e.Item.Parent;
                menu.ChildItems.Remove(e.Item);
            }
            else
            {
                Menu menu = (Menu)sender;
                menu.Items.Remove(e.Item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void LogOut_Click(object sender, EventArgs e)
        {
            Global.logout();
            if (!AUTH.MEBSContext.Current.IsAdmin)
                Response.Redirect("Login.aspx", false);
            else
                Response.Redirect("~/Login.aspx", false);
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Proprity (ies) -.-.-.-.-.-.-.-.-.-.-.-

        public string UserName
        {
            get
            {
                try
                {
                    return HttpContext.Current.User.Identity.Name; 
                }
                catch
                {
                    return string.Empty;
                }
            }
        }
        #endregion
    }
}