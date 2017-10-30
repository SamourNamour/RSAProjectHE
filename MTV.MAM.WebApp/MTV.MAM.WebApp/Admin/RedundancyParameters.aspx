<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="RedundancyParameters.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.RedundancyParameters_aspx" %>
<%@ Register Src="~/Admin/Modules/RedundancyParameters.ascx" TagName="RedundancyParameters" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:RedundancyParameters ID="ctrlRedundancyParameters" runat="server" />
</asp:Content>
