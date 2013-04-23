/**
 */
var perpetuality = perpetuality || {};

/**
 * Map for Perpetuality.
 *
 * Support for heatmaps if a server can deliver arrays of objects as:
 *   {
 *   "lat": 35.123,
 *   "lon": 139.123,
 *   "weight": 123
 *   }
 * The client here attempts to connect to /api/dataset---easy to change.
 * Query parameters are a pair of ranges:
 *   {
 *   "lat_r": [ 35.1, 36.2 ],
 *   "lon_r": [ 135.1, 136.2 ],
 *   }
 */
perpetuality.map = perpetuality.map || function () {
  this.root = {};
  this.dataServer = window.location.protocol + "//" + window.location.host;
  this.panes = {};
  this.heatMapCache = {};
};

perpetuality.map.prototype.buildMap = function(options) {
  return new google.maps.Map(document.getElementById("map"), options || {});
};

perpetuality.map.prototype.init = function() {
    var mapOptions = {
        center: new google.maps.LatLng(37.0625, -95.677068), // Boston
        zoom: 14,
        mapTypeId: google.maps.MapTypeId.SATELLITE,
        zoomControlOptions: {
            style: google.maps.ZoomControlStyle.SMALL
        },
        mapTypeControl: false,
        streetViewControl: false,
        overviewMapControl: true,
        overviewMapControlOptions: {
            opened: true
        }
  };
  var map = this.buildMap(mapOptions);

  var me = this;
  var updateMap = function() {
    var targetURL = me.dataServer + "/api/dataset";
    var northEast = this.getBounds().getNorthEast();
    var southWest = this.getBounds().getSouthWest();
    var data = {
      lon_r: [ southWest.lng(), northEast.lng() ],
      lat_r: [ southWest.lat(), northEast.lat() ],
    };
    if (me.dataServer != (window.location.protocol + "//" + window.location.host)) {
      $.ajax(targetURL, {
        data: data,
        dataType: "jsonp",
        success: function(data) {
          me.buildHeatMapLayer(data);
        }
      });
    } else {
      $.ajax(targetURL, {
        data: data,
        dataType: "json",
        timeout: 5,
        success: function(data, textStatus, jqXHR) {
          me.buildHeatMapLayer(data);
        },
        error: function(jqXHR, textStatus, errorThrown) {
            alert("failed to update heat map data!");
        }
      });
    }
  };
  google.maps.event.addListenerOnce(map, 'center_changed', updateMap);
  google.maps.event.addListener(map, 'dragend', updateMap);
  google.maps.event.addListener(map, 'zoom_changed', updateMap);

  this.root = map;
};

/**
 * Layers.
 */
perpetuality.map.prototype.buildHeatMapLayer = function (data) {
    var me = this;
    if (this.heatMap) {
        me.heatMap.setMap(null);
    }
    var zoom = "" + me.root.getZoom();
    var center = "(" + me.root.getCenter().lat() + "," + me.root.getCenter().lng() + ")";
    var dataSet = [];
    if (me.heatMapCache[zoom] == undefined || me.heatMapCache[zoom][center] == undefined) {
        $(data).each(function (idx, d) {
            dataSet.push({
                location: new google.maps.LatLng(d.lat, d.lon),
                weight: d.weight
            });
        });
    }

    if (me.heatMapCache[zoom] == undefined) {
        me.heatMapCache[zoom] = {};
    }

    if (me.heatMapCache[zoom][center] == undefined) {
        var heatmap = undefined;
        heatmap = new google.maps.visualization.HeatmapLayer({
            data: dataSet
        });
        heatmap.setOptions({
            radius: $("#map").width() / 20
        });
        me.heatMapCache[zoom][center] = heatmap;
    } else {
        heatmap = me.heatMapCache[zoom][center];
    }
    heatmap.setMap(this.root);
    me.heatMap = heatmap;
};

/**
 * Timer
 */

function Timer(settings) {
    this.settings = settings;
    this.timer = null;

    this.fps = settings.fps || 30;
    this.interval = Math.floor(1000 / this.fps);
    this.timeInit = null;

    return this;
}

Timer.prototype =
{
    run: function () {
        var $this = this;

        this.settings.run();
        this.timeInit += this.interval;

        this.timer = setTimeout(
            function () { $this.run() },
            this.timeInit - (new Date).getTime()
        );
    },

    start: function () {
        if (this.timer == null) {
            this.timeInit = (new Date).getTime();
            this.run();
        }
    },

    stop: function () {
        clearTimeout(this.timer);
        this.timer = null;
    }
}


/**
 * Application's map.
 */
$(document).ready(function () {
    function getToday() {
        // Should come from the game
        var today = new Date();
        return today.getDate() + "-" + today.getMonth() + 1 + "-" + today.getFullYear();
    };

  var map = new perpetuality.map;
  map.init();
  map.dataServer = "http://api.perpetuality.org/"

  /**
   * Use current location if available.
   */
  if(navigator.geolocation) {
    navigator.geolocation.getCurrentPosition(function(position) {
      var center = new google.maps.LatLng(
        position.coords.latitude,
        position.coords.longitude
      );
      map.root.setCenter(center);
    }, function() {});
  } else {
    // TODO Need to generate the layers?
  }

  var timer = new Timer({
      fps: 1,
      run: function () {
          var tm = perpetuality.state.time().getTime();
          tm = tm + 365000;
          var dt = new Date();
          dt.setTime(tm);
          perpetuality.state.time(dt);

          perpetuality.state.credits(perpetuality.state.credits() + 365 * perpetuality.state.creditProduction());
      }
  });

  var nummer = 0;

  perpetuality.state = new perpetuality.state.StateModel();
  perpetuality.state.map = map;
  ko.applyBindings(perpetuality.state);

  timer.start();

  google.maps.event.addListener(map.root, 'click', function (event) {
      perpetuality.state.addPlant(event);
  });

});
