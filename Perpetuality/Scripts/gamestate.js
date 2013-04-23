var perpetuality = perpetuality || {};

function StateModel() {

    this.plants = {};

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
};
