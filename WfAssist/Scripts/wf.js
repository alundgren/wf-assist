$(document).ready(function () {
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