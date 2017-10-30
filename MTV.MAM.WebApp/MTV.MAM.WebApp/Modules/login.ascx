<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="login.ascx.cs" Inherits="MTV.MAM.WebApp.Modules.loginControl" %>
     <div class="login-block">
        <table id="LoginForm" cellspacing="0" cellpadding="0" style="border-collapse:collapse;">
	<tr>
		<td>
                <table class="login-table-container">
                    <tbody>
                        <tr class="row">
                            <td class="item-name">

                                Login:
                            </td>
                        </tr>
                        <tr class="row">
                            <td class="item-value">
                                <asp:TextBox runat="server" ID="LoginForm_UserName" CssClass="adminInput" style="width: 200px;" ></asp:TextBox>
                                <span id="LoginForm_UserNameOrEmailRequired" title="E-Mail is required" style="color:Red;visibility:hidden;">
                                 *
                                </span>
                            </td>

                        </tr>
                        <tr class="row">
                            <td class="item-name">
                                Password:
                            </td>
                        </tr>
                        <tr class="row">
                            <td class="item-value">
                                <asp:TextBox  runat="server" TextMode="Password" maxlength="50" ID="LoginForm_Password" CssClass="adminInput" style="width: 200px;"></asp:TextBox>

                                <span id="LoginForm_PasswordRequired" title="Password is required" style="color:Red;visibility:hidden;">
                                            *
                                </span>
                            </td>
                        </tr>
                       
                        <tr class="row">
                            <td class="message-error">
                                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="False"></asp:Label></td>
                        </tr>
                        <tr class="row">
                            <td>
                                <div class="buttons">
                                    <asp:Button runat="server" ID="LoginForm_LoginButton" Text="Log in" CssClass="adminButtonRed" OnClick="LoginForm_LoginButton_Click" />

                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </td>
	</tr>
</table>
    </div>