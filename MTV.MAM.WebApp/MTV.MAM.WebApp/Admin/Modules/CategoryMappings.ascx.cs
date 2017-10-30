
#region - Copyright Motive Television 2013 -
//
// All rights are reserved. Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
//
// Filename: CategoryMappings.cs
//
#endregion

#region Name Space(s)
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
    public partial class CategoryMappings : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        mebs_category _CurrentCategory;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        //---- OK
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
                _CurrentCategory = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();
                if (_CurrentCategory == null)
                {
                    Response.Redirect(@"Categories.aspx", false);
                }

                if (!Page.IsPostBack)
                {
                    this.BindData();
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryMappings : Page_Load : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategoryMappings_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTitle = (Label)e.Row.FindControl("lblTitle");
                Label lblChannelVirtualName = (Label)e.Row.FindControl("lblChannelVirtualName");

                HiddenField hf = (HiddenField)e.Row.FindControl("hfCategoryID");
                int IdChildCategory;
                if (int.TryParse(hf.Value, out IdChildCategory))
                {
                    mebs_category Cat = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IdChildCategory), UriKind.Relative)).FirstOrDefault();
                    lblTitle.Text = GetCategoryDesignation(Cat, this.IDLanguage);
                    lblChannelVirtualName.Text = GetCategoryVirtualChannelName(Cat, this.IDLanguage);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategoryMappings_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvCategoryMappings.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAvialbleCategoryCollection_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAvialbleCategoryCollection.PageIndex = e.NewPageIndex;
            BindData();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAvialbleCategoryCollection_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Compare(e.CommandName, "AddCategory") == 0)
            {
                try
                {
                    int _idChildCategory = Convert.ToInt32(e.CommandArgument);
                    mebs_mixedcategory _newMixedCategory = new mebs_mixedcategory();
                    _newMixedCategory.DateCreation = DateTime.UtcNow;
                    _newMixedCategory.IdChildCategory = _idChildCategory;
                    _newMixedCategory.IdParentCategory = _CurrentCategory.IdCategory;
                    _newMixedCategory.IsDefault = false;
                    _newMixedCategory.Orden = 0;

                    _context.AddTomebs_mixedcategory(_newMixedCategory);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                    Response.Redirect("CategoryDetails.aspx?CategoryID=" + this.CategoriaID.ToString() + "&TabID=1", false);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is DataServiceClientException)
                    {
                        // Parse the DataServieClientException
                        BLC.DataServiceErrorInfo innerException = MEBSConfigHelper.ParseDataServiceClientException(ex.InnerException.Message);
                        // Display the DataServiceClientException message
                        if (innerException != null)
                        {
                            LogHelper.logger.Error(string.Format("CategoryMappings : gvAvialbleCategoryCollection_RowCommand : {0} - {1}", innerException.Code, innerException.Message));
                        }
                        else
                        {
                            LogHelper.logger.Error(string.Format("CategoryMappings : gvAvialbleCategoryCollection_RowCommand : {0}", ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoryMappings : gvAvialbleCategoryCollection_RowCommand : {0} ", ex.Message));
                    } 
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvCategoryMappings_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (string.Compare(e.CommandName, "DeleteCategory") == 0)
            {
                if (_CurrentCategory == null)
                    return;

                try
                {
                    int _idChildCategory = Convert.ToInt32(e.CommandArgument);
                    mebs_mixedcategory _deletedMixedCategory = _context.Execute<mebs_mixedcategory>(new Uri(string.Format(Config.GetMixedCategoryByChildID, _idChildCategory, _CurrentCategory.IdCategory), UriKind.Relative)).FirstOrDefault();
                    if (_deletedMixedCategory == null)
                        return;

                    _context.DeleteObject(_deletedMixedCategory);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                }
                catch (Exception ex)
                {
                    if (ex.InnerException is DataServiceClientException)
                    {
                        // Parse the DataServieClientException
                        BLC.DataServiceErrorInfo innerException = MEBSConfigHelper.ParseDataServiceClientException(ex.InnerException.Message);
                        // Display the DataServiceClientException message
                        if (innerException != null)
                        {
                            LogHelper.logger.Error(string.Format("CategoryMappings : gvCategoryMappings_RowCommand : {0} - {1}", innerException.Code, innerException.Message));
                        }
                        else
                        {
                            LogHelper.logger.Error(string.Format("CategoryMappings : gvCategoryMappings_RowCommand : {0}", ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoryMappings : gvCategoryMappings_RowCommand : {0} ", ex.Message));
                    } 
                }
            }
            Response.Redirect("CategoryDetails.aspx?CategoryID=" + this.CategoriaID.ToString() + "&TabID=1", false);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void gvAvialbleCategoryCollection_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblTitle = (Label)e.Row.FindControl("lblTitle");
                Label lblChannelVirtualName = (Label)e.Row.FindControl("lblChannelVirtualName");

                HiddenField hf = (HiddenField)e.Row.FindControl("hfCategoryID");
                int IdChildCategory;
                if (int.TryParse(hf.Value, out IdChildCategory))
                {
                    mebs_category Cat = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IdChildCategory), UriKind.Relative)).FirstOrDefault();
                    lblTitle.Text = GetCategoryDesignation(Cat, this.IDLanguage);
                    lblChannelVirtualName.Text = GetCategoryVirtualChannelName(Cat, this.IDLanguage);
                }

            }
        }
        #endregion


        #region  -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public int IDLanguage
        {
            get
            {
                return BLC.DefaultValue.IdDefaultLanguage;
                //if (Session["IDLanguage"] == null)
                //    throw new ArgumentException("IDLanguage session is null");

                //return Int32.Parse(Convert.ToString(Session["IDLanguage"]));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int CategoriaID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("CategoryID", int.MinValue);
            }
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {          
            //------ Get List of Mixed Categories on the Selected Category
            List<mebs_category> MixedCatCollection = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetMixedCategoryElements, this.CategoriaID), UriKind.Relative)).ToList();
            gvCategoryMappings.DataSource = MixedCatCollection;
            gvCategoryMappings.DataBind();


            //------- Get List of Available Categories that can e Mixed on the Selected Category
            List<mebs_category> AvailableCatCollection = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetAvailableCategoryElementsToBeMixed, this.CategoriaID), UriKind.Relative)).ToList();
            gvAvialbleCategoryCollection.DataSource = AvailableCatCollection;
            gvAvialbleCategoryCollection.DataBind();



            lblComment_Linked.Text = string.Format(@"Click Delete button (Form 1 ) to remove selected category from mixed category {0} collection (Form 2 ).",
                                                       GetCategoryDesignation(_CurrentCategory, this.IDLanguage));
            lblComment_Available.Text = string.Format(@"Click Add button (Form 2 ) to add selected category into mixed category {0} collection (Form 1 ).",
                                                       GetCategoryDesignation(_CurrentCategory, this.IDLanguage));
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        private string GetCategoryDesignation(mebs_category item, int languageID)
        {
            try
            {
                if (item.mebs_category_language_mapping == null || item.mebs_category_language_mapping.Count <= 0)
                    return string.Empty;

                return item.mebs_category_language_mapping.Where<mebs_category_language_mapping>
                    (x => x.IdLanguage == BLC.DefaultValue.IdDefaultLanguage).FirstOrDefault().Title;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoriesControl : GetCategoryDesignation : {0}", ex.Message));
                return string.Empty;

            }
        }

        //------ OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        private string GetCategoryVirtualChannelName(mebs_category item, int languageID)
        {
            try
            {
                if (item.mebs_category_language_mapping == null || item.mebs_category_language_mapping.Count <= 0)
                    return string.Empty;

                return item.mebs_category_language_mapping.Where<mebs_category_language_mapping>
                    (x => x.IdLanguage == BLC.DefaultValue.IdDefaultLanguage).FirstOrDefault().VirtualChannelDisignation;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoriesControl : GetCategoryVirtualChannelName : {0}", ex.Message));
                return string.Empty;

            }
        }
        #endregion

    }
}