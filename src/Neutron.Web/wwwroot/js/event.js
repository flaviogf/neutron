$(document).ready(function () {
    var userId = $("[data-user]").data("user")

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/eventHub")
        .withAutomaticReconnect()
        .build();

    connection.on("Tack", function (event) {
        var timeLeft = event.timeLeft

        var days = timeLeft.days.toString().padStart(2, "0")
        var hours = timeLeft.hours.toString().padStart(2, "0")
        var minutes = timeLeft.minutes.toString().padStart(2, "0")
        var seconds = timeLeft.seconds.toString().padStart(2, "0")
        var text = `Days: ${days} Hours: ${hours}:${minutes}:${seconds}`;

        var p = $("<p>").text(text)

        $(`[data-event=${event.id}]`).html(p)

        connection.invoke("Tick", userId, event.id);
    });

    connection.on("Start", function (event) {
        location.reload();
    })

    connection.on("Stop", function (event) {
        location.reload();
    });

    connection.start().then(function () {
        $("[data-event]").each(function () {
            var eventId = $(this).data("event")

            connection.invoke("Tick", userId, eventId);
        })
    });
});
