<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_DCScheduleDetails.ascx.cs" Inherits="MTV.MAM.WebApp.Modules.UC_DCScheduleDetails" %>
<%@ Register Src="~/Controles/NumericTextBox.ascx" TagName="NumericTextBox" TagPrefix="MEBSMAM"  %>
<%@ Register Src="~/Controles/ToolTipLabelControl.ascx" TagName="ToolTipLabel" TagPrefix="MEBSMAM"  %>
<%@ Register Src="~/Controles/ImageUpload.ascx" TagName="ImageUpload" TagPrefix="MEBSMAM"  %>
<div class="section-header">
    <div class="title">
        <img src="Common/schedule.png" alt="" /> 
        <span>Vod Schedule Details :</span>
            <a href="VodPrograming.aspx" title="Back to Vod Programing">(<span>Back to Vod Programing</span>)</a>
    </div>
    <div class="options">
    <div class="options">
        <asp:Button ID="BtnSave" runat="server" Text="Save"
        CssClass="adminButtonRed" Width="60px" 
        OnClick="BtnSave_Click" ToolTip="Save The Modification" 
            ValidationGroup="SaveVG" />
        
        <asp:Button runat="server" CssClass="adminButtonRed" ID="BtnDelete" Text="Delete"
            ToolTip="Delete Current Event" OnClick="BtnDelete_Click"  />
        
    </div>
</div>
</div>
<table class="adminContent">
    <tr>
        <td class="adminTitle" width="210px">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblTitle"
                ToolTip="Event Title."
                ToolTipImage="~/Common/ico-help.png" Text="Title" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtTitle" Width="250px" CssClass="InputBlue" MaxLength="256"/>
        </td>
        <td rowspan="18" class="TdDetails" align="left">
            <div class="overview">
                <strong><asp:Label ID="lblContentIDOverview" runat="server" Text="Content ID : " ></asp:Label></strong>
                <asp:Label ID="lblContentIDResult" runat="server" CssClass="InputBlue" ></asp:Label>
                <br /><br />     
                 <strong><asp:Label ID="lblCodiIOverviewD" runat="server" Text="CodiID : " ></asp:Label></strong>
                <asp:Label ID="lblCodiIDResult" runat="server" CssClass="InputBlue" ></asp:Label>
                <br /><br />      
                <strong><asp:Label ID="lblTitleOverview" runat="server" Text="Title : " ></asp:Label></strong>
                <asp:Label ID="lblTitleResult" runat="server" CssClass="InputBlue"></asp:Label>
                <br /><br />
                <strong><asp:Label ID="lblStartTimeOverview" runat="server" Text="Estimated Start : "></asp:Label></strong>
                <asp:Label ID="lblStartTimeResult" runat="server" CssClass="InputBlue"></asp:Label>
                <br /><br />
                <strong><asp:Label ID="lblStopTimeOverview" runat="server" Text="Estimated Stop : " ></asp:Label></strong>
                <asp:Label ID="lblStopTimeResult" runat="server" CssClass="InputBlue"></asp:Label>
                <br /><br />
                <strong><asp:Label ID="lblChannelOverview" runat="server" Text="Channel Name : " ></asp:Label></strong>
                <asp:Label ID="lblChannelTimeResult" runat="server" CssClass="InputBlue"></asp:Label>
                <br /><br />
                <strong><asp:Label ID="lblEventstatusOverview" runat="server" Text="Event Status : " ></asp:Label></strong>
               <strong> <asp:Label ID="lblStatusResult" runat="server" CssClass="InputBlue"></asp:Label></strong>
            </div>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblDirector" 
                ToolTip="Event Directors."
                ToolTipImage="~/Common/ico-help.png" Text="Directors" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtDirectors" Width="456px" CssClass="InputBlue" MaxLength="48"/>
            <asp:Label ID="lblInfosDirectos" runat="server" CssClass="LabelInfo" Text="The Directors must be separed by (;)" ></asp:Label></td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblActors" 
                ToolTip="Event Actors."
                ToolTipImage="~/Common/ico-help.png" Text="Actors" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtActors" Width="456px" CssClass="InputBlue" MaxLength="48" />
            <asp:Label ID="lblInfosActors" runat="server" CssClass="LabelInfo" Text="The Actors must be separed by (;)" ></asp:Label>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblYears" 
                ToolTip="Event Year."
                ToolTipImage="~/Common/ico-help.png" Text="Year" />
        </td>
        <td class="adminData">
            <MEBSMAM:NumericTextBox ID="txtYear" runat="server" CssClass="InputBlue" Value="0" ValidationGroup="MKE"
            RequiredErrorMessage="Yearis required" RangeErrorMessage="The value must be less than 9999"
            MinimumValue="0" MaximumValue="9999" Width="100" MaxLength="4"/>           
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblCountry" 
                ToolTip="Event Country."
                ToolTipImage="~/Common/ico-help.png" Text="Country" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtCountry" Width="250px" CssClass="InputBlue"/>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblShortDescription" 
                ToolTip="Event Short Description."
                ToolTipImage="~/Common/ico-help.png" Text="Short Description" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtShortDescription" TextMode="MultiLine" CssClass="InputBlue" Height="75px" Width="456px" MaxLength="1024" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblDescription" 
                ToolTip="Event Long Description."
                ToolTipImage="~/Common/ico-help.png" Text="Long Description " />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtDescription" TextMode="MultiLine" CssClass="InputBlue" Height="75px" Width="456px" MaxLength="1024" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblGenre" 
                ToolTip="Event Genre." Text="Genre"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtGenre" CssClass="InputBlue" Width="456px" MaxLength="300" />
            <asp:Label ID="lblGenreInfos" runat="server" CssClass="LabelInfo" Text="The Genre must be separed by (;)"></asp:Label>
        </td>
    </tr>
