var perpetuality = perpetuality || {};

perpetuality.plant = perpetuality.plant || function (type, basecost, standardsize, energypermeter) {
    alert("Space!");
    this.type = type;
    this.basecost = basecost;
    this.standardsize = standardsize;
    this.energypermeter = energypermeter;
};

perpetuality.plant.prototype.build = function (map, latLng) {
    var marker = google.maps.Marker({
        position: latLng,
        map: map.root,
        title:"Solar Field Power Plant",
        icon: "/Content/Images/original/icoon_paneelpark.png"
    });
};

perpetuality.plant.number = 0;

perpetuality.plant.placePlant = function (map, plant) {
    $("#" + plant.type + "button").addClass("selected");
    $("#plantcost").empty().html("" + plant.basecost + " €");
    $("#plantsize").empty().html("" + plant.standardsize + " m²");

    google.maps.event.addListener(map.root, 'click', function(event) {
        alert(event.latLng);
        plant.build(map, event.latLng);
    });
};