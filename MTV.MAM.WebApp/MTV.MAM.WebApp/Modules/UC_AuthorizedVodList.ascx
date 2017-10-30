<%@ Control Language="C#" AutoEventWireup="true" Codebehind="UC_AuthorizedVodList.ascx.cs"
    Inherits="MTV.MAM.WebApp.Modules.UC_AuthorizedVodListControl" %>
<%@ Register Src="~/Controles/ToolTipLabelControl.ascx" TagName="ToolTipLabel" TagPrefix="EBSMAM" %>
<%@ Register Src="~/Controles/TextBoxTiming.ascx" TagName="TextBoxTiming" TagPrefix="EBSMAM" %>
<div class="section-header">
    <div class="title">
        <img src="Common/ico-Epg.png" alt="" />
        <span>
            Authorized Vod :
        </span><a href="VodPrograming.aspx" title="Back to Vod Programing">
            (<span>Back to Vod Programing</span>)</a>
    </div>
    <div class="options">
    </div>
</div>
<br />
<asp:PlaceHolder ID="TableTiming" runat="server">
<table border="0" class="UserContent">
    <tr>
        <td class="UserTitle">
            <EBSMAM:ToolTipLabel runat="server" ID="lblDate" ToolTip="Selected Date." ToolTipImage="~/Common/ico-help.png" Text="Date" />
        </td>
        <td class="adminData" colspan="2">
            <asp:Label ID="lblStartDateResult" runat="server" Width="115px" Height="15px" CssClass="lblDesabled"></asp:Label>
        </td>
        <td align="right">
            &nbsp;</td>
        <td colspan="2">
            <asp:Label ID="lblEndDateResult" runat="server" Width="115px" CssClass="lblDesabled"></asp:Label>
        </td>
    </tr>
    <tr>
        <td></td>
        <td colspan="2">
            From</td>
        <td style="width: 144px"></td>
        <td colspan="2">
            To</td>
    </tr>
    <tr>
        <td class="UserTitle">
            <EBSMAM:ToolTipLabel runat="server" ID="lblStartTime" ToolTip="Start Time." ToolTipImage="~/Common/ico-help.png" Text="Start Time" />
        </td>
        <td style="width: 60px">
            <asp:Label ID="lblStartTimeResultat" runat="server" Width="50px" CssClass="lblDesabled"></asp:Label>
        </td>
        <td>
           <asp:TextBox ID="txtNewStartTime" runat="server" Width="50px" ValidationGroup="MKE"></asp:TextBox>
        <ajaxToolkit:MaskedEditExtender ID="MEExtenderNewStartTime" runat="server"
            TargetControlID="txtNewStartTime" 
            Mask="99:99"
            MessageValidatorTip="true"
            OnFocusCssClass="MaskedEditFocus"
            OnInvalidCssClass="MaskedEditError"
            MaskType="Time"
            AcceptAMPM="false"
            ErrorTooltipEnabled="True" />
        <ajaxToolkit:MaskedEditValidator ID="MEValidatorNewStartTime" runat="server"
            ControlExtender="MEExtenderNewStartTime"
            ControlToValidate="txtNewStartTime"
            IsValidEmpty="False"
            EmptyValueMessage="Time is required"
            InvalidValueMessage="Time is invalid"
            Display="Dynamic"
            EmptyValueBlurredText="Time is required"
            InvalidValueBlurredMessage="Time is invalid"
            ValidationGroup="MKE"/>
        </td>
        <td style="width: 144px">
            &nbsp;</td>
        <td align="right">
            <asp:Label ID="lblMaxEndTime" runat="server" Width="50px" Height="15px" CssClass="lblDesabled" ></asp:Label>
        </td>
        <td style="padding-left:10px;width:50px" align="right">
            <asp:Label ID="lblEndTimeResult" runat="server" Width="50px" CssClass="lblDesabled" ></asp:Label>
        </td>
    </tr>
</table>
<br />
</asp:PlaceHolder>

