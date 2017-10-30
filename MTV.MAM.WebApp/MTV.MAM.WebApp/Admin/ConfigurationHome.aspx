<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="ConfigurationHome.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.ConfigurationHome_aspx" %>
<%@ Register Src="~/Admin/Modules/ConfigurationHome.ascx" TagName="ConfigurationHome" TagPrefix="MEBSConfig"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:ConfigurationHome ID="IdConfigurationHome" runat="server"></MEBSConfig:ConfigurationHome>
</asp:Content>
