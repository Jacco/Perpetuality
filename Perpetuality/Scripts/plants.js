var perpetuality = perpetuality || {};

perpetuality.plant = function () {
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
        newPlant.appendTo("body");
    }

    var plant = $("#plantsource");
    plant.draggable({ revert: true });
    plant.on({ mouseup: function (e) { placePlant(e.pageX, e.pageY) } });

    return plant;
}();