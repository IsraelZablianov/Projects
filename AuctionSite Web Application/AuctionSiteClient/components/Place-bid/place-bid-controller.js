var PlaceBidController = (function () {
    function PlaceBidController($uibModalInstance, auctionsService, categoriesService, auction, culTimeService, signalRService) {
        var _this = this;
        this.$uibModalInstance = $uibModalInstance;
        this.auctionsService = auctionsService;
        this.categoriesService = categoriesService;
        this.auction = auction;
        this.signalRService = signalRService;
        this.imgArr = [];
        this.currentImg = 0;
        this.daysLeft = 0;
        this.hoursLeft = 0;
        this.bidPlaced = false;
        this.higherBidError = false;
        this.auctionDeletedError = false;
        this.imgArr[0] = this.auction.picture1;
        this.imgArr[1] = this.auction.picture2;
        this.imgArr[2] = this.auction.picture3;
        this.imgArr[3] = this.auction.picture4;
        this.timeLeft = culTimeService.culcTimeToEnd(this.auction);
        culTimeService.start(this.auction, function (newTimeLeft) { return _this.timeLeft = newTimeLeft; });
        this.enableMoveLeft = false;
        if (this.imgArr[1] == undefined) {
            this.enableMoveRight = false;
        }
        else {
            this.enableMoveRight = true;
        }
    }
    PlaceBidController.prototype.onRightButtonClick = function () {
        if (this.currentImg + 1 < 4) {
            if (this.imgArr[this.currentImg + 1] != undefined) {
                this.enableMoveLeft = true;
                if (this.imgArr[this.currentImg + 2] != undefined) {
                    this.enableMoveRight = true;
                }
                else {
                    this.enableMoveRight = false;
                }
                this.currentImg = this.currentImg + 1;
            }
        }
        return this.imgArr[this.currentImg];
    };
    PlaceBidController.prototype.onLeftButtonClick = function () {
        if (this.currentImg - 1 < 4) {
            if (this.imgArr[this.currentImg - 1] != undefined) {
                this.enableMoveRight = true;
                if (this.imgArr[this.currentImg - 2] != undefined) {
                    this.enableMoveLeft = true;
                }
                else {
                    this.enableMoveLeft = false;
                }
                this.currentImg = this.currentImg - 1;
            }
            return this.imgArr[this.currentImg];
        }
    };
    PlaceBidController.prototype.textChange = function () {
        var lastBid;
        if (this.auction.highestBid == undefined) {
            lastBid = this.auction.startBid;
        }
        else {
            lastBid = this.auction.highestBid.bid;
        }
        if (this.highestBid.bid > lastBid) {
            this.validOffer = true;
        }
        else {
            this.validOffer = false;
        }
    };
    PlaceBidController.prototype.onPlaceBidClick = function () {
        var _this = this;
        this.auctionDeletedError = false;
        this.higherBidError = false;
        this.highestBid.bidTime = new Date();
        this.auctionsService.addBid(this.highestBid, this.auction.id)
            .then(function (result) {
            switch (result) {
                case AddBidStatuse.SUCCESS:
                    {
                        _this.signalRService.notifyAll();
                        _this.bidPlaced = true;
                        break;
                    }
                case AddBidStatuse.HIGHER_BID:
                    {
                        _this.higherBidError = true;
                        break;
                    }
                case AddBidStatuse.AUCTION_DELETED:
                    {
                        _this.auctionDeletedError = true;
                        break;
                    }
            }
        });
    };
    PlaceBidController.prototype.close = function () {
        this.$uibModalInstance.close('cancel');
    };
    PlaceBidController.$inject = ['$uibModalInstance', 'auctionsService', 'categoriesService', 'auction', 'culTimeService', 'signalRService'];
    return PlaceBidController;
}());
//# sourceMappingURL=place-bid-controller.js.map