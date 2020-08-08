$(document).ready(function () {
    $('[data-user]').click(function (e) {
        e.stopPropagation();

        $('[data-sign-out]').toggleClass('navbar__button--visible');
    });

    $(document).click(function () {
        if (!$('[data-sign-out]').hasClass('navbar__button--visible')) {
            return;
        }

        $('[data-sign-out]').removeClass('navbar__button--visible');
    });
})
