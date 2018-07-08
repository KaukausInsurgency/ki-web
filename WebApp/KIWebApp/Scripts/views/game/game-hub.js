$(function () {
    // setup signalR
    $.connection.hub.logging = true;
    var GameHubProxy = $.connection.gameHub;    // apparently first letter is lowercase (signalr converts this)
    var $Chat = $('#ingame-chat').kiChat();
    var $Notifications = $('#ingame-notifications').kiChat();
    var $Missions = $('#ingame-missions').kiChat();
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

                LiveMap.updateTooltipContent($dataSel, LiveMap.CPTemplate, this);
            }
            // there's techically a bug here as we're not checking to see if the server was restarted and whether any of the markers have been removed or newly added
            // later we should add support to refresh all the markers on the map          
        });
    }

    GameHubProxy.client.UpdateDepots = function (modelObj) {
        $(modelObj).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('Depot', this.ID);
            var id = 'Depot-' + this.ID;
            var mrk = LiveMap.DEPOTS.find(x => x.args.marker_id === id);

            if (typeof mrk !== 'undefined') {
                mrk.args.htmlContent = iconBuilder.depot(this.Status)
                mrk.draw();

                var arraytable = LiveMap.splitStringIntoArrayTable(this.ResourceString, "\n", "|");
                this.ResourceHtml = LiveMap.generateResourceTable(arraytable);
                if (this.Capacity === -1 || this.Capacity === "-1") {
                    this.Capacity = 'infinite';
                    this.CurrentCapacity = 'infinite';
                }

                LiveMap.updateTooltipContent($dataSel, LiveMap.DepotTemplate, this);
            }
            // there's techically a bug here as we're not checking to see if the server was restarted and whether any of the markers have been removed or newly added
            // later we should add support to refresh all the markers on the map        
        });
    }

    GameHubProxy.client.UpdateMissions = function (modelObj) {
        $(modelObj).each(function (i) {   
            var timeLeftInSeconds = this.TimeRemaining;
            this.TimeRemaining = LiveMap.convertSecondsToTimeString(this.TimeRemaining);

            var $dataSel = LiveMap.createMarkerSelector('SM', this.ID);      
            var id = 'SM-' + this.ID;
            var index = LiveMap.SIDEMISSIONS.findIndex(x => x.args.marker_id === id);

            // new marker - create and initialize
            if (index < 0) {
                var mrk = new customMarker(
                    new google.maps.LatLng(this.Latitude, this.Longitude),
                    map,
                    {
                        marker_id: 'SM-' + this.ID,
                        htmlContent: iconBuilder.mission(this.IconClass)
                    }
                );

                mrk.draw();
                LiveMap.SIDEMISSIONS.push(mrk);

                KI.tooltipster($dataSel);

                $Missions.add(this.TimeRemaining, 0, "New mission '" + this.TaskName + "' available (Lat Long: " + this.LatLong + ", MGRS: " + this.MGRS + ")");
            }

            // delete expired missions after 30 seconds
            if (timeLeftInSeconds <= 0 || this.Status === "Timeout") {

                $Missions.add(this.TimeRemaining, 0, "Time has run out for mission '" + this.TaskName + "' (Lat Long: " + this.LatLong + ", MGRS: " + this.MGRS + ")");
                
                setTimeout(function () {
                    // need to get the new index, as its possible the array might have been resized before this call
                    var removeIndex = LiveMap.SIDEMISSIONS.findIndex(x => x.args.marker_id === id);
                    var removedMarkers = [];
                    if (removeIndex > -1) {
                        removedMarkers = LiveMap.SIDEMISSIONS.splice(removeIndex, 1);
                    }
                    removedMarkers[0].setMap(null);
                    delete removedMarkers[0];
                }, 30000);
            }        

            LiveMap.updateTooltipContent($dataSel, LiveMap.SMTemplate, this);

            $($dataSel).addClass('js-updated-marker');
        });

        // remove all rows that have not been marked as updated, and are orphaned markers
        $('[data-marker_id^="SM-"]:not(.js-updated-marker)').each(function (i) {
            var index = LiveMap.SIDEMISSIONS.findIndex(x => x.args.marker_id === $(this).data('marker_id'));
            if (index > -1) {
                var removedMarkers = LiveMap.SIDEMISSIONS.splice(index, 1);
                removedMarkers[0].setMap(null);
                delete removedMarkers[0];  
            }                  
        });

        // remove this class from all rows
        $('[data-marker_id]').removeClass('js-updated-marker');   
    }

    GameHubProxy.client.UpdateChat = function (modelObj) {
        $Chat.add(modelObj.Name, modelObj.Side, modelObj.Message);
    }

    GameHubProxy.client.UpdateNotifications = function (modelObj) {
        $Notifications.add(modelObj.Time, 0, modelObj.Message);
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
    };

    GameHubProxy.client.OnServerError = function (error) {
        LiveMap.createErrorModal(error);
    }


    $.connection.hub.start().done(function () {

        $(window).bind('beforeunload', function () {
            var GHubProxy = $.connection.gameHub;
            GHubProxy.server.unsubscribe(MODEL.ServerID);
        });

        var GHubProxy = $.connection.gameHub;
        GHubProxy.server.subscribe(MODEL.ServerID);
    });

    $.connection.hub.error(function (error) {
        LiveMap.createErrorModal("There was an error with the connection - please reload the page");
    });

    $.connection.hub.disconnected(function () {
        LiveMap.createErrorModal("Lost connection to server - please reload the page");
    });

})