<%--    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblFrom" 
                ToolTip="Event Form." Text="Form"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtForm" CssClass="InputBlue" Width="456px" MaxLength="300" />
        </td>
    </tr>--%>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblCategory" 
                ToolTip="Event Category." Text="Category"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:ListBox ID="listCategory" runat="server" Width="250px" SelectionMode="Multiple" Height="150px"></asp:ListBox> 
                    </td>
                    <td valign="middle" style="padding-left:10px" >
<%--                        <asp:CustomValidator ID="CustomValidator1" runat="server" 
                        ValidationGroup="MKE"  EnableClientScript="true" 
                        ErrorMessage="Theme Is Required" OnServerValidate="CustomValidator1_ServerValidate" ></asp:CustomValidator>     --%>      
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblParentalRating" 
                ToolTip="Event Parental Rating." Text="Parental Rating"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox ID="txtParentalRating" runat="server" Width="116px" CssClass="InputBlue"></asp:TextBox>
            <asp:Label ID="lblParentalInfos" runat="server" CssClass="LabelInfo" ></asp:Label>
        </td>
    </tr>
        <tr>
            <td class="adminTitle">
                <MEBSMAM:ToolTipLabel runat="server" ID="lblExpiration" 
                    ToolTip="Specifies the amount of time (in minutes) after which the content will expire" Text="Expiration"
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData" colspan="2">
                <asp:TextBox ID="txtExpiration" runat="server" Width="116px" MaxLength="1" style="text-align:justify" ValidationGroup="MKE" CssClass="InputBlue" />
                <asp:ImageButton ID="iExpiration" runat="server" ImageUrl="~/Common/Ico_Calendar.png" CausesValidation="False" />
                <ajaxtoolkit:maskededitextender ID="MaskedEditExtenderExpiration" runat="server"
                    TargetControlID="txtExpiration"
                    Mask="99/99/9999"
                    MessageValidatorTip="true"
                    OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError"
                    MaskType="Date"
                    DisplayMoney="Left"
                    AcceptNegative="Left"
                    ErrorTooltipEnabled="True" />
                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidatorExpiration" runat="server"
                    ControlExtender="MaskedEditExtenderExpiration"
                    ControlToValidate="txtExpiration"
                    EmptyValueMessage="Date is required"
                    IsValidEmpty="false"
                    Display="Dynamic"
                    EmptyValueBlurredText="Date is required"
                    ValidationGroup="MKE" />
                <ajaxToolkit:CalendarExtender ID="cExpirationButtonExtender" Format="dd/MM/yyyy"
                    runat="server" TargetControlID="txtExpiration" 
                    PopupButtonID="iExpiration" />   
            </td>
        </tr>
        <tr>
            <td class="adminTitle">
                <MEBSMAM:ToolTipLabel runat="server" ID="lblImmortality" 
                    ToolTip="specifies the amount of time (in minutes) during which the content will be immortal. This value cannot be greater than ‘ExpiresAfter’." Text="Immortality"
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData" colspan="2">
                <asp:TextBox ID="txtImmortality" runat="server" Width="116px" MaxLength="1" style="text-align:justify" ValidationGroup="MKE" CssClass="InputBlue" />
                <asp:ImageButton ID="iImmortality" runat="server" ImageUrl="~/Common/Ico_Calendar.png" CausesValidation="False" />
                <ajaxtoolkit:maskededitextender ID="MaskedEditExtenderImmortality" runat="server"
                    TargetControlID="txtImmortality"
                    Mask="99/99/9999"
                    MessageValidatorTip="true"
                    OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError"
                    MaskType="Date"
                    DisplayMoney="Left"
                    AcceptNegative="Left"
                    ErrorTooltipEnabled="True" />
                <ajaxtoolkit:maskededitvalidator ID="MaskedEditValidatorImmortality" runat="server"
                    ControlExtender="MaskedEditExtenderImmortality"
                    ControlToValidate="txtImmortality"
                    EmptyValueMessage="Date is required"
                    IsValidEmpty="false"
                    Display="Dynamic"
                    EmptyValueBlurredText="Date is required"
                    ValidationGroup="MKE" />
                <ajaxtoolkit:calendarextender ID="cImmortalityButtonExtender" Format="dd/MM/yyyy"
                    runat="server" TargetControlID="txtImmortality" PopupButtonID="iImmortality" />   

            </td>
        </tr>

    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblScreenFormat" 
                ToolTip="Event Genre." Text="Content Format"
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
            <asp:TextBox runat="server" ID="txtScreenFormat" CssClass="InputBlue" Width="116px" MaxLength="300" />
        </td>
    </tr>
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblPoster" Text="Select Poster"
                ToolTip="Event Poster."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
           <MEBSMAM:ImageUpload ID="IdImageUpload" runat="server" />
        </td>
    </tr>  
    <tr>
        <td class="adminTitle">
            <MEBSMAM:ToolTipLabel runat="server" ID="lblPriority" Text="Preservation Priority"
                ToolTip="Content Priority."
                ToolTipImage="~/Common/ico-help.png" />
        </td>
        <td class="adminData">
           <MEBSMAM:NumericTextBox ID="txtPriority" runat="server" MinimumValue="0" MaximumValue="255" Value="0" Width="116px" />
        </td>
    </tr>
    </table>   
