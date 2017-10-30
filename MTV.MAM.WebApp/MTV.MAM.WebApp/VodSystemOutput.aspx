<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VodSystemOutput.aspx.cs" Inherits="MTV.MAM.WebApp.VodSystemOutput_aspx" %>
<%@ Register Src="~/Modules/UC_VodSystemOutput.ascx" TagName="VodSystemOutput" TagPrefix="MEBSMAM"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
<MEBSMAM:VodSystemOutput ID="IdVodSystemOutput" runat="server"></MEBSMAM:VodSystemOutput>
</asp:Content>
