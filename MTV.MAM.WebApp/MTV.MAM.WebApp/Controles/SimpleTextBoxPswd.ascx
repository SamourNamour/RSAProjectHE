<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleTextBoxPswd.ascx.cs" Inherits="MTV.MAM.WebApp.Controles.SimpleTextBoxPswd" %>
<asp:TextBox ID="txtValue" runat="server" TextMode="Password"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvValue" ControlToValidate="txtValue" Font-Name="verdana"
    Font-Size="9pt" runat="server" Display="None"></asp:RequiredFieldValidator>
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="rfvValueE" TargetControlID="rfvValue"
    HighlightCssClass="validatorCalloutHighlight" />