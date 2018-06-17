function initGoogleMap() {
	function customMarker(latlng, map, args) {
		this.latlng = latlng;
		this.args = args;
		this.setMap(map);
	}

	customMarker.prototype = new google.maps.OverlayView();

	customMarker.prototype.draw = function () {
		var self = this;
		var div = this.div;

		if (!div) {

			div = this.div = document.createElement('div');
			div.className = 'marker';
			div.style.position = 'absolute';

			//set the values passed in from the creation of the custom marker
			div.innerHTML = this.args.htmlContent;
			div.style.color = this.args.color;

			if (typeof (self.args.marker_id) !== 'undefined') {
				div.dataset.marker_id = self.args.marker_id;
			}

			//add events to the marker

			google.maps.event.addDomListener(div, "mouseover", function (e) {
				//do something on mouseover, maybe show some tooltip text
			})

			google.maps.event.addDomListener(div, "mouseout", function (e) {
				//do something on mosueout, hide the tooltip text
            })

            google.maps.event.addDomListener(div, "click", function (event) {
                google.maps.event.trigger(self, "click");
            });

			var panes = this.getPanes();
			panes.overlayImage.appendChild(div);
		}

		var point = this.getProjection().fromLatLngToDivPixel(this.latlng);

		//position the custom marker on the map
		if (point) {
			div.style.left = (point.x - 15) + 'px';
			div.style.top = (point.y - 30) + 'px';
		}
	};

	customMarker.prototype.remove = function () {
		if (this.div) {
			this.div.parentNode.removeChild(this.div);
			this.div = null;
		}
	};

	customMarker.prototype.getPosition = function () {
		return this.latlng;
	};
	var mapProp = {
		zoom: 6.5,
		center: { lat: 42.332864120611, lng: 41.875537702983 },
		mapTypeId: 'terrain',
		styles: [
			{
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#242f3e"
					}
				]
			},
			{
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#746855"
					}
				]
			},
			{
				"elementType": "labels.text.stroke",
				"stylers": [
					{
						"color": "#242f3e"
					}
				]
			},
			{
				"featureType": "administrative",
				"elementType": "geometry",
				"stylers": [
					{
						"visibility": "off"
					}
				]
			},
			{
				"featureType": "administrative.locality",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#d59563"
					}
				]
			},
			{
				"featureType": "poi",
				"stylers": [
					{
						"visibility": "off"
					}
				]
			},
			{
				"featureType": "poi",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#d59563"
					}
				]
			},
			{
				"featureType": "poi.park",
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#263c3f"
					}
				]
			},
			{
				"featureType": "poi.park",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#6b9a76"
					}
				]
			},
			{
				"featureType": "road",
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#38414e"
					}
				]
			},
			{
				"featureType": "road",
				"elementType": "geometry.stroke",
				"stylers": [
					{
						"color": "#212a37"
					}
				]
			},
			{
				"featureType": "road",
				"elementType": "labels.icon",
				"stylers": [
					{
						"visibility": "off"
					}
				]
			},
			{
				"featureType": "road",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#9ca5b3"
					}
				]
			},
			{
				"featureType": "road.highway",
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#746855"
					}
				]
			},
			{
				"featureType": "road.highway",
				"elementType": "geometry.stroke",
				"stylers": [
					{
						"color": "#1f2835"
					}
				]
			},
			{
				"featureType": "road.highway",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#f3d19c"
					}
				]
			},
			{
				"featureType": "transit",
				"stylers": [
					{
						"visibility": "off"
					}
				]
			},
			{
				"featureType": "transit",
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#2f3948"
					}
				]
			},
			{
				"featureType": "transit.station",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#d59563"
					}
				]
			},
			{
				"featureType": "water",
				"elementType": "geometry",
				"stylers": [
					{
						"color": "#17263c"
					}
				]
			},
			{
				"featureType": "water",
				"elementType": "labels.text.fill",
				"stylers": [
					{
						"color": "#515c6d"
					}
				]
			},
			{
				"featureType": "water",
				"elementType": "labels.text.stroke",
				"stylers": [
					{
						"color": "#17263c"
					}
				]
			}
		]
    };
	
    map = new google.maps.Map(document.getElementById("googleMap"), mapProp);

    $(MODEL.Depots).each(function (i) {
        LiveMap.DEPOTS.push(new customMarker(
            new google.maps.LatLng(this.Latitude, this.Longitude),
            map,
            {
                marker_id: 'Depot-' + this.ID,
                htmlContent: iconBuilder.depot(this.Status)
            }
        ));  
    });

    $(MODEL.CapturePoints).each(function (i) {
        LiveMap.CAPTUREPOINTS.push(new customMarker(
            new google.maps.LatLng(this.Latitude, this.Longitude),
            map,
            {
                marker_id: 'CP-' + this.ID,
                htmlContent: iconBuilder.create(this.Status, this.Type)
            }
        ));
    });

    $(MODEL.Missions).each(function (i) {
        LiveMap.SIDEMISSIONS.push(new customMarker(
            new google.maps.LatLng(this.Latitude, this.Longitude),
            map,
            {
                marker_id: 'SM-' + this.ID,
                htmlContent: iconBuilder.mission(this.IconClass)
            }
        ));
    });




    google.maps.event.addListenerOnce(map, 'tilesloaded', postInitGoogleMap);

    function postInitGoogleMap() {
        $(MODEL.Depots).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('Depot', this.ID);
            KI.tooltipster($dataSel);

            var arraytable = LiveMap.splitStringIntoArrayTable(this.ResourceString, "\n", "|");
            this.ResourceHtml = LiveMap.generateResourceTable(arraytable);
            if (this.Capacity === -1 || this.Capacity === "-1")
            {
                this.Capacity = 'infinite';
                this.CurrentCapacity = 'infinite';
            }

            LiveMap.updateTooltipContent($dataSel, LiveMap.DepotTemplate, this);
        });

        $(MODEL.CapturePoints).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('CP', this.ID);
            KI.tooltipster($dataSel);
            LiveMap.updateTooltipContent($dataSel, LiveMap.CPTemplate, this);
        });

        $(MODEL.Missions).each(function (i) {
            var $dataSel = LiveMap.createMarkerSelector('SM', this.ID);
            this.TimeRemaining = LiveMap.convertSecondsToTimeString(this.TimeRemaining);
            KI.tooltipster($dataSel);
            LiveMap.updateTooltipContent($dataSel, LiveMap.SMTemplate, this);
        });
        
    }
	
}
