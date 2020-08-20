$(document).ready(function () {
    var userId = $("[data-user]").data("user");

    $("[data-event]").each(function () {
        var timeout = null

        var eventId = $(this).data("event");

        var connection = new signalR.HubConnectionBuilder()
            .withUrl("/eventHub")
            .withAutomaticReconnect()
            .build();

        connection.on("Tack", function (event) {
            if (eventId !== event.id) return;

            var timeLeft = event.timeLeft;

            var days = timeLeft.days.toString().padStart(2, "0");
            var hours = timeLeft.hours.toString().padStart(2, "0");
            var minutes = timeLeft.minutes.toString().padStart(2, "0");
            var seconds = timeLeft.seconds.toString().padStart(2, "0");
            var text = `Days: ${days} Hours: ${hours}:${minutes}:${seconds}`;

            var p = $("<p>").text(text).addClass('event__item__time-left');

            $(`[data-event=${event.id}]`).html(p);

            timeout && clearTimeout(timeout);

            timeout = setTimeout(function () {
                connection.invoke("Tick", userId, event.id);
            }, 1000)
        });

        connection.on("Start", function (event) {
            location.reload();
        });

        connection.on("Stop", function (event) {
            location.reload();
        });

        connection.start().then(function () {
            connection.invoke("Tick", userId, eventId);
        });
    });
});
