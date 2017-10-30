<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemEncapsulatorDetails.aspx.cs" 
Inherits="MTV.MAM.WebApp.Admin.SystemEncapsulatorDetails" Title="EBS Config" %>
<%@ Register Src="~/Admin/Modules/SystemEncapsulatorDetails.ascx" TagName="EncSystemEditEnc" TagPrefix="MSPCONFIG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MSPCONFIG:EncSystemEditEnc ID="ctrlConfigurationHome" runat="server" />
</asp:Content>
