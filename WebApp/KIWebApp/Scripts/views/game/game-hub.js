$(function () {
    // setup signalR
    $.connection.hub.logging = true;
    var GameHubProxy = $.connection.gameHub;    // apparently first letter is lowercase (signalr converts this)
    var $Chat = $('#ingame-chat').kiChat();
    var $ChatNotif = $('#ingame-chat').kiNotif();

    var $OPTable = $('#players-table tbody');

    GameHubProxy.client.UpdateCapturePoints = function (modelObj) {
        console.log("UpdateCapturePoints: " + JSON.stringify(modelObj));
    }

    GameHubProxy.client.UpdateDepots = function (modelObj) {
        console.log("UpdateDepots: " + JSON.stringify(modelObj));
    }

    GameHubProxy.client.UpdateMissions = function (modelObj) {
        console.log("UpdateMissions: " + JSON.stringify(modelObj));
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
    }

    GameHubProxy.client.UpdateServer = function (modelObj) {
        console.log("UpdateServer: " + JSON.stringify(modelObj));
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