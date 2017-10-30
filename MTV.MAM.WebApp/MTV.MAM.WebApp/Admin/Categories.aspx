<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.Categories" Title="EBS Config" %>
<%@ Register TagPrefix="MEBSConfig" TagName="Categories" Src="~/Admin/Modules/Categories.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
<MEBSConfig:Categories runat="server" ID="ctrlCategories" />
</asp:Content>
