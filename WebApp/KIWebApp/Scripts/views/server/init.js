$(document).ready(function () {
    var urlServersGame = $('[data-url-game]').data('url-game');
    var urlStatisticsPlayer = $('[data-url-player-statistics]').data('url-player-statistics');
    var urlStatisticsServer = $('[data-url-server-statistics]').data('url-server-statistics');
    var type = "";
    function getServerID(el) {
        var serverIDDiv = $(el).closest('tr').find('.ServerID');
        var id = parseInt(serverIDDiv.text());
        return id;
    }

    function getUCID(el) {
        var UCIDDiv = $(el).closest('tr').find('.UCID');
        return UCIDDiv.text();
    }

    function dynatableRowClick(e) {
        var url = "";
        if (type === 'Servers') {
            url = urlServersGame + "?serverID=" + getServerID(this);
        }
        else if (type === 'Players') {
            url = urlStatisticsPlayer + "?playerUCID=" + getUCID(this);
        }
        window.location.href = url;
        return false;
    }

    function dynatableGraphClick(e) {
        var url = urlStatisticsServer + "?serverID=" + getServerID(this);
        window.location.href = url;
        return false;
    }

    function initDynatable(a) {
        if ($('#search-table').hasClass('players')) {
            type = 'Players';
        }
        else {
            type = 'Servers';
        }
        $('.js-clickable').click(dynatableRowClick);
        $('.js-clickable-img').click(dynatableGraphClick);
        KI.tooltipster('.js-clickable-img');
    };

    $('#search-table').dynatable({
        writers: {
            // custom row writer to add the js-clickable class to each tr
            _rowWriter: function myRowWriter(rowIndex, record, columns, cellWriter) {
                var tr = '';
        
                // grab the record's attribute for each column
                for (var i = 0, len = columns.length; i < len; i++) {
                    tr += cellWriter(columns[i], record);
                }
        
                return '<tr class="js-clickable">' + tr + '</tr>';
            },
            // custom function provided, because default write hides cells with inline-style
            // when the page is viewed on mobile with columns hidden
            // this causes the data to be hidden even when the media queries unhide the table columns
            _cellWriter: 
            function myCellWriter(column, record) {
                var html = column.attributeWriter(record);
                return '<td>' + html + '</td>';
            }
        }
    }).bind('dynatable:afterUpdate', initDynatable);

    initDynatable();
});