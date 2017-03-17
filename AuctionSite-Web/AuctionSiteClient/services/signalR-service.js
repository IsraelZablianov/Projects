var SignalRService = (function () {
    function SignalRService(auctionsService) {
        var _this = this;
        this.auctionsService = auctionsService;
        this.$inject = ['auctionsService'];
        var con = $.hubConnection();
        this.myHub = con.createHubProxy('auctionAdd');
        this.myHub.on('OnAuctionAdded', function () {
            _this.auctionsService.updateAuctions();
        });
        con.start();
    }
    SignalRService.prototype.notifyAll = function () {
        this.myHub.invoke('NotifyAuctionWasAdded');
    };
    return SignalRService;
}());
app.service('signalRService', SignalRService);
//# sourceMappingURL=signalR-service.js.map