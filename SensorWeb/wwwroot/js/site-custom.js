jQuery(function ($) {
    $(".maskdata").mask("99/99/9999");
    $(".maskcpf").mask("999.999.999-99");
});


function disableForm() {
    var inputs = document.getElementsByTagName("input");
    for (var i = 0; i < inputs.length; i++) {
        inputs[i].disabled = true;
    }
    var selects = document.getElementsByTagName("select");
    for (var i = 0; i < selects.length; i++) {
        selects[i].disabled = true;
    }
    var textareas = document.getElementsByTagName("textarea");
    for (var i = 0; i < textareas.length; i++) {
        textareas[i].disabled = true;
    }
    var buttons = document.getElementsByTagName("button");
    for (var i = 0; i < buttons.length; i++) {
        buttons[i].disabled = true;
    }
}
