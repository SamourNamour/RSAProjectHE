<%@ Page Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="AuthorizedVodList.aspx.cs" Inherits="MTV.MAM.WebApp.AuthorizedVodList_aspx" Title="MTV.MAM.WebApp" %>
<%@ Register Src="~/Modules/UC_AuthorizedVodList.ascx" TagName="AuthorizedVodList" TagPrefix="MEBSMAM" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph1" runat="server">
    <MEBSMAM:AuthorizedVodList ID="IdAuthorizedVodList" runat="server" />
</asp:Content>
