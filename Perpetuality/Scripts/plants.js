var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || {};

perpetuality.plant.types = {
    none: {
        cost: 0,
        size: 0,
        energyPerMeter: 0,
    },
    solarroof: {
        type: "solarroof",
        cost: 3000,
        size: 1500,
        energyPerMeter: 2,
        mapImage: "/Content/Images/original/icoon_dakpaneel.png"
    },
    solarfield: {
        type: "solarfield",
        cost: 200000,
        size: 2500,
        energyPerMeter: 160,
        mapImage: "/Content/Images/original/icoon_paneelpark.png"
    },
    solartower: {
        type: "solartower",
        cost: 45000000,
        size: 785398,
        energyPerMeter: 2000,
        mapImage: "/Content/Images/original/icoon_zoutcollector.png"
   } 
};

perpetuality.plant.typesIdx = [];
perpetuality.plant.typesIdx[1] = perpetuality.plant.types.solarroof;
perpetuality.plant.typesIdx[2] = perpetuality.plant.types.solarfield;
perpetuality.plant.typesIdx[3] = perpetuality.plant.types.solartower;

/**
 * Returns a function that sets a plant type in a perpetuality.state.StateModel object.
 */
perpetuality.plant.clickable = function (type) {
    return function (data) {
        var plantType = data.plantTypes[type];
        if (data.selectedPlantType() == plantType) data.selectedPlantType(data.plantTypes.none)
        else data.selectedPlantType(plantType);
    }
}


/**
 * perpetuality.plant.CustomMarker
 *
 * google.maps.OverlayView that also places a div over itself that can be easily used in other code.
 */
perpetuality.plant.CustomMarker = function (gmap, url, plantId, location, size) {
    this.gmap_ = gmap;
    this.location_ = location;
    this.plantId_ = plantId;
    this.position_ = new google.maps.LatLng(location.latitude, location.longitude);
    this.size_ = size;

    this.div_ = $('<div id="' + plantId + '" style="position: absolute; z-index: 1;"><img style="cursor: pointer; position: relative; left: -50%; top: -7px;" src="' + url + '"/></div>');
    this.img_ = $('img', this.div_);
    var that = this;
    google.maps.event.addDomListener(this.img_.get(0), 'click', function () {
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
    var zoom = 19 - this.gmap_.zoom;
    if (zoom < 0)
        zoom = 0;
    if (zoom > 2)
        zoom = 2;
    this.img_.css('width', 120 / Math.pow(2, zoom));
}

perpetuality.plant.CustomMarker.prototype.onRemove = function () {
    this.div_.hide();
    if (this.getPanes() && this.getPanes().overlayImage) {
        this.getPanes().overlayImage.removeChild(this.div_.get(0));
    }
}
