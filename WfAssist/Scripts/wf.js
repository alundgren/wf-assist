function debugViewport() {
    $("#window-height").html(document.documentElement.clientHeight);
    $("#window-width").html(document.documentElement.clientWidth);
    $("#window-inner-height").html(window.innerHeight);
    $("#window-inner-width").html(window.innerWidth);
    $("#window-x-offset").html(window.pageXOffset);
    $("#window-y-offset").html(window.pageYOffset);
}
$(document).ready(function () {
    debugViewport();
    $(window).resize(function () {
        debugViewport();
    });
    $("#user-agent").html(navigator.userAgent);
    $("#tabs").tabs();
    $("#lang-radio-set").buttonset();
    $("#progress").hide();
    $("#search").button({ icons: { primary: 'ui-icon-search' }, text: false });
    $("#pattern").focus();
    $("#search").click(function () {
        $("#progress").show();
        var patternText = $("#pattern").val();
        var lettersText = $("#availableLetters").val();
        var lang = $('input[name=lang-radio]:checked').val();
        var url = "";
        if (lettersText) {
            url = "wordsfiltered/" + encodeURIComponent(lang) + "/" + encodeURIComponent(patternText) + "/" + encodeURIComponent(lettersText);
        } else {
            url = "words/" + encodeURIComponent(lang) + "/" + encodeURIComponent(patternText);
        }
        //todo: pretty error popup
        $.ajax(url, {
            success: function (data) {
                $("#result").html($("#wordsTemplate").tmpl({ words: data }));
            },
            complete: function () {
                $("#progress").hide();
            }
        });
    });
});