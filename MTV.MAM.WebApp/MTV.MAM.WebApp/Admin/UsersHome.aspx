<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UsersHome.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.UsersHome_aspx" %>
<%@ Register Src="~/Admin/Modules/UsersHome.ascx" TagName="UsersHome" TagPrefix="MEBSConfig"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:UsersHome ID="IdUsersHome" runat="server"></MEBSConfig:UsersHome>
</asp:Content>