<asp:Label ID="lblInfos" runat="server" Visible="false"></asp:Label>
<asp:GridView ID="gvwdatacast" runat="server" AutoGenerateColumns="false" 
    Width="100%"  AllowPaging="true" 
    PageSize="25" OnPageIndexChanging="gvwdatacast_PageIndexChanging" onrowcommand="gvwdatacast_RowCommand" 
    >
    <Columns>
        <asp:TemplateField HeaderText="Product ID" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="center">
            <ItemTemplate>
                    <strong><asp:Label ID="lblMasterProdId" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "EventId") %>'  ></asp:Label></strong>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Title" ItemStyle-Width="35%" ItemStyle-HorizontalAlign="Left">
            <ItemTemplate>                    
                <strong><asp:LinkButton ID="linkTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "Title") %>' CommandName="SelectItem" 
                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IdIngesta").ToString() %>'
                ></asp:LinkButton></strong>
                <asp:HiddenField ID="HFCodePackage" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "Code_Package") %>' />
                <asp:HiddenField ID="HFEventID" runat="server" Value='<%#DataBinder.Eval(Container.DataItem, "EventId") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="center" ItemStyle-Width="10%">
            <ItemTemplate>
               <asp:LinkButton ID="btnSelectItem" runat="server" ValidationGroup="MKE" Text="Select"
                CommandName="SelectItem" 
                CommandArgument='<%#DataBinder.Eval(Container.DataItem, "IdIngesta").ToString() %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField> 
    </Columns>
</asp:GridView>

<%--<asp:GridView ID="gvwPVod" runat="server" AutoGenerateColumns="false" Width="100%" PageSize ="20"
    OnRowDataBound="gvwPVod_RowDataBound" OnRowCommand="gvwPVod_RowCommand" AllowPaging="True" OnPageIndexChanging="gvwPVod_PageIndexChanging">
    <EmptyDataTemplate>
    	<table cellspacing="0" rules="all" style="width:100%;border-collapse:collapse;">
		<tr class="headerstyle">
             <th scope="col" style="width:12%;">Package Asset ID</th>
             <th scope="col" style="width:12%;">Title Asset ID</th>
             <th scope="col" style="width:35%;">ENG Title</th>
             <th scope="col" style="width:25%;">TRK Title</th>
             <th scope="col" style="width:7%;">Select</th>
		</tr>
		<tr class="rowstyle">
            <td align="center" colspan="9">
               MAM_No_Matching_Event
            </td>
		</tr>
	</table>
    </EmptyDataTemplate>
    <Columns>
        <asp:TemplateField HeaderText="Package Asset ID" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
            <ItemTemplate>
                <asp:Label ID="lblAssetIdPackage" runat="server"></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Title Asset ID" ItemStyle-HorizontalAlign="left" ItemStyle-Width="12%">
            <ItemTemplate>
                <asp:Label ID="lblAssetIdTitle" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "AssetId_Title") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="ENG Title" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%">
            <ItemTemplate>    
                <asp:LinkButton ID="linkEngTitle" runat="server" 
                PostBackUrl='<%# string.Format("~/PushVodOverDVBSDCInfos.aspx?EventID={0}",DataBinder.Eval(Container.DataItem, "PackageId").ToString()) %>' Text='<%#DataBinder.Eval(Container.DataItem, "Titol").ToString().ToUpper() %>'
                ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="TRK Title" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="35%" >
            <ItemTemplate>    
                <asp:LinkButton ID="linkTrkTitle" runat="server" 
                PostBackUrl='<%# string.Format("~/PushVodOverDVBSDCInfos.aspx?EventID={0}",DataBinder.Eval(Container.DataItem, "PackageId").ToString()) %>' Text='<%#DataBinder.Eval(Container.DataItem, "Turkish_Titol").ToString().ToUpper() %>'
                ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Select" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="7%">
            <ItemTemplate>
                    <asp:LinkButton ID="btnSelectItem" runat="server" ValidationGroup="MKE" Text='<%# GetLocaleStringResource("MAM_Button_Select") %>' CommandName="SelectItem" CommandArgument='<%#DataBinder.Eval(Container.DataItem, "PackageId").ToString() %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>--%>
</asp:GridView>
