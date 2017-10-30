<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="CategoryAdd.aspx.cs" Inherits="MTV.MAM.WebApp.Admin.CategoryAdd" Title="EBS Config" ValidateRequest="false" %>

<%@ Register TagPrefix="MEBSConfig" TagName="CategoryAdd" Src="~/Admin/Modules/CategoryAdd.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
<MEBSConfig:CategoryAdd runat="server" ID="ctrlCategoryAdd" />
</asp:Content>
