var CulTimeService = (function () {
    function CulTimeService($interval) {
        this.$interval = $interval;
    }
    CulTimeService.prototype.start = function (auction, callBack) {
        var _this = this;
        this.$interval(function () {
            var time = _this.culcTimeToEnd(auction);
            callBack(time);
        }, 60000);
    };
    CulTimeService.prototype.culcTimeToEnd = function (auction) {
        var timeLeft = this.timeLeftCount(auction);
        if (timeLeft.daysLeft === 0) {
            return +timeLeft.hoursLeft + 'h ' + timeLeft.minutsLeft + 'm';
        }
        else {
            return +timeLeft.daysLeft + 'd ' + timeLeft.hoursLeft + 'h';
        }
    };
    CulTimeService.prototype.timeLeftCount = function (auction) {
        var timeLeft = new TimeLeft();
        var d = Date.parse(auction.endTime.toString()) - +new Date();
        var minutes = 1000 * 60;
        var hours = minutes * 60;
        var days = hours * 24;
        timeLeft.daysLeft = Math.floor(d / days);
        var hoursInMillLeft = d - (timeLeft.daysLeft * days);
        timeLeft.hoursLeft = Math.floor(hoursInMillLeft / hours);
        var minuteInMillLeft = hoursInMillLeft - (timeLeft.hoursLeft * hours);
        timeLeft.minutsLeft = Math.floor(minuteInMillLeft / minutes);
        return timeLeft;
    };
    CulTimeService.$inject = ['$interval'];
    return CulTimeService;
}());
app.service('culTimeService', CulTimeService);
//# sourceMappingURL=culc-time-service.js.map