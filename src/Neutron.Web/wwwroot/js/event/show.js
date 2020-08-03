$(document).ready(function () {
    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/eventHub")
        .withAutomaticReconnect()
        .build();

    connection.on("Tack", function (event) {
        var timeLeft = event.timeLeft
        var days = timeLeft.days.toString().padStart(2, '0')
        var hours = timeLeft.hours.toString().padStart(2, '0')
        var minutes = timeLeft.minutes.toString().padStart(2, '0')
        var seconds = timeLeft.seconds.toString().padStart(2, '0')
        var text = `Days: ${days} Hours: ${hours}:${minutes}:${seconds}`;
        $('[data-time-left]').html(text)
    });

    connection.start().then(function () {
        var eventId = $('[name=Id]').val();
        connection.invoke("Tick", eventId);
    });
});
