<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DCDetails.aspx.cs" Inherits="MTV.MAM.WebApp.DCDetails_aspx" %>
<%@ Register Src="~/Modules/UC_DCDetails.ascx" TagName="DCDetails" TagPrefix="MEBSMAM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSMAM:DCDetails ID="idDCDetails" runat="server" />
</asp:Content>
