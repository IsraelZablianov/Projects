app.component("auctionItem",
    {
        templateUrl: "components/Auction-item/auction-item-template.html",
        controller: AuctionItemController,
        bindings:
        {
            auction: "=",
            onDelete:"&"
        }
    });