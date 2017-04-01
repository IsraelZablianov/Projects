class AuctionItemController {
    public auction: Auction;
    public onDelete: Function;
    public timeLeft: string;
    public deleting:boolean = false;
    public static $inject: string[] = ['categoriesService', 'auctionsService', 'culTimeService', 'signalRService'];

    public constructor(public categoriesService: CategoriesService, private auctionsService: AuctionsService, culTimeService: CulTimeService,
    private signalRService: SignalRService) {
        this.timeLeft = culTimeService.culcTimeToEnd(this.auction);
        culTimeService.start(this.auction, (newTimeLeft) => this.timeLeft = newTimeLeft);
    }

    public currentPrice(): number {
        return this.auction.highestBid !== undefined && this.auction.highestBid !== null ? this.auction.highestBid.bid : this.auction.startBid;
    }

    public onEmailClick($event:ng.IAngularEvent) {
        $event.stopPropagation();
    }

    public onDeleteItemClick($event: ng.IAngularEvent) {
        $event.stopPropagation();
        this.deleting = true;
        this.auctionsService.deleteAuction(this.auction).then(() => {
            this.onDelete();
            this.signalRService.notifyAll();
        });
    }

    public isAuctionOfCurrentSeller(): boolean {
        return this.auction.user.id === window.localStorage.getItem('UserName');
    }
}
