<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SettingsControl" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<div class="section-header">
    <div class="title">
        <img alt="System settings" src="Common/ico-configuration.png" />
        System settings
    </div>
    <div class="options">
     <asp:Button 
                        ID="btnSearch" 
                        runat="server" 
                        CssClass="adminButtonRed" 
                        Text="Search" 
                        CausesValidation="false" 
                        ToolTip="Search for system setting(s) based on the criteria below." OnClick="btnSearch_Click"
                         /> 
                <input type="button" onclick="location.href='SettingsAdd.aspx'" value="Add new"
            id="btnAddNew" class="adminButtonRed" title="Add a new Setting parameter." />
    </div>
</div>
<table width="50%">
    <tr>
        <td class="adminData" style="width: 25%">
            <MEBSConfig:ToolTipLabel ID="lblFilterKey" runat="server" Text="Filter by Key:"
                ToolTip="Setting key." ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData" style="width: 25%">
            <asp:TextBox ID="txtFilterKey" runat="server" Width="200px"></asp:TextBox></td>
        <td>
        </td>
    </tr>
    <tr>
        <td colspan="3">
        </td>
    </tr>
</table>
<br />
<asp:GridView ID="gvSettings" runat="server" AutoGenerateColumns="false" Width="100%" OnPageIndexChanging="gvSettings_PageIndexChanging" PageSize="20" AllowPaging="true">
    <Columns>
        <asp:BoundField DataField="SettingID" HeaderText="Setting ID" Visible="False" ></asp:BoundField>
        <asp:TemplateField HeaderText="Setting Name" ItemStyle-Width="20%">
           <ItemTemplate>
                <a href="SettingsDetails.aspx?SettingID=<%#Eval("IdSetting")%>" title="Edit Setting">
                    <%#Server.HtmlEncode(Eval("SettingName").ToString())%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>        
        
        <asp:BoundField DataField="SettingValue" HeaderText="Value" ItemStyle-Width="20%">
        </asp:BoundField>
        
        <asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="50%">
        </asp:BoundField>
            <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" 
            ItemStyle-Width="10%">
           <ItemTemplate>
                <a href="SettingsDetails.aspx?SettingID=<%#Eval("IdSetting")%>" title="Edit">
                    Edit
                </a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
