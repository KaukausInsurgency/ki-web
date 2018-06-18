$(function () {
    // setup signalR
    $.connection.hub.logging = true;
    var GameHubProxy = $.connection.gameHub;    // apparently first letter is lowercase (signalr converts this)
    var $Chat = $('#ingame-chat').kiChat();
    var $ChatNotif = $('#ingame-chat').kiNotif();

    var $OPTable = $('#players-table tbody');

    GameHubProxy.client.UpdateCapturePoints = function (modelObj) {
        $(modelObj).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('CP', this.ID);
            var id = 'CP-' + this.ID;
            var mrk = LiveMap.CAPTUREPOINTS.find(x => x.args.marker_id === id);

            if (typeof mrk !== 'undefined')
            {
                mrk.args.htmlContent = iconBuilder.create(this.Status, this.Type)
                mrk.draw();  
            }

            LiveMap.updateTooltipContent($dataSel, LiveMap.CPTemplate, this);
        });
    }

    GameHubProxy.client.UpdateDepots = function (modelObj) {
        console.log("UpdateDepots: " + JSON.stringify(modelObj));
    }

    GameHubProxy.client.UpdateMissions = function (modelObj) {
        $(modelObj).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('SM', this.ID);
            var id = 'SM-' + this.ID;
            var timeLeftInSeconds = this.TimeRemaining;
            this.TimeRemaining = LiveMap.convertSecondsToTimeString(this.TimeRemaining);

            if (timeLeftInSeconds <= 0 || this.Status === "Timeout") {
                var mrk = LiveMap.SIDEMISSIONS.find(x => x.args.marker_id === id);
                var index = LiveMap.SIDEMISSIONS.findIndex(x => x.args.marker_id === id);
                // TODO - this delete is not 100% functional yet
                // delete expired missions after 2 minutes
                setTimeout(function () {
                    if (index > -1) {
                        LiveMap.SIDEMISSIONS.splice(index, 1);
                    }
                    mrk.setMap(null);
                    delete mrk;
                }, 3000);
            }        
            // TODO - Create new marker and add to list if does not exist in DOM
            // TODO - when markers are initialized first time on page load, call this expire function too

            LiveMap.updateTooltipContent($dataSel, LiveMap.CPTemplate, this);
        });

        
    }

    GameHubProxy.client.UpdateChat = function (modelObj) {
        console.log("UpdateChat: " + JSON.stringify(modelObj));
        $Chat.add(modelObj.Name, modelObj.Side, modelObj.Message);
        $ChatNotif.incr();
    }

    GameHubProxy.client.UpdateOnlinePlayers = function (modelObj) {
        $(modelObj).each(function (i) {
            var sel = '[data-UCID="' + this.UCID + '"]';
            var $tr = $(sel);
            this.IsRole = !LiveMap.isStringNullOrWhiteSpace(this.Role);

            var factionClass = "online-players-neutral";
            var faction = "Neutral";
            if (this.Side == 1) {
                factionClass = "online-players-redfor";
                faction = "Redfor";
            }
            else if (this.Side == 2) {
                factionClass = "online-players-blufor";
                faction = "Blufor";
            }

            this.Faction = faction;
            this.FactionClass = factionClass;

            // create the tr row
            if ($tr.length === 0) {
                $OPTable.append(Mustache.render(LiveMap.OPTemplate, this));
            }
            else {
                $tr.replaceWith(Mustache.render(LiveMap.OPTemplate, this));
            }         
        });

        // remove all rows that have not been marked with the .js-updated-record class
        $('[data-UCID]:not(.js-updated-record)').remove();

        // remove this class from all rows
        $('[data-UCID]').removeClass('js-updated-record');

        KI.setDivText('#OnlinePlayersCount', modelObj.length);
        // need to divide by zero as we apply these classes on two table cells of the table
        KI.setDivText('#RedforPlayersCount', $('.online-players-redfor').length / 2);
        KI.setDivText('#BlueforPlayersCount', $('.online-players-blufor').length / 2);
        KI.setDivText('#NeutralPlayersCount', $('.online-players-neutral').length / 2);
    }

    GameHubProxy.client.UpdateServer = function (modelObj) {
        var restartTime = ((14400 - modelObj.RestartTime) / 14400.0) * 100;
        $('#RestartTimeProgress').css('width', restartTime + '%');
        KI.setDivText('#RestartTimeString', 'Time Remaining: ' + modelObj.RestartTimeString);
        KI.setDivText('#Status', modelObj.Status);
        KI.setDivText('#Map', modelObj.Map);
    };


    $.connection.hub.start().done(function () {

        $(window).bind('beforeunload', function () {
            var GHubProxy = $.connection.gameHub;
            GHubProxy.server.unsubscribe(MODEL.ServerID);
        });

        var GHubProxy = $.connection.gameHub;
        GHubProxy.server.subscribe(MODEL.ServerID);
    });

    $.connection.hub.error(function (error) {
        console.log('SignalR error: ' + error)
    });

})