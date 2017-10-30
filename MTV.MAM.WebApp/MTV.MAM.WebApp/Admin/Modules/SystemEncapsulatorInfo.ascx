<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemEncapsulatorInfo.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SystemEncapsulatorInfoControl" %>
<%@ Register TagPrefix="MSPCONFIG" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="MSPCONFIG" TagName="SimpleTextBox" Src="~/Controles/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="MSPCONFIG" TagName="NumericTextBox" Src="~/Controles/NumericTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblName" runat="server" Text="Name:" ToolTip="The Encapsulator server name"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MSPCONFIG:SimpleTextBox ID="txtName" runat="server" CssClass="adminInput" ErrorMessage="Name is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblType" runat="server" Text="Encapsulator Type:" ToolTip="Encapsulator type."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlEncapsulatorType" runat="server" CssClass="adminInput" >
            </asp:DropDownList>
        </td>
    </tr>
<%--    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblStatus" runat="server" Text="Encapsulator Status:"
                ToolTip="Encapsulator status." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:DropDownList ID="ddlEncapsulatorStatus" runat="server" CssClass="adminInput">
            </asp:DropDownList>
        </td>
    </tr>--%>
    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblPublished" runat="server" Text="Published:" ToolTip="Determines whether this Encapsulator is published."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbPublished" runat="server" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblIpAddress" runat="server" Text="IP Address:" ToolTip="The IP address of the Encapsulator"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MSPCONFIG:SimpleTextBox ID="txtIpAddress" runat="server" CssClass="adminInput"
                ErrorMessage="IP Address is required" />
        </td>
    </tr>
<%--    <tr>
        <td class="adminTitle">
            <MSPCONFIG:ToolTipLabel ID="lblMultiInstancesNum" runat="server" Text="Multi Instances N°:" ToolTip="Multi Instances Number of the Encapsulator"
                ToolTipImage="~/Administration/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MSPCONFIG:NumericTextBox ID="txtMultiInstancesNum" runat="server" CssClass="adminInput"
                MaximumValue="65536" MinimumValue="0" RangeErrorMessage="The value must be from 0 to 65536"
                RequiredErrorMessage="Multi Instances Number is required" Value="-1" />
        </td>
    </tr>--%>
</table>
