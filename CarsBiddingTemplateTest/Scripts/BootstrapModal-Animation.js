function Animation(animationName,eventSource) {
    $(eventSource).find('.modal-dialog').attr('class', 'modal-dialog animate__animated  animate__' + animationName);
};
$('.modal').on('show.bs.modal', function (e) {
    let eventSource = $(this),
        anim = "zoomIn";
    Animation(anim, eventSource);
});
$('.modal').on('hide.bs.modal', function (e) {
    let eventSource = $(this),
        anim = "zoomOut";
    Animation(anim, eventSource);
});
/*
 * we send eventSource to Animation function in order to 
 * execute animation just at Current Modal 
 * 
 * ex to explain:
 * suppose we use $('.modal .modal-dialog') instead of $(eventSource).find('.modal-dialog')
 * so when open BiddingProcess Modal then increase the price successfuly so will open Success Notification
 * popUp so now there are 2 Model opening (Bidding Process & Success Notification popUp)
 * so actually when open or close Modal will add Animation for all Opening Modal
 * so we will note that when close Success Notification Modal will also close Bidding Process Modal 
 * automatically becasue we give it zoomOut animation so we need to make animation code applying just
 * in current Modal(eventSource) so we used $(eventSource).find('.modal-dialog') instead of $('.modal .modal-dialog')
 */