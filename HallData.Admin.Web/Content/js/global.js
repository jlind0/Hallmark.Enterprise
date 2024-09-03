$(document).ready(function () {

    try {
        if ($("form").length) {
            $("form").validate({
                ignore: ".ignore"
            });
            $("form").submit(function () {
                if ($(this).valid()) {
                    pleaseWait();
                    return true;
                }
                else {
                    stopWait();
                    return false;
                }
            });
        };
    }
    catch (e) { }

    $(".grid").each(function () {
        $(this).find("table").dataTable({
            //"paging": false
        });
    });

    $(".account").click(function () {
        $(this).css("box-shadow", "none");
        var X = $(this).attr('id');
        if (X == 1) {
            $(".submenu").hide();
            $(this).attr('id', '0');
        }
        else {
            $(".submenu").show();
            $(this).attr('id', '1');
        }
    });

    $(document).mouseup(function () {
        $(".submenu").hide();
        $(".account").attr('id', '');
    });

});
function showMessage(type, subject, body) {
    if (body != "" && body != undefined) {
        noty({ text: subject + ' - ' + body, dismissQueue: false, type: type, timeout: 2000 });
    }
    else {
        noty({ text: subject, dismissQueue: false, type: type, timeout: 2000 });
    }
}
function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results == null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function loadSelect(element, url, selectedvalue, addempty, autohide) {
    element.html("");
    if (addempty == true) {
        element.append($('<option>', { value: "" }).text(""));
    }
    $.ajax({
        type: "GET",
        contentType: "application/json; charset=utf-8",
        async: false,
        url: url,
        success: function (data) {
            if (data.options.length == 0) {
                if (autohide == true) {
                    element.hide();
                }
            }
            else {
                element.prop("disabled", "");
                element.show();
                for (var i = 0; i < data.options.length; i++) {
                    if (selectedvalue == data.options[i].Key) {
                        element.append($('<option>', { value: data.options[i].Key, selected: "selected" }).text(data.options[i].Value));
                    }
                    else {
                        element.append($('<option>', { value: data.options[i].Key }).text(data.options[i].Value));
                    }
                }
            }
        }
    });
}

function pleaseWait() {
    var target = document.getElementById("divPleaseWait");
    $(target).fadeIn();
    var opts = {
        lines: 12, // The number of lines to draw
        length: 7, // The length of each line
        width: 4, // The line thickness
        radius: 10, // The radius of the inner circle
        color: '#000', // #rgb or #rrggbb
        speed: 1, // Rounds per second
        trail: 60, // Afterglow percentage
        shadow: false, // Whether to render a shadow
        hwaccel: false // Whether to use hardware acceleration
    };
    var spinner = new Spinner(opts).spin(target);
    $(target).data('spinner', spinner);

    return spinner;
}
function stopWait() {
    $('#divPleaseWait').fadeOut();
    $('#divPleaseWait').data('spinner').stop();
}

function createCookie(name, value, days) {
    if (days) {
        var date = new Date();
        date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
        var expires = "; expires=" + date.toGMTString();
    } else var expires = "";
    document.cookie = escape(name) + "=" + escape(value) + expires + "; path=/";
}

function readCookie(name) {
    var nameEQ = escape(name) + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') c = c.substring(1, c.length);
        if (c.indexOf(nameEQ) == 0) return unescape(c.substring(nameEQ.length, c.length));
    }
    return "";
}