/**
 */
var perpetuality = perpetuality || {};

/**
 * Map
 */
perpetuality.map = perpetuality.map || function() {
  this.root = {};
  this.panes = {};
  this.overlays = [];
  this.sprites = [];
};

perpetuality.map.prototype.init = function() {
  var mapOptions = {
    center: new google.maps.LatLng(42.34941, -71.056137),
    zoom: 12,
    mapTypeId: google.maps.MapTypeId.ROADMAP
  };
  this.root = new google.maps.Map(document.getElementById("map"), mapOptions);
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

perpetuality.map.prototype.makePane = function(name, contentList, extraClass) {
  var pane = document.createElement("div");
  pane.id = name + "-pane";
  $(pane).addClass("pane");
  if (extraClass != undefined) {
    $(pane).addClass(extraClass);
  }
  for (var i = 0; i < contentList.length; i++) {
    var contentDiv = document.createElement("div");
    var content = new perpetuality.map.PaneItem();
    content.title = contentList[i].title;
    content.action = contentList[i].action;
    var title = document.createElement("div");
    title.innerHTML = content.title;
    var image = document.createElement("img");
    image.src = contentList[i].image;
    $(image).click(content.action);
    content.image = image;
    contentDiv.appendChild(image);
    contentDiv.appendChild(title);
    pane.appendChild(contentDiv);
  }
  return pane;
};

/**
 * Pane Contents.
 */
perpetuality.map.PaneItem = function() {
  this.image = undefined;
  this.title = "Title";
  this.action = function() {};
};

/**
 * Application's map.
 */
$(document).ready(function() {
  var map = new perpetuality.map;
  map.init();

  /**
   * Panes.
   */
  var overlayControlPane = map.makePane("overlay", [{
    "title": "HeatMap",
    "action": function() { alert("HeatMap") },
    "image": "images/default-image.jpeg"
  }], "bottom");

  var spritePane = map.makePane("sprite", [{
    "title": "Plant",
    "action": function() { alert("Plant action") },
    "image": "images/default-image.jpeg"
  }], "right");

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
    /**
      {
        "name": "status",
        "position": google.maps.ControlPosition.TOP_CENTER,
        "pane": statusPane
      },
      {
        "name": "detail",
        "position": google.maps.ControlPosition.LEFT_CENTER,
        "pane": detailPane
      },
      */
      {
        "name": "sprite",
        "position": google.maps.ControlPosition.RIGHT_CENTER,
        "pane": spritePane
      }
    ]
  };
  $.each(config["panes"], function(key, value) {
    map.registerPane(value["name"], value["position"], value["pane"]);
  });

});

