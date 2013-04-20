var perpetuality = perpetuality || {};

perpetuality.plant = function () {
    alert("");
    var number = 0;

    function removePlant(plantId) {
        $("#" + plantId).remove();
    }

    function placePlant(x, y) {
        var plantId = "plant" + number++;
        var newPlant = $("<div />", {
            id: plantId,
            "class": "plant",
            style: "top: " + y + "px; " + "left: " + x + "px;"
        });
        newPlant.click(function (e) { removePlant(plantId) });
        newPlant.appendTo("#map");
    }

    var plant = $("<div />", {
        id: "plantsource",
        "class": "plant"
    });
    plant.draggable({ revert: true });
    plant.on({ mouseup: function (e) { placePlant(e.pageX, e.pageY) } });
    plant.appendTo("body")

    return plant;
};

$(document).ready(perpetuality.plant)
