var AuctionsService = (function () {
    function AuctionsService($http) {
        this.$http = $http;
        this.auctions = [];
        this.currentPart = 0;
        this.busy = false;
    }
    AuctionsService.prototype.getAuctions = function () {
        return this.$http.get("api/auctions")
            .then(function (data) {
            return data.data;
        });
    };
    AuctionsService.prototype.getMoreAuctions = function () {
        var _this = this;
        if (this.busy)
            return;
        this.busy = true;
        return this.$http.get("api/auctions/?partNumber=" + this.currentPart)
            .then(function (data) {
            _this.currentPart++;
            if (data.data !== null) {
                _this.auctions = _this.auctions.concat(data.data);
            }
            _this.busy = false;
        });
    };
    AuctionsService.prototype.getAuction = function (id) {
        return this.$http.get("api/auctions/?id=" + id)
            .then(function (data) {
            return data.data;
        });
    };
    AuctionsService.prototype.addAuction = function (auction) {
        return this.$http.post("api/auctions", auction);
    };
    AuctionsService.prototype.addBid = function (bid, auctionId) {
        return this.$http.post("api/bids?auctionId=" + auctionId, bid).then(function (data) {
            if (data.data) {
                return AddBidStatuse.SUCCESS;
            }
            else {
                return AddBidStatuse.HIGHER_BID;
            }
        }, function () { return AddBidStatuse.AUCTION_DELETED; });
    };
    AuctionsService.prototype.deleteAuction = function (auction) {
        return this.$http.delete("api/auctions?auctionId=" + auction.id).then(function () { });
    };
    AuctionsService.prototype.updateAuctions = function () {
        this.currentPart = 0;
        this.auctions = [];
        return this.getMoreAuctions();
    };
    AuctionsService.$inject = ['$http'];
    return AuctionsService;
}());
var AddBidStatuse;
(function (AddBidStatuse) {
    AddBidStatuse[AddBidStatuse["SUCCESS"] = 0] = "SUCCESS";
    AddBidStatuse[AddBidStatuse["AUCTION_DELETED"] = 1] = "AUCTION_DELETED";
    AddBidStatuse[AddBidStatuse["HIGHER_BID"] = 2] = "HIGHER_BID";
})(AddBidStatuse || (AddBidStatuse = {}));
app.service('auctionsService', AuctionsService);
//# sourceMappingURL=auctions-service.js.map