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

using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
using BLC = MTV.Library.Common;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CategoryInfoControl : BLC.BaseMEBSMAMUserControl
    {
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-.- 
        private Random ObjRandom;
        mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class :  Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        //------ OK
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
                if (!IsPostBack)
                {
                    FeedLanguageList();
                    FeedParentCategoryList();
                    FeedVisibilityList();
                    BindData();
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryInfoControl : Page_Load : {0}", ex.Message));
            }
        }

        //------ OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlLanguage_SelectedIndexChanged(object sender,
                                                        EventArgs e)
        {
            ddlParentCategory.ClearSelection();
            ddlParentCategory.Items.Clear();
            FeedParentCategoryList();
            BindData();
        }

        protected void ddlParentCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            mebs_category Cat = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();

            FeedDefaultList(Cat);
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-.-.- 
        //----- OK
        /// <summary>
        /// 
        /// </summary>
        private void FeedLanguageList()
        {
            List<mebs_language> items = _context.Execute<mebs_language>(new Uri(Config.GetListOfPublishedLanguage, UriKind.Relative)).ToList();

            if (items != null &&
                items.Count > 0)
            {
                foreach (mebs_language var in items)
                {
                    ListItem item = new ListItem(var.Name, Convert.ToString(var.IdLanguage));
                    ddlLanguage.Items.Add(item);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FeedParentCategoryList()
        {
            ddlParentCategory.Items.Add(new ListItem("Select Parent Category", "0"));// UInt32.MinValue.ToString()
            //IList items = DAL.Category.ListAll();
            //if (items != null &&
            //    items.Count > 0
            //    )
            //{
            //    foreach (DAL.Category var in items)
            //    {
            //        if (this.CategoriaID == var.CategoryID
            //            || var.CategoryValue == DAL.DefaultValue.AllCategoryDataBaseValue.ToString()
            //            || var.CategoryValue == DAL.DefaultValue.UnclassCategoryDataBaseValue.ToString()) continue;
            //        ListItem item = new ListItem(GetCategoryDesignation(var, Convert.ToInt32(ddlLanguage.SelectedItem.Value)), var.CategoryID.ToString());
            //        ddlParentCategory.Items.Add(item);
            //    }
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        private void FeedDefaultList(mebs_category Cat)
        {
            ddlDefault.Items.Clear();
            ListItem item = null;
            item = new ListItem("-1", "-1");
            ddlDefault.Items.Add(item);
            int ParentId = 0;//Convert.ToInt32(ddlParentCategory.SelectedValue);
            int _value = 0;

            if (Cat != null && Cat.Default.Value != -1)
            {
                item = new ListItem(Cat.Default.Value.ToString(), Cat.Default.Value.ToString());
                ddlDefault.Items.Add(item);
                item.Selected = true;
            }

            _value = GetMaxDefault(ParentId) + 1;

            item = new ListItem(_value.ToString(), _value.ToString());
            ddlDefault.Items.Add(item);

        }

        private int GetMaxDefault(int ParentId)
        {
            try
            {
                mebs_category CatMaxDefault = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetMaxDefault, ParentId), UriKind.Relative)).FirstOrDefault();
                if (CatMaxDefault != null)
                    return CatMaxDefault.Default.Value;
                else
                    return 0;
            }
            catch
            {
                return 0;
            }
        }

        private int GetMaxOrder()
        {
            try
            {
                mebs_category CatMaxOrder = _context.Execute<mebs_category>(new Uri(Config.GetMaxOrder, UriKind.Relative)).FirstOrDefault();
                if (CatMaxOrder != null)
                    return CatMaxOrder.Orden.Value;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryInfoControl : GetMaxOrder : {0}", ex.Message));
                return 0;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void FeedVisibilityList()
        {
            ddlVisibility.Items.Add(new ListItem(BLC.DefaultValue.Visibility_True.ToUpper(), BLC.DefaultValue.Visibility_True));
            ddlVisibility.Items.Add(new ListItem(BLC.DefaultValue.Visibility_False.ToUpper(), BLC.DefaultValue.Visibility_False));
            ddlVisibility.Items.Add(new ListItem(BLC.DefaultValue.Visibility_Always.ToUpper(), BLC.DefaultValue.Visibility_Always));
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="languageID"></param>
        /// <returns></returns>
        private string GetCategoryDesignation(mebs_category item,int languageID)
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

        //----- OK
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

        /// <summary>
        /// 
        /// </summary>
        private void BindData()
        {
            try
            {
                mebs_category item = null;
                if (this.CategoriaID > 0)
                {
                    item = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();
                    //item = categoryProvider.GetCategory(this.CategoriaID);
                    if (item != null)
                    {
                        txtCategoryTitle.Text = GetCategoryDesignation(item, Convert.ToInt32(ddlLanguage.SelectedValue));        // Title
                        txtMediasetName.Text = GetCategoryVirtualChannelName(item, Convert.ToInt32(ddlLanguage.SelectedValue));  // Virtual name
                        ListItem li = this.ddlVisibility.Items.FindByValue(item.Visibility);                                     // Visibility
                        if (li != null)
                        {
                            this.ddlVisibility.SelectedValue = li.Value;
                        }

                        //li = this.ddlParentCategory.Items.FindByValue(item.ParentID.ToString());                                 // ParentId
                        //if (li != null)
                        //{
                        //    this.ddlParentCategory.SelectedValue = li.Value;
                        //}
                        //if (item.Value == BLC.DefaultValue.AllCategoryXMLValue.ToString() ||
                        //    item.IsUnclass.Value)
                        //{
                        //    ddlParentCategory.Enabled = false;
                        //}


                        txtStandardLCN.Value = item.StandardLCN.Value;                                                                 // StandardLCN
                        txtMediasetLCN.Value = item.MediasetLCN.Value;                                                                 // MediasetLCN
                        if (item.IsMixed.Value)                                                                                // IsMixed
                        {
                            this.cbUnclass.Enabled =
                            this.cbUnclass.Checked = !item.IsMixed.Value;
                        }
                        //this.cbPublished.Checked = Convert.ToBoolean(item.IsPublished);                                          // Published

                        this.cbUnclass.Checked = Convert.ToBoolean(item.IsUnclass);                                              // Unclass                                           
                    }
                }
                //else
                //{
                //    if (this.ParentCategoryID > 0)
                //    {
                //        // ToDo :: categoryProvider.GetMaxSubCategoryOrder(int Category Id).
                //        item = categoryProvider.GetCategory(this.ParentCategoryID);
                //        this.ddlParentCategory.Items.FindByValue(this.ParentCategoryID.ToString()).Selected = true;
                //        this.ddlParentCategory.Enabled = false;
                //        //int maxOrder = item.GetMaxSubCategoryOrder() + 1;
                //        //ddlOrder.Items.Add(new ListItem((ddlOrder.Items.Count + 1).ToString(), maxOrder.ToString()));
                //        //ddlOrder.SelectedIndex = ddlOrder.Items.Count - 1;
                //        //ddlOrder.Enabled = false;
                //    }
                //    else
                //    {
                //        // Add new order value to ddlOrder list based on Max(CategoryOrder) :                
                //        //int maxOrder = categoryProvider.GetMaxOrder() + 1;
                //        //ddlOrder.Items.Add(new ListItem((ddlOrder.Items.Count + 1).ToString(), maxOrder.ToString()));
                //        //ddlOrder.SelectedIndex = ddlOrder.Items.Count - 1;
                //        //ddlOrder.Enabled = false;
                //        ddlParentCategory.Enabled = false;
                //    }
                //}
                FeedDefaultList(item);
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryInfoControl : BindDate : {0}", ex.Message));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private mebs_category ManageMainCategory()
        {
            
            mebs_category item = null;
            int MaxOrder = 0;
            try
            {
                if (this.CategoriaID > 0)
                {
                    item = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, this.CategoriaID), UriKind.Relative)).FirstOrDefault();
                    if (item != null)
                    {
                        int langaugeID = Convert.ToInt32(ddlLanguage.SelectedItem.Value);                              // Title & Virtual Channel Name
                        int categoryID = this.CategoriaID;
                        // Check if current Category culture already exists:
                        if (item.mebs_category_language_mapping != null && item.mebs_category_language_mapping.Count > 0)
                        {
                            item.mebs_category_language_mapping[0].Title = txtCategoryTitle.Text;
                            item.mebs_category_language_mapping[0].VirtualChannelDisignation = txtMediasetName.Text;
                        }
                        item.Visibility = ddlVisibility.SelectedItem.Value.ToLower();                        // Visibility
                        item.Default = Convert.ToInt32(ddlDefault.SelectedItem.Value);                       // Default
                        int ParentId = Convert.ToInt32(ddlParentCategory.SelectedItem.Value);

                        item.StandardLCN = (int)txtStandardLCN.Value;                                                // StandardLCN
                        item.MediasetLCN = (int)txtMediasetLCN.Value;                                                // MediasetLCN                     
                        item.IsPublished = true; //Convert.ToInt32(cbPublished.Checked);                             // Published
                        item.IsUnclass = cbUnclass.Checked;                                                          // Unclass

                        if (cbUnclass.Checked)
                        {
                            mebs_category _unclassCategory = _context.Execute<mebs_category>(new Uri(Config.GetUnclassCategory, UriKind.Relative)).FirstOrDefault();
                            if (_unclassCategory != null)
                            {
                                _unclassCategory.IsUnclass = false;
                                _context.UpdateObject(_unclassCategory);
                            }
                        }

                        _context.UpdateObject(item);
                        _context.UpdateObject(item.mebs_category_language_mapping[0]);

                        _context.SaveChanges(SaveChangesOptions.Batch);
                    }
                }
                else
                {
                    item = new mebs_category();
                    ObjRandom = new Random();
                    // Generate Catgory Value [1-65536] : ( 0 = Separator ) ( 1 = All ) (2 = Unclassified)
                    item.Value = Convert.ToString(ObjRandom.Next(BLC.DefaultValue.MinCategoryValue, BLC.DefaultValue.MaxCategoryValue));

                    item.Visibility = ddlVisibility.SelectedItem.Value.ToLower();
                    item.ParentID = Convert.ToInt32(ddlParentCategory.SelectedItem.Value);
                    MaxOrder = GetMaxOrder();
                    item.Orden = MaxOrder + 1;
                    if (ddlParentCategory.SelectedIndex > 0)                                                     // ParentId (check index) ToDo
                    {
                        item.ParentID = Convert.ToInt32(ddlParentCategory.SelectedItem.Value);
                    }

                    item.Default = Convert.ToInt32(ddlDefault.SelectedItem.Value);
                    item.StandardLCN = (int)txtStandardLCN.Value;                                                // StandardLCN
                    item.MediasetLCN = (int)txtMediasetLCN.Value;                                                // MediasetLCN                     
                    item.IsPublished = true; //Convert.ToInt32(cbPublished.Checked);                             // Published
                    item.IsUnclass = cbUnclass.Checked;                                                          // Unclass
                    item.DateCreation = DateTime.UtcNow;
                    item.IsMixed = false;

                     mebs_category_language_mapping  ObjCategoryMapping = new mebs_category_language_mapping();
                    ObjCategoryMapping.IdLanguage = Convert.ToInt32(ddlLanguage.SelectedItem.Value);
                    ObjCategoryMapping.Title = txtCategoryTitle.Text;
                    ObjCategoryMapping.VirtualChannelDisignation = txtMediasetName.Text;

                    if (cbUnclass.Checked)
                    {
                        mebs_category _unclassCategory = _context.Execute<mebs_category>(new Uri(Config.GetUnclassCategory, UriKind.Relative)).FirstOrDefault();
                        if (_unclassCategory != null)
                        {
                            _unclassCategory.IsUnclass = false;
                            _context.UpdateObject(_unclassCategory);
                        }
                    }

                    //-------------- SAVE CATEGORY AND DETAILS
                    _context.AddTomebs_category(item);
                    _context.AddTomebs_category_language_mapping(ObjCategoryMapping);
                    _context.AddLink(item, "mebs_category_language_mapping", ObjCategoryMapping);
                    _context.SaveChanges(SaveChangesOptions.Batch);

                }
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
                        LogHelper.logger.Error(string.Format("CategoryInfoControl : ManageMainCategory : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoryInfoControl : ManageMainCategory : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("CategoryInfoControl : ManageMainCategory : {0}", ex.Message));
                }
            }
            return item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns> DAL.Category</returns>
        public mebs_category SaveInfo()
        {
            //if (this.ParentCategoryID > 0)
            //{
            //    return ManageSubCategory();
            //}
            return ManageMainCategory();
        }

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        /// <summary>
        /// 
        /// </summary>
        public int IDLanguage
        {
            get
            {
                //if (Session["IDLanguage"] == null)
                //    throw new ArgumentException("IDLanguage session is null");

                //return Int32.Parse(Convert.ToString(Session["IDLanguage"]));
                return BLC.DefaultValue.IdDefaultLanguage;
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

        /// <summary>
        /// 
        /// </summary>
        public int ParentCategoryID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("ParentCategoryID", 0);
            }
        }
        #endregion
        #endregion
    }
}