var ReporeService = (function () {
    function ReporeService() {
        this.inReportMode = false;
    }
    return ReporeService;
}());
app.service("reporeService", ReporeService);
