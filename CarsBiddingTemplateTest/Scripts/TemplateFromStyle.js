//this file represent script for html form input & button style

/* Html Input Script*/
$(() => {
    $('.form-group').each((i, e) => {
        $('.form-control', e)
            .focus(function () {
                e.classList.add('not-empty');
            })
            .blur(function () {
                this.value === '' ? e.classList.remove('not-empty') : null;
            });
    });
});
/* Html Input Script*/

/* bth hover effect */
$(function () {
    $('.btn-6')
        .on('mouseenter', function (e) {
            var parentOffset = $(this).offset(),
                relX = e.pageX - parentOffset.left,
                relY = e.pageY - parentOffset.top;
            $(this).find('span').css({
                top: relY,
                left: relX
            })
        })
        .on('mouseout', function (e) {
            var parentOffset = $(this).offset(),
                relX = e.pageX - parentOffset.left,
                relY = e.pageY - parentOffset.top;
            $(this).find('.btn-6 span').css({
                top: relY,
                left: relX
            })
        });
});
/* bth hover effect */