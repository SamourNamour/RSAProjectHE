<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CatalogHome.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.CatalogHomeControl" %>
<div class="section-title">
    <img src="Common/ico-catalog.png" alt="Catalog" />
    Catalog Home
</div>
<div class="homepage">
    <div class="intro">
        <p>
            Use the links on this page to manage broadcast events Theme list.
        </p>
    </div>
    <div class="options">
        <ul>
            <li>
                <div class="title">
                    <a href="Categories.aspx" title="Manage product categories.">Categories</a>
                </div>
                <div class="description">
                    <p>
                    <!-- 
                        Categories are the hierarchical grouping for the Contents in your catalog
                        (e.g. "Drama"). This form allows you to define how categories will be arranged and displayed to the end-user on the STB.
                    -->
                    
                    Theme list for broadcast events, this list is used later for Search Indexing mechanism (quick filtering of events based on their themes).
                    </p>
                    <p>
                        Manage Theme details.
                    </p>
                </div>
            </li>
           
           
            
        </ul>
    </div>
</div>