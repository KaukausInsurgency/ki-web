$(document).ready(function () {
    var urlServersGame = $('[data-url-game]').data('url-game');

    function getServerID(el) {
        var serverIDDiv = $(el).closest('tr').find('.ServerID');
        var id = parseInt(serverIDDiv.text());
        return id;
    }

    function dynatableRowClick(e) {
        var url = urlServersGame + "?serverID=" + getServerID(this);
        window.location.href = url;
        return false;
    }

    function dynatableGraphClick(e) {
        var url = urlServersGame + "?serverID=" + getServerID(this);
        window.location.href = url;
        return false;
    }

    function initDynatable(a) {
        $('.js-clickable').click(dynatableRowClick);
        $('.js-clickable-img').click(dynatableGraphClick);
        KI.tooltipster('.js-clickable-img');
    };

    $('#servers-table').dynatable({
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