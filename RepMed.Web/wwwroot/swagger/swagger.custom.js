

function updateTitle() {
    var elements = document.getElementsByTagName('title');
    if (elements.length > 0) {
        var content = elements[0].textContent;
        document.title = "LMS";
    }
    else {
        setTimeout(updateTitle, 1000);
    }
}
function updateFavicon() {
    //$('link[rel="icon"][sizes="32x32"]').attr('href', "/swagger/favicon-32x32.png");
    //$('link[rel="icon"][sizes="16x16"]').attr('href', "/swagger/favicon-16x16.png");
    $('link[rel="icon"][sizes="32x32"]').attr('href', "/favicon.ico");
}

function setTimeZoneHeader() {
    $('.try-out__btn').on('click', function () {
        debugger;
        if ($(this).data('param-name') === "X-User-TimeZone") {
            var sdf = "";
        }
    });
    
}
//setTimeZoneHeader();
updateFavicon();
updateTitle();