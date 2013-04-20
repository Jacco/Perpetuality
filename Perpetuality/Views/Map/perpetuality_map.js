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

perpetuality.map.prototype.makeOverlayControlPane = function(overlayList, extraClass) {
  var pane = document.createElement("div");
  pane.id = "overlay-pane";
  $(pane).addClass("pane");
  if (extraClass != undefined) {
    $(pane).addClass(extraClass);
  }
  for (var i = 0; i < overlayList.length; i++) {
    var overlayDiv = document.createElement("div");
    var overlay = new perpetuality.map.OverlayControl();
    overlay.title = overlayList[i].title || "Title";
    overlay.action = overlayList[i].action || function() {};
    var title = document.createElement("div");
    title.innerHTML = overlay.title;
    var image = document.createElement("img");
    image.src = overlayList[i].image || "images/default-image.jpeg";
    $(image).click(overlay.action);
    overlay.image = image;
    overlayDiv.appendChild(image);
    overlayDiv.appendChild(title);
    this.overlays.push(overlay);
    pane.appendChild(overlayDiv);
  }
  return pane;
};

perpetuality.map.prototype.makeSpritePane = function(spriteList, extraClass) {
  var pane = document.createElement("div");
  pane.id = "sprite-pane";
  $(pane).addClass("pane");
  if (extraClass != undefined) {
    $(pane).addClass(extraClass);
  }
  for (var i = 0; i < spriteList.length; i++) {
    var spriteDiv = document.createElement("div");
    var sprite = new perpetuality.map.Sprite();
    sprite.title = spriteList[i].title || "Object";
    sprite.action = spriteList[i].action || function() {};
    var title = document.createElement("div");
    title.innerHTML = sprite.title;
    var image = document.createElement("img");
    image.src = spriteList[i].image || "images/default-image.jpeg";
    $(image).click(sprite.action);
    sprite.image = image;
    spriteDiv.appendChild(image);
    spriteDiv.appendChild(title);
    this.sprites.push(sprite);
    pane.appendChild(spriteDiv);
  }
  return pane;
}

/**
 * Pane Contents.
 */
perpetuality.map.OverlayControl = function() {
  this.image = "";
  this.title = "Title";
  this.action = function() {};
};

perpetuality.map.Sprite = function() {
  this.image = "";
  this.title = "Object";
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
  var overlayControlPane = map.makeOverlayControlPane([{
    "title": "HeatMap",
    "action": function() { alert("HeatMap") }
  }], "bottom");

  var spritePane = map.makeSpritePane([{
    "title": "Plant",
    "action": function() { alert("Plant action") }
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

