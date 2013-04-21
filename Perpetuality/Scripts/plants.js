var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || function (type, basecost, standardsize, energypermeter) {
    this.type = type;
    this.basecost = basecost;
    this.standardsize = standardsize;
    this.energypermeter = energypermeter;
};

perpetuality.plant.prototype.build = function (map, event) {
    this.marker = new perpetuality.plant.CustomMarker(map.root,
        "/Content/Images/original/icoon_paneelpark.png",
        { latitude: event.latLng.lat(), longitude: event.latLng.lng() });
};

perpetuality.plant.number = 0;

perpetuality.plant.placePlant = function (map, plant) {
    var buttonId = "#" + plant.type + "button";
    $(buttonId).toggleClass("selected");
    if ($(buttonId).hasClass("selected")) {
        $("#plantcost").empty().html("" + plant.basecost + " €");
        $("#plantsize").empty().html("" + plant.standardsize + " m²");
    }
    else {
        $("#plantcost").empty();
        $("#plantsize").empty();
    }
    google.maps.event.addListener(map.root, 'click', function (event) {
        if ($(buttonId).hasClass("selected")) {
            plant.build(map, event);
        }
    });
};

perpetuality.plant.CustomMarker = function (gmap, url, house) {
    this.gmap_ = gmap;
    this.house_ = house;
    this.position_ = new google.maps.LatLng(house.latitude, house.longitude),

    this.div_ = $('<div style="position: absolute; z-index: 1;"><img style="cursor: pointer; position: relative; left: -50%; top: -7px; font-size: 10px;" src="' + url + '"/></div>');
    var that = this;
    google.maps.event.addDomListener($('img', this.div_).get(0), 'click', function () {
        google.maps.event.trigger(that, 'click');
    });

    this.setMap(this.gmap_);
}

perpetuality.plant.CustomMarker.prototype = new google.maps.OverlayView();

perpetuality.plant.CustomMarker.prototype.onAdd = function () {
    this.getPanes().overlayImage.appendChild(this.div_.get(0));
    var that = this;

    this.div_.show();
}

perpetuality.plant.CustomMarker.prototype.draw = function () {
    var overlayProjection = this.getProjection();
    var position = overlayProjection.fromLatLngToDivPixel(this.position_);
    this.div_.css('left', position.x + 'px');
    this.div_.css('top', position.y + 'px');
}

perpetuality.plant.CustomMarker.prototype.onRemove = function () {
    this.div_.hide();
    if (this.getPanes() && this.getPanes().overlayImage) {
        this.getPanes().overlayImage.removeChild(this.div_.get(0));
    }
}
