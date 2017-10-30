<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserInfo.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.UserInfoControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="SimpleTextBox" Src="~/Controles/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="SimpleTextBoxPswd" Src="~/Controles/SimpleTextBoxPswd.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="NumericTextBox" Src="~/Controles/NumericTextBox.ascx" %>
<table class="adminContent">
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblLogin" runat="server" Text="Login:" ToolTip="User Login"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBox ID="txtLogin" runat="server" CssClass="adminInput" ErrorMessage="User Login  is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblPassword" runat="server" Text="Password:"
                ToolTip="User Password" ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBoxPswd ID="txtPassword" runat="server" CssClass="adminInput" ErrorMessage="User Password is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblPasswordQuestion" runat="server" Text="Password Question:"
                ToolTip="User Password Question." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPasswordQuestion" runat="server"
                Width="450px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblPasswordAnswer" runat="server" Text="Password Answer:"
                ToolTip="User Password Answer." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPasswordAnswer" runat="server"
                Width="450px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblComment" runat="server" Text="Comment:"
                ToolTip="User Description." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtDescription" runat="server" Height="50px" TextMode="MultiLine"
                Width="450px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblFirstName" runat="server" Text="First Name:"
                ToolTip="User First Name" ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBox ID="txtFirstName" runat="server" CssClass="adminInput" ErrorMessage="User First Name is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblLastName" runat="server" Text="Last Name:" ToolTip="User Last Name"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <MEBSConfig:SimpleTextBox ID="txtLastName" runat="server" CssClass="adminInput" ErrorMessage="User Last Name is required" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblEmail" runat="server" Text="E-mail:"
                ToolTip="E-mail." ToolTipImage="~/Common/ico-help.png"/>
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtEmail" runat="server" Width="250px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblStreetAddress" runat="server" Text="Street Address:"
                ToolTip="User Street Address." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtStreetAddress" runat="server" Width="450px" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblStreetAddress2" runat="server" Text="Street Address:"
                ToolTip="User Street Address." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtStreetAddress2" runat="server" Width="450px" TextMode="MultiLine" Height="50px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblPostalCode" runat="server" Text="Postal Code:"
                ToolTip="Postal Code." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPostalCode" runat="server" Width="250px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblPhone" runat="server" Text="Phone:" ToolTip="Phone number."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtPhone" runat="server" Width="250px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblMobile" runat="server" Text="Mobile:" ToolTip="Mobile number."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtMobile" runat="server" Width="250px"></asp:TextBox></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCountry" runat="server" Text="Country:" ToolTip="Country."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData"><asp:DropDownList ID="ddlCountry" runat="server" Width="255px">
        </asp:DropDownList>
            </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblCity" runat="server" Text="City:" ToolTip="City."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
        <asp:DropDownList ID="ddlCity" runat="server" Width="255px">
        </asp:DropDownList>
            </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblGender" runat="server" Text="User Gender:" ToolTip="User Gender"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:RadioButtonList ID="rblGender" runat="server" RepeatDirection="Horizontal">
                <asp:ListItem Selected="True">Male</asp:ListItem>
                <asp:ListItem>Female</asp:ListItem>
            </asp:RadioButtonList></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblIsActive" runat="server" Text="Is Active:"
                ToolTip="Determines whether the account is an adminstrator account  WARNING - THIS ALLOWS ACCESS TO THE ADMINISTRATION AREA."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:CheckBox ID="cbIsActive" runat="server" /></td>
    </tr>
        <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel ID="lblRoles" runat="server" Text="User Role(s):" ToolTip="User Role(s)"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:RadioButtonList ID="rblRoles" runat="server" RepeatDirection="Horizontal">
            </asp:RadioButtonList></td>
    </tr>
</table>