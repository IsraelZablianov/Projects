class SellPopupController {
    public static $inject: string[] = ['$uibModalInstance', 'categoriesService', 'auctionsService', 'signalRService'];
    public newAuction: AuctionAdding = new AuctionAdding();
    public endDates: string[] = ['1 Day', '1 Week', '1 Month'];
    public endDate: string = '1 Week';

    constructor(private $uibModalInstance, public categoriesService: CategoriesService, private auctionsService: AuctionsService, private signalRService: SignalRService) {
        this.newAuction.startTime = new Date(); 
    }

    public save(): void {
        this.updateDateOfNewAuction();
        this.auctionsService.addAuction(this.newAuction).then(() => {
            this.$uibModalInstance.close();
            this.signalRService.notifyAll();
        });
    }

    public close() {
        this.$uibModalInstance.dismiss('cancel');
    }

    private parseEndDate(endDate: string): Date {
        let endDateReturned = new Date();
        let daysToAdd = Number(endDate.split(' ')[0]);

        if (endDate.split(' ')[1] === 'Week') {
            daysToAdd *= 7;
        }
        else if (endDate.split(' ')[1] === 'Month') {
            daysToAdd *= 30;
        }

        endDateReturned.setDate(endDateReturned.getDate() + daysToAdd);

        return endDateReturned;
    }

    private updateDateOfNewAuction(): void {
        this.newAuction.startTime = new Date();
        this.newAuction.endTime = this.parseEndDate(this.endDate);
    }
}
