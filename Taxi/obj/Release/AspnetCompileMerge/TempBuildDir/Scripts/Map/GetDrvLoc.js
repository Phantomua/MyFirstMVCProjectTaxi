function codeLatLng() {
    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
                var geocoder = new google.maps.Geocoder();
                var pos = new google.maps.LatLng(position.coords.latitude,
                    position.coords.longitude);
                var latlng = new google.maps.LatLng(pos.A, pos.F);

                geocoder.geocode({ 'latLng': latlng }, function(results, status) {
                    if (status == google.maps.GeocoderStatus.OK) {
                        if (results[1]) {
                            $.ajax({
                                async: false,
                                dataType: "json",
                                cache: false,
                                url: "/Driver/GetLocation",
                                processData: false,
                                data: "loc=" + results[1].formatted_address,
                                success: function () {
                                }
                            });
                        } 
                    } 
                });
            }
        );
    };
}
