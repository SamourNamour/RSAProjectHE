<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="Settings.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.Settings" %>

<%@ Register Src="~/Admin/Modules/Settings.ascx" TagName="SystemSettings" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:SystemSettings ID="ctrlSystemSettings" runat="server" />
</asp:Content>
