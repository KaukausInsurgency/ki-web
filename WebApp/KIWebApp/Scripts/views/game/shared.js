(function ($) {
    LiveMap = {
        CPTemplate: document.getElementById('InfoWindowTemplateCP').innerHTML,
        DepotTemplate: document.getElementById('InfoWindowTemplateDepot').innerHTML,
        SMTemplate: document.getElementById('InfoWindowTemplateSM').innerHTML,
        SMModalTemplate: document.getElementById('ModalTemplateSM').innerHTML,
        OPTemplate: document.getElementById('RowTemplateOP').innerHTML,

        createMarkerSelector: function (type, id) {
            return '[data-marker_id="' + type + '-' + id + '"]';
        },

        splitStringIntoArrayTable: function (str, sep1, sep2) {
            var tablearray = [];
            var rows = str.split(sep1);
            for (var i = 0; i < rows.length; i++) {
                tablearray.push(rows[i].split(sep2));
            }

            return tablearray;
        },

        generateResourceTable: function (arraytable) {
            var table = '<table class="depot-resource-table">';
            var tr = "";
            for (var i = 0; i < arraytable.length; i++) {
                if (i === 0) {
                    tr += '<thead><tr>';
                    for (var j = 0; j < arraytable[i].length; j++) {
                        tr += '<td class="depot-resource-row">' + arraytable[i][j] + '</td>';
                    }
                    tr += '</tr></thead><tbody>';
                }
                else {
                    tr += '<tr>';
                    for (j = 0; j < arraytable[i].length; j++) {
                        tr += '<td class="depot-resource-row">' + arraytable[i][j] + '</td>';
                    }
                    tr += '</tr>';
                }
            }

            table += tr + "</tbody></table>";
            return table;
        },

        isStringNullOrWhiteSpace: function (str) {
            return (str.length === 0 || !str.trim());
        },

        convertSecondsToTimeString: function (seconds) {
            var date = new Date(null);
            date.setSeconds(seconds); // specify value for SECONDS here
            return date.toISOString().substr(11, 8);
        },

        updateTooltipContent: function ($sel, mustacheTemplate, model) {
            var instances = $.tooltipster.instances($sel);
            $.each(instances, function (i, instance) {
                instance.content(Mustache.render(mustacheTemplate, model));
            });
        },

        createModal: function (options, mustacheTemplate, model) {
            $("#modal").iziModal({
                title: options.title,
                icon: options.icon,
                iconText: options.iconText,
                iconColor: options.iconColor,
                width: options.width,
                headerColor: '#e36209',      
                background: '#474747'
            });

            $("#modal").iziModal('setContent', {
                content: Mustache.render(mustacheTemplate, model),
                default: true
            });

            $('#modal').iziModal('open', {
                transition: 'fadeInDown'
            });
        },

        testExecute: function () {
            var m = MODEL.Missions[0];
            m.ProgressBarWidth = ((14400 - 1200) / 14400.0) * 100
            LiveMap.createModal({
                title: m.TaskName,
                icon: m.IconClass,
                width: 400
            }, LiveMap.SMModalTemplate, m);
        },

        DEPOTS: [],
        CAPTUREPOINTS: [],
        SIDEMISSIONS: [],
    }
})(jQuery);
