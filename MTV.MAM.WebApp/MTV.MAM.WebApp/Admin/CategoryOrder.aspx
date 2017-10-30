<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CategoryOrder.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.CategoryOrder" %>
<%@ Register Src="~/Admin/Modules/CategoryOrder.ascx" TagName="CategoryOrder" TagPrefix="MEBSConfig"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSConfig:CategoryOrder ID="IdCategoryOrder" runat="server" />
</asp:Content>
