function GetOrder() {
    $.ajax({
        dataType: "json",
        cache: false,
        url: "/Driver/GetAjax",
        success: function (data) {
            if (!data) return;
            calcRoute(data[0].AdressFrom, data[0].AdressWhere);
        }
    });
}


setInterval(GetOrder(),10000);


var directionsService = new google.maps.DirectionsService();

function initialize() {
    directionsDisplay = new google.maps.DirectionsRenderer();
    var manhattan = new google.maps.LatLng(49.422983, 26.987133);
    var mapOptions = {
        zoom: 14,
        center: manhattan
    };
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    directionsDisplay.setMap(map);
}

function calcRoute(start, end) {
    var waypts = [];
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var pos = new google.maps.LatLng(position.coords.latitude,
                                             position.coords.longitude);
            var latlng = new google.maps.LatLng(pos.A, pos.F);
            waypts.push({
                location: start,
                stopover: true
            });
            var request = {
                origin: latlng,
                destination: end,
                waypoints: waypts,
                optimizeWaypoints: true,
                travelMode: google.maps.TravelMode.DRIVING
            };
            directionsService.route(request, function (response, status) {
                if (status == google.maps.DirectionsStatus.OK) {
                    directionsDisplay.setDirections(response);
                }
            });
            map.setCenter(latlng);
        });
    } 
}

google.maps.event.addDomListener(window, 'load', initialize);

