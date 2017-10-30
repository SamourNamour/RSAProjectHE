<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RedundancyDetails.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.RedundancyDetails_aspx" %>
<%@ Register Src="~/Admin/Modules/RedundancyDetails.ascx" TagName="RedundancyDetails" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:RedundancyDetails ID="IdRedundancyDetails" runat="server"></MEBSConfig:RedundancyDetails>
</asp:Content>
