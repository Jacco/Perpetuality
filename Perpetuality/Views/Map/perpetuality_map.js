/**
 * TODO Bottom to switch map overlay,
 *      top for status info,
 *      left for detailed info on selected power plant,
 *      right for dragging new plants on the map.
 */
var perpetuality = perpetuality || {};

perpetuality.map = perpetuality.map || function() {};

perpetuality.map.prototype.init = function() {
  var mapOptions = {
    center: new google.maps.LatLng(42.34941, -71.056137),
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
  };
  var map = new google.maps.Map(document.getElementById("map"), mapOptions);
};

// Keep at the end. Until the code is integrated with jQuery.
window.onload = function() { (new perpetuality.map).init(); };

