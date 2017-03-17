var SellPopupController = (function () {
    function SellPopupController($uibModalInstance, categoriesService, auctionsService, signalRService) {
        this.$uibModalInstance = $uibModalInstance;
        this.categoriesService = categoriesService;
        this.auctionsService = auctionsService;
        this.signalRService = signalRService;
        this.newAuction = new AuctionAdding();
        this.endDates = ['1 Day', '1 Week', '1 Month'];
        this.endDate = '1 Week';
        this.newAuction.startTime = new Date();
    }
    SellPopupController.prototype.save = function () {
        var _this = this;
        this.updateDateOfNewAuction();
        this.auctionsService.addAuction(this.newAuction).then(function () {
            _this.$uibModalInstance.close();
            _this.signalRService.notifyAll();
        });
    };
    SellPopupController.prototype.close = function () {
        this.$uibModalInstance.dismiss('cancel');
    };
    SellPopupController.prototype.parseEndDate = function (endDate) {
        var endDateReturned = new Date();
        var daysToAdd = Number(endDate.split(' ')[0]);
        if (endDate.split(' ')[1] === 'Week') {
            daysToAdd *= 7;
        }
        else if (endDate.split(' ')[1] === 'Month') {
            daysToAdd *= 30;
        }
        endDateReturned.setDate(endDateReturned.getDate() + daysToAdd);
        return endDateReturned;
    };
    SellPopupController.prototype.updateDateOfNewAuction = function () {
        this.newAuction.startTime = new Date();
        this.newAuction.endTime = this.parseEndDate(this.endDate);
    };
    SellPopupController.$inject = ['$uibModalInstance', 'categoriesService', 'auctionsService', 'signalRService'];
    return SellPopupController;
}());
//# sourceMappingURL=sell-popup-controller.js.map