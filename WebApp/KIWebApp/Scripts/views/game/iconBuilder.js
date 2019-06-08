
iconBuilder = (function () {
    var self = this;
    var iconSpan = $('<span class="' + $('.prop-icons').css('--icon-span').trim() + '"></span>');
    var iconBorder = $('<i class="' + $('.prop-icons').css('--icon-border').trim() + '"></i>');
    var iconCapturePoint = $('<i class="' + $('.prop-icons').css('--icon-capture-point').trim() + '"></i>');
    var iconDepot = $('<i class="' + $('.prop-icons').css('--icon-depot').trim() + '"></i>');
    var iconAirport = $('<i class="' + $('.prop-icons').css('--icon-airport').trim() + '"></i>');
    var iconFARP = $('<i class="' + $('.prop-icons').css('--icon-farp').trim() + '"></i>');

    function getSideClass(side) { // this function not available outside your module
        switch (side) {
            case 1:
                return 'redfor-bg-icon';
            case 2:
                return 'blufor-bg-icon';
            case "Red":
                return 'redfor-bg-icon';
            case "Blue":
                return 'blufor-bg-icon';
            case "Mission":
                return 'mission-bg-icon';
            case "Contested":
                return 'contested-bg-icon';
            default:
                return 'neutral-bg-icon';
        }
    }

    function createIconBase(side) {
        var iconClone = iconSpan.clone();
        var iconBorderClone = iconBorder.clone();
        iconBorderClone.addClass(getSideClass(side));
        iconClone.append(iconBorderClone);
        return iconClone;
    }

    function create(side, type, iconClass) {
        var icon = createIconBase(side);

        switch (String(type)) {
            case "AIRPORT":
                icon.append(iconAirport);
                break;
            case "DEPOT":
                icon.append(iconDepot);
                break;
            case "CAPTUREPOINT":
                icon.append(iconCapturePoint);
                break;
            case "FARP":
                icon.append(iconFARP);
                break;
            case "MISSION":
                if (typeof iconClass !== "undefined")
                {
                    icon.append('<i class="' + iconClass + ' fa-stack-1x"></i>');    // side is actually the iconClass used by mission    
                }                     
            default:
                break;
        }

        return icon.prop('outerHTML');
    }

    return {
        create: create,
        capturePoint: function (side) {
            return create(side, "CAPTUREPOINT");
        },
        depot: function (side) {
            return create(side, "DEPOT");
        },
        airport: function (side) {
            return create(side, "AIRPORT");
        },
        farp: function (side) {
            return create(side, "FARP");
        },
        mission: function (iconClass) {
            return create("Mission", "MISSION", iconClass);
        }
    };
})();