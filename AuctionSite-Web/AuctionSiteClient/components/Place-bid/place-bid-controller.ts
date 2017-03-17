class PlaceBidController {
    imgArr: string[] = [];
    currentImg: number = 0;
    daysLeft: number = 0;
    hoursLeft: number = 0;
    highestBid: HighestBid;
    enableMoveLeft: boolean;
    enableMoveRight: boolean;
    validOffer: boolean;
    bidPlaced: boolean = false;
    higherBidError: boolean = false;
    auctionDeletedError: boolean = false;
    timeLeft: string;

    public static $inject: string[] = ['$uibModalInstance', 'auctionsService', 'categoriesService', 'auction', 'culTimeService', 'signalRService'];

    constructor(private $uibModalInstance, private auctionsService: AuctionsService,
        private categoriesService: CategoriesService, public auction, culTimeService: CulTimeService, private signalRService: SignalRService) {
     
               
                    this.imgArr[0] = this.auction.picture1;
                    this.imgArr[1] = this.auction.picture2;
                    this.imgArr[2] = this.auction.picture3;
                    this.imgArr[3] = this.auction.picture4;


                this.timeLeft = culTimeService.culcTimeToEnd(this.auction);
                culTimeService.start(this.auction, (newTimeLeft) => this.timeLeft = newTimeLeft);

                this.enableMoveLeft = false;
                if (this.imgArr[1] == undefined) {
                    this.enableMoveRight = false;
                }
                else {
                    this.enableMoveRight = true;
                }
           

    }

    public onRightButtonClick() {
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
    }

    public onLeftButtonClick() {
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
    }

    public textChange() {
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

    }

    public onPlaceBidClick() {
        this.auctionDeletedError = false;
        this.higherBidError = false;
        this.highestBid.bidTime = new Date();
        this.auctionsService.addBid(this.highestBid, this.auction.id)
            .then((result) => {
                switch (result) {
                case AddBidStatuse.SUCCESS:
                {
                    this.signalRService.notifyAll();
                    this.bidPlaced = true;
                    break;
                }
                case AddBidStatuse.HIGHER_BID:
                {
                    this.higherBidError = true;
                    break;
                }
                case AddBidStatuse.AUCTION_DELETED:
                {
                    this.auctionDeletedError = true;
                    break;
                }
                }
            });
    }

    public close() {
        this.$uibModalInstance.close('cancel');
    }
}
