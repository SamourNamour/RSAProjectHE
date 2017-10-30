<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemEncapsulatorAdd.ascx.cs" Inherits=" MTV.MAM.WebApp.Admin.Modules.SystemEncapsulatorAddControl" %>
<%@ Register Src="SystemEncapsulatorInfo.ascx" TagName="SystemEncapsulatorInfo" TagPrefix="MSPCONFIG" %>


    <div class="section-header">
        <div class="title">
        <img src="Common/ico-configuration.png" alt="" />
        Add a new Encapsulator <a href="SystemEncapsulator.aspx" title="Back to language list">
        (back to Encapsulator list)</a>
    </div>
    <div class="options">
        <asp:Button ID="Button1" runat="server" Text="Save" CssClass="adminButtonRed"
            OnClick="AddButton_Click" ToolTip="Save Encapsulator." />
    </div>
</div>
<MSPCONFIG:SystemEncapsulatorInfo ID="SystemEncapsulatorInfo" runat="server" />

