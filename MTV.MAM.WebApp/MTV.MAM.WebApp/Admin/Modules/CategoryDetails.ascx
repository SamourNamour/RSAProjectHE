<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CategoryDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.CategoryDetailsControl" %>
<%@ Register Src="~/Admin/Modules/CategoryInfo.ascx" TagName="CategoryInfo" TagPrefix="MEBSConfig" %>
<%@ Register TagPrefix="MEBSConfig" TagName="CatgeoryMappings" Src="~/Admin/Modules/CategoryMappings.ascx" %>
<%@ Register Src="~/Admin/Modules/CategoryOrder.ascx" TagName="CategoryOrder" TagPrefix="MEBSConfig" %>
<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-catalog.png" />
        Edit System Categorizations details <a href="Categories.aspx" title="Back to System Categorizations list">
            (back to System Categorizations list)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonRed" OnClick="SaveButton_Click"
            Text="Save" ToolTip="Save current Categorizations." />
        <asp:Button ID="DeleteButton" runat="server" CausesValidation="false" CssClass="adminButtonRed"
            OnClick="DeleteButton_Click" Text="Delete" ToolTip="Delete current Categorizations." />
    </div>
</div>

<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server" DisplayModalPopupID="ModalPopupExtenderDelete" TargetControlID="DeleteButton">
</ajaxToolkit:ConfirmButtonExtender>

<br />


<ajaxToolkit:TabContainer runat="server" ID="CategoryTabs" ActiveTabIndex="0">
    <ajaxToolkit:TabPanel runat="server" ID="pnlCategoryInfo" HeaderText="Category Info">
        <ContentTemplate>
            <MEBSConfig:CategoryInfo ID="ctrlCategoryInfo" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    
      <ajaxToolkit:TabPanel runat="server" ID="pnlCategoryMappings" HeaderText="Category Mappings" TabIndex="1">
        <ContentTemplate>
           <MEBSConfig:CatgeoryMappings ID="ctrlCatgeoryMappings" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    
      <ajaxToolkit:TabPanel runat="server" ID="pnlCategoryOrder" HeaderText="Swap SubCategories" TabIndex="2">
        <ContentTemplate>
           <MEBSConfig:CategoryOrder ID="ctrlCategoryOrder" IdTab="2" runat="server" />
        </ContentTemplate>
    </ajaxToolkit:TabPanel>
    
</ajaxToolkit:TabContainer>


<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" BackgroundCssClass="modalBackground"
    CancelControlID="deleteButtonCancel" OkControlID="deleteButtonOk" PopupControlID="pnlDeletePopup"
    TargetControlID="DeleteButton">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlDeletePopup" runat="server" Style="border-right: black 2px solid;
    padding-right: 20px; border-top: black 2px solid; display: none; padding-left: 20px;
    padding-bottom: 20px; border-left: black 2px solid; width: 250px; padding-top: 20px;
    border-bottom: black 2px solid; background-color: white">
    <div style="text-align: center">
        Are you sure you want to permanently delete this record?
        <br />
        <br />
        <asp:Button ID="deleteButtonOk" runat="server" CausesValidation="false" CssClass="adminButton"
            Text="OK" />
        <asp:Button ID="deleteButtonCancel" runat="server" CausesValidation="false" CssClass="adminButton"
            Text="Cancel" />
    </div>
</asp:Panel>
<br />
<br />
<div id="CategorizationRaisedErrors" runat="server" class="warnings" visible="false">
    <div class="section-header">
        <div class="title">
            <img alt="Warnings" src="Common/ico-warnings.gif" />
            Error
        </div>
    </div>
    <div>
        <asp:Label ID="lblErrors" runat="server"></asp:Label>
    </div>
</div>
<br />