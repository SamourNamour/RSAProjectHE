<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SimpleTextBox.ascx.cs" Inherits="MTV.MAM.WebApp.Controles.SimpleTextBox" %>
<asp:TextBox ID="txtValue" runat="server" Width="250px"></asp:TextBox>
<asp:RequiredFieldValidator ID="rfvValue" ControlToValidate="txtValue" Font-Name="verdana"
    Font-Size="9pt" runat="server" Display="None"></asp:RequiredFieldValidator>
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="rfvValueE" TargetControlID="rfvValue"
    HighlightCssClass="validatorCalloutHighlight" />