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
    mapTypeId: google.maps.MapTypeId.ROADMAP,
    zoomControlOptions: {
      style: google.maps.ZoomControlStyle.SMALL
    },
    streetViewControl: false
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
        var image = document.createElement("img");
        image.src = content.image;
        var imageClass = content.imageClass;
        var imageSize = content.imageSize;
        if (imageSize) {
            if (imageSize.width) { image.width = imageSize.width }
            if (imageSize.height) { image.height = imageSize.height }
        }
        $(image).click(content.action);

        return image;
    };

    var pane = document.createElement("div");
    pane.id = name + "-pane";
    $(pane).addClass("map-pane");
    if (extraClass != undefined) {
        $(pane).addClass(extraClass);
    }
    for (var i = 0; i < contentList.length; i++) {
        var content = contentList[i];
        var contentDiv = document.createElement("div");
        $(contentDiv).addClass("map-pane-item");
        var itemExtraClass = content.itemExtraClass;
        if (itemExtraClass) {
            $(contentDiv).addClass(itemExtraClass);
        }
        var contentPane = new perpetuality.map.ItemizedPaneItem();
        contentPane.title = content.title;
        contentPane.action = content.action;
        var title = document.createElement("div");
        title.innerHTML = content.title;
        if (content.image) {
            var image = createImage(content);
            contentPane.image = image;
            contentDiv.appendChild(image);
        }
        else if (content.button) {
            var button = document.createElement("div");
            button.id = content.button;
            var jButton = $(button);
            jButton.click(content.action);
            jButton.addClass("plantbutton");
            contentDiv.appendChild(button);
        }
        contentDiv.appendChild(title);
        pane.appendChild(contentDiv);
    }
  return pane;
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
//  map.dataServer = "http://localhost:3000" Change to proper address.
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


    /**
     * Panes.
     */
  var overlayControlPane = map.makeItemizedPane("overlay", [{
      "title": "HeatMap",
      action: function() {
        map.heatMap.setMap(map.heatMap.getMap() ? null : map.root);
      },
      "image": "/Content/Images/default-image.jpeg",
      "itemExtraClass": "map-pane-item-horizontal"
  }], "map-pane-bottom");

  var statusPane = map.makeItemizedPane("status", [
    {
        "title": getToday(),
        "image": "/Content/Images/original/logo2.png",
        "itemExtraClass": "map-pane-item-horizontal"
    },
    {
        "title": "KwH",
        "image": "/Content/Images/orange.png",
        "imageSize": { "width": 16 },
        "itemExtraClass": "map-pane-item-horizontal"
    },
    {
        title: "Credits",
        image: "/Content/Images/red.png",
        imageSize: { width: 16 },
        itemExtraClass: "map-pane-item-horizontal"
    },
  ], "map-pane-top");

  var detailPane = map.makeTextPane("detail", [{
      "title": "Detail Title",
      "content": "Detail Contents."
  }], "map-pane-left");

  var nummer = 0;
  var spritePane = map.makeItemizedPane("sprite", [
    {
        title: "Solar Panels",
        button: "solarroofbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.Plant.placePlant(++nummer * 50, nummer * 50) }
    },
    {
        title: "Solar Field",
        button: "solarfieldbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.Plant.placePlant(++nummer * 50, nummer * 50) }
    },
    {
        title: "Solar Plant",
        button: "solartowerbutton",
        itemExtraClass: "map-pane-item-vertical",
        action: function (e) { perpetuality.Plant.placePlant(++nummer * 50, nummer * 50) }
    }
  ], "map-pane-right");

    /**
     * Layout.
     */
  var config = {
      "panes": [
        {
            "name": "overlay",
            "position": google.maps.ControlPosition.BOTTOM_CENTER,
            "pane": overlayControlPane
        },
        {
            "name": "status",
            "position": google.maps.ControlPosition.TOP_LEFT,
            "pane": statusPane
        },
        {
            "name": "detail",
            "position": google.maps.ControlPosition.LEFT_CENTER,
            "pane": detailPane
        },
        {
            "name": "sprite",
            "position": google.maps.ControlPosition.RIGHT_CENTER,
            "pane": spritePane
        }
      ]
  };
  $.each(config["panes"], function (key, value) {
      map.registerPane(value["name"], value["position"], value["pane"]);
  });

});
