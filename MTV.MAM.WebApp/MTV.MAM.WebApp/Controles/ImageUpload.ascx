<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ImageUpload.ascx.cs" Inherits="MTV.MAM.WebApp.Controles.ImageUploadControl" %>
<table class="TableImageUpload">
    <tr>
        <td align="left">
            <asp:Image ID="ImgCover" runat="server" CssClass="ImageFile" Width="86px" Height="118px" ImageUrl="~/Common/transparent.gif"  />
        </td>
    </tr>
    <tr>
        <td>
            <asp:FileUpload ID="IdFileUpload" runat="server" Width="250px"  />
        </td>
    </tr>
</table>