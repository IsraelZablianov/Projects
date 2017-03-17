var PlaceBidController = (function () {
    function PlaceBidController(auctionsService) {
        var _this = this;
        this.auctionsService = auctionsService;
        this.auction = null;
        auctionsService.getAuction(this.auctionId).then(function (data) { return _this.auction = data; });
    }
    PlaceBidController.$inject = ['auctionsService'];
    return PlaceBidController;
}());
app.component("placeBid", {
    templateUrl: "template/place-bid-template.html",
    controller: PlaceBidController,
    bindings: {
        auctionId: "="
    }
});
//# sourceMappingURL=place-bid.js.map