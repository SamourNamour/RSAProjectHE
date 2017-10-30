<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryAdd.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.CategoryAddControl" %>

<%@ Register TagPrefix="MEBSConfig" TagName="CategoryInfo" Src="~/Admin/Modules/CategoryInfo.ascx" %>
    <div class="section-header">
        <div class="title">
        <img src="/Admin/Common/ico-catalog.png" alt="" />
        Add a new System Content Category <a href="Categories.aspx" title="Back to System Categorizations list">
        (back to System Categorization list)</a>
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="Save" CssClass="adminButtonRed"
            OnClick="AddButton_Click" ToolTip="Save Setting." />
    </div>
</div>
<MEBSConfig:CategoryInfo ID="ctrlCategorizationInfo" runat="server" />
<br />
<br />
<div id="CategorizationRaisedErrors" runat="server" class="warnings" visible="false">
    <div class="section-header">
        <div class="title">
            <img alt="Warnings" src="Admin/Common/ico-warnings.gif" />
            Error
        </div>
    </div>
    <div>
        <asp:Label ID="lblErrors" runat="server"></asp:Label>
    </div>
</div>
<br />