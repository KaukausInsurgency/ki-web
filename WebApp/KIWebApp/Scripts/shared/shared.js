(function($){
    KI = {
        tooltipster: function(sel) {
            $(sel).tooltipster({
                theme: 'tooltipster-borderless',
                contentAsHTML: true,
                trigger: 'click',
                contentCloning: true,
                functionPosition: function(instance, helper, position) {
                    var leftOffset = $('.prop-vnav').css('--min-width').trim().replace('px','');
                    position.coord.left = position.coord.left - leftOffset;
                    position.target = position.target - leftOffset;
                    return position;
                }
            });   
        },

        loader: function (state, sel, selParent) {
            var $parent = $(selParent);
            if (state) {
                $(sel + '.loader').addClass('loading');
                if ($parent.length) {
                    $parent.show();
                }
            }
            else {
                $(sel + '.loader').removeClass('loading');
                if ($parent.length) {
                    $parent.hide();
                }
            }       
        },

        loadScript: function (url, completeCallback) {
            var script = document.createElement('script'), done = false,
                head = document.getElementsByTagName("head")[0];
            script.src = url;
            script.onload = script.onreadystatechange = function () {
                if (!done && (!this.readyState ||
                    this.readyState == "loaded" || this.readyState == "complete")) {
                    done = true;
                    completeCallback();

                    // IE memory leak
                    script.onload = script.onreadystatechange = null;
                    head.removeChild(script);
                }
            };
            head.appendChild(script);
        },

        setDivText: function (sel, text) {
            $(sel).contents().filter(function () {
                return this.nodeType == 3
            }).each(function () {
                this.textContent = text;
            });
        }
    } 
})(jQuery);
