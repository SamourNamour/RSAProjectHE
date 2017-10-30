<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="SystemEncapsulator.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.SystemEncapsulator_aspx" Title="EBS Config" %>

<%@ Register Src="~/Admin/Modules/SystemEncapsulator.ascx" TagName="EncSystem" TagPrefix="MSPCONFIG" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MSPCONFIG:EncSystem ID="ctrlConfigurationHome" runat="server" />
</asp:Content>


