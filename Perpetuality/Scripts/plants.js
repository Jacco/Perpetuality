var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || function () { };

perpetuality.plant.number = 0;

perpetuality.plant.placePlant = function (x, y) {
    var plantId = "plant" + perpetuality.plant.number++;
    var newPlant = $("<div />", {
        id: plantId,
        "class": "plant",
        style: "top: " + y + "px; " + "left: " + x + "px;"
    });
    newPlant.click(function (e) { $("#" + plantId).remove() });
    newPlant.appendTo("body");
};
