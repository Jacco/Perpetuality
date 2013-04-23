var perpetuality = perpetuality || {};

perpetuality.state = perpetuality.state || {};

function PlantModel() {
}

// Advances the time in the game in state
perpetuality.state.advanceTime = function (state) {
    function round(num) {
        return Math.round(num * Math.pow(10, 2)) / Math.pow(10, 2)
    };

    var tm = state.time().getTime();
    tm = tm + 365000;
    var dt = new Date();
    dt.setTime(tm);
    state.time(dt);

    state.credits(round(state.credits() + 365 * state.creditProduction()));
}

// Number of plants built by player
perpetuality.state.numberOfPlants = 0;

/*
 * Game state data model
 * 
 * the map variable is a perpetuality.map
 */
perpetuality.state.StateModel = function (map) {
    var self = this;

    this.plants = ko.observableArray();
    this.map = map;

    var initialPlayerState = JSON.parse($('#playerState').attr('data'));

    this.plantTypes = perpetuality.plant.types;
    
    this.power = ko.observable(0);
    this.powerText = ko.computed(function () { return self.padZeroes(self.power(), 7); });
    this.credits = ko.observable(initialPlayerState.balance);
    this.creditsText = ko.computed(function () { return self.padZeroes(self.credits(), 14); });

    this.time = ko.observable(new Date());
    this.timeText = ko.computed(function () { return self.computeTime(self.time()); });

    this.creditProduction = ko.computed(function () { return self.computeProduction(initialPlayerState.rate) }); // in credits per world-second

    this.selectedOverlay = ko.observable('none');
    this.selectedPlantType = ko.observable(this.plantTypes.none);
};

perpetuality.state.StateModel.prototype =
{
    padZeroes: function (strng, size) {
        var raw = new Array(size + 1).join("0") + strng;
        return raw.substr(raw.length - size);
    },

    computeTime: function (time) {
        return time.getDate() + "-"
            + (time.getMonth() + 1) + "-"
            + time.getFullYear() + '   '
            + this.padZeroes(time.getHours(), 2) + ':'
            + this.padZeroes(time.getMinutes(), 2);
    },

    // For now I'm assuming 1 credit per hour per 1 power per 1000 square meters.
    computeProduction: function (initialValue) {
        var result = initialValue ? initialValue : 0;
        ko.utils.arrayForEach(this.plants(), function (plant) {
            result += plant.energyPerMeter * plant.size / 3600000;
        });
        return result;
    },

    addPlant: function (event) {
        var newPlant = this.selectedPlantType()
        if (newPlant != this.plantTypes.none) {
            var newPlantId = newPlant.type + perpetuality.state.numberOfPlants++;
            newPlant.id = newPlantId;
            // place the plant
            var marker = new perpetuality.plant.CustomMarker(
                this.map.root,
                newPlant.mapImage,
                newPlantId,
                { latitude: event.latLng.lat(), longitude: event.latLng.lng() });

            // add plant to state
            this.plants.push(newPlant);

            // deselect the button
            this.selectedPlantType(this.plantTypes.none);
        } else {
            // maybe give some info
        }
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
