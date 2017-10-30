function ValidateListBox(source, args) {
    args.IsValid = document.getElementById(sender.controltovalidate).options.length > 0;
}

function LoadIngestaInformation(layer) {

    resume = document.getElementById("text_charts");
    resume.innerHTML = '<div class="loading_chart"></div>';
    sendByAjax("IngestaInformation.aspx", "id=0", "post", resume, "layer");

}