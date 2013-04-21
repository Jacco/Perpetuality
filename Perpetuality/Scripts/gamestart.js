var perpetuality = perpetuality || {};

perpetuality.start = perpetuality.start || {};

perpetuality.start.animatedBegin = function () {
    function recursiveAnimate(lever, animationArray, callback) {
        if (animationArray.length < 1) {
            callback();
        }
        else {
            var animation = animationArray.shift();
            lever.animate({"font-size": "+=20"}, animation.in, "linear", function () {
                lever.removeClass(animation.from);
                lever.addClass(animation.to);
                recursiveAnimate(lever, animationArray, callback);
            });
        }
    };

    var startGameCallback = function() { $("#starter").submit(); };
    recursiveAnimate($(".startgame"), [
        { from: "leverup",          to: "levercrackle1","in": 80 },
        { from: "levercrackle1",    to: "levercrackle2","in": 80 },
        { from: "levercrackle2",    to: "levercrackle1","in": 150 },
        { from: "levercrackle1",    to: "levercrackle2","in": 150 },
        { from: "levercrackle2",    to: "leverup",      "in": 220 },
        { from: "leverup",          to: "leverdown",    "in": 300 },
        { from: "leverdown",        to: "leverdown",    "in": 300 }
    ], startGameCallback);
}

perpetuality.start.init = function () {
    var lever = $("<div />", { "class": "startgame leverup" })
    lever.click(perpetuality.start.animatedBegin);
    lever.appendTo("body");
};

$(document).ready(perpetuality.start.init);
