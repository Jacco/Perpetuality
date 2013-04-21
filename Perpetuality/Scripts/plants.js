var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || function (type, basecost, standardsize, energypermeter) {
    this.type = type;
    this.basecost = basecost;
    this.standardsize = standardsize;
    this.energypermeter = energypermeter;
};

perpetuality.plant.number = 0;

perpetuality.plant.placePlant = function (plant) {
    $("#" + plant.type + "button").addClass("selected");
    $("#plantcost").empty().html("" + plant.basecost + " €");
    $("#plantsize").empty().html("" + plant.standardsize + " m²");
};
