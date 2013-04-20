/**
 * TODO Bottom to switch map overlay,
 *      top for status info,
 *      left for detailed info on selected power plant,
 *      right for dragging new plants on the map.
 */
function initialize() {
  var mapOptions = {
    center: new google.maps.LatLng(42.34941, -71.056137),
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
  };
  var map = new google.maps.Map(document.getElementById("map"), mapOptions);
}
//google.maps.event.addDomListener(window, 'load', initialize);

function loadScript() {
  var script = document.createElement("script");
  script.type = "text/javascript";
  script.src = "https://maps.googleapis.com/maps/api/js?key=AIzaSyDxynbvhnTHYl8nIIAoct1AjbJCxWZSwPw&sensor=false&callback=initialize";
  document.body.appendChild(script);
}

window.onload = loadScript;

