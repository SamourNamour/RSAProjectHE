<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SettingsDetailsControl" %>
<%@ Register Src="SettingsInfo.ascx" TagName="SystemSettingsInfo" TagPrefix="MEBSConfig" %>
<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-configuration.png" />
        Edit System Setting details <a href="Settings.aspx" title="Back to System Settings list">(back to
            System Settings list)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonRed" OnClick="SaveButton_Click"
            Text="Save" ToolTip="Save current Setting." />
    </div>
</div>
<MEBSConfig:SystemSettingsInfo ID="ctrlSystemSettingsInfo" runat="server" />

