class AuctionsService {
    public static $inject: string[] = ['$http'];
    public auctions: Auction[] = [];
    public currentPart: number = 0;
    public busy: boolean = false;

    public constructor(private $http: ng.IHttpService) {
    }

    public getAuctions(): ng.IPromise<Auction[]> {
        return this.$http.get("api/auctions")
            .then(data => {
                return data.data;
            });
    }

    public getMoreAuctions() {
        if (this.busy) return;
        this.busy = true;
        return this.$http.get("api/auctions/?partNumber=" + this.currentPart)
            .then((data: ng.IHttpPromiseCallbackArg<Auction[]>) => {
                this.currentPart ++;
                if (data.data !== null) {
                    this.auctions = this.auctions.concat(data.data);
                }
                this.busy = false;
            });
    }

    public getAuction(id: string): ng.IPromise<Auction> {
        return this.$http.get("api/auctions/?id=" + id)
            .then(data => {
                return data.data;
            });
    }

    public addAuction(auction: AuctionAdding) {
        return this.$http.post("api/auctions", auction);
    }

    public addBid(bid: HighestBid, auctionId: string): ng.IPromise<AddBidStatuse> {
        return this.$http.post("api/bids?auctionId=" + auctionId, bid).then(
            (data: ng.IHttpPromiseCallbackArg<boolean>) => {
                if (data.data) {
                    return AddBidStatuse.SUCCESS;
                } else {
                    return AddBidStatuse.HIGHER_BID;
                }
            },
        ()=> { return AddBidStatuse.AUCTION_DELETED; });
    }

    public deleteAuction(auction: Auction): ng.IPromise<void> {
        return this.$http.delete("api/auctions?auctionId=" + auction.id).then(() => {});
    }

    public updateAuctions() {
        this.currentPart = 0;
        this.auctions = [];
        return this.getMoreAuctions();
    }
}
enum AddBidStatuse {
    SUCCESS, AUCTION_DELETED, HIGHER_BID
}
app.service('auctionsService', AuctionsService);