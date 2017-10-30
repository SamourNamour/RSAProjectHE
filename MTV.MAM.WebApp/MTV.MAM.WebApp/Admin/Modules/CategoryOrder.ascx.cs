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
using AUTH = MTV.MAM.WebApp.Authentication;
#endregion

namespace MTV.MAM.WebApp.Admin.Modules
{
    public partial class CategoryOrderControl : BLC.BaseMEBSMAMUserControl
    {
        //---- OK
        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Field(s) -.-.-.-.-.-.-.-.-.-.-.-
        mebsEntities _context;
        #endregion

        #region -.-.-.-.-.-.-.-.-.-.-.- Class : Event (s) -.-.-.-.-.-.-.-.-.-.-.-
        //----- OK
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                _context = new mebsEntities(Config.MTVCatalogLocation);
                if (!IsPostBack)
                {
                    BindGridView();
                }
            }
            catch (Exception ex)
            {
                LogHelper.logger.Error(string.Format("CategoryOrderControl : Page_Load : {0}", ex.Message));
            }
        }

        protected void gvCategorization_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                mebs_category cat = (mebs_category) e.Row.DataItem;
                if (cat != null)
                {
                    //IList lCatDetails = cat.ReferringMultiLanguageCategoryDetails();
                    if (cat.mebs_category_language_mapping != null && cat.mebs_category_language_mapping.Count > 0)
                    {
                        //DAL.MultiLanguageCategoryDetails CatDetails = (DAL.MultiLanguageCategoryDetails)lCatDetails[0];
                        mebs_category_language_mapping CatDetails = cat.mebs_category_language_mapping[0];
                        Label lblTitle = (Label)e.Row.FindControl("lblTitle");
                        if (lblTitle != null)
                            lblTitle.Text = CatDetails.Title;

                        Label lblMediaSetName = (Label)e.Row.FindControl("lblMediaSetName");
                        if (lblMediaSetName != null)
                            lblMediaSetName.Text = CatDetails.VirtualChannelDisignation;

                        //DAL.Languages Lang = new DAL.Languages().GetLanguage(CatDetails.IDLanguage);
                        if (CatDetails.mebs_language != null)
                        {
                            Label lblISOCode = (Label)e.Row.FindControl("lblISOCode");
                            if (lblISOCode != null)
                                lblISOCode.Text = CatDetails.mebs_language.ISOCode;
                        }

                    }
                }
                //-- Disabled the first UP button
                ImageButton btnUP = (ImageButton)e.Row.FindControl("btnUP");
                if(btnUP != null)
                    btnUP.Enabled = !(e.Row.RowIndex == 0);
                //-- Disabled the last DOWN Button
                ImageButton btnDown = (ImageButton)e.Row.FindControl("btnDown");
                GridView grid = (GridView)sender;
                //IList lCat = (IList)grid.DataSource;
                List<mebs_category> lCat = (List<mebs_category>)grid.DataSource;
                if (btnDown != null && lCat != null && lCat.Count >0)
                    btnDown.Enabled = !(e.Row.RowIndex == (lCat.Count - 1));
            }
        }

        protected void gvCategorization_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "UP":
                    {
                        string[] tab = Convert.ToString(e.CommandArgument).Split('_');
                        int IDCategoryUP = Convert.ToInt32(tab[0]);
                        int CategoryOrderUP = Convert.ToInt32(tab[1]);
                        int rowIndexUP = Convert.ToInt32(tab[2]);
                        int defaultUP = Convert.ToInt32(tab[3]);

                        //--- Get the Row UP to cpermute the data
                        GridViewRow row1 = this.gvCategorization.Rows[rowIndexUP - 1];
                        ImageButton btnDown = (ImageButton)row1.FindControl("btnUP");
                        if (btnDown != null)
                        {
                            tab = Convert.ToString(btnDown.CommandArgument).Split('_');
                            int IDCategoryDown = Convert.ToInt32(tab[0]);
                            int CategoryOrderDown = Convert.ToInt32(tab[1]);
                            int defaultDown = Convert.ToInt32(tab[3]);
                            if (defaultDown != -1 || defaultUP != -1)
                                MoveCategoryByDefault(IDCategoryDown, defaultDown, IDCategoryUP, defaultUP);
                            else
                                MoveCategoryByOrder(IDCategoryDown, CategoryOrderDown, IDCategoryUP, CategoryOrderUP);
                        }

                        if (this.IdTab == 2)
                        {
                            Response.Redirect("CategoryDetails.aspx?CategoryID=" + this.CategoryID.ToString() + "&TabID=2", false);
                        }


                        break;
                    }
                case "DOWN":
                    {
                        string[] tab = Convert.ToString(e.CommandArgument).Split('_');
                        int IDCategoryUP = Convert.ToInt32(tab[0]);
                        int CategoryOrderUP = Convert.ToInt32(tab[1]);
                        int rowIndexUP = Convert.ToInt32(tab[2]);
                        int defaultUP = Convert.ToInt32(tab[3]);
                        //--- Get the Row UP to cpermute the data
                        GridViewRow row1 = this.gvCategorization.Rows[rowIndexUP + 1];
                        ImageButton btnDown = (ImageButton)row1.FindControl("btnUP");
                        if (btnDown != null)
                        {
                            tab = Convert.ToString(btnDown.CommandArgument).Split('_');
                            int IDCategoryDown = Convert.ToInt32(tab[0]);
                            int CategoryOrderDown = Convert.ToInt32(tab[1]);
                            int defaultDown = Convert.ToInt32(tab[3]);

                            if (defaultDown != -1 || defaultUP != -1)
                                MoveCategoryByDefault(IDCategoryDown, defaultDown, IDCategoryUP, defaultUP);
                            else
                                MoveCategoryByOrder(IDCategoryDown, CategoryOrderDown, IDCategoryUP, CategoryOrderUP);
                        }
                        if (this.IdTab == 2)
                        {
                            Response.Redirect("CategoryDetails.aspx?CategoryID=" + this.CategoryID.ToString() + "&TabID=2", false);
                        }
                        break;
                    }
            }
        }
        #endregion

        #region Class - Methodes
        //----- OK
        public void BindGridView()
        {
            this.gvCategorization.DataSource = GetListCategories();
            this.gvCategorization.DataBind();
        }

        //----- OK
        /// <summary>
        /// 
        /// </summary>
        /// <param name="CategoryID"></param>
        /// <returns></returns>
        public List<mebs_category> GetListCategories()
        {
            _context = new mebsEntities(Config.MTVCatalogLocation);
            List<mebs_category_language_mapping> lDefaultCategories = _context.Execute<mebs_category_language_mapping>(new Uri(string.Format(Config.GetCategoriesInDefaultOrder, this.CategoryID), UriKind.Relative)).ToList();
            List<mebs_category_language_mapping> lCategories = _context.Execute<mebs_category_language_mapping>(new Uri(string.Format(Config.GetCategoriesInOrder, this.CategoryID), UriKind.Relative)).ToList();
            List<mebs_category> AllCategories = new List<mebs_category>();

            if (lDefaultCategories != null)
            {
                foreach (mebs_category_language_mapping Item in lDefaultCategories)
                {
                    if (Item.mebs_category == null)
                        continue;

                    Item.mebs_category.mebs_category_language_mapping.Add(Item);
                    AllCategories.Add(Item.mebs_category);
                }
            }

            if (lCategories != null)
            {
                foreach (mebs_category_language_mapping Item in lCategories)
                {
                    if (Item.mebs_category == null)
                        continue;

                    Item.mebs_category.mebs_category_language_mapping.Add(Item);
                    AllCategories.Add(Item.mebs_category);
                }
            }

            return AllCategories;
        }

        //----- OK
        public void MoveCategoryByOrder(int IDCategoryDown, int CategoryOrderDown, int IDCategoryUP, int CategoryOrderUP)
        {
            try
            {
                mebs_category CatDown = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IDCategoryDown), UriKind.Relative)).FirstOrDefault();
                mebs_category CatUp = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IDCategoryUP), UriKind.Relative)).FirstOrDefault();

                if (CatDown == null || CatUp == null)
                    return;

                CatDown.Orden = CategoryOrderUP;
                CatUp.Orden = CategoryOrderDown;

                _context.UpdateObject(CatDown);
                _context.UpdateObject(CatUp);
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
                        LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByOrder : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByOrder : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByOrder : {0} ", ex.Message));
                } 
            }


            BindGridView();
        }

        //----- OK
        public void MoveCategoryByDefault(int IDCategoryDown, int DefaultDown, int IDCategoryUP, int DefaultUP)
        {
            try
            {
                mebs_category CatDown = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IDCategoryDown), UriKind.Relative)).FirstOrDefault();
                mebs_category CatUp = _context.Execute<mebs_category>(new Uri(string.Format(Config.GetCategoryByID, IDCategoryUP), UriKind.Relative)).FirstOrDefault();
                if (CatDown == null || CatUp == null)
                    return;

                CatDown.Default = DefaultUP;
                CatUp.Default = DefaultDown;

                _context.UpdateObject(CatDown);
                _context.UpdateObject(CatUp);
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
                        LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByDefault : {0} - {1}", innerException.Code, innerException.Message));
                    }
                    else
                    {
                        LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByDefault : {0}", ex.InnerException.Message));
                    }
                }
                else
                {
                    LogHelper.logger.Error(string.Format("CategoryOrderControl : MoveCategoryByDefault : {0} ", ex.Message));
                } 
            }
            BindGridView();
        }


        #endregion

        #region Class - Proprity (ies)
        public int CategoryID
        {
            get
            {
                return BLC.CommonHelper.QueryStringInt("CategoryID", 0);
            }
        }
        private int _IdTab;

        public int IdTab
        {
            get { return _IdTab; }
            set { _IdTab = value; }
        }

        #endregion

    }
}