<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryInfo.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.CategoryInfoControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="NumericTextBox" Src="~/Controles/NumericTextBox.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="SimpleTextBox" Src="~/Controles/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>

<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCategoryTitle" runat="server" Text="Category Title:"
                ToolTip="Category Title." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBox ID="txtCategoryTitle" runat="server" Width="500px" CssClass="adminInput"
                ErrorMessage="Category Title is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblMediasetName" runat="server" Text="Mediaset Name:"
                ToolTip="Mediaset Name." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:TextBox ID="txtMediasetName" runat="server" ToolTip="Mediaset Name" Width="500px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblVisibility" runat="server" Text="Category Visibility:"
                ToolTip="Category Visibility." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:DropDownList ID="ddlVisibility" runat="server" Width="150px">
            </asp:DropDownList></td>
    </tr>
<%--    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblOrder" runat="server" Text="Category Order:"
                ToolTip="Category Order." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:DropDownList ID="ddlOrder" runat="server" Width="150px">
            </asp:DropDownList></td>
    </tr>--%>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblDefault" runat="server" Text="Category Default:"
                ToolTip="Category Default." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:DropDownList ID="ddlDefault" runat="server" Width="150px">
            </asp:DropDownList></td>
    </tr>
    <tr id="lang" visible="false" runat="server">
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCategoryLanguage" runat="server" Text="Category Language:"
                ToolTip="Category Language." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" dir="ltr">
            <asp:DropDownList ID="ddlLanguage" runat="server" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddlLanguage_SelectedIndexChanged">
            </asp:DropDownList></td>
    </tr>
    <tr style="display:none;">
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblParentCategory" runat="server" Text="Category Parent:"
                ToolTip="Category Parent." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:DropDownList 
            ID="ddlParentCategory" 
            runat="server" 
            Width="150px" 
            AutoPostBack="True" OnSelectedIndexChanged="ddlParentCategory_SelectedIndexChanged" 
            Enabled = "False">
            </asp:DropDownList></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblStandardLCN" runat="server" Text="Standard LCN:"
                ToolTip="Standard LCN." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:NumericTextBox ID="txtStandardLCN" runat="server" Value="-1" Width="150px"
                MinimumValue="-1" MaximumValue="65536" RangeErrorMessage="*" RequiredErrorMessage="*">
            </MEBSConfig:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblMediasetLCN" runat="server" Text="Mediaset LCN:"
                ToolTip="Standard LCN." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:NumericTextBox ID="txtMediasetLCN" runat="server" Value="-1" Width="150px"
                MinimumValue="-1" MaximumValue="65536" RangeErrorMessage="*" RequiredErrorMessage="*">
            </MEBSConfig:NumericTextBox>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCategoryUnclass" runat="server" Text="Is Unclassified:"
                ToolTip="special option can be used to include all contents that do not belong to any category."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbUnclass" runat="server" /></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCategoryPublished" runat="server" Text="Is Published:" Visible="false"
                ToolTip="Check to publish this category (visible On STB). Uncheck to unpublish (category not available On STB)"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbPublished" runat="server" Visible="false" /></td>
    </tr>
</table>
