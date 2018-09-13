var latlng, userLat, userLng;
window.onload = function () {
        document.getElementById("submit").disabled = true;
        if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showMap, showError);
        }
        else {
                document.getElementById("msg").children['text'].textContent = "Location Service is not supported by current browser.";
        }
}
function showMap(position) {
        latlng = position.coords.latitude + ', ' + position.coords.longitude;
        userLat = position.coords.latitude;
        userLng = position.coords.longitude;
        var address;
        var map = new google.maps.Map(document.getElementById('map'), {
                center: new google.maps.LatLng(position.coords.latitude, position.coords.longitude),
                mapTypeId: 'roadmap',
                mapTypeControl: false,
                scaleControl: true,
                streetViewControl: false,
                fullscreenControl: false,
                zoom: 11
        });
        var input = document.getElementById('search');
        var autocomplete = new google.maps.places.Autocomplete(input, { placeIdOnly: true });
        autocomplete.bindTo('bounds', map);
        map.controls[google.maps.ControlPosition.TOP_LEFT].push(input);
        var infowindow = new google.maps.InfoWindow();
        var infowindowContent = document.getElementById('infowindow-content');
        infowindow.setContent(infowindowContent);
        var geocoder = new google.maps.Geocoder;
        var markers = new Array();
        var marker = createMarker(infowindow, infowindowContent, position.coords.latitude, position.coords.longitude, address, map);
        document.getElementById("goBack").addEventListener("click", function () {
                address=null;
                var useLocation = new google.maps.LatLng(userLat, userLng)
                map.setZoom(11);
                map.setCenter(useLocation);
                markers.forEach(element => {
                        element.setMap(null);
                });
                latlng = userLat + ', ' + userLng;
                console.log(latlng);
                var userLocMarker = createMarker(infowindow, infowindowContent, userLat, userLng, address, map);
                markers.push(userLocMarker);
                input.value = "";
        });
        autocomplete.addListener('place_changed', function () {
                infowindow.close();
                var place = autocomplete.getPlace();
                if (!place.place_id) {
                        return;
                }
                markers.forEach(element => {
                        element.setMap(null);
                });
                geocoder.geocode({ 'placeId': place.place_id }, function (results, status) {
                        if (status !== 'OK') {
                                window.alert('Failed due to: ' + status);
                                return;
                        }
                        address = results[0].formatted_address;
                        var location = results[0].geometry.location;
                        latLng = location.toString().substr(1, location.toString().indexOf(')') - 1);
                        map.setZoom(11);
                        map.setCenter(location);
                        var position = latLng.toString().replace(/[(^\s+)(\s+$)]/g, "").replace('，', ',').split(',');
                        var lat = parseFloat(position[0]);
                        var lng = parseFloat(position[1]);
                        var newMarker = createMarker(infowindow, infowindowContent, lat, lng, address, map);
                        markers = new Array();
                        markers.push(newMarker);
                });
        });
        document.getElementById("submit").disabled = false;
        document.getElementById("msg").style.visibility = "hidden";
}
function showError(error) {
        var msg = document.getElementById("msg");
        switch (error.code) {
                case error.PERMISSION_DENIED:
                        msg.innerHTML = "User denied the request for Location information."
                        break;
                case error.POSITION_UNAVAILABLE:
                        msg.innerHTML = "Location service is unavailable."
                        break;
                case error.TIMEOUT:
                        msg.innerHTML = "The request for location timed out."
                        break;
                case error.UNKNOWN_ERROR:
                        msg.innerHTML = "An unknown error occurred."
                        break;
        }
}
function onClick() {
        console.log(latlng);
        try {
                window.top.postMessage(latlng, '*');
        }
        catch (error) {
                alert("An error occurred.");
        }
}
//解析经纬度
function toAddress(latlng, infowindowContent) {
        var location = latlng.toString().replace(/[(^\s+)(\s+$)]/g, "").replace('，', ',').split(',');
        var lat = parseFloat(location[0]);
        var lng = parseFloat(location[1]);
        var geocoder = new google.maps.Geocoder()
        geocoder.geocode({
                //传入经纬度
                'location': new google.maps.LatLng(lat, lng)
        }, function (results, status) {
                var text = "Cannot get current position's address.";
                if (status == google.maps.GeocoderStatus.OK) {
                        text = results[0].formatted_address;
                }
                infowindowContent.children['place-address'].textContent = text;
        });
}

function createMarker(infowindow, infowindowContent, lat, lng, address, map) {
        var marker = new google.maps.Marker({
                map: map,
                draggable: true,
                position: new google.maps.LatLng(lat, lng)
        });
        google.maps.event.addListener(marker, 'click', function () {     
                if (address != null) {
                        console.log("addresss "+ address);
                        infowindowContent.children['place-address'].textContent = address;
                }
                else {
                        console.log("latlng "+ latlng);
                        infowindowContent.children['place-address'].textContent = "";
                        toAddress(latlng, infowindowContent);
                }
                infowindow.open(map, marker);
        });
        google.maps.event.addListener(marker, 'dragend', function (MouseEvent) {
                var temp = MouseEvent.latLng.toString();
                latlng = temp.substr(1, temp.indexOf(')') - 1);
                address =null;
                console.log(latlng);
                infowindow.close();
        });
        return marker;
}