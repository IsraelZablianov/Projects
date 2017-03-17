var AuctionItemController = (function () {
    function AuctionItemController(categoriesService, auctionsService, culTimeService, signalRService) {
        var _this = this;
        this.categoriesService = categoriesService;
        this.auctionsService = auctionsService;
        this.signalRService = signalRService;
        this.deleting = false;
        this.timeLeft = culTimeService.culcTimeToEnd(this.auction);
        culTimeService.start(this.auction, function (newTimeLeft) { return _this.timeLeft = newTimeLeft; });
    }
    AuctionItemController.prototype.currentPrice = function () {
        return this.auction.highestBid !== undefined && this.auction.highestBid !== null ? this.auction.highestBid.bid : this.auction.startBid;
    };
    AuctionItemController.prototype.onEmailClick = function ($event) {
        $event.stopPropagation();
    };
    AuctionItemController.prototype.onDeleteItemClick = function ($event) {
        var _this = this;
        $event.stopPropagation();
        this.deleting = true;
        this.auctionsService.deleteAuction(this.auction).then(function () {
            _this.onDelete();
            _this.signalRService.notifyAll();
        });
    };
    AuctionItemController.prototype.isAuctionOfCurrentSeller = function () {
        return this.auction.user.id === window.localStorage.getItem('UserName');
    };
    AuctionItemController.$inject = ['categoriesService', 'auctionsService', 'culTimeService', 'signalRService'];
    return AuctionItemController;
}());
//# sourceMappingURL=auction-item-controller.js.map