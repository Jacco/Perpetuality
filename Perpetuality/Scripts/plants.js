var perpetuality = perpetuality || {};

CustomMarker = function (gmap, url, house) {
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

CustomMarker.prototype = new google.maps.OverlayView();

CustomMarker.prototype.onAdd = function () {
    this.getPanes().overlayImage.appendChild(this.div_.get(0));
    var that = this;

    this.div_.show();
}

CustomMarker.prototype.draw = function () {
    var overlayProjection = this.getProjection();
    var position = overlayProjection.fromLatLngToDivPixel(this.position_);
    this.div_.css('left', position.x + 'px');
    this.div_.css('top', position.y + 'px');
}

CustomMarker.prototype.onRemove = function () {
    this.div_.hide();
    if (this.getPanes() && this.getPanes().overlayImage) {
        this.getPanes().overlayImage.removeChild(this.div_.get(0));
    }
}
