var Auction = (function () {
    function Auction() {
        this.id = "";
        this.title = "";
        this.description = "";
        this.startTime = undefined;
        this.endTime = undefined;
        this.startBid = undefined;
        this.isItemNew = true;
        this.picture1 = "";
        this.picture2 = "";
        this.picture3 = "";
        this.picture4 = "";
        this.bidCount = 0;
        this.user = undefined;
        this.category = undefined;
        this.highestBid = undefined; //  higest bid id
    }
    return Auction;
}());
//# sourceMappingURL=auction.js.map