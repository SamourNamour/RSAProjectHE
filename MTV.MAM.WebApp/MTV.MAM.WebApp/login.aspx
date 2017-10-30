<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="MTV.MAM.WebApp.login" Title="MEBS Media Asset Management" %>
<%@ Register Src="~/Modules/login.ascx" TagName="login" TagPrefix="MEBSMAM" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link rel="SHORTCUT ICON" href="BesTV.ico" />
</head>
<body style="background-color: #6c6d70;" runat="server">
    <form id="form1" runat="server">
        <MEBSMAM:login Id="IdLogin" runat="server"></MEBSMAM:login>
    </form>
</body>
</html>
