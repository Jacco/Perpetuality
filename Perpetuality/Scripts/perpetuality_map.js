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
 * Pane management.
 */
perpetuality.map.prototype.registerPane = function(name, position, pane) {
  if (this.panes[name] == undefined) {
    this.panes[name] = pane;
    this.root.controls[position].push(pane);
  }
};

perpetuality.map.prototype.deRegisterPane = function(name) {
  delete this.panes[name];
};

perpetuality.map.prototype.makeItemizedPane = function (name, contentList, extraClass) {
    function createImage(content) {
        var image = $("<img />", { src: content.image });
        var imageSize = content.imageSize;
        if (imageSize) {
            if (imageSize.width) { image.width(imageSize.width) }
            if (imageSize.height) { image.height(imageSize.height) }
        }
        image.click(content.action);

        return image;
    };

    var pane = $("<div />", { id: name + "-pane", "class": "map-pane" });
    if (extraClass) {
        $(pane).addClass(extraClass);
    }
    for (var i = 0; i < contentList.length; i++) {
        var content = contentList[i];
        var contentDiv = $("<div />", { "class": "map-pane-item" });
        if ("itemExtraClass" in content) {
            $(contentDiv).addClass(content.itemExtraClass);
        }

        var contentPane = new perpetuality.map.ItemizedPaneItem();
        contentPane.title = content.title;
        contentPane.action = content.action;

        if (content.image) {
            var image = createImage(content);
            contentPane.image = image;
            contentDiv.append(image);
        }
        else if (content.button) {
            var button = $("<div />", { id: content.button, "class": "plantbutton" });
            button.click(content.action);
            contentDiv.append(button);
        }
        else if (content.content) {
            var div = $("<div />", { id: content.contentId, "data-bind": content.db });
            div.append(content.content);
            contentDiv.append(div);
        }

        contentDiv.append("<div>" + content.title + "</div>");
        pane.append(contentDiv);
    }
  return pane[0];
};

perpetuality.map.prototype.makeTextPane = function(name, contentList, extraClass) {
  var pane = document.createElement("div");
  pane.id = name + "-pane";
  $(pane).addClass("map-pane");
  if (extraClass != undefined) {
    $(pane).addClass(extraClass);
  }
  for (var i = 0; i < contentList.length; i++) {
    var contentDiv = document.createElement("div");
    var section = new perpetuality.map.TextPaneItem();
    var title = contentList[i].title;
    if (title != undefined) {
      section.title = title;
      var titleDiv = document.createElement("div");
      titleDiv.innerHTML = section.title;
      $(titleDiv).addClass("map-section-title");
      contentDiv.appendChild(titleDiv);
    }
    section.content = contentList[i].content;
    var textDiv = document.createElement("div");
    textDiv.innerHTML = section.content;
    $(textDiv).addClass("map-section-text");
    contentDiv.appendChild(textDiv);
    pane.appendChild(contentDiv);
  }
  return pane;
};

/**
 * Pane Contents.
 */
perpetuality.map.ItemizedPaneItem = function() {
  this.image = undefined;
  this.title = "Title";
  this.action = function() {};
};

perpetuality.map.TextPaneItem = function() {
  this.title = "Title";
  this.content = "Content";
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

    /**
     * Panes.
     */
  var overlayControlPane = map.makeItemizedPane("overlay", [{
      title: "Geothermal",
      action: function() {
        map.heatMap.setMap(map.heatMap.getMap() ? null : map.root);
      },
      image: "/Content/Images/button-geothermal-overlay.png",
      itemExtraClass: "map-pane-item-horizontal"
  }, {
      title: "Solar",
      action: function () {
          map.heatMap.setMap(map.heatMap.getMap() ? null : map.root);
      },
      image: "/Content/Images/button-solar-overlay.png",
      itemExtraClass: "map-pane-item-horizontal"
  }, {
      title: "Water",
      action: function () {
          map.heatMap.setMap(map.heatMap.getMap() ? null : map.root);
      },
      image: "/Content/Images/button-water-overlay.png",
      itemExtraClass: "map-pane-item-horizontal"
  }, {
      title: "Wind",
      action: function () {
          map.heatMap.setMap(map.heatMap.getMap() ? null : map.root);
      },
      image: "/Content/Images/button-wind-overlay.png",
      itemExtraClass: "map-pane-item-horizontal"
  }], "map-pane-bottom");

  //var statusPane = map.makeItemizedPane("status", [
  //  {
  //      title: getToday(),
  //      image: "/Content/Images/original/logo2.png",
  //      itemExtraClass: "map-pane-item-horizontal"
  //  },
  //  {
  //      title: "KwH",
  //      content: "0",
  //      db: "powertext",
  //      itemExtraClass: "map-pane-item-horizontal"
  //  },
  //  {
  //      title: "Credits",
  //      content: "0",
  //      db: "creditstext",
  //      itemExtraClass: "map-pane-item-horizontal"
  //  },
  //], "map-pane-top");

  var detailPane = map.makeTextPane("detail", [{
      title: "Detail Title",
      content: "Detail Contents."
  }], "map-pane-left");

  var nummer = 0;
  var spritePane = map.makeItemizedPane("sprite", [
    {
        title: "Plant cost",
        content: "None",
        contentId: "plantcost"
    },
    {
        title: "Plant size",
        content: "None",
        contentId: "plantsize"
    },
    {
        title: "Solar Panels",
        button: "solarroofbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.plant.placePlant(map, new perpetuality.plant("solarroof", 3000, 4, 2)) }
    },
    {
        title: "Solar Field",
        button: "solarfieldbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.plant.placePlant(map, new perpetuality.plant("solarfield", 200000, 2500, 160)) }
    },
    {
        title: "Solar Plant",
        button: "solartowerbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.plant.placePlant(map, new perpetuality.plant("solartower", 45000000, 785398, 2000)) }
    },
    {
        title: "Persist Plant",
        button: "deselectbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) {}
    }
  ], "map-pane-right");

    /**
     * Layout.
     */
  var config = {
      panes: [
        {
            name: "overlay",
            position: google.maps.ControlPosition.BOTTOM_CENTER,
            pane: overlayControlPane
        },
        //{
        //    name: "status",
        //    position: google.maps.ControlPosition.TOP_LEFT,
        //    pane: statusPane
        //},
        {
            name: "detail",
            position: google.maps.ControlPosition.LEFT_CENTER,
            pane: detailPane
        },
        {
            name: "sprite",
            position: google.maps.ControlPosition.RIGHT_CENTER,
            pane: spritePane
        }
      ]
  };
  $.each(config.panes, function (key, pane) {
      map.registerPane(pane.name, pane.position, pane.pane);
  });

  perpetuality.state = new StateModel();

  ko.applyBindings(perpetuality.state);

  timer.start();
});
