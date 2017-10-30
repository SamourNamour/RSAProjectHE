<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RedundancyParameters.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.RedundancyParametersControl" %>
<div class="section-header">
    <div class="title">
        <img alt="System settings" src="Common/ico-content.png" />
        Redundancy Parametr(s)</div>
</div>
<asp:GridView ID="gvRedundancyParameters" runat="server" AutoGenerateColumns="false" Width="100%">
    <Columns>
        
        <asp:BoundField DataField="IdSetting" HeaderText="Setting ID" Visible="False"></asp:BoundField>
            <asp:TemplateField HeaderText="Setting Key Name">
           <ItemTemplate>
                <a href="RedundancyDetails.aspx?SettingID=<%#Eval("IdSetting")%>" title="Edit Setting">
                    <%#Server.HtmlEncode(Eval("SettingName").ToString())%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
        
        <asp:BoundField DataField="SettingValue" HeaderText="Value">
        </asp:BoundField>
        
        <asp:BoundField DataField="Description" HeaderText="Description">
        </asp:BoundField>            
    </Columns>
</asp:GridView>
