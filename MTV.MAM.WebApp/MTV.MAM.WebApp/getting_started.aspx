<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getting_started.aspx.cs" Inherits="MTV.MAM.WebApp.getting_started" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv=Content-Type content="text/html; charset=windows-1252">
<meta name=Generator content="Microsoft Word 14 (filtered)">
<title>Title</title>
<style>
<!--
 /* Font Definitions */
 @font-face
	{font-family:Wingdings;
	panose-1:5 0 0 0 0 0 0 0 0 0;}
@font-face
	{font-family:Wingdings;
	panose-1:5 0 0 0 0 0 0 0 0 0;}
@font-face
	{font-family:Cambria;
	panose-1:2 4 5 3 5 4 6 3 2 4;}
@font-face
	{font-family:Calibri;
	panose-1:2 15 5 2 2 2 4 3 2 4;}
@font-face
	{font-family:Tahoma;
	panose-1:2 11 6 4 3 5 4 4 2 4;}
@font-face
	{font-family:"\30D2\30E9\30AE\30CE\89D2\30B4 Pro W3";}
@font-face
	{font-family:"Lucida Grande";}
@font-face
	{font-family:Consolas;
	panose-1:2 11 6 9 2 2 4 3 2 4;}
 /* Style Definitions */
 p.MsoNormal, li.MsoNormal, div.MsoNormal
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
h1
	{mso-style-link:"Titre 1 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:18.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;}
h2
	{mso-style-link:"Titre 2 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:16.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;}
h3
	{mso-style-link:"Titre 3 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:14.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;
	font-style:italic;}
h4
	{mso-style-link:"Titre 4 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:12.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;}
h5
	{mso-style-link:"Titre 5 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:11.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;}
h6
	{mso-style-link:"Titre 6 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:0cm;
	margin-bottom:.0001pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-weight:normal;}
p.MsoHeading7, li.MsoHeading7, div.MsoHeading7
	{mso-style-link:"Titre 7 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;}
p.MsoHeading8, li.MsoHeading8, div.MsoHeading8
	{mso-style-link:"Titre 8 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;}
p.MsoHeading9, li.MsoHeading9, div.MsoHeading9
	{mso-style-link:"Titre 9 Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;}
p.MsoToc1, li.MsoToc1, div.MsoToc1
	{margin-top:3.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoToc2, li.MsoToc2, div.MsoToc2
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:11.9pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoToc3, li.MsoToc3, div.MsoToc3
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:24.1pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoToc4, li.MsoToc4, div.MsoToc4
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:36.0pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoToc5, li.MsoToc5, div.MsoToc5
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:47.9pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoToc6, li.MsoToc6, div.MsoToc6
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:60.0pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoToc7, li.MsoToc7, div.MsoToc7
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:72.0pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoToc8, li.MsoToc8, div.MsoToc8
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:83.9pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoToc9, li.MsoToc9, div.MsoToc9
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:96.1pt;
	text-align:justify;
	font-size:10.0pt;
	font-family:"Times New Roman","serif";}
p.MsoFootnoteText, li.MsoFootnoteText, div.MsoFootnoteText
	{mso-style-link:"Note de bas de page Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoCommentText, li.MsoCommentText, div.MsoCommentText
	{mso-style-link:"Commentaire Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoHeader, li.MsoHeader, div.MsoHeader
	{mso-style-link:"En-tête Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoFooter, li.MsoFooter, div.MsoFooter
	{mso-style-link:"Pied de page Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoCaption, li.MsoCaption, div.MsoCaption
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:10.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:9.0pt;
	font-family:"Arial","sans-serif";
	color:#666666;
	font-weight:bold;}
span.MsoFootnoteReference
	{vertical-align:super;}
span.MsoEndnoteReference
	{vertical-align:super;}
p.MsoEndnoteText, li.MsoEndnoteText, div.MsoEndnoteText
	{mso-style-link:"Note de fin Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoTitle, li.MsoTitle, div.MsoTitle
	{mso-style-link:"Titre Car";
	margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:center;
	line-height:120%;
	font-size:16.0pt;
	font-family:"Cambria","serif";
	font-weight:bold;}
p.MsoBodyText, li.MsoBodyText, div.MsoBodyText
	{mso-style-link:"Corps de texte Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:6.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoSubtitle, li.MsoSubtitle, div.MsoSubtitle
	{mso-style-link:"Sous-titre Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:center;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Cambria","serif";}
a:link, span.MsoHyperlink
	{font-family:"Calibri","sans-serif";
	color:blue;
	text-decoration:underline;}
a:visited, span.MsoHyperlinkFollowed
	{color:purple;
	text-decoration:underline;}
em
	{font-family:"Calibri","sans-serif";
	font-weight:bold;}
p.MsoDocumentMap, li.MsoDocumentMap, div.MsoDocumentMap
	{mso-style-link:"Explorateur de documents Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:8.0pt;
	font-family:"Tahoma","sans-serif";}
p.MsoPlainText, li.MsoPlainText, div.MsoPlainText
	{mso-style-link:"Texte brut Car";
	margin:0cm;
	margin-bottom:.0001pt;
	font-size:10.5pt;
	font-family:Consolas;}
p.MsoCommentSubject, li.MsoCommentSubject, div.MsoCommentSubject
	{mso-style-link:"Objet du commentaire Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	font-weight:bold;}
p.MsoAcetate, li.MsoAcetate, div.MsoAcetate
	{mso-style-link:"Texte de bulles Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:8.0pt;
	font-family:"Tahoma","sans-serif";}
p.MsoNoSpacing, li.MsoNoSpacing, div.MsoNoSpacing
	{mso-style-link:"Sans interligne Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoListParagraph, li.MsoListParagraph, div.MsoListParagraph
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:36.0pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoListParagraphCxSpFirst, li.MsoListParagraphCxSpFirst, div.MsoListParagraphCxSpFirst
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:36.0pt;
	margin-bottom:.0001pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoListParagraphCxSpMiddle, li.MsoListParagraphCxSpMiddle, div.MsoListParagraphCxSpMiddle
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:0cm;
	margin-left:36.0pt;
	margin-bottom:.0001pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoListParagraphCxSpLast, li.MsoListParagraphCxSpLast, div.MsoListParagraphCxSpLast
	{margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:36.0pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
p.MsoQuote, li.MsoQuote, div.MsoQuote
	{mso-style-link:"Citation Car";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	font-style:italic;}
p.MsoIntenseQuote, li.MsoIntenseQuote, div.MsoIntenseQuote
	{mso-style-link:"Citation intense Car";
	margin-top:0cm;
	margin-right:36.0pt;
	margin-bottom:3.0pt;
	margin-left:36.0pt;
	text-align:justify;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";
	font-weight:bold;
	font-style:italic;}
span.MsoSubtleEmphasis
	{color:#7C7C7C;
	font-style:italic;}
span.MsoIntenseEmphasis
	{font-weight:bold;
	font-style:italic;
	text-decoration:underline;}
span.MsoSubtleReference
	{text-decoration:underline;}
span.MsoIntenseReference
	{font-weight:bold;
	text-decoration:underline;}
span.MsoBookTitle
	{font-family:"Cambria","serif";
	font-weight:bold;
	font-style:italic;}
p.MsoTocHeading, li.MsoTocHeading, div.MsoTocHeading
	{margin-top:12.0pt;
	margin-right:0cm;
	margin-bottom:3.0pt;
	margin-left:0cm;
	text-align:justify;
	line-height:120%;
	page-break-after:avoid;
	font-size:18.0pt;
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre1Car
	{mso-style-name:"Titre 1 Car";
	mso-style-link:"Titre 1";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre2Car
	{mso-style-name:"Titre 2 Car";
	mso-style-link:"Titre 2";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre3Car
	{mso-style-name:"Titre 3 Car";
	mso-style-link:"Titre 3";
	font-family:"Arial","sans-serif";
	color:#C4161C;
	font-style:italic;}
span.Titre4Car
	{mso-style-name:"Titre 4 Car";
	mso-style-link:"Titre 4";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre5Car
	{mso-style-name:"Titre 5 Car";
	mso-style-link:"Titre 5";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre6Car
	{mso-style-name:"Titre 6 Car";
	mso-style-link:"Titre 6";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre7Car
	{mso-style-name:"Titre 7 Car";
	mso-style-link:"Titre 7";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre8Car
	{mso-style-name:"Titre 8 Car";
	mso-style-link:"Titre 8";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.Titre9Car
	{mso-style-name:"Titre 9 Car";
	mso-style-link:"Titre 9";
	font-family:"Arial","sans-serif";
	color:#C4161C;}
span.TitreCar
	{mso-style-name:"Titre Car";
	mso-style-link:Titre;
	font-family:"Cambria","serif";
	font-weight:bold;}
span.Sous-titreCar
	{mso-style-name:"Sous-titre Car";
	mso-style-link:Sous-titre;
	font-family:"Cambria","serif";}
span.SansinterligneCar
	{mso-style-name:"Sans interligne Car";
	mso-style-link:"Sans interligne";
	font-family:"Times New Roman","serif";}
span.CitationCar
	{mso-style-name:"Citation Car";
	mso-style-link:Citation;
	font-family:"Times New Roman","serif";
	font-style:italic;}
span.CitationintenseCar
	{mso-style-name:"Citation intense Car";
	mso-style-link:"Citation intense";
	font-family:"Times New Roman","serif";
	font-weight:bold;
	font-style:italic;}
span.CommentaireCar
	{mso-style-name:"Commentaire Car";
	mso-style-link:Commentaire;
	font-family:"Times New Roman","serif";}
span.ObjetducommentaireCar
	{mso-style-name:"Objet du commentaire Car";
	mso-style-link:"Objet du commentaire";
	font-family:"Times New Roman","serif";
	font-weight:bold;}
span.TextedebullesCar
	{mso-style-name:"Texte de bulles Car";
	mso-style-link:"Texte de bulles";
	font-family:"Tahoma","sans-serif";}
span.CorpsdetexteCar
	{mso-style-name:"Corps de texte Car";
	mso-style-link:"Corps de texte";
	font-family:"Times New Roman","serif";}
span.NotedefinCar
	{mso-style-name:"Note de fin Car";
	mso-style-link:"Note de fin";
	font-family:"Times New Roman","serif";}
span.NotedebasdepageCar
	{mso-style-name:"Note de bas de page Car";
	mso-style-link:"Note de bas de page";
	font-family:"Times New Roman","serif";}
span.ExplorateurdedocumentsCar
	{mso-style-name:"Explorateur de documents Car";
	mso-style-link:"Explorateur de documents";
	font-family:"Tahoma","sans-serif";}
span.PieddepageCar
	{mso-style-name:"Pied de page Car";
	mso-style-link:"Pied de page";
	font-family:"Times New Roman","serif";}
p.Code, li.Code, div.Code
	{mso-style-name:Code;
	margin:0cm;
	margin-bottom:.0001pt;
	text-align:justify;
	line-height:115%;
	font-size:9.0pt;
	font-family:"Courier New";}
p.Figuretitle, li.Figuretitle, div.Figuretitle
	{mso-style-name:"Figure title";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:12.0pt;
	margin-left:0cm;
	text-align:center;
	line-height:120%;
	font-size:10.0pt;
	font-family:"Arial","sans-serif";}
span.En-tteCar
	{mso-style-name:"En-tête Car";
	mso-style-link:En-tête;
	font-family:"Times New Roman","serif";}
span.Codecharacter
	{mso-style-name:"Code \(character\)";
	font-family:"Courier New";
	color:windowtext;}
p.Prrafodelista1, li.Prrafodelista1, div.Prrafodelista1
	{mso-style-name:"Párrafo de lista1";
	margin-top:0cm;
	margin-right:0cm;
	margin-bottom:10.0pt;
	margin-left:36.0pt;
	line-height:115%;
	font-size:11.0pt;
	font-family:"Calibri","sans-serif";
	color:black;}
p.Default, li.Default, div.Default
	{mso-style-name:Default;
	margin:0cm;
	margin-bottom:.0001pt;
	font-size:12.0pt;
	font-family:"Times New Roman","serif";
	color:black;}
span.TextebrutCar
	{mso-style-name:"Texte brut Car";
	mso-style-link:"Texte brut";
	font-family:Consolas;}
.MsoChpDefault
	{font-family:"Calibri","sans-serif";}
.MsoPapDefault
	{margin-bottom:10.0pt;
	line-height:115%;}
 /* Page Definitions */
 @page WordSection1
	{size:595.3pt 841.9pt;
	margin:99.25pt 2.0cm 3.0cm 2.0cm;}
div.WordSection1
	{page:WordSection1;}
 /* List Definitions */
 ol
	{margin-bottom:0cm;}
ul
	{margin-bottom:0cm;}
-->
</style>
</head>
<body link=blue vlink=purple>
    <form id="form1" runat="server">
<div class=WordSection1>

<p class=MsoNormal><img width=244 height=73 id="Imagen 4"
src="getting_started_fichiers/image001.jpg"></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<table class=MsoTableGrid border=0 cellspacing=0 cellpadding=0
 style='border-collapse:collapse;border:none'>
 <tr style='height:118.4pt'>
  <td width=633 valign=bottom style='width:474.7pt;padding:0cm 5.4pt 0cm 5.4pt;
  height:118.4pt'>
  <p class=MsoNormal align=left style='margin-right:123.7pt;text-align:left'><span
  lang=EN-US style='font-size:32.0pt;line-height:120%;color:#C4161C'>Subscription
  Push VOD Service for Siyaya </span></p>
  <p class=MsoNormal style='margin-right:123.7pt'><span lang=EN-US
  style='font-size:32.0pt;line-height:120%;color:#C4161C'>&nbsp;</span></p>
  </td>
 </tr>
</table>

<span lang=EN-US style='font-size:10.0pt;line-height:115%;font-family:"Arial","sans-serif"'><br
clear=all style='page-break-before:always'>
</span>

<p class=MsoNormal style='margin-bottom:10.0pt;line-height:115%'><b><span
lang=EN-US style='font-size:18.0pt;line-height:115%;font-family:"Cambria","serif"'>&nbsp;</span></b></p>

<p class=MsoTocHeading><span lang=EN-US>Contents</span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199252"><b><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>D</span></b><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>ocument control
sheet</span><span style='color:windowtext;display:none;text-decoration:none'> </span><span
style='color:windowtext;display:none;text-decoration:none'>3</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199253"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>1.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Executive Summary</span><span style='color:windowtext;
display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>5</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199254"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>2.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Motive Content Express™ - Brief Overview</span><span
style='color:windowtext;display:none;text-decoration:none'>.. </span><span
style='color:windowtext;display:none;text-decoration:none'>5</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199255"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>4.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Push VOD Workflow</span><span style='color:windowtext;
display:none;text-decoration:none'>.. </span><span
style='color:windowtext;display:none;text-decoration:none'>10</span></a></span></p>

<p class=MsoToc2><span lang=EN-US><a href="#_Toc386199256"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>4.1.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>        </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Content Preparation – Files Upload and Metadata
Integrity Check</span><span style='color:windowtext;display:none;text-decoration:
none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>10</span></a></span></p>

<p class=MsoToc2><span lang=EN-US><a href="#_Toc386199257"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>4.2.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>        </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Content Scheduling</span><span style='color:windowtext;
display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>11</span></a></span></p>

<p class=MsoToc2><span lang=EN-US><a href="#_Toc386199258"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>4.3.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>        </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Content Sending</span><span style='color:windowtext;
display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>12</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199259"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>5.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Remote Connectivity</span><span style='color:windowtext;
display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>13</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199260"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>6.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>MUX Configuration and CAS</span><span style='color:
windowtext;display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>14</span></a></span></p>

<p class=MsoToc1><span lang=EN-US><a href="#_Toc386199261"><span
style='font-size:12.0pt;font-family:"Calibri","sans-serif"'>7.</span><span
style='font-size:11.0pt;font-family:"Calibri","sans-serif";color:windowtext;
text-decoration:none'>       </span><span style='font-size:12.0pt;font-family:
"Calibri","sans-serif"'>Redundancy</span><span style='color:windowtext;
display:none;text-decoration:none'>. </span><span
style='color:windowtext;display:none;text-decoration:none'>15</span></a></span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<span lang=EN-US style='font-size:10.0pt;line-height:120%;font-family:"Arial","sans-serif"'><br
clear=all style='page-break-before:always'>
</span>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc382234794"></a><a
name="_Toc365890698"></a><a name="_Toc386199253"><span lang=EN-US>1.<span
style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US>Executive Summary</span></a></h1>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The scope of this document is to give an overview of
the Motive Content Express Solution and describes how it is applied for enable
the Push VOD Service for SIyaya.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Moreover are detailed all the aspects related to the
integration between Siyaya, Sentech and Motive.</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;&nbsp;</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc386199254"><span
lang=EN-US>2.<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US>Motive Content Express™ - Brief Overview</span></a></h1>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The Motive’s Content Express™ technology enables
delivery of non-linear Television services from the broadcasters’ premises
along with normal linear programming. Viewers receive these non-linear TV
services on their enabled devices (STBs, Tablet or Televisions). </span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Moreover thanks to Ad’s Platform Motive allows its
customers to open a door to the Advertisement Market and raise new revenues
associated to the VOD Contents.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Motive’s Content Express™ is CA (Conditional Access)
&amp; DRM (Digital Rights Management) system agnostic and has been integrated
with both card-less and card-based industry standard CA &amp; DRM systems to
ensure security &amp; integrity of content on the STB hard disk.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The Content Express™ Platform is based on the Motive
Enhanced Broadcast System (MEBS) that allows the two main functionalities Autorecording
and Datacasting.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-GB>Autorecording</span></h5>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The Autorecording service allows recording of content
on the STB hard disk from live TV – both scrambled and Free-To-Air (FTA) by
sending start / stop recording commands without using expensive bandwidth to
re-transmit live content.&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Contents to be recorded are selected by a TV operator
based on the EPG of the single channels.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-GB>Datacasting</span></h5>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The Datacasting service allows a one-way transmission
of content via DVB-T/S. By using the flexibility of available bandwidth,
different transmission bit-rates may be used, during the day and night to store
content on the STB hard disk.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Contents are sent over a single PID (channel) encoded
in private sections in the TS and then are encrypted using the CAS as data PID.
</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><a name="_Toc384053623"><span lang=EN-GB>System Dataflow</span></a></h5>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>A generic representation of the Motive Enhanced Broadcast
System (MEBS) platform is reported here below:</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal align=center style='margin-bottom:0cm;margin-bottom:.0001pt;
text-align:center;line-height:115%'><span style='line-height:115%;font-family:
"Calibri","sans-serif"'><img width=416 height=189 id="Immagine 2"
src="getting_started_fichiers/image002.jpg" alt=ssssssssssssss.jpg></span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>In particular the MEBS Platform needs to communicate
with:</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'> </span></p>

<p class=MsoListParagraph style='margin-bottom:0cm;margin-bottom:.0001pt;
text-indent:-18.0pt;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;
line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><b><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>EPG Server</span></b></p>

<p class=MsoNormal style='margin-top:0cm;margin-right:0cm;margin-bottom:0cm;
margin-left:35.45pt;margin-bottom:.0001pt;line-height:115%'><span lang=EN-GB
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>We
need to Ingest EPG Data in order to prepare the autorecordings and generate the
scheduling data that will be sent to the test Set Top boxes.</span></p>

<p class=MsoListParagraph style='margin-bottom:0cm;margin-bottom:.0001pt;
text-indent:-18.0pt;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;
line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><b><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Automation System</span></b></p>

<p class=MsoNormal style='margin-top:0cm;margin-right:0cm;margin-bottom:0cm;
margin-left:35.45pt;margin-bottom:.0001pt;line-height:115%'><span lang=EN-GB
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Receiving
Start and Stop Triggers from Automation System is needed in order to Start and
Stop the recordings </span></p>

<p class=MsoListParagraph style='margin-bottom:0cm;margin-bottom:.0001pt;
text-indent:-18.0pt;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;
line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><b><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>File Transfer</span></b></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;text-indent:
35.45pt;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>Set up a transfer mechanism in order
to upload contents to be Datacast in our platform</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;text-indent:
35.45pt;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The outputs of the MEBS consist of two PID streams (IP
or ASI output)to be muxed together with the DVB signal.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-GB style='font-size:11.0pt;line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>PID 85: signalization</span></p>

<p class=MsoListParagraphCxSpLast style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-GB style='font-size:11.0pt;line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>PID 86: Push VOD data</span></p>

<p class=MsoNormal align=left style='margin-bottom:0cm;margin-bottom:.0001pt;
text-align:left;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal align=left style='margin-bottom:0cm;margin-bottom:.0001pt;
text-align:left;line-height:115%'><span lang=EN-GB style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraph align=left style='margin-top:0cm;margin-right:0cm;
margin-bottom:0cm;margin-left:18.0pt;margin-bottom:.0001pt;text-align:left;
text-indent:-18.0pt;line-height:115%'><span lang=EN-US style='font-size:18.0pt;
line-height:115%;color:#C4161C'>3.<span style='font:7.0pt "Times New Roman"'>&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:18.0pt;
line-height:115%;color:#C4161C'>Motive Content Express™ - Subscription Push VOD
</span></p>

<p class=MsoNormal><span lang=EN-US style='line-height:120%;color:#C4161C'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The service to be implemented in a first phase of the
project is the <b>Subscription Push VOD</b>.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>It is a quite straightforward service solution because
there is no need to integrate our platform with EPG server and Automation
System as explained in the previous paragraph. The operator just need to</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:0cm;margin-bottom:.0001pt;
text-indent:-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;
line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Upload files in the FTP
server</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:0cm;margin-bottom:
.0001pt;text-indent:-18.0pt;line-height:115%'><span lang=EN-US
style='font-size:11.0pt;line-height:115%'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Schedule Contents using an
ad hoc Web Application</span></p>

<p class=MsoListParagraphCxSpLast style='margin-bottom:0cm;margin-bottom:.0001pt;
line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>Then the platform is responsible for the sending of the
contents.</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>The platform to be installed in Sentech Premises in
order to enable the Subscription Push VOD service for Siyaya is represented in
the following:</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:0cm;margin-bottom:.0001pt;
line-height:115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpLast align=center style='margin-bottom:0cm;
margin-bottom:.0001pt;text-align:center;line-height:115%'><span
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'><img
width=437 height=260 id="11 Imagen" src="getting_started_fichiers/image003.jpg"
alt="Siyaya WF 000.jpg"></span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>It is composed by one main Server responsible for the
configuration and management of the service and two encapsulators that are the
connector with the DVB Broadcast Network. The main services/components of the
Platform are </span></p>

<p class=MsoNormal style='margin-bottom:0cm;margin-bottom:.0001pt;line-height:
115%'><span lang=EN-GB style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><a name="_Toc276575657"></a><span lang=EN-US>MAM Web Application:</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The MAM Web App
allows the operator to </span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Schedule Contents</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Remove Contents</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Edit Metadata</span></p>

<p class=MsoListParagraphCxSpLast style='margin-bottom:10.0pt;text-indent:-18.0pt;
line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Update Metadata</span></p>

<h5><span lang=EN-US>Admin &amp; Config Web Application</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Through this Web
Application the operator can configure several operational parameters of the
platform such as</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>IP Addresses </span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>FTP Folders Detail</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Timing Parameters</span></p>

<p class=MsoListParagraphCxSpLast style='margin-bottom:10.0pt;text-indent:-18.0pt;
line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Encapsulator bitrate.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The configuration
of the headend is then stored in the central db. </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>FTP Server</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>This is the entry
point of the platform where the operator uploads the files needed for schedule
contents. Here there are dedicated folders for</span></p>

<p class=MsoListParagraphCxSpFirst align=left style='margin-bottom:10.0pt;
text-align:left;text-indent:-18.0pt;line-height:115%'><span lang=EN-US
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>-<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Metadata (MPEG7)</span></p>

<p class=MsoListParagraphCxSpMiddle align=left style='margin-bottom:10.0pt;
text-align:left;text-indent:-18.0pt;line-height:115%'><span lang=EN-US
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>-<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Media Files (TS File)</span></p>

<p class=MsoListParagraphCxSpMiddle align=left style='margin-bottom:10.0pt;
text-align:left;text-indent:-18.0pt;line-height:115%'><span lang=EN-US
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>-<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Cover (Jpeg files)</span></p>

<p class=MsoListParagraphCxSpLast align=left style='margin-bottom:10.0pt;
text-align:left;line-height:115%'><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>Scheduler</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>This service is the
core of the MEBS platform. It is the metronome being in charge of triggering </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Moreover it is </span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Process the ts file  </span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Generate and Send Commands</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Send Covers</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Send Content</span></p>

<p class=MsoListParagraphCxSpLast style='margin-bottom:10.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>Pusher</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The main task of
the Pusher is to listen the database and sent notification to the Scheduler. At
the moment the Pusher notifies the Scheduler the following:</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-bottom:10.0pt;text-indent:
-18.0pt;line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:
115%;font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Removed</span></p>

<p class=MsoListParagraphCxSpLast style='margin-bottom:10.0pt;text-indent:-18.0pt;
line-height:115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Locked</span></p>

<p class=MsoNormal style='line-height:115%'><b><u><span lang=EN-US
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Note</span></u></b><b><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>
</span></b><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>It’s possible to configure the receivers
list of the Pusher in order to forward the notification to several receivers.
This is very useful for monitoring activities.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>Database</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Database where are
stored the system configuration and all the information related to the
Contents.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>Encapsulators</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The Encapsulators
are the connector of the MEBS with the Operator’s MUX.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc386199255"><span
lang=EN-US>4.<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US>Push VOD Workflow</span></a></h1>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>In this section is
described the Workflow for Push VOD split in the three main phases:</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='line-height:
115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>-<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Preparation</span></p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-18.0pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Schedulation</span></p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-18.0pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif"'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'>Content Sending</span></p>

<p class=MsoListParagraphCxSpLast style='margin-left:18.0pt'><span lang=EN-US>&nbsp;</span></p>

<h2 style='margin-left:39.6pt;text-indent:-21.6pt'><a name="_Toc386199256"><span
lang=EN-US>4.1.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US>Content Preparation – Files
Upload and Metadata Integrity Check</span></a></h2>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>1.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The operator upload new content file (TS
File) and related metadata (MPEG7) in the dedicate FTP Folder</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>2.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The Scheduler pick up the TS file and
generate the redundant version of it</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>3.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The Scheduler pick up the MPEG7 file does
an integrity check and store it into the db</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>4.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>At the end of 2 and 3 a xml status file is
generated</span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>5.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>If status is OK the uploaded content
appears in the MAM Web App and it is ready to be edited/scheduled </span></p>

<p class=MsoNormal style='margin-bottom:10.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal align=center style='text-align:center'><img width=462
height=260 id="0 Imagen" src="getting_started_fichiers/image004.jpg"
alt="Siyaya WF 001.jpg"></p>

<p class=MsoNormal align=center style='text-align:center'><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<h2 style='margin-left:39.6pt;text-indent:-21.6pt'><a name="_Toc386199257"><span
lang=EN-US>4.2.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US>Content Scheduling</span></a></h2>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>6.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The operator accessing into the MAM select
the content and associate a cover and a time window for the sending</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>7.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The content is flagged as “scheduled” into
the database</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>8.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The Pusher that is a listener of the
changes of database sends a notification to the Scheduler: New content
scheduled</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>9.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US style='font-size:11.0pt;line-height:115%;
font-family:"Calibri","sans-serif"'>The Scheduler generate the DC Command and 
update the “Incoming Content List” inserting the new contents information (MPEG
7 and DC Command)</span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>10.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The
updated “Incoming Content List” is sent out from the HE carried in PID 85</span></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal align=center style='text-align:center'><img width=462
height=280 id="1 Imagen" src="getting_started_fichiers/image005.jpg"
alt="Siyaya WF 002.jpg"></p>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<h2 style='margin-left:39.6pt;text-indent:-21.6pt'><a name="_Toc386199258"><span
lang=EN-US>4.3.<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US>Content Sending</span></a></h2>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>11.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'> <span
style='background:lightgrey'>XX minutes</span> before the Start Time content is
flagged as “Locked” into the database the Pusher sends a notification to the
Scheduler</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>12.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The
Scheduler generate the DC Command and MPEG 7 and sends them to the Encapsulator</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>13.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The
Encapsulator starts to download the Redundant TS File from the FTP Folder </span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>14.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
background:lightgrey'>YY minutes</span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'> before the Start Time the
DC Command is sent out from the Encapsulator</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>15.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
background:lightgrey'>ZZ minutes</span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif"'> before the Start Time the
Scheduler Picks Up the cover from the FTP Folder and send it to the
Encapsulator</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>16.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The
Encapsulator sends out the cover to the STB</span></p>

<p class=MsoListParagraphCxSpMiddle style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>17.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>At
the Start Time the Encapsulator starts to stream the TS File</span></p>

<p class=MsoListParagraphCxSpLast style='margin-top:0cm;margin-right:0cm;
margin-bottom:10.0pt;margin-left:18.0pt;text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>18.<span
style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp; </span></span><span dir=LTR></span><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>At
the Start Time the Encapsulator starts to stream the MPEG 7 file </span></p>

<p class=MsoNormal style='margin-bottom:10.0pt;line-height:115%'><span
lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal align=center style='text-align:center'><img width=461
height=260 id="2 Imagen" src="getting_started_fichiers/image006.jpg"
alt="Siyaya WF 003.jpg"></p>

<h1><span lang=EN-US>&nbsp;</span></h1>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc386199259"><span
lang=EN-US>5.<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US>Remote Connectivity</span></a></h1>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<h5><span lang=EN-US>Motive Requirements</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>For Monitoring and
Management purpose, Motive Support Team needs to have <span style='color:black'>full
access to the Platform (all servers and all services) </span>from everywhere.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Moreover it’s
required that the servers are able to send real time mail notifications to Motive
Support Team in case of fault/issue/warning. </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<h5><span lang=EN-US>Siyaya Requirements</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>Siyaya
as operator of the Push VOD platform needs to connect to the Motive Headend
located in Sentech Premises to perform the following tasks:</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Upload files
into the FTP server</span></p>

<p class=MsoListParagraphCxSpLast style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Access to the
MAM and Admin&amp;Config Web Application</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<h5><span lang=EN-US>Proposed Solution</span></h5>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>The remote
connection has to be secure and good enough to guarantee a good level of
service.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>For these reasons,
the use of a dedicated <b>VPN is mandatory </b>creating different account to
assign to Siyaya and Motive Teams.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoPlainText style='text-align:justify;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Regarding
the mail notification is required that the servers have connectivity to a local
Mail Server or in the worst case scenario the SMTP port enabled to be able to
use a Motive mail account (less desirable for security purposes).</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US>&nbsp;</span></p>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc386199260"><span
lang=EN-US>6.<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US>MUX Configuration and CAS</span></a></h1>

<p class=MsoListParagraph style='margin-left:18.0pt;line-height:115%'><span
lang=EN-US style='font-size:18.0pt;line-height:115%;color:#C4161C'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>The
outputs of the Motive EBS Platform are two PID Streams:</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>PID 85
responsible for signaling</span></p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-18.0pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif";color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>PID 86
responsible for Push VOD</span></p>

<p class=MsoListParagraphCxSpLast style='line-height:115%'><span lang=EN-US
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>The PID
85 will be broadcasted in clear and has to be unreferenced on the MUX.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>It has
to be broadcasted in all the Freevision Transponders in order to let the STB
receive Motive Signalization once tuned on whatever Freevision Service.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>The
bandwidth to be reserved on the MUX is <span style='background:lightgrey'>100kbps
constant</span>.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>The PID
86 will be treated as a linear service with just one PID associated to it. PID
86 will be encrypt real time using Nagra CAS. For PID 86 is needed:</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoListParagraphCxSpFirst style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Create and ad
hoc Service in the MUX identified by a unique DVB Triplet (Service ID,
Transport Stream ID, Original Network ID) </span></p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-18.0pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif";color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Associate to
this Service the only PID 86</span></p>

<p class=MsoListParagraphCxSpMiddle style='text-indent:-18.0pt;line-height:
115%'><span lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:
"Calibri","sans-serif";color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Reserve on the
MUX the bandwidth defined for the Push VOD Service (<u><span style='background:
lightgrey'>To be defined</span></u>)</span></p>

<p class=MsoListParagraphCxSpLast style='text-indent:-18.0pt;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>-<span style='font:7.0pt "Times New Roman"'>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
</span></span><span dir=LTR></span><span lang=EN-US style='font-size:11.0pt;
line-height:115%;font-family:"Calibri","sans-serif";color:black'>Encrypt it
using the Nagra CAS (as a linear service)</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>PID 86 will
be broadcasted on one specific MUX (<span style='background:lightgrey'>to be
defined</span>) </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>Assuming
that the MUX is responsible for the Encryption we can resume as follow: </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal align=center style='text-align:center;line-height:115%'><span
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'><img width=477 height=208 id="7 Imagen"
src="getting_started_fichiers/image007.jpg" alt="MEBS MUX.jpg"></span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>In this
way, all the Siyaya STBs will receive the PIDs 85 and 86 but only the ones
having an active subscription to the Push VOD service will be able to decrypt
the Push VOD stream and store the VOD contents on the storage device. </span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<h1 style='margin-left:18.0pt;text-indent:-18.0pt'><a name="_Toc386199261"><span
lang=EN-US>7.<span style='font:7.0pt "Times New Roman"'>&nbsp; </span></span><span
dir=LTR></span><span lang=EN-US>Redundancy</span></a></h1>

<p class=MsoNormal><span lang=EN-US>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>Motive
Push VOD platform is deployed in a 1+1 full redundant configuration. Every
machine has its backup.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>A
Monitoring Software checks continuously the status of the single services of
the platform and it is able to perform list of action in order to guarantee the
continuity of the service.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>Motive
Platform provides 2+2 PID streams as output so we need to define how to manage
the connection to the MUX.</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>&nbsp;</span></p>

<p class=MsoNormal style='line-height:115%'><span lang=EN-US style='font-size:
11.0pt;line-height:115%;font-family:"Calibri","sans-serif";color:black'>We
could think to insert an ad hoc device between the Motive Platform and the MUX
or send all four PID streams to the MUX and leave the MUX manage the redundancy
<span style='background:lightgrey'>(VERIFY WITH SENTECH IF POSSIBLE)</span></span></p>

<p class=MsoNormal align=center style='text-align:center;line-height:115%'><span
lang=EN-US style='font-size:18.0pt;line-height:115%;color:#C4161C'>&nbsp;</span></p>

<p class=MsoNormal align=left style='text-align:left;line-height:115%'><u><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif"'>Option
1</span></u></p>

<p class=MsoNormal align=center style='text-align:center;line-height:115%'><span
style='font-size:18.0pt;line-height:115%;color:#C4161C'><img width=594
height=382 id="9 Imagen" src="getting_started_fichiers/image008.jpg"
alt="SIY Redundancy.jpg"></span></p>

<p class=MsoNormal align=left style='text-align:left;line-height:115%'><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>&nbsp;</span></p>

<p class=MsoNormal align=left style='text-align:left;line-height:115%'><u><span
lang=EN-US style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'>Option 2</span></u></p>

<p class=MsoNormal align=center style='text-align:center;line-height:115%'><span
style='font-size:11.0pt;line-height:115%;font-family:"Calibri","sans-serif";
color:black'><img width=594 height=382 id="10 Imagen"
src="getting_started_fichiers/image009.jpg" alt="SIY Redundancy2.jpg"></span></p>

</div>
    </form>
</body>
</html>
