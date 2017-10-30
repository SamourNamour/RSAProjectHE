<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CatalogHome.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.CatalogHome" Title="EBS Config" %>
<%@ Register TagPrefix="MEBSConfig" TagName="CatalogHome" Src="~/Admin/Modules/CatalogHome.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:CatalogHome runat="server" ID="ctrlCatalogHome" />
</asp:Content>
