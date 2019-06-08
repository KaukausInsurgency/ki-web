$(function() {  
	$('.game-dropdown').click(function()
	{
		var container = $(this.closest('.game-dropdown'));
		container.toggleClass('js-flyout');
		if (container.hasClass('js-flyout'))
			container.children('.game-flyout-content').css('display','block');
		else
			container.children('.game-flyout-content').css('display','none');
	});

	$("body").click
    (
        function(e)
        {
            if(!$(e.target).closest(".game-dropdown").length)
            {
				$('.game-dropdown').removeClass('js-flyout');
				$('.game-flyout-content').css('display','none');
            }
        }
    );
});