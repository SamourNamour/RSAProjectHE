<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UC_VodSystemOutput.ascx.cs" Inherits="MTV.MAM.WebApp.Modules.UC_VodSystemOutputControl" %>
<%@ Register Src="~/Controles/ToolTipLabelControl.ascx" TagName="ToolTipLabel" TagPrefix="MEBSMAM"  %>
<%@ import namespace="System.Data" %>
<div class="section-header">
    <div class="title">
        <img src="Common/schedule.png" alt="" /> 
        <asp:Label ID="lblLysisOutPutTitle" runat="server" ></asp:Label>
        <span>
            Vod System Output :
        </span>
    </div>
    <div class="options">
        <asp:Button runat="server" CssClass="adminButtonRed" ID="btnSearch" 
            ToolTip="Search" OnClick="btnSearch_Click" ValidationGroup="MKE" Text="Search" />
    </div>
</div>
    <table class="adminContent">
        <tr>
            <td class="adminTitle">
                <MEBSMAM:ToolTipLabel runat="server" ID="lblFrom" 
                    ToolTip="From." Text="From"
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData" colspan="2">
                <asp:TextBox ID="txtStartDate" runat="server" Width="130px" MaxLength="1" style="text-align:justify" ValidationGroup="MKE" CssClass="InputBlue" />
                <asp:ImageButton ID="iTimeFrom" runat="server" ImageUrl="~/Common/Ico_Calendar.png" CausesValidation="False" />
                <ajaxtoolkit:maskededitextender ID="MaskedEditExtenderFrom" runat="server"
                    TargetControlID="txtStartDate"
                    Mask="99/99/9999"
                    MessageValidatorTip="true"
                    OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError"
                    MaskType="Date"
                    DisplayMoney="Left"
                    AcceptNegative="Left"
                    ErrorTooltipEnabled="True" />
                <ajaxToolkit:MaskedEditValidator ID="MaskedEditValidatorFrom" runat="server"
                    ControlExtender="MaskedEditExtenderFrom"
                    ControlToValidate="txtStartDate"
                    EmptyValueMessage="Date is required"
                    IsValidEmpty="false"
                    Display="Dynamic"
                    EmptyValueBlurredText="Date is required"
                    ValidationGroup="MKE" />
                <ajaxToolkit:CalendarExtender ID="cStartDateButtonExtender" Format="dd/MM/yyyy"
                    runat="server" TargetControlID="txtStartDate" 
                    PopupButtonID="iTimeFrom" />   
            </td>
        </tr>
        <tr>
            <td class="adminTitle">
                <MEBSMAM:ToolTipLabel runat="server" ID="lblTo" 
                    ToolTip="To." Text="To"
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData" colspan="2">
                <asp:TextBox ID="txtStopDate" runat="server" Width="130px" MaxLength="1" style="text-align:justify" ValidationGroup="MKE" CssClass="InputBlue" />
                <asp:ImageButton ID="iTimeTo" runat="server" ImageUrl="~/Common/Ico_Calendar.png" CausesValidation="False" />
                <ajaxtoolkit:maskededitextender ID="MaskedEditExtenderTo" runat="server"
                    TargetControlID="txtStopDate"
                    Mask="99/99/9999"
                    MessageValidatorTip="true"
                    OnFocusCssClass="MaskedEditFocus"
                    OnInvalidCssClass="MaskedEditError"
                    MaskType="Date"
                    DisplayMoney="Left"
                    AcceptNegative="Left"
                    ErrorTooltipEnabled="True" />
                <ajaxtoolkit:maskededitvalidator ID="MaskedEditValidatorTo" runat="server"
                    ControlExtender="MaskedEditExtenderTo"
                    ControlToValidate="txtStopDate"
                    EmptyValueMessage="Date is required"
                    IsValidEmpty="false"
                    Display="Dynamic"
                    EmptyValueBlurredText="Date is required"
                    ValidationGroup="MKE" />
                <ajaxtoolkit:calendarextender ID="cStopDateButtonExtender" Format="dd/MM/yyyy"
                    runat="server" TargetControlID="txtStopDate" PopupButtonID="iTimeTo" />   

            </td>
        </tr>
        <tr>
            <td class="adminTitle">
                <MEBSMAM:ToolTipLabel runat="server" ID="lblTitle"
                    ToolTip="Event Title." Text="Title"
                    ToolTipImage="~/Common/ico-help.png" />
            </td>
            <td class="adminData" colspan="2">
                <asp:TextBox runat="server" ID="txtTitle" Width="220px" />
            </td>
        </tr>
</table>   
<br />

<asp:GridView ID="gvwdatacast" runat="server" AutoGenerateColumns="false" 
    Width="100%"  AllowPaging="true" 
    PageSize="25" OnPageIndexChanging="gvwdatacast_PageIndexChanging" 
    >
    <Columns>
        <asp:TemplateField HeaderText="Product ID" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                    <strong><asp:Label ID="lblMasterProdId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EventId") %>'  ></asp:Label></strong>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Title" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left">
            <ItemTemplate>                    
                <strong><asp:LinkButton ID="linkTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title") %>' PostBackUrl='<%# String.Format("~/DCDetails.aspx?EventID={0}", DataBinder.Eval(Container.DataItem, "IdIngesta")) %>'  ></asp:LinkButton></strong>
            </ItemTemplate>
        </asp:TemplateField>
 
        <asp:TemplateField HeaderText="Edit" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
            <ItemTemplate>
               <asp:ImageButton ID="btnEdit" runat="server" 
               ImageUrl="~/Common/ico-edit.png" 
               PostBackUrl='<%# String.Format("~/DCDetails.aspx?EventID={0}", DataBinder.Eval(Container.DataItem, "IdIngesta")) %>' />
            </ItemTemplate>
        </asp:TemplateField> 
    </Columns>
</asp:GridView>
<br />