<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemEncapsulatorAdd.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.SystemEncapsulatorAdd" Title="EBS Config" %>

<%@ Register Src="Modules/SystemEncapsulatorAdd.ascx" TagName="EncSystemAddEnc" TagPrefix="MSPCONFIG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MSPCONFIG:EncSystemAddEnc ID="ctrlConfigurationHome" runat="server" />
</asp:Content>
