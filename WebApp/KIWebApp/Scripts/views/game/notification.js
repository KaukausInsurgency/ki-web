(function ($) {
    $.fn.kiNotif = function () {
        var obj = this;
        var childSel = 'span.live-map-footer-span';
        var value = 0;
        return {
            incr: function (val) {
                if (typeof val !== 'undefined')
                    value += val;
                else
                    value += 1;

                var span = $(obj).children(childSel);
                if (!span.children('a').length)
                    span.append("<a class='notification'>" + value + "</a>");
                else
                    span.children('a').html(value);
            },
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
                value = 0;
            }
        }
    };
})(jQuery);