$(document).ready(function () {
    $('[data-alert-close]').click(function () {
        $(this).parent().parent().hide();
    });
});
