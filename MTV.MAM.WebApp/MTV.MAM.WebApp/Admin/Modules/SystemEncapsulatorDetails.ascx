<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SystemEncapsulatorDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Admin.Modules.SystemEncapsulatorDetailsControl" %>

<%@ Register Src="~/Admin/Modules/SystemEncapsulatorInfo.ascx" TagName="SystemEncapsulatorInfo" TagPrefix="MSPCONFIG" %>
<div class="section-header">
    <div class="title">
        <img alt="" src="Common/ico-configuration.png" />
        Edit Encapsulator details <a href="SystemEncapsulator.aspx" title="Back to language list">(back to
            Encapsulator list)</a>
    </div>
    <div class="options">
        <asp:Button ID="SaveButton" runat="server" CssClass="adminButtonRed" OnClick="SaveButton_Click"
            Text="Save" ToolTip="Save current Encapsulator" />
        <asp:Button ID="DeleteButton" runat="server" CausesValidation="false" CssClass="adminButtonRed"
            OnClick="DeleteButton_Click" Text="Delete" ToolTip="Delete current Encapsulator" />
    </div>
</div>
<MSPCONFIG:SystemEncapsulatorInfo ID="ctrlEncapsulatorInfo" runat="server" />
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
