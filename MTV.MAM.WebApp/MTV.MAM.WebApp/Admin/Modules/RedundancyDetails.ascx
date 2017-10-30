<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RedundancyDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.RedundancyDetailsControl" %>
<%@ Register Src="SettingsInfo.ascx" TagName="SystemSettingsInfo" TagPrefix="MEBSConfig" %>
<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-configuration.png" />
        Edit Redundancy Setting details <a href="RedundancyParameters.aspx" title="Back to Redundancy Settings list">(back to
            Redundancy Settings list)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonRed" OnClick="SaveButton_Click"
            Text="Save" ToolTip="Save current Setting." />
    </div>
</div>
<MEBSConfig:SystemSettingsInfo ID="ctrlSystemSettingsInfo" runat="server" />