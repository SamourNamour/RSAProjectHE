//---- Added by Karima in 19/01/2010 - 13:26:00
window.onload = function () {
    var strCook = document.cookie;
    if (strCook.indexOf("!~") != 0) {
        var intS = strCook.indexOf("!~");
        var intE = strCook.indexOf("~!");
        var strPos = strCook.substring(intS + 2, intE);
        var div = document.getElementById("ctl00_cph1_IdVodPrograming_Panel_cal_content");
        if (div != null) {
            div.scrollTop = strPos;
        }
    }
}
function SetDivPosition() {
    var intY = document.getElementById("ctl00_cph1_IdVodPrograming_Panel_cal_content").scrollTop;
    //document.title = intY;
    document.cookie = "yPos=!~" + intY + "~!";
}

window.scrollBy(100,100);
function foo()
{
    if(grdWithScroll != null) alert(grdWithScroll.scrollTop);
}