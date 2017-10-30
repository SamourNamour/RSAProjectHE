<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemEncapsulator.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SystemEncapsulatorControl" %>
<div class="section-header">
    <div class="title">
        <img alt="Encapsulator(s)" src="Common/ico-content.png" />
        System Encapsulator(s)
    </div>
    <div class="options">
                <input type="button" onclick="location.href='SystemEncapsulatorAdd.aspx'" value="Add new"
            id="btnAddNew" class="adminButtonRed" title="Add a new Encapsulator" />
    </div>
</div>
<br />
<asp:GridView ID="gvEncapsulators" runat="server" AutoGenerateColumns="false" Width="100%">
    <Columns>
        
        <asp:BoundField DataField="IdEncapsulador" HeaderText="EncapsulatorID" Visible="False"></asp:BoundField>
            <asp:TemplateField HeaderText="Encapsulator Server Name" ItemStyle-Width="20%">
           <ItemTemplate>
                <a href="SystemEncapsulatorDetails.aspx?IdEncapsulador=<%#Eval("IdEncapsulator")%>" title="Edit Encapsulator">
                    <%#Server.HtmlEncode(Eval("Name").ToString())%>
                </a>
            </ItemTemplate>
        </asp:TemplateField>
                
        <asp:BoundField DataField="Type" HeaderText="Type">
        </asp:BoundField>         
<%--        <asp:BoundField DataField="Status" HeaderText="Status">
        </asp:BoundField>--%>
        <asp:BoundField DataField="IsPublished" HeaderText="Published">
        </asp:BoundField>
        <asp:BoundField DataField="IpAddress" HeaderText="IpAdress">
        </asp:BoundField>
        <asp:TemplateField HeaderText="Edit" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
            <ItemTemplate>
                <a href="SystemEncapsulatorDetails.aspx?IdEncapsulador=<%#Eval("IdEncapsulator")%>"
                    title="Edit Encapsulator">Edit</a>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>

