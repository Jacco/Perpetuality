var perpetuality = perpetuality || {};

perpetuality.state = perpetuality.state || {};

function PlantModel() {
}

// Advances the time in the game in state
perpetuality.state.advanceTime = function (state) {
    var tm = state.time().getTime();
    tm = tm + 365000;
    var dt = new Date();
    dt.setTime(tm);
    state.time(dt);

    state.credits(state.credits() + 365 * state.creditProduction());
}

/*
 * Game state data model
 * 
 * the map variable is a perpetuality.map
 */
perpetuality.state.StateModel = function (map) {
    var self = this;

    this.plants = {};
    this.map = map;

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


/**
 * Timer
 */
perpetuality.state.Timer = function (settings) {
    this.settings = settings;
    this.timer = null;

    this.fps = settings.fps || 30;
    this.interval = Math.floor(1000 / this.fps);
    this.timeInit = null;
}

perpetuality.state.Timer.prototype =
{
    run: function () {
        var self = this;

        this.settings.run();
        this.timeInit += this.interval;

        this.timer = setTimeout(
            function () { self.run() },
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
