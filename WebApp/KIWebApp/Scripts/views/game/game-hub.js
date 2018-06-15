$(function () {
    // setup signalR
    $.connection.hub.logging = true;
    var GameHubProxy = $.connection.gameHub;    // apparently first letter is lowercase (signalr converts this)
    var $Chat = $('#ingame-chat').kiChat();
    var $ChatNotif = $('#ingame-chat').kiNotif();

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
        console.log("UpdateOnlinePlayers: " + JSON.stringify(modelObj));
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