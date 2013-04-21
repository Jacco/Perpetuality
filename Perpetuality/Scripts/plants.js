var perpetuality = perpetuality || {};

perpetuality.Plant = perpetuality.Plant || function () { };

perpetuality.Plant.number = 0;

perpetuality.Plant.placePlant = function(x, y) {
    var plantId = "plant" + perpetuality.Plant.number++;
    var newPlant = $("<div />", {
        id: plantId,
        "class": "plant",
        style: "top: " + y + "px; " + "left: " + x + "px;"
    });
    newPlant.click(function (e) { removePlant(plantId) });
    newPlant.appendTo("body");
    alert(x + y);
};
