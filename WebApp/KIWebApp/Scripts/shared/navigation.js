$(function () {
	var vnavMaxWidth = $('.prop-vnav').css('--max-width').trim();
	var vnavMinWidth = $('.prop-vnav').css('--min-width').trim();
	var $expandClass = $('.prop-vnav').css('--font-arrow-expand').trim();
	var $collapseClass = $('.prop-vnav').css('--font-arrow-collapse').trim();

	var $vnav = $('.vnav');
	var $vnavDropdownContent = $('.vnav-dropdown-content');
	var width = $(window).width();

	var fncAnimate = function ($el, width, animComplete) {
		$el.stop(true).animate({
			width: width
		}, 200, animComplete);
	}

	var toggleNavExpand = function(shouldExpand) {
		if (shouldExpand) {
			$vnavDropdownContent.removeClass('vnav-collapsed');
			$vnavDropdownContent.addClass('vnav-expanded');
			$vnav.removeClass('vnav-collapsed');
			$vnav.addClass('vnav-expanded');
		}
		else {
			$vnavDropdownContent.removeClass('vnav-expanded');
			$vnavDropdownContent.addClass('vnav-collapsed');
			$vnav.removeClass('vnav-expanded');
			$vnav.addClass('vnav-collapsed');
		}
	}

	if (width > 450) {
		$vnavDropdownContent.addClass('vnav-expanded');	// by default the ribbon is expanded
		$('.vnav-btn-expander').children('i').addClass($collapseClass);
		$vnav.addClass('vnav-expanded');
		$vnav.css('width', vnavMaxWidth);
	}
	else {
		$vnavDropdownContent.addClass('vnav-collapsed');	// by default the ribbon is collapsed
		$('.vnav-btn-expander').children('i').addClass($expandClass);
		$('.js-nav-text').hide();
		$vnav.addClass('vnav-collapsed');
		
	}

	$('.vnav-btn-expander').click(function () {
		var expander_icon = $(this).children('i');
		if (expander_icon.hasClass($expandClass))	// expand the nav ribbon
		{
			expander_icon.removeClass($expandClass);
			expander_icon.addClass($collapseClass);
			fncAnimate($vnav, vnavMaxWidth,
				function() {
					$('.js-nav-text').show();
				});
				toggleNavExpand(true);
		}
		else  // collapse the nav ribbon
		{
			expander_icon.removeClass($collapseClass);
			expander_icon.addClass($expandClass);
			$('.js-nav-text').hide();
			fncAnimate($vnav, vnavMinWidth);
			toggleNavExpand(false);
		}
	});

	// when hovering over a elements on the vnav, animate the widths to look smooth, but only if the nav is collapsed
	$('.vnav a').hover(function(e) {
		// only animate if the nav bar is collapsed
		if ($vnav.hasClass('vnav-collapsed')) {
			fncAnimate($(this), vnavMaxWidth, function() {
				$(this).children('.js-nav-text').show();
			});
		}
	}, function(e) {
		// only animate if the nav bar is collapsed
		if ($vnav.hasClass('vnav-collapsed')) {
			$(this).children('.js-nav-text').hide();
			fncAnimate($(this), '100%');
		}
	});

	$('.vnav-dropdown').hover(function(e) {
		var $dropdownContent = $(this).children('.vnav-dropdown-content');
		var $dropdownText = $dropdownContent.children('a').children('.js-dropdown-text');
		// hide the text first, other wise the text tries to display in the 0 width box and it makes the transition look janky
		$dropdownText.hide();	
		$dropdownContent.css('display', 'block');
		fncAnimate($dropdownContent, vnavMaxWidth, function() {
			$dropdownText.show();
		});
	}, function(e) {
		var $dropdownContent = $(this).children('.vnav-dropdown-content');	
		var $dropdownText = $dropdownContent.children('a').children('.js-dropdown-text');
		// hide the text first, other wise the text tries to display in the 0 width box and it makes the transition look janky
		$dropdownText.hide();
		fncAnimate($dropdownContent, '0px', function() {
			$dropdownContent.css('display', 'none');
		});
	});
});