var perpetuality = perpetuality || {};

perpetuality.start = perpetuality.start || {};

perpetuality.start.animatedBegin = function () {
    function recursiveAnimate(lever, from, animationArray, callback) {
        if (animationArray.length < 1) {
            callback();
        }
        else {
            var animation = animationArray.shift();
            lever.animate({ "font-size": "+=20" }, animation.milis, "linear", function () {
                lever.removeClass(from);
                lever.addClass(animation.to);
                recursiveAnimate(lever, animation.to, animationArray, callback);
            });
        }
    };

    var startGameCallback = function() { $("#starter").submit(); };
    recursiveAnimate($(".startgame"), "leverup", [
        { to: "levercrackle1", milis: 80 },
        { to: "levercrackle2", milis: 80 },
        { to: "levercrackle1", milis: 150 },
        { to: "levercrackle2", milis: 150 },
        { to: "leverup",       milis: 220 },
        { to: "leverdown",     milis: 300 },
        { to: "leverdown",     milis: 300 }
    ], startGameCallback);
}

perpetuality.start.init = function () {
    var lever = $("<div />", { "class": "startgame leverup" })
    lever.click(perpetuality.start.animatedBegin);
    lever.appendTo("body");
};

$(document).ready(perpetuality.start.init);
