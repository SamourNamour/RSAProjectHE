/**
*  Crea un nuevo objeto Ajax
*/
function newAjax() {
    var xmlhttp = false;
    try {
        xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
        try {
            xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
        } catch (E) {
            xmlhttp = false;
        }
    }

    if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
        xmlhttp = new XMLHttpRequest();
    }
    return xmlhttp;
}


/**
* Envia a la url por el method los valores de values y la respuesta la inserta en target
*/
function sendByAjax(url, values, method, target, type, post_callback, loading_data, callback) {
    //
    var ajax = newAjax();

    //alert(ajax + ":" + url);

    if (method.toUpperCase() == 'GET')
        ajax.open("GET", url + values, true);
    else
        ajax.open("POST", url, true);


    ajax.onreadystatechange = function () {
        //cargando
        /*if(ajax.readyState == 1)
        {
        if(target!="" && loading_data!="")
        target.innerHTML = loading_data;
				
        }*/

        //callback
        if (ajax.readyState == 4) {

            switch (type) {
                case "input":
                    target.value = ajax.responseText;
                    break;
                case "layer":

                    target.innerHTML = ajax.responseText;

                    break;
                case "select":
                    var response = unescape(ajax.responseText);
                    response = response.replace(/\+/gi, " ");
                    //alert(response);
                    buildOptionsFromHTML(target, response);
                    break;
                case "callback":
                    try {

                        callback(ajax.responseText);
                    } catch (err) { }
                    break;

            }
            try {
                post_callback(ajax.responseText);
            } catch (err) { }
        }
    }

    if (method.toUpperCase() == 'GET')
        ajax.send(null);

    else {
        ajax.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        ajax.send(values);
    }
}

/**
* env�a un evento generado a una url via ajax y el resultado es depositado en el value de target
*/
function send2input(target, url, array_input) {

    var values = "";
    for (var i = 0; i < array_input.length; i++) {
        if (i != 0)
            values += "&";

        values += array_input[i].name + "=" + escape(array_input[i].value);
    }
    sendByAjax(url, values, "post", target, "input");
}

/**
* Env�a un evento generado a una url via ajax y el resultado es depositado en una layer
*/
function send2layer(target, url, array_input) {
    var values = "";
    for (var i = 0; i < array_input.length; i++) {
        if (i != 0)
            values += "&";

        values += array_input[i].name + "=" + array_input[i].value;
    }
    sendByAjax(url, values, "post", target, "layer");
}

/**
* Obtiene la informacion del input element devolviendo el 'nombre del input' = 'valores'
*/
function getInputInfo(element) {
    var values = '';
    if (element.tagName == "INPUT") {
        if
        (
           ((element.type == "checkbox" || element.type == "radio") && element.checked)
           ||
           (element.type == "text")
        )
            values += "&" + element.name + "=" + escape(element.value);
    }
    else if (element.tagName == "SELECT") {
        var sel = element;
        var multiple = '';
        var first = true;
        for (var j = 0; j < sel.options.length; j++) {
            if (sel.options[j].selected) {
                //alert(sel.options[j].value);
                if (first) {
                    multiple = sel.options[j].value;
                    first = false;
                }
                else
                    multiple += "," + sel.options[j].value;

            }
        }
        values += "&" + sel.name + "=" + multiple;
    }

    return values;
}
/**
* Envia un formulario via ajax y deja el resultado en la capa target
*/
function sendFormByAjax(target, form, action, method, post_callback) {
    var values = "";
    for (i = 0; i < form.childNodes.length; i++) {
        if (form.childNodes[i].tagName != "FIELDSET")
            values += getInputInfo(form.childNodes[i]);

        else {
            var fieldset = form.childNodes[i];

            for (var j = 0; j < fieldset.childNodes.length; j++) {

                values += getInputInfo(fieldset.childNodes[j]);
            }
        }
    }
    if (target)
        sendByAjax(action, values, method, target, "layer", post_callback);
    else
        sendByAjax(action, values, method, '', '', post_callback);
}