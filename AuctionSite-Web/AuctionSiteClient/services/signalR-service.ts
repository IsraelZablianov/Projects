class SignalRService {
    public $inject: string[] = ['auctionsService'];
    private myHub: SignalR.Hub.Proxy;

    public constructor(private auctionsService: AuctionsService) {
        var con = $.hubConnection();
        this.myHub = con.createHubProxy('auctionAdd');
        this.myHub.on('OnAuctionAdded', () => {
            this.auctionsService.updateAuctions();
        });
        con.start();
    }

    public notifyAll() {
        this.myHub.invoke('NotifyAuctionWasAdded');
    }
}

app.service('signalRService', SignalRService);