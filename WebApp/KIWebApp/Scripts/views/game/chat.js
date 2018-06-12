(function ($) {
    $.fn.kiChat = function () {
        var obj = this;
        
        var $chatMessageBody= obj.find('.chat-message-body');
        var $chatMessage = $('<div class="chat-message"></div>');
        var $chatUsername = $('<span class="chat-username"></span>');
        var $chatBody = $('<span class="chat-body"></span>');

        // bind events
        obj.on('click', showWindow);
        $('body').on('click', hideWindow);

        function showWindow(event) {
            var $chatWindow = $(event.target).children('.chat-window');
            if ($chatWindow.length) {
                $chatWindow.css('display', 'block');
                $('.chat-window').not($chatWindow).css('display', 'none');
            }
            
        }

        function hideWindow(event) {
            var $chatLayout = $(event.target).closest('.live-map-footer-column');
            if (!$chatLayout.length) {
                $('.chat-window').css('display', '');
            }
        }

        function getSideClass(side) {
            switch (side) {
                case 1:
                    return 'chat-redfor';
                    break;
                case 2:
                    return 'chat-blufor';
                    break;
                default:
                    return 'chat-neutral';
                    break;
            }
        }

        function create(username, side, body) {

            var $chatUsernameClone = $chatUsername.clone();
            $chatUsernameClone.addClass(getSideClass(side));
            $chatUsernameClone.html(username);

            var $chatBodyClone = $chatBody.clone();
            $chatBodyClone.html(' : ' + body);

            var $chatMessageClone = $chatMessage.clone();
            $chatMessageClone.append($chatUsernameClone);
            $chatMessageClone.append($chatBodyClone);

            return $chatMessageClone;
        }

        return {
            add: function (username, side, text) {
                var $newChatMessage = create(username, side, text);
                $chatMessageBody.append($newChatMessage);
                $chatMessageBody.animate({ scrollTop: obj.prop('scrollHeight') }, 1000);
            }
        }
    };
})(jQuery);
