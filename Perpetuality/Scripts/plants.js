var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || {};

perpetuality.plant.types = {
    none: {
            cost: 0,
            size: 0,
            energyPerMeter: 0,
            },
    solarroof: {
            cost: 3000,
            size: 4,
            energyPerMeter: 2,
            mapImage: "/Content/Images/original/icoon_dakpaneel.png"
    },
    solarfield: {
            cost: 200000,
            size: 2500,
            energyPerMeter: 160,
            mapImage: "/Content/Images/original/icoon_paneelpark.png"
    },
    solartower: {
            cost: 45000000,
            size: 785398,
            energyPerMeter: 2000,
            mapImage: "/Content/Images/original/icoon_zoutcollector.png"
    }
};

/**
 * Returns a function that sets a plant type in a perpetuality.state.StateModel object.
 */
perpetuality.plant.clickable = function (type) {
    return function(data) { data.selectedPlantType(data.plantTypes[type]); }
}


/**
 * perpetuality.plant.CustomMarker
 *
 * google.maps.OverlayView that also places a div over itself that can be easily used in other code.
 */
perpetuality.plant.CustomMarker = function (gmap, url, house) {
    this.gmap_ = gmap;
    this.house_ = house;
    this.position_ = new google.maps.LatLng(house.latitude, house.longitude),

    this.div_ = $('<div style="position: absolute; z-index: 1;"><img style="cursor: pointer; position: relative; left: -50%; top: -7px;" src="' + url + '"/></div>');
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
