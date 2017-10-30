<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SettingsInfo.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SettingsInfoControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="SimpleTextBox" Src="~/Controles/SimpleTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblName" runat="server" Text="Setting Name:" ToolTip="Setting Key Name."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBox ID="txtName" runat="server" CssClass="adminInput" ErrorMessage="Setting Name is required" Width="500" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblValue" runat="server" 
                Text="Setting Value:" ToolTip="Setting Value." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:TextBox ID="txtValue" runat="server" Width="500px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblDescription" runat="server" 
                Text="Setting Description:" ToolTip="Setting Description." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtDescription" runat="server" Height="100px" TextMode="MultiLine"
                ToolTip="Setting Description is required" Width="500px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblVisibility" runat="server" 
                Text="Setting Visibility:" ToolTip="Setting Visibility." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="chkVisibility" runat="server" />
        </td>
    </tr>
</table>
