//
function showNotification(message) {
    var $notification = $('#notification');
    $notification.text(message).addClass('show');

    setTimeout(function () {
        $notification.removeClass('show').fadeOut(500, function () {
            $(this).css('display', 'none').css('opacity', ''); // Reset CSS properties
        });
    }, 2000);
}
