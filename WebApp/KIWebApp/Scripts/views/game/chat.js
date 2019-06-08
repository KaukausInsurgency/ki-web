(function ($) {
    $.fn.kiChat = function () {
        var obj = this;
        
        var $chatMessageBody= obj.find('.chat-message-body');
        var $chatMessage = $('<div class="chat-message"></div>');
        var $chatUsername = $('<span class="chat-username"></span>');
        var $chatBody = $('<span class="chat-body"></span>');

        var childSel = 'span.live-map-footer-span';
        var notificationValue = 0;

        // bind events
        obj.on('click', showWindow);
        $chatMessageBody.scroll(onScroll);
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
                $('.chat-window').css('display', '');   // not setting to none - this resets back to what it was before
            }
        }

        function onScroll(event) {
            if (isWindowOpen() && isScrollNearBottom()) {
                clearNotification();
            } 
        }

        // Notification Functions
        function incrementNotification(val) {
            if (typeof val !== 'undefined')
                notificationValue += val;
            else
                notificationValue += 1;

            var span = $(obj).children(childSel);
            if (!span.children('a').length)
                span.append("<a class='notification'>" + notificationValue + "</a>");
            else
                span.children('a').html(notificationValue);
        }

        function setNotification(content) {
            notificationValue = content;
            var span = $(obj).children(childSel);
            if (!span.children('a').length)
                span.append("<a class='notification'>" + content + "</a>");
            else
                span.children('a').html(content);
        }

        function clearNotification() {
            var span = $(obj).children(childSel);
            span.children('a').remove('');
            notificationValue = 0;
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

        function isScrollNearBottom() {
            var scrolledHeight = $chatMessageBody.scrollTop();
            var realHeight = $chatMessageBody[0].scrollHeight;
            var elementHeight = $chatMessageBody.height();

            return (realHeight - scrolledHeight) <= (elementHeight + 50);
        }

        function scrollDown() {
            var realHeight = $chatMessageBody[0].scrollHeight;
            var elementHeight = $chatMessageBody.height();
            $chatMessageBody.scrollTop(realHeight - elementHeight);
        }

        function isWindowOpen() {
            var $chatWindow = $(obj).children('.chat-window');
            return $chatWindow.css('display') !== 'none';
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
                if (isWindowOpen() && isScrollNearBottom()) {
                    scrollDown();
                    clearNotification();
                } 
                else {
                    incrementNotification();
                }                
            }
        }
    };
})(jQuery);
