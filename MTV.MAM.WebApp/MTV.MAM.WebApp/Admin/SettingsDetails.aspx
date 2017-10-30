<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SettingsDetails.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.SettingsDetails" %>
<%@ Register Src="~/Admin/Modules/SettingsDetails.ascx" TagName="SystemSettingsDetails" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:SystemSettingsDetails ID="ctrlSystemSettingsDetails" runat="server" />
</asp:Content>
