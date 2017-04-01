var AuctionItemController = (function () {
    function AuctionItemController() {
    }
    AuctionItemController.prototype.culcTimeToEnd = function () {
        return this.auction.endTime;
        //var currentTime = new Date().getTime();
        //var timeToEnd = Math.abs(timeOfEnd - currentTime);
        //return timeToEnd;
    };
    return AuctionItemController;
}());
app.component("auctionItem", {
    templateUrl: "template/auction-item-template.html",
    controller: AuctionItemController,
    bindings: {
        auction: "="
    }
});
