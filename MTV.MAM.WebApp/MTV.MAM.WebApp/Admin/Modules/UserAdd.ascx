<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserAdd.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.UserAddControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="UserInfo" Src="UserInfo.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="" />
        Add a new User <a href="Users.aspx" title="Back to User list">(back to User list)</a>
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="Add" CssClass="adminButtonRed" 
            ToolTip="Add customer" OnClick="AddButton_Click" />
    </div>
</div>
<MEBSConfig:UserInfo ID="ctrlCustomerInfo" runat="server" />