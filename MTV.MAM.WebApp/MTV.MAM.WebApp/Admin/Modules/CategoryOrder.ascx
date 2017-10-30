<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryOrder.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.CategoryOrderControl" %>

<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-catalog.png" />
        Edit System Categorizations details <a href="Categories.aspx" title="Back to System Categorizations list">
            (back to System Categorizations list)</a>
    </div>
    <div class="options">
    </div>
</div>

<asp:GridView ID="gvCategorization" runat="server" AutoGenerateColumns="false" Width="100%" OnRowCommand="gvCategorization_RowCommand" OnRowDataBound="gvCategorization_RowDataBound" >
    <EmptyDataTemplate>
	<table cellspacing="0" rules="all" style="width:100%;border-collapse:collapse;">
		<tr class="headerstyle">
			<th scope="col" style="width:5%">#</th>
			<th scope="col" style="width:5%">Order</th>
			<th scope="col" style="width:25%">Title</th>
			<th scope="col" style="width:25%">MediaSet Name</th>
			<th scope="col" style="width:6%">ISO Code</th>
			<th scope="col" style="width:10%">MediaSet LCN</th>
			<th scope="col" style="width:10%">Standard LCN</th>
			<th scope="col" style="width:6%">Visibility</th>
			<th scope="col" style="width:5%">Default</th>
		</tr>
		<tr class="rowstyle">
			<td colspan="9" align="center" >
                   No Categories
            </td>
		</tr>
	</table>
    </EmptyDataTemplate>
    <Columns>      
        <asp:TemplateField HeaderText="#" HeaderStyle-HorizontalAlign="Center" 
        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
            <ItemTemplate>
                 <%# Container.DataItemIndex + 1 %>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="Order" HeaderStyle-HorizontalAlign="Center" 
        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%">
            <ItemTemplate>
                <asp:ImageButton ID="btnUP" CommandName="UP"
                        CommandArgument='<%# string.Format("{0}_{1}_{2}_{3}",Eval("IdCategory"),Eval("Orden"),Container.DataItemIndex,Eval("Default")) %>'
                         runat="server" ImageUrl="~/Admin/Common/up.png" />
                        <asp:ImageButton ID="btnDown" 
                        CommandArgument='<%# string.Format("{0}_{1}_{2}_{3}",Eval("IdCategory"),Eval("Orden"),Container.DataItemIndex,Eval("Default")) %>'
                        CommandName="DOWN" runat="server" ImageUrl="~/Admin/Common/down.png" />
            </ItemTemplate>
        </asp:TemplateField>    
        <asp:TemplateField HeaderText="Title" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="25%">
            <ItemTemplate>
                <asp:Label ID="lblTitle" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="MediaSet Name" HeaderStyle-HorizontalAlign="left" ItemStyle-Width="25%">
            <ItemTemplate>
                <asp:Label ID="lblMediaSetName" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:TemplateField HeaderText="ISO Code" HeaderStyle-HorizontalAlign="Center" 
        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%">
            <ItemTemplate>
                <asp:Label ID="lblISOCode" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField> 
        <asp:BoundField DataField="MediasetLCN" HeaderText="MediaSet LCN" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
        <asp:BoundField DataField="StandardLCN" HeaderText="Standard LCN" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%"></asp:BoundField>
        <asp:BoundField DataField="Visibility" HeaderText="Visibility" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="6%"></asp:BoundField>
        <asp:BoundField DataField="Default" HeaderText="Default" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="5%"></asp:BoundField>
    </Columns>    
</asp:GridView>