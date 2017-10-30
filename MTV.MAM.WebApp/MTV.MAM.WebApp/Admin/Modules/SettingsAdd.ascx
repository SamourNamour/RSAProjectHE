<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsAdd.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SettingsAddControl" %>
<%@ Register Src="~/Admin/Modules/SettingsInfo.ascx" TagName="SystemSettingsInfo" TagPrefix="MEBSConfig" %>
    <div class="section-header">
        <div class="title">
        <img src="Common/ico-configuration.png" alt="" />
        Add a new System Setting <a href="Settings.aspx" title="Back to System Settings list">
        (back to System Settings  list)</a>
    </div>
    <div class="options">
        <asp:Button ID="AddButton" runat="server" Text="Save" CssClass="adminButtonRed"
            OnClick="AddButton_Click" ToolTip="Save Setting." />
    </div>
</div>
<MEBSConfig:SystemSettingsInfo ID="ctrlSystemSettingsInfo" runat="server" />
