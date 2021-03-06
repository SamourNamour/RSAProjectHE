#region Name Space(s)
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Data.Services.Client;
using BLC = MTV.Library.Common;
using System.Xml;
using System.Threading;
using System.Web.UI.WebControls;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;
using MTV.MAM.WebApp.MEBSCatalog;
using MTV.MAM.WebApp.Helper;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    /// <summary>
    /// User control for displaying all information about categorization.
    /// </summary>
    public partial class CategoriesControl : BLC.BaseMEBSMAMUserControl
    {
        //---- OK
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        public object verrou = new object();
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event(s) -.-.-.-.-.-.-.-.-.-.-.-
        //----- OK
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
                    FeedCategorizationTreeView();
                }
            }
            catch (Exception exc)
            {
                LogHelper.logger.Error(string.Format("CategoriesControl : Page_Load : {0}", exc.Message));
            }
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCategoryLanguage_SelectedIndexChanged(object sender,EventArgs e)
        {
            FeedCategorizationTreeView();
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FeedCategorizationTreeView();
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSendCategory_Click(object sender, EventArgs e)
        {
            SendCategorizationXmlFileToEncapsulator();//ExportManager.CreateXMLCategory()
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnFlatFormat_Click(object sender, EventArgs e)
        {
            SendCategorizationXmlFileToEncapsulator(); //ExportManager.HideAllContent()
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExportXML_Click(object sender, EventArgs e)
        {
            CategorizationRaisedErrors.Visible = false;
            if (Page.IsValid)
            {
                StringBuilder errorMessage = new StringBuilder();
                try
                {
                    string fileName = string.Format("categories_{0}.xml", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                    string xml = ExportManager.CreateXMLCategory();
                    BLC.CommonHelper.WriteResponseXML(xml, fileName);
                }
                 catch (System.Threading.ThreadAbortException threadAborted)
                {
                    LogHelper.logger.Error(string.Format("Categories : btnExportXML_Click : {0}", threadAborted.Message));
                }
                catch (SystemException deniedAccess)
                {                    
                    errorMessage.AppendFormat("The operating system denies create content.categories.xml.");
                    errorMessage.Append("<br />");
                    errorMessage.Append("<br />");

                    lblErrors.Text = errorMessage.ToString();
                    CategorizationRaisedErrors.Visible = true;
                    LogHelper.logger.Error(string.Format("Categories : btnExportXML_Click : {0}", deniedAccess.Message));
                }
                catch (Exception ex)
                {
                    LogHelper.logger.Error(string.Format("Categories : btnExportXML_Click : {0}", ex.Message));
                }
                finally
                {
                    ExportManager.DeleteGeneratedClassificationXMlFile();
                }
            }
        }

        protected void btnOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("CategoryOrder.aspx", false);
        }

        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Private Method(s) -.-.-.-.-.-.-.-.-.-.-.-
        //----- OK
        /// <summary>
        /// 
        /// </summary>
        private void FeedCategorizationTreeView()
        {
            this.CategoryTreeView.Nodes.Clear();
            TreeNode root = PrepareCategorizationTreeViewContent();

            this.CategoryTreeView.Nodes.Add(root);
            this.CategoryTreeView.ExpandAll();
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        private TreeNode PrepareCategorizationTreeViewContent()
        {
            TreeNode root = new TreeNode("EBS Content Categories");
            root.SelectAction = TreeNodeSelectAction.Expand;
            
            try
            {
                string strIconePath = string.Empty;

                List<mebs_category> categories = GetListCategories(0); //--- Get Root Categories //bLayer.GetRootCategoryCollection();
                if (categories != null && categories.Count > 0)
                {
                    foreach (mebs_category var in categories)
                    {
                        root.ChildNodes.Add(CreateCategoryNode(var));
                    }
                }
                string categoryAddURLParent = BLC.CommonHelper.GetLocation() + "Admin/CategoryAdd.aspx";
                root.ChildNodes.Add(new TreeNode("Add new category:", "", "", categoryAddURLParent, ""));
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoriesControl : PrepareCategorizationTreeViewContent : {0}", ex.Message));
            }
            return root;
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        private void FeedLanguageList()
        {
            ddlCategoryLanguage.Items.Add(new ListItem("Select language", "0"));
            List<mebs_language> items = _context.Execute<mebs_language>(new Uri(Config.GetListOfPublishedLanguage, UriKind.Relative)).ToList();

            if (items != null &&
                items.Count > 0)
            {
                foreach (mebs_language var in items)
                {
                    ListItem item = new ListItem(var.Name, Convert.ToString(var.IdLanguage));
                    ddlCategoryLanguage.Items.Add(item);
                }
            }
        }

        //----- OK
        /// <summary>
        /// For Each Category , Create a New Node (for the Tree) deponding on the Category kind (Mixed, Real, Unclassified, All)
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private TreeNode CreateCategoryNode(mebs_category item)
        {
            ImageTreeViewNode node = new ImageTreeViewNode();
            try
            {
                string strIconePath = string.Empty;
                string strVisibilyIcon = string.Empty;

                // Step 1:
                // Set TreeViewNode image regarding category kind (Mixed, Real, Unclassified, All, ...):
                strIconePath = GetCategoryIcone(item);

                string categoryDetailsURL = BLC.CommonHelper.GetLocation() + "Admin/CategoryDetails.aspx?CategoryID=" + item.IdCategory.ToString();
                node.NavigateUrl = categoryDetailsURL;
                node.Text = GetCategoryDesignation(item);
                node.ImageUrl = strIconePath;
                node.StatusIcons = GetCategoryVisibility(item);

                if (string.Compare(item.Value, BLC.DefaultValue.AllCategoryDataBaseValue.ToString()) == 0 &&
                    string.Compare(item.Value, BLC.DefaultValue.UnclassCategoryDataBaseValue.ToString()) == 0)
                    {
                        string categoryAddURL = BLC.CommonHelper.GetLocation() + "Admin/CategoryAdd.aspx?ParentCategoryID=" + item.IdCategory.ToString();
                       
                    }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoriesControl : CreateCategoryNode : {0}", ex.Message));
            }
            return node;
        }

        //------ OK
        private ListDictionary GetCategoryVisibility(mebs_category item)
        {
            string Name = string.Empty;
            string ImageName = "blank.gif";
            if (string.Compare(item.Visibility, Convert.ToString(BLC.DefaultValue.Visibility_Always)) == 0)
            {
                Name = "Always";
                ImageName = "Always.png";
            }

            else if (string.Compare(item.Visibility, Convert.ToString(BLC.DefaultValue.Visibility_False)) == 0)
            {
                Name = "False";
                ImageName = "False.png";
            }

            else if (string.Compare(item.Visibility, Convert.ToString(BLC.DefaultValue.Visibility_True)) == 0)
            {
                Name = "True";
                ImageName = "True.png";
            }

            ListDictionary StatusIcons = new ListDictionary();
            StatusIcons.Add(string.Format("Visibility : {0} ", Name), ImageName);

            return StatusIcons;
        }


        //----- OK
        /// <summary>
        /// Get category icone regarding category kind (Mixed, Real, Unclassified, All, ...).
        /// </summary>
        /// <param name="objCategory">Category</param>
        /// <returns>string</returns>
        private string GetCategoryIcone(mebs_category item)
        {
            if (string.Compare(item.Value, Convert.ToString(BLC.DefaultValue.AllCategoryDataBaseValue)) == 0)
                return "../Common/all.png";

            else if (string.Compare(item.Value, Convert.ToString(BLC.DefaultValue.UnclassCategoryDataBaseValue)) == 0)
                return "../Common/unclass.png";

            else if (item.IsMixed.Value)
                return "../Common/MixedCategory.png";

            else return "../Common/blank.gif";
        }


        //------ OK
        /// <summary>
        /// Get Category Title
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private string GetCategoryDesignation(mebs_category item)
        {
            try
            {
                if(item.mebs_category_language_mapping == null || item.mebs_category_language_mapping.Count <= 0)
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
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public List<mebs_category> GetListCategories(int CategoryID)
        {

            List<mebs_category_language_mapping> lDefaultCategories = _context.Execute<mebs_category_language_mapping>(new Uri(string.Format(Config.GetCategoriesInDefaultOrder,CategoryID),UriKind.Relative)).ToList();
            List<mebs_category_language_mapping> lCategories = _context.Execute<mebs_category_language_mapping>(new Uri(string.Format(Config.GetCategoriesInOrder,CategoryID),UriKind.Relative)).ToList();
            List<mebs_category> AllCategories = new List<mebs_category>();

            if (lDefaultCategories != null)
            {
                foreach (mebs_category_language_mapping Item in lDefaultCategories)
                {
                    if(Item.mebs_category == null)
                        continue;

                    Item.mebs_category.mebs_category_language_mapping.Add(Item);
                    AllCategories.Add(Item.mebs_category);
                }
            }

            if (lCategories != null)
            {
                foreach (mebs_category_language_mapping Item in lCategories)
                {
                    if(Item.mebs_category == null)
                        continue;
                    Item.mebs_category.mebs_category_language_mapping.Add(Item);
                    AllCategories.Add(Item.mebs_category);
                }
            }

            return AllCategories;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlBodyString"></param>
        void SendCategorizationXmlFileToEncapsulator()
        {
            // Lock send categorization treatment:
            lock (verrou)
            {
                try
                {
                    mebs_settings _sLastCategoryItemsChangedOn =
                        _context.Execute<mebs_settings>(new Uri(string.Format(Config.GetSettingsByName, "LastCategoryItemsChangedOn"), UriKind.Relative)).FirstOrDefault();

                    if (_sLastCategoryItemsChangedOn == null || string.IsNullOrEmpty(_sLastCategoryItemsChangedOn.SettingName))
                    {
                        LogHelper.logger.Error("The Setting LastCategoryItemsChangedOn Not Exist.");
                        return;
                    }

                    _sLastCategoryItemsChangedOn.SettingValue = BLC.DateTimeHelper.ConvertDateTimeToMySQLString(DateTime.UtcNow);
                    _context.UpdateObject(_sLastCategoryItemsChangedOn);
                    _context.SaveChanges(SaveChangesOptions.Batch);
                }
                catch (Exception ex)
                {
                    ShowError(BLC.DefaultValue.MSG_INTERNAL_ERROR);
                    if (ex.InnerException is DataServiceClientException)
                    {
                        // Parse the DataServieClientException
                        BLC.DataServiceErrorInfo innerException = MEBSMAMHelper.ParseDataServiceClientException(ex.InnerException.Message);
                        // Display the DataServiceClientException message
                        if (innerException != null)
                        {
                            LogHelper.logger.Error(string.Format("CategoriesControl : SendCategorizationXmlFileToEncapsulator : {0} - {1}", innerException.Code, innerException.Message));
                        }
                        else
                        {
                            LogHelper.logger.Error(string.Format("CategoriesControl : SendCategorizationXmlFileToEncapsulator : {0}", ex.InnerException.Message));
                        }
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoriesControl : SendCategorizationXmlFileToEncapsulator : {0}", ex.Message));
                    }
                }

            }
            FeedCategorizationTreeView();
            CategoryTreeView.CollapseAll();
        }
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Property(ies) -.-.-.-.-.-.-.-.-.-.-.-
        //----- OK
        /// <summary>
        /// 
        /// </summary>
        public int IDLanguage
        {
            get
            {
                return BLC.DefaultValue.IdDefaultLanguage;
            }
        }
        #endregion
    }
}