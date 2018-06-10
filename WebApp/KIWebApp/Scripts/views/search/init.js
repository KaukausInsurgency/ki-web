$(document).ready(function () {
    var urlServerAjax = $('[data-url-search-servers-ajax]').data('url-search-servers-ajax');
    var urlPlayerAjax = $('[data-url-search-players-ajax]').data('url-search-players-ajax');

    var $serverMenuItem = $('#js-server-search');
    var $playerMenuItem = $('#js-player-search');

    // reuse the existing script in the server/init.js as this view initializes the same dom elements
    var urlBase = $('[data-url-base]').data('url-base');
    var urlServerInitScript = urlBase + 'scripts/views/server/init.js';

    var initiateAjax = function (url, onComplete) {
        $('.search-results').html('');
        KI.loader(true, '');

        $.ajax({
            url: url,
            datatype: "text/plain",
            type: "GET",
            data: {
                Query: MODEL.Query
            },
            success: function (data) {
                $('.search-results').html(data);
                onComplete(data);
            }
        });
    }

    var menuShouldPerformAction = function ($menuItem, results) {
        if (!$menuItem.hasClass('selected') && results > 0) {
            $('.menu-item').removeClass('selected');
            $menuItem.addClass('selected');
            return true;
        }
        else {
            return false;
        }
    }

    if (MODEL.ServerResults > 0) {
        $serverMenuItem.addClass('selected');
        initiateAjax(urlServerAjax, function (data) {
            KI.loadScript(urlServerInitScript, function () { });
            KI.loader(false, '');
        });     
    }
    else if (MODEL.PlayerResults > 0) {
        $playerMenuItem.addClass('selected');
        initiateAjax(urlPlayerAjax, function (data) {
            KI.loadScript(urlServerInitScript, function () { });
            KI.loader(false, '');
        });     
    }

    $serverMenuItem.click(function () {
        if (menuShouldPerformAction($serverMenuItem, MODEL.ServerResults)) {
            initiateAjax(urlServerAjax, function (data) {
                KI.loadScript(urlServerInitScript, function () { });
                KI.loader(false, '');
            }); 
        }  
    });

    $playerMenuItem.click(function () {
        if (menuShouldPerformAction($playerMenuItem, MODEL.PlayerResults)) {
            initiateAjax(urlPlayerAjax, function (data) {
                KI.loadScript(urlServerInitScript, function () { });
                KI.loader(false, '');
            });
        }
    });
    
});