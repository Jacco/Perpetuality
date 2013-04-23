var perpetuality = perpetuality || {};

perpetuality.state = perpetuality.state || {};

function PlantModel() {
}

perpetuality.state.StateModel = function () {
    var self = this;
    this.plants = {};

    this.initialPlayerState = JSON.parse($('#playerState').attr('data'));

    this.plantTypes = perpetuality.plant.types;
    
    this.power = ko.observable(0);
    this.powerText = ko.computed(function () { return self.power(); });
    this.credits = ko.observable(this.initialPlayerState.balance);
    this.creditsText = ko.computed(function () { return self.credits(); });

    this.time = ko.observable(new Date());
    this.timeText = ko.computed(function () { self.computeTime() });

    this.creditProduction = ko.observable(this.initialPlayerState.rate); // in credits per world-second

    this.selectedOverlay = ko.observable('none');
    this.selectedPlantType = ko.observable(this.plantTypes.none);
};

perpetuality.state.StateModel.prototype.computeTime = function () {
    var hrs = '0' + this.time().getHours();
    hrs = hrs.substr(hrs.length - 2);
    var min = '0' + this.time().getMinutes();
    min = min.substr(min.length - 2);
    return this.time().getDate() + "-" + this.time().getMonth() + 1 + "-" + this.time().getFullYear() + '   ' + hrs + ':' + min;
}

perpetuality.state.StateModel.prototype.addPlant = function (event) {
    if (this.selectedPlantType() != this.plantTypes.none) {
        // place the plant
        var marker = new perpetuality.plant.CustomMarker(
            this.map.root,
            this.selectedPlantType().mapImage,
            { latitude: event.latLng.lat(), longitude: event.latLng.lng() });

        // deselect the button
        this.selectedPlantType(this.plantTypes.none);
    } else {
        // maybe give some info
    }
};