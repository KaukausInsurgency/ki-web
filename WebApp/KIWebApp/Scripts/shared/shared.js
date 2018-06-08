(function($){
    KI = {
        tooltipster: function(sel) {
            $(sel).tooltipster({
                theme: 'tooltipster-borderless',
                functionPosition: function(instance, helper, position) {
                    var leftOffset = $('.prop-vnav').css('--min-width').trim().replace('px','');
                    position.coord.left = position.coord.left - leftOffset;
                    position.target = position.target - leftOffset;
                    return position;
                }
            });   
        }
    } 
})(jQuery);
