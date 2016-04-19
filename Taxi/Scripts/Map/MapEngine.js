var map;
var directionsDisplay;
var directionsService;
var stepDisplay;
var markerArray = [];

var placeSearch, autocomplete, autocompleteWhere;
var componentForm = {
    street_number: 'short_name',
    route: 'long_name',
    locality: 'long_name',
    administrative_area_level_1: 'short_name',
    country: 'long_name',
    postal_code: 'short_name'
};



function initialize() {
    // Instantiate a directions service.
    autocomplete = new google.maps.places.Autocomplete(
      (document.getElementById('start')),
      { types: ['geocode'] });
    autocompleteWhere = new google.maps.places.Autocomplete(
      (document.getElementById('end')),
      { types: ['geocode'] });
    directionsService = new google.maps.DirectionsService();

    // Create a map and center it on Manhattan.
    var manhattan = new google.maps.LatLng(49.422983, 26.987133);
    var mapOptions = {
        zoom: 14,
        center: manhattan
    };
    map = new google.maps.Map(document.getElementById('map-canvas'), mapOptions);

    // Create a renderer for directions and bind it to the map.
    var rendererOptions = {
        map: map
    };
    directionsDisplay = new google.maps.DirectionsRenderer(rendererOptions);

    // Instantiate an info window to hold step text.
    stepDisplay = new google.maps.InfoWindow();
    google.maps.event.addListener(autocomplete, 'place_changed', function () {
        calcRoute();
    });
    google.maps.event.addListener(autocompleteWhere, 'place_changed', function () {
        calcRoute();
    });
}

// [END region_fillform]

function geolocate() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function (position) {
            var geolocation = new google.maps.LatLng(
                position.coords.latitude, position.coords.longitude);
            var circle = new google.maps.Circle({
                center: geolocation,
                radius: position.coords.accuracy
            });
            autocomplete.setBounds(circle.getBounds());
        });
    }
}

function calcRoute() {

    // First, remove any existing markers from the map.
    for (var i = 0; i < markerArray.length; i++) {
        markerArray[i].setMap(null);
    }

    // Now, clear the array itself.
    markerArray = [];

    // Retrieve the start and end locations and create
    // a DirectionsRequest using WALKING directions.
    var start = document.getElementById('start').value;
    var end = document.getElementById('end').value;
    if (start === "" || end === "") return;

    GetDriversLoc();

    var request = {
        origin: start,
        destination: end,
        travelMode: google.maps.TravelMode.DRIVING
    };

    // Route the directions and pass the response to a
    // function to create markers for each step.
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            var warnings = document.getElementById('warnings_panel');
            warnings.innerHTML = '<b>' + response.routes[0].warnings + '</b>';
            directionsDisplay.setDirections(response);
            showSteps(response);
            google.maps.geometry.spherical.computeDistanceBetween(start, end);
        }
    });
}

function GetDriversLoc() {
    $.ajax({
        async: false,
        dataType: "json",
        cache: false,
        url: "/Dispetcher/GetAjax", success: function (data) {
            var distances = [];
            for (var i = 0; i < data.length; i++) {
                chooseDriver(data[i], function (dist, id) {
                    distances.push({ id: id, dist: dist });
                    if (distances.length === data.length) {
                        $("#driver").val(_.min(distances, function (item) {
                            return item.dist;
                        }).id);
                    }
                });
            }
        }
    });
}

function chooseDriver(start2, callback) {
    var end = document.getElementById('start').value;
    var request = {
        origin: start2.location,
        destination: end,
        travelMode: google.maps.TravelMode.DRIVING
    };
    directionsService.route(request, function (response, status) {
        if (status == google.maps.DirectionsStatus.OK) {
            var warnings = document.getElementById('warnings_panel');
            warnings.innerHTML = '<b>' + response.routes[0].warnings + '</b>';
            directionsDisplay.setDirections(response);
            callback(showSteps2(response), start2.id);
        }
    });
}

function showSteps2(directionResult) {
    var myRoute = directionResult.routes[0].legs[0];
    return myRoute.distance.value;
}

function showSteps(directionResult) {
    // For each step, place a marker, and add the text to the marker's
    // info window. Also attach the marker to an array so we
    // can keep track of it and remove it when calculating new
    // routes.
    var myRoute = directionResult.routes[0].legs[0];

    $("#Kilometrage").val(myRoute.distance.value);

    for (var i = 0; i < myRoute.steps.length; i++) {
        var marker = new google.maps.Marker({
            position: myRoute.steps[i].start_location,
            map: map
        });
        attachInstructionText(marker, myRoute.steps[i].instructions);
        markerArray[i] = marker;
    }
}

function attachInstructionText(marker, text) {
    google.maps.event.addListener(marker, 'click', function () {
        // Open an info window when the marker is clicked on,
        // containing the text of the step.
        stepDisplay.setContent(text);
        stepDisplay.open(map, marker);
    });
}

google.maps.event.addDomListener(window, 'load', initialize);

