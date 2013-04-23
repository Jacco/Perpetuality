var perpetuality = perpetuality || {};

function PlantModel() {
}

function StateModel() {

    this.plants = {};

    this.plantTypes = {
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


    this.power = ko.observable(0);
    this.powerText = ko.computed(function () {
        return this.power();
    }, this);

    this.credits = ko.observable(3000000);
    this.creditsText = ko.computed(function () {
        return this.credits();
    }, this);

    this.time = ko.observable(new Date());
    this.timeText = ko.computed(function () {
        var hrs = '0' + this.time().getHours();
        hrs = hrs.substr(hrs.length - 2);
        var min = '0' + this.time().getMinutes();
        min = min.substr(min.length - 2);
        return this.time().getDate() + "-" + this.time().getMonth() + 1 + "-" + this.time().getFullYear() + '   ' + hrs + ':' + min;
    }, this);

    this.creditProduction = ko.observable(0.04); // credits per world-second

    this.selectedOverlay = ko.observable('none');
    this.selectedPlantType = ko.observable(this.plantTypes.none);

    this.addPlant = function (event) {
        if (this.selectedPlantType() != this.plantTypes.none) {
            // place the plant
            var marker = new CustomMarker(
                this.map.root,
                this.selectedPlantType().mapImage,
                { latitude: event.latLng.lat(), longitude: event.latLng.lng() });

            // deselect the button
            this.selectedPlantType(this.plantTypes.none);
        } else {
            // maybe give some info
        }
    };
};
