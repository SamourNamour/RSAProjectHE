<%@ Page Language="C#" MasterPageFile="~/Main.master" AutoEventWireup="true" CodeBehind="SettingsAdd.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.SettingsAdd" %>
<%@ Register Src="~/Admin/Modules/SettingsAdd.ascx" TagName="SystemSettingsAdd" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:SystemSettingsAdd ID="ctrlSystemSettingsAdd" runat="server" />
</asp:Content>
