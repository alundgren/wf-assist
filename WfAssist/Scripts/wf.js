$(document).ready(function () {
    if (screen.width <= 800) {
        //HACK:Should really be adaptive but I give up. Damn htc browser is just determined to pretend it
        //has a huge viewport (reports 800 screen.width, innerWidth, width all report lies)
        $("#container").width(300);
    }
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