<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UserDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.UserDetailsControl" %>
<%@ Register Src="~/Admin/Modules/UserInfo.ascx" TagName="UserInfo" TagPrefix="MEBSConfig" %>
<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-configuration.png" />
        Edit User details <a href="Users.aspx" title="Back to User list">(back to User list)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonRed" OnClick="SaveButton_Click"
            Text="Save" ToolTip="Save language" />
        <asp:Button ID="DeleteButton" runat="server" CausesValidation="false" CssClass="adminButtonRed"
            OnClick="DeleteButton_Click" Text="Delete" ToolTip="Delete language" />
    </div>
</div>
<MEBSConfig:UserInfo ID="ctrlUserInfo" runat="server" />
<br />


<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    DisplayModalPopupID="ModalPopupExtenderDelete" TargetControlID="DeleteButton">
</ajaxToolkit:ConfirmButtonExtender>
<br />
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
