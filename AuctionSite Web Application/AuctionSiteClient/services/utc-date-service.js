var UtcDateService = (function () {
    function UtcDateService() {
    }
    UtcDateService.prototype.getUtcDate = function (date) {
        var dateUtc = new Date(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds());
        return dateUtc;
    };
    return UtcDateService;
}());
app.service('utcDateService', UtcDateService);
//# sourceMappingURL=utc-date-service.js.map