var AuctionsController = (function () {
    function AuctionsController(auctionsService) {
        var _this = this;
        this.auctionsService = auctionsService;
        this.auctions = [];
        this.categories = [];
        auctionsService.getAuctions().then(function (data) { return _this.auctions = data; });
        auctionsService.getCategories().then(function (data) { return _this.categories = data; });
    }
    AuctionsController.$inject = ['auctionsService'];
    return AuctionsController;
}());
app.component("auctions", {
    templateUrl: "template/auctions-template.html",
    controller: AuctionsController
});
//# sourceMappingURL=auctions.js.map