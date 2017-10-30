<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_VodPrograming.ascx.cs" Inherits="MTV.MAM.WebApp.Modules.UC_VodProgramingControl" %>
<%@ Register Src="~/Controles/ToolTipLabelControl.ascx" TagName="ToolTipLabel" TagPrefix="MEBSMAM" %>
<div class="section-header">
    <div class="title">
        <img src="Common/schedule.png" alt="" /> 
       <span>
        Vod Programing :
       </span>
    </div>
    <div class="options">
        <asp:Button runat="server" Text="Search" CssClass="adminButtonRed" ID="btnSearch" ValidationGroup="MKE" ToolTip="Search" OnClick="btnSearch_Click"  />
        <asp:Button runat="server" Text="Refresh" Visible="false" CssClass="adminButtonRed" ID="btnRefresh" ToolTip="Refresh" OnClick="btnRefresh_Click"  />
        <asp:Button ID="btnParcourir" runat="server" Text="..." CssClass="adminButtonRed" OnClick="btnParcourir_Click" Width="53px" Enabled="false" />    
        <asp:Button ID="btnEdit" runat="server" Text="Edit" CssClass="adminButtonRed" OnClick="btnEdit_Click" Enabled="false" />
        <asp:Button ID="btnSchedule" runat="server" Text="Schedule" CssClass="adminButtonRed" OnClick="btnSchedule_Click" Enabled="false" />
    </div>
</div>
<table>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblDate"
                ToolTip="Date" Text="Date"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtSelectedDate" CssClass="InputBlue" />
            <asp:ImageButton runat="Server" ID="iStartDate" ImageUrl="~/Common/Ico_Calendar.png"
                AlternateText="Click to show calendar" />
                <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderSelectedDate" runat="server"
                    TargetControlID="txtSelectedDate"
                    Mask="99/99/9999"
                    MessageValidatorTip="true"
                    OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError"
                    MaskType="Date"
                    DisplayMoney="Left"
                    AcceptNegative="Left"
                    ErrorTooltipEnabled="True" />
                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidatorSelectedDate" runat="server"
                    ControlExtender="MaskedEditExtenderSelectedDate"
                    ControlToValidate="txtSelectedDate"
                    EmptyValueMessage="Date is required"
                    Display="Dynamic"
                    EmptyValueBlurredText="Date is required"
                    IsValidEmpty="false"
                    ValidationGroup="MKE" />
            <ajaxToolkit:CalendarExtender ID="cStartDateButtonExtender" runat="server" TargetControlID="txtSelectedDate" Format="dd/MM/yyyy"
                PopupButtonID="iStartDate" />
        </td>
    </tr>
</table>
<br />
<table width="100%" >
    <tr>
        <td style="width:70%;vertical-align:top">
            <asp:PlaceHolder ID="PlaceTimeTable" runat="server"></asp:PlaceHolder>
        </td>
        <td style="vertical-align:top" >
            <asp:PlaceHolder ID="PH_IngestaInformation" runat="server">
                <div id="charts">
                    <div id="text_charts">

                    </div>
                </div>
            </asp:PlaceHolder>
            
            <asp:PlaceHolder ID="PHLegend" runat="server">
                <div id="legendIng" style="padding-top:15px;">
                    <div id="text_legend">
                        <fieldset>
                            <legend><asp:Label ID="lblContentStatus" runat="server" CssClass="Label" > STATUS </asp:Label></legend>
                            <table class="TableLegend">
                                <tr>
                                    <td><asp:Image ID="imgPrepared" runat="server" ImageUrl="~/Common/prapared.png" /> </td>
                                    <td><asp:Label ID="lblPrepared" runat="server"> EVENT_PREPARED </asp:Label> </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="imgRecording" runat="server" ImageUrl="~/Common/started.png" /> </td>
                                    <td><asp:Label ID="lblRecording" runat="server">EVENT_RECORDING</asp:Label> </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="imgEnded" runat="server" ImageUrl="~/Common/stoped.png" /> </td>
                                    <td><asp:Label ID="lblEnded" runat="server">EVENT_RECORDED</asp:Label> </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="imgMissing" runat="server" ImageUrl="~/Common/unknownerror.png" /> </td>
                                    <td><asp:Label ID="lblMissing" runat="server">ERROR</asp:Label> </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="imgLocked" runat="server" ImageUrl="~/Common/locked.png" /> </td>
                                    <td><asp:Label ID="lblLocked" runat="server">EVENT_LOCKED</asp:Label> </td>
                                </tr>
                                <tr>
                                    <td><asp:Image ID="imgExpired" runat="server" ImageUrl="~/Common/Expired.png" /> </td>
                                    <td><asp:Label ID="lblExpired" runat="server" >EVENT_EXPIRED</asp:Label> </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </div>
            </asp:PlaceHolder>
        </td>
    </tr>
</table>

