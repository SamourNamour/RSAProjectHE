<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="VodPrograming.aspx.cs" Inherits="MTV.MAM.WebApp.VodPrograming_aspx" %>
<%@ Register Src="~/Modules/UC_VodPrograming.ascx" TagName="VodPrograming" TagPrefix="MEBSMAM"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSMAM:VodPrograming ID="IdVodPrograming" runat="server" />
</asp:Content>
