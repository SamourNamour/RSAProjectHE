#region -.-.-.-.-.-.-.-.-.-.-.- Class : Name Space(s) -.-.-.-.-.-.-.-.-.-.-.-
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.Services.Client;
using System.Xml;
using System.Threading;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

using BLC = MTV.Library.Common;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CategoryDetailsControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                if (!Page.IsPostBack)
                {

                    //----- Karima
                    //if (DAL.Category.ListAll().Count < 3)
                    //{
                    //    this.CategoryTabs.Tabs[1].Visible = false;
                    //}
                    //---- if the selected Category Is All or Unclass :: Never MixedCategory.
                    //DAL.Category item = new DAL.Category().GetCategory(this.CategoriaID);
                    mebs_category Cat = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();
                    if (Cat != null && (string.Compare(Cat.Value,BLC.DefaultValue.AllCategoryDataBaseValue.ToString()) == 0 ||
                        string.Compare(Cat.Value, BLC.DefaultValue.UnclassCategoryDataBaseValue.ToString()) == 0))
                    {
                        this.pnlCategoryOrder.Visible = false;
                        this.pnlCategoryMappings.Visible = false;
                        DeleteButton.Visible = false;
                    }
                }

                this.CategoryTabs.ActiveTab = this.CategoryTabs.Tabs[Convert.ToInt16(this.TabID)];

                this.CategoryTabs.Tabs[2].Enabled =
                this.CategoryTabs.Tabs[2].Visible = false;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryDetailsControl : Page_Load : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //SimpleTextBox txtCategoryTitle = (SimpleTextBox)ctrlCategoryInfo.FindControl("txtCategoryTitle");
                //DropDownList ddlLanguage = (DropDownList)ctrlCategoryInfo.FindControl("ddlLanguage");
                ////---- Check if the same Name Exist
                //if (txtCategoryTitle != null && ddlLanguage != null)
                //{
                //    DAL.MultiLanguageCategoryDetails ObjMultiLangDetails =
                //        new DAL.MultiLanguageCategoryDetails().GetMultiLanguageCategoryDetails(
                //        txtCategoryTitle.Text.ToUpper().Replace(" ",string.Empty),
                //        Convert.ToInt32(ddlLanguage.SelectedValue));
                //    //---- Display Message to the Usert
                //    if (ObjMultiLangDetails != null && ObjMultiLangDetails.IDCategory != this.CategoriaID)
                //    {
                //        lblErrors.Text = "Category with the same name already exist";
                //        CategorizationRaisedErrors.Visible = true;
                //        return;
                //    }
                //}


                mebs_category item = ctrlCategoryInfo.SaveInfo();
                if (item != null)
                {
                    Response.Redirect("CategoryDetails.aspx?CategoryID=" + item.IdCategory.ToString() + "&TabID=0", false);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                //DAL.Category item = new DAL.Category().GetCategory(this.CategoriaID);
                mebs_category Cat = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();
                if (Cat != null)
                {
                    //item.DeleteWithReferences();
                    //---- Check when deleting a category : all details are deleted ?? ToDo : Karima
                    _context.DeleteObject(Cat);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                    Response.Redirect("Categories.aspx", false);
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryDetailsControl : DeleteButton_Click : {0}", ex.Message));
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public int CategoriaID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("CategoryID");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int TabID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("TabID");
            }
        }
        #endregion
    }
}