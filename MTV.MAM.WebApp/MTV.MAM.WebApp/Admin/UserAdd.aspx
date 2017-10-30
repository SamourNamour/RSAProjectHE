<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="UserAdd.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.UserAdd_aspx" %>
<%@ Register Src="~/Admin/Modules/UserAdd.ascx" TagName="UserAdd" TagPrefix="MEBSConfig" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:UserAdd ID="idUserAdd" runat="server"></MEBSConfig:UserAdd>
</asp:Content>