<br />
<ajaxToolkit:ConfirmButtonExtender ID="ConfirmDeleteButtonExtender" runat="server"
    DisplayModalPopupID="ModalPopupExtenderDelete" TargetControlID="BtnDelete">
</ajaxToolkit:ConfirmButtonExtender>
<br />
<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderDelete" runat="server" BackgroundCssClass="modalBackground"
    CancelControlID="deleteButtonCancel" OkControlID="deleteButtonOk" PopupControlID="pnlDeletePopup"
    TargetControlID="BtnDelete">
</ajaxToolkit:ModalPopupExtender>
<asp:Panel ID="pnlDeletePopup" runat="server" Style="border-right: black 2px solid;
    padding-right: 20px; border-top: black 2px solid; display: none; padding-left: 20px;
    padding-bottom: 20px; border-left: black 2px solid; width: 250px; padding-top: 20px;
    border-bottom: black 2px solid; background-color: white">
    <div style="text-align: center">
            Are you sure want to delete this record ??
        <br />
        <br />
        <asp:Button ID="deleteButtonOk" runat="server" CausesValidation="false" CssClass="adminButton"
            Text="OK" />
        <asp:Button ID="deleteButtonCancel" runat="server" CausesValidation="false" CssClass="adminButton"
            Text="Cancel" />
    </div>
</asp:Panel>