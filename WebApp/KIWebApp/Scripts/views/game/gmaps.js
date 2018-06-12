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

			var panes = this.getPanes();
			panes.overlayImage.appendChild(div);
		}

		var point = this.getProjection().fromLatLngToDivPixel(this.latlng);

		//position the custom marker on the map
		if (point) {
			div.style.left = (point.x) + 'px';
			div.style.top = (point.y) + 'px';
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
	
	overlay = new customMarker(
		new google.maps.LatLng(43.582658,39.763357 ),
		map,
		{
			marker_id: '1',
			htmlContent: iconBuilder.airport(2),
			tooltip: 'marker tooltp'
		}
	);

	overlay2 = new customMarker(
		new google.maps.LatLng(43.782658,39.763357 ),
		map,
		{
			marker_id: '2',
			htmlContent: iconBuilder.capturePoint(1),
			tooltip: 'marker tooltp'
		}
	);

	overlay3 = new customMarker(
		new google.maps.LatLng(43.712098,39.744357 ),
		map,
		{
			marker_id: '3',
			htmlContent: iconBuilder.farp(1),
			tooltip: 'marker tooltp'
		}
	);

	overlay4 = new customMarker(
		new google.maps.LatLng(43.822798,39.724397 ),
		map,
		{
			marker_id: '4',
			htmlContent: iconBuilder.depot(0),
			tooltip: 'marker tooltp'
		}
	);

	// overlay.args.marker_id;

	
}
