$(document).ready(function() {

    var hub = $.connection.notificationHub;

    $.connection.hub.start().done(function () {
        console.log("Hub Connected!" + $.connection.hub.id);

        })
        .fail(function () {
            console.log("Could not Connect!");
        });

});