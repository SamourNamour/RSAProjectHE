<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IngestaInformation.aspx.cs" Inherits="MTV.MAM.WebApp.IngestaInformation_aspx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
</head>
<div id="wizard">
        <center>
        <b><label id="lblSelectedIngesta" runat="server">SELECTION : </label>
        <label id="lblTitleCodePackage" runat="server" class="sel_chart" ></label></b>
        </center>
        <fieldset class="resume_filter">
        <legend>INFORMATION</legend>
            <p>
                <strong>Title : </strong><br />
                <label id="lblTitle" runat="server" >NONE</label>
            </p>
           <p>
            <strong>Content ID : </strong><br/>
             <label id="lblContentID" runat="server" >NONE</label>
            </p>
            <p>
            <strong>Start Time : </strong><br/>
             <label id="lblStartTime" runat="server">NONE</label>
            </p>
            <p>
            <strong>Stop Time :</strong><br/>
            <label id="lblStopTime" runat="server">NONE</label>
            </p>
            <p>
            <strong>Movie Duration :</strong><br/>
             <label id="lblDurationMovie" runat="server" >NONE</label>
            </p>
            <p>
            <strong>Movie Size :</strong><br/>
             <label id="lblMovieSize" runat="server" >NONE</label>
            </p>
            <p>

            <strong>Event Status :</strong><br/>
            <strong><label id="lblStatusResult" runat="server" >NONE</label></strong>
            </p>
            <%-- <p>
            <strong><%= GetLocaleStringResource("MAM_Label_BonusSize")%> </strong><br/>
             <label id="lblBonusSize" runat="server" ></label>
            </p>
            <p>
            <strong><%= GetLocaleStringResource("MAM_Label_Advertisement")%> </strong><br/>
             <label id="lblAdv" runat="server" ></label>
            </p>--%>
        </fieldset>
        </div>
 </html>
