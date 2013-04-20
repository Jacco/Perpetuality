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

perpetuality.map.prototype.makeItemizedPane = function (name, contentList, extraClass) {
    var pane = document.createElement("div");
    pane.id = name + "-pane";
    $(pane).addClass("map-pane");
    if (extraClass != undefined) {
        $(pane).addClass(extraClass);
    }
    for (var i = 0; i < contentList.length; i++) {
        var contentDiv = document.createElement("div");
        $(contentDiv).addClass("map-pane-item");
        var itemExtraClass = contentList[i].itemExtraClass;
        if (itemExtraClass) {
            $(contentDiv).addClass(itemExtraClass);
        }
        var content = new perpetuality.map.ItemizedPaneItem();
        content.title = contentList[i].title;
        content.action = contentList[i].action;
        var title = document.createElement("div");
        title.innerHTML = content.title;
        var image = document.createElement("img");
        image.src = contentList[i].image;
        var imageSize = contentList[i].imageSize;
        if (imageSize) {
            if (imageSize.width) { image.width = imageSize.width }
            if (imageSize.height) { image.height = imageSize.height }
        }
        $(image).click(content.action);
        content.image = image;
        contentDiv.appendChild(image);
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
$(document).ready(function() {
  var map = new perpetuality.map;
  map.init();
    /**
     * Panes.
     */
  var overlayControlPane = map.makeItemizedPane("overlay", [{
      "title": "HeatMap",
      "action": function () { alert("HeatMap") },
      "image": "/Content/Images/default-image.jpeg",
      "itemExtraClass": "map-pane-item-horizontal"
  }], "map-pane-bottom");

  var statusPane = map.makeItemizedPane("status", [
    {
        "title": "KWh",
        "image": "/Content/Images/green.png",
        "imageSize": { "width": 16 },
        "itemExtraClass": "map-pane-item-horizontal"
    },
    {
        "title": "Awesomeness",
        "image": "/Content/Images/orange.png",
        "imageSize": { "width": 16 },
        "itemExtraClass": "map-pane-item-horizontal"
    },
    {
        "title": "YA Hard-coded Value",
        "image": "/Content/Images/red.png",
        "imageSize": { "width": 16 },
        "itemExtraClass": "map-pane-item-horizontal"
    },
  ], "map-pane-top");

  var detailPane = map.makeTextPane("detail", [{
      "title": "Detail Title",
      "content": "Detail Contents."
  }], "map-pane-left");

  var spritePane = map.makeItemizedPane("sprite", [{
      "title": "Plant",
      "action": function () { alert("Plant action") },
      "image": "/Content/Images/nuclear-power-plant.png",
      "itemExtraClass": "map-pane-item-vertical"
  }], "map-pane-right");

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
            "position": google.maps.ControlPosition.TOP_CENTER,
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

