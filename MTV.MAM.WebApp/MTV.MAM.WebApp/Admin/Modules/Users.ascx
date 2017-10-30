<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Users.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.UsersControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-customers.png" alt="Users" />
        Manage Users
    </div>
    <div class="options">
        <asp:Button ID="SearchButton" runat="server" Text="Refresh" CssClass="adminButtonRed"
            OnClick="SearchButton_Click" ToolTip="Search for users based on the criteria below" />

        <asp:Button ID="btnAllusers" runat="server" Text="All User(s)" CssClass="adminButtonRed"
            OnClick="btnAllusers_Click" ToolTip="Get list of all EBS HeadEnd user(s)" />

        <input type="button" onclick="location.href='UserAdd.aspx'" value="Add new" id="btnAddNew"
            class="adminButtonRed" title="Add a new User" />
    </div>
</div>
<table width="100%">
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel runat="server" ID="lblRegistrationFrom" Text="Registration from:"
                ToolTip="The registration from date for the search in Coordinated Universal Time (UTC)."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtStartDate" CausesValidation="True" />
            <asp:ImageButton runat="Server" ID="iStartDate" ImageUrl="~/Common/Ico_Calendar.png"
                AlternateText="Click to show calendar" /><br />
            <ajaxToolkit:CalendarExtender ID="cStartDateButtonExtender" runat="server" TargetControlID="txtStartDate"
                PopupButtonID="iStartDate" Format="dd/MM/yyyy" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel runat="server" ID="lblRegistrationTo" Text="Registration to:"
                ToolTip="The registration to date for the search in Coordinated Universal Time (UTC)."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtEndDate" CausesValidation="True" />
            <asp:ImageButton runat="Server" ID="iEndDate" ImageUrl="~/Common/Ico_Calendar.png"
                AlternateText="Click to show calendar" /><br />
            <ajaxToolkit:CalendarExtender ID="cEndDateButtonExtender" runat="server" TargetControlID="txtEndDate"
                PopupButtonID="iEndDate" Format="dd/MM/yyyy" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSConfig:ToolTipLabel runat="server" ID="lblEmail" Text="Email:" ToolTip="A user Email."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtEmail" CssClass="adminInput" runat="server"></asp:TextBox>
        </td>
    </tr>
    <asp:PlaceHolder runat="server" ID="phUsername">
        <tr>
            <td class="adminTitle">
                <MEBSConfig:ToolTipLabel runat="server" ID="lblUsername" Text="Login:" ToolTip="A user username (if usernames are enabled)."
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData">
                <asp:TextBox ID="txtUsername" CssClass="adminInput" runat="server"></asp:TextBox>
            </td>
        </tr>
    </asp:PlaceHolder>
</table>
<p>
</p>
<asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" Width="100%"
              OnPageIndexChanging="gvUsers_PageIndexChanging" AllowPaging="true" PageSize="25" OnRowDataBound="gvUsers_RowDataBound">
              
    <Columns>      
        <asp:BoundField DataField="UserName" HeaderText="User Name">
        </asp:BoundField>                 
        
        <asp:BoundField DataField="Email" HeaderText="Email">
        </asp:BoundField>
        
        <asp:BoundField DataField="Comment" HeaderText="Comment">
        </asp:BoundField>
        
        <asp:BoundField DataField="IsApproved" HeaderText="Is Approved">
        </asp:BoundField>
        
        <asp:BoundField DataField="CreationDate" HeaderText="Creation Date">
        </asp:BoundField>
        
        <asp:BoundField DataField="LastLoginDate" HeaderText="Last Login Date">
        </asp:BoundField>
        
        <asp:BoundField DataField="LastActivityDate" HeaderText="Last Activity Date">
        </asp:BoundField>                        
        
        <asp:TemplateField HeaderText="User Role(s)">
            <ItemTemplate>
                <asp:GridView ID="gvUserRoles" runat="server" AutoGenerateColumns="false">               
                    <Columns>
                        <asp:TemplateField HeaderText="Role(s)">
                            <ItemTemplate>
                                <asp:Label ID="lblRole" runat="server" Text='<%# Container.DataItem %>'  ></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </ItemTemplate>
        </asp:TemplateField>    
                
        <asp:TemplateField HeaderText="Edit">
            <ItemTemplate>
                <a href="UserDetails.aspx?UserName=<%#Eval("UserName")%>" title="Edit User">Edit</a>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Center" />
            <ItemStyle HorizontalAlign="Center" />
        </asp:TemplateField>                
    </Columns>    
</asp:GridView>