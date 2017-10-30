<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="PushVodHome.aspx.cs" Inherits="MTV.MAM.WebApp.PushVodHome_aspx" %>
<%@ Register Src="~/Modules/UC_PushVodHome.ascx" TagName="PushVodHome" TagPrefix="MEBSMAM"  %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSMAM:PushVodHome ID="IdPushVodHome" runat="server"></MEBSMAM:PushVodHome>
</asp:Content>
