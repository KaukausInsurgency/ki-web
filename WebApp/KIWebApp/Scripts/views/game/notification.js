(function ($) {
    $.fn.kiNotif = function () {
        var obj = this;
        var childSel = 'span.live-map-footer-item';
        return {
            set: function (content) {
                var span = $(obj).children(childSel);
                if (!span.children('a').length)
                    span.append("<a class='notification'>" + content + "</a>");
                else
                    span.children('a').html(content);
            },
            remove: function () {
                var span = $(obj).children(childSel);
                span.children('a').remove('');
            }
        }
    };
})(jQuery);