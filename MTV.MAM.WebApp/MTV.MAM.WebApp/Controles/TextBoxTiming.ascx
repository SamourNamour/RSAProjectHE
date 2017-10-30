<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TextBoxTiming.ascx.cs" Inherits="MTV.MAM.WebApp.Controles.TextBoxTimingControl" %>
<asp:TextBox ID="txtTiming" runat="server" MaxLength="5" ></asp:TextBox>

<asp:RequiredFieldValidator ID="rfvValue" Font-Names="verdana" Font-Size="9pt" ControlToValidate="txtTiming"
 runat="server" Display="None" ErrorMessage="Timing is required field" ></asp:RequiredFieldValidator>
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="rfvValueE" TargetControlID="rfvValue"
    HighlightCssClass="validatorCalloutHighlight" />
  
  
<asp:RegularExpressionValidator ID="revValue" runat="server" ControlToValidate="txtTiming"
    ValidationExpression="^([0-1][0-9]|[2][0-3]):([0-5][0-9])$" ErrorMessage="Wrong format" Display="None" />     
<ajaxToolkit:ValidatorCalloutExtender runat="Server" ID="revValueE" TargetControlID="revValue"
    HighlightCssClass="validatorCalloutHighlight" />    