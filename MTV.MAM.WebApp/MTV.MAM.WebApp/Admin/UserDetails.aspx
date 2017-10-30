<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserDetails.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.UserDetails_aspx" %>
<%@ Register Src="~/Admin/Modules/UserDetails.ascx" TagName="UserEdit" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:UserEdit ID="ctrlUserDetails" runat="server" />
</asp:Content>
