<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CategoryDetails.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.CategoryDetails" Title="EBS Config" %>
<%@ Register TagPrefix="MEBSConfig" TagName="CategoryDetails" Src="~/Admin/Modules/CategoryDetails.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
 <MEBSConfig:CategoryDetails runat="server" ID="ctrlCategoryDetails" />
</asp:Content>
