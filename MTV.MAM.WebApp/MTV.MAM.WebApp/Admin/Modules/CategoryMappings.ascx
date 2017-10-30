<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CategoryMappings.ascx.cs"
    Inherits="MTV.MAM.WebApp.Admin.Modules.CategoryMappings" %>
<%@ Register TagPrefix="MEBSConfig" TagName="NumericTextBox" Src="~/Controles/NumericTextBox.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="SimpleTextBox" Src="~/Controles/SimpleTextBox.ascx" %>
<%@ Register TagPrefix="MEBSConfig" TagName="ToolTipLabel" Src="~/Controles/ToolTipLabelControl.ascx" %>
<table class="adminContent" width="100%">
    <tr>
        <td colspan="3">
            <MEBSConfig:ToolTipLabel ID="lblComment_Linked" runat="server" ToolTipImage="~/Common/ico-help.png" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="height: 25px">
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:GridView ID="gvCategoryMappings" runat="server" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="gvCategoryMappings_RowDataBound" OnPageIndexChanging="gvCategoryMappings_PageIndexChanging"
                AllowPaging="true" PageSize="15" OnRowCommand="gvCategoryMappings_RowCommand">
                <Columns>
                    <asp:BoundField DataField="idCategory" HeaderText="ID" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="Value" HeaderText="Value"></asp:BoundField>
                    <asp:TemplateField HeaderText="Title">
                        <ItemTemplate>
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mediaset Name">
                        <ItemTemplate>
                            <asp:Label ID="lblChannelVirtualName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Visibility" HeaderText="Visibility"></asp:BoundField>
                    <asp:BoundField DataField="MediasetLCN" HeaderText="Mediaset LCN"></asp:BoundField>
                    <asp:BoundField DataField="StandardLCN" HeaderText="Standard LCN"></asp:BoundField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfCategoryID" runat="server" Value='<%# Eval("idCategory") %>' />
                            <asp:Button ID="DeleteCategoryButton" runat="server" CssClass="adminButton" CommandName="DeleteCategory"
                                Text="Delete" CommandArgument='<%#Eval("idCategory")%>'
                                CausesValidation="false" ToolTip="Delete Category" />
                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
                                DisplayModalPopupID="ModalPopupExtenderDelete" TargetControlID="DeleteCategoryButton">
                            </ajaxToolkit:ConfirmButtonExtender>
                            <br />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="deleteButtonCancel" OkControlID="deleteButtonOk" PopupControlID="pnlDeletePopup"
                                TargetControlID="DeleteCategoryButton">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlDeletePopup" runat="server" Style="border-right: black 2px solid;
                                padding-right: 20px; border-top: black 2px solid; display: none; padding-left: 20px;
                                padding-bottom: 20px; border-left: black 2px solid; width: 250px; padding-top: 20px;
                                border-bottom: black 2px solid; background-color: white">
                                <div style="text-align: center; color: #cd3711">
                                    Are you sure about deleting this Category?
                                    <br />
                                    <br />
                                    <asp:Button ID="deleteButtonOk" runat="server" CausesValidation="false" CssClass="adminButton"
                                        Text="OK" />
                                    <asp:Button ID="deleteButtonCancel" runat="server" CausesValidation="false" CssClass="adminButton"
                                        Text="Cancel" />
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <center>
                <h2>
                    Form - 1 -</h2>
            </center>
        </td>
    </tr>
    <tr>
        <td colspan="3" style="height: 50px">
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <MEBSConfig:ToolTipLabel ID="lblComment_Available" runat="server" ToolTipImage="~/Common/ico-help.png" />
        </td>
    </tr>
    <tr>
        <td colspan="3" style="height: 25px">
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:GridView ID="gvAvialbleCategoryCollection" runat="server" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="gvAvialbleCategoryCollection_RowDataBound" OnPageIndexChanging="gvAvialbleCategoryCollection_PageIndexChanging"
                AllowPaging="true" PageSize="50" OnRowCommand="gvAvialbleCategoryCollection_RowCommand">
                <Columns>
                    <asp:BoundField DataField="idCategory" HeaderText="ID" Visible="False"></asp:BoundField>
                    <asp:BoundField DataField="Value" HeaderText="Value"></asp:BoundField>
                    <asp:TemplateField HeaderText="Title">
                        <ItemTemplate>
                            <asp:Label ID="lblTitle" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Mediaset Name">
                        <ItemTemplate>
                            <asp:Label ID="lblChannelVirtualName" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Visibility" HeaderText="Visibility"></asp:BoundField>
                    <asp:BoundField DataField="MediasetLCN" HeaderText="Mediaset LCN"></asp:BoundField>
                    <asp:BoundField DataField="StandardLCN" HeaderText="Standard LCN"></asp:BoundField>
                    <asp:TemplateField HeaderText="Delete">
                        <ItemTemplate>
                            <asp:HiddenField ID="hfCategoryID" runat="server" Value='<%# Eval("idCategory") %>' />
                            <asp:Button ID="DeleteCategoryButton" runat="server" CssClass="adminButton" CommandName="AddCategory"
                                Text="Add" CommandArgument='<%#Eval("idCategory")%>'
                                CausesValidation="false" ToolTip="Delete Category" />
                            <ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
                                DisplayModalPopupID="ModalPopupExtenderDelete" TargetControlID="DeleteCategoryButton">
                            </ajaxToolkit:ConfirmButtonExtender>
                            <br />
                            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" BackgroundCssClass="modalBackground"
                                CancelControlID="deleteButtonCancel" OkControlID="deleteButtonOk" PopupControlID="pnlDeletePopup"
                                TargetControlID="DeleteCategoryButton">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="pnlDeletePopup" runat="server" Style="border-right: black 2px solid;
                                padding-right: 20px; border-top: black 2px solid; display: none; padding-left: 20px;
                                padding-bottom: 20px; border-left: black 2px solid; width: 250px; padding-top: 20px;
                                border-bottom: black 2px solid; background-color: white">
                                <div style="text-align: center; color: #cd3711">
                                    Are you sure about adding this Category?
                                    <br />
                                    <br />
                                    <asp:Button ID="deleteButtonOk" runat="server" CausesValidation="false" CssClass="adminButton"
                                        Text="OK" />
                                    <asp:Button ID="deleteButtonCancel" runat="server" CausesValidation="false" CssClass="adminButton"
                                        Text="Cancel" />
                                </div>
                            </asp:Panel>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <center>
                <h2>
                    Form - 2 -</h2>
            </center>
        </td>
    </tr>
</table>
<br />
&nbsp;<br />
<br />
<br />
<br />
