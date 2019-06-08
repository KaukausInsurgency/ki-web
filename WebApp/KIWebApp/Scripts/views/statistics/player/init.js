$(document).ready(function () {
    var urlServersGame = $('[data-url-game]').data('url-game');
    var urlStatisticsAirframe = $('[data-url-airframe-statistics]').data('url-airframe-statistics');
    var urlStatisticsServer = $('[data-url-server-statistics]').data('url-server-statistics');
    var airframeTable = $('#airframe-table');
    var ucid = $('[data-ucid]').data('ucid');

    function getAirframe($el) {
        var tr = $($el).closest('tr').find('.js-airframe-name');
        return tr.text();
    }

    $('.js-clickable-airframe').click(function () {
        var url = urlStatisticsAirframe + "?playerUCID=" + ucid + "&airframe=" + getAirframe(this);
        window.location.href = url;
        return false;
    });
    
});