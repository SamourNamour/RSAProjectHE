<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="DCScheduleDetails.aspx.cs" Inherits="MTV.MAM.WebApp.DCScheduleDetails_aspx" %>
<%@ Register Src="~/Modules/UC_DCScheduleDetails.ascx" TagName="DCScheduleDetails" TagPrefix="MEBSMAM" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSMAM:DCScheduleDetails ID="IdDCScheduleDetails" runat="server" />
</asp:Content>
