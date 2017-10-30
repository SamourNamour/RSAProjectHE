<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Users.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.Users_aspx" %>
<%@ Register Src="~/Admin/Modules/Users.ascx" TagName="Users" TagPrefix="MEBSConfig"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:Users ID="IdUsers" runat="server"></MEBSConfig:Users>
</asp:Content>
