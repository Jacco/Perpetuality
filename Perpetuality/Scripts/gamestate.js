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
        return this.time().getDate() + "-" + this.time().getMonth() + 1 + "-" + this.time().getFullYear() + '   ' + +this.time().getHours() + ':' + this.time().getMinutes();
    }, this);
};